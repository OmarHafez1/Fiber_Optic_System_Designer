using System.Diagnostics;

namespace Fiber_Optic_System_Designer
{
    public partial class SettingsPanel : UserControl
    {
        WebBrowser webBrowser;
        public SettingsPanel()
        {
            InitializeComponent();

            webBrowser = new WebBrowser();
            this.Padding = new Padding(15, 15, 0, 0);
            webBrowser.DocumentText = "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n  <style>\r\n    body {\r\n      display: flex;\r\n      justify-content: center;\r\n      align-items: center;\r\n      height: 100vh;\r\n      margin: 0;\r\n      font-family: \"Arial\", sans-serif;\r\n    }\r\n\r\n    .content {\r\n      text-align: center;\r\n    }\r\n  </style>\r\n</head>\r\n<body>\r\n  <div class=\"content\">\r\n    <h2>Settings Page</h2>\r\n    <p><strong>I am working on it and will be updated soon.</strong></p>\r\n    <p>For future updates, please follow this repository:</p>\r\n    <p><a href=\"https://github.com/OmarHafez1/Fiber_Optic_System_Designer\"><strong>https://github.com/OmarHafez1/Fiber_Optic_System_Designer</strong></a></p>\r\n  </div>\r\n</body>\r\n</html>\r\n";
            webBrowser.Navigating += WebBrowser_Navigating;
            webBrowser.Dock = DockStyle.Fill;
            webBrowser.ScrollBarsEnabled = false;
            this.Controls.Add(webBrowser);
        }

        private void WebBrowser_Navigating(object? sender, WebBrowserNavigatingEventArgs e)
        {
            e.Cancel = true;
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = e.Url.ToString(),
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
