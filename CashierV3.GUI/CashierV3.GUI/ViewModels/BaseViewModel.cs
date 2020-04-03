using BackendHandler;
using System.ComponentModel;


namespace CashierV3.GUI
{
    /// <summary>
    /// BaseViewModel that holds Commands, Propertychanged and connection to database
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region Connects to database
        public IDatabase rep { get; set; } = Helpers.GetSelectedBackend();
        #endregion

        #region Commands
        public RelayCommand UpdateOrderWhenServed { get; set; }
        public RelayCommand Login { get; set; }
        public RelayCommand Logout { get; set; }
        #endregion

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        #region Methods
        public void OnPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #endregion
    }
}
