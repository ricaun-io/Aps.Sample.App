﻿using System.Windows;
using Aps.Sample.App.Services;

namespace Aps.Sample.App
{
    public partial class MainWindow : Window
    {
        private static string ClientId = "LtSI0DgPFsVmBLndZSsG8a2pb1unHNJu";
        private static string CallbackUrl = "http://localhost:8080/";

        ApsService ApsService = new ApsService(ClientId, CallbackUrl);

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel(this, ApsService);
        }
    }
}