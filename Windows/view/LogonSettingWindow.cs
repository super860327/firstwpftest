using IDKin.IM.CustomComponents.Controls;
using IDKin.IM.Theme.Helper;
using IDKin.IM.Windows.Model;
using IDKin.IM.Windows.Properties;
using IDKin.IM.Windows.Util;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
namespace IDKin.IM.Windows.View
{
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class LogonSettingWindow : Window//, IComponentConnector
    {
        //internal LogonSettingWindow logonSetting;
        //internal Border OuterBorder;
        //internal Border InnerBorder;
        //internal ImageBrush imgLogonBG;
        //internal Grid RootGrid;
        //internal StatusBar topBar;
        //internal Image imgIcon;
        //internal ImageButton btnClose;
        //internal TextBox tbxServer;
        //internal TextBox tbxPort;
        //internal Button btnSure;
        //internal Button btnCancel;
        //internal Button btnApply;
        //internal CheckBox chkAutoStart;
        //internal CheckBox chkAutoLogon;
        //private bool _contentLoaded;
        public LogonSettingWindow()
        {
            ThemeSwitcher.LoadSkin(ThemeEnum.Aero, this);
            this.InitializeComponent();
            this.InitDataHandler();
        }
        private void InitDataHandler()
        {
            this.tbxServer.Text = Settings.Default.Server;
            this.tbxPort.Text = Settings.Default.Port.ToString();
            this.chkAutoStart.IsChecked = new bool?(Settings.Default.SystemSetup_Base_AutoStart);
            this.chkAutoLogon.IsChecked = new bool?(Settings.Default.SystemSetup_Base_AutoLogin);
        }
        private void logonSetting_Closing(object sender, CancelEventArgs e)
        {
            WindowModel.Instance.LogonSettingWindow = null;
        }
        private void HideHandlerDialog()
        {
            Settings.Default.Save();
            base.Close();
        }
        private void btnSure_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.Server = this.tbxServer.Text;
            Settings.Default.Port = System.Convert.ToInt32(this.tbxPort.Text);
            Settings.Default.SystemSetup_Base_AutoStart = this.chkAutoStart.IsChecked.Value;
            Settings.Default.SystemSetup_Base_AutoLogin = this.chkAutoLogon.IsChecked.Value;
            this.HideHandlerDialog();
        }
        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            this.tbxServer.Text = "192.168.10.230";
            this.tbxPort.Text = "6200";
            Settings.Default.Server = this.tbxServer.Text;
            Settings.Default.Port = System.Convert.ToInt32(this.tbxPort.Text);
            Settings.Default.SystemSetup_Base_AutoStart = this.chkAutoStart.IsChecked.Value;
            Settings.Default.SystemSetup_Base_AutoLogin = this.chkAutoLogon.IsChecked.Value;
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.HideHandlerDialog();
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.MakeSure();
        }
        private void logonSetting_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.HideHandlerDialog();
            }
            if (e.Key == Key.Return)
            {
                this.MakeSure();
            }
        }
        private void MakeSure()
        {
            if (this.tbxServer.Text.Length == 0)
            {
                Settings.Default.Server = "192.168.10.230";
            }
            else
            {
                Settings.Default.Server = this.tbxServer.Text;
            }
            if (this.tbxPort.Text.Length == 0)
            {
                Settings.Default.Port = System.Convert.ToInt32("6200");
            }
            else
            {
                Settings.Default.Port = int.Parse(this.tbxPort.Text);
            }
            Settings.Default.SystemSetup_Base_AutoStart = this.chkAutoStart.IsChecked.Value;
            Settings.Default.SystemSetup_Base_AutoLogin = this.chkAutoLogon.IsChecked.Value;
            this.HideHandlerDialog();
            LogonWindow logonWindow = ServiceUtil.Instance.DataService.LoginWindow as LogonWindow;
            logonWindow.LogonPanel.btnLogon.Focus();
        }
    }
}
