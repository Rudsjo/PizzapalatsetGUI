namespace CustomerTerminalWPF
{
    /// <summary>
    /// Static class that contains data 
    /// about the program state
    /// </summary>
    public static class ProgramState
    {
        /// <summary>
        /// Is the program running
        /// </summary>
        public static bool IsRunning { get; set; } = false;

        /// <summary>
        /// Single instance of the connection
        /// </summary>
        public static ServerConnection ServerConnection { get; set; }

    }
}
