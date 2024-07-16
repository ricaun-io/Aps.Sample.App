using System;
using System.Web;
using System.Windows;

namespace Aps.Sample.App
{
    public partial class WebViewLogin : Window
    {
        public string Code { get; set; }
        public WebViewLogin(string url, string title = null)
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(title))
            {
                Title = title;
            }

            if (!string.IsNullOrEmpty(url))
            {
                webView2.Source = new Uri(url);
                webView2.NavigationStarting += WebView2_NavigationStarting;
            }

            this.Closing += (s, e) =>
            {
                if (Code is null)
                {
                    tcs.SetException(new Exception("Code is null"));
                }
            };
        }
        TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
        public Task<string> GetCodeAsync()
        {
            return tcs.Task;
        }

        public Task<string> ShowGetCodeAsync()
        {
            Show();
            return GetCodeAsync();
        }

        private void WebView2_NavigationStarting(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs e)
        {
            var query = e.Uri.Split('?').LastOrDefault();
            var querys = HttpUtility.ParseQueryString(query);
            if (querys.Get("code") is string code)
            {
                Code = code;
                tcs.SetResult(code);
                this.Close();
            }
        }
    }
}