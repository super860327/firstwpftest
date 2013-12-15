using IDKin.IM.Core;
using IDKin.IM.CustomComponents.Controls;
using IDKin.IM.Data;
using IDKin.IM.ImageService;
using IDKin.IM.Log;
using IDKin.IM.Windows.Model;
using IDKin.IM.Windows.Properties;
using IDKin.IM.Windows.Util;
using IDKin.IM.Windows.View;
using IDKin.IM.Windows.ViewModel;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
namespace IDKin.IM.Windows.Comps
{
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class LogonPanel : UserControl
    {
        public delegate void LogOnMsgDelegate(string message);
        public delegate void LogonButtonTimerDelegate();
        public delegate void AutoLogOnDelegate();
        private LogonWindow logonWindow = null;
        private ISessionService sessionService = null;
        private IDataService dataService = null;
        private IImageService imageService = null;
        private LogonPanelViewModel viewModel = null;
        private ILogger logger = null;
        private System.Timers.Timer reLogOnTimer;
        private System.Timers.Timer logonButtonEnableTimer = null;
        private int logonButtonEnableCount = 5;
        //internal Grid LoginPanal;
        //internal StackPanel logonForm;
        //internal Border headBG;
        //internal ImageBrush userHead;
        //internal PromptingTextBox tbUserName;
        //internal PromptingPasswordBox tbPassword;
        //internal CheckBox chkRememberPass;
        //internal StatusButton btnStatus;
        //internal ImageButton btnLogon;
        //internal TextBlock tbLogon;
        //internal TextBlock tbMsg;
        //internal Grid gifGrid;
        ////private bool _contentLoaded;
        public System.Timers.Timer LogonButtonEnableTimer
        {
            set
            {
                this.logonButtonEnableTimer = value;
            }
        }
        public LogonPanel()
        {
            this.InitializeComponent();
            this.InitData();
            this.AddEventListenerHandler();
        }
        private void InitData()
        {
            this.dataService = ServiceUtil.Instance.DataService;
            this.sessionService = ServiceUtil.Instance.SessionService;
            this.imageService = ServiceUtil.Instance.ImageService;
            this.logger = ServiceUtil.Instance.Logger;
            this.viewModel = new LogonPanelViewModel();
            this.tbUserName.Text = Settings.Default.Username;
            this.tbPassword.txtPassword.Password = Settings.Default.Password;
            this.chkRememberPass.IsChecked = new bool?(Settings.Default.SavePassword);
            this.InitStatus();
        }
        private void InitStatus()
        {
            this.imageService = ServiceUtil.Instance.ImageService;
            if (this.imageService != null)
            {
                this.btnStatus.IconContent.Source = this.imageService.GetStatusIcon((UserStatus)System.Enum.Parse(typeof(UserStatus), Settings.Default.Status.ToString()));
            }
        }
        private void AddEventListenerHandler()
        {
            this.btnStatus.miOnline.Click += new RoutedEventHandler(this.StatusMenuItem_Click);
            this.btnStatus.miAway.Click += new RoutedEventHandler(this.StatusMenuItem_Click);
            this.btnStatus.miBusy.Click += new RoutedEventHandler(this.StatusMenuItem_Click);
            this.btnStatus.miDoNotDisturb.Click += new RoutedEventHandler(this.StatusMenuItem_Click);
            this.btnStatus.miOut.Click += new RoutedEventHandler(this.StatusMenuItem_Click);
            this.btnStatus.miHide.Click += new RoutedEventHandler(this.StatusMenuItem_Click);
            this.btnStatus.miMeeting.Click += new RoutedEventHandler(this.StatusMenuItem_Click);
            this.btnStatus.miOffline.Click += new RoutedEventHandler(this.StatusMenuItem_Click);
        }
        private void StatusMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MenuItem mi = sender as MenuItem;
                System.Windows.Controls.Image image = mi.Icon as System.Windows.Controls.Image;
                UserStatus status = (UserStatus)System.Enum.Parse(typeof(UserStatus), mi.DataContext.ToString());
                if (image != null)
                {
                    this.btnStatus.IconContent.Source = image.Source;
                }
                Settings.Default.Status = (int)status;
                Settings.Default.Save();
            }
            catch (System.Exception ex)
            {
                this.logger.Error(ex.ToString());
            }
        }
        private void BindingText()
        {
            Binding bind = new Binding();
            bind.Source = this.viewModel.logOnMsg;
            bind.Path = new PropertyPath("Message", new object[0]);
            bind.Mode = BindingMode.TwoWay;
            bind.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            this.tbMsg.SetBinding(TextBlock.TextProperty, bind);
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.viewModel != null)
                {
                    this.BindingText();
                }
            }
            catch (System.Exception ex)
            {
                this.logger.Error(ex.ToString());
            }
        }
        public void btnLogon_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.btnLogon.Visibility == Visibility.Visible)
                {
                    this.StopReLogOnTimer();
                    this.LogOn();
                }
            }
            catch (System.Exception ex)
            {
                this.logger.Error(ex.ToString());
            }
        }
        public void ReLogOnByTime()
        {
            this.reLogOnTimer = new System.Timers.Timer(5000.0);
            this.reLogOnTimer.Elapsed += new ElapsedEventHandler(this.ReLogOnTimerHandler);
            this.reLogOnTimer.AutoReset = true;
            this.reLogOnTimer.Enabled = true;
        }
        private void ReLogOnTimerHandler(object sender, ElapsedEventArgs e)
        {
            try
            {
                base.Dispatcher.BeginInvoke(new LogonPanel.AutoLogOnDelegate(this.AutoLogOnHandler), new object[0]);
            }
            catch (System.Exception ex)
            {
                this.logger.Error(ex.ToString());
            }
        }
        public void StopReLogOnTimer()
        {
            if (this.reLogOnTimer != null)
            {
                this.reLogOnTimer.Stop();
                this.reLogOnTimer.Dispose();
                this.reLogOnTimer.Close();
                this.reLogOnTimer = null;
            }
        }
        public void AutoLogOnHandler()
        {
            try
            {
                this.LogOn();
            }
            catch (System.Exception e)
            {
                this.logger.Error(e.ToString());
            }
        }
        public void LogOn()
        {
            try
            {
                if (this.sessionService != null && !this.sessionService.IsLogin)
                {
                    if (string.IsNullOrEmpty(this.tbPassword.Password) || string.IsNullOrWhiteSpace(this.tbUserName.Text))
                    {
                        this.tbMsg.Text = "用户名密码不可以为空...";
                    }
                    else
                    {
                        if (Settings.Default.Status == 0)
                        {
                            this.tbMsg.Text = "不能以离线状态登陆...";
                        }
                        else
                        {
                            if (this.gifGrid.Visibility != Visibility.Visible)
                            {
                                this.StartAnimation();
                            }
                            this.LogonButtonUnable();
                            this.tbMsg.Text = "正在获取服务器信息...";
                            this.sessionService.UserName = this.tbUserName.Text;
                            this.sessionService.Password = this.tbPassword.Password;
                            if (this.sessionService.UserName.Length > 0 && this.sessionService.Password.Length > 0)
                            {
                                if (this.chkRememberPass.IsChecked.Value)
                                {
                                    Settings.Default.Username = this.sessionService.UserName;
                                    Settings.Default.Password = this.sessionService.Password;
                                    Settings.Default.SavePassword = true;
                                }
                                else
                                {
                                    Settings.Default.Username = "";
                                    Settings.Default.Password = "";
                                    Settings.Default.SavePassword = false;
                                }
                                Settings.Default.Save();
                                System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(this.GetServerInfo), null);
                            }
                            else
                            {
                                base.Dispatcher.BeginInvoke(new LogonPanel.LogOnMsgDelegate(this.LogOnMsgHandle), new object[]
								{
									"请正确输入用户名和密码！"
								});
                                this.LogonButtonEnable();
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                this.logger.Error(ex.ToString());
            }
        }
        private void StartAnimation()
        {
            this.LoginPanal.Visibility = Visibility.Collapsed;
            this.gifGrid.Visibility = Visibility.Visible;
            AnimatedImage image = this.GetAnimatebyIDImage();
            image.Stretch = Stretch.None;
            image.HorizontalAlignment = HorizontalAlignment.Center;
            image.VerticalAlignment = VerticalAlignment.Top;
            image.Margin = new Thickness(0.0, 90.0, 0.0, 0.0);
            this.gifGrid.Children.Add(image);
        }
        public void StopAnimation()
        {
            if (this.gifGrid.Children.Count > 0)
            {
                foreach (object child in this.gifGrid.Children)
                {
                    if (child.GetType() == typeof(AnimatedImage))
                    {
                        (child as AnimatedImage).Dispose();
                    }
                }
                this.gifGrid.Visibility = Visibility.Collapsed;
                this.LoginPanal.Visibility = Visibility.Visible;
            }
        }
        public AnimatedImage GetAnimatebyIDImage()
        {
            AnimatedImage animatedImage = null;
            Bitmap image = IDKin.IM.Windows.Properties.Resources.login;
            if (image != null)
            {
                animatedImage = new AnimatedImage();
                animatedImage.LoadSmile(image);
            }
            return animatedImage;
        }
        public void LogonButtonEnable()
        {
            try
            {
                if (this.logonButtonEnableTimer == null)
                {
                    this.logonButtonEnableTimer = new System.Timers.Timer(1000.0);
                    this.logonButtonEnableTimer.Elapsed += new ElapsedEventHandler(this.LogonButtonTimerTick);
                    this.logonButtonEnableTimer.AutoReset = true;
                    this.logonButtonEnableTimer.Enabled = true;
                }
                this.sessionService.IsLogin = false;
            }
            catch (System.Exception e)
            {
                this.logger.Error(e.ToString());
            }
        }
        private void LogonButtonTimerTick(object sender, ElapsedEventArgs e)
        {
            base.Dispatcher.BeginInvoke(new LogonPanel.LogonButtonTimerDelegate(this.LogonButtonTimerHandle), new object[0]);
        }
        private void LogonButtonTimerHandle()
        {
            if (this.logonButtonEnableCount > 0)
            {
                this.tbLogon.Visibility = Visibility.Visible;
                this.tbLogon.Text = this.logonButtonEnableCount.ToString();
                this.logonButtonEnableCount--;
            }
            else
            {
                this.StopAnimation();
                this.logonButtonEnableCount = 5;
                this.tbLogon.Visibility = Visibility.Collapsed;
                this.btnLogon.Visibility = Visibility.Visible;
                if (this.logonButtonEnableTimer != null)
                {
                    this.logonButtonEnableTimer.Stop();
                    this.logonButtonEnableTimer.Dispose();
                    this.logonButtonEnableTimer.Close();
                    this.logonButtonEnableTimer = null;
                }
            }
        }
        public void LogonButtonUnable()
        {
            this.sessionService.IsLogin = true;
            this.btnLogon.Visibility = Visibility.Collapsed;
        }
        public void GetServerInfo(object obj)
        {
            try
            {
                ServerInfo si = this.viewModel.GetServerInfo(Settings.Default.Server, Settings.Default.Port);
                if (si != null)
                {
                    base.Dispatcher.BeginInvoke(new LogonPanel.LogOnMsgDelegate(this.LogOnMsgHandle), new object[]
					{
						"获取服务器信息成功!正在验证!"
					});
                    if (this.viewModel.ConnectServer(si))
                    {
                        this.InitLogger(si.WebLoggerAddress);
                        this.viewModel.Login(si, this.sessionService.UserName, this.sessionService.Password, Settings.Default.Status);
                    }
                    else
                    {
                        this.LogonButtonEnable();
                        base.Dispatcher.BeginInvoke(new LogonPanel.LogOnMsgDelegate(this.LogOnMsgHandle), new object[]
						{
							"连接服务器失败!"
						});
                    }
                }
                else
                {
                    this.LogonButtonEnable();
                    base.Dispatcher.BeginInvoke(new LogonPanel.LogOnMsgDelegate(this.LogOnMsgHandle), new object[]
					{
						"获取服务器信息失败!"
					});
                }
            }
            catch (System.Exception e)
            {
                this.logger.Error(e.ToString());
            }
        }
        private void InitLogger(string WebLoggerAddress)
        {
            Logger.IMVersion = AppUtil.Instance.AppVersion();
            Logger.IP = AppUtil.Instance.LocalIP();
            Logger.PCVersion = System.Environment.OSVersion.VersionString;
            Logger.WebLoggerAddress = WebLoggerAddress;
        }
        private void LogOnMsgHandle(string message)
        {
            this.tbMsg.Text = message;
        }
        private void LogonSettingHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                LogonSettingWindow logonSettingWindow = WindowModel.Instance.LogonSettingWindow;
                this.logonWindow = (this.dataService.LoginWindow as LogonWindow);
                if (logonSettingWindow != null && this.logonWindow != null && this.logonWindow.IsActive)
                {
                    logonSettingWindow.Owner = this.logonWindow;
                    logonSettingWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    logonSettingWindow.tbxServer.Focus();
                    DoubleAnimation animate = new DoubleAnimation();
                    animate.FillBehavior = FillBehavior.HoldEnd;
                    animate.From = new double?(0.0);
                    animate.To = new double?(0.5);
                    animate.Duration = new Duration(System.TimeSpan.FromSeconds(0.8));
                    this.logonWindow.RootBorder.Visibility = Visibility.Visible;
                    this.logonWindow.RootBorder.Background.BeginAnimation(System.Windows.Media.Brush.OpacityProperty, animate);
                    logonSettingWindow.ShowDialog();
                    logonSettingWindow.Activate();
                    this.logonWindow.RootBorder.Visibility = Visibility.Collapsed;
                }
            }
            catch (System.Exception ex)
            {
                this.logger.Error(ex.ToString());
            }
        }
    }
}
