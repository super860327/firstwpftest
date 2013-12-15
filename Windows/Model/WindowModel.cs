using IDKin.IM.Core;
using IDKin.IM.Data;
using IDKin.IM.Log;
using IDKin.IM.Windows.Comps;
using IDKin.IM.Windows.Comps.ChatTab;
using IDKin.IM.Windows.Comps.MessageCenter;
using IDKin.IM.Windows.Util;
using IDKin.IM.Windows.View;
using IDKin.IM.Windows.View.AddFriends;
using IDKin.IM.Windows.View.Pages;
using System;
using System.Collections.Generic;
namespace IDKin.IM.Windows.Model
{
	public class WindowModel
	{
		private ILogger logger = ServiceUtil.Instance.Logger;
		private IDataService dataService = ServiceUtil.Instance.DataService;
		private static WindowModel instance;
		private LogonSettingWindow logonSettingWindow = null;
		private UserProfilePage userProfilePage = null;
		private SearchRosterWindow searchWindow = null;
		private SearchResultWindow resultWindow = null;
		private SystemSettingWindow systemSettingWindow = null;
		private AboutWindow aboutWindow = null;
		private ClosingTipWindow closingTipWindow = null;
		private UpdateWindow updateWindow = null;
		private System.Collections.Generic.Dictionary<long, EntGroupManagerWindow> entGroupManagerWindows = new System.Collections.Generic.Dictionary<long, EntGroupManagerWindow>();
		private EntGroupManagerWindow groupManagerWindow = null;
		private MessageCenterWindow messageCenterWindow = null;
		private MsgRecordPage msgRecordPage = null;
		private OANoticeRecordPage oaRecordPage = null;
		private MarkedTreeView markView = null;
		private MarkedRecordPage markPage = null;
		private RecentContactsPage recentPage = null;
		private OANoticeListView OAtreeview = null;
		public static WindowModel Instance
		{
			get
			{
				if (WindowModel.instance == null)
				{
					WindowModel.instance = new WindowModel();
				}
				return WindowModel.instance;
			}
			set
			{
				WindowModel.instance = value;
			}
		}
		public UserProfilePage UserProfilePage
		{
			get
			{
				if (this.userProfilePage == null)
				{
					this.userProfilePage = new UserProfilePage();
				}
				return this.userProfilePage;
			}
			set
			{
				this.userProfilePage = value;
			}
		}
		public LogonSettingWindow LogonSettingWindow
		{
			get
			{
				if (this.logonSettingWindow == null)
				{
					this.logonSettingWindow = new LogonSettingWindow();
					this.logonSettingWindow.Activate();
				}
				else
				{
					this.logonSettingWindow.Activate();
				}
				return this.logonSettingWindow;
			}
			set
			{
				this.logonSettingWindow = value;
			}
		}
		public SearchRosterWindow SearchWindow
		{
			get
			{
				if (this.searchWindow == null)
				{
					this.searchWindow = new SearchRosterWindow();
					this.searchWindow.Activate();
				}
				else
				{
					this.searchWindow.Activate();
				}
				return this.searchWindow;
			}
			set
			{
				this.searchWindow = value;
			}
		}
		public SearchResultWindow ResultWindow
		{
			get
			{
				if (this.resultWindow == null)
				{
					this.resultWindow = new SearchResultWindow();
					this.searchWindow.Activate();
				}
				else
				{
					this.searchWindow.Activate();
				}
				return this.resultWindow;
			}
			set
			{
				this.resultWindow = value;
			}
		}
		public SystemSettingWindow SystemSettingWindow
		{
			get
			{
				if (this.systemSettingWindow == null)
				{
					this.systemSettingWindow = new SystemSettingWindow();
					this.systemSettingWindow.Activate();
				}
				else
				{
					this.systemSettingWindow.Activate();
				}
				return this.systemSettingWindow;
			}
			set
			{
				this.systemSettingWindow = value;
			}
		}
		public AboutWindow AboutWindow
		{
			get
			{
				if (this.aboutWindow == null)
				{
					this.aboutWindow = new AboutWindow();
					this.aboutWindow.Activate();
				}
				else
				{
					this.aboutWindow.Activate();
				}
				return this.aboutWindow;
			}
			set
			{
				this.aboutWindow = value;
			}
		}
		public ClosingTipWindow ClosingTipWindow
		{
			get
			{
				if (this.closingTipWindow == null)
				{
					this.closingTipWindow = new ClosingTipWindow();
					this.closingTipWindow.Activate();
				}
				else
				{
					this.closingTipWindow.Activate();
				}
				return this.closingTipWindow;
			}
			set
			{
				this.closingTipWindow = value;
			}
		}
		public UpdateWindow UpdateWindow
		{
			get
			{
				if (this.updateWindow == null)
				{
					this.updateWindow = new UpdateWindow();
					this.updateWindow.Activate();
				}
				else
				{
					this.updateWindow.Activate();
				}
				return this.updateWindow;
			}
			set
			{
				this.updateWindow = value;
			}
		}
		public EntGroupManagerWindow GroupManagerWindow
		{
			get
			{
				if (this.groupManagerWindow == null)
				{
					this.groupManagerWindow = new EntGroupManagerWindow();
				}
				this.groupManagerWindow.Activate();
				return this.groupManagerWindow;
			}
			set
			{
				this.groupManagerWindow = value;
			}
		}
		public MessageCenterWindow MessageCenterWindow
		{
			get
			{
				if (this.messageCenterWindow == null)
				{
					this.messageCenterWindow = new MessageCenterWindow();
					this.messageCenterWindow.Activate();
				}
				else
				{
					this.messageCenterWindow.Activate();
				}
				return this.messageCenterWindow;
			}
			set
			{
				this.messageCenterWindow = value;
			}
		}
		public MsgRecordPage MsgRecordPage
		{
			get
			{
				if (this.msgRecordPage == null)
				{
					this.msgRecordPage = new MsgRecordPage();
				}
				return this.msgRecordPage;
			}
			set
			{
				this.msgRecordPage = value;
			}
		}
		public OANoticeRecordPage OARecordPage
		{
			get
			{
				if (this.oaRecordPage == null)
				{
					this.oaRecordPage = new OANoticeRecordPage();
				}
				return this.oaRecordPage;
			}
			set
			{
				this.oaRecordPage = value;
			}
		}
		public MarkedTreeView MarkTreeView
		{
			get
			{
				if (this.markView == null)
				{
					this.markView = new MarkedTreeView();
				}
				return this.markView;
			}
			set
			{
				this.markView = value;
			}
		}
		public MarkedRecordPage Markpage
		{
			get
			{
				if (this.markPage == null)
				{
					this.markPage = new MarkedRecordPage();
				}
				return this.markPage;
			}
			set
			{
				this.markPage = value;
			}
		}
		public RecentContactsPage RecentPage
		{
			get
			{
				if (this.recentPage == null)
				{
					this.recentPage = new RecentContactsPage();
				}
				return this.recentPage;
			}
			set
			{
				this.recentPage = value;
			}
		}
		public OANoticeListView OATreeView
		{
			get
			{
				if (this.OAtreeview == null)
				{
					this.OAtreeview = new OANoticeListView();
				}
				return this.OAtreeview;
			}
			set
			{
				this.OAtreeview = value;
			}
		}
		public UserProfilePage GetUserProfilePage(long uid)
		{
			EntStaffTab staffTab = this.dataService.GetStaffChatTab(uid) as EntStaffTab;
			UserProfilePage result;
			if (staffTab != null)
			{
				result = staffTab.TabContent.ProfilePage;
			}
			else
			{
				result = null;
			}
			return result;
		}
		public AddressComponent getAddress()
		{
			INWindow inWindow = this.dataService.INWindow as INWindow;
			AddressComponent result;
			if (inWindow != null)
			{
				AddressComponent address = new AddressComponent();
				result = address;
			}
			else
			{
				result = null;
			}
			return result;
		}
		public SelfProfilePage GetSelfProfilePage()
		{
			INWindow inWindow = this.dataService.INWindow as INWindow;
			SelfProfilePage result;
			if (inWindow != null)
			{
				result = inWindow.workDesktopControl.SelPage;
			}
			else
			{
				result = null;
			}
			return result;
		}
		public bool IsOpenUpdateWindow()
		{
			return this.updateWindow != null;
		}
		public void CreateOrAtcivateEntGroupManagerWindow(EntGroup entGroup, OperationType type)
		{
			long gid;
			if (entGroup == null)
			{
				gid = -1L;
			}
			else
			{
				gid = entGroup.Gid;
			}
			if (this.entGroupManagerWindows.ContainsKey(gid))
			{
				EntGroupManagerWindow temEntGroupManagerWindow = this.entGroupManagerWindows[gid];
				temEntGroupManagerWindow.Activate();
			}
			else
			{
				if (type == OperationType.Add)
				{
					EntGroupManagerWindow temEntGroupManagerWindow = new EntGroupManagerWindow();
					this.entGroupManagerWindows.Add(gid, temEntGroupManagerWindow);
					temEntGroupManagerWindow.Show();
				}
				else
				{
					if (type == OperationType.Modify)
					{
						if (entGroup == null)
						{
							throw new System.InvalidOperationException("操作类型为Modify时,参数不可为空!");
						}
						EntGroupManagerWindow temEntGroupManagerWindow = new EntGroupManagerWindow(entGroup);
						this.entGroupManagerWindows.Add(gid, temEntGroupManagerWindow);
						temEntGroupManagerWindow.Show();
					}
				}
			}
		}
		public void RemoveEntGroupManagerWindow(long gid)
		{
			if (this.entGroupManagerWindows.ContainsKey(gid))
			{
				this.entGroupManagerWindows.Remove(gid);
			}
		}
		public bool IsOpenMessageCenterWindow()
		{
			return this.messageCenterWindow != null;
		}
		public void ClearAll()
		{
			try
			{
				if (this.resultWindow != null)
				{
					this.resultWindow.Close();
				}
				if (this.searchWindow != null)
				{
					this.searchWindow.Close();
				}
				if (this.systemSettingWindow != null)
				{
					this.systemSettingWindow.Close();
				}
				if (this.aboutWindow != null)
				{
					this.aboutWindow.Close();
				}
				if (this.closingTipWindow != null)
				{
					this.closingTipWindow.Close();
				}
				if (this.updateWindow != null)
				{
					this.updateWindow.Close();
					this.updateWindow = null;
				}
				if (this.groupManagerWindow != null)
				{
					this.groupManagerWindow.Close();
				}
				if (this.messageCenterWindow != null)
				{
					this.messageCenterWindow.Close();
				}
				if (this.systemSettingWindow != null)
				{
					this.systemSettingWindow.Close();
				}
				if (this.logonSettingWindow != null)
				{
					this.logonSettingWindow.Close();
					this.logonSettingWindow = null;
				}
				this.ClearEntGroupManagerWindows();
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		private void ClearEntGroupManagerWindows()
		{
			System.Collections.Generic.List<EntGroupManagerWindow> temEntGroupManagerWindows = new System.Collections.Generic.List<EntGroupManagerWindow>();
			foreach (System.Collections.Generic.KeyValuePair<long, EntGroupManagerWindow> item in this.entGroupManagerWindows)
			{
				temEntGroupManagerWindows.Add(item.Value);
			}
			for (int i = 0; i < temEntGroupManagerWindows.Count; i++)
			{
				temEntGroupManagerWindows[i].Close();
			}
			temEntGroupManagerWindows.Clear();
			this.entGroupManagerWindows.Clear();
		}
	}
}
