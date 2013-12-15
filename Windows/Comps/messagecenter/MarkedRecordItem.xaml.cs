using IDKin.IM.Core;
using IDKin.IM.Data;
using IDKin.IM.Protocol.Commons;
using IDKin.IM.Protocol.Enterprise;
using IDKin.IM.Windows.Model;
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
	public partial class MarkedRecordItem : ListViewItem, System.IDisposable//, IComponentConnector
	{
		private IDataService dataService = ServiceUtil.Instance.DataService;
		private System.Collections.Generic.ICollection<IDKin.IM.Core.Staff> stafflist = null;
		private System.Collections.Generic.ICollection<EntGroup> groups = null;
		private EntGroup entgroup = null;
		private IDKin.IM.Core.Staff st = null;
		private ISessionService sessionService = ServiceUtil.Instance.SessionService;
		private MessageCenterViewModel messageCenter = new MessageCenterViewModel();
		public bool IsValue = false;
		//internal Image imgHead;
		//internal TextBlock tbkName;
		//internal TextBlock tbkID;
		//internal TextBlock tbkDate;
        ////private bool _contentLoaded;
		public MarkedRecordItem(GetIsMark mark, bool IsStaff)
		{
			this.InitializeComponent();
			this.InitData();
			this.InitServic();
			if (IsStaff)
			{
				if (this.StaffInfo(mark))
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
				if (this.GroupInfo(mark))
				{
					this.IsValue = true;
				}
				else
				{
					this.IsValue = false;
				}
			}
		}
		private void InitServic()
		{
			base.MouseDoubleClick += new MouseButtonEventHandler(this.MarkedRecordItem_MouseDoubleClick);
		}
		private void InitData()
		{
			this.stafflist = this.dataService.GetStaffList();
			this.groups = this.dataService.GetEntGroupList();
		}
		public MarkedRecordItem(IDKin.IM.Core.Staff staff)
		{
			this.InitializeComponent();
			this.InitData();
			this.InitServic();
			this.imgHead.Source = staff.HeaderImageOnline;
			this.tbkName.Text = staff.Name;
			this.tbkDate.Text = staff.CreateTime;
			this.tbkID.Text = staff.Uid.ToString();
			this.st = staff;
		}
		private bool StaffInfo(GetIsMark mark)
		{
			IDKin.IM.Core.Staff staff = this.dataService.GetStaff(mark.id);
			bool result;
			if (staff != null && mark.id != this.sessionService.Uid)
			{
				this.imgHead.Source = staff.HeaderImageOnline;
				this.tbkName.Text = staff.Name;
				this.tbkDate.Text = mark.updateTime;
				this.tbkID.Text = staff.Uid.ToString();
				this.st = staff;
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}
		private bool GroupInfo(GetIsMark mark)
		{
			EntGroup group = this.dataService.GetEntGroup(mark.id);
			bool result;
			if (group != null)
			{
				this.imgHead.Source = group.AdminIcon;
				this.tbkName.Text = group.Name;
				this.tbkDate.Text = mark.updateTime;
				this.tbkID.Text = group.Gid.ToString();
				this.entgroup = group;
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}
		private void MarkedRecordItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (this.st != null)
			{
				this.messageCenter.SendStaffMessageRecordMark(this.sessionService.Uid, this.st.Uid, "0", 1, 10, MessageRecordType.MESSAGE_CENTER_RECORD);
				WindowModel.Instance.MsgRecordPage.trgMessageTable.Rows.Clear();
				WindowModel.Instance.MsgRecordPage.InitData(this.st.Uid, true, true, 0);
				WindowModel.Instance.MsgRecordPage.InitTitleName("与" + this.st.Name + "的消息记录");
				WindowModel.Instance.MessageCenterWindow.ViewMsgFrame.NavigationService.Navigate(WindowModel.Instance.MsgRecordPage);
			}
			else
			{
				this.messageCenter.SendGroupMessageRecordMark(this.entgroup.Gid, "0", 1, 10, this.sessionService.Uid, MessageRecordType.MESSAGE_CENTER_RECORD);
				WindowModel.Instance.MsgRecordPage.trgMessageTable.Rows.Clear();
				WindowModel.Instance.MsgRecordPage.InitData(this.entgroup.Gid, true, false, 0);
				WindowModel.Instance.MsgRecordPage.InitTitleName("在" + this.entgroup.Name + "群的消息记录");
				WindowModel.Instance.MessageCenterWindow.ViewMsgFrame.NavigationService.Navigate(WindowModel.Instance.MsgRecordPage);
			}
		}
		public void Dispose()
		{
			this.groups.Clear();
			this.stafflist.Clear();
			throw new System.NotImplementedException();
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/messagecenter/markedrecorditem.xaml", UriKind.Relative);
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
