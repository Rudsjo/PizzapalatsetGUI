namespace TerminalServer
{
    using System;

    /// <summary>
    /// Required namespaces
    /// </summary>
    #region Namespaces
    using System.Net.Sockets;
    using System.Text;
    #endregion

    /// <summary>
    /// Class that represents a terminal
    /// </summary>
    public class Terminal
    {
        /// <summary>
        /// Stores what terminal this class is
        /// </summary>
        public Terminals TerminalType { get; set; }

        /// <summary>
        /// Connection to the terminal
        /// </summary>
        public Socket Connection { get; private set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Terminal(Terminals Type, Socket socket)
        {
            this.TerminalType = Type;
            this.Connection   = socket;
        }

        #region Functions

        /// <summary>
        /// Send data to the terminal
        /// </summary>
        /// <param name="msg">Data to send</param>
        /// <returns>returns true if the data was successfuly sent</returns>
        public bool SendData(string msg)
        {
            // if socket is null or not connected, return false
            if (!IsConnected()) return false;

            // Send the message to the terminal
            Connection.Send(Encoding.Default.GetBytes(msg));

            // Message was sent successfuly
            return true;
        }

        /// <summary>
        /// Tests the connection
        /// </summary>
        /// <returns>returns true if connected</returns>
        public bool IsConnected()
        {
            if (!Connection.Connected)
                return false;

            bool p1 = Connection.Poll(1000, SelectMode.SelectRead);
            bool p2 = (Connection.Available == 0);
            if (p1 && p2)
                return false;
            else return true;
        }

        /// <summary>
        /// Disconnect the connection to this terminal
        /// </summary>
        public void Close()
        {
            // Send disconnecing message to terminal, then close the connection
            SendData(":dc");
            Connection.Close();
        }

        #endregion
    }
}
