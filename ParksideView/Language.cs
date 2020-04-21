// Set the language to German (undefine to keep it at English)
// This can also be done through the VS compile options
#if !EXTOPT
    #define GERMAN
#endif

namespace ParksideView
{
    /// <summary>
    /// Holds the entire UI translation for the language selected at compile time.
    /// </summary>
    internal static class Language
    {
#if GERMAN // Compile with German strings
        public const string ConnectionHeading = "Verbindung";
        public const string PortLabel = "Port:";
        public const string RefreshButton = "Aktualisieren";
        public const string StartButton = "Start";
        public const string PauseButton = "Pause";
        public const string StopButton = "Stop";
        public const string RecordButton = "Aufzeichnen";
        public const string SaveButton = "Speichern";
        public const string ContinueButton = "Weiter";
        public const string AcquisitionHeading = "Datenerfassung";
        public const string Interval = "Intervall: 500ms *";
        public const string CSVFormatHeading = "CSV-Format";
        public const string WindowHeading = "Fenster";
        public const string TopMostCheckBox = "Deckend";
        public const string MinimizeButton = "Minimieren";
        public const string AcqStatusRecording = " Aufzeichnung ({0} seit {1:00}h {2:00}m {3:00}s).";
        public const string AcqStatusPrefix = "Status: ";
        public const string AcqStatusRunning = "Laufend.";
        public const string AcqStatusPaused = "Pausiert.";
        public const string AcqStatusSilent = "Stumm.";
        public const string AcqStatusConnected = "Verbunden.";
        public const string AcqStatusDisconnected = "Nicht verbunden.";
        public const string SavingFailedText = "Speichern fehlgeschlagen!\nBitte versuchen Sie es erneut!";
        public const string SavingFailedTitle = "Speichern fehlgeschlagen";
        public const string DiscardDataText = "Möchten Sie wirklich die {0} aufgezeichneten Datenpunkte ungespeichert verwerfen?";
        public const string DiscardDataTitle = "Daten verwerfen";
        public const string SaveDialogTitle = "Aufzeichnung speichern unter";
        public const string SaveDialogFilter = "CSV-Dateien|*.csv";
        public const string CSVVersionFormat = "{0} {1} von Benedikt Muessig";
        public const string CSVUnit = "Einheit";
        public const string CSVValue = "Wert";
        public const string CSVDelta = "Zeitdifferenz (s)";
        public const string CSVInterval = "Intervall (s)";
        public const string CSVTime = "Uhrzeit";
        public const string CSVDate = "Datum";
        public const string CSVSoftware = "Software";
        public const string CSVModel = "Modell";
        public const string ConnectionErrorText = "Port {0} konnte nicht geöffnet werden!\n\nFehler: {1}";
        public const string ConnectionErrorTitle = "Verbindungsfehler";
        public const string PortsListErrorText = "Abrufen der Portliste fehlgeschlagen!\nDas Programm wird beendet.\n\nFehler: {0}";
        public const string PortsListErrorTitle = "Kritischer Fehler";
        public const string CopyValueText = "Wert \"{0}\" in die Zwischenablage kopieren?";
        public const string CopyValueTitle = "Wert kopieren";
        public const string ModeUnknown = "Unbekannter Modus";
        public const string ModeSquarewave = "Rechtecksignal";
        public const string ModeVoltageDC = "Spannung (DC)";
        public const string ModeVoltageAC = "Spannung (AC)";
        public const string ModeResistance = "Widerstand";
        public const string ModeDiode = "Diode";
        public const string ModeContinuity = "Kontinuität";
        public const string ModeCurrent = "Strom";
#else // Fallback to English strings
        public const string ConnectionHeading = "Connection";
        public const string PortLabel = "Port:";
        public const string RefreshButton = "Refresh";
        public const string StartButton = "Start";
        public const string PauseButton = "Pause";
        public const string StopButton = "Stop";
        public const string RecordButton = "Record";
        public const string SaveButton = "Save";
        public const string ContinueButton = "Continue";
        public const string AcquisitionHeading = "Acquisition";
        public const string Interval = "Interval: 500ms *";
        public const string CSVFormatHeading = "CSV format";
        public const string WindowHeading = "Window";
        public const string TopMostCheckBox = "Top most";
        public const string MinimizeButton = "Minimize";
        public const string AcqStatusRecording = " Recording ({0} since {1:00}h {2:00}m {3:00}s).";
        public const string AcqStatusPrefix = "Status: ";
        public const string AcqStatusRunning = "Running.";
        public const string AcqStatusPaused = "Paused.";
        public const string AcqStatusSilent = "Silent.";
        public const string AcqStatusConnected = "Connected.";
        public const string AcqStatusDisconnected = "Not connected.";
        public const string SavingFailedText = "Saving failed!\nPlease try again!";
        public const string SavingFailedTitle = "Saving failed";
        public const string DiscardDataText = "Do you really want to discard the {0} unsaved data points?";
        public const string DiscardDataTitle = "Discard data";
        public const string SaveDialogTitle = "Save recording as";
        public const string SaveDialogFilter = "CSV files|*.csv";
        public const string CSVVersionFormat = "{0} {1} by Benedikt Muessig";
        public const string CSVUnit = "Unit";
        public const string CSVValue = "Value";
        public const string CSVDelta = "Time delta (s)";
        public const string CSVInterval = "Interval (s)";
        public const string CSVTime = "Time";
        public const string CSVDate = "Date";
        public const string CSVSoftware = "Software";
        public const string CSVModel = "Model";
        public const string ConnectionErrorText = "Port {0} could not be opened!\n\nError: {1}";
        public const string ConnectionErrorTitle = "Connection error";
        public const string PortsListErrorText = "Could not fetch the ports list.!\nThe program will be terminated.\n\nError: {0}";
        public const string PortsListErrorTitle = "Critical error";
        public const string CopyValueText = "Shall the value \"{0}\" be copied to the clipboard?";
        public const string CopyValueTitle = "Copy value";
        public const string ModeUnknown = "Unknown mode";
        public const string ModeSquarewave = "Square wave";
        public const string ModeVoltageDC = "Voltage (DC)";
        public const string ModeVoltageAC = "Voltage (AC)";
        public const string ModeResistance = "Resistance";
        public const string ModeDiode = "Diode";
        public const string ModeContinuity = "Continuity";
        public const string ModeCurrent = "Current";
#endif // End of language definition
    }
}
