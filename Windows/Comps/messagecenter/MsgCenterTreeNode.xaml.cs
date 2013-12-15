using IDKin.IM.Core;
using IDKin.IM.Data;
using IDKin.IM.Log;
using IDKin.IM.Protocol.Commons;
using IDKin.IM.Windows.Model;
using IDKin.IM.Windows.Util;
using IDKin.IM.Windows.ViewModel;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
namespace IDKin.IM.Windows.Comps.MessageCenter
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public partial class MsgCenterTreeNode : UserControl//, IComponentConnector
	{
		private ILogger logger = ServiceUtil.Instance.Logger;
		private ChatTabControlViewModel viewModel = new ChatTabControlViewModel();
		private MessageCenterViewModel messageCenter = new MessageCenterViewModel();
		private Staff staff = null;
		private EntGroup group = null;
		private bool StaffIsMark = false;
		private bool GroupIsMark = false;
		private ISessionService sessionService = null;
		private IDataService dataService = ServiceUtil.Instance.DataService;
		private UserStatus status;
        ////internal Border Bd;
        ////internal Grid canvas;
        ////internal TreeNodeStaffHead imgFace;
        ////internal Image imgFaceForeground;
        ////internal TextBlock txtName;
        ////private bool _contentLoaded;
		public ISessionService SessionService
		{
			get
			{
				return this.sessionService;
			}
			set
			{
				this.sessionService = value;
			}
		}
		public IDataService DataService
		{
			get
			{
				return this.dataService;
			}
			set
			{
				this.dataService = value;
			}
		}
		public Staff Staff
		{
			get
			{
				return this.staff;
			}
		}
		public UserStatus Status
		{
			get
			{
				return this.staff.Status;
			}
			set
			{
				this.staff.Status = value;
			}
		}
		public MsgCenterTreeNode(Staff staff, bool isMark)
		{
			this.viewModel.AddEventListenerHandler();
			this.InitializeComponent();
			this.staff = staff;
			this.status = UserStatus.Online;
			this.StaffIsMark = isMark;
			base.MouseLeftButtonUp += new MouseButtonEventHandler(this.StaffMsgCenterTreeNode_MouseLeftButtonUp);
			this.UpdateInfoStaff();
		}
		public MsgCenterTreeNode(EntGroup gp, bool isMark)
		{
			this.viewModel.AddEventListenerHandler();
			this.InitializeComponent();
			this.group = gp;
			this.GroupIsMark = isMark;
			base.MouseLeftButtonUp += new MouseButtonEventHandler(this.GroupMsgCenterTreeNode_MouseLeftButtonUp);
			this.UpdateInfoGroup();
		}
		public bool UpdateInfoGroup()
		{
			this.txtName.Text = this.group.Name;
			this.imgFace.bgHead.ImageSource = this.group.AdminIcon;
			return true;
		}
		public bool UpdateInfoStaff()
		{
			bool update = false;
			this.txtName.Text = this.staff.Name;
			this.imgFace.bgHead.ImageSource = this.staff.HeaderImageOnline;
			this.imgFaceForeground.Source = this.staff.StatusIcon;
			if (this.status != this.staff.Status)
			{
				this.status = this.staff.Status;
				update = true;
			}
			return update;
		}
		public void GroupMsgCenterTreeNode_MouseLeftButtonUp(object sender, System.EventArgs e)
		{
			if (!this.GroupIsMark)
			{
				this.viewModel.SendGroupMessageRecord(this.group.Gid, "0", 1, 10, this.sessionService.Uid, MessageRecordType.MESSAGE_CENTER_RECORD);
				WindowModel.Instance.MsgRecordPage.trgMessageTable.Rows.Clear();
				WindowModel.Instance.MsgRecordPage.InitData(this.group.Gid, false, false, 0);
				WindowModel.Instance.MsgRecordPage.InitTitleName("在" + this.group.Name + "群的消息记录");
				WindowModel.Instance.MessageCenterWindow.ViewMsgFrame.NavigationService.Navigate(WindowModel.Instance.MsgRecordPage);
				System.GC.Collect();
			}
			else
			{
				this.messageCenter.SendGroupMessageRecordMark(this.group.Gid, "0", 1, 10, this.sessionService.Uid, MessageRecordType.MESSAGE_CENTER_RECORD);
				WindowModel.Instance.MsgRecordPage.trgMessageTable.Rows.Clear();
				WindowModel.Instance.MsgRecordPage.InitData(this.group.Gid, true, false, 0);
				WindowModel.Instance.MsgRecordPage.InitTitleName("在" + this.group.Name + "群的消息记录");
				WindowModel.Instance.MessageCenterWindow.ViewMsgFrame.NavigationService.Navigate(WindowModel.Instance.MsgRecordPage);
				System.GC.Collect();
			}
		}
		public void StaffMsgCenterTreeNode_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (!this.StaffIsMark)
			{
				this.viewModel.SendStaffMessageRecord(this.sessionService.Uid, this.staff.Uid, "0", 1, 10, MessageRecordType.MESSAGE_CENTER_RECORD);
				WindowModel.Instance.MsgRecordPage.trgMessageTable.Rows.Clear();
				WindowModel.Instance.MsgRecordPage.InitData(this.staff.Uid, false, true, 0);
				WindowModel.Instance.MsgRecordPage.InitTitleName("与" + this.staff.Name + "的消息记录");
				WindowModel.Instance.MessageCenterWindow.ViewMsgFrame.NavigationService.Navigate(WindowModel.Instance.MsgRecordPage);
			}
			else
			{
				this.messageCenter.SendStaffMessageRecordMark(this.sessionService.Uid, this.staff.Uid, "0", 1, 10, MessageRecordType.MESSAGE_CENTER_RECORD);
				WindowModel.Instance.MsgRecordPage.trgMessageTable.Rows.Clear();
				WindowModel.Instance.MsgRecordPage.InitData(this.staff.Uid, true, true, 0);
				WindowModel.Instance.MsgRecordPage.InitTitleName("与" + this.staff.Name + "的消息记录");
				WindowModel.Instance.MessageCenterWindow.ViewMsgFrame.NavigationService.Navigate(WindowModel.Instance.MsgRecordPage);
			}
		}
		private void Bd_MouseEnter(object sender, MouseEventArgs e)
		{
			this.Bd.Background = new SolidColorBrush(Color.FromRgb(248, 248, 216));
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/messagecenter/msgcentertreenode.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[System.Diagnostics.DebuggerNonUserCode]
        ////internal System.Delegate _CreateDelegate(System.Type delegateType, string handler)
        //{
        //    return System.Delegate.CreateDelegate(delegateType, this, handler);
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.Bd = (Border)target;
        //        this.Bd.MouseEnter += new MouseEventHandler(this.Bd_MouseEnter);
        //        break;
        //    case 2:
        //        this.canvas = (Grid)target;
        //        break;
        //    case 3:
        //        this.imgFace = (TreeNodeStaffHead)target;
        //        break;
        //    case 4:
        //        this.imgFaceForeground = (Image)target;
        //        break;
        //    case 5:
        //        this.txtName = (TextBlock)target;
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
