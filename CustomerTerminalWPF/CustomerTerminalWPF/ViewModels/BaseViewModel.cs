namespace CustomerTerminalWPF
{
    /// <summary>
    /// Required namespaces
    /// </summary>
    #region Namespaces
    using System.ComponentModel;
    #endregion

    /// <summary>
    /// A base ViewModel for all other ViewModel's to inherit from
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Fires when a property has changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        /// <summary>
        /// If the property needs to be fired manually
        /// </summary>
        /// <param name="VM"></param>
        /// <param name="Name"></param>
        protected void OnPropertyChanged(object VM, string Name) => PropertyChanged?.Invoke(VM, new PropertyChangedEventArgs(Name));
    }
}