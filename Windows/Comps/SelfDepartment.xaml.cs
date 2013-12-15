using IDKin.IM.Core;
using IDKin.IM.Data;
using IDKin.IM.Log;
using IDKin.IM.Windows.Model;
using IDKin.IM.Windows.Util;
using IDKin.IM.Windows.ViewModel;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
namespace IDKin.IM.Windows.Comps
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class SelfDepartment : TreeViewItem//, IComponentConnector
	{
		private IDataService dataService = null;
		private ISessionService sessionService = null;
		private ILogger logger = null;
		private System.Collections.Generic.IDictionary<long, SelfDepartmentStaffNode> staffNode = new System.Collections.Generic.Dictionary<long, SelfDepartmentStaffNode>();
		private readonly int MOUSEEVENTF_LEFTDOWN = 2;
        ////private bool _contentLoaded;
		public System.Collections.Generic.IDictionary<long, SelfDepartmentStaffNode> StaffNode
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
		[System.Runtime.InteropServices.DllImport("user32")]
		public static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
		public SelfDepartment()
		{
			this.InitializeComponent();
			base.PreviewMouseRightButtonDown += delegate(object s, MouseButtonEventArgs e)
			{
				SelfDepartment.mouse_event(this.MOUSEEVENTF_LEFTDOWN, (int)e.GetPosition(this).X, (int)e.GetPosition(this).Y, 0, 0);
			};
		}
		public void InitService()
		{
			this.dataService = ServiceUtil.Instance.DataService;
			this.sessionService = ServiceUtil.Instance.SessionService;
			this.logger = ServiceUtil.Instance.Logger;
		}
		public void UpdateStaff(Staff staff)
		{
			try
			{
				if (staff != null && this.staffNode.ContainsKey(staff.Uid) && staff.DepartmentId == this.sessionService.DepartmentId)
				{
					this.staffNode[staff.Uid].UpdateInfo();
					this.SortAllDepartment();
				}
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		public void AddStaff(Staff staff)
		{
			try
			{
				if (staff != null)
				{
					if (!this.staffNode.ContainsKey(staff.Uid))
					{
						SelfDepartmentStaffNode node = new SelfDepartmentStaffNode(staff);
						node.SessionService = this.sessionService;
						node.DataService = this.dataService;
						if (!this.staffNode.ContainsKey(staff.Uid))
						{
							this.staffNode.Add(staff.Uid, node);
							base.Items.Add(node);
							node.ContextMenu = this.GetContextMenu(node);
							node.ContextMenuOpening += delegate(object s, ContextMenuEventArgs e)
							{
								if (DataModel.Instance.CustomeGroupName.Count > 0)
								{
									this.AddContextMenu(node, s);
								}
							};
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		private void AddContextMenu(SelfDepartmentStaffNode node, object s)
		{
			(node.ContextMenu.Items[0] as MenuItem).Items.Clear();
			SelfDepartmentStaffNode treeNodeStaff = s as SelfDepartmentStaffNode;
			if (treeNodeStaff == null || treeNodeStaff.Staff.Uid != this.sessionService.Uid)
			{
				foreach (CustomGroup item in DataModel.Instance.CustomeGroupName.Values)
				{
					MenuItem menuitem = new MenuItem();
					menuitem.Header = item.GroupName;
					menuitem.Tag = item;
					if (treeNodeStaff != null && item.Members != null && item.Members.Count > 0)
					{
						foreach (Staff sta in item.Members)
						{
							if (sta.Uid == treeNodeStaff.Staff.Uid)
							{
								menuitem.IsCheckable = false;
								menuitem.IsEnabled = false;
								menuitem.IsChecked = true;
								break;
							}
						}
					}
					(node.ContextMenu.Items[0] as MenuItem).Items.Add(menuitem);
					menuitem.Click += new RoutedEventHandler(this.menuitem_Click);
				}
			}
		}
		private SelfDepartmentStaffNode GetSelectedNode()
		{
			SelfDepartmentStaffNode result;
			if (base.Items.Count > 0)
			{
				foreach (object item in (System.Collections.IEnumerable)base.Items)
				{
					SelfDepartmentStaffNode node = item as SelfDepartmentStaffNode;
					if (node != null && node.IsBigHead)
					{
						result = node;
						return result;
					}
				}
			}
			result = null;
			return result;
		}
		private void menuitem_Click(object sender, RoutedEventArgs e)
		{
			if (this.GetSelectedNode() != null)
			{
				SelfDepartmentStaffNode staff = this.GetSelectedNode();
				if (staff != null)
				{
					CustomGroup group = (sender as MenuItem).Tag as CustomGroup;
					CustomGroupManagerWindowViewModel viewModel = CustomGroupManagerWindowViewModel.GetInstance();
					viewModel.AddMemberToCustomGroup((int)this.sessionService.Uid, group.GroupID, this.GetMembers(group) + staff.Staff.Uid + ":", staff.Staff.Uid.ToString());
					if (DataModel.Instance.CustomeGroupName[group.GroupID].Members == null)
					{
						DataModel.Instance.CustomeGroupName[group.GroupID].Members = new System.Collections.Generic.List<Staff>();
					}
					DataModel.Instance.CustomeGroupName[group.GroupID].Members.Add(staff.Staff);
				}
			}
		}
		private string GetMembers(CustomGroup item)
		{
			string result = string.Empty;
			if (item.Members != null)
			{
				foreach (Staff staff in item.Members)
				{
					result = result + staff.Uid + ":";
				}
			}
			return result;
		}
		private ContextMenu GetContextMenu(SelfDepartmentStaffNode node)
		{
			return new ContextMenu
			{
				Items = 
				{
					new MenuItem
					{
						Header = "将联系人添加至"
					}
				}
			};
		}
		public void Clear()
		{
			this.staffNode.Clear();
			base.Items.Clear();
		}
		public void SortAllDepartment()
		{
			try
			{
				this.SortStatus(this);
			}
			catch (System.Exception e)
			{
				System.Console.WriteLine(e.ToString());
			}
		}
		private void SortStatus(TreeViewItem parentNode)
		{
			System.Collections.Generic.IList<System.Collections.Generic.IList<SelfDepartmentStaffNode>> list = new System.Collections.Generic.List<System.Collections.Generic.IList<SelfDepartmentStaffNode>>();
			System.Collections.Generic.IList<SelfDepartmentStaffNode> nodes = null;
			int count = parentNode.Items.Count;
			for (int i = 0; i < count; i++)
			{
				SelfDepartmentStaffNode node = parentNode.Items[i] as SelfDepartmentStaffNode;
				if (node != null)
				{
					if (nodes == null)
					{
						nodes = new System.Collections.Generic.List<SelfDepartmentStaffNode>();
					}
					nodes.Add(node);
				}
			}
			if (nodes != null && nodes.Count > 0)
			{
				System.Collections.Generic.IList<SelfDepartmentStaffNode> offline = null;
				System.Collections.Generic.IList<SelfDepartmentStaffNode> online = null;
				System.Collections.Generic.IList<SelfDepartmentStaffNode> away = null;
				System.Collections.Generic.IList<SelfDepartmentStaffNode> busy = null;
				System.Collections.Generic.IList<SelfDepartmentStaffNode> hide = null;
				System.Collections.Generic.IList<SelfDepartmentStaffNode> inme = null;
				System.Collections.Generic.IList<SelfDepartmentStaffNode> outting = null;
				System.Collections.Generic.IList<SelfDepartmentStaffNode> meeting = null;
				System.Collections.Generic.IList<SelfDepartmentStaffNode> doNotDisturb = null;
				foreach (SelfDepartmentStaffNode node in nodes)
				{
					switch (node.Status)
					{
					case UserStatus.Offline:
						if (offline == null)
						{
							offline = new System.Collections.Generic.List<SelfDepartmentStaffNode>();
						}
						offline.Add(node);
						break;
					case UserStatus.Online:
						if (online == null)
						{
							online = new System.Collections.Generic.List<SelfDepartmentStaffNode>();
						}
						online.Add(node);
						break;
					case UserStatus.Away:
						if (away == null)
						{
							away = new System.Collections.Generic.List<SelfDepartmentStaffNode>();
						}
						away.Add(node);
						break;
					case UserStatus.Busy:
						if (busy == null)
						{
							busy = new System.Collections.Generic.List<SelfDepartmentStaffNode>();
						}
						busy.Add(node);
						break;
					case UserStatus.Hide:
						if (hide == null)
						{
							hide = new System.Collections.Generic.List<SelfDepartmentStaffNode>();
						}
						hide.Add(node);
						break;
					case UserStatus.In:
						if (inme == null)
						{
							inme = new System.Collections.Generic.List<SelfDepartmentStaffNode>();
						}
						inme.Add(node);
						break;
					case UserStatus.Out:
						if (outting == null)
						{
							outting = new System.Collections.Generic.List<SelfDepartmentStaffNode>();
						}
						outting.Add(node);
						break;
					case UserStatus.Meeting:
						if (meeting == null)
						{
							meeting = new System.Collections.Generic.List<SelfDepartmentStaffNode>();
						}
						meeting.Add(node);
						break;
					case UserStatus.DoNotDisturb:
						if (doNotDisturb == null)
						{
							doNotDisturb = new System.Collections.Generic.List<SelfDepartmentStaffNode>();
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
				foreach (SelfDepartmentStaffNode tn in nodes)
				{
					parentNode.Items.Remove(tn);
				}
				foreach (System.Collections.Generic.IList<SelfDepartmentStaffNode> tlist in list)
				{
					foreach (SelfDepartmentStaffNode tn in tlist)
					{
						parentNode.Items.Add(tn);
					}
				}
				Department department = parentNode.DataContext as Department;
				if (department != null)
				{
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
		private void SortUsername(System.Collections.Generic.IList<SelfDepartmentStaffNode> list)
		{
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/selfdepartment.xaml", UriKind.Relative);
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
