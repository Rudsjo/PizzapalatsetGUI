namespace TerminalServer
{
    /// <summary>
    /// Required namespaces
    /// </summary>
    #region Namespaces
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    #endregion

    /// <summary>
    /// This is the main ServerHandler
    /// </summary>
    public class ServerHandler
    {
        /// <summary>
        /// All of the public properties
        /// </summary>
        #region Properties

        /// <summary>
        /// All of the terminals that are connected
        /// </summary>
        public List<Terminal> ActiveTerminals { get; set; }

        /// <summary>
        /// The port that the server will listen on
        /// </summary>
        public int ServerListeningPort { get; set; }

        /// <summary>
        /// Current server staus
        /// </summary>
        public ServerStates ServerState { get; set; }

        /// <summary>
        /// Servers packet buffersize
        /// </summary>
        public int ServerBufferSize { get; set; }

        #endregion

        /// <summary>
        /// All of the private members
        /// </summary>
        #region Private Members

        /// <summary>
        /// The server socket
        /// </summary>
        private Socket ServerSocket;

        /// <summary>
        /// The server buffer
        /// </summary>
        private byte[] ServerBuffer;

        #endregion

        /// <summary>
        /// Default constructor
        /// </summary>
        public ServerHandler()
        {
            #region Set server settings
            ActiveTerminals     = new List<Terminal>();
            ServerListeningPort = 400;
            ServerState         = ServerStates.Offline;
            ServerBufferSize    = 128;
            ServerBuffer        = new byte[ServerBufferSize];
            #endregion
        }

        #region Functions

        /// <summary>
        /// Start the server
        /// </summary>
        /// <returns>True if successful</returns>
        public bool StartServer()
        {
            // Return if the server is running
            if (ServerState == ServerStates.Online)
                return false;

            // Create the socket
            ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the specified port
            ServerSocket.Bind(new IPEndPoint(IPAddress.Any, ServerListeningPort));
            ServerSocket.Listen(10);

            // Begin accepting connections
            ServerSocket.BeginAccept(new AsyncCallback(ServerAcceptCallback), null);

            // Startup was successful
            ServerState = ServerStates.Online;
            return true;
        }

        /// <summary>
        /// Stop the server
        /// </summary>
        public void Shutdown()
        {
            // Return if socket has not been set
            if (ServerSocket == null) return;

            // Set server state to offline
            ServerState = ServerStates.Offline;

            // Go through all of the active terminals and disconnect them from the server
            foreach (Terminal c in ActiveTerminals)
                c.Close(); // Closes the connection properly

            /* Close the ServerSocket and then clear
             * the list of active terminals
             */
            ServerSocket.Close();
            ActiveTerminals.Clear();
        }

        #endregion


        #region Socket callbacks

        /// <summary>
        /// When a client has connected
        /// </summary>
        /// <param name="ar"></param>
        private void ServerAcceptCallback(IAsyncResult ar)
        {
            /*    Procedure =>
             * 1- Accept the connection
             * 2- Wait for the client to identify itself
             * 3- Start receiving data from the terminal
             */

            // If the server is stopped while listening for connections, then this will fail
            try
            {
                Socket s = ServerSocket.EndAccept(ar);

                byte[] IdentificationBytes = new byte[2];
                s.Receive(IdentificationBytes);

                if (int.TryParse(Encoding.Default.GetString(IdentificationBytes), out int TerminalID) &&
                        // ID should never be less then 0
                            TerminalID >= 0 &&
                        // ID should exist in the Terminals enum
                            TerminalID < Enum.GetNames(typeof(Terminals)).Length
                    )
                {
                    Terminal t = new Terminal((Terminals)TerminalID, s);
                    s.BeginReceive(ServerBuffer, 0, ServerBuffer.Length, SocketFlags.None, new AsyncCallback(ServerReceiveCallback), s);
                    ActiveTerminals.Add(t);

                    Program.PrintMessage(true, $"[{s.RemoteEndPoint.ToString()}]:[{t.TerminalType.ToString()}] -> Connection Established", ConsoleColor.Green);
                }
                else
                    // Illigal identificationID - Close the socket
                    s.Close();

                ServerSocket.BeginAccept(new AsyncCallback(ServerAcceptCallback), null);
            }

            // Do nothing then
            catch { }
        }

        /// <summary>
        /// Begin receiving data from the terminal
        /// </summary>
        /// <param name="ar"></param>
        private void ServerReceiveCallback(IAsyncResult ar)
        {
            // This can fail if client disconnects
            try
            {
                Socket Client = (Socket)ar.AsyncState;
                var _terminal = ActiveTerminals.First(a => a.Connection.Equals(Client));

                // Check the lengt of the sent data
                int Rec = Client.EndReceive(ar);

                // Check if an empty packet was received
                if (Rec > 0)
                {
                    // Create new buffer and resize it to the correct size
                    byte[] ReceivedBytes = ServerBuffer;
                    Array.Resize(ref ReceivedBytes, Rec);

                    // Convert the bytes to a string
                    string Message = Encoding.Default.GetString(ReceivedBytes);

                    #region Received data
                    if (Message.Equals("[NEWORDER]"))
                    {
                        Program.PrintMessage(true, $"New order from [{Client.RemoteEndPoint}]:[{_terminal.TerminalType}]", ConsoleColor.White);

                        // Notify terminals
                        ActiveTerminals.Where(c => c.TerminalType.Equals(Terminals.ChefTerminal)||c.TerminalType.Equals(Terminals.InfoTerminal)).ToList().ForEach(t =>
                        {
                            // Try to send notifying message
                            if (t.SendData("[NEWORDER]"))
                            {
                                // Show what terminal was notified
                                Program.PrintMessage(false, $"Notifying -> [{t.Connection.RemoteEndPoint}]:[{t.TerminalType}]", ConsoleColor.Green);
                            } else {

                                // The terminal has disconnected so remove it and display message

                                Program.PrintMessage(false, $"[{t.Connection.RemoteEndPoint}] -> Not active", ConsoleColor.DarkRed);
                                t.Close();
                                ActiveTerminals.Remove(t);
                            }
                        });
                    }

                    if (Message.Equals("[ORDERBAKED]"))
                    {
                        Program.PrintMessage(true, $"[{Client.RemoteEndPoint}]:[{_terminal.TerminalType}] marked an order as baked", ConsoleColor.White);

                        // Notify terminals
                        ActiveTerminals.Where(c => c.TerminalType.Equals(Terminals.CashierTerminal) || c.TerminalType.Equals(Terminals.InfoTerminal)).ToList().ForEach(t =>
                        {
                            // Try to send notifying message
                            if (t.SendData("[ORDERBAKED]"))
                            {
                                // Show what terminal was notified
                                Program.PrintMessage(false, $"Notifying -> [{t.Connection.RemoteEndPoint}]:[{t.TerminalType}]", ConsoleColor.Green);
                            }
                            else
                            {

                                // The terminal has disconnected so remove it and display message

                                Program.PrintMessage(false, $"[{t.Connection.RemoteEndPoint}] -> Not active", ConsoleColor.DarkRed);
                                t.Close();
                                ActiveTerminals.Remove(t);
                            }
                        });
                    }
                    if (Message.Equals("[ORDERDONE]"))
                    {
                        Program.PrintMessage(true, $"An order was served from [{Client.RemoteEndPoint}]:[{_terminal.TerminalType}]", ConsoleColor.White);

                        // Notify terminals
                        ActiveTerminals.Where(c => c.TerminalType.Equals(Terminals.InfoTerminal)).ToList().ForEach(t =>
                        {
                            // Try to send notifying message
                            if (t.SendData("[ORDERDONE]"))
                            {
                                // Show what terminal was notified
                                Program.PrintMessage(false, $"Notifying -> [{t.Connection.RemoteEndPoint}]:[{t.TerminalType}]", ConsoleColor.Green);
                            } else {

                                // The terminal has disconnected so remove it and display message

                                Program.PrintMessage(false, $"[{t.Connection.RemoteEndPoint}] -> Not active", ConsoleColor.DarkRed);
                                t.Close();
                                ActiveTerminals.Remove(t);
                            }
                        });
                    }
                    #endregion
                }

                // Begin receiving again
                Client.BeginReceive(ServerBuffer, 0, ServerBuffer.Length,
                                                     SocketFlags.None,
                                                     new AsyncCallback(ServerReceiveCallback),
                                                     Client);
            }
            // Remove the connection
            catch { }
        }

        #endregion
    }
}
