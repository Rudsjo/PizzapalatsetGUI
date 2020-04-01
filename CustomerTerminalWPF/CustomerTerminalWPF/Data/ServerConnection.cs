namespace CustomerTerminalWPF
{
    /// <summary>
    /// Required namespaces
    /// </summary>
    #region Namespaces
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    #endregion

    /// <summary>
    /// Class that contains all of the server related stuff
    /// </summary>
    public class ServerConnection
    {
        #region Properties

        /// <summary>
        /// Server IP and Port
        /// </summary>
        public IPEndPoint Server { get; private set; }

        /// <summary>
        /// Current state
        /// </summary>
        public ConnectionStates ConnectionState
        {
            get
            {
                // Check connection and return value based on the connction state
                return (Connection == null || Connection.Connected == false) ? ConnectionStates.Disconnected : ConnectionStates.Connected;
            }
        }

        /// <summary>
        /// Server socket
        /// </summary>
        private Socket Connection;

        private ManualResetEvent WaitConnect;

        /// <summary>
        /// If the client should listen for incomming data from the server
        /// </summary>
        private bool ListenOnServer;

        /// <summary>
        /// Private buffer
        /// </summary>
        private byte[] Buffer;

        #endregion

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ServerConnection(bool ListenOnServer, int BufferSize)
        {
            // Create the reset event
            WaitConnect = new ManualResetEvent(false);

            // Set the listening mode
            this.ListenOnServer = ListenOnServer;

            // Set the buffer size
            this.Buffer = new byte[BufferSize];
        }

        /// <summary>
        /// Class Functions
        /// </summary>
        #region Functions

        /// <summary>
        /// Connect to the server
        /// </summary>
        /// <returns>true if connection was successful</returns>
        public Task<bool> ConnectAsync()
        {
            // Cancel if no EndPoint is created or allready is connected
            if (Server == null || ConnectionState == ConnectionStates.Connected)
                return Task.FromResult(false);

            // Create the socket
            Connection = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Connect to the server
            Connection.BeginConnect(Server, new AsyncCallback(ConnectCallback), null);

            // Wait until WaitHandler receives signal from the ConnectCallbackk
            WaitConnect.WaitOne();

            // Check if connection was successful
            return (ConnectionState == ConnectionStates.Connected) ? Task.FromResult(true) : Task.FromResult(false);
        }

        /// <summary>
        /// Disconnect from the server
        /// </summary>
        public void Disconnect()
        {
            // Cancel if socket is null
            if (Connection == null) return;

            Connection.Disconnect(false);
            Connection.Dispose();
        }

        /// <summary>
        /// Send message to the server
        /// </summary>
        /// <returns></returns>
        public bool SendMessage(string message)
        {          
            // Return false if not connected or Socket is null
            if (Connection == null || Connection.Connected == false) return false;

            // Send the message to the server
            Connection.Send(Encoding.Default.GetBytes(message));

            // Successful
            return true;
        }

        /// <summary>
        /// creates the endpoint for the server
        /// </summary>
        /// <param name="IP">Server IP address</param>
        /// <param name="Port">Server port</param>
        /// <returns>True if a valid EndPoint was provided</returns>
        public bool CreateEndPoint(string IP, uint Port)
        {
            // Try to convert provided string to an IPAddress
            if (IPAddress.TryParse(IP, out IPAddress newIP))
                Server = new IPEndPoint(newIP, (int)Port);

            // Failed to parse IPAddress
            else return false;

            // Successful
            return true;
        }

        /// <summary>
        /// Resolves a request from the server
        /// </summary>
        private void ResolveServerMessage(byte[] Req)
        {
            // Convert the bytes
            string RequestString = Encoding.Default.GetString(Req);
        }

        #endregion

        /// <summary>
        /// Socket Callbacks
        /// </summary>
        #region Callbacks
            
        /// <summary>
        /// Server Connect Callback
        /// </summary>
        private void ConnectCallback(IAsyncResult ar)
        {
            try 
            {
                // End Connect
                Connection.EndConnect(ar);

                WaitConnect.Set();

                // Send handshake message
                Connection.Send(Encoding.Default.GetBytes("0"));

                if (ListenOnServer)
                    // Start receiving packets from the server
                    Connection.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), Connection);
            }
            catch
            {
                WaitConnect.Set();
            }
        }

        /// <summary>
        /// Server Receive Callback
        /// </summary>
        /// <param name="ar"></param>
        private void ReceiveCallback(IAsyncResult ar)
        {            
            try
            {
                // Create internal socket
                Socket s = ar.AsyncState as Socket;
                // Received data size
                int Rec = s.EndReceive(ar);

                // Don't do anything if empty packet was received
                if(Rec > 0)
                {
                    // Create the new rezised byte[]
                    byte[] ReceivedBytes = Buffer;
                    Array.Resize(ref ReceivedBytes, Rec);

                    // Resolve the Message
                    ResolveServerMessage(ReceivedBytes);
                }

                // Start receiving packets from the server again
                Connection.BeginReceive(Buffer, 0, Buffer.Length,
                    SocketFlags.None,
                    new AsyncCallback(ReceiveCallback),
                    s);
            }
            catch { }
        }

        #endregion

    }
}
