using IDKin.IM.Core;
using IDKin.IM.Data;
using IDKin.IM.ImageService;
using IDKin.IM.Log;
using IDKin.IM.Windows.Comps.ChatTab;
using IDKin.IM.Windows.Model;
using IDKin.IM.Windows.Properties;
using IDKin.IM.Windows.Util;
using IDKin.IM.Windows.View;
using IDKin.IM.Windows.ViewModel;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
namespace IDKin.IM.Windows.Comps
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class ItemGroup : ListViewItem//, IComponentConnector
	{
		private IImageService imageService = ServiceUtil.Instance.ImageService;
		private ILogger logger = ServiceUtil.Instance.Logger;
		private ChatTabControlViewModel viewModel = new ChatTabControlViewModel();
		private bool isShield = false;
		private ItemGroupViewModel igViewModel = new ItemGroupViewModel();
		private ISessionService sessionService = null;
		private IDataService dataService = null;
		private EntGroup group = null;
		private System.Collections.Generic.List<Message> messageCache = null;
		//internal Image imgGroupHead;
		//internal TextBlock tbkName;
		//internal MenuItem ShieldMenuItem;
        ////private bool _contentLoaded;
		public long Gid
		{
			get
			{
				long result;
				if (this.group != null)
				{
					result = this.group.Gid;
				}
				else
				{
					result = 0L;
				}
				return result;
			}
		}
		public bool IsShield
		{
			get
			{
				return this.isShield;
			}
			set
			{
				this.isShield = value;
			}
		}
		public ItemGroup(EntGroup group)
		{
			this.InitializeComponent();
			this.group = group;
			this.AddEventListenerHandler();
			this.InitService();
			this.InitData();
		}
		private void InitData()
		{
			this.tbkName.Text = string.Concat(new object[]
			{
				this.group.Name,
				"(",
				this.group.Member.Length,
				")"
			});
			string groups = Settings.Default.ShieldGroups;
			if (groups.IndexOf("#" + this.group.Gid + ";") != -1)
			{
				this.isShield = true;
			}
			this.ShieldMenuItem.Header = (this.isShield ? "取消屏蔽" : "屏蔽消息");
		}
		private void InitService()
		{
			this.sessionService = ServiceUtil.Instance.SessionService;
			this.dataService = ServiceUtil.Instance.DataService;
		}
		public void AddMessageCache(Message message)
		{
			if (this.messageCache == null)
			{
				this.messageCache = new System.Collections.Generic.List<Message>();
			}
			this.messageCache.Add(message);
		}
		private void ClearMessageCache()
		{
			if (this.messageCache != null)
			{
				this.messageCache.Clear();
			}
		}
		public void ChangeGroupName(string name, bool blen)
		{
			if (blen)
			{
				this.tbkName.Text = string.Concat(new object[]
				{
					name,
					"(",
					this.group.Member.Length,
					")"
				});
			}
			else
			{
				this.tbkName.Text = name;
			}
		}
		public void UpdateShieldCount()
		{
			if (this.messageCache != null)
			{
				if (this.messageCache != null && this.messageCache.Count > 0)
				{
					this.tbkName.Text = string.Concat(new object[]
					{
						this.group.Name,
						"(",
						this.group.Member.Length,
						")未读消息:",
						this.messageCache.Count
					});
				}
				else
				{
					this.ChangeGroupName(this.group.Name, true);
				}
			}
		}
		private void AddEventListenerHandler()
		{
			base.MouseDoubleClick += new MouseButtonEventHandler(this.MouseDoubleClickHandler);
			base.MouseLeftButtonDown += new MouseButtonEventHandler(this.MouseLeftButtonDownHandler);
			base.Loaded += delegate(object sender, RoutedEventArgs e)
			{
				this.imgGroupHead.Source = this.imageService.GetIcon(ImageTypeIcon.Group);
			};
			base.Unloaded += delegate(object sender, RoutedEventArgs e)
			{
				this.imageService.RemoveIcon(ImageTypeIcon.Group);
			};
		}
		private void MouseDoubleClickHandler(object sender, MouseButtonEventArgs e)
		{
			try
			{
				if (WindowModel.Instance.IsOpenMessageCenterWindow())
				{
				}
				if (e.LeftButton == MouseButtonState.Pressed)
				{
					EntGroupTab item = this.dataService.GetEntGroupChatTab(this.group.Gid) as EntGroupTab;
					if (item != null)
					{
						((INWindow)this.dataService.INWindow).ContentTab.SelectedItem = item;
						GroupChatTabControl chat = item.TabContent;
						if (chat != null)
						{
							this.WatchMessage(chat);
							this.WatchShieldMessage(chat);
							this.UpdateShieldCount();
						}
					}
					else
					{
						item = new EntGroupTab(this.group);
						item.SetDefaultStyle();
						((INWindow)this.dataService.INWindow).ContentTab.Items.Add(item);
						this.dataService.AddEntGroupChatTab(this.group.Gid, item);
						((INWindow)this.dataService.INWindow).ContentTab.SelectedItem = item;
						this.WatchMessage(item.TabContent);
						this.WatchShieldMessage(item.TabContent);
						this.UpdateShieldCount();
					}
				}
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		private void WatchMessage(GroupChatTabControl chatTab)
		{
			System.Collections.Generic.List<Message> list = DataModel.Instance.GetMessage(this.group.Gid, MessageActorType.EntGroup);
			if (list != null)
			{
				foreach (Message message in list)
				{
					if (message != null)
					{
						chatTab.ChatComponent.AddMessageGroup(message, false);
					}
				}
				DataModel.Instance.RemoveMessage(this.group.Gid, MessageActorType.EntGroup);
				MessageBoxWindow mbw = DataModel.Instance.GetMessageBox();
				if (mbw != null)
				{
					mbw.Refresh();
				}
				this.FlashIconPorcessor();
			}
		}
		private void FlashIconPorcessor()
		{
			System.Collections.Generic.List<Message> list2 = DataModel.Instance.GetLastMessage();
			if (list2 != null && list2.Count > 0)
			{
				Message message2 = list2[0];
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
				}
			}
			else
			{
				NotifyIconUtil.Instance.SetFlashIcon(FlashIconType.Default);
			}
		}
		private void WatchShieldMessage(GroupChatTabControl chatTab)
		{
			if (chatTab != null && this.messageCache != null && this.messageCache.Count > 0)
			{
				foreach (Message message in this.messageCache)
				{
					if (message != null)
					{
						chatTab.ChatComponent.AddMessageGroup(message, false);
					}
				}
				this.ClearMessageCache();
			}
		}
		private void MouseLeftButtonDownHandler(object sender, MouseButtonEventArgs e)
		{
			base.Background = new SolidColorBrush(Color.FromRgb(234, 250, 227));
		}
		private void GroupMemberManager_Click(object sender, RoutedEventArgs e)
		{
			if (this.group.IsAdmin(this.sessionService.Uid))
			{
				this.ShowManagerWindow();
			}
			else
			{
				MessageBox.Show("你不是群管理员,不能对成员进行管理！", "提示", MessageBoxButton.OK, MessageBoxImage.Exclamation);
			}
		}
		private void GroupExit_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (this.group.IsAdmin(this.sessionService.Uid))
				{
					MessageBox.Show("你是管理员,不能退出群！", "提示");
				}
				else
				{
					if (MessageBox.Show("你确定要退出该群吗？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
					{
						this.igViewModel.Exit(this.sessionService.Uid, this.group.Gid);
					}
				}
			}
			catch (System.Exception ex)
			{
				ServiceUtil.Instance.Logger.Error(ex.ToString());
			}
		}
		private void GroupDelete_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (!this.group.IsAdmin(this.sessionService.Uid))
				{
					MessageBox.Show("你不是群管理员,不能解散该群！", "提示");
				}
				else
				{
					if (MessageBox.Show("你确定要解散该群吗？", "提示", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
					{
						this.igViewModel.Delete(this.group.Gid);
					}
				}
			}
			catch (System.Exception ex)
			{
				ServiceUtil.Instance.Logger.Error(ex.ToString());
			}
		}
		private void ShieldMessage_Click(object sender, RoutedEventArgs e)
		{
			if (!this.isShield)
			{
				this.ShieldMenuItem.Header = "取消屏蔽";
				this.SaveShieldStatus();
			}
			else
			{
				this.ShieldMenuItem.Header = "屏蔽消息";
				this.CancelShileStatus();
			}
			this.isShield = !this.isShield;
		}
		private void SaveShieldStatus()
		{
			string groups = Settings.Default.ShieldGroups;
			groups = string.Concat(new object[]
			{
				groups,
				"#",
				this.group.Gid,
				";"
			});
			Settings.Default.ShieldGroups = groups;
			Settings.Default.Save();
		}
		private void CancelShileStatus()
		{
			string groups = Settings.Default.ShieldGroups;
			groups = groups.Replace("#" + this.group.Gid + ";", string.Empty);
			Settings.Default.ShieldGroups = groups;
			Settings.Default.Save();
		}
		private void ShowManagerWindow()
		{
			WindowModel.Instance.CreateOrAtcivateEntGroupManagerWindow(this.group, OperationType.Modify);
		}
		private EntGroupManagerWindow GetManagerWindow(bool create = false)
		{
			EntGroupManagerWindow window;
			if (create)
			{
				window = new EntGroupManagerWindow();
			}
			else
			{
				window = new EntGroupManagerWindow(this.group);
			}
			return window;
		}
		private void Item_CloseTab(object sender, RoutedEventArgs e)
		{
			TabItem tabItem = e.Source as TabItem;
			if (tabItem != null)
			{
				TabControl tabControl = tabItem.Parent as TabControl;
				if (tabControl != null)
				{
					tabControl.Items.Remove(tabItem);
					this.dataService.RemoveEntGroupChatTab(this.group.Gid);
				}
			}
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/itemgroup.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.imgGroupHead = (Image)target;
        //        break;
        //    case 2:
        //        this.tbkName = (TextBlock)target;
        //        break;
        //    case 3:
        //        this.ShieldMenuItem = (MenuItem)target;
        //        this.ShieldMenuItem.Click += new RoutedEventHandler(this.ShieldMessage_Click);
        //        break;
        //    case 4:
        //        ((MenuItem)target).Click += new RoutedEventHandler(this.GroupMemberManager_Click);
        //        break;
        //    case 5:
        //        ((MenuItem)target).Click += new RoutedEventHandler(this.GroupExit_Click);
        //        break;
        //    case 6:
        //        ((MenuItem)target).Click += new RoutedEventHandler(this.GroupDelete_Click);
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
