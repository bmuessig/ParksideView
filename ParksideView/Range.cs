namespace ParksideView
{
    /// <summary>
    /// The range that the value was taken in.
    /// </summary>
    public enum Range : byte
    {
        /// <summary>
        /// 0000 0000 (observed during startup)
        /// </summary>
        None = 0x0,

        /// <summary>
        /// 0000 0001
        /// </summary>
        A = 0x1,

        /// <summary>
        /// 0000 0010
        /// </summary>
        B = 0x2,

        /// <summary>
        /// 0000 0100
        /// </summary>
        C = 0x4,

        /// <summary>
        /// 0000 1000
        /// </summary>
        D = 0x8,

        /// <summary>
        /// 0001 0000
        /// </summary>
        E = 0x10,

        /// <summary>
        /// 0010 0000
        /// </summary>
        F = 0x20,

        /// <summary>
        /// 0100 0000
        /// </summary>
        G = 0x40,

        /// <summary>
        /// 1000 0000
        /// </summary>
        H = 0x80
    }
}
