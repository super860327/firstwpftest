using IDKin.IM.Core;
using IDKin.IM.CustomComponents.Controls;
using IDKin.IM.Theme.Helper;
using IDKin.IM.Windows.Model;
using IDKin.IM.Windows.Util;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
namespace IDKin.IM.Windows.View.EmailAlert
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class SettingEmailAccountWindow : Window//, IComponentConnector
	{
		private const int TIMESPAN = 1;
		private Storyboard storyboard;
		private string strMsg = string.Empty;
		public static SettingEmailAccountWindow settingEmailAccountWindow;
        //internal Border OuterBorder;
        //internal Border InnerBorder;
        //internal StatusBar topBar;
        //internal Image imgIcon;
        //internal ImageButton btnMin;
        //internal TabControl TabRoot;
        //internal CommonEmailItem CommonTabItem;
        //internal EnterpriseEmailItem EntTabItem;
        //internal EntCustomEmailItem EntCustomTabItem;
        //internal CheckBox cboxOpen;
        //internal ComboBox cboxTime;
        //internal Grid LayoutRoot;
        //internal ScaleTransform SpinnerScale;
        //internal RotateTransform SpinnerRotate;
        //private bool _contentLoaded;
		public static SettingEmailAccountWindow GetSettingEmailAccountWindow
		{
			get
			{
				SettingEmailAccountWindow result;
				if (SettingEmailAccountWindow.settingEmailAccountWindow == null || !SettingEmailAccountWindow.settingEmailAccountWindow.IsLoaded)
				{
					SettingEmailAccountWindow.settingEmailAccountWindow = new SettingEmailAccountWindow();
					SettingEmailAccountWindow.settingEmailAccountWindow.Owner = (ServiceUtil.Instance.DataService.INWindow as INWindow);
					result = SettingEmailAccountWindow.settingEmailAccountWindow;
				}
				else
				{
					result = SettingEmailAccountWindow.settingEmailAccountWindow;
				}
				return result;
			}
		}
		private SettingEmailAccountWindow()
		{
			ThemeSwitcher.LoadSkin(ThemeEnum.Aero, this);
			this.InitializeComponent();
			this.InitialEmail();
			this.AddListenerEvent();
		}
		private void AddListenerEvent()
		{
			base.Loaded += new RoutedEventHandler(this.MailSetting_Loaded);
			this.CommonTabItem.BeginStory += new System.EventHandler(this.CommonTabItem_BeginStory);
			this.CommonTabItem.StopStory += new System.EventHandler(this.CommonTabItem_StopStory);
			this.EntCustomTabItem.BeginStory += new System.EventHandler(this.CommonTabItem_BeginStory);
			this.EntCustomTabItem.StopStory += new System.EventHandler(this.CommonTabItem_StopStory);
			this.EntTabItem.BeginStory += new System.EventHandler(this.CommonTabItem_BeginStory);
			this.EntTabItem.StopStory += new System.EventHandler(this.CommonTabItem_StopStory);
		}
		private void CommonTabItem_StopStory(object sender, System.EventArgs e)
		{
			if (this.storyboard != null)
			{
                //base.Dispatcher.BeginInvoke(delegate
                //{
                //    this.storyboard.Stop();
                //    this.LayoutRoot.Visibility = Visibility.Hidden;
                //}, new object[0]);
			}
		}
		private void CommonTabItem_BeginStory(object sender, System.EventArgs e)
		{
			this.storyboard = (base.FindResource("tt") as Storyboard);
			if (this.storyboard != null)
			{
				this.storyboard.Begin();
				this.LayoutRoot.Visibility = Visibility.Visible;
			}
		}
		private void MailSetting_Loaded(object sender, RoutedEventArgs e)
		{
			this.InitialTimeSpan();
		}
		private void InitialTimeSpan()
		{
			this.cboxTime.ItemsSource = new System.Collections.Generic.List<ValueText>
			{
				new ValueText
				{
					Text = "5分钟",
					Value = 5
				},
				new ValueText
				{
					Text = "30分钟",
					Value = 30
				},
				new ValueText
				{
					Text = "1小时",
					Value = 60
				}
			};
			System.Collections.Generic.List<EmailServerInfo> list = new System.Collections.Generic.List<EmailServerInfo>();
			list.Add(new EmailServerInfo
			{
				URL = "http://mail.sunupcg.cn:81/default.asp",
				Server = "mail.sunupcg.cn",
				Text = "@sunupcg.com"
			});
			list.Add(new EmailServerInfo
			{
				URL = "http://webmail.idccenter.net",
				Server = "pop.idccenter.net",
				Text = "@jbcidu.com"
			});
			list.Add(new EmailServerInfo
			{
				URL = "http://mail.cn.yahoo.com",
				Server = "pop.mail.yahoo.com",
				Text = "@yahoo.com.cn"
			});
		}
		private void txtPwd_KeyDown(object sender, KeyEventArgs e)
		{
		}
		private void InitialEmail()
		{
			if (DataModel.Instance.EmailList != null && DataModel.Instance.EmailList.Count > 0)
			{
				for (int i = 0; i < DataModel.Instance.EmailList.Count; i++)
				{
				}
			}
		}
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonDown(e);
			base.DragMove();
		}
		private void btnMin_Click(object sender, RoutedEventArgs e)
		{
			base.WindowState = WindowState.Minimized;
		}
		private void CloseWindowHandler(object sender, RoutedEventArgs e)
		{
			base.Close();
			this.SaveEmailModals();
		}
		public void SaveEmailModals()
		{
			System.Collections.Generic.List<EMailModal> modals = DataModel.Instance.EmailList;
			if (modals != null && modals.Count != 0 && ServiceUtil.Instance.SessionService != null && !string.IsNullOrEmpty(ServiceUtil.Instance.SessionService.DirLocalData))
			{
				using (System.IO.FileStream write = new System.IO.FileStream(System.IO.Path.Combine(ServiceUtil.Instance.SessionService.DirLocalData, "email.email"), System.IO.FileMode.Create))
				{
					System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
					foreach (EMailModal i in modals)
					{
						formatter.Serialize(write, i);
					}
				}
			}
		}
		private void btnOpen_Click(object sender, RoutedEventArgs e)
		{
			this.Open();
		}
		private void Open()
		{
		}
		private void item_ItemDelete(object sender, System.EventArgs e)
		{
		}
		private void CallBack(System.IAsyncResult ar)
		{
		}
		private void tt(string mailId)
		{
		}
		private void GetMailInfo2(string mailId)
		{
			try
			{
			}
			catch (System.Exception ex)
			{
				ServiceUtil.Instance.Logger.Error(ex.ToString());
			}
		}
		private void InitialModel(string mailId)
		{
		}
		private System.DateTime TestConnectAndReturnDateTime(EMailModal modal)
		{
			return System.DateTime.Now;
		}
		private void EndVoke(System.IAsyncResult ar)
		{
			Func<EMailModal, System.DateTime> f = ar.AsyncState as Func<EMailModal, System.DateTime>;
			if (f != null)
			{
                //base.Dispatcher.BeginInvoke(delegate
                //{
                //    if (string.IsNullOrEmpty(this.strMsg))
                //    {
                //    }
                //}, new object[0]);
                //if (this.storyboard != null)
                //{
                //    base.Dispatcher.BeginInvoke(delegate
                //    {
                //        this.storyboard.Stop();
                //        this.LayoutRoot.Visibility = Visibility.Hidden;
                //    }, new object[0]);
                //}
			}
		}
		private void EndVoke2(System.IAsyncResult ar)
		{
			Func<string, string, string, System.DateTime> f = ar.AsyncState as Func<string, string, string, System.DateTime>;
			if (f != null)
			{
				System.DateTime str = f.EndInvoke(ar);
                //base.Dispatcher.BeginInvoke(delegate
                //{
                //}, new object[0]);
			}
		}
		private bool IsExist(string mailID)
		{
			return false;
		}
		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			this.TextChanged();
		}
		private void TextChanged()
		{
		}
		private void txtPwd_PasswordChanged(object sender, RoutedEventArgs e)
		{
			this.TextChanged();
		}
		private void lstMailID_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
		}
		private void Delete(ListBoxItem item)
		{
			if (item != null)
			{
				EMailModal model = item.DataContext as EMailModal;
				if (model != null && DataModel.Instance.EmailList.Contains(model))
				{
				}
			}
		}
		private void button1_Click(object sender, RoutedEventArgs e)
		{
			if (DataModel.Instance.EmailList.Count > 0)
			{
				Setting s = new Setting(DataModel.Instance.EmailList[DataModel.Instance.EmailList.Count - 1]);
				s.GetNewMailCountLoop();
			}
		}
		private void cboxServer_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.TextChanged();
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/view/emailalert/settingemailaccountwindow.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
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
        //        this.topBar = (StatusBar)target;
        //        break;
        //    case 4:
        //        this.imgIcon = (Image)target;
        //        break;
        //    case 5:
        //        this.btnMin = (ImageButton)target;
        //        this.btnMin.Click += new RoutedEventHandler(this.btnMin_Click);
        //        break;
        //    case 6:
        //        ((ImageButton)target).Click += new RoutedEventHandler(this.CloseWindowHandler);
        //        break;
        //    case 7:
        //        this.TabRoot = (TabControl)target;
        //        break;
        //    case 8:
        //        this.CommonTabItem = (CommonEmailItem)target;
        //        break;
        //    case 9:
        //        this.EntTabItem = (EnterpriseEmailItem)target;
        //        break;
        //    case 10:
        //        this.EntCustomTabItem = (EntCustomEmailItem)target;
        //        break;
        //    case 11:
        //        this.cboxOpen = (CheckBox)target;
        //        break;
        //    case 12:
        //        this.cboxTime = (ComboBox)target;
        //        break;
        //    case 13:
        //        ((Button)target).Click += new RoutedEventHandler(this.CloseWindowHandler);
        //        break;
        //    case 14:
        //        this.LayoutRoot = (Grid)target;
        //        break;
        //    case 15:
        //        this.SpinnerScale = (ScaleTransform)target;
        //        break;
        //    case 16:
        //        this.SpinnerRotate = (RotateTransform)target;
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
