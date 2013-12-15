using IDKin.IM.Core;
using IDKin.IM.Data;
using IDKin.IM.Protocol.Commons;
using IDKin.IM.Protocol.Enterprise;
using IDKin.IM.Theme.Helper;
using IDKin.IM.Windows.Util;
using IDKin.IM.Windows.ViewModel;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
namespace IDKin.IM.Windows.Comps.MessageCenter
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public partial class MarkedRecordPage : Page//, IComponentConnector
	{
		private ISessionService sessionService = ServiceUtil.Instance.SessionService;
		private IDataService dataService = ServiceUtil.Instance.DataService;
		private System.Collections.Generic.ICollection<EntGroup> groups = null;
		private System.Collections.Generic.IList<IDKin.IM.Core.Staff> departStaff = new System.Collections.Generic.List<IDKin.IM.Core.Staff>();
		private MessageCenterViewModel messageCenter = new MessageCenterViewModel();
		//internal ListView markedList;
        ////private bool _contentLoaded;
		public MarkedRecordPage()
		{
			ThemeSwitcher.LoadSkin(ThemeEnum.Aero, this);
			this.InitializeComponent();
			this.markedList.Items.Clear();
			this.groups = this.dataService.GetEntGroupList();
		}
		public void InitData()
		{
			this.messageCenter.SendMarkByType(this.sessionService.Uid, IsMarkType.FELLOW);
			System.Collections.Generic.List<long> list = this.GetGroupGid();
			this.messageCenter.SendGroupMarkByType(list, IsMarkType.GROUP, this.sessionService.Uid);
		}
		private System.Collections.Generic.List<long> GetGroupGid()
		{
			System.Collections.Generic.List<long> list = new System.Collections.Generic.List<long>();
			foreach (EntGroup gp in this.groups)
			{
				list.Add(gp.Gid);
			}
			return list;
		}
		public void ShowStaffList(System.Collections.Generic.List<GetIsMark> getIsMark)
		{
			for (int i = 0; i < getIsMark.Count; i++)
			{
				MarkedRecordItem item = new MarkedRecordItem(getIsMark[i], true);
				if (item.IsValue)
				{
					this.markedList.Items.Add(item);
				}
				IDKin.IM.Core.Staff staff = this.dataService.GetStaff(getIsMark[i].id);
				if (staff != null)
				{
					staff.CreateTime = getIsMark[i].updateTime;
					this.departStaff.Add(staff);
				}
			}
		}
		public void ShowGroupList(System.Collections.Generic.List<GetIsMark> getIsMark)
		{
			for (int i = 0; i < getIsMark.Count; i++)
			{
				MarkedRecordItem item = new MarkedRecordItem(getIsMark[i], false);
				if (item.IsValue)
				{
					this.markedList.Items.Add(item);
				}
			}
		}
		public void ShowDepartList(System.Collections.Generic.List<IDKin.IM.Core.Staff> stafflist)
		{
			foreach (IDKin.IM.Core.Staff staff in stafflist)
			{
				if (staff.Uid != this.sessionService.Uid)
				{
					foreach (IDKin.IM.Core.Staff st in this.departStaff)
					{
						if (st.Uid == staff.Uid)
						{
							staff.CreateTime = st.CreateTime;
							break;
						}
					}
					MarkedRecordItem item = new MarkedRecordItem(staff);
					this.markedList.Items.Add(item);
				}
			}
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/messagecenter/markedrecordpage.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
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
        //        this.markedList = (ListView)target;
        //    }
        //}
	}
}
