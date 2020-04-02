namespace TerminalServer
{
    /// <summary>
    /// Class that keeps track of the program
    /// </summary>
    public class ProgramState
    {
        /// <summary>
        /// Is the program running
        /// </summary>
        public static bool Running { get; set; }

        /// <summary>
        /// Single instance of the ServerHandlerClass
        /// </summary>
        public static ServerHandler Server { get; set; }
    }
}
