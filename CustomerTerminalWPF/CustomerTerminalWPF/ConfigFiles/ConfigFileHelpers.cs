namespace CustomerTerminalWPF
{
    /// <summary>
    /// Required namespaces
    /// </summary>
    #region Namespaces
    using System;
    using System.IO;
    using System.Linq;
    using System.Windows;
    #endregion

    /// <summary>
    /// ConfigFile Helpers Class
    /// </summary>
    public static class ConfigFileHelpers
    {
        /// <summary>
        /// Paths for all of the Config Files
        /// </summary>
        #region Filepaths

        /// <summary>
        /// server-config file path
        /// </summary>
        private static string ServerConfigFilePath { get => 
                Environment.CurrentDirectory + "\\ConfigFiles\\ServerConfig.cfg"; }

        #endregion

        /// <summary>
        /// Reads and acts upon the server-config file
        /// </summary>
        public static void ReadServerConfigFile()
        {
            // Validate all of the files first
            ValidateConfigFiles();

            // Load in all lines from the config-file
            string[] Props = File.ReadAllLines(ServerConfigFilePath);

            #region Load in all of the properties from the config-file
            bool.TryParse(Props.FirstOrDefault ( p => p.StartsWith("listen_on_server")).Split(':')[1], out bool Listen);
            uint.TryParse(Props.FirstOrDefault ( p => p.StartsWith("server_port")).Split(':')[1],      out uint port);
            int.TryParse(Props.FirstOrDefault  ( p => p.StartsWith("buffer_size")).Split(':')[1],      out int  buffer_size);
            string ip = Props.FirstOrDefault   ( p => p.StartsWith("server_ip")).Split(':')[1];
            #endregion

            // Create the server connection
            ProgramState.ServerConnection = new ServerConnection(Listen, buffer_size);
            ProgramState.ServerConnection.CreateEndPoint(ip, port);
        }

        /// <summary>
        /// Validates all of the config-files
        /// creates missing or corrupted ones
        /// </summary>
        private static void ValidateConfigFiles()
        {
            // Check if server config file exists
            if(!File.Exists(ServerConfigFilePath))
            {
                // Create the file
                File.Create(ServerConfigFilePath);
                File.AppendAllLines(ServerConfigFilePath, new string[] {

                    /* Add all of the standard properties
                     * to the file after it has been
                     * created.
                     */

                    /// <summary>
                    /// Standard properties and values
                    /// </summary>
                    #region Properties
                    "server_ip:127.0.0.1",
                    "server_port:400",
                    "buffer_size:1024",
                    "listen_on_server:false"
                    #endregion
                });
            }
            // Check if the file is corrupted
            else
            {

            }
        }

    }
}
