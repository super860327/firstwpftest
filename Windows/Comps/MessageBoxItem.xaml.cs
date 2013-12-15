using IDKin.IM.Core;
using IDKin.IM.Core.Core;
using IDKin.IM.CustomComponents.Controls;
using IDKin.IM.Data;
using IDKin.IM.Log;
using IDKin.IM.Protocol.Center;
using IDKin.IM.Windows.Comps.ChatTab;
using IDKin.IM.Windows.Model;
using IDKin.IM.Windows.Util;
using IDKin.IM.Windows.View;
using IDKin.IM.Windows.View.AddFriends;
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
namespace IDKin.IM.Windows.Comps
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class MessageBoxItem : UserControl//, IComponentConnector
	{
		private MessageActorType messageType;
		private BaseTab baseTab = null;
		private long id;
		private string projectid;
		private IDataService dataService = ServiceUtil.Instance.DataService;
		private DataModel dataModel = DataModel.Instance;
		private ILogger logger = ServiceUtil.Instance.Logger;
		private INWindow inWindow = null;
		//internal Image imgFace;
		//internal TextBlock tbkName;
		//internal TextBlock tbkCount;
        ////private bool _contentLoaded;
		public MessageActorType MessageType
		{
			get
			{
				return this.messageType;
			}
		}
		public string Projectid
		{
			get
			{
				return this.projectid;
			}
		}
		public long Id
		{
			get
			{
				return this.id;
			}
		}
		public MessageBoxItem(MessageActorType type, long id, string name, string count, ImageSource imageSource)
		{
			this.InitializeComponent();
			this.messageType = type;
			this.id = id;
			this.imgFace.Source = imageSource;
			this.tbkName.Text = name;
			this.tbkCount.Text = "(" + count + ")";
			this.inWindow = (this.dataService.INWindow as INWindow);
		}
		public MessageBoxItem(MessageActorType type, long id, string projectid, string name, string count, ImageSource imageSource)
		{
			this.InitializeComponent();
			this.messageType = type;
			this.id = id;
			this.projectid = projectid;
			this.imgFace.Source = imageSource;
			this.tbkName.Text = name;
			this.tbkCount.Text = "(" + count + ")";
			this.inWindow = (this.dataService.INWindow as INWindow);
		}
		private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.PickUpMessage();
		}
		private void RosterNewMessage(Message msg)
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
				}
			}
			this.baseTab = item;
			FriendsChatTabControl tab = item.TabContent;
			if (tab != null)
			{
				tab.ChatComponent.AddMessageRoster(msg, false);
				tab.ChatComponent.inputMsgBox.Focus();
			}
		}
		private void GroupNewMessage(Message message)
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
				}
			}
			this.baseTab = item;
			GroupChatTabControl tab = item.TabContent;
			if (tab != null)
			{
				tab.ChatComponent.AddMessageGroup(message, false);
				tab.ChatComponent.inputMsgBox.Focus();
			}
		}
		private void CooperationStaffNewMessage(Message message)
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
				}
			}
			this.baseTab = item;
			CoopStaffChatTabControl tab = item.TabContent;
			if (tab != null)
			{
				tab.ChatComponent.AddCooperationMessageStaff(message, false);
				tab.ChatComponent.inputMsgBox.Focus();
			}
		}
		private void StaffNewMessage(Message message)
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
				}
			}
			this.baseTab = item;
			PersonalChatTabControl tab = item.TabContent;
			if (tab != null)
			{
				tab.ChatComponent.AddMessageStaff(message, false);
				tab.ChatComponent.inputMsgBox.Focus();
			}
		}
		private bool PickUpMessageProcessor()
		{
			bool hasSelected = false;
			TabItem tempItem = null;
			ItemCollection ic = this.inWindow.ContentTab.Items;
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
						System.Collections.Generic.List<Message> list = this.dataModel.GetMessage(pctc.StaffId, MessageActorType.EntStaff);
						if (list != null)
						{
							if (tabHeader != null)
							{
								tabHeader.SetFlashingStyle();
							}
							this.ShowStaffNewMessage(pctc.StaffId, list);
						}
						if (!hasSelected)
						{
							hasSelected = this.HasSelectedItem(MessageActorType.CooperationStaff, pctc.StaffId);
							if (hasSelected)
							{
								tempItem = item;
							}
						}
					}
					if (gctc != null)
					{
						System.Collections.Generic.List<Message> list = this.dataModel.GetMessage(gctc.GroupId, MessageActorType.EntGroup);
						if (list != null)
						{
							if (tabHeader != null)
							{
								tabHeader.SetFlashingStyle();
							}
							this.ShowGroupNewMessage(gctc.GroupId, list);
						}
						if (!hasSelected)
						{
							hasSelected = this.HasSelectedItem(MessageActorType.EntGroup, gctc.GroupId);
							if (hasSelected)
							{
								tempItem = item;
							}
						}
					}
					if (fctc != null)
					{
						System.Collections.Generic.List<Message> list = this.dataModel.GetMessage(fctc.RosterId, MessageActorType.Roster);
						if (list != null)
						{
							if (tabHeader != null)
							{
								tabHeader.SetFlashingStyle();
							}
							this.ShowRosterNewMessage(fctc.RosterId, list);
						}
						if (!hasSelected)
						{
							hasSelected = this.HasSelectedItem(MessageActorType.AddRoster, fctc.RosterId);
							if (hasSelected)
							{
								tempItem = item;
							}
						}
					}
					if (coopStaffChatTabControl != null)
					{
						System.Collections.Generic.List<Message> list = this.dataModel.GetCooperationStaffMessage(coopStaffChatTabControl.Uid, coopStaffChatTabControl.Projectid, MessageActorType.CooperationStaff);
						if (list != null)
						{
							if (tabHeader != null)
							{
								tabHeader.SetFlashingStyle();
							}
							this.ShowCooperationStaffNewMessage(coopStaffChatTabControl.Uid, coopStaffChatTabControl.Projectid, list);
						}
						if (!hasSelected)
						{
							hasSelected = this.HasCoopeationSelectedItem(MessageActorType.CooperationStaff, coopStaffChatTabControl.Projectid);
							if (hasSelected)
							{
								tempItem = item;
							}
						}
					}
				}
			}
			if (tempItem != null)
			{
				this.baseTab = (tempItem as BaseTab);
			}
			return hasSelected;
		}
		private bool HasSelectedItem(MessageActorType selecteItemType, long itemId)
		{
			return this.messageType == selecteItemType && itemId == this.id;
		}
		private bool HasCoopeationSelectedItem(MessageActorType selecteItemType, string itemId)
		{
			return this.messageType == selecteItemType && itemId == this.projectid;
		}
		private void ShowAddRosterAskNewMessage(long rosterId, System.Collections.Generic.List<Message> list)
		{
			Message message = list[0];
			if (message != null)
			{
				RosterAddResponse response = message.MessageObject as RosterAddResponse;
				if (response != null)
				{
					this.RosterAddAskNewMessage(response);
					DataModel.Instance.RemoveMessage(response.uid, MessageActorType.AddRosterAsk);
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
				MessageBox.Show(response.user.name + "同意您添加为好友!");
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
			MessageBox.Show(response.user.name + "拒绝您添加为好友!");
		}
		private void RosterAddNewMessage(RosterAddRequest request)
		{
			FriendRequestWindow requestWindow = new FriendRequestWindow(request.uid, request.ruid, request.rjid, request.message, request.user, request.category_id);
			requestWindow.Show();
		}
		private void ShowAddRosterNewMessage(long rosterId, System.Collections.Generic.List<Message> list)
		{
			Message message = list[0];
			if (message != null)
			{
				RosterAddRequest request = message.MessageObject as RosterAddRequest;
				if (request != null)
				{
					FriendRequestWindow requestWindow = new FriendRequestWindow(request.uid, request.ruid, request.rjid, request.message, request.user, request.category_id);
					requestWindow.Show();
				}
				DataModel.Instance.RemoveMessage(rosterId, MessageActorType.AddRoster);
			}
		}
		private void ShowRosterNewMessage(long rosterId, System.Collections.Generic.List<Message> list)
		{
			if (list != null)
			{
				foreach (Message message in list)
				{
					this.RosterNewMessage(message);
				}
				DataModel.Instance.RemoveMessage(rosterId, MessageActorType.Roster);
			}
		}
		private void ShowGroupNewMessage(long groupId, System.Collections.Generic.List<Message> list)
		{
			if (list != null)
			{
				foreach (Message message in list)
				{
					this.GroupNewMessage(message);
				}
				DataModel.Instance.RemoveMessage(groupId, MessageActorType.EntGroup);
			}
		}
		private void ShowCooperationStaffNewMessage(long staffId, string projectid, System.Collections.Generic.List<Message> list)
		{
			if (list != null)
			{
				foreach (Message message in list)
				{
					this.CooperationStaffNewMessage(message);
				}
				this.dataModel.RemoveCooperationStatffMessage(staffId, projectid, MessageActorType.CooperationStaff);
			}
		}
		private void ShowStaffNewMessage(long staffId, System.Collections.Generic.List<Message> list)
		{
			if (list != null)
			{
				foreach (Message message in list)
				{
					this.StaffNewMessage(message);
				}
				this.dataModel.RemoveMessage(staffId, MessageActorType.EntStaff);
			}
		}
		public void PickUpMessage()
		{
			try
			{
				if (!this.PickUpMessageProcessor())
				{
					if (this.messageType == MessageActorType.EntStaff)
					{
						Staff staff = this.dataService.GetStaff(this.id);
						if (staff != null)
						{
							System.Collections.Generic.List<Message> list = this.dataModel.GetMessage(this.id, MessageActorType.EntStaff);
							this.ShowStaffNewMessage(this.id, list);
						}
					}
					if (this.messageType == MessageActorType.EntGroup)
					{
						EntGroup group = this.dataService.GetEntGroup(this.id);
						if (group != null)
						{
							System.Collections.Generic.List<Message> list = this.dataModel.GetMessage(this.id, MessageActorType.EntGroup);
							this.ShowGroupNewMessage(this.id, list);
						}
					}
					if (this.messageType == MessageActorType.Roster)
					{
						Roster roster = this.dataService.GetRoster(this.id);
						if (roster != null)
						{
							System.Collections.Generic.List<Message> list = this.dataModel.GetMessage(this.id, MessageActorType.Roster);
							this.ShowRosterNewMessage(this.id, list);
						}
					}
					if (this.messageType == MessageActorType.AddRoster)
					{
						System.Collections.Generic.List<Message> list = this.dataModel.GetMessage(this.id, MessageActorType.AddRoster);
						if (list != null && list.Count > 0)
						{
							this.ShowAddRosterNewMessage(this.id, list);
						}
					}
					if (this.messageType == MessageActorType.AddRosterAsk)
					{
						System.Collections.Generic.List<Message> list = this.dataModel.GetMessage(this.id, MessageActorType.AddRosterAsk);
						if (list != null && list.Count > 0)
						{
							this.ShowAddRosterAskNewMessage(this.id, list);
						}
					}
					if (this.messageType == MessageActorType.CooperationStaff)
					{
						System.Collections.Generic.List<Message> list = this.dataModel.GetCooperationStaffMessage(this.id, this.projectid, MessageActorType.CooperationStaff);
						if (list != null && list.Count > 0)
						{
							this.ShowCooperationStaffNewMessage(this.id, this.projectid, list);
						}
					}
				}
				this.FlashIconPorcessor();
				MessageBoxWindow mbw = this.dataModel.GetMessageBox();
				if (mbw != null)
				{
					mbw.Refresh();
				}
				this.ActiveInWindow();
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
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
		private void ActiveInWindow()
		{
			INWindow inWindow = this.dataService.INWindow as INWindow;
			if (inWindow != null)
			{
				inWindow.Activate();
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
				this.SelectedTab();
			}
		}
		private void SelectedTab()
		{
			INWindow inWindow = this.dataService.INWindow as INWindow;
			if (this.baseTab != null && inWindow != null)
			{
				inWindow.ContentTab.SelectedItem = this.baseTab;
				this.baseTab.SetDefaultStyle();
			}
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/messageboxitem.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        ((MessageBoxItem)target).MouseLeftButtonDown += new MouseButtonEventHandler(this.UserControl_MouseDown);
        //        break;
        //    case 2:
        //        this.imgFace = (Image)target;
        //        break;
        //    case 3:
        //        this.tbkName = (TextBlock)target;
        //        break;
        //    case 4:
        //        this.tbkCount = (TextBlock)target;
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
