using IDKin.IM.Core;
using IDKin.IM.Data;
using IDKin.IM.Log;
using IDKin.IM.Windows.Model;
using IDKin.IM.Windows.Model.MessageCenter;
using IDKin.IM.Windows.Util;
using IDKin.IM.Windows.View;
using IDKin.IM.Windows.ViewModel;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
namespace IDKin.IM.Windows.Comps.MessageCenter
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public partial class WorkTreeView : TreeView, System.IDisposable//, IComponentConnector
	{
		private INWindow inWindow = null;
		private INViewModel inViewModel = new INViewModel();
		private ILogger logger = ServiceUtil.Instance.Logger;
		private System.Collections.Generic.IDictionary<long, MessageCenterChatInfo> MessageCenter = new System.Collections.Generic.Dictionary<long, MessageCenterChatInfo>();
		private System.Collections.Generic.List<MessageCenterChatInfo> listmessage = LocalDataUtil.Instance.GetAllChatInfoBySort();
		private System.Collections.Generic.ICollection<Staff> listsatff = null;
		private IDataService dataService = ServiceUtil.Instance.DataService;
		private ISessionService sessionService = ServiceUtil.Instance.SessionService;
		private System.Collections.Generic.IDictionary<long, TreeViewItem> departmentNode = new System.Collections.Generic.Dictionary<long, TreeViewItem>();
		private System.Collections.Generic.IDictionary<long, MsgCenterTreeNode> staffNode = new System.Collections.Generic.Dictionary<long, MsgCenterTreeNode>();
		private System.Collections.Generic.IDictionary<long, MsgCenterTreeNode> GroupNode = new System.Collections.Generic.Dictionary<long, MsgCenterTreeNode>();
		private MsgCenterTreeNode node = null;
		private System.Collections.Generic.IList<Department> GetClickdepart = new System.Collections.Generic.List<Department>();
		//internal WorkTreeView treeview;
		//internal TreeViewItem WorkName;
		//internal TreeViewItem visitor;
		//internal TreeViewItem Provide;
		//internal TreeViewItem GroupName;
		//internal TreeViewItem discuss;
        ////private bool _contentLoaded;
		public System.Collections.Generic.IDictionary<long, MsgCenterTreeNode> StaffNode
		{
			get
			{
				return this.staffNode;
			}
		}
		public WorkTreeView()
		{
			this.InitializeComponent();
			this.ClearData();
			this.InitServer();
			this.ShowEntGroup();
			this.WorkName.Visibility = Visibility.Visible;
			this.WorkName.IsExpanded = true;
		}
		private void ClearData()
		{
			this.departmentNode.Clear();
			this.staffNode.Clear();
			this.GroupNode.Clear();
		}
		private void workTreeView_Loaded(object sender, RoutedEventArgs e)
		{
			WindowModel.Instance.RecentPage.InitData();
			WindowModel.Instance.MessageCenterWindow.ViewMsgFrame.NavigationService.Navigate(WindowModel.Instance.RecentPage);
		}
		public void InitServer()
		{
			try
			{
				this.inWindow = (this.dataService.INWindow as INWindow);
				this.DepartmentListEventHandle(this.dataService.GetDepartmentList());
				this.listsatff = this.dataService.GetStaffList();
			}
			catch (System.Exception)
			{
			}
		}
		public void AddDepartment(Department department)
		{
			TreeViewItem tvi = new TreeViewItem();
			TextBlock nameBlock = new TextBlock();
			nameBlock.VerticalAlignment = VerticalAlignment.Center;
			nameBlock.Text = department.Name;
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
				this.WorkName.Items.Add(tvi);
			}
			if (!this.departmentNode.ContainsKey(department.Id))
			{
				this.departmentNode.Add(department.Id, tvi);
			}
		}
		public void DepartmentListEventHandle(System.Collections.Generic.ICollection<Department> list)
		{
			try
			{
				if (list != null)
				{
					foreach (Department depart in list)
					{
						if (depart != null)
						{
							Department department = new Department();
							department.Id = depart.Id;
							department.Description = depart.Description;
							department.Name = depart.Name;
							department.Pid = depart.Pid;
							department.Uid = depart.Uid;
							this.dataService.AddDepartment(department);
							if (department.Id == this.sessionService.DepartmentId)
							{
								this.sessionService.DepartmentName = department.Name;
								this.inWindow.InitSelfDepartment();
							}
						}
					}
					this.ShowDepartment();
					System.Collections.Generic.ICollection<Staff> stafflist = this.dataService.GetStaffList();
					this.ShowStaff(stafflist);
				}
			}
			catch (System.Exception)
			{
			}
		}
		private void ShowDepartment()
		{
			System.Collections.Generic.ICollection<Department> departments = this.dataService.GetDepartmentList();
			if (departments != null && departments.Count > 0)
			{
				foreach (Department department in departments)
				{
					this.AddDepartment(department);
				}
			}
		}
		public void ShowStaff(System.Collections.Generic.ICollection<Staff> stafflist)
		{
			foreach (Staff staff in stafflist)
			{
				this.AddStaff(staff);
			}
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
						MsgCenterTreeNode node = new MsgCenterTreeNode(staff, false);
						node.SessionService = this.sessionService;
						node.DataService = this.dataService;
						if (!this.staffNode.ContainsKey(staff.Uid))
						{
							this.staffNode.Add(staff.Uid, node);
							this.departmentNode[staff.DepartmentId].Items.Add(node);
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
		private void ShowEntGroup()
		{
			System.Collections.Generic.ICollection<EntGroup> groups = null;
			try
			{
				groups = this.dataService.GetEntGroupList();
			}
			catch (System.Exception e)
			{
				ServiceUtil.Instance.Logger.Error(e.ToString());
			}
			if (groups != null && groups.Count > 0)
			{
				foreach (EntGroup group in groups)
				{
					if (!this.GroupNode.ContainsKey(group.Gid))
					{
						MsgCenterTreeNode node = new MsgCenterTreeNode(group, false);
						node.SessionService = this.sessionService;
						node.DataService = this.dataService;
						this.GroupNode.Add(group.Gid, node);
						this.GroupName.Items.Add(node);
					}
				}
				System.GC.Collect();
			}
		}
		public void NodeExpand(long id, bool IsStaff)
		{
			if (this.node != null)
			{
				this.node.Bd.Visibility = Visibility.Collapsed;
			}
			if (!IsStaff)
			{
				this.GroupName.IsExpanded = true;
				this.node = this.GroupNode[id];
				if (this.node != null)
				{
					this.node.Focusable = true;
					this.node.Focus();
					this.node.Bd.Visibility = Visibility.Visible;
				}
			}
			else
			{
				System.Collections.Generic.List<Department> list = new System.Collections.Generic.List<Department>();
				Staff staff = this.dataService.GetStaff(id);
				if (staff != null)
				{
					Department depart = this.dataService.GetDepartment(staff.DepartmentId);
					list.Add(depart);
					while (depart.Pid != 0L)
					{
						depart = this.dataService.GetDepartment(depart.Pid);
						if (depart == null)
						{
							break;
						}
						list.Add(depart);
					}
					for (int i = list.Count - 1; i >= 0; i--)
					{
						long temp = list[i].Id;
						this.departmentNode[temp].IsExpanded = true;
					}
					if (this.node != null)
					{
						this.node.Bd.Visibility = Visibility.Collapsed;
					}
					this.node = this.staffNode[id];
					if (this.node != null)
					{
						this.node.Focusable = true;
						this.node.Focus();
						this.node.Bd.Visibility = Visibility.Visible;
					}
				}
			}
		}
		private void treeview_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			TreeViewItem tvi = base.SelectedItem as TreeViewItem;
			if (tvi != null)
			{
				Department department = tvi.DataContext as Department;
				if (department != null)
				{
					WindowModel.Instance.RecentPage.recentList.Items.Clear();
					this.MessageCenter.Clear();
					if (this.listmessage != null)
					{
						for (int i = this.listmessage.Count - 1; i >= 0; i--)
						{
							if (this.listmessage[i] != null && !this.MessageCenter.ContainsKey(this.listmessage[i].Id))
							{
								Staff staff = this.IsDepartStaff(this.listmessage[i], department.Id);
								if (staff != null)
								{
									RecentContactsItem item = new RecentContactsItem(this.listmessage[i], false, false);
									if (item.IsValue)
									{
										this.MessageCenter.Add(this.listmessage[i].Id, this.listmessage[i]);
										if (this.sessionService.Uid != this.listmessage[i].Id)
										{
											WindowModel.Instance.RecentPage.recentList.Items.Add(item);
										}
									}
								}
							}
						}
					}
					WindowModel.Instance.MessageCenterWindow.ViewMsgFrame.NavigationService.Navigate(WindowModel.Instance.RecentPage);
					this.departmentNode[department.Id].IsExpanded = !this.departmentNode[department.Id].IsExpanded;
					System.GC.Collect();
				}
			}
		}
		private System.Collections.Generic.IList<Department> GetDepart(System.Collections.Generic.IList<long> id, System.Collections.Generic.ICollection<Department> departList)
		{
			System.Collections.Generic.IList<long> ID = new System.Collections.Generic.List<long>();
			if (id.Count > 0)
			{
				foreach (long lg in id)
				{
					foreach (Department dep in departList)
					{
						if (dep.Pid == lg)
						{
							this.GetClickdepart.Add(dep);
							ID.Add(dep.Id);
						}
					}
				}
				this.GetDepart(ID, departList);
			}
			return this.GetClickdepart;
		}
		private Staff IsDepartStaff(MessageCenterChatInfo infom, long Departid)
		{
			Staff staff = null;
			Department Currentdepart = this.dataService.GetDepartment(Departid);
			Staff result;
			foreach (Staff st in this.listsatff)
			{
				if (infom.Id == st.Uid && st.DepartmentId == Currentdepart.Id)
				{
					staff = (result = st);
					return result;
				}
			}
			System.Collections.Generic.ICollection<Department> departList = this.dataService.GetDepartmentList();
			System.Collections.Generic.IList<long> id = new System.Collections.Generic.List<long>();
			this.GetClickdepart.Clear();
			id.Add(Departid);
			System.Collections.Generic.IList<Department> depart = this.GetDepart(id, departList);
			foreach (Department dep in depart)
			{
				foreach (Staff st in this.listsatff)
				{
					if (infom.Id == st.Uid && st.DepartmentId == dep.Id)
					{
						staff = (result = st);
						return result;
					}
				}
			}
			result = staff;
			return result;
		}
		private void WorkName_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			TreeViewItem tvi = base.SelectedItem as TreeViewItem;
			if (tvi != null && "WorkName".Equals(tvi.Name))
			{
				WindowModel.Instance.RecentPage.recentList.Items.Clear();
				WindowModel.Instance.RecentPage.InitStaff();
				WindowModel.Instance.MessageCenterWindow.ViewMsgFrame.NavigationService.Navigate(WindowModel.Instance.RecentPage);
				this.WorkName.IsExpanded = !this.WorkName.IsExpanded;
			}
		}
		private void GroupName_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			TreeViewItem tvi = base.SelectedItem as TreeViewItem;
			if (tvi != null && "GroupName".Equals(tvi.Name))
			{
				WindowModel.Instance.RecentPage.recentList.Items.Clear();
				WindowModel.Instance.RecentPage.InitGroup();
				WindowModel.Instance.MessageCenterWindow.ViewMsgFrame.NavigationService.Navigate(WindowModel.Instance.RecentPage);
				this.GroupName.IsExpanded = !this.GroupName.IsExpanded;
			}
		}
		private void visitor_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			TreeViewItem tvi = base.SelectedItem as TreeViewItem;
			if (tvi != null && "visitor".Equals(tvi.Name))
			{
				WindowModel.Instance.RecentPage.recentList.Items.Clear();
				WindowModel.Instance.MessageCenterWindow.ViewMsgFrame.NavigationService.Navigate(WindowModel.Instance.RecentPage);
			}
		}
		private void Provide_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			TreeViewItem tvi = base.SelectedItem as TreeViewItem;
			if (tvi != null && "Provide".Equals(tvi.Name))
			{
				WindowModel.Instance.RecentPage.recentList.Items.Clear();
				WindowModel.Instance.MessageCenterWindow.ViewMsgFrame.NavigationService.Navigate(WindowModel.Instance.RecentPage);
			}
		}
		private void discuss_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			TreeViewItem tvi = base.SelectedItem as TreeViewItem;
			if (tvi != null && "discuss".Equals(tvi.Name))
			{
				WindowModel.Instance.RecentPage.recentList.Items.Clear();
				WindowModel.Instance.MessageCenterWindow.ViewMsgFrame.NavigationService.Navigate(WindowModel.Instance.RecentPage);
			}
		}
		private void WorkTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			if (this.node != null)
			{
				if (e.Source != this.node)
				{
					this.node.Bd.Visibility = Visibility.Collapsed;
				}
			}
		}
		public void Dispose()
		{
			this.listmessage.Clear();
			this.listsatff.Clear();
			this.staffNode.Clear();
			this.departmentNode.Clear();
			this.GroupNode.Clear();
			this.MessageCenter.Clear();
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/messagecenter/worktreeview.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.treeview = (WorkTreeView)target;
        //        this.treeview.Loaded += new RoutedEventHandler(this.workTreeView_Loaded);
        //        this.treeview.MouseLeftButtonUp += new MouseButtonEventHandler(this.treeview_MouseLeftButtonUp);
        //        this.treeview.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(this.WorkTreeView_SelectedItemChanged);
        //        break;
        //    case 2:
        //        this.WorkName = (TreeViewItem)target;
        //        this.WorkName.MouseLeftButtonUp += new MouseButtonEventHandler(this.WorkName_MouseLeftButtonUp);
        //        break;
        //    case 3:
        //        this.visitor = (TreeViewItem)target;
        //        this.visitor.MouseLeftButtonUp += new MouseButtonEventHandler(this.visitor_MouseLeftButtonUp);
        //        break;
        //    case 4:
        //        this.Provide = (TreeViewItem)target;
        //        this.Provide.MouseLeftButtonUp += new MouseButtonEventHandler(this.Provide_MouseLeftButtonUp);
        //        break;
        //    case 5:
        //        this.GroupName = (TreeViewItem)target;
        //        this.GroupName.MouseLeftButtonUp += new MouseButtonEventHandler(this.GroupName_MouseLeftButtonUp);
        //        break;
        //    case 6:
        //        this.discuss = (TreeViewItem)target;
        //        this.discuss.MouseLeftButtonUp += new MouseButtonEventHandler(this.discuss_MouseLeftButtonUp);
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
