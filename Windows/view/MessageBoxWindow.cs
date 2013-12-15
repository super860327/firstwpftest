using IDKin.IM.Core;
using IDKin.IM.CustomComponents.Controls;
using IDKin.IM.Data;
using IDKin.IM.ImageService;
using IDKin.IM.Log;
using IDKin.IM.Protocol.Center;
using IDKin.IM.Windows.Comps;
using IDKin.IM.Windows.Comps.ChatTab;
using IDKin.IM.Windows.Model;
using IDKin.IM.Windows.Util;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
namespace IDKin.IM.Windows.View
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class MessageBoxWindow : DesktopAlertBase//, IComponentConnector
	{
		private const string ROSTER_ADD_MESSAGE = "验证消息";
		private System.Collections.Hashtable ListBoxItems = new System.Collections.Hashtable();
		private IImageService imageService = ServiceUtil.Instance.ImageService;
		private ISessionService sessionService = ServiceUtil.Instance.SessionService;
		private IDataService dataService = ServiceUtil.Instance.DataService;
		private ILogger logger = ServiceUtil.Instance.Logger;
		private DataModel dataModel = DataModel.Instance;
        //internal ListView lbMessageBox;
        //internal TextBlock tbkClose;
        //internal TextBlock tbkPickUp;
        //private bool _contentLoaded;
		public MessageBoxWindow()
		{
			try
			{
				this.InitializeComponent();
				base.Left = SystemParameters.WorkArea.Width - base.Width - 5.0;
				base.Top = SystemParameters.WorkArea.Height - 47.0 - 30.0;
				this.InitEventHandler();
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		private void InitEventHandler()
		{
			this.tbkClose.MouseLeftButtonDown += new MouseButtonEventHandler(this.CancelFlickerHandler);
			this.tbkPickUp.MouseLeftButtonDown += new MouseButtonEventHandler(this.PickUpAllHandler);
			base.Closing += new CancelEventHandler(this.Window_Closing);
		}
		private void Window_Closing(object sender, CancelEventArgs e)
		{
			try
			{
				this.lbMessageBox.Items.Clear();
				DataModel.Instance.RemoveMessageBox();
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		public void Refresh()
		{
			try
			{
				this.lbMessageBox.Items.Clear();
				if (!this.dataModel.HasMessage())
				{
					NotifyIconUtil.Instance.SetFlashIcon(FlashIconType.Default);
					base.Close();
				}
				foreach (string key in this.dataModel.MessageBoxSort)
				{
					if (!string.IsNullOrEmpty(key))
					{
						System.Collections.Generic.List<Message> list = this.dataModel.GetMessage(key);
						if (list != null && list.Count > 0)
						{
							Message message = list[0];
							if (message != null)
							{
								if (message.MessageObjectType == MessageActorType.EntStaff)
								{
									Staff staff = this.dataService.GetStaff((long)((ulong)Jid.GetUid(message.FromJid)));
									if (staff != null)
									{
										MessageBoxItem mbi = new MessageBoxItem(MessageActorType.EntStaff, staff.Uid, staff.Name, list.Count.ToString(), staff.HeaderImage);
										this.lbMessageBox.Items.Add(mbi);
									}
								}
								if (message.MessageObjectType == MessageActorType.EntGroup)
								{
									EntGroup group = this.dataService.GetEntGroup(message.Gid);
									if (group != null)
									{
										MessageBoxItem mbi = new MessageBoxItem(MessageActorType.EntGroup, group.Gid, group.Name, list.Count.ToString(), this.imageService.GetIcon(ImageTypeIcon.Group));
										this.lbMessageBox.Items.Add(mbi);
									}
								}
								if (message.MessageObjectType == MessageActorType.Roster)
								{
									Roster roster = this.dataService.GetRoster((long)((ulong)Jid.GetUid(message.FromJid)));
									if (roster != null)
									{
										MessageBoxItem mbi = new MessageBoxItem(MessageActorType.Roster, roster.Uid, roster.Name, list.Count.ToString(), roster.HeaderImage);
										this.lbMessageBox.Items.Add(mbi);
									}
								}
								if (message.MessageObjectType == MessageActorType.AddRoster)
								{
									RosterAddRequest request = message.MessageObject as RosterAddRequest;
									if (request != null && request.user != null)
									{
										MessageBoxItem mbi = new MessageBoxItem(MessageActorType.AddRoster, request.uid, "验证消息", list.Count.ToString(), null);
										this.lbMessageBox.Items.Add(mbi);
									}
								}
								if (message.MessageObjectType == MessageActorType.AddRosterAsk)
								{
									RosterAddResponse response = message.MessageObject as RosterAddResponse;
									if (response != null && response.user != null)
									{
										MessageBoxItem mbi = new MessageBoxItem(MessageActorType.AddRosterAsk, response.uid, "验证消息", list.Count.ToString(), null);
										this.lbMessageBox.Items.Add(mbi);
									}
								}
								if (message.MessageObjectType == MessageActorType.CooperationStaff)
								{
									CooperationStaff staff2 = this.dataService.GetCooperationStaff((long)((ulong)Jid.GetUid(message.FromJid)), message.ProjectId);
									if (staff2 != null)
									{
										MessageBoxItem mbi = new MessageBoxItem(MessageActorType.CooperationStaff, staff2.Uid, staff2.UnitedProjectid, staff2.Name, list.Count.ToString(), staff2.HeaderImage);
										this.lbMessageBox.Items.Add(mbi);
									}
								}
							}
						}
					}
				}
				base.Top = SystemParameters.WorkArea.Height - 47.0 - (double)(this.lbMessageBox.Items.Count * 30);
			}
			catch (System.Exception e)
			{
				this.logger.Error(e.ToString());
			}
		}
		public void RemoveItemByUserID(long userid)
		{
			try
			{
				if (this.ListBoxItems.ContainsKey(userid))
				{
					ListBoxItem lbi = this.ListBoxItems[userid] as ListBoxItem;
					this.lbMessageBox.Items.Remove(lbi);
					this.ListBoxItems.Remove(lbi);
				}
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		private ImageSource GetIcon(string type, int sex = -1)
		{
			return null;
		}
		private void PickUpMessage(string type, long id)
		{
			if (type != null && !type.Equals("") && id != 0L)
			{
				this.Refresh();
			}
		}
		private void btnClose_Click(object sender, RoutedEventArgs e)
		{
			base.Close();
		}
		private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
		{
			try
			{
				MessageBoxItem stackPanel = sender as MessageBoxItem;
				if (stackPanel != null)
				{
					string typeID = stackPanel.DataContext as string;
					char[] reg = new char[]
					{
						';'
					};
					string[] s = typeID.Split(reg);
					if (s.Length == 2)
					{
						string type = s[0];
						long id = long.Parse(s[1]);
						this.PickUpMessage(type, id);
					}
				}
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		private void PickUpAllHandler(object sender, MouseButtonEventArgs e)
		{
			try
			{
				MessageBoxItem[] stackPanels = new MessageBoxItem[this.lbMessageBox.Items.Count];
				this.lbMessageBox.Items.CopyTo(stackPanels, 0);
				MessageBoxItem[] array = stackPanels;
				for (int i = 0; i < array.Length; i++)
				{
					MessageBoxItem mbi = array[i];
					mbi.PickUpMessage();
				}
				array = stackPanels;
				for (int i = 0; i < array.Length; i++)
				{
					MessageBoxItem mbi = array[i];
					BaseTab baseTab = this.FindChatTab(mbi.MessageType, mbi.Id, mbi.Projectid);
					if (baseTab != null)
					{
						baseTab.SetFlashingStyle();
					}
				}
				INWindow inWindow = this.dataService.INWindow as INWindow;
				BaseTab selectedTab = inWindow.ContentTab.SelectedItem as BaseTab;
				if (selectedTab != null)
				{
					selectedTab.SetDefaultStyle();
				}
				this.ActiveInWindow();
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		private void CancelFlickerHandler(object sender, MouseButtonEventArgs e)
		{
			base.Close();
		}
		private BaseTab FindChatTab(MessageActorType type, long id, string projectid)
		{
			INWindow inWindow = this.dataService.INWindow as INWindow;
			BaseTab result;
			if (inWindow != null)
			{
				ItemCollection ic = inWindow.ContentTab.Items;
				foreach (TabItem item in (System.Collections.IEnumerable)ic)
				{
					if (item != null)
					{
						BaseTab cti = item as BaseTab;
						if (MessageActorType.EntGroup == type)
						{
							GroupChatTabControl gctc = item.Content as GroupChatTabControl;
							if (gctc != null && gctc.GroupId == id)
							{
								result = cti;
								return result;
							}
						}
						if (MessageActorType.EntStaff == type)
						{
							PersonalChatTabControl pctc = item.Content as PersonalChatTabControl;
							if (pctc != null && pctc.StaffId == id)
							{
								result = cti;
								return result;
							}
						}
						if (MessageActorType.Roster == type)
						{
							FriendsChatTabControl fctc = item.Content as FriendsChatTabControl;
							if (fctc != null && fctc.RosterId == id)
							{
								result = cti;
								return result;
							}
						}
						if (MessageActorType.CooperationStaff == type)
						{
							CoopStaffChatTabControl coopStaffChatTabControl = item.Content as CoopStaffChatTabControl;
							if (coopStaffChatTabControl != null && coopStaffChatTabControl.Uid == id && coopStaffChatTabControl.Projectid == projectid)
							{
								result = cti;
								return result;
							}
						}
					}
				}
			}
			result = null;
			return result;
		}
		private void ActiveInWindow()
		{
			INWindow inWindow = this.dataService.INWindow as INWindow;
			if (inWindow != null)
			{
				if (inWindow.WindowState == WindowState.Maximized)
				{
					inWindow.WindowState = WindowState.Maximized;
				}
				else
				{
					inWindow.WindowState = WindowState.Normal;
					if (inWindow.Top <= 0.0 || inWindow.Left <= 0.0 || inWindow.Left + inWindow.Width >= SystemParameters.WorkArea.Width)
					{
						inWindow.Left = (SystemParameters.WorkArea.Width - inWindow.Width) / 2.0;
						inWindow.Top = (SystemParameters.WorkArea.Height - inWindow.Height) / 2.0;
					}
				}
				inWindow.Show();
				inWindow.Activate();
			}
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/view/messageboxwindow.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.lbMessageBox = (ListView)target;
        //        break;
        //    case 2:
        //        this.tbkClose = (TextBlock)target;
        //        break;
        //    case 3:
        //        this.tbkPickUp = (TextBlock)target;
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
