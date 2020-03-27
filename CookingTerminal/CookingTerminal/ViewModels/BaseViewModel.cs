﻿using BackendHandler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CookingTerminal
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region PropertyChanged
        /// <summary>
        /// The event to fire when a child-property changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        /// <summary>
        /// Method to shorten the call for a property change
        /// </summary>
        /// <param name="name"></param>
        public void OnPropertyChanged(string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        #region Public Properties

        /// <summary>
        /// The repository to use
        /// </summary>
        public IDatabase rep { get; set; } = Helpers.GetSelectedBackend();

        /// <summary>
        /// The selected order to cook
        /// </summary>
        public static OrderViewModel OrderToCook { get; set; }

        #endregion

    }
}
