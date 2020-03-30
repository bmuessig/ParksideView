namespace ParksideView
{
    /// <summary>
    /// Represents the mode that the reading was taken in.
    /// </summary>
    public enum Mode : byte
    {
        /// <summary>
        /// DC Volt
        /// </summary>
        VoltDC = 0x16,

        /// <summary>
        /// AC Volt
        /// </summary>
        VoltAC = 0x15,

        /// <summary>
        /// Microampere (AC or DC)
        /// </summary>
        AmpereMicro = 0x1a,

        /// <summary>
        /// Milliampere (AC or DC)
        /// </summary>
        AmpereMilli = 0x19,

        /// <summary>
        /// Ampere (AC or DC)
        /// </summary>
        Ampere = 0x18,

        /// <summary>
        /// Resistance in Ohms
        /// </summary>
        ResistanceOhm = 0x1d,

        /// <summary>
        /// Continuity in Ohms
        /// </summary>
        ContinuityOhm = 0x1b,

        /// <summary>
        /// Diode in Volts
        /// </summary>
        DiodeVolt = 0x1c,

        /// <summary>
        /// Squarewave mode (without readings)
        /// </summary>
        Squarewave = 0x3
    }
}
