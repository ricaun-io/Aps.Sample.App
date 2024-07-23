using Aps.Sample.App.Services;
using Aps.Sample.App.ViewModels;
using Aps.Sample.App.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;

namespace Aps.Sample.App
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly Window window;
        private readonly ApsService ApsService;

        public MainViewModel(Window window, ApsService apsService)
        {
            this.window = window;
            ApsService = apsService;
            User.Name = "Login";

            if (apsService.IsLoggedIn())
            {
                var task = Task.Run(async () =>
                {
                    var userInfo = await ApsService.GetUserInfoAsync();
                    User.Name = userInfo.Name;
                    User.Image = userInfo.Picture;
                });
            }

        }

        public User User { get; set; } = new User();

        [RelayCommand]
        public async Task Login()
        {
            if (User.Image is not null)
            {
                User.Image = null;
                User.Name = "Login";
                await ApsService.Logout();
                return;
            }

            var view = new WebViewLogin(ApsService.Authorize(), ApsService.callbackUri, "Login with Autodesk Account.");
            view.Owner = window;
            try
            {
                var code = await view.ShowGetCodeAsync();
                await ApsService.GetPKCEThreeLeggedTokenAsync(code);
                var userInfo = await ApsService.GetUserInfoAsync();
                User.Name = userInfo.Name;
                User.Image = userInfo.Picture;
            }
            catch { }
        }
    }
}