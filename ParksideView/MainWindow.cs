using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.IO;

namespace ParksideView
{
    public partial class MainWindow : Form
    {
        private bool isConnected = false, isPaused = false, isRecording = false, isTimerUpdatePending = false, isAlive = true;
        private StringBuilder recordingBuffer = new StringBuilder();
        private DateTime recordingStart = DateTime.Now;
        private int blankCount = 0, recordingCount = 0;
        private Multimeter meter = null;

        public MainWindow()
        {
            // Initialize the controls
            InitializeComponent();

            // Refresh the ports list
            RefreshPorts();

            // Setup the UI
            ChangeUI(false, false, false);
            UpdateTimer();
        }

        private void valueLabel_Click(object sender, EventArgs e)
        {
            CopyValue();
        }

        private void unitLabel_Click(object sender, EventArgs e)
        {
            CopyValue();
        }

        private void modeLabel_Click(object sender, EventArgs e)
        {
            CopyValue();
        }

        private void readoutLayout_Click(object sender, EventArgs e)
        {
            CopyValue();
        }

        private void topMostCheck_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = topMostCheck.Checked;
        }

        private void minimizeButton_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void copyrightLabel_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.mikrocontroller.net/topic/491973");
        }

        private void refreshPortsButton_Click(object sender, EventArgs e)
        {
            RefreshPorts();
        }

        private void acquireTimer_Tick(object sender, EventArgs e)
        {
            // Make sure that the state and meter objects are valid
            if (!isConnected || meter == null || !meter.IsConnected)
            {
                // Disconnect and return, if not
                Disconnect();
                return;
            }

            // Stop the timer
            acquireTimer.Stop();

            // Handle pause mode
            if (isPaused)
            {
                // Discard the samples, start the timer again and continue
                meter.Flush();
                acquireTimer.Start();
                return;
            }

            // Check, if the maximum duration of missed screen updates has been exceeded
            if ((blankCount - 1) * acquireTimer.Interval >= 1000)
            {
                // Clear the readout, bargraph and alive flag after a certain number of missed updates
                isAlive = false;
                UpdateStatusLabels();
                ClearReadout();
                ClearBargraph();
            }

            // Always update the blank count (it will be reset if there was a successful update)
            blankCount++;

            // Handle pending timer updates
            if (isTimerUpdatePending)
                UpdateTimer();

            // Read all available packets
            while (meter.IsAvailable)
            {
                Packet sample;
                if (!meter.Receive(out sample))
                {
                    // Update the blank screen count
                    blankCount++;

                    // And turn the sampling on again
                    acquireTimer.Start();
                    return;
                }

                // Clear the blank screen count and set the alive flag after a successful reception
                blankCount = 0;
                if (!isAlive)
                {
                    isAlive = true;
                    UpdateStatusLabels();
                }

                // Handle regular display
                // Set the mode label accordingly first
                bool validMode = true;
                bool valueMode = true;
                switch (sample.Mode)
                {
                    case Mode.Ampere:
                        modeLabel.Text = "Strom";
                        break;

                    case Mode.AmpereMicro:
                        modeLabel.Text = "Strom";
                        break;

                    case Mode.AmpereMilli:
                        modeLabel.Text = "Strom";
                        break;

                    case Mode.ContinuityOhm:
                        modeLabel.Text = "Kontinuität";
                        break;

                    case Mode.DiodeVolt:
                        modeLabel.Text = "Diode";
                        break;

                    case Mode.ResistanceOhm:
                        modeLabel.Text = "Widerstand";
                        break;

                    case Mode.VoltAC:
                        modeLabel.Text = "Spannung (AC)";
                        break;

                    case Mode.VoltDC:
                        modeLabel.Text = "Spannung (DC)";
                        break;

                    case Mode.Squarewave:
                        modeLabel.Text = "Rechtecksignal";
                        valueMode = false;
                        break;

                    default:
                        modeLabel.Text = "Unbekannter Modus!";
                        valueMode = false;
                        validMode = false;
                        break;
                }

                // Allocate variables for the number parsing
                bool negative = false;
                int integer = 0, fractional = 0, exponent = 0, precision = 0;
                char unit = '\0', unitPrefix = '\0';

                // Attempt to parse the number
                if (validMode && valueMode)
                    validMode = Multimeter.Parse(sample, out negative, out integer, out fractional, out exponent, out precision, out unit, out unitPrefix);

                // Check, if the mode is invalid or that the number is OL
                bool overloaded = Multimeter.IsOverloaded(sample);
                if (overloaded && valueMode) // Overload
                {
                    // Update the value and unit labels
                    valueLabel.Text = "OL";
                    unitLabel.Text = unit.ToString();

                    // Fill the bargraph
                    FillBargraph();
                }
                else if (!validMode || !valueMode)
                {
                    // No valid mode
                    valueLabel.Text = "";
                    unitLabel.Text = "";

                    // Clear the bargraph
                    ClearBargraph();

                    // Continue and skip the rest
                    continue;
                }
                else
                {
                    // Just format the value according to the precision
                    valueLabel.Text = precision < 1 ? integer.ToString() :
                        string.Format("{0}{1}.{2:D" + precision.ToString() + "}", negative ? "-" : "", integer, fractional);
                    // Also update the unit label
                    unitLabel.Text = unitPrefix == '\0' ? unit.ToString() : string.Format("{0}{1}", unitPrefix, unit);
                }

                // Update the UI
                if (!overloaded)
                {
                    bargraphBar.Minimum = 0;
                    bargraphBar.Maximum = sample.Value < 0 ? Math.Abs(Multimeter.RangeMin(sample.Mode, sample.Range)) : Multimeter.RangeMax(sample.Mode, sample.Range);
                    bargraphBar.Value = Math.Abs(sample.Value);
                }

                // Handle record mode only for valid value modes
                if (isRecording)
                {
                    // Calculate the offset in seconds
                    TimeSpan delta = sample.ReceptionTime - recordingStart;

                    // Assemble the line, e.g.: 4.32,12.34E-3,V
                    // Start with the time offset
                    recordingBuffer.AppendFormat(new NumberFormatInfo() { NumberDecimalSeparator = GetCSVFractionalSeparator().ToString(), NumberDecimalDigits = 2 },
                        "{0:E}", delta.TotalSeconds);
                    // Followed by the delimiter
                    recordingBuffer.Append(GetCSVDelimiter());
                    // Followed by the value
                    if (overloaded) // Overload
                        recordingBuffer.Append("OL");
                    else if (precision < 1) // Integer value
                        recordingBuffer.AppendFormat("{0}E{1}", integer, exponent);
                    else // Fixed point value
                        recordingBuffer.AppendFormat("{0}{1}{2}{3:D" + precision.ToString() + "}E{4}", negative ? "-" : "", integer,
                            GetCSVFractionalSeparator(), fractional, exponent);
                    // Followed by the delimiter
                    recordingBuffer.Append(GetCSVDelimiter());
                    // Followed by the unit
                    recordingBuffer.Append(unit);
                    // And a final line break
                    recordingBuffer.AppendLine();

                    // Update the running for label and increment the sample counter
                    recordingCount++;
                    UpdateStatusLabels();
                }
            }

            // Start the timer again
            acquireTimer.Start();
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            // If already connected, stop the connection and return
            if (isConnected)
            {
                // Stop the acquisition
                PauseAcquire();
                
                // Stop and save any recording
                if (isRecording)
                {
                    StopRecording();
                    SaveRecording();
                }

                // Disconnect and return
                Disconnect();
                return;
            }

            // If not connected, establish the connection
            // Try to fetch the port name first
            if (portsListBox.SelectedItem == null || !(portsListBox.SelectedItem is string))
                return;
            string portName = (string)portsListBox.SelectedItem;

            // Establish the connection
            Connect(portName);
        }

        private void acquirePauseButton_Click(object sender, EventArgs e)
        {
            // Toggle the acquisition state
            if (isPaused)
                StartAcquire();
            else
                PauseAcquire();
        }

        private void recordToggleButton_Click(object sender, EventArgs e)
        {
            // Handle running recordings
            if (isRecording)
            {
                StopRecording();
                SaveRecording();
                return;
            }

            // Make sure that the DMM is connected
            if (!isConnected)
                return;

            // Start the recording
            StartRecording();
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Pause the acquisition
            PauseAcquire();

            // Stop and save any recording
            if (isRecording)
            {
                StopRecording();
                SaveRecording();
            }
        }

        private void intervalNumeric_ValueChanged(object sender, EventArgs e)
        {
            if (!isConnected || isPaused)
                UpdateTimer();
            else
                isTimerUpdatePending = true;
        }

        private void intervalNumeric_Leave(object sender, EventArgs e)
        {
            if (!isConnected || isPaused)
                UpdateTimer();
            else
                isTimerUpdatePending = true;
        }

        private void intervalNumeric_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            e.Handled = true;
            e.SuppressKeyPress = true;
            if (!isConnected || isPaused)
                UpdateTimer();
            else
                isTimerUpdatePending = true;
        }

        /// <summary>
        /// Copies the currently displayed value to the clipboard.
        /// </summary>
        private void CopyValue()
        {
            // Assemble the value
            string value = valueLabel.Text + unitLabel.Text;

            // Only continue, if the value is not empty
            if (string.IsNullOrWhiteSpace(value))
                return;

            // Ask the user about copying the value into the clipboard
            if (MessageBox.Show(string.Format("Wert \"{0}\" in die Zwischenablage kopieren?", value),
                "Wert kopieren", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                Clipboard.SetText(value);
        }

        /// <summary>
        /// Refreshes the list of displayed ports.
        /// </summary>
        private void RefreshPorts()
        {
            // Make sure that the list might be refreshed
            if (!portsListBox.Enabled)
                return;

            // Catch any potential errors from the SerialPort function
            try
            {
                // Fetch the list of ports
                string[] ports = SerialPort.GetPortNames();

                // Clear the old list
                portsListBox.Items.Clear();

                // Add all items to it
                portsListBox.Items.AddRange(ports);

                // If any ports are present, select the first
                if (ports.Length > 0)
                    portsListBox.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                // If there was any error, exit, as there is no recovery from this
                MessageBox.Show(string.Format(
                    "Abrufen der Portliste fehlgeschlagen!\nDas Programm wird beendet.\n\nFehler: {0}", ex.Message),
                    "Kritischer Fehler!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                this.Close();
            }
        }

        /// <summary>
        /// Establish a connection to the DMM.
        /// </summary>
        /// <param name="portName">The name of the serial port to use.</param>
        private void Connect(string portName)
        {
            // Validate the port name
            if (string.IsNullOrWhiteSpace(portName))
                return;

            // Initialize the multimeter object
            meter = new Multimeter(portName);

            // Set the alive flag and clear the blank counter
            isAlive = true;
            blankCount = 0;

            // Try to establish the connection
            Exception error = meter.Connect();
            if (error != null)
            {
                // If there was any error, exit, as there is no recovery from this
                MessageBox.Show(string.Format(
                    "Port {0} konnte nicht geöffnet werden!\n\nFehler: {1}", portName, error.Message),
                    "Fehler!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                ChangeUI(false);
                return;
            }

            // Change the UI
            ChangeUI(true);

            // Start the timer
            acquireTimer.Start();
        }

        /// <summary>
        /// Disconnect from the DMM.
        /// </summary>
        private void Disconnect()
        {
            // If there is a valid DMM object, call its' disconnect function
            if (meter != null)
                meter.Disconnect();

            // Stop the timer
            acquireTimer.Stop();

            // Update the UI
            ChangeUI(false);
        }

        /// <summary>
        /// If possible, begins acquiring data
        /// </summary>
        private void StartAcquire()
        {
            // Make sure the connection is established and that the acquisition is paused
            if (!isConnected || !isPaused)
                return;

            // Update the UI
            ChangeUI(true, false, isRecording);
        }

        /// <summary>
        /// If possible, pauses acquiring data
        /// </summary>
        private void PauseAcquire()
        {
            // Make sure the connection is established and that the acquisition is running
            if (!isConnected || isPaused)
                return;

            // Update the UI
            ChangeUI(true, true, isRecording);
        }

        /// <summary>
        /// Starts the recording and writes the CSV header.
        /// </summary>
        private void StartRecording()
        {
            // Make sure that the recording is not running
            if (!isConnected || isRecording)
                return;
            
            // Clear the buffer and get the current timestamp
            recordingBuffer.Clear();
            recordingStart = DateTime.Now;
            
            // Write the model
            recordingBuffer.Append("Model");
            recordingBuffer.Append(GetCSVDelimiter());
            recordingBuffer.Append("Parkside PDM300 C2");
            recordingBuffer.Append(GetCSVDelimiter());
            recordingBuffer.AppendLine();

            // Write the version
            recordingBuffer.Append("Software");
            recordingBuffer.Append(GetCSVDelimiter());
            recordingBuffer.AppendFormat("{0} {1} by Benedikt Muessig", Application.ProductName, Application.ProductVersion);
            recordingBuffer.Append(GetCSVDelimiter());
            recordingBuffer.AppendLine();

            // Write the date
            recordingBuffer.Append("Datum");
            recordingBuffer.Append(GetCSVDelimiter());
            recordingBuffer.AppendFormat("{0:00}.{1:00}.{2:0000}", recordingStart.Day, recordingStart.Month, recordingStart.Year);
            recordingBuffer.Append(GetCSVDelimiter());
            recordingBuffer.AppendLine();

            // Write the time
            recordingBuffer.Append("Uhrzeit");
            recordingBuffer.Append(GetCSVDelimiter());
            recordingBuffer.AppendFormat("{0:00}:{1:00}:{2:00}:{3:000}",
                recordingStart.Hour, recordingStart.Minute, recordingStart.Second, recordingStart.Millisecond);
            recordingBuffer.Append(GetCSVDelimiter());
            recordingBuffer.AppendLine();

            recordingBuffer.Append("Intervall (s)");
            recordingBuffer.Append(GetCSVDelimiter());
            recordingBuffer.AppendFormat(new NumberFormatInfo() { NumberDecimalSeparator = GetCSVFractionalSeparator().ToString(), NumberDecimalDigits = 2 },
                        "{0:E}", acquireTimer.Interval / 1000d);
            recordingBuffer.Append(GetCSVDelimiter());
            recordingBuffer.AppendLine();
            recordingBuffer.AppendLine();

            // Write the CSV header (3 columns)
            recordingBuffer.Append("Zeitdifferenz (s)");
            recordingBuffer.Append(GetCSVDelimiter());
            recordingBuffer.Append("Wert");
            recordingBuffer.Append(GetCSVDelimiter());
            recordingBuffer.Append("Einheit");
            recordingBuffer.AppendLine();

            // Update the UI
            ChangeUI(isConnected, isPaused, true);
        }

        /// <summary>
        /// Stops the recording.
        /// </summary>
        private void StopRecording()
        {
            // Make sure that the recording is running
            if (!isRecording)
                return;

            // Change the UI and flags
            ChangeUI(isConnected, isPaused, false);
        }

        /// <summary>
        /// Attempts to save the recording. Asks the user to specify a filename.
        /// </summary>
        private void SaveRecording()
        {
            // Make sure the recording is stopped
            if (isRecording)
                StopRecording();

            // Make sure that data is available
            if (recordingBuffer.Length < 1)
                return;

            // Present the save dialog and keep asking to save
            while (true)
            {
                while (recordSaveDialog.ShowDialog() != DialogResult.OK)
                    if (MessageBox.Show(string.Format("Möchten Sie wirklich die {0} aufgezeichneten Datenpunkte ungespeichert verwerfen?", recordingCount),
                        "Daten verwerfen", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        return;

                // Save the data
                try
                {
                    File.WriteAllText(recordSaveDialog.FileName, recordingBuffer.ToString());
                    recordingBuffer.Clear();
                    recordSaveDialog.FileName = "";
                    return;
                }
                catch (Exception)
                {
                    MessageBox.Show("Speichern fehlgeschlagen!\nBitte versuchen Sie es erneut!", "Speichern fehlgeschlagen",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        /// <summary>
        /// Change the current mode and the UI accordingly, without affecting the recording setting, as long, as the connection is not stopped.
        /// </summary>
        /// <param name="isConnected">Indicates, whether the DMM is currently connected.</param>
        private void ChangeUI(bool isConnected)
        {
            ChangeUI(isConnected, false, false);
        }

        /// <summary>
        /// Change the current mode and the UI accordingly.
        /// </summary>
        /// <param name="isConnected">Indicates, whether the DMM is currently connected.</param>
        /// <param name="isPaused">Indicates, whether the acquisition is currently paused.</param>
        /// <param name="isRecording">Indicates, whether the recorder is currently running.</param>
        private void ChangeUI(bool isConnected, bool isPaused, bool isRecording)
        {
            // Sanitize the new local values
            isPaused = isConnected ? isPaused : false;
            isRecording = isConnected ? isRecording : false;

            // Copy the values to the global context
            this.isConnected = isConnected;
            this.isPaused = isPaused;
            this.isRecording = isRecording;

            // Update the status labels
            UpdateStatusLabels();

            // Decide what to do with the UI
            refreshPortsButton.Enabled = !isConnected;
            portsListBox.Enabled = !isConnected;
            connectButton.Text = isConnected ? "Stop" : "Start";
            recordToggleButton.Text = (isRecording && isConnected) ? "Speichern" : "Aufzeichnen";
            acquirePauseButton.Text = (isPaused && isConnected) ? "Weiter" : "Pause";
            recordToggleButton.Enabled = isConnected;
            acquirePauseButton.Enabled = isConnected;
            csvFormatGroup.Enabled = isConnected && !isRecording;
            intervalNumeric.Enabled = !isRecording;

            // Clear the readout and bargraph after disconnecting
            if (!isConnected)
            {
                ClearReadout();
                ClearBargraph();
            }
        }

        /// <summary>
        /// Updates the status labels.
        /// </summary>
        private void UpdateStatusLabels()
        {
            // Handle idle
            if (!isConnected)
            {
                connectionStatusLabel.Text = "Status: Nicht verbunden.";
                acquireStatusLabel.Text = connectionStatusLabel.Text;
                return;
            }

            // Always set the connected label
            connectionStatusLabel.Text = "Status: Verbunden.";

            // Assemble the acquire status text
            StringBuilder statusBuilder = new StringBuilder("Status: ");
            statusBuilder.Append(isAlive ? (isPaused ? "Pausiert." : "Laufend.") : "Stumm.");
            if (isRecording)
            {
                // Calculate the time that has passed
                TimeSpan recordingSpan = DateTime.Now - recordingStart;
                int seconds = recordingSpan.Seconds, minutes = recordingSpan.Minutes, hours = recordingSpan.Hours + recordingSpan.Days * 24;

                // Update the builder
                statusBuilder.AppendFormat(" Aufzeichnung ({0} seit {1:00}h {2:00}m {3:00}s).", recordingCount, hours, minutes, seconds);
            }

            // Apply the label
            acquireStatusLabel.Text = string.Copy(statusBuilder.ToString());
            statusBuilder.Clear();
        }

        /// <summary>
        /// Returns the localized CSV delimiter.
        /// </summary>
        /// <returns></returns>
        private char GetCSVDelimiter()
        {
            return csvFormatDERadio.Checked ? ';' : ',';
        }

        /// <summary>
        /// Returns the localized CSV fractional separator.
        /// </summary>
        /// <returns></returns>
        private char GetCSVFractionalSeparator()
        {
            return csvFormatDERadio.Checked ? ',' : '.';
        }

        /// <summary>
        /// Updates the timer's interval immediately and clears the pending flag.
        /// </summary>
        private void UpdateTimer()
        {
            isTimerUpdatePending = false;
            acquireTimer.Interval = (int)intervalNumeric.Value * 500 + 50;
        }

        /// <summary>
        /// Clears the whole readout.
        /// </summary>
        private void ClearReadout()
        {
            modeLabel.Text = "";
            unitLabel.Text = "";
            valueLabel.Text = "";
        }

        /// <summary>
        /// Fills the bargraph display.
        /// </summary>
        private void FillBargraph()
        {
            bargraphBar.Minimum = 0;
            bargraphBar.Maximum = 1;
            bargraphBar.Value = 1;
        }

        /// <summary>
        /// Clears the bargraph display.
        /// </summary>
        private void ClearBargraph()
        {
            bargraphBar.Minimum = 0;
            bargraphBar.Maximum = 1;
            bargraphBar.Value = 0;
        }
    }
}
