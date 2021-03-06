﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace CookingTerminal
{
    public class MainWindowViewModel : BaseViewModel
    {
        /// <summary>
        /// The property to set the current page of the application
        /// </summary>
        public Navigator CurrentPage { get; set; } = Navigator.Login;

        /// <summary>
        /// Static property to access this ViewModel
        /// </summary>
        public static MainWindowViewModel VM { get; set; }

        #region Window Sizes

        /// <summary>
        /// The maximum window height
        /// </summary>
        public static int MaxWindowHeight { get; set; } = 800;

        /// <summary>
        /// The maximum window width
        /// </summary>
        public static int MaxWindowWidth { get; set; } = (MaxWindowHeight * 2);

        /// <summary>
        /// The minimum window height
        /// </summary>
        public static int MinWindowHeight { get; set; } = 600;

        /// <summary>
        /// The minimum window width
        /// </summary>
        public static int MinWindowWidth { get; set; } = (MinWindowHeight * 2);

        public int CurrentHeight { get; set; } = MaxWindowHeight;

        public int CurrentWidth { get; set; } = MaxWindowWidth;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainWindowViewModel()
        {
            // Sets the value of the static property
            VM = this;

            Init();
        }

        #endregion

        /// <summary>
        /// First Init
        /// </summary>
        private async void Init()
        {
            ConfigFileHelpers.ReadServerConfigFile();
            if (!await ProgramState.Server.ConnectAsync())
                MessageBox.Show("Kunde inte etablera anslutning till servern.\nProgrammet körs i offline-läge.");
        }

    }
}
