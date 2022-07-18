using System;
using System.IO.Ports;

namespace ParksideView
{
    /// <summary>
    /// Represents a multimeter device and takes care of the communication.
    /// </summary>
    public class Multimeter
    {
#if !BLUETOOTH
        private const int BAUDRATE = 2400;
#else
        private const int BAUDRATE = 38400;
#endif

        /// <summary>
        /// The name or path of the serial port to connect to.
        /// </summary>
        public string PortName { get; private set; }

        /// <summary>
        /// The timeout in milliseconds, after which a synchronization or reception attempt is cancelled.
        /// </summary>
        public int TimeoutMilliseconds { get; private set; }
        
        /// <summary>
        /// Indicates, whether the multimeter is still connected.
        /// </summary>
        public bool IsConnected
        {
            get
            {
                // Make sure that the port is open and working
                // All port operations are wrapped in try-blocks to make sure no IO exception ever occurs
                try
                {
                    return port != null && port.IsOpen;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Indicates, whether there are new packets to read.
        /// </summary>
        public bool IsAvailable
        {
            get
            {
                // If the stream is synchronized, the two byte header has already been consumed, therefore, fewer bytes are required
                try
                {
                    return IsConnected && (IsSynchronized && port.BytesToRead >= 8 || !IsSynchronized && port.BytesToRead >= 10);
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        
        /// <summary>
        /// Indicates, whether the data stream is synchronized to the data after the next packet header.
        /// </summary>
        public bool IsSynchronized { get; private set; }

        /// <summary>
        /// The internally used serial port object.
        /// </summary>
        private SerialPort port;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="portName">The name or path of the serial port to connect to.</param>
        /// <param name="timeout">The timeout, after which a synchronization or reception attempt is cancelled.</param>
        public Multimeter(string portName, int timeoutMilliseconds = 1000)
        {
            // Sanity check the port name
            if (string.IsNullOrWhiteSpace(portName))
                throw new ArgumentNullException(portName);
            if(timeoutMilliseconds < 1)
                throw new ArgumentOutOfRangeException("timeoutMilliseconds");

            // Copy the values
            PortName = portName;
            TimeoutMilliseconds = timeoutMilliseconds;

            // Create the new serial port object
            port = new SerialPort(portName, BAUDRATE, Parity.None, 8, StopBits.One)
            {
                ReadBufferSize = 1000,
                ReadTimeout = timeoutMilliseconds,
                Handshake = System.IO.Ports.Handshake.None,
                DtrEnable = false,
                RtsEnable = false
            };

            // Initialize the synchronization flag
            IsSynchronized = false;
        }

        /// <summary>
        /// Attempts to establish a connection to the multimeter device.
        /// </summary>
        /// <returns>Null, if a connection could be successfully established, or the error, if there was one.</returns>
        public Exception Connect()
        {
            // Try to establish a connection
            try
            {
                port.Open();
            }
            catch (Exception ex)
            {
                return ex;
            }

            // Success
            return null;
        }

        /// <summary>
        /// Disconnects any open connection to the multimeter.
        /// </summary>
        public void Disconnect()
        {
            // Try to close the port
            // If it throws an error, it does not matter
            try
            {
                if (IsConnected)
                    port.Close();
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Discards any data in the reception buffer.
        /// </summary>
        /// <returns>True, if flushing the data stream succeeded, or false, if there was an error.</returns>
        public bool Flush()
        {
            // Clear the synchronization flag
            IsSynchronized = false;

            // Try to flush the port
            try
            {
                if (IsConnected)
                {
                    port.DiscardInBuffer();
                    return true;
                }
            }
            catch (Exception) { }

            // Fall through on error or if the port is closed
            return false;
        }

        /// <summary>
        /// Attempts to manually synchronize the data stream to the data after the next packet header. Uses the timeout.
        /// </summary>
        /// <returns>True, if the data stream could be synchronized within the timeout, or false, if there was an error.</returns>
        private bool Synchronize()
        {
            // Clear the synchronization flag
            IsSynchronized = false;

            // Try to synchonize the data stream
            try
            {
                // Make sure that the connection is established
                if (!IsConnected)
                    return false;
                
                // Retry at most ten times
                for (int retry = 0; retry < 10; retry++)
                {
                    // Check the first byte
                    if (port.ReadByte() != 0xdc)
                        continue;

                    // Check the second byte
                    if (port.ReadByte() != 0xba)
                        continue;

                    // Set the synchronization flag and return success
                    IsSynchronized = true;
                    return true;
                }
            }
            catch (Exception) { }

            // Fall through on error or if the port is closed
            return false;
        }

        /// <summary>
        /// Attempts to receive a single packet. If the data stream is not synchronized, it will take care of this. Uses the timeout.
        /// </summary>
        /// <param name="result">A variable that the result of the receive operation will be stored to.</param>
        /// <returns>True, if a packet could be received, or false, if there was an error.</returns>
        public bool Receive(out Packet result)
        {
            // Temporarily assign the result argument
            result = default(Packet);

            try
            {
                // Synchronize, if necessary
                if (!IsSynchronized)
                    if (!Synchronize())
                        return false;

                // Clear the synchronization flag again
                IsSynchronized = false;

                // Allocate a temporary buffer and a checksum buffer
                byte[] buffer = new byte[8];
                ushort actualChecksum = 0;

                // Attempt to read the eight bytes
                for (int i = 0; i < buffer.Length; i++)
                {
                    // Read and validate the received data
                    int data = port.ReadByte();
                    if (data < byte.MinValue || data > byte.MaxValue)
                        return false;

                    // Store the byte
                    buffer[i] = (byte)data;

                    // Add packet bytes 2 to 7 (array bytes 0 to 5) to the checksum
                    if (i <= 5)
                        actualChecksum += (ushort)data;
                }

                // Assemble the expected checksum and the value
                ushort expectedChecksum = unchecked((ushort)((buffer[6] << 8) | buffer[7]));
                short value = unchecked((short)((buffer[4] << 8) | buffer[5]));

                // Create the result packet
                result = new Packet((Mode)buffer[1], (Range)buffer[2], value, expectedChecksum == actualChecksum, 
                    new byte[2] { buffer[0], buffer[3] }, DateTime.Now);
            }
            catch (Exception)
            {
                return false;
            }

            // Fall through to success
            return true;
        }

        /// <summary>
        /// Returns, whether a mode is valid or not.
        /// </summary>
        /// <param name="mode">The mode to check.</param>
        /// <param name="allowValueless">If true, valueless modes are valid too.</param>
        /// <returns>True, if the mode is valid, or false, if it is not.</returns>
        public static bool ModeValid(Mode mode, bool allowValueless = true)
        {
            switch (mode)
            {
                // Value modes
                case Mode.Ampere:
                case Mode.AmpereMicro:
                case Mode.AmpereMilli:
                case Mode.ContinuityOhm:
                case Mode.DiodeVolt:
                case Mode.ResistanceOhm:
                case Mode.VoltAC:
                case Mode.VoltDC:
                    return true;

                // Valueless modes
                case Mode.Squarewave:
                    return allowValueless;

                // All unknown modes
                default:
                    return false;
            }
        }

        /// <summary>
        /// Returns the smallest, raw value that given mode and range combination allows.
        /// Useful for making bargraph displays.
        /// </summary>
        /// <param name="mode">The mode.</param>
        /// <param name="range">The range.</param>
        /// <returns>Smallest raw value in this combination.</returns>
        public static int RangeMin(Mode mode, Range range)
        {
            // Check the mode first
            if (!ModeValid(mode, false))
                return 0;

            // Handle the exceptions next
            if (mode == Mode.VoltDC && range == Range.F)
                return -300;
            else if (mode == Mode.Ampere && range == Range.G)
                return -1000;

            // Handle ranges without negative numbers
            if (mode == Mode.VoltAC || mode == Mode.ResistanceOhm || mode == Mode.ContinuityOhm || mode == Mode.DiodeVolt)
                return 0;

            // Return full scale (-1999 counts) for all other ranges
            return -1999;
        }

        /// <summary>
        /// Returns the largest, raw value that given mode and range combination allows.
        /// Useful for making bargraph displays.
        /// </summary>
        /// <param name="mode">The mode.</param>
        /// <param name="range">The range.</param>
        /// <returns>Largest raw value in this combination.</returns>
        public static int RangeMax(Mode mode, Range range)
        {
            // Check the mode first
            if (!ModeValid(mode, false))
                return 0;

            // Handle the exceptions next
            if (mode == Mode.VoltDC && range == Range.F || mode == Mode.VoltAC && range == Range.F)
                return 300;
            else if (mode == Mode.Ampere && range == Range.G)
                return 1000;

            // Return full scale (1999 counts) for all other ranges
            return 1999;
        }

        /// <summary>
        /// Checks, whether the input packet indicates an overload condition.
        /// </summary>
        /// <param name="sample">The packet to parse.</param>
        /// <returns>True, if the sample indicates an overload, or false, if not.</returns>
        public static bool IsOverloaded(Packet sample)
        {
            // Handle the square wave separately
            if (sample.Mode == Mode.Squarewave)
                return true;

            // Check, if the value is within bounds
            return sample.Value < RangeMin(sample.Mode, sample.Range) || sample.Value > RangeMax(sample.Mode, sample.Range);
        }

        /// <summary>
        /// Attempts to parse the input packet and returns the integer, fractional, unit and exponent of the value.
        /// </summary>
        /// <param name="sample">The packet to parse.</param>
        /// <param name="negative">If true, the number is negative.</param>
        /// <param name="integer">The signed integer part of the value.</param>
        /// <param name="fractional">The unsigned fractional part of the value.</param>
        /// <param name="unit">A char variable that the electrical base unit of the value is stored to.</param>
        /// <param name="unitPrefix">A char variable that the optional prefix (null if there is none) of the unit will be stored to.</param>
        /// <param name="exponent">An integer variable that the exponent of the value, relative to the base unit, will be stored to.</param>
        /// <param name="precision">An integer variable that precision (number of decimal places) of the value will be stored to.</param>
        /// <returns>True, if the packet could be successfully parsed, or false, if there was an error.</returns>
        public static bool Parse(Packet sample, out bool negative, out int integer, out int fractional, out int exponent, out int precision, out char unit, out char unitPrefix)
        {
            // Set the out variables to default values
            integer = 0;
            fractional = 0;
            exponent = 0;
            precision = 0;
            unit = '\0';
            unitPrefix = '\0';
            negative = false;

            // Validate the checksum first
            if (!sample.ChecksumValid)
                return false;

            // Switch the mode
            switch (sample.Mode)
            {
                // Volt DC
                case Mode.VoltDC:
                    // Switch the ranges
                    switch (sample.Range)
                    {
                        // 0000 0010: mV DC [000.0] E-1
                        case Range.B:
                            // Set the prefix, exponent and precision and calculate the integer and fractional parts
                            unitPrefix = 'm';
                            exponent = -3;
                            precision = 1;
                            integer = Math.Abs(sample.Value / 10);
                            fractional = Math.Abs(sample.Value % 10);
                            break;

                        // 0000 0100: V DC  [0.000] E-3
                        case Range.C:
                            // Set the prefix, exponent and precision and calculate the integer and fractional parts
                            unitPrefix = '\0';
                            exponent = 0;
                            precision = 3;
                            integer = Math.Abs(sample.Value / 1000);
                            fractional = Math.Abs(sample.Value % 1000);
                            break;

                        // 0000 1000: V DC  [00.00] E-2
                        case Range.D:
                            // Set the prefix, exponent and precision and calculate the integer and fractional parts
                            unitPrefix = '\0';
                            exponent = 0;
                            precision = 2;
                            integer = Math.Abs(sample.Value / 100);
                            fractional = Math.Abs(sample.Value % 100);
                            break;

                        // 0001 0000: V DC  [000.0] E-1
                        case Range.E:
                            // Set the prefix, exponent and precision and calculate the integer and fractional parts
                            unitPrefix = '\0';
                            exponent = 0;
                            precision = 1;
                            integer = Math.Abs(sample.Value / 10);
                            fractional = Math.Abs(sample.Value % 10);
                            break;

                        // 0010 0000: V DC  [0000.] E-0
                        case Range.F:
                            // Set the prefix, exponent and precision and calculate the integer and fractional parts
                            unitPrefix = '\0';
                            exponent = 0;
                            precision = 0;
                            integer = Math.Abs(sample.Value);
                            fractional = 0;
                            break;

                        // Invalid range
                        default:
                            return false;
                    }

                    // Set the unit
                    unit = 'V';

                    // Fallthrough
                    break;

                // Volt AC
                case Mode.VoltAC:
                    // Switch the ranges
                    switch (sample.Range)
                    {
                        // 0000 0100: V AC [0.000] E-3
                        case Range.C:
                            // Set the exponent and precision and calculate the integer and fractional parts
                            exponent = 0;
                            precision = 3;
                            integer = Math.Abs(sample.Value / 1000);
                            fractional = Math.Abs(sample.Value % 1000);
                            break;

                        // 0000 1000: V AC [00.00] E-2
                        case Range.D:
                            // Set the exponent and precision and calculate the integer and fractional parts
                            exponent = 0;
                            precision = 2;
                            integer = Math.Abs(sample.Value / 100);
                            fractional = Math.Abs(sample.Value % 100);
                            break;

                        // 0001 0000: V AC [000.0] E-1
                        case Range.E:
                            // Set the exponent and precision and calculate the integer and fractional parts
                            exponent = 0;
                            precision = 1;
                            integer = Math.Abs(sample.Value / 10);
                            fractional = Math.Abs(sample.Value % 10);
                            break;

                        // 0010 0000: V AC [0000.] E-0
                        case Range.F:
                            // Set the exponent and precision and calculate the integer and fractional parts
                            exponent = 0;
                            precision = 0;
                            integer = Math.Abs(sample.Value);
                            fractional = 0;
                            break;

                        // Invalid range
                        default:
                            return false;
                    }

                    // Set the unit and prefix
                    unit = 'V';
                    unitPrefix = '\0';

                    // Fallthrough
                    break;

                // Ampere
                case Mode.Ampere:
                    // Switch the ranges
                    switch (sample.Range)
                    {
                        // 0010 0000: A [0.000] E-3
                        case Range.F:
                            // Set the exponent and precision and calculate the integer and fractional parts
                            exponent = 0;
                            precision = 3;
                            integer = Math.Abs(sample.Value / 1000);
                            fractional = Math.Abs(sample.Value % 1000);
                            break;

                        // 0100 0000: A [00.00] E-2
                        case Range.G:
                            // Set the exponent and precision and calculate the integer and fractional parts
                            exponent = 0;
                            precision = 2;
                            integer = Math.Abs(sample.Value / 100);
                            fractional = Math.Abs(sample.Value % 100);
                            break;

                        // Invalid range
                        default:
                            return false;
                    }

                    // Set the unit and prefix
                    unit = 'A';
                    unitPrefix = '\0';

                    // Fallthrough
                    break;

                // Milliampere
                case Mode.AmpereMilli:
                    // Switch the ranges
                    switch (sample.Range)
                    {
                        // 0000 1000: mA [00.00] E-2
                        case Range.D:
                            // Set the exponent and precision and calculate the integer and fractional parts
                            exponent = -3;
                            precision = 2;
                            integer = Math.Abs(sample.Value / 100);
                            fractional = Math.Abs(sample.Value % 100);
                            break;

                        // 0001 0000: mA [000.0] E-1
                        case Range.E:
                            // Set the exponent and precision and calculate the integer and fractional parts
                            exponent = -3;
                            precision = 1;
                            integer = Math.Abs(sample.Value / 10);
                            fractional = Math.Abs(sample.Value % 10);
                            break;

                        // 0010 0000: mA [0.000] E-3
                        case Range.F:
                            // Set the exponent and precision and calculate the integer and fractional parts
                            exponent = -3;
                            precision = 3;
                            integer = Math.Abs(sample.Value / 1000);
                            fractional = Math.Abs(sample.Value % 1000);
                            break;

                        // Invalid range
                        default:
                            return false;
                    }

                    // Set the unit and prefix
                    unit = 'A';
                    unitPrefix = 'm';

                    // Fallthrough
                    break;

                // Microampere
                case Mode.AmpereMicro:
                    // Switch the ranges
                    switch (sample.Range)
                    {
                        // 0000 0010: uA [000.0] E-1
                        case Range.B:
                            // Set the exponent and precision and calculate the integer and fractional parts
                            exponent = -6;
                            precision = 1;
                            integer = Math.Abs(sample.Value / 10);
                            fractional = Math.Abs(sample.Value % 10);
                            break;

                        // 0000 0100: uA [0000.] E-0
                        case Range.C:
                            // Set the exponent and precision and calculate the integer and fractional parts
                            exponent = -6;
                            precision = 0;
                            integer = Math.Abs(sample.Value);
                            fractional = 0;
                            break;

                        // Invalid range
                        default:
                            return false;
                    }

                    // Set the unit and prefix
                    unit = 'A';
                    unitPrefix = 'µ';

                    // Fallthrough
                    break;

                // Resistance Ohms
                case Mode.ResistanceOhm:
                    // Switch the ranges
                    switch (sample.Range)
                    {
                        // 0000 0001: Ohm  [000.0] E-1
                        case Range.A:
                            // Set the prefix, exponent and precision and calculate the integer and fractional parts
                            unitPrefix = '\0';
                            exponent = 0;
                            precision = 1;
                            integer = Math.Abs(sample.Value / 10);
                            fractional = Math.Abs(sample.Value % 10);
                            break;

                        // 0000 0010: kOhm [0.000] E-3
                        case Range.B:
                            // Set the prefix, exponent and precision and calculate the integer and fractional parts
                            unitPrefix = 'k';
                            exponent = 3;
                            precision = 3;
                            integer = Math.Abs(sample.Value / 1000);
                            fractional = Math.Abs(sample.Value % 1000);
                            break;

                        // 0000 0100: kOhm [00.00] E-2
                        case Range.C:
                            // Set the prefix, exponent and precision and calculate the integer and fractional parts
                            unitPrefix = 'k';
                            exponent = 3;
                            precision = 2;
                            integer = Math.Abs(sample.Value / 100);
                            fractional = Math.Abs(sample.Value % 100);
                            break;

                        // 0000 1000: kOhm [000.0] E-1
                        case Range.D:
                            // Set the prefix, exponent and precision and calculate the integer and fractional parts
                            unitPrefix = 'k';
                            exponent = 3;
                            precision = 1;
                            integer = Math.Abs(sample.Value / 10);
                            fractional = Math.Abs(sample.Value % 10);
                            break;

                        // 0001 0000: MOhm [0.000] E-3
                        case Range.E:
                            // Set the prefix, exponent and precision and calculate the integer and fractional parts
                            unitPrefix = 'M';
                            exponent = 6;
                            precision = 3;
                            integer = Math.Abs(sample.Value / 1000);
                            fractional = Math.Abs(sample.Value % 1000);
                            break;

                        // 0010 0000: MOhm [00.00] E-2
                        case Range.F:
                            // Set the prefix, exponent and precision and calculate the integer and fractional parts
                            unitPrefix = 'M';
                            exponent = 6;
                            precision = 2;
                            integer = Math.Abs(sample.Value / 100);
                            fractional = Math.Abs(sample.Value % 100);
                            break;

                        // Invalid range
                        default:
                            return false;
                    }

                    // Set the unit
                    unit = 'Ω';

                    // Fallthrough
                    break;

                // Continuity Ohms
                case Mode.ContinuityOhm:
                    // 0000 0001: Ohm [000.0] E-1
                    // 0000 0100: Only for OL
                    // There is only a single range, make sure it is active
                    if (sample.Range != Range.A && sample.Range != Range.C)
                        return false;

                    // Set the unit, prefix, exponent and precision
                    unit = 'Ω';
                    unitPrefix = '\0';
                    exponent = 0;
                    precision = 1;

                    // Calculate the integer and fractional parts
                    integer = Math.Abs(sample.Value / 10);
                    fractional = Math.Abs(sample.Value % 10);

                    // Fallthrough
                    break;

                // Diode Volts
                case Mode.DiodeVolt:
                    // 0000 0100: V [0.000] E-3
                    // There is only a single range, make sure it is active
                    if (sample.Range != Range.C)
                        return false;

                    // Set the unit, prefix, exponent and precision
                    unit = 'V';
                    unitPrefix = '\0';
                    exponent = 0;
                    precision = 3;

                    // Calculate the integer and fractional parts
                    integer = Math.Abs(sample.Value / 1000);
                    fractional = Math.Abs(sample.Value % 1000);

                    // Fallthrough
                    break;

                // Squarewave (included for clarity) and invalid modes can't be parsed
                case Mode.Squarewave:
                default:
                    return false;
            }

            // Set the sign bit
            negative = sample.Value < 0;

            // Fall through to success
            return true;
        }
    }
}
