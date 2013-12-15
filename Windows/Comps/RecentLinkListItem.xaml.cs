using IDKin.IM.Core;
using IDKin.IM.Data;
using IDKin.IM.Log;
using IDKin.IM.Protocol.Commons;
using IDKin.IM.Windows.Model;
using IDKin.IM.Windows.Model.RecentLink;
using IDKin.IM.Windows.Util;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
namespace IDKin.IM.Windows.Comps
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class RecentLinkListItem : TreeViewItem//, IComponentConnector
	{
		private const int MaxLength = 20;
		private IDataService dataService = null;
		private ISessionService sessionService = null;
		private ILogger logger = null;
		private System.Collections.Generic.IDictionary<long, RecentLinkNode> staffNode = new System.Collections.Generic.Dictionary<long, RecentLinkNode>();
        ////private bool _contentLoaded;
		public System.Collections.Generic.IDictionary<long, RecentLinkNode> StaffNode
		{
			get
			{
				return this.staffNode;
			}
			set
			{
				this.staffNode = value;
			}
		}
		public RecentLinkListItem()
		{
			this.InitializeComponent();
		}
		public void InitService()
		{
			this.dataService = ServiceUtil.Instance.DataService;
			this.sessionService = ServiceUtil.Instance.SessionService;
			this.logger = ServiceUtil.Instance.Logger;
		}
		public void AddRecentLinkList(IsMarkType type, System.Collections.Generic.List<RecentLinkInfo> list)
		{
			switch (type)
			{
			case IsMarkType.FELLOW:
				foreach (RecentLinkInfo info in list)
				{
					this.AddEntStaffLinkListItem(info);
				}
				break;
			}
		}
		public void AddRecentLink(RecentLinkType type, long id)
		{
			switch (type)
			{
			case RecentLinkType.EntStaffChat:
			{
				this.AddEntStaffLink(id);
				RecentLinkInfo info = new RecentLinkInfo();
				info.Id = id;
				info.Type = IsMarkType.FELLOW;
				info.RecentTime = this.sessionService.CalculateSystemTime();
				DataModel.Instance.RecentLinkInfoList.Add(info);
				break;
			}
			}
		}
		private void SaveRecentLinkList()
		{
		}
		public void UpdateStaffStaus(Staff staff)
		{
			if (staff != null && this.staffNode.ContainsKey(staff.Uid))
			{
				RecentLinkNode tns = this.staffNode[staff.Uid];
				tns.UpdateInfo();
			}
		}
		private void AddEntStaffLinkListItem(RecentLinkInfo info)
		{
			if (info != null)
			{
				try
				{
					Staff staff = this.dataService.GetStaff(info.Id);
					if (staff != null)
					{
						if (!this.staffNode.ContainsKey(info.Id) && base.Items.Count < 20)
						{
							RecentLinkNode node = new RecentLinkNode(staff);
							base.Items.Insert(0, node);
							this.staffNode.Add(info.Id, node);
							node.LastTime = info.RecentTime;
						}
					}
				}
				catch (System.ArgumentNullException e)
				{
					this.logger.Error(e.ToString());
				}
				catch (System.Exception e2)
				{
					this.logger.Error(e2.ToString());
				}
			}
		}
		private void AddEntStaffLink(long id)
		{
			try
			{
				Staff staff = this.dataService.GetStaff(id);
				if (staff != null)
				{
					RecentLinkNode node;
					if (this.staffNode.ContainsKey(id))
					{
						node = this.staffNode[id];
						base.Items.Remove(node);
						base.Items.Insert(0, node);
					}
					else
					{
						node = new RecentLinkNode(staff);
						base.Items.Insert(0, node);
						this.staffNode.Add(id, node);
					}
					node.LastTime = this.sessionService.CalculateSystemTime();
					if (base.Items.Count > 20)
					{
						for (int i = base.Items.Count - 1; i >= 20; i--)
						{
							RecentLinkNode tns = base.Items[i] as RecentLinkNode;
							if (tns != null)
							{
								this.staffNode.Remove(tns.Staff.Uid);
							}
							base.Items.RemoveAt(i);
						}
					}
				}
			}
			catch (System.ArgumentNullException e)
			{
				ServiceUtil.Instance.Logger.Error(e.ToString());
			}
			catch (System.Exception e2)
			{
				ServiceUtil.Instance.Logger.Error(e2.ToString());
			}
		}
		private System.Collections.Generic.List<RecentLinkInfo> BuildRecentLinkInfoList()
		{
			System.Collections.Generic.List<RecentLinkInfo> list = new System.Collections.Generic.List<RecentLinkInfo>();
			foreach (RecentLinkNode node in (System.Collections.IEnumerable)base.Items)
			{
				if (node != null)
				{
					list.Add(new RecentLinkInfo
					{
						Id = node.Staff.Uid,
						Type = IsMarkType.FELLOW,
						RecentTime = node.LastTime
					});
				}
			}
			return list;
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/recentlinklistitem.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    this._contentLoaded = true;
        //}
	}
}
