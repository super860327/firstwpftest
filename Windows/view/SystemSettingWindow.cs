using IDKin.IM.CustomComponents.Controls;
using IDKin.IM.Data;
using IDKin.IM.Theme.Helper;
using IDKin.IM.Windows.Model;
using IDKin.IM.Windows.Properties;
using IDKin.IM.Windows.Util;
using IDKin.IM.Windows.View.Commons;
using IDKin.IM.Windows.View.SystemSetting;
using System;
using System.CodeDom.Compiler;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
namespace IDKin.IM.Windows.View
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class SystemSettingWindow : Window//, IComponentConnector
	{
		private ISessionService sessionService = ServiceUtil.Instance.SessionService;
		private IDataService dataService = ServiceUtil.Instance.DataService;

        //private bool _contentLoaded;
		public SystemSettingWindow()
		{
			ThemeSwitcher.LoadSkin(ThemeEnum.Aero, this);
			this.InitializeComponent();
			this.InitData();
			this.AddEventListenerHandler();
			this.rdoBasic.IsChecked = new bool?(true);
		}
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonDown(e);
			base.DragMove();
		}
		private void AddEventListenerHandler()
		{
			this.fileTransmitSetting.btnScan.Click += new RoutedEventHandler(this.FileTransportSelectDir_Click);
			this.autoReplySetting.btnAutoReplyAdd.Click += new RoutedEventHandler(this.btnAutoReplyAdd_Click);
			this.autoReplySetting.btnAutoReplyEdit.Click += new RoutedEventHandler(this.btnAutoReplyEdit_Click);
			this.autoReplySetting.btnAutoReplyDelete.Click += new RoutedEventHandler(this.btnAutoReplyDelete_Click);
			this.autoReplySetting.cbxFastReply.SelectionChanged += new SelectionChangedEventHandler(this.FastReplyChange);
			this.autoReplySetting.btnFastReplyAdd.Click += new RoutedEventHandler(this.btnFastReplyAdd_Click);
			this.autoReplySetting.btnFastReplyDelete.Click += new RoutedEventHandler(this.btnFastReplyDelete_Click);
			base.Closing += delegate(object sender, CancelEventArgs e)
			{
				WindowModel.Instance.SystemSettingWindow = null;
			};
		}
		private void InitData()
		{
			this.InitBase();
			this.InitFileTransport();
			this.InitAutoReply();
			this.InitFastReply();
			this.InitMessageNotfly();
		}
		private void InitBase()
		{
			this.basicSetting.chkBase_AutoLogin.IsChecked = new bool?(Settings.Default.SystemSetup_Base_AutoLogin);
			this.basicSetting.chkBase_AutoStart.IsChecked = new bool?(Settings.Default.SystemSetup_Base_AutoStart);
			this.basicSetting.rdoBase_Exit_Hide.IsChecked = new bool?(Settings.Default.SystemSetup_Base_ExitHide);
			this.basicSetting.rdoBase_Exit_ShutDown.IsChecked = new bool?(!Settings.Default.SystemSetup_Base_ExitHide);
		}
		private void InitFileTransport()
		{
			this.fileTransmitSetting.txtFileTransportSaveDir.Text = Settings.Default.SystemSetup_FileTransport_SaveDir;
		}
		private void InitAutoReply()
		{
			this.autoReplySetting.chkAutoReply.IsChecked = new bool?(Settings.Default.SystemSetup_AutoReply_Is);
			StringCollection sc = Settings.Default.SystemSetup_AutoReply_Message;
			int size = sc.Count;
			this.autoReplySetting.lbAutoReplyContent.Items.Clear();
			for (int i = 0; i < size; i++)
			{
				ListBoxItem lbi = new ListBoxItem();
				lbi.Content = i + 1 + "   " + sc[i];
				this.autoReplySetting.lbAutoReplyContent.Items.Add(lbi);
			}
			this.autoReplySetting.lbAutoReplyContent.SelectedIndex = Settings.Default.SystemSetup_AutoReply_Index;
		}
		private void InitFastReply()
		{
			StringCollection sc = Settings.Default.SystemSetup_FastReply_Message;
			int size = sc.Count;
			this.autoReplySetting.cbxFastReply.Items.Clear();
			for (int i = 0; i < size; i++)
			{
				ComboBoxItem item = new ComboBoxItem();
				item.Content = i + 1;
				item.DataContext = sc[i];
				this.autoReplySetting.cbxFastReply.Items.Add(item);
			}
			this.autoReplySetting.cbxFastReply.SelectedIndex = Settings.Default.SystemSetup_FastReply_Index;
			if (sc.Count == 0)
			{
				this.autoReplySetting.tbxFastReply.IsEnabled = false;
			}
			if (sc.Count > 0)
			{
				ComboBoxItem item = this.autoReplySetting.cbxFastReply.SelectedItem as ComboBoxItem;
				if (item != null)
				{
					this.autoReplySetting.tbxFastReply.Text = ((item.DataContext == null) ? "" : item.DataContext.ToString());
					this.autoReplySetting.cbxFastReply.DataContext = item.DataContext;
				}
				else
				{
					this.autoReplySetting.tbxFastReply.Text = "";
				}
			}
		}
		private void InitMessageNotfly()
		{
			this.msgNotifySetting.chkShowMessageBox.IsChecked = new bool?(Settings.Default.SystemSetup_MessageNotify_MessageBoxShow);
		}
		private void FileTransportSelectDir_Click(object sender, RoutedEventArgs e)
		{
			FolderBrowserDialog fdb = new FolderBrowserDialog();
			DialogResult dr = fdb.ShowDialog();
			if (System.Windows.Forms.DialogResult.OK == dr && fdb.SelectedPath != string.Empty)
			{
				if (fdb.SelectedPath.Length <= 3)
				{
					this.fileTransmitSetting.txtFileTransportSaveDir.Text = fdb.SelectedPath;
				}
				else
				{
					this.fileTransmitSetting.txtFileTransportSaveDir.Text = fdb.SelectedPath + "\\";
				}
			}
		}
		private void btnAutoReplyAdd_Click(object sender, RoutedEventArgs e)
		{
			new PromptWindow
			{
				Owner = this,
				WindowStartupLocation = WindowStartupLocation.CenterOwner,
				Title = "添加自动回复",
				lblTitle = 
				{
					Content = "回复内容(100字以内)"
				},
				Enter = new PromptWindowEnter(this.AutoReplAddEnter)
			}.ShowDialog();
		}
		private void AutoReplAddEnter(string str)
		{
			Settings.Default.SystemSetup_AutoReply_Message.Add(str);
			this.InitAutoReply();
		}
		private void btnAutoReplyEdit_Click(object sender, RoutedEventArgs e)
		{
			new PromptWindow
			{
				Owner = this,
				WindowStartupLocation = WindowStartupLocation.CenterOwner,
				Title = "修改自动回复",
				lblTitle = 
				{
					Content = "回复内容(100字以内)"
				},
				Enter = new PromptWindowEnter(this.AutoReplAddEdit),
				txtContext = 
				{
					Text = Settings.Default.SystemSetup_AutoReply_Message[this.autoReplySetting.lbAutoReplyContent.SelectedIndex]
				}
			}.ShowDialog();
		}
		private void AutoReplAddEdit(string str)
		{
			Settings.Default.SystemSetup_AutoReply_Message[this.autoReplySetting.lbAutoReplyContent.SelectedIndex] = str;
			this.InitAutoReply();
		}
		private void btnAutoReplyDelete_Click(object sender, RoutedEventArgs e)
		{
			if (Settings.Default.SystemSetup_AutoReply_Message.Count == 1)
			{
				Alert.Show("自动回复至少要保留一条内容！", "提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
			}
			else
			{
				int i = this.autoReplySetting.lbAutoReplyContent.SelectedIndex;
				Settings.Default.SystemSetup_AutoReply_Message.RemoveAt(i);
				this.autoReplySetting.lbAutoReplyContent.Items.RemoveAt(i);
				this.autoReplySetting.lbAutoReplyContent.SelectedIndex = 0;
			}
		}
		private void FastReplyAddHandler(string str)
		{
			Settings.Default.SystemSetup_FastReply_Message.Add(str);
			this.InitFastReply();
		}
		private void FastReplyChange(object sender, SelectionChangedEventArgs e)
		{
			ComboBoxItem item = this.autoReplySetting.cbxFastReply.SelectedItem as ComboBoxItem;
			if (item != null)
			{
				this.autoReplySetting.tbxFastReply.Text = ((item.DataContext == null) ? "" : item.DataContext.ToString());
				this.autoReplySetting.cbxFastReply.DataContext = item.DataContext;
			}
			else
			{
				this.autoReplySetting.tbxFastReply.Text = "";
			}
		}
		private void btnFastReplyAdd_Click(object sender, RoutedEventArgs e)
		{
			if (!this.autoReplySetting.tbxFastReply.IsEnabled)
			{
				this.autoReplySetting.tbxFastReply.IsEnabled = true;
			}
			ComboBoxItem item = new ComboBoxItem();
			item.Content = this.autoReplySetting.cbxFastReply.Items.Count + 1;
			item.DataContext = this.autoReplySetting.tbxFastReply.Text;
			if (this.autoReplySetting.cbxFastReply.Items.Count <= 0)
			{
				this.autoReplySetting.cbxFastReply.Items.Add(item);
				this.autoReplySetting.cbxFastReply.SelectedItem = item;
			}
			else
			{
				if (this.autoReplySetting.tbxFastReply.Text.Length == 0)
				{
					Alert.Show("请添加消息！", "提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
				}
				else
				{
					this.autoReplySetting.cbxFastReply.Items.Add(item);
					this.autoReplySetting.cbxFastReply.SelectedItem = item;
					this.autoReplySetting.cbxFastReply.DataContext = item.DataContext;
					this.FastReplyAddHandler(item.DataContext.ToString());
					this.autoReplySetting.tbxFastReply.Text = "";
				}
			}
			this.autoReplySetting.tbxFastReply.Focus();
		}
		private void btnFastReplyDelete_Click(object sender, RoutedEventArgs e)
		{
			if (this.autoReplySetting.cbxFastReply.SelectedItem != null)
			{
				if (this.autoReplySetting.cbxFastReply.Items.Count != 0)
				{
					Settings.Default.SystemSetup_FastReply_Message.RemoveAt(this.autoReplySetting.cbxFastReply.SelectedIndex);
					this.autoReplySetting.cbxFastReply.Items.RemoveAt(this.autoReplySetting.cbxFastReply.SelectedIndex);
					this.autoReplySetting.cbxFastReply.SelectedIndex = this.autoReplySetting.cbxFastReply.SelectedIndex + 1;
				}
				if (this.autoReplySetting.cbxFastReply.SelectedIndex == -1)
				{
					this.autoReplySetting.tbxFastReply.IsEnabled = false;
					Settings.Default.SystemSetup_FastReply_Message.Clear();
				}
			}
		}
		private void btnApply_Click(object sender, RoutedEventArgs e)
		{
			this.Save();
			Alert.Show("您的设置更改已成功!", "提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
		}
		private void btnClose_Click(object sender, RoutedEventArgs e)
		{
			base.Close();
		}
		private void btnMin_Click(object sender, RoutedEventArgs e)
		{
			base.WindowState = WindowState.Minimized;
		}
		private void btnOK_Click(object sender, RoutedEventArgs e)
		{
			this.Save();
			base.Close();
		}
		private void Save()
		{
			this.Base();
			this.FileTransport();
			this.AutoReply();
			this.FastReply();
			this.MessageNotify();
			Settings.Default.Save();
			this.sessionService.IsSetHotKey = false;
		}
		private void Base()
		{
			Settings.Default.SystemSetup_Base_AutoStart = this.basicSetting.chkBase_AutoStart.IsChecked.Value;
			Settings.Default.SystemSetup_Base_AutoLogin = this.basicSetting.chkBase_AutoLogin.IsChecked.Value;
			Settings.Default.SystemSetup_Base_ExitHide = this.basicSetting.rdoBase_Exit_Hide.IsChecked.Value;
			AppUtil.Instance.StartAtLogon(AppUtil.FilePath, Settings.Default.SystemSetup_Base_AutoStart);
		}
		private void FileTransport()
		{
			Settings.Default.SystemSetup_FileTransport_SaveDir = this.fileTransmitSetting.txtFileTransportSaveDir.Text;
		}
		private void AutoReply()
		{
			Settings.Default.SystemSetup_AutoReply_Is = this.autoReplySetting.chkAutoReply.IsChecked.Value;
			Settings.Default.SystemSetup_AutoReply_Index = this.autoReplySetting.lbAutoReplyContent.SelectedIndex;
		}
		private void FastReply()
		{
			ComboBoxItem item = this.autoReplySetting.cbxFastReply.SelectedItem as ComboBoxItem;
			if (item != null)
			{
				if (this.autoReplySetting.tbxFastReply.Text.Length == 0)
				{
					Alert.Show("消息不能为空！", "提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
				}
				else
				{
					item.DataContext = this.autoReplySetting.tbxFastReply.Text;
					Settings.Default.SystemSetup_FastReply_Index = this.autoReplySetting.cbxFastReply.SelectedIndex;
				}
			}
		}
		private void MessageNotify()
		{
			Settings.Default.SystemSetup_MessageNotify_MessageBoxShow = this.msgNotifySetting.chkShowMessageBox.IsChecked.Value;
		}
		private void MinHandler(object sender, RoutedEventArgs e)
		{
			base.WindowState = WindowState.Minimized;
		}
		private void CloseHandler(object sender, RoutedEventArgs e)
		{
			base.Close();
		}
		private void DragMoveHandler(object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				base.DragMove();
			}
		}
		private void StateChangeHandler(object sender, RoutedEventArgs e)
		{
			if (e.Source == this.rdoBasic)
			{
				this.fileTransmitSetting.Visibility = Visibility.Collapsed;
				this.autoReplySetting.Visibility = Visibility.Collapsed;
				this.msgNotifySetting.Visibility = Visibility.Collapsed;
				this.hotkeySetting.Visibility = Visibility.Collapsed;
				this.basicSetting.Visibility = Visibility.Visible;
			}
			if (e.Source == this.rdoFile)
			{
				this.autoReplySetting.Visibility = Visibility.Collapsed;
				this.msgNotifySetting.Visibility = Visibility.Collapsed;
				this.hotkeySetting.Visibility = Visibility.Collapsed;
				this.basicSetting.Visibility = Visibility.Collapsed;
				this.fileTransmitSetting.Visibility = Visibility.Visible;
			}
			if (e.Source == this.rdoReply)
			{
				this.fileTransmitSetting.Visibility = Visibility.Collapsed;
				this.msgNotifySetting.Visibility = Visibility.Collapsed;
				this.hotkeySetting.Visibility = Visibility.Collapsed;
				this.basicSetting.Visibility = Visibility.Collapsed;
				this.autoReplySetting.Visibility = Visibility.Visible;
			}
			if (e.Source == this.rdoNotify)
			{
				this.fileTransmitSetting.Visibility = Visibility.Collapsed;
				this.autoReplySetting.Visibility = Visibility.Collapsed;
				this.hotkeySetting.Visibility = Visibility.Collapsed;
				this.basicSetting.Visibility = Visibility.Collapsed;
				this.msgNotifySetting.Visibility = Visibility.Visible;
			}
			if (e.Source == this.rdoHotkey)
			{
				this.fileTransmitSetting.Visibility = Visibility.Collapsed;
				this.autoReplySetting.Visibility = Visibility.Collapsed;
				this.basicSetting.Visibility = Visibility.Collapsed;
				this.msgNotifySetting.Visibility = Visibility.Collapsed;
				this.hotkeySetting.Visibility = Visibility.Visible;
			}
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/view/systemsettingwindow.xaml", UriKind.Relative);
        //        System.Windows.Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[System.Diagnostics.DebuggerNonUserCode]
        //internal System.Delegate _CreateDelegate(System.Type delegateType, string handler)
        //{
        //    return System.Delegate.CreateDelegate(delegateType, this, handler);
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.OuterBorder = (Border)target;
        //        break;
        //    case 2:
        //        this.InnerBorder = (Border)target;
        //        break;
        //    case 3:
        //        this.topBar = (System.Windows.Controls.Primitives.StatusBar)target;
        //        break;
        //    case 4:
        //        this.imgIcon = (Image)target;
        //        break;
        //    case 5:
        //        this.btnMin = (ImageButton)target;
        //        this.btnMin.Click += new RoutedEventHandler(this.btnMin_Click);
        //        break;
        //    case 6:
        //        this.btnClose = (ImageButton)target;
        //        this.btnClose.Click += new RoutedEventHandler(this.btnClose_Click);
        //        break;
        //    case 7:
        //        this.rdoBasic = (System.Windows.Controls.RadioButton)target;
        //        this.rdoBasic.Checked += new RoutedEventHandler(this.StateChangeHandler);
        //        break;
        //    case 8:
        //        this.rdoFile = (System.Windows.Controls.RadioButton)target;
        //        this.rdoFile.Checked += new RoutedEventHandler(this.StateChangeHandler);
        //        break;
        //    case 9:
        //        this.rdoReply = (System.Windows.Controls.RadioButton)target;
        //        this.rdoReply.Checked += new RoutedEventHandler(this.StateChangeHandler);
        //        break;
        //    case 10:
        //        this.rdoNotify = (System.Windows.Controls.RadioButton)target;
        //        this.rdoNotify.Checked += new RoutedEventHandler(this.StateChangeHandler);
        //        break;
        //    case 11:
        //        this.rdoHotkey = (System.Windows.Controls.RadioButton)target;
        //        this.rdoHotkey.Checked += new RoutedEventHandler(this.StateChangeHandler);
        //        break;
        //    case 12:
        //        this.ContentGrid = (Grid)target;
        //        break;
        //    case 13:
        //        this.basicSetting = (BasicSetting)target;
        //        break;
        //    case 14:
        //        this.fileTransmitSetting = (FileTransmitSetting)target;
        //        break;
        //    case 15:
        //        this.autoReplySetting = (AutoReplySetting)target;
        //        break;
        //    case 16:
        //        this.msgNotifySetting = (MessageNotifySetting)target;
        //        break;
        //    case 17:
        //        this.hotkeySetting = (HotkeySetting)target;
        //        break;
        //    case 18:
        //        this.btnOK = (System.Windows.Controls.Button)target;
        //        this.btnOK.Click += new RoutedEventHandler(this.btnOK_Click);
        //        break;
        //    case 19:
        //        this.btnCancel = (System.Windows.Controls.Button)target;
        //        this.btnCancel.Click += new RoutedEventHandler(this.btnClose_Click);
        //        break;
        //    case 20:
        //        this.btnApply = (System.Windows.Controls.Button)target;
        //        this.btnApply.Click += new RoutedEventHandler(this.btnApply_Click);
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
