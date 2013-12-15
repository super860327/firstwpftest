using IDKin.IM.Communicate;
using IDKin.IM.CustomComponents.Controls;
using IDKin.IM.Data;
using IDKin.IM.ImageService;
using IDKin.IM.Log;
using IDKin.IM.Theme.Helper;
using IDKin.IM.Windows.Comps;
using IDKin.IM.Windows.Model;
using IDKin.IM.Windows.Properties;
using IDKin.IM.Windows.Util;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
namespace IDKin.IM.Windows.View
{
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0"), Export("IDKin.IM.Windows.View.LogonWindow", typeof(Window))]
    public partial class LogonWindow : BaseWindow
    {
        private IDataService dataService = null;
        private ISessionService sessionService = null;
        private IWSClient wsClient = null;
        private ILogger logger = null;
        private IConnection connection = null;
        private IImageService imageService = null;

        public LogonWindow()
        {
            try
            {
                ThemeSwitcher.LoadSkin(ThemeEnum.Aero, this);
                this.InitializeComponent();
                this.AddEventListenerHandler();
            }
            catch (System.Exception e)
            {
                if (this.logger != null)
                {
                    this.logger.Error(e.ToString());
                }
            }
        }
        public void AddMessage(string message)
        {
            try
            {
                if (this.LogonPanel != null)
                {
                    this.LogonPanel.tbMsg.Text = message;
                }
            }
            catch (System.Exception e)
            {
                if (this.logger != null)
                {
                    this.logger.Error(e.ToString());
                }
            }
        }
        private void AddEventListenerHandler()
        {
            base.KeyDown += new KeyEventHandler(this.LogonWindow_KeyDown);
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.connection = ServiceUtil.Instance.Connection;
                this.dataService = ServiceUtil.Instance.DataService;
                this.logger = ServiceUtil.Instance.Logger;
                this.sessionService = ServiceUtil.Instance.SessionService;
                this.wsClient = ServiceUtil.Instance.WsClient;
                this.imageService = ServiceUtil.Instance.ImageService;
                if (this.sessionService != null)
                {
                    this.sessionService.IsReLogOn = false;
                }
                if (Settings.Default.SystemSetup_FileTransport_SaveDir.Equals(""))
                {
                    Settings.Default.SystemSetup_FileTransport_SaveDir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "\\";
                    Settings.Default.Save();
                }
            }
            catch (System.Exception ex)
            {
                this.logger.Error(ex.ToString());
            }
        }
        private void LogonWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (this.LogonPanel.btnLogon.IsEnabled && this.LogonPanel.btnLogon.Visibility == Visibility.Visible)
                {
                    this.LogonPanel.btnLogon_Click(sender, e);
                }
            }
        }
        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            base.Hide();
        }
        private void btnMax_Click(object sender, RoutedEventArgs e)
        {
            this.WindowMaxHandler();
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            base.Close();
            Application.Current.Shutdown(1);
        }
        private void topBar_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.WindowMaxHandler();
        }
        private void WindowMaxHandler()
        {
            if (base.WindowState == WindowState.Normal)
            {
                base.WindowState = WindowState.Maximized;
                this.btnMax.NormalImage = "/IDKin.IM.Windows;component/Resources/Image/restore.png";
                this.btnMax.HoverImage = "/IDKin.IM.Windows;component/Resources/Image/restoreOver.png";
                this.btnMax.PressedImage = "/IDKin.IM.Windows;component/Resources/Image/restorePress.png";
            }
            else
            {
                base.WindowState = WindowState.Normal;
                this.btnMax.NormalImage = "/IDKin.IM.Windows;component/Resources/Image/max.png";
                this.btnMax.HoverImage = "/IDKin.IM.Windows;component/Resources/Image/maxOver.png";
                this.btnMax.PressedImage = "/IDKin.IM.Windows;component/Resources/Image/maxPress.png";
            }
        }
        private void BaseWindow_Closing(object sender, CancelEventArgs e)
        {
            this.LogonPanel.StopReLogOnTimer();
            if (WindowModel.Instance.LogonSettingWindow != null)
            {
                WindowModel.Instance.LogonSettingWindow.Close();
            }
        }
    }
}
