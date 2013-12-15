using IDKin.IM.Communicate;
using IDKin.IM.Core;
using IDKin.IM.Data;
using IDKin.IM.Protocol.Commons;
using IDKin.IM.Windows.Model;
using IDKin.IM.Windows.Model.MessageCenter;
using IDKin.IM.Windows.Util;
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
namespace IDKin.IM.Windows.Comps.MessageCenter
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public partial class RecentContactsItem : ListViewItem, System.IDisposable//, IComponentConnector
	{
		private ChatTabControlViewModel viewModel = new ChatTabControlViewModel();
		private IDataService dataService = null;
		private ISessionService sessionService = ServiceUtil.Instance.SessionService;
		private IConnection connection = ServiceUtil.Instance.Connection;
		private IUtilService utilService = ServiceUtil.Instance.utilService;
		private System.Collections.Generic.ICollection<Staff> liststaff = null;
		private Staff staff = null;
		private EntGroup gp = null;
		private System.Collections.Generic.ICollection<EntGroup> groups = null;
		public bool IsValue = true;
		//internal Image imgHead;
		//internal TextBlock tbkName;
		//internal TextBlock tbkID;
		//internal TextBlock tbkDate;
        ////private bool _contentLoaded;
		public RecentContactsItem(MessageCenterChatInfo info, bool InitGroup, bool All)
		{
			if (info != null)
			{
				this.InitializeComponent();
				this.InitServer();
				if (All)
				{
					if (this.InitData(info))
					{
						this.IsValue = true;
					}
					else
					{
						this.IsValue = false;
					}
				}
				else
				{
					if (InitGroup)
					{
						if (this.InitGroupInfo(info))
						{
							this.IsValue = true;
						}
						else
						{
							this.IsValue = false;
						}
					}
					else
					{
						if (this.InitStaffInfo(info))
						{
							this.IsValue = true;
						}
						else
						{
							this.IsValue = false;
						}
					}
				}
				this.AddEventListener();
			}
		}
		public void InitServer()
		{
			this.dataService = ServiceUtil.Instance.DataService;
			this.groups = this.dataService.GetEntGroupList();
			this.liststaff = this.dataService.GetStaffList();
		}
		private bool InitStaffInfo(MessageCenterChatInfo info)
		{
			bool result;
			if (info.Type == MessageCenterType.EntStaff)
			{
				Staff staf = this.dataService.GetStaff(info.Id);
				if (staf != null && staf.HeaderImage != null)
				{
					this.tbkID.Text = info.Id.ToString();
					this.tbkName.Text = staf.Name;
					this.tbkDate.Text = info.LastTime;
					this.imgHead.Source = staf.HeaderImageOnline;
					this.staff = staf;
					result = true;
				}
				else
				{
					result = false;
				}
			}
			else
			{
				result = false;
			}
			return result;
		}
		private bool InitGroupInfo(MessageCenterChatInfo info)
		{
			bool result;
			if (info.Type == MessageCenterType.EntGroup)
			{
				EntGroup group = this.dataService.GetEntGroup(info.Id);
				if (group != null)
				{
					this.tbkID.Text = info.Id.ToString();
					this.tbkName.Text = group.Name;
					this.tbkDate.Text = info.LastTime;
					this.imgHead.Source = group.AdminIcon;
					this.gp = group;
					result = true;
				}
				else
				{
					result = false;
				}
			}
			else
			{
				result = false;
			}
			return result;
		}
		public bool InitData(MessageCenterChatInfo info)
		{
			bool result;
			if (info.Type == MessageCenterType.EntStaff)
			{
				result = this.StaffHeadInfo(info);
			}
			else
			{
				result = this.GroupHeadInfo(info);
			}
			return result;
		}
		private bool GroupHeadInfo(MessageCenterChatInfo info)
		{
			EntGroup group = this.dataService.GetEntGroup(info.Id);
			bool result;
			if (group != null)
			{
				this.tbkID.Text = info.Id.ToString();
				this.tbkName.Text = group.Name;
				this.tbkDate.Text = info.LastTime;
				this.imgHead.Source = group.AdminIcon;
				this.gp = group;
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}
		private bool StaffHeadInfo(MessageCenterChatInfo info)
		{
			Staff staff = this.dataService.GetStaff(info.Id);
			bool result;
			if (staff != null && staff.HeaderImage != null)
			{
				this.tbkID.Text = info.Id.ToString();
				this.tbkName.Text = staff.Name;
				this.tbkDate.Text = info.LastTime;
				this.imgHead.Source = staff.HeaderImageOnline;
				this.staff = staff;
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}
		private void AddEventListener()
		{
			this.viewModel.AddEventListenerHandler();
			base.MouseDoubleClick += new MouseButtonEventHandler(this.RecentContactsItem_MouseDoubleClick);
		}
		public void RecentContactsItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (this.staff != null)
			{
				if (this.staff != null)
				{
					WindowModel.Instance.MessageCenterWindow.workTreeView.NodeExpand(this.staff.Uid, true);
					this.viewModel.SendStaffMessageRecord(this.sessionService.Uid, this.staff.Uid, "0", 1, 10, MessageRecordType.MESSAGE_CENTER_RECORD);
					WindowModel.Instance.MsgRecordPage.trgMessageTable.Rows.Clear();
					WindowModel.Instance.MsgRecordPage.InitData(this.staff.Uid, false, true, 0);
					WindowModel.Instance.MsgRecordPage.InitTitleName("与" + this.staff.Name + "的消息记录");
					WindowModel.Instance.MessageCenterWindow.ViewMsgFrame.NavigationService.Navigate(WindowModel.Instance.MsgRecordPage);
					System.GC.Collect();
				}
			}
			else
			{
				WindowModel.Instance.MessageCenterWindow.workTreeView.NodeExpand(this.gp.Gid, false);
				this.viewModel.SendGroupMessageRecord(this.gp.Gid, "0", 1, 10, this.sessionService.Uid, MessageRecordType.MESSAGE_CENTER_RECORD);
				WindowModel.Instance.MsgRecordPage.trgMessageTable.Rows.Clear();
				WindowModel.Instance.MsgRecordPage.InitData(this.gp.Gid, false, false, 0);
				WindowModel.Instance.MsgRecordPage.InitTitleName("在" + this.gp.Name + "群的消息记录");
				WindowModel.Instance.MessageCenterWindow.ViewMsgFrame.NavigationService.Navigate(WindowModel.Instance.MsgRecordPage);
				System.GC.Collect();
			}
		}
		public void Dispose()
		{
			this.groups.Clear();
			this.liststaff.Clear();
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/messagecenter/recentcontactsitem.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.imgHead = (Image)target;
        //        break;
        //    case 2:
        //        this.tbkName = (TextBlock)target;
        //        break;
        //    case 3:
        //        this.tbkID = (TextBlock)target;
        //        break;
        //    case 4:
        //        this.tbkDate = (TextBlock)target;
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	
    }
}
