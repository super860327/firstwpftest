using CSharpWin;
using IDKin.IM.Communicate;
using IDKin.IM.Core;
using IDKin.IM.Core.Core;
using IDKin.IM.CustomComponents.Controls;
using IDKin.IM.Data;
using IDKin.IM.ImageService;
using IDKin.IM.Log;
using IDKin.IM.Protocol.Center;
using IDKin.IM.Windows.Comps;
using IDKin.IM.Windows.Comps.ChatTab;
using IDKin.IM.Windows.Model;
using IDKin.IM.Windows.Properties;
using IDKin.IM.Windows.Util;
using IDKin.IM.Windows.View.AddFriends;
using IDKin.IM.Windows.ViewModel;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
namespace IDKin.IM.Windows.View
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0"), Export("IDKin.IM.Windows.View.SystemWindow", typeof(Window))]
    public partial class SystemWindow : Window//, IComponentConnector
	{
		[Import]
		private IDataService dataService = null;
		[Import]
		private ISessionService sessionService = null;
		[Import]
		private IWSClient wsClient = null;
		[Import]
		private ILogger logger = null;
		[Import]
		private IConnection connection = null;
		[Import]
		private IImageService imageService = null;
		[Import]
		private IFileService fileService = null;
		[Import]
		private IUtilService utilService = null;
		private LogonWindow logonWindow = null;
		public NotifyIcon NotifyIcon = null;
		private System.Windows.Forms.ContextMenu notificationMenu = null;
		private System.Windows.Forms.MenuItem separator1 = null;
		private System.Windows.Forms.MenuItem separator2 = null;
		private System.Windows.Forms.MenuItem separator3 = null;
		public System.Windows.Forms.MenuItem openItem = null;
		public System.Windows.Forms.MenuItem optionItem = null;
		private System.Windows.Forms.MenuItem updateItem = null;
		public System.Windows.Forms.MenuItem aboutItem = null;
		public System.Windows.Forms.MenuItem logoutItem = null;
		public System.Windows.Forms.MenuItem exitItem = null;
        //private bool _contentLoaded;
		public SystemWindow()
		{
			this.InitializeComponent();
		}
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			this.InitTray();
			this.InitEventHandler();
			ServiceUtil.Instance.Connection = this.connection;
			ServiceUtil.Instance.DataService = this.dataService;
			ServiceUtil.Instance.ImageService = this.imageService;
			ServiceUtil.Instance.Logger = this.logger;
			ServiceUtil.Instance.SessionService = this.sessionService;
			ServiceUtil.Instance.WsClient = this.wsClient;
			ServiceUtil.Instance.FileService = this.fileService;
			ServiceUtil.Instance.utilService = this.utilService;
			AppUtil.Instance.StartAtLogon(AppUtil.FilePath, Settings.Default.SystemSetup_Base_AutoStart);
			this.logonWindow = new LogonWindow();
			this.logonWindow.Show();
			this.dataService.LoginWindow = this.logonWindow;
			if (Settings.Default.SystemSetup_Base_AutoLogin)
			{
				if (!string.IsNullOrEmpty(this.logonWindow.LogonPanel.tbUserName.Text) && !string.IsNullOrEmpty(this.logonWindow.LogonPanel.tbPassword.Password))
				{
					this.logonWindow.LogonPanel.LogOn();
				}
			}
			this.InitAppHotKey();
		}
		private void InitEventHandler()
		{
			System.Windows.Application.Current.Exit += new ExitEventHandler(this.App_Exit);
			System.Windows.Application.Current.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(this.Current_DispatcherUnhandledException);
		}
		private void InitAppHotKey()
		{
			DataModel.Instance.PickUpMsgKey = Settings.Default.PickupMsg;
			DataModel.Instance.PickUpMsgType = Settings.Default.PickUpMsgType;
			DataModel.Instance.PickUpMsgCAS = Settings.Default.PickUpMsgCAS;
			DataModel.Instance.CutScreenKey = Settings.Default.CutScreen;
			DataModel.Instance.CutScreenType = Settings.Default.CutScreenType;
			DataModel.Instance.CutScreenCAS = Settings.Default.CutScreenCAS;
		}
		private void Current_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			this.logger.Error(e.Exception.ToString());
			System.Windows.MessageBox.Show("应用程序遇到问题，请重启应用程序!", "提示");
		}
		public void OnHookKeyDownHandler(object sender, HookEventArgs e)
		{
			bool singleCutScreen = e.Key == DataModel.Instance.CutScreenKey;
			bool ctrlCutScreen = (Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.None && e.Key == DataModel.Instance.CutScreenKey;
			bool altCutScreen = (Keyboard.Modifiers & ModifierKeys.Alt) != ModifierKeys.None && e.Key == DataModel.Instance.CutScreenKey;
			bool shiftCutScreen = (Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.None && e.Key == DataModel.Instance.CutScreenKey;
			bool ctrlAltCutScreen = (Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.None && (Keyboard.Modifiers & ModifierKeys.Alt) != ModifierKeys.None && e.Key == DataModel.Instance.CutScreenKey;
			bool ctrlShiftCutScreen = (Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.None && (Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.None && e.Key == DataModel.Instance.CutScreenKey;
			bool altShiftCutScreen = (Keyboard.Modifiers & ModifierKeys.Alt) != ModifierKeys.None && (Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.None && e.Key == DataModel.Instance.CutScreenKey;
			bool singlePickupMsg = e.Key == DataModel.Instance.PickUpMsgKey;
			bool ctrlPickupMsg = (Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.None && e.Key == DataModel.Instance.PickUpMsgKey;
			bool altPickupMsg = (Keyboard.Modifiers & ModifierKeys.Alt) != ModifierKeys.None && e.Key == DataModel.Instance.PickUpMsgKey;
			bool shiftPickupMsg = (Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.None && e.Key == DataModel.Instance.PickUpMsgKey;
			bool ctrlAltPickupMsg = (Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.None && (Keyboard.Modifiers & ModifierKeys.Alt) != ModifierKeys.None && e.Key == DataModel.Instance.PickUpMsgKey;
			bool ctrlShiftPickupMsg = (Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.None && (Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.None && e.Key == DataModel.Instance.PickUpMsgKey;
			bool altShiftPickupMsg = (Keyboard.Modifiers & ModifierKeys.Alt) != ModifierKeys.None && (Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.None && e.Key == DataModel.Instance.PickUpMsgKey;
			try
			{
				if (DataModel.Instance.IsSetHotKey)
				{
					if (WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Focus())
					{
						if (e.Key == Keys.F1 || e.Key == Keys.F2 || e.Key == Keys.F3 || e.Key == Keys.F4 || e.Key == Keys.F5 || e.Key == Keys.F6 || e.Key == Keys.F7 || e.Key == Keys.F8 || e.Key == Keys.F9 || e.Key == Keys.F10 || e.Key == Keys.F11 || e.Key == Keys.F12)
						{
							if (e.Key == DataModel.Instance.PickUpMsgKey)
							{
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Text = "";
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.KeyUp += delegate
								{
									WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Text = "无";
								};
							}
							else
							{
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Text = e.Key.ToString();
								Settings.Default.CutScreen = e.Key;
								Settings.Default.CutScreenString = e.Key.ToString();
								Settings.Default.CutScreenType = 1;
								DataModel.Instance.CutScreenKey = Settings.Default.CutScreen;
								DataModel.Instance.CutScreenType = Settings.Default.CutScreenType;
								Settings.Default.Save();
							}
						}
						if (e.Key == Keys.Q || e.Key == Keys.W || e.Key == Keys.E || e.Key == Keys.R || e.Key == Keys.T || e.Key == Keys.Y || e.Key == Keys.U || e.Key == Keys.I || e.Key == Keys.O || e.Key == Keys.P || e.Key == Keys.A || e.Key == Keys.S || e.Key == Keys.D || e.Key == Keys.F || e.Key == Keys.G || e.Key == Keys.H || e.Key == Keys.J || e.Key == Keys.K || e.Key == Keys.L || e.Key == Keys.Z || e.Key == Keys.X || e.Key == Keys.C || e.Key == Keys.V || e.Key == Keys.B || e.Key == Keys.N || e.Key == Keys.M)
						{
							if (e.Key == Keys.LControlKey || e.Key == Keys.LMenu || e.Key == Keys.LWin || e.Key == Keys.RControlKey || e.Key == Keys.RMenu || e.Key == Keys.RWin || e.Key == DataModel.Instance.PickUpMsgKey)
							{
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Text = "Ctrl + Alt + ";
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.KeyUp += delegate
								{
									WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Text = "无";
								};
							}
							else
							{
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Text = "Ctrl + Alt + " + e.Key.ToString();
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.KeyUp += delegate
								{
									WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Text = "Ctrl + Alt + " + e.Key.ToString();
								};
								Settings.Default.CutScreen = e.Key;
								Settings.Default.CutScreenString = "Ctrl + Alt + " + e.Key.ToString();
								Settings.Default.CutScreenType = 3;
								Settings.Default.CutScreenCAS = 1;
								DataModel.Instance.CutScreenKey = Settings.Default.CutScreen;
								DataModel.Instance.CutScreenType = Settings.Default.CutScreenType;
								DataModel.Instance.CutScreenCAS = Settings.Default.CutScreenCAS;
								Settings.Default.Save();
							}
						}
						if (e.Key == Keys.D0 || e.Key == Keys.D1 || e.Key == Keys.D2 || e.Key == Keys.D3 || e.Key == Keys.D4 || e.Key == Keys.D5 || e.Key == Keys.D6 || e.Key == Keys.D7 || e.Key == Keys.D8 || e.Key == Keys.D9)
						{
							if (e.Key == DataModel.Instance.PickUpMsgKey)
							{
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Text = "";
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.KeyUp += delegate
								{
									WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Text = "无";
								};
							}
							else
							{
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Text = e.Key.ToString().Substring(1, 1);
								Settings.Default.CutScreen = e.Key;
								Settings.Default.CutScreenString = e.Key.ToString().Substring(1, 1);
								Settings.Default.CutScreenType = 1;
								DataModel.Instance.CutScreenKey = Settings.Default.CutScreen;
								DataModel.Instance.CutScreenType = Settings.Default.CutScreenType;
								Settings.Default.Save();
							}
						}
						if (e.Key == Keys.NumPad0 || e.Key == Keys.NumPad1 || e.Key == Keys.NumPad2 || e.Key == Keys.NumPad3 || e.Key == Keys.NumPad4 || e.Key == Keys.NumPad5 || e.Key == Keys.NumPad6 || e.Key == Keys.NumPad7 || e.Key == Keys.NumPad8 || e.Key == Keys.NumPad9)
						{
							if (e.Key == DataModel.Instance.PickUpMsgKey)
							{
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Text = "";
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.KeyUp += delegate
								{
									WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Text = "无";
								};
							}
							else
							{
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Text = e.Key.ToString();
								Settings.Default.CutScreen = e.Key;
								Settings.Default.CutScreenString = e.Key.ToString();
								Settings.Default.CutScreenType = 1;
								DataModel.Instance.CutScreenKey = Settings.Default.CutScreen;
								DataModel.Instance.CutScreenType = Settings.Default.CutScreenType;
								Settings.Default.Save();
							}
						}
						if (e.Key == Keys.Home || e.Key == Keys.End || e.Key == Keys.Insert || e.Key == Keys.Prior || e.Key == Keys.Next)
						{
							if (e.Key == DataModel.Instance.PickUpMsgKey)
							{
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Text = "";
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.KeyUp += delegate
								{
									WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Text = "无";
								};
							}
							else
							{
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Text = e.Key.ToString();
								Settings.Default.CutScreen = e.Key;
								Settings.Default.CutScreenString = e.Key.ToString();
								Settings.Default.CutScreenType = 1;
								DataModel.Instance.CutScreenKey = Settings.Default.CutScreen;
								DataModel.Instance.CutScreenType = Settings.Default.CutScreenType;
								Settings.Default.Save();
							}
						}
						if ((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.None)
						{
							if (e.Key == Keys.LControlKey || e.Key == Keys.LMenu || e.Key == Keys.LWin || e.Key == Keys.RControlKey || e.Key == Keys.RMenu || e.Key == Keys.RWin || e.Key == DataModel.Instance.PickUpMsgKey)
							{
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Text = "Ctrl + ";
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.KeyUp += delegate
								{
									WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Text = "无";
								};
							}
							else
							{
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Text = "Ctrl + " + e.Key.ToString();
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.KeyUp += delegate
								{
									WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Text = "Ctrl + " + e.Key.ToString();
								};
								Settings.Default.CutScreen = e.Key;
								Settings.Default.CutScreenString = "Ctrl + " + e.Key.ToString();
								Settings.Default.CutScreenType = 2;
								DataModel.Instance.CutScreenKey = Settings.Default.CutScreen;
								DataModel.Instance.CutScreenType = Settings.Default.CutScreenType;
								Settings.Default.Save();
							}
						}
						if ((Keyboard.Modifiers & ModifierKeys.Alt) != ModifierKeys.None)
						{
							if (e.Key == Keys.LControlKey || e.Key == Keys.LMenu || e.Key == Keys.LWin || e.Key == Keys.RControlKey || e.Key == Keys.RMenu || e.Key == Keys.RWin || e.Key == DataModel.Instance.PickUpMsgKey)
							{
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Text = "Alt + ";
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.KeyUp += delegate
								{
									WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Text = "无";
								};
							}
							else
							{
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Text = "Alt + " + e.Key.ToString();
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.KeyUp += delegate
								{
									WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Text = "Alt + " + e.Key.ToString();
								};
								Settings.Default.CutScreen = e.Key;
								Settings.Default.CutScreenString = "Alt + " + e.Key.ToString();
								Settings.Default.CutScreenType = 2;
								DataModel.Instance.CutScreenKey = Settings.Default.CutScreen;
								DataModel.Instance.CutScreenType = Settings.Default.CutScreenType;
								Settings.Default.Save();
							}
						}
						if ((Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.None)
						{
							if (e.Key == Keys.LControlKey || e.Key == Keys.LMenu || e.Key == Keys.LWin || e.Key == Keys.RControlKey || e.Key == Keys.RMenu || e.Key == Keys.RWin || e.Key == DataModel.Instance.PickUpMsgKey)
							{
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Text = "Shift + ";
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.KeyUp += delegate
								{
									WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Text = "无";
								};
							}
							else
							{
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Text = "Shift + " + e.Key.ToString();
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.KeyUp += delegate
								{
									WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Text = "Shift + " + e.Key.ToString();
								};
								Settings.Default.CutScreen = e.Key;
								Settings.Default.CutScreenString = "Shift + " + e.Key.ToString();
								Settings.Default.CutScreenType = 2;
								DataModel.Instance.CutScreenKey = Settings.Default.CutScreen;
								DataModel.Instance.CutScreenType = Settings.Default.CutScreenType;
								Settings.Default.Save();
							}
						}
						if ((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.None && (Keyboard.Modifiers & ModifierKeys.Alt) != ModifierKeys.None)
						{
							if (e.Key == Keys.LControlKey || e.Key == Keys.LMenu || e.Key == Keys.LWin || e.Key == Keys.LShiftKey || e.Key == Keys.RControlKey || e.Key == Keys.RMenu || e.Key == Keys.RWin || e.Key == Keys.RShiftKey || e.Key == DataModel.Instance.PickUpMsgKey)
							{
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Text = "Ctrl + Alt + ";
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.KeyUp += delegate
								{
									WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Text = "无";
								};
							}
							else
							{
								if (e.Key == Keys.D0 || e.Key == Keys.D1 || e.Key == Keys.D2 || e.Key == Keys.D3 || e.Key == Keys.D4 || e.Key == Keys.D5 || e.Key == Keys.D6 || e.Key == Keys.D7 || e.Key == Keys.D8 || e.Key == Keys.D9)
								{
									WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Text = "Ctrl + Alt + " + e.Key.ToString().Substring(1, 1);
								}
								else
								{
									WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Text = "Ctrl + Alt + " + e.Key.ToString();
								}
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.KeyUp += delegate
								{
									if (e.Key == Keys.D0 || e.Key == Keys.D1 || e.Key == Keys.D2 || e.Key == Keys.D3 || e.Key == Keys.D4 || e.Key == Keys.D5 || e.Key == Keys.D6 || e.Key == Keys.D7 || e.Key == Keys.D8 || e.Key == Keys.D9)
									{
										WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Text = "Ctrl + Alt + " + e.Key.ToString().Substring(1, 1);
									}
									else
									{
										WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Text = "Ctrl + Alt + " + e.Key.ToString();
									}
								};
								Settings.Default.CutScreen = e.Key;
								Settings.Default.CutScreenString = WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Text;
								Settings.Default.CutScreenType = 3;
								Settings.Default.CutScreenCAS = 1;
								DataModel.Instance.CutScreenKey = Settings.Default.CutScreen;
								DataModel.Instance.CutScreenType = Settings.Default.CutScreenType;
								DataModel.Instance.CutScreenCAS = Settings.Default.CutScreenCAS;
								Settings.Default.Save();
							}
						}
						if ((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.None && (Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.None)
						{
							if (e.Key == Keys.LControlKey || e.Key == Keys.LMenu || e.Key == Keys.LWin || e.Key == Keys.LShiftKey || e.Key == Keys.RControlKey || e.Key == Keys.RMenu || e.Key == Keys.RWin || e.Key == Keys.RShiftKey || e.Key == DataModel.Instance.PickUpMsgKey)
							{
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Text = "Ctrl + Shift + ";
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.KeyUp += delegate
								{
									WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Text = "无";
								};
							}
							else
							{
								if (e.Key == Keys.D0 || e.Key == Keys.D1 || e.Key == Keys.D2 || e.Key == Keys.D3 || e.Key == Keys.D4 || e.Key == Keys.D5 || e.Key == Keys.D6 || e.Key == Keys.D7 || e.Key == Keys.D8 || e.Key == Keys.D9)
								{
									WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Text = "Ctrl + Shift + " + e.Key.ToString().Substring(1, 1);
								}
								else
								{
									WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Text = "Ctrl + Shift + " + e.Key.ToString();
								}
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.KeyUp += delegate
								{
									if (e.Key == Keys.D0 || e.Key == Keys.D1 || e.Key == Keys.D2 || e.Key == Keys.D3 || e.Key == Keys.D4 || e.Key == Keys.D5 || e.Key == Keys.D6 || e.Key == Keys.D7 || e.Key == Keys.D8 || e.Key == Keys.D9)
									{
										WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Text = "Ctrl + Shift + " + e.Key.ToString().Substring(1, 1);
									}
									else
									{
										WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Text = "Ctrl + Shift + " + e.Key.ToString();
									}
								};
								Settings.Default.CutScreen = e.Key;
								Settings.Default.CutScreenString = WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Text;
								Settings.Default.CutScreenType = 3;
								Settings.Default.CutScreenCAS = 2;
								DataModel.Instance.CutScreenKey = Settings.Default.CutScreen;
								DataModel.Instance.CutScreenType = Settings.Default.CutScreenType;
								DataModel.Instance.CutScreenCAS = Settings.Default.CutScreenCAS;
								Settings.Default.Save();
							}
						}
						if ((Keyboard.Modifiers & ModifierKeys.Alt) != ModifierKeys.None && (Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.None)
						{
							if (e.Key == Keys.LControlKey || e.Key == Keys.LMenu || e.Key == Keys.LWin || e.Key == Keys.LShiftKey || e.Key == Keys.RControlKey || e.Key == Keys.RMenu || e.Key == Keys.RWin || e.Key == Keys.RShiftKey || e.Key == DataModel.Instance.PickUpMsgKey)
							{
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Text = "Alt + Shift + ";
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.KeyUp += delegate
								{
									WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Text = "无";
								};
							}
							else
							{
								if (e.Key == Keys.D0 || e.Key == Keys.D1 || e.Key == Keys.D2 || e.Key == Keys.D3 || e.Key == Keys.D4 || e.Key == Keys.D5 || e.Key == Keys.D6 || e.Key == Keys.D7 || e.Key == Keys.D8 || e.Key == Keys.D9)
								{
									WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Text = "Alt + Shift + " + e.Key.ToString().Substring(1, 1);
								}
								else
								{
									WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Text = "Alt + Shift + " + e.Key.ToString();
								}
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.KeyUp += delegate
								{
									if (e.Key == Keys.D0 || e.Key == Keys.D1 || e.Key == Keys.D2 || e.Key == Keys.D3 || e.Key == Keys.D4 || e.Key == Keys.D5 || e.Key == Keys.D6 || e.Key == Keys.D7 || e.Key == Keys.D8 || e.Key == Keys.D9)
									{
										WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Text = "Alt + Shift + " + e.Key.ToString().Substring(1, 1);
									}
									else
									{
										WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Text = "Alt + Shift + " + e.Key.ToString();
									}
								};
								Settings.Default.CutScreen = e.Key;
								Settings.Default.CutScreenString = WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxCut.Text;
								Settings.Default.CutScreenType = 3;
								Settings.Default.CutScreenCAS = 3;
								DataModel.Instance.CutScreenKey = Settings.Default.CutScreen;
								DataModel.Instance.CutScreenType = Settings.Default.CutScreenType;
								DataModel.Instance.CutScreenCAS = Settings.Default.CutScreenCAS;
								Settings.Default.Save();
							}
						}
					}
					if (WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Focus())
					{
						if (e.Key == Keys.F1 || e.Key == Keys.F2 || e.Key == Keys.F3 || e.Key == Keys.F4 || e.Key == Keys.F5 || e.Key == Keys.F6 || e.Key == Keys.F7 || e.Key == Keys.F8 || e.Key == Keys.F9 || e.Key == Keys.F10 || e.Key == Keys.F11 || e.Key == Keys.F12)
						{
							if (e.Key == DataModel.Instance.CutScreenKey)
							{
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Text = "";
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.KeyUp += delegate
								{
									WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Text = "无";
								};
							}
							else
							{
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Text = e.Key.ToString();
								Settings.Default.PickupMsg = e.Key;
								Settings.Default.PickupMsgString = e.Key.ToString();
								Settings.Default.PickUpMsgType = 1;
								DataModel.Instance.PickUpMsgKey = Settings.Default.PickupMsg;
								DataModel.Instance.PickUpMsgType = Settings.Default.PickUpMsgType;
								Settings.Default.Save();
							}
						}
						if (e.Key == Keys.Q || e.Key == Keys.W || e.Key == Keys.E || e.Key == Keys.R || e.Key == Keys.T || e.Key == Keys.Y || e.Key == Keys.U || e.Key == Keys.I || e.Key == Keys.O || e.Key == Keys.P || e.Key == Keys.A || e.Key == Keys.S || e.Key == Keys.D || e.Key == Keys.F || e.Key == Keys.G || e.Key == Keys.H || e.Key == Keys.J || e.Key == Keys.K || e.Key == Keys.L || e.Key == Keys.Z || e.Key == Keys.X || e.Key == Keys.C || e.Key == Keys.V || e.Key == Keys.B || e.Key == Keys.N || e.Key == Keys.M)
						{
							if (e.Key == Keys.LControlKey || e.Key == Keys.LMenu || e.Key == Keys.LWin || e.Key == Keys.RControlKey || e.Key == Keys.RMenu || e.Key == Keys.RWin || e.Key == DataModel.Instance.CutScreenKey)
							{
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Text = "Ctrl + Alt + ";
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.KeyUp += delegate
								{
									WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Text = "无";
								};
							}
							else
							{
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Text = "Ctrl + Alt + " + e.Key.ToString();
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.KeyUp += delegate
								{
									WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Text = "Ctrl + Alt + " + e.Key.ToString();
								};
								Settings.Default.PickupMsg = e.Key;
								Settings.Default.PickupMsgString = "Ctrl + Alt + " + e.Key.ToString();
								Settings.Default.PickUpMsgType = 3;
								Settings.Default.PickUpMsgCAS = 1;
								DataModel.Instance.PickUpMsgKey = Settings.Default.PickupMsg;
								DataModel.Instance.PickUpMsgType = Settings.Default.PickUpMsgType;
								DataModel.Instance.PickUpMsgCAS = Settings.Default.PickUpMsgCAS;
								Settings.Default.Save();
							}
						}
						if (e.Key == Keys.D0 || e.Key == Keys.D1 || e.Key == Keys.D2 || e.Key == Keys.D3 || e.Key == Keys.D4 || e.Key == Keys.D5 || e.Key == Keys.D6 || e.Key == Keys.D7 || e.Key == Keys.D8 || e.Key == Keys.D9)
						{
							if (e.Key == DataModel.Instance.CutScreenKey)
							{
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Text = "";
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.KeyUp += delegate
								{
									WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Text = "无";
								};
							}
							else
							{
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Text = e.Key.ToString().Substring(1, 1);
								Settings.Default.PickupMsg = e.Key;
								Settings.Default.PickupMsgString = e.Key.ToString().Substring(1, 1);
								Settings.Default.PickUpMsgType = 1;
								DataModel.Instance.PickUpMsgKey = Settings.Default.PickupMsg;
								DataModel.Instance.PickUpMsgType = Settings.Default.PickUpMsgType;
								Settings.Default.Save();
							}
						}
						if (e.Key == Keys.NumPad0 || e.Key == Keys.NumPad1 || e.Key == Keys.NumPad2 || e.Key == Keys.NumPad3 || e.Key == Keys.NumPad4 || e.Key == Keys.NumPad5 || e.Key == Keys.NumPad6 || e.Key == Keys.NumPad7 || e.Key == Keys.NumPad8 || e.Key == Keys.NumPad9)
						{
							if (e.Key == DataModel.Instance.CutScreenKey)
							{
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Text = "";
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.KeyUp += delegate
								{
									WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Text = "无";
								};
							}
							else
							{
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Text = e.Key.ToString();
								Settings.Default.PickupMsg = e.Key;
								Settings.Default.PickupMsgString = e.Key.ToString();
								Settings.Default.PickUpMsgType = 1;
								DataModel.Instance.PickUpMsgKey = Settings.Default.PickupMsg;
								DataModel.Instance.PickUpMsgType = Settings.Default.PickUpMsgType;
								Settings.Default.Save();
							}
						}
						if (e.Key == Keys.Home || e.Key == Keys.End || e.Key == Keys.Insert || e.Key == Keys.Prior || e.Key == Keys.Next)
						{
							if (e.Key == DataModel.Instance.CutScreenKey)
							{
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Text = "";
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.KeyUp += delegate
								{
									WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Text = "无";
								};
							}
							else
							{
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Text = e.Key.ToString();
								Settings.Default.PickupMsg = e.Key;
								Settings.Default.PickupMsgString = e.Key.ToString();
								Settings.Default.PickUpMsgType = 1;
								DataModel.Instance.PickUpMsgKey = Settings.Default.PickupMsg;
								DataModel.Instance.PickUpMsgType = Settings.Default.PickUpMsgType;
								Settings.Default.Save();
							}
						}
						if ((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.None)
						{
							if (e.Key == Keys.LControlKey || e.Key == Keys.LMenu || e.Key == Keys.LWin || e.Key == Keys.RControlKey || e.Key == Keys.RMenu || e.Key == Keys.RWin || e.Key == DataModel.Instance.CutScreenKey)
							{
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Text = "Ctrl + ";
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.KeyUp += delegate
								{
									WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Text = "无";
								};
							}
							else
							{
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Text = "Ctrl + " + e.Key.ToString();
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.KeyUp += delegate
								{
									WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Text = "Ctrl + " + e.Key.ToString();
								};
								Settings.Default.PickupMsg = e.Key;
								Settings.Default.PickupMsgString = "Ctrl + " + e.Key.ToString();
								Settings.Default.PickUpMsgType = 2;
								DataModel.Instance.PickUpMsgKey = Settings.Default.PickupMsg;
								DataModel.Instance.PickUpMsgType = Settings.Default.PickUpMsgType;
								Settings.Default.Save();
							}
						}
						if ((Keyboard.Modifiers & ModifierKeys.Alt) != ModifierKeys.None)
						{
							if (e.Key == Keys.LControlKey || e.Key == Keys.LMenu || e.Key == Keys.LWin || e.Key == Keys.RControlKey || e.Key == Keys.RMenu || e.Key == Keys.RWin || e.Key == DataModel.Instance.CutScreenKey)
							{
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Text = "Alt + ";
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.KeyUp += delegate
								{
									WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Text = "无";
								};
							}
							else
							{
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Text = "Alt + " + e.Key.ToString();
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.KeyUp += delegate
								{
									WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Text = "Alt + " + e.Key.ToString();
								};
								Settings.Default.PickupMsg = e.Key;
								Settings.Default.PickupMsgString = "Alt + " + e.Key.ToString();
								Settings.Default.PickUpMsgType = 2;
								DataModel.Instance.PickUpMsgKey = Settings.Default.PickupMsg;
								DataModel.Instance.PickUpMsgType = Settings.Default.PickUpMsgType;
								Settings.Default.Save();
							}
						}
						if ((Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.None)
						{
							if (e.Key == Keys.LControlKey || e.Key == Keys.LMenu || e.Key == Keys.LWin || e.Key == Keys.RControlKey || e.Key == Keys.RMenu || e.Key == Keys.RWin || e.Key == DataModel.Instance.CutScreenKey)
							{
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Text = "Shift + ";
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.KeyUp += delegate
								{
									WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Text = "无";
								};
							}
							else
							{
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Text = "Shift + " + e.Key.ToString();
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.KeyUp += delegate
								{
									WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Text = "Shift + " + e.Key.ToString();
								};
								Settings.Default.PickupMsg = e.Key;
								Settings.Default.PickupMsgString = "Shift + " + e.Key.ToString();
								Settings.Default.PickUpMsgType = 2;
								DataModel.Instance.PickUpMsgKey = Settings.Default.PickupMsg;
								DataModel.Instance.PickUpMsgType = Settings.Default.PickUpMsgType;
								Settings.Default.Save();
							}
						}
						if ((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.None && (Keyboard.Modifiers & ModifierKeys.Alt) != ModifierKeys.None)
						{
							if (e.Key == Keys.LControlKey || e.Key == Keys.LMenu || e.Key == Keys.LWin || e.Key == Keys.LShiftKey || e.Key == Keys.RControlKey || e.Key == Keys.RMenu || e.Key == Keys.RWin || e.Key == Keys.RShiftKey || e.Key == DataModel.Instance.CutScreenKey)
							{
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Text = "Ctrl + Alt + ";
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.KeyUp += delegate
								{
									WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Text = "无";
								};
							}
							else
							{
								if (e.Key == Keys.D0 || e.Key == Keys.D1 || e.Key == Keys.D2 || e.Key == Keys.D3 || e.Key == Keys.D4 || e.Key == Keys.D5 || e.Key == Keys.D6 || e.Key == Keys.D7 || e.Key == Keys.D8 || e.Key == Keys.D9)
								{
									WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Text = "Ctrl + Alt + " + e.Key.ToString().Substring(1, 1);
								}
								else
								{
									WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Text = "Ctrl + Alt + " + e.Key.ToString();
								}
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.KeyUp += delegate
								{
									if (e.Key == Keys.D0 || e.Key == Keys.D1 || e.Key == Keys.D2 || e.Key == Keys.D3 || e.Key == Keys.D4 || e.Key == Keys.D5 || e.Key == Keys.D6 || e.Key == Keys.D7 || e.Key == Keys.D8 || e.Key == Keys.D9)
									{
										WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Text = "Ctrl + Alt + " + e.Key.ToString().Substring(1, 1);
									}
									else
									{
										WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Text = "Ctrl + Alt + " + e.Key.ToString();
									}
								};
								Settings.Default.PickupMsg = e.Key;
								Settings.Default.PickupMsgString = WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Text;
								Settings.Default.PickUpMsgType = 3;
								Settings.Default.PickUpMsgCAS = 1;
								DataModel.Instance.PickUpMsgKey = Settings.Default.PickupMsg;
								DataModel.Instance.PickUpMsgType = Settings.Default.PickUpMsgType;
								DataModel.Instance.PickUpMsgCAS = Settings.Default.PickUpMsgCAS;
								Settings.Default.Save();
							}
						}
						if ((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.None && (Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.None)
						{
							if (e.Key == Keys.LControlKey || e.Key == Keys.LMenu || e.Key == Keys.LWin || e.Key == Keys.LShiftKey || e.Key == Keys.RControlKey || e.Key == Keys.RMenu || e.Key == Keys.RWin || e.Key == Keys.RShiftKey || e.Key == DataModel.Instance.CutScreenKey)
							{
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Text = "Ctrl + Shift + ";
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.KeyUp += delegate
								{
									WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Text = "无";
								};
							}
							else
							{
								if (e.Key == Keys.D0 || e.Key == Keys.D1 || e.Key == Keys.D2 || e.Key == Keys.D3 || e.Key == Keys.D4 || e.Key == Keys.D5 || e.Key == Keys.D6 || e.Key == Keys.D7 || e.Key == Keys.D8 || e.Key == Keys.D9)
								{
									WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Text = "Ctrl + Shift + " + e.Key.ToString().Substring(1, 1);
								}
								else
								{
									WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Text = "Ctrl + Shift + " + e.Key.ToString();
								}
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.KeyUp += delegate
								{
									if (e.Key == Keys.D0 || e.Key == Keys.D1 || e.Key == Keys.D2 || e.Key == Keys.D3 || e.Key == Keys.D4 || e.Key == Keys.D5 || e.Key == Keys.D6 || e.Key == Keys.D7 || e.Key == Keys.D8 || e.Key == Keys.D9)
									{
										WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Text = "Ctrl + Shift + " + e.Key.ToString().Substring(1, 1);
									}
									else
									{
										WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Text = "Ctrl + Shift + " + e.Key.ToString();
									}
								};
								Settings.Default.PickupMsg = e.Key;
								Settings.Default.PickupMsgString = WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Text;
								Settings.Default.PickUpMsgType = 3;
								Settings.Default.PickUpMsgCAS = 2;
								DataModel.Instance.PickUpMsgKey = Settings.Default.PickupMsg;
								DataModel.Instance.PickUpMsgType = Settings.Default.PickUpMsgType;
								DataModel.Instance.PickUpMsgCAS = Settings.Default.PickUpMsgCAS;
								Settings.Default.Save();
							}
						}
						if ((Keyboard.Modifiers & ModifierKeys.Alt) != ModifierKeys.None && (Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.None)
						{
							if (e.Key == Keys.LControlKey || e.Key == Keys.LMenu || e.Key == Keys.LWin || e.Key == Keys.LShiftKey || e.Key == Keys.RControlKey || e.Key == Keys.RMenu || e.Key == Keys.RWin || e.Key == Keys.RShiftKey || e.Key == DataModel.Instance.CutScreenKey)
							{
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Text = "Alt + Shift + ";
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.KeyUp += delegate
								{
									WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Text = "无";
								};
							}
							else
							{
								if (e.Key == Keys.D0 || e.Key == Keys.D1 || e.Key == Keys.D2 || e.Key == Keys.D3 || e.Key == Keys.D4 || e.Key == Keys.D5 || e.Key == Keys.D6 || e.Key == Keys.D7 || e.Key == Keys.D8 || e.Key == Keys.D9)
								{
									WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Text = "Alt + Shift + " + e.Key.ToString().Substring(1, 1);
								}
								else
								{
									WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Text = "Alt + Shift + " + e.Key.ToString();
								}
								WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.KeyUp += delegate
								{
									if (e.Key == Keys.D0 || e.Key == Keys.D1 || e.Key == Keys.D2 || e.Key == Keys.D3 || e.Key == Keys.D4 || e.Key == Keys.D5 || e.Key == Keys.D6 || e.Key == Keys.D7 || e.Key == Keys.D8 || e.Key == Keys.D9)
									{
										WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Text = "Alt + Shift + " + e.Key.ToString().Substring(1, 1);
									}
									else
									{
										WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Text = "Alt + Shift + " + e.Key.ToString();
									}
								};
								Settings.Default.PickupMsg = e.Key;
								Settings.Default.PickupMsgString = WindowModel.Instance.SystemSettingWindow.hotkeySetting.tbxRecover.Text;
								Settings.Default.PickUpMsgType = 3;
								Settings.Default.PickUpMsgCAS = 3;
								DataModel.Instance.PickUpMsgKey = Settings.Default.PickupMsg;
								DataModel.Instance.PickUpMsgType = Settings.Default.PickUpMsgType;
								DataModel.Instance.PickUpMsgCAS = Settings.Default.PickUpMsgCAS;
								Settings.Default.Save();
							}
						}
					}
				}
				if (this.sessionService.IsLogin)
				{
					switch (DataModel.Instance.CutScreenType)
					{
					case 1:
						if (singleCutScreen)
						{
							this.AllowCutScreen();
						}
						break;
					case 2:
						if (ctrlCutScreen)
						{
							this.AllowCutScreen();
						}
						if (altCutScreen)
						{
							this.AllowCutScreen();
						}
						if (shiftCutScreen)
						{
							this.AllowCutScreen();
						}
						break;
					case 3:
						switch (DataModel.Instance.CutScreenCAS)
						{
						case 1:
							if (ctrlAltCutScreen)
							{
								this.AllowCutScreen();
							}
							break;
						case 2:
							if (ctrlShiftCutScreen)
							{
								this.AllowCutScreen();
							}
							break;
						case 3:
							if (altShiftCutScreen)
							{
								this.AllowCutScreen();
							}
							break;
						}
						break;
					}
					switch (DataModel.Instance.PickUpMsgType)
					{
					case 1:
						if (singlePickupMsg)
						{
							this.AllowPickup();
						}
						break;
					case 2:
						if (ctrlPickupMsg)
						{
							this.AllowPickup();
						}
						if (altPickupMsg)
						{
							this.AllowPickup();
						}
						if (shiftPickupMsg)
						{
							this.AllowPickup();
						}
						break;
					case 3:
						switch (DataModel.Instance.PickUpMsgCAS)
						{
						case 1:
							if (ctrlAltPickupMsg)
							{
								this.AllowPickup();
							}
							break;
						case 2:
							if (ctrlShiftPickupMsg)
							{
								this.AllowPickup();
							}
							break;
						case 3:
							if (altShiftPickupMsg)
							{
								this.AllowPickup();
							}
							break;
						}
						break;
					}
				}
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		private void AllowCutScreen()
		{
			if (this.sessionService.IsAllowCut)
			{
				if (this.sessionService.IsCutScreenHidenWindow)
				{
					this.sessionService.IsAllowCut = false;
					this.CutScreenHidenWindowProcessor();
					this.StartCaptureImage();
					this.CutScreenEndShowWindow();
				}
				else
				{
					this.sessionService.IsAllowCut = false;
					this.StartCaptureImage();
					this.CutScreenEndShowWindow();
				}
			}
		}
		private void AllowPickup()
		{
			if (DataModel.Instance.IsAllowPickup)
			{
				this.OnOpenHandler();
				this.PickUpMessage();
			}
		}
		private void StartCaptureImage()
		{
			CaptureImageTool capture = new CaptureImageTool();
			if (capture.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				System.Drawing.Image image = capture.Image;
				this.SetImageClipboard(image);
				string file = this.SaveCaptureImage(image);
				this.SetImageChatWindow(file);
			}
			this.sessionService.IsAllowCut = true;
		}
		public void SetImageChatWindow(string file)
		{
			try
			{
				if (!string.IsNullOrEmpty(file))
				{
					this.InserCaptureImageStaffChatWindow(file);
					this.InsertCaptureImageGroupChatWindow(file);
				}
			}
			catch (System.Exception e)
			{
				this.logger.Error(e.ToString());
			}
		}
		private void InsertCaptureImageGroupChatWindow(string file)
		{
			System.Collections.Generic.ICollection<TabItem> items = this.dataService.GetEntGroupChatTabs().Values;
			if (items != null)
			{
				INWindow inWindow = this.dataService.INWindow as INWindow;
				using (System.Collections.Generic.IEnumerator<TabItem> enumerator = items.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						CloseableTabItem item = (CloseableTabItem)enumerator.Current;
						if (inWindow != null)
						{
							if (item != null && item == inWindow.ContentTab.SelectedItem)
							{
								GroupChatTabControl chat = item.Content as GroupChatTabControl;
								if (chat != null)
								{
									chat.SnippingScreenHandler(file);
									break;
								}
							}
						}
					}
				}
			}
		}
		private void InserCaptureImageStaffChatWindow(string file)
		{
			System.Collections.Generic.ICollection<TabItem> items = this.dataService.GetStaffChatTabDics().Values;
			if (items != null)
			{
				INWindow inWindow = this.dataService.INWindow as INWindow;
				using (System.Collections.Generic.IEnumerator<TabItem> enumerator = items.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						CloseableTabItem item = (CloseableTabItem)enumerator.Current;
						if (inWindow != null)
						{
							if (item != null && item == inWindow.ContentTab.SelectedItem)
							{
								PersonalChatTabControl chat = item.Content as PersonalChatTabControl;
								if (chat != null)
								{
									chat.SnippingScreenHandler(file);
									break;
								}
							}
						}
					}
				}
			}
		}
		private string SaveCaptureImage(System.Drawing.Image image)
		{
			string file = "";
			if (image != null)
			{
				System.IO.MemoryStream ms = new System.IO.MemoryStream();
				image.Save(ms, ImageFormat.Png);
				BitmapImage bitmap = new BitmapImage();
				bitmap.BeginInit();
				bitmap.StreamSource = ms;
				bitmap.EndInit();
				string tmp = this.sessionService.DirImage + "SnippingScreenTempFile.jpg";
				System.IO.FileStream fs = new System.IO.FileStream(tmp, System.IO.FileMode.Create);
				new PngBitmapEncoder
				{
					Frames = 
					{
						BitmapFrame.Create(bitmap)
					}
				}.Save(fs);
				fs.Position = 0L;
				string md5 = this.MD5Stream(fs);
				fs.Flush();
				fs.Close();
				if (md5 != string.Empty)
				{
					file = this.sessionService.DirImage + md5 + ".png";
					if (!System.IO.File.Exists(file))
					{
						System.IO.File.Move(tmp, file);
					}
				}
			}
			return file;
		}
		public void SetImageClipboard(System.Drawing.Image image)
		{
			try
			{
				if (image != null)
				{
					System.IO.MemoryStream ms = new System.IO.MemoryStream();
					image.Save(ms, ImageFormat.Png);
					BitmapImage bitmap = new BitmapImage();
					bitmap.BeginInit();
					bitmap.StreamSource = ms;
					bitmap.EndInit();
					System.Windows.Clipboard.SetImage(bitmap);
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
		private string MD5Stream(System.IO.Stream stream)
		{
			System.Security.Cryptography.MD5CryptoServiceProvider md5 = null;
			string resule = string.Empty;
			try
			{
				md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
				byte[] hash = md5.ComputeHash(stream);
				resule = System.BitConverter.ToString(hash);
				resule = resule.Replace("-", "");
			}
			finally
			{
				if (md5 != null)
				{
					md5.Dispose();
				}
			}
			return resule;
		}
		private void CutScreenHidenWindowProcessor()
		{
			if (this.sessionService.IsCutScreenHidenWindow)
			{
				INWindow inWindow = this.dataService.INWindow as INWindow;
				if (inWindow != null)
				{
					inWindow.WindowState = WindowState.Minimized;
				}
			}
		}
		private void CutScreenEndShowWindow()
		{
			if (this.sessionService.IsCutScreenHidenWindow)
			{
				INWindow inWindow = this.dataService.INWindow as INWindow;
				if (inWindow != null)
				{
					inWindow.WindowState = WindowState.Normal;
				}
			}
		}
		private System.Windows.Forms.MenuItem[] InitializeMenu()
		{
			this.separator1 = new System.Windows.Forms.MenuItem("-");
			this.separator2 = new System.Windows.Forms.MenuItem("-");
			this.separator3 = new System.Windows.Forms.MenuItem("-");
			this.openItem = new System.Windows.Forms.MenuItem("打开", new System.EventHandler(this.OnOpenClick));
			this.optionItem = new System.Windows.Forms.MenuItem("选项", new System.EventHandler(this.OnOptionClick));
			this.updateItem = new System.Windows.Forms.MenuItem("检查更新", new System.EventHandler(this.OnUpdateClick));
			this.aboutItem = new System.Windows.Forms.MenuItem("关于", new System.EventHandler(this.OnAboutClick));
			this.logoutItem = new System.Windows.Forms.MenuItem("注销", new System.EventHandler(this.OnLogoutClick));
			this.exitItem = new System.Windows.Forms.MenuItem("退出", new System.EventHandler(this.OnExitClick));
			return new System.Windows.Forms.MenuItem[]
			{
				this.openItem,
				this.optionItem,
				this.separator1,
				this.updateItem,
				this.separator2,
				this.aboutItem,
				this.logoutItem,
				this.separator3,
				this.exitItem
			};
		}
		private void InitTray()
		{
			this.NotifyIcon = new NotifyIcon();
			this.notificationMenu = new System.Windows.Forms.ContextMenu(this.InitializeMenu());
			this.NotifyIcon.Text = "IDKin";
			this.NotifyIcon.Icon = IDKin.IM.Windows.Properties.Resources.notifyIcon_gray;
			this.NotifyIcon.ContextMenu = this.notificationMenu;
			this.NotifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.NotifyIcon_MouseClick);
			this.NotifyIcon.Visible = true;
			this.updateItem.Enabled = false;
			this.optionItem.Enabled = this.sessionService.IsEnable;
			this.aboutItem.Enabled = this.sessionService.IsEnable;
			this.logoutItem.Enabled = this.sessionService.IsEnable;
		}
		private void App_Exit(object sender, ExitEventArgs e)
		{
			if (this.NotifyIcon != null)
			{
				this.NotifyIcon.Visible = false;
				this.NotifyIcon.Dispose();
				this.NotifyIcon = null;
			}
			this.logger.Info(e.ApplicationExitCode.ToString());
		}
		private void NotifyIcon_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			this.OnOpenHandler();
			this.MouseClickHandler(e);
		}
		private void OnOpenHandler()
		{
			try
			{
				switch (this.sessionService.IsLogin)
				{
				case false:
					if (this.dataService.LoginWindow != null)
					{
						this.dataService.LoginWindow.Show();
						this.dataService.LoginWindow.Activate();
					}
					break;
				case true:
					if (this.dataService.INWindow != null)
					{
						if (this.dataService.INWindow.WindowState == WindowState.Minimized)
						{
							this.dataService.INWindow.WindowState = WindowState.Normal;
						}
						this.dataService.INWindow.Activate();
					}
					break;
				}
			}
			catch (System.Exception e)
			{
				this.logger.Error(e.Message);
			}
		}
		private void NotifyIcon_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			this.MouseClickHandler(e);
		}
		private void MouseClickHandler(System.Windows.Forms.MouseEventArgs e)
		{
			try
			{
				if (e.Button == MouseButtons.Left)
				{
					if (this.sessionService.IsLogin)
					{
						if (!DataModel.Instance.HasMessage())
						{
							if (this.sessionService.Uid != 0L)
							{
								INWindow inWindow = this.dataService.INWindow as INWindow;
								if (inWindow != null)
								{
									if (inWindow.Top <= 0.0 || inWindow.Left <= 0.0 || inWindow.Left + inWindow.Width >= SystemParameters.WorkArea.Width)
									{
										inWindow.Left = (SystemParameters.WorkArea.Width - inWindow.Width) / 2.0;
										inWindow.Top = (SystemParameters.WorkArea.Height - inWindow.Height) / 2.0;
									}
									inWindow.Show();
									inWindow.Activate();
								}
							}
							else
							{
								LogonWindow logonWindow = this.dataService.LoginWindow as LogonWindow;
								if (logonWindow != null)
								{
									logonWindow.Show();
									logonWindow.Activate();
								}
							}
						}
						else
						{
							this.PickUpMessage();
						}
					}
					else
					{
						LogonWindow logonWindow = this.dataService.LoginWindow as LogonWindow;
						if (logonWindow != null)
						{
							logonWindow.Show();
							logonWindow.Activate();
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		private void GroupNewMessage(IDKin.IM.Core.Message message)
		{
			EntGroupTab item = this.dataService.GetEntGroupChatTab(message.Gid) as EntGroupTab;
			if (item == null)
			{
				EntGroup group = this.dataService.GetEntGroup(message.Gid);
				if (group != null)
				{
					item = new EntGroupTab(group);
					item.SetDefaultStyle();
					((INWindow)this.dataService.INWindow).ContentTab.Items.Add(item);
					this.dataService.AddEntGroupChatTab(group.Gid, item);
					((INWindow)this.dataService.INWindow).ContentTab.SelectedItem = item;
				}
			}
			GroupChatTabControl tab = item.TabContent;
			if (tab != null)
			{
				tab.ChatComponent.AddMessageGroup(message, false);
				tab.ChatComponent.inputMsgBox.Focus();
			}
		}
		private void RosterNewMessage(IDKin.IM.Core.Message msg)
		{
			RosterTab item = this.dataService.GetRosterChatTab((long)((ulong)Jid.GetUid(msg.FromJid))) as RosterTab;
			if (item == null)
			{
				Roster roster = this.dataService.GetRoster((long)((ulong)Jid.GetUid(msg.FromJid)));
				if (roster != null)
				{
					item = new RosterTab(roster);
					item.SetDefaultStyle();
					((INWindow)this.dataService.INWindow).ContentTab.Items.Add(item);
					this.dataService.AddRosterChatTab(roster.Uid, item);
					((INWindow)this.dataService.INWindow).ContentTab.SelectedItem = item;
				}
			}
			FriendsChatTabControl tab = item.TabContent;
			if (tab != null)
			{
				tab.ChatComponent.AddMessageRoster(msg, false);
				tab.ChatComponent.inputMsgBox.Focus();
			}
		}
		private void StaffNewMessage(IDKin.IM.Core.Message message)
		{
			EntStaffTab item = this.dataService.GetStaffChatTab((long)((ulong)Jid.GetUid(message.FromJid))) as EntStaffTab;
			if (item == null)
			{
				Staff staff = this.dataService.GetStaff((long)((ulong)Jid.GetUid(message.FromJid)));
				if (staff != null)
				{
					item = new EntStaffTab(staff);
					item.SetDefaultStyle();
					((INWindow)this.dataService.INWindow).ContentTab.Items.Add(item);
					this.dataService.AddStaffChatTab(staff.Uid, item);
					((INWindow)this.dataService.INWindow).ContentTab.SelectedItem = item;
				}
			}
			PersonalChatTabControl tab = item.TabContent;
			if (tab != null)
			{
				tab.ChatComponent.AddMessageStaff(message, false);
				tab.ChatComponent.inputMsgBox.Focus();
			}
		}
		public void PickUpMessage()
		{
			try
			{
				this.PickUpMessageProcessor();
				System.Collections.Generic.List<IDKin.IM.Core.Message> list = DataModel.Instance.GetLastMessage();
				if (list != null && list.Count > 0)
				{
					IDKin.IM.Core.Message message = list[0];
					if (message != null)
					{
						if (message.MessageObjectType == MessageActorType.EntStaff)
						{
							Staff staff = this.dataService.GetStaff((long)((ulong)Jid.GetUid(message.FromJid)));
							if (staff != null)
							{
								foreach (IDKin.IM.Core.Message msg in list)
								{
									this.StaffNewMessage(msg);
								}
								DataModel.Instance.RemoveMessage(staff.Uid, MessageActorType.EntStaff);
							}
						}
						if (message.MessageObjectType == MessageActorType.EntGroup)
						{
							EntGroup group = this.dataService.GetEntGroup(message.Gid);
							if (group != null)
							{
								foreach (IDKin.IM.Core.Message msg in list)
								{
									this.GroupNewMessage(msg);
								}
								DataModel.Instance.RemoveMessage(group.Gid, MessageActorType.EntGroup);
							}
						}
						if (message.MessageObjectType == MessageActorType.Roster)
						{
							Roster roster = this.dataService.GetRoster((long)((ulong)Jid.GetUid(message.FromJid)));
							if (roster != null)
							{
								foreach (IDKin.IM.Core.Message msg in list)
								{
									this.RosterNewMessage(msg);
								}
								DataModel.Instance.RemoveMessage(roster.Uid, MessageActorType.Roster);
							}
						}
						if (message.MessageObjectType == MessageActorType.AddRoster)
						{
							RosterAddRequest request = message.MessageObject as RosterAddRequest;
							if (request != null)
							{
								this.RosterAddNewMessage(request);
								DataModel.Instance.RemoveMessage(request.uid, MessageActorType.AddRoster);
							}
						}
						if (message.MessageObjectType == MessageActorType.AddRosterAsk)
						{
							RosterAddResponse response = message.MessageObject as RosterAddResponse;
							if (response != null)
							{
								this.RosterAddAskNewMessage(response);
								DataModel.Instance.RemoveMessage(response.uid, MessageActorType.AddRosterAsk);
							}
						}
						if (message.MessageObjectType == MessageActorType.CooperationStaff)
						{
							CooperationStaff staff2 = this.dataService.GetCooperationStaff((long)((ulong)Jid.GetUid(message.FromJid)), message.ProjectId);
							if (staff2 != null)
							{
								foreach (IDKin.IM.Core.Message msg in list)
								{
									this.CooperationStaffNewMessage(msg);
								}
								DataModel.Instance.RemoveCooperationStatffMessage(staff2.Uid, staff2.UnitedProjectid, MessageActorType.CooperationStaff);
							}
						}
						this.FlashIconPorcessor();
					}
				}
			}
			catch (System.NullReferenceException ex)
			{
				this.logger.Error("PickUpMessage:" + ex.Message + ":" + ex.StackTrace);
			}
			finally
			{
				MessageBoxWindow box = DataModel.Instance.GetMessageBox();
				if (box != null)
				{
					box.Refresh();
				}
				INWindow inWindow = this.dataService.INWindow as INWindow;
				if (inWindow != null)
				{
					if (inWindow.Top <= 0.0 || inWindow.Left <= 0.0 || inWindow.Left + inWindow.Width >= SystemParameters.WorkArea.Width)
					{
						inWindow.Left = (SystemParameters.WorkArea.Width - inWindow.Width) / 2.0;
						inWindow.Top = (SystemParameters.WorkArea.Height - inWindow.Height) / 2.0;
					}
					inWindow.Activate();
				}
			}
		}
		private void RosterAddAskNewMessage(RosterAddResponse response)
		{
			switch (response.type)
			{
			case 1:
				this.ArgeeAndRoster(response);
				break;
			case 2:
				this.ArgeeAndRoster(response);
				break;
			case 3:
				this.RejectRoster(response);
				break;
			}
		}
		private void ArgeeAndRoster(RosterAddResponse response)
		{
			if (response != null)
			{
				System.Windows.MessageBox.Show(response.user.name + "同意您添加为好友!");
				Roster roster = new Roster();
				roster.Uid = response.user.uid;
				roster.Jid = response.user.jid;
				roster.Name = response.user.name;
				roster.Nickname = response.user.nickname;
				roster.Status = (UserStatus)System.Enum.Parse(typeof(UserStatus), response.user.status.ToString());
				roster.Signature = response.user.signature;
				this.dataService.AddRoster(roster);
				INWindow inWindow = this.dataService.INWindow as INWindow;
				inWindow.FriendsList.AddRoster(roster);
			}
		}
		private void RejectRoster(RosterAddResponse response)
		{
			System.Windows.MessageBox.Show(response.user.name + "拒绝您添加为好友!");
		}
		private void RosterAddNewMessage(RosterAddRequest request)
		{
			FriendRequestWindow requestWindow = new FriendRequestWindow(request.uid, request.ruid, request.rjid, request.message, request.user, request.category_id);
			requestWindow.Show();
		}
		private void FlashIconPorcessor()
		{
			System.Collections.Generic.List<IDKin.IM.Core.Message> list2 = DataModel.Instance.GetLastMessage();
			if (list2 != null && list2.Count > 0)
			{
				IDKin.IM.Core.Message message2 = list2[0];
				if (message2 != null)
				{
					if (message2.MessageObjectType == MessageActorType.EntStaff)
					{
						NotifyIconUtil.Instance.SetFlashIcon(FlashIconType.EntStaff);
					}
					if (message2.MessageObjectType == MessageActorType.EntGroup)
					{
						NotifyIconUtil.Instance.SetFlashIcon(FlashIconType.EntGroup);
					}
					if (message2.MessageObjectType == MessageActorType.Roster)
					{
						NotifyIconUtil.Instance.SetFlashIcon(FlashIconType.Roster);
					}
					if (message2.MessageObjectType == MessageActorType.AddRoster)
					{
						NotifyIconUtil.Instance.SetFlashIcon(FlashIconType.MessageCenter);
					}
					if (message2.MessageObjectType == MessageActorType.AddRosterAsk)
					{
						NotifyIconUtil.Instance.SetFlashIcon(FlashIconType.MessageCenter);
					}
				}
			}
			else
			{
				NotifyIconUtil.Instance.SetFlashIcon(FlashIconType.Default);
			}
		}
		private void ShowGroupNewMessage(long groupId, System.Collections.Generic.List<IDKin.IM.Core.Message> list)
		{
			if (list != null)
			{
				foreach (IDKin.IM.Core.Message message in list)
				{
					this.GroupNewMessage(message);
				}
				DataModel.Instance.RemoveMessage(groupId, MessageActorType.EntGroup);
			}
		}
		private void ShowStaffNewMessage(long staffId, System.Collections.Generic.List<IDKin.IM.Core.Message> list)
		{
			if (list != null)
			{
				foreach (IDKin.IM.Core.Message message in list)
				{
					this.StaffNewMessage(message);
				}
				DataModel.Instance.RemoveMessage(staffId, MessageActorType.EntStaff);
			}
		}
		private void PickUpMessageProcessor()
		{
			INWindow inWindow = this.dataService.INWindow as INWindow;
			DataModel dataModel = DataModel.Instance;
			ItemCollection ic = inWindow.ContentTab.Items;
			foreach (TabItem item in (System.Collections.IEnumerable)ic)
			{
				if (item != null)
				{
					CloseableTabItem cti = item as CloseableTabItem;
					TabItemHeaderControl tabHeader = null;
					if (cti != null)
					{
						tabHeader = (cti.Header as TabItemHeaderControl);
					}
					PersonalChatTabControl pctc = item.Content as PersonalChatTabControl;
					GroupChatTabControl gctc = item.Content as GroupChatTabControl;
					FriendsChatTabControl fctc = item.Content as FriendsChatTabControl;
					CoopStaffChatTabControl coopStaffChatTabControl = item.Content as CoopStaffChatTabControl;
					if (pctc != null)
					{
						System.Collections.Generic.List<IDKin.IM.Core.Message> list = dataModel.GetMessage(pctc.StaffId, MessageActorType.EntStaff);
						if (list != null)
						{
							if (tabHeader != null)
							{
								tabHeader.SetFlashingStyle();
							}
							this.ShowStaffNewMessage(pctc.StaffId, list);
						}
					}
					else
					{
						if (gctc != null)
						{
							System.Collections.Generic.List<IDKin.IM.Core.Message> list = dataModel.GetMessage(gctc.GroupId, MessageActorType.EntGroup);
							if (list != null)
							{
								if (tabHeader != null)
								{
									tabHeader.SetFlashingStyle();
								}
								this.ShowGroupNewMessage(gctc.GroupId, list);
							}
						}
					}
					if (fctc != null)
					{
						System.Collections.Generic.List<IDKin.IM.Core.Message> list = dataModel.GetMessage(fctc.RosterId, MessageActorType.Roster);
						if (list != null)
						{
							if (tabHeader != null)
							{
								tabHeader.SetFlashingStyle();
							}
							this.ShowRosterNewMessage(fctc.RosterId, list);
						}
					}
					if (coopStaffChatTabControl != null)
					{
						System.Collections.Generic.List<IDKin.IM.Core.Message> list = dataModel.GetCooperationStaffMessage(coopStaffChatTabControl.Uid, coopStaffChatTabControl.Projectid, MessageActorType.CooperationStaff);
						if (list != null)
						{
							if (tabHeader != null)
							{
								tabHeader.SetFlashingStyle();
							}
							this.ShowCooperationStaffNewMessage(coopStaffChatTabControl.Uid, coopStaffChatTabControl.Projectid, list);
						}
					}
				}
			}
		}
		private void ShowCooperationStaffNewMessage(long staffId, string projectid, System.Collections.Generic.List<IDKin.IM.Core.Message> list)
		{
			if (list != null)
			{
				foreach (IDKin.IM.Core.Message message in list)
				{
					this.CooperationStaffNewMessage(message);
				}
				DataModel.Instance.RemoveCooperationStatffMessage(staffId, projectid, MessageActorType.CooperationStaff);
			}
		}
		private void CooperationStaffNewMessage(IDKin.IM.Core.Message message)
		{
			CoopStaffTab item = this.dataService.GetCooperationStaffChatTab((long)((ulong)Jid.GetUid(message.FromJid)), message.ProjectId) as CoopStaffTab;
			if (item == null)
			{
				CooperationStaff staff = this.dataService.GetCooperationStaff((long)((ulong)Jid.GetUid(message.FromJid)), message.ProjectId);
				CooperationProjectWrapper cooperationProjectWrapper = this.dataService.GetCooperationProjectWrapper(message.ProjectId);
				if (staff != null && cooperationProjectWrapper != null)
				{
					item = new CoopStaffTab(staff, cooperationProjectWrapper);
					item.SetDefaultStyle();
					((INWindow)this.dataService.INWindow).ContentTab.Items.Add(item);
					this.dataService.AddCooperationStaffChatTab(staff.Uid, staff.UnitedProjectid, item);
					((INWindow)this.dataService.INWindow).ContentTab.SelectedItem = item;
				}
			}
			CoopStaffChatTabControl tab = item.TabContent;
			if (tab != null)
			{
				tab.ChatComponent.AddCooperationMessageStaff(message, false);
				tab.ChatComponent.inputMsgBox.Focus();
			}
		}
		private void ShowRosterNewMessage(long rosterId, System.Collections.Generic.List<IDKin.IM.Core.Message> list)
		{
			if (list != null)
			{
				foreach (IDKin.IM.Core.Message message in list)
				{
					this.RosterNewMessage(message);
				}
				DataModel.Instance.RemoveMessage(rosterId, MessageActorType.Roster);
			}
		}
		private void OnOpenClick(object sender, System.EventArgs e)
		{
			this.OnOpenHandler();
		}
		private void OnOptionClick(object sender, System.EventArgs e)
		{
			try
			{
				SystemSettingWindow systemSetting = WindowModel.Instance.SystemSettingWindow;
				systemSetting.Show();
				systemSetting.Activate();
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		private void OnUpdateClick(object sender, System.EventArgs e)
		{
			try
			{
				UpdateWindow updateWindow = WindowModel.Instance.UpdateWindow;
				updateWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
				updateWindow.ShowDialog();
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		private void OnAboutClick(object sender, System.EventArgs e)
		{
			try
			{
				AboutWindow about = WindowModel.Instance.AboutWindow;
				INWindow inWindow = this.dataService.INWindow as INWindow;
				about.Owner = inWindow;
				about.WindowStartupLocation = WindowStartupLocation.CenterOwner;
				DoubleAnimation animate = new DoubleAnimation();
				animate.FillBehavior = FillBehavior.HoldEnd;
				animate.From = new double?(0.0);
				animate.To = new double?(0.5);
				animate.Duration = new Duration(System.TimeSpan.FromSeconds(0.8));
				inWindow.RootBorder.Visibility = Visibility.Visible;
				inWindow.RootBorder.Background.BeginAnimation(System.Windows.Media.Brush.OpacityProperty, animate);
				about.ShowDialog();
				about.tbxChangelog.Focus();
				inWindow.RootBorder.Visibility = Visibility.Collapsed;
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		private void OnLogoutClick(object sender, System.EventArgs e)
		{
			try
			{
				INWindow inWindow = this.dataService.INWindow as INWindow;
				if (inWindow != null)
				{
					inWindow.IsTaskClose = false;
					inWindow.ChangeUserHandler();
				}
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		private void OnExitClick(object sender, System.EventArgs e)
		{
			try
			{
				INViewModel inViewModel = new INViewModel();
				inViewModel.InitService();
				inViewModel.LogOff();
				INWindow inWindow = this.dataService.INWindow as INWindow;
				if (inWindow != null)
				{
					inWindow.IsTaskClose = false;
				}
				System.Windows.Application.Current.Shutdown(1004);
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/view/systemwindow.xaml", UriKind.Relative);
        //        System.Windows.Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    if (connectionId != 1)
        //    {
        //        this._contentLoaded = true;
        //    }
        //    else
        //    {
        //        ((SystemWindow)target).Loaded += new RoutedEventHandler(this.Window_Loaded);
        //    }
        //}
	}
}
