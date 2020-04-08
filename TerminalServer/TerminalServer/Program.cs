namespace TerminalServer
{
    /// <summary>
    /// Required namespaces
    /// </summary>
    #region Namespaces
    using System;
    #endregion

    /// <summary>
    /// Main program class
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Main entry point for the program
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            PrintStartupMessage();

            // Set running to true
            ProgramState.Running = true;

            // Create the server backend
            ProgramState.Server  = new ServerHandler();

            // Start the server loop
            while (ProgramState.Running)
                ServerLoop();
        }

        /// <summary>
        /// Class functions
        /// </summary>
        #region Functions

        /// <summary>
        /// First functin that executes on startup
        /// </summary>
        private static void PrintStartupMessage()
        {
            Console.WriteLine("Terminal Server [Version 1.0.0]");
            Console.WriteLine("(c) 2020 Tony's Pizzeria");
            Console.WriteLine();
            Console.WriteLine("Type ':h' to show all available commands");
            Console.WriteLine();
        }

        /// <summary>
        /// Shows all commands
        /// </summary>
        private static void ShowHelpScreen()
        {
            Console.WriteLine();
            Console.WriteLine("====================================================================");
            Console.WriteLine(":start - Starts the server");
            Console.WriteLine(":stop  - Disconnects all terminals and terminates the server");
            Console.WriteLine(":test  - Polls all active terminals and aborts the disconnected ones");
            Console.WriteLine(":cls   - Clears the terminal");
            Console.WriteLine(":h     - Show the help menu");
            Console.WriteLine("====================================================================");
            Console.WriteLine();
        }

        /// <summary>
        /// The main server loop
        /// </summary>
        private static void ServerLoop()
        {
            string Command = Console.ReadLine();
            ProcessCommand(Command);
        }

        /// <summary>
        /// Process the typed command
        /// </summary>
        /// <param name="Command"></param>
        private static void ProcessCommand(string Command)
        {
            // Check the command
            switch (Command.ToLower())
            {
                // Show help menu
                case ":h":
                    ShowHelpScreen();
                    break;

                // Start the server
                case ":start":
                    PrintMessage(true, "Creating the server socket...", ConsoleColor.Yellow);
                    if (ProgramState.Server.StartServer())
                        PrintMessage(true, "Server is now running",     ConsoleColor.Green);
                    else
                        PrintMessage(true, "There was an error while attempting to start the server", ConsoleColor.Red);
                    break;
                // Clear the console
                case ":stop":
                    ProgramState.Server.Shutdown();
                    PrintMessage(true, "Server was successfuly stopped", ConsoleColor.Green);
                    break;
                case ":cls": 
                    Console.Clear();
                    break;
                case ":exit": 
                    PrintMessage(false, "Stopping services...", ConsoleColor.Yellow);
                    ProgramState.Running = false;
                    ProgramState.Server.Shutdown();
                    PrintMessage(false, "Services stopped", ConsoleColor.Green);
                    PrintMessage(false, "Exiting...", ConsoleColor.Yellow);
                    break;
                case ":test":
                    // Return if server is'nt running
                    if(ProgramState.Server.ServerState != ServerStates.Online)
                    {
                        PrintMessage(false, "The server is not running", ConsoleColor.Yellow);
                        return;
                    }
                    // Loop through all connections and remove non active ones
                    for (int i = 0; i < ProgramState.Server.ActiveTerminals.Count; i++)
                        if (!ProgramState.Server.ActiveTerminals[i].IsConnected())
                        {
                            PrintMessage(false, $"[{ProgramState.Server.ActiveTerminals[i].Connection.RemoteEndPoint}] -> Not active", ConsoleColor.DarkRed);
                            ProgramState.Server.ActiveTerminals.RemoveAt(i);
                            i--;
                        }
                        else PrintMessage(false, $"[{ProgramState.Server.ActiveTerminals[i].Connection.RemoteEndPoint}] -> active", ConsoleColor.Green);
                    break;

                // Bad command
                default:
                    PrintMessage(false, $"'{Command}' is not recognized as a command, ':h' for help", ConsoleColor.White);
                    break;
            }
        }

        /// <summary>
        /// Prints a message to the console
        /// </summary>
        /// <param name="ShowTime">show the time</param>
        /// <param name="Message">message to show</param>
        /// <param name="MessageColor">message color</param>
        public static void PrintMessage(bool ShowTime, string Message, ConsoleColor MessageColor)
        {
            Console.Write(" ");
            if(ShowTime)
                Console.Write(DateTime.Now.ToString() + ": ");
            Console.ForegroundColor = MessageColor;
            Console.Write(Message + "\n");
            Console.ForegroundColor = ConsoleColor.White;
        }

        #endregion
    }
}
