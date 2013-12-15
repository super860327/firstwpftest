using IDKin.IM.Core;
using IDKin.IM.Data;
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
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
namespace IDKin.IM.Windows.Comps
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class DepartmentTreeView : TreeView//, IComponentConnector
	{
		private IDataService dataService = null;
		private ISessionService sessionService = null;
		private System.Collections.Generic.IDictionary<long, TreeViewItem> departmentNode = new System.Collections.Generic.Dictionary<long, TreeViewItem>();
		private System.Collections.Generic.IDictionary<long, TreeNodeStaff> staffNode = new System.Collections.Generic.Dictionary<long, TreeNodeStaff>();
		private long currentRootId;
		private System.Collections.Generic.List<TreeViewItem> unParentDepartmentNodes = new System.Collections.Generic.List<TreeViewItem>();
		private readonly int MOUSEEVENTF_LEFTDOWN = 2;
		//internal DepartmentTreeView tree;
        ////private bool _contentLoaded;
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
		public System.Collections.Generic.IDictionary<long, TreeNodeStaff> StaffNode
		{
			get
			{
				return this.staffNode;
			}
		}
		public long CurrentRootId
		{
			get
			{
				return this.currentRootId;
			}
			set
			{
				this.currentRootId = value;
			}
		}
		[System.Runtime.InteropServices.DllImport("user32")]
		public static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
		public DepartmentTreeView()
		{
			this.InitializeComponent();
			base.PreviewMouseRightButtonDown += delegate(object s, MouseButtonEventArgs e)
			{
				DepartmentTreeView.mouse_event(this.MOUSEEVENTF_LEFTDOWN, (int)e.GetPosition(this).X, (int)e.GetPosition(this).Y, 0, 0);
			};
		}
		public void UnParentDepartmentNodeProcessor()
		{
			try
			{
				if (this.unParentDepartmentNodes != null && this.unParentDepartmentNodes.Count > 0)
				{
					while (this.unParentDepartmentNodes.Count > 0)
					{
						bool hasfind = false;
						TreeViewItem[] items = this.unParentDepartmentNodes.ToArray();
						TreeViewItem[] array = items;
						for (int i = 0; i < array.Length; i++)
						{
							TreeViewItem item = array[i];
							Department department = item.DataContext as Department;
							if (department != null && this.departmentNode.ContainsKey(department.Pid))
							{
								this.departmentNode[department.Pid].Items.Add(item);
								this.unParentDepartmentNodes.Remove(item);
								hasfind = true;
							}
						}
						if (!hasfind)
						{
							break;
						}
						this.unParentDepartmentNodes.Clear();
						this.unParentDepartmentNodes = null;
					}
				}
			}
			catch (System.Exception e)
			{
				ServiceUtil.Instance.Logger.Error(e.ToString());
			}
		}
		public void AddDepartment(Department department)
		{
			TreeViewItem tvi = new TreeViewItem();
			TextBlock nameBlock = new TextBlock();
			nameBlock.VerticalAlignment = VerticalAlignment.Center;
			nameBlock.Text = department.Name + "[0/0]";
			tvi.Header = new StackPanel
			{
				Height = 22.0,
				Orientation = Orientation.Horizontal,
				Children = 
				{
					nameBlock
				}
			};
			tvi.DataContext = department;
			if (this.departmentNode.ContainsKey(department.Pid))
			{
				this.departmentNode[department.Pid].Items.Add(tvi);
			}
			else
			{
				if (department.Pid == this.currentRootId)
				{
					this.tree.Items.Add(tvi);
				}
				else
				{
					this.unParentDepartmentNodes.Add(tvi);
				}
			}
			if (!this.departmentNode.ContainsKey(department.Id))
			{
				this.departmentNode.Add(department.Id, tvi);
			}
		}
		private ContextMenu GetContextMenu(TreeNodeStaff node)
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
		private void menuItem_Click(object sender, RoutedEventArgs e)
		{
		}
		public void AddStaff(Staff staff)
		{
			try
			{
				if (staff != null)
				{
					System.Threading.Monitor.Enter(this.staffNode);
					if (!this.staffNode.ContainsKey(staff.Uid) && this.departmentNode.ContainsKey(staff.DepartmentId))
					{
						TreeNodeStaff node = new TreeNodeStaff(staff);
						node.SessionService = this.sessionService;
						node.DataService = this.dataService;
						if (!this.staffNode.ContainsKey(staff.Uid))
						{
							this.staffNode.Add(staff.Uid, node);
							this.departmentNode[staff.DepartmentId].Items.Add(node);
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
					System.Threading.Monitor.Exit(this.staffNode);
				}
			}
			catch (System.Exception ex)
			{
				System.Console.WriteLine(ex.ToString());
			}
		}
		private void AddContextMenu(TreeNodeStaff node, object s)
		{
			(node.ContextMenu.Items[0] as MenuItem).Items.Clear();
			TreeNodeStaff treeNodeStaff = s as TreeNodeStaff;
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
		private void menuitem_Click(object sender, RoutedEventArgs e)
		{
			if (this.tree.SelectedItem != null)
			{
				TreeNodeStaff staff = this.tree.SelectedItem as TreeNodeStaff;
				if (staff != null)
				{
					CustomGroup group = (sender as MenuItem).Tag as CustomGroup;
					CustomGroupManagerWindowViewModel viewModel = CustomGroupManagerWindowViewModel.GetInstance();
					viewModel.AddMemberToCustomGroup((int)this.SessionService.Uid, group.GroupID, this.GetMembers(group) + staff.Staff.Uid + ":", staff.Staff.Uid.ToString());
					if (DataModel.Instance.CustomeGroupName[group.GroupID].Members == null)
					{
						DataModel.Instance.CustomeGroupName[group.GroupID].Members = new System.Collections.Generic.List<Staff>();
					}
					DataModel.Instance.CustomeGroupName[group.GroupID].Members.Add(staff.Staff);
				}
			}
		}
		public void UpdateDepartmentName(TreeViewItem parentNode, string str)
		{
			StackPanel stackPanel = parentNode.Header as StackPanel;
			if (stackPanel != null)
			{
				if (stackPanel.Children.Count > 0)
				{
					TextBlock tb = stackPanel.Children[0] as TextBlock;
					if (tb != null)
					{
						tb.Text = str;
					}
				}
			}
		}
		public TreeNodeStaff FindTreeNodeStaff(long userID)
		{
			TreeNodeStaff result;
			if (this.staffNode != null && this.staffNode.ContainsKey(userID))
			{
				result = this.staffNode[userID];
			}
			else
			{
				result = null;
			}
			return result;
		}
		public void UpdateStaff(Staff staff)
		{
			if (this.staffNode.ContainsKey(staff.Uid))
			{
				if (this.staffNode[staff.Uid].UpdateInfo())
				{
					if (this.departmentNode.ContainsKey(staff.DepartmentId))
					{
						this.SortAllDepartment();
					}
				}
			}
		}
		public void ClearAllDepartmentOnline()
		{
			foreach (TreeViewItem tvi in this.departmentNode.Values)
			{
				Department department = tvi.DataContext as Department;
				if (department != null)
				{
					int count = this.CountStaffTotal(tvi);
					TextBlock nameBlock = new TextBlock();
					nameBlock.VerticalAlignment = VerticalAlignment.Center;
					nameBlock.Text = string.Concat(new object[]
					{
						department.Name,
						"[0/",
						count,
						"]"
					});
					tvi.Header = new StackPanel
					{
						Height = 22.0,
						Orientation = Orientation.Horizontal,
						Children = 
						{
							nameBlock
						}
					};
				}
			}
		}
		private int CountStaffTotal(TreeViewItem tvi)
		{
			int count = 0;
			for (int i = 0; i < tvi.Items.Count; i++)
			{
				TreeNodeStaff tree = tvi.Items[i] as TreeNodeStaff;
				if (tree != null)
				{
					count++;
				}
			}
			return count;
		}
		public void ChangeAllUserOffline()
		{
			foreach (TreeViewItem node in this.departmentNode.Values)
			{
				int count = node.Items.Count;
				for (int i = 0; i < count; i++)
				{
					TreeNodeStaff tns = node.Items[i] as TreeNodeStaff;
					if (tns != null)
					{
						tns.Staff.Status = UserStatus.Offline;
						tns.imgFaceForeground.Source = null;
						tns.Status = UserStatus.Offline;
					}
				}
			}
		}
		public void SortAllDepartment()
		{
			try
			{
				System.Collections.Generic.List<TreeViewItem> rootDepartments = new System.Collections.Generic.List<TreeViewItem>();
				foreach (TreeViewItem item in this.departmentNode.Values)
				{
					Department temDepartment = item.DataContext as Department;
					if (temDepartment != null)
					{
						if (temDepartment.Pid == this.currentRootId)
						{
							rootDepartments.Add(item);
						}
					}
					this.SortStatus(item);
				}
				foreach (TreeViewItem item in rootDepartments)
				{
					this.OneByOne(item);
				}
			}
			catch (System.Exception e)
			{
				System.Console.WriteLine(e.ToString());
			}
		}
		private void OneByOne(TreeViewItem departmentNode)
		{
			int[] temOnLineAndTotalCount = new int[2];
			int[] temOnLineAndTotalCountOld = departmentNode.Tag as int[];
			Department department = departmentNode.DataContext as Department;
			if (department != null)
			{
				temOnLineAndTotalCount = this.GetOnLineAndTotalCount(departmentNode, temOnLineAndTotalCount);
				this.UpdateDepartmentName(departmentNode, string.Concat(new object[]
				{
					department.Name,
					"[",
					temOnLineAndTotalCount[0],
					"/",
					temOnLineAndTotalCount[1],
					"]"
				}));
			}
			foreach (object item in (System.Collections.IEnumerable)departmentNode.Items)
			{
				TreeViewItem tem = item as TreeViewItem;
				if (tem != null)
				{
					this.OneByOne(tem);
				}
			}
		}
		private int[] GetOnLineAndTotalCount(TreeViewItem parentDepartment, int[] onLineAndTotalCount)
		{
			int[] temOnLineAndTotalCount = parentDepartment.Tag as int[];
			if (temOnLineAndTotalCount != null)
			{
				for (int i = 0; i < onLineAndTotalCount.Length; i++)
				{
					onLineAndTotalCount[i] += temOnLineAndTotalCount[i];
				}
			}
			foreach (object item in (System.Collections.IEnumerable)parentDepartment.Items)
			{
				TreeViewItem tem = item as TreeViewItem;
				if (tem != null)
				{
					this.GetOnLineAndTotalCount(tem, onLineAndTotalCount);
				}
			}
			return onLineAndTotalCount;
		}
		private void SortStatus(TreeViewItem parentNode)
		{
			System.Collections.Generic.IList<System.Collections.Generic.IList<TreeNodeStaff>> list = new System.Collections.Generic.List<System.Collections.Generic.IList<TreeNodeStaff>>();
			System.Collections.Generic.IList<TreeNodeStaff> nodes = null;
			int count = parentNode.Items.Count;
			int[] onLineAndTotalCount = new int[2];
			parentNode.Tag = onLineAndTotalCount;
			for (int i = 0; i < count; i++)
			{
				TreeNodeStaff node = parentNode.Items[i] as TreeNodeStaff;
				if (node != null)
				{
					if (nodes == null)
					{
						nodes = new System.Collections.Generic.List<TreeNodeStaff>();
					}
					nodes.Add(node);
				}
			}
			if (nodes != null && nodes.Count > 0)
			{
				System.Collections.Generic.IList<TreeNodeStaff> offline = null;
				System.Collections.Generic.IList<TreeNodeStaff> online = null;
				System.Collections.Generic.IList<TreeNodeStaff> away = null;
				System.Collections.Generic.IList<TreeNodeStaff> busy = null;
				System.Collections.Generic.IList<TreeNodeStaff> hide = null;
				System.Collections.Generic.IList<TreeNodeStaff> inme = null;
				System.Collections.Generic.IList<TreeNodeStaff> outting = null;
				System.Collections.Generic.IList<TreeNodeStaff> meeting = null;
				System.Collections.Generic.IList<TreeNodeStaff> doNotDisturb = null;
				foreach (TreeNodeStaff node in nodes)
				{
					switch (node.Status)
					{
					case UserStatus.Offline:
						if (offline == null)
						{
							offline = new System.Collections.Generic.List<TreeNodeStaff>();
						}
						offline.Add(node);
						break;
					case UserStatus.Online:
						if (online == null)
						{
							online = new System.Collections.Generic.List<TreeNodeStaff>();
						}
						online.Add(node);
						break;
					case UserStatus.Away:
						if (away == null)
						{
							away = new System.Collections.Generic.List<TreeNodeStaff>();
						}
						away.Add(node);
						break;
					case UserStatus.Busy:
						if (busy == null)
						{
							busy = new System.Collections.Generic.List<TreeNodeStaff>();
						}
						busy.Add(node);
						break;
					case UserStatus.Hide:
						if (hide == null)
						{
							hide = new System.Collections.Generic.List<TreeNodeStaff>();
						}
						hide.Add(node);
						break;
					case UserStatus.In:
						if (inme == null)
						{
							inme = new System.Collections.Generic.List<TreeNodeStaff>();
						}
						inme.Add(node);
						break;
					case UserStatus.Out:
						if (outting == null)
						{
							outting = new System.Collections.Generic.List<TreeNodeStaff>();
						}
						outting.Add(node);
						break;
					case UserStatus.Meeting:
						if (meeting == null)
						{
							meeting = new System.Collections.Generic.List<TreeNodeStaff>();
						}
						meeting.Add(node);
						break;
					case UserStatus.DoNotDisturb:
						if (doNotDisturb == null)
						{
							doNotDisturb = new System.Collections.Generic.List<TreeNodeStaff>();
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
				foreach (TreeNodeStaff tn in nodes)
				{
					parentNode.Items.Remove(tn);
				}
				foreach (System.Collections.Generic.IList<TreeNodeStaff> tlist in list)
				{
					foreach (TreeNodeStaff tn in tlist)
					{
						parentNode.Items.Add(tn);
					}
				}
				Department department = parentNode.DataContext as Department;
				if (department != null)
				{
					onLineAndTotalCount[0] = onlineCount;
					onLineAndTotalCount[1] = nodes.Count;
					parentNode.Tag = onLineAndTotalCount;
					this.UpdateDepartmentName(parentNode, string.Concat(new object[]
					{
						department.Name,
						"[",
						onlineCount,
						"/",
						nodes.Count,
						"]"
					}));
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
		private void SortUsername(System.Collections.Generic.IList<TreeNodeStaff> list)
		{
		}
		public void Clear()
		{
			this.departmentNode.Clear();
			this.staffNode.Clear();
			this.tree.Items.Clear();
		}
		private void treDepartment_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			TreeViewItem tvi = this.tree.SelectedItem as TreeViewItem;
			if (tvi != null)
			{
				tvi.IsExpanded = !tvi.IsExpanded;
			}
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/departmenttreeview.xaml", UriKind.Relative);
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
        //        this.tree = (DepartmentTreeView)target;
        //        this.tree.MouseLeftButtonUp += new MouseButtonEventHandler(this.treDepartment_MouseLeftButtonUp);
        //    }
        //}
	}
}
