namespace CustomerTerminalWPF
{
    /// <summary>
    /// All the different menus
    /// </summary>
    public enum Enums
    {
        /// <summary>
        /// Object Types
        /// </summary>
        #region Types
        Pizza,
        Drink,
        Sallad,
        Pasta,
        Extra,
        #endregion

        /// <summary>
        /// Ingredient Types
        /// </summary>
        #region Types
        PizzaIngredient,
        PastaIngredient,
        SalladIngredient
        #endregion
    }

    /// <summary>
    /// Main pages
    /// </summary>
    public enum MainPages
    {
        /// <summary>
        /// First page
        /// </summary>
        WellcomeScreen,

        /// <summary>
        /// Main page
        /// </summary>
        OrderScreen
    }

    /// <summary>
    /// All of the sub pages for the order page
    /// </summary>
    public enum OrderSubPages
    {
        /// <summary>
        /// Show no page
        /// </summary>
        None,

        /// <summary>
        /// Show the payment window inside the order window
        /// </summary>
        PaymentPage,

        /// <summary>
        /// Edit an object in a seperate page
        /// </summary>
        EditObjectPage,
    }

    /// <summary>
    /// All of the different payment statuses
    /// </summary>
    public enum PaymentStatueses
    {
        /// <summary>
        /// Waiting for payment, show loading screen
        /// </summary>
        WaitingForPayment,

        /// <summary>
        /// Payment was accepted, show payment complete screen
        /// </summary>
        PaymentOK,

        /// <summary>
        /// Payment was not accepted
        /// </summary>
        PaymentNotOK
    }

    /// <summary>
    /// All of the different connectionstates
    /// </summary>
    public enum ConnectionStates
    {
        /// <summary>
        /// Disconntected from the server
        /// </summary>
        Disconnected,

        /// <summary>
        /// Connecting to the server
        /// </summary>
        Connecting,

        /// <summary>
        /// Connected to the server
        /// </summary>
        Connected
    }
}
