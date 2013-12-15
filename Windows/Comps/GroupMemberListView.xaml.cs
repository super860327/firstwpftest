using IDKin.IM.Core;
using IDKin.IM.Data;
using IDKin.IM.Windows.Util;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
namespace IDKin.IM.Windows.Comps
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class GroupMemberListView : ListView//, IComponentConnector
	{
		private ISessionService sessionService = ServiceUtil.Instance.SessionService;
		private IDataService dataService = ServiceUtil.Instance.DataService;
		private long gid;
		private System.Collections.Generic.IDictionary<long, GroupNodeStaff> nodeStaff = new System.Collections.Generic.Dictionary<long, GroupNodeStaff>();
		//internal GroupMemberListView GroupMemberList;
		//private bool _contentLoaded;
		public long Gid
		{
			get
			{
				return this.gid;
			}
			set
			{
				this.gid = value;
			}
		}
		public GroupMemberListView()
		{
			this.InitializeComponent();
		}
		public void DeleAdmin(long uid)
		{
			GroupNodeStaff gns = this.FindListItem(uid);
			if (gns != null)
			{
				gns.DeleAdminImage();
				EntGroup group = this.dataService.GetEntGroup(this.gid);
				if (group != null && group.IsAdmin(this.sessionService.Uid))
				{
					gns.AddAdminContextMenu();
				}
				if (uid == this.sessionService.Uid)
				{
					foreach (GroupNodeStaff gnstaff in (System.Collections.IEnumerable)this.GroupMemberList.Items)
					{
						gnstaff.ClearAdminContextMenu();
					}
				}
			}
		}
		public void AddAdmin(long uid)
		{
			GroupNodeStaff gns = this.FindListItem(uid);
			if (gns != null)
			{
				gns.AddAdminImage();
				EntGroup group = this.dataService.GetEntGroup(this.gid);
				if (group != null && group.IsAdmin(this.sessionService.Uid))
				{
					gns.DeleAdminContextMenu();
				}
				if (uid == this.sessionService.Uid)
				{
					foreach (GroupNodeStaff gnstaff in (System.Collections.IEnumerable)this.GroupMemberList.Items)
					{
						if (gnstaff.Staff.Uid == this.sessionService.Uid)
						{
							gnstaff.ClearAdminContextMenu();
						}
						else
						{
							if (group.IsAdmin(gnstaff.Staff.Uid))
							{
								gnstaff.DeleAdminContextMenu();
							}
							else
							{
								gnstaff.AddAdminContextMenu();
							}
						}
					}
				}
			}
		}
		public void AddGroupMemberList(System.Collections.Generic.List<Staff> list)
		{
			System.Threading.Monitor.Enter(this.GroupMemberList);
			foreach (Staff staff in list)
			{
				this.AddGroupMember(staff);
			}
			System.Threading.Monitor.Exit(this.GroupMemberList);
		}
		public void AddGroupMembers(long[] members)
		{
			for (int i = 0; i < members.Length; i++)
			{
				long member = members[i];
				Staff staff = this.dataService.GetStaff(member);
				if (staff != null)
				{
					this.AddGroupMember(staff);
				}
			}
		}
		public void AddGroupMember(Staff staff)
		{
			if (staff != null)
			{
				GroupNodeStaff gns = new GroupNodeStaff(this.gid, staff);
				if (!this.nodeStaff.ContainsKey(staff.Uid))
				{
					this.nodeStaff.Add(staff.Uid, gns);
					this.GroupMemberList.Items.Add(gns);
				}
			}
		}
		public void RemoveMembers(long[] members)
		{
			if (members != null && members.Length > 0)
			{
				for (int i = 0; i < members.Length; i++)
				{
					long member = members[i];
					this.RemoveMember(member);
				}
			}
		}
		public void RemoveMember(long uid)
		{
			this.nodeStaff.Remove(uid);
			GroupNodeStaff gnstaff = this.FindListItem(uid);
			if (gnstaff != null)
			{
				this.GroupMemberList.Items.Remove(gnstaff);
			}
		}
		private GroupNodeStaff FindListItem(long uid)
		{
			GroupNodeStaff result;
			foreach (GroupNodeStaff gns in (System.Collections.IEnumerable)this.GroupMemberList.Items)
			{
				if (gns != null && gns.Staff.Uid == uid)
				{
					result = gns;
					return result;
				}
			}
			result = null;
			return result;
		}
		public void UpdateGroupMember(Staff staff)
		{
			if (staff != null)
			{
				if (this.nodeStaff.ContainsKey(staff.Uid))
				{
					if (this.nodeStaff[staff.Uid].UpdateInfo())
					{
						this.SortGroupMember();
					}
				}
			}
		}
		public void SortGroupMember()
		{
			ItemCollection ic = this.GroupMemberList.Items;
			System.Collections.Generic.IList<System.Collections.Generic.IList<GroupNodeStaff>> list = new System.Collections.Generic.List<System.Collections.Generic.IList<GroupNodeStaff>>();
			System.Collections.Generic.IList<GroupNodeStaff> nodes = null;
			int count = ic.Count;
			for (int i = 0; i < count; i++)
			{
				GroupNodeStaff node = ic[i] as GroupNodeStaff;
				if (node != null)
				{
					if (nodes == null)
					{
						nodes = new System.Collections.Generic.List<GroupNodeStaff>();
					}
					nodes.Add(node);
				}
			}
			if (nodes != null && nodes.Count > 0)
			{
				System.Collections.Generic.IList<GroupNodeStaff> offline = null;
				System.Collections.Generic.IList<GroupNodeStaff> online = null;
				System.Collections.Generic.IList<GroupNodeStaff> away = null;
				System.Collections.Generic.IList<GroupNodeStaff> busy = null;
				System.Collections.Generic.IList<GroupNodeStaff> hide = null;
				System.Collections.Generic.IList<GroupNodeStaff> inme = null;
				System.Collections.Generic.IList<GroupNodeStaff> outting = null;
				System.Collections.Generic.IList<GroupNodeStaff> meeting = null;
				System.Collections.Generic.IList<GroupNodeStaff> doNotDisturb = null;
				foreach (GroupNodeStaff node in nodes)
				{
					switch (node.Status)
					{
					case UserStatus.Offline:
						if (offline == null)
						{
							offline = new System.Collections.Generic.List<GroupNodeStaff>();
						}
						offline.Add(node);
						break;
					case UserStatus.Online:
						if (online == null)
						{
							online = new System.Collections.Generic.List<GroupNodeStaff>();
						}
						online.Add(node);
						break;
					case UserStatus.Away:
						if (away == null)
						{
							away = new System.Collections.Generic.List<GroupNodeStaff>();
						}
						away.Add(node);
						break;
					case UserStatus.Busy:
						if (busy == null)
						{
							busy = new System.Collections.Generic.List<GroupNodeStaff>();
						}
						busy.Add(node);
						break;
					case UserStatus.Hide:
						if (hide == null)
						{
							hide = new System.Collections.Generic.List<GroupNodeStaff>();
						}
						hide.Add(node);
						break;
					case UserStatus.In:
						if (inme == null)
						{
							inme = new System.Collections.Generic.List<GroupNodeStaff>();
						}
						inme.Add(node);
						break;
					case UserStatus.Out:
						if (outting == null)
						{
							outting = new System.Collections.Generic.List<GroupNodeStaff>();
						}
						outting.Add(node);
						break;
					case UserStatus.Meeting:
						if (meeting == null)
						{
							meeting = new System.Collections.Generic.List<GroupNodeStaff>();
						}
						meeting.Add(node);
						break;
					case UserStatus.DoNotDisturb:
						if (doNotDisturb == null)
						{
							doNotDisturb = new System.Collections.Generic.List<GroupNodeStaff>();
						}
						doNotDisturb.Add(node);
						break;
					}
				}
				int onlineCount = 0;
				if (online != null)
				{
					list.Add(online);
					onlineCount += online.Count;
				}
				if (inme != null)
				{
					list.Add(inme);
					onlineCount += inme.Count;
				}
				if (away != null)
				{
					list.Add(away);
					onlineCount += away.Count;
				}
				if (meeting != null)
				{
					list.Add(meeting);
					onlineCount += meeting.Count;
				}
				if (busy != null)
				{
					list.Add(busy);
					onlineCount += busy.Count;
				}
				if (doNotDisturb != null)
				{
					list.Add(doNotDisturb);
					onlineCount += doNotDisturb.Count;
				}
				if (outting != null)
				{
					list.Add(outting);
					onlineCount += outting.Count;
				}
				if (hide != null)
				{
					list.Add(hide);
				}
				if (offline != null)
				{
					list.Add(offline);
				}
				foreach (GroupNodeStaff tn in nodes)
				{
					ic.Remove(tn);
				}
				foreach (System.Collections.Generic.IList<GroupNodeStaff> tlist in list)
				{
					foreach (GroupNodeStaff tn in tlist)
					{
						ic.Add(tn);
					}
				}
				online = null;
				inme = null;
				away = null;
				meeting = null;
				busy = null;
				doNotDisturb = null;
				outting = null;
				hide = null;
				offline = null;
				nodes = null;
				list = null;
			}
		}
		public void SortGroupMemberByName()
		{
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/groupmemberlistview.xaml", UriKind.Relative);
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
        //        this.GroupMemberList = (GroupMemberListView)target;
        //    }
        //}
	}
}
