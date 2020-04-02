namespace TerminalServer
{
    /// <summary>
    /// All of the terminals that the server
    /// should handle
    /// </summary>
    public enum Terminals
    {
        CustomerTerminal = 0,
        AdminTerminal    = 1,
        CashierTerminal  = 2,
        ChefTerminal     = 3,
        InfoTerminal     = 4
    }

    /// <summary>
    /// All of the different server states
    /// </summary>
    public enum ServerStates
    {
        Offline = 0,
        Online  = 1
    }
}
