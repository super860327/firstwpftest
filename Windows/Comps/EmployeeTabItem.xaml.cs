using IDKin.IM.Core;
using IDKin.IM.Data;
using IDKin.IM.ImageService;
using IDKin.IM.Log;
using IDKin.IM.Protocol.Commons;
using IDKin.IM.Windows.Model;
using IDKin.IM.Windows.Model.RecentLink;
using IDKin.IM.Windows.Util;
using IDKin.IM.Windows.View;
using IDKin.IM.Windows.ViewModel;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
namespace IDKin.IM.Windows.Comps
{
	public partial class EmployeeTabItem : TabItem//, IComponentConnector, IStyleConnector
	{
		public delegate void AddRecentLinkDelegate(System.Collections.Generic.List<RecentLinkInfo> list);
		private IImageService imageService;
		private ISessionService sessionService;
		private IDataService dataService;
		private ILogger logger;
		private CustomGroupManagerWindowViewModel customGroupManagerViewModel;
		//internal RowDefinition DepartmentRow;
		//internal RowDefinition OrganizationRow;
		//internal RowDefinition EnterpriseGroupRow;
		//internal TreeView FirstTreeView;
		//internal RecentLinkListItem RecentLinkListItem;
		//internal SelfDepartment SelfDepartmentItem;
		//internal Expander OrgExpander;
		//internal TextBlock tbkEmpCount;
		//internal DepartmentTreeView DepartmentTree;
		//internal StatusBar GridHideBar;
		//internal Button btnAddGroup;
		//internal ContextMenu treEntGroupContextMenu;
		//internal Path pathGridHide;
		//internal EnterpriseGroupListView listEntGroup;
        ////private bool _contentLoaded;
		public EmployeeTabItem()
		{
			this.InitializeComponent();
			this.dataService = ServiceUtil.Instance.DataService;
			this.sessionService = ServiceUtil.Instance.SessionService;
			this.customGroupManagerViewModel = CustomGroupManagerWindowViewModel.GetInstance();
			this.FirstTreeView.ContextMenu = this.GetContextMenuForTree();
			this.FirstTreeView.ContextMenu.Visibility = Visibility.Hidden;
			this.customGroupManagerViewModel.AddMemberToCustomGroupEvent += new CustomGroupManagerWindowViewModel.MemberToCustomGroup(this.customGroupManagerViewModel_AddMemberToCustomGroupEvent);
			this.FirstTreeView.Focusable = true;
			this.FirstTreeView.MouseLeftButtonDown += new MouseButtonEventHandler(this.FirstTreeView_MouseLeftButtonDown);
		}
		private void customGroupManagerViewModel_AddMemberToCustomGroupEvent(object sender, CustomGroupManagerWindowViewModel.MemberToCustomGroupEventArgs args)
		{
			string groupId = args.GroupId;
			System.Collections.Generic.List<Staff> staffs = new System.Collections.Generic.List<Staff>();
			if (!string.IsNullOrEmpty(args.Uid))
			{
				staffs = new System.Collections.Generic.List<Staff>();
				string[] Uids = args.Uid.Split(new char[]
				{
					':'
				});
				string[] array = Uids;
				for (int i = 0; i < array.Length; i++)
				{
					string id = array[i];
					if (!string.IsNullOrEmpty(id))
					{
						Staff staff = this.dataService.GetStaff((long)int.Parse(id));
						staffs.Add(staff);
					}
				}
			}
			if (staffs != null)
			{
				this.Sort(staffs, groupId);
			}
		}
		private void Sort(System.Collections.Generic.List<Staff> staffs, string groupId)
		{
			foreach (TreeViewItem item in (System.Collections.IEnumerable)this.FirstTreeView.Items)
			{
				if (item is CustomGroupTreeViewItem)
				{
					if (item.DataContext is CustomGroup)
					{
						if ((item.DataContext as CustomGroup).GroupID == groupId)
						{
							item.Items.Clear();
							if (staffs.Count > 0)
							{
								IOrderedEnumerable<Staff> v = 
									from t in staffs
									orderby t.Status == UserStatus.Offline, t.Status == UserStatus.Hide, t.Status == UserStatus.Out, t.Status == UserStatus.DoNotDisturb, t.Status == UserStatus.Busy, t.Status == UserStatus.Meeting, t.Status == UserStatus.Away, t.Status == UserStatus.Online
									select t;
								foreach (Staff staff in v)
								{
									TreeNodeCustomStaff node = new TreeNodeCustomStaff(staff);
									node.SessionService = this.sessionService;
									node.DataService = this.dataService;
									node.ContextMenu = new ContextMenu();
									MenuItem menuItem = new MenuItem();
									menuItem.Header = "将联系人从该组移除";
									menuItem.Tag = groupId;
									menuItem.DataContext = node.Staff;
									node.ContextMenu.Items.Add(menuItem);
									menuItem.Click += new RoutedEventHandler(this.menuItem_Click);
									item.Items.Add(node);
								}
								staffs.Clear();
							}
							break;
						}
					}
				}
			}
		}
		public void ClearCustomGroup()
		{
			for (int i = this.FirstTreeView.Items.Count - 1; i >= 0; i--)
			{
				CustomGroupTreeViewItem customGroupTreeViewItem = this.FirstTreeView.Items[i] as CustomGroupTreeViewItem;
				if (customGroupTreeViewItem != null)
				{
					this.FirstTreeView.Items.Remove(customGroupTreeViewItem);
				}
			}
		}
		private void menuItem_Click(object sender, RoutedEventArgs e)
		{
			MenuItem menuItem = sender as MenuItem;
			if (menuItem != null)
			{
				Staff staff = menuItem.DataContext as Staff;
				string groupId = string.Empty;
				if (staff != null && menuItem.Tag != null)
				{
					groupId = menuItem.Tag.ToString();
					foreach (TreeViewItem item in (System.Collections.IEnumerable)this.FirstTreeView.Items)
					{
						if (item is CustomGroupTreeViewItem)
						{
							if (item.DataContext is CustomGroup)
							{
								if (item.DataContext is CustomGroup && (item.DataContext as CustomGroup).GroupID == groupId)
								{
									for (int i = 0; i < item.Items.Count; i++)
									{
										if ((item.Items[i] as TreeNodeCustomStaff).Staff.Uid == staff.Uid)
										{
											CustomGroupManagerWindowViewModel vmodel = CustomGroupManagerWindowViewModel.GetInstance();
											DataModel.Instance.CustomeGroupName[groupId].Members.Remove(staff);
											vmodel.DeleteMemberToCustomGroup((int)this.sessionService.Uid, groupId, this.GetMembers(DataModel.Instance.CustomeGroupName[groupId]));
											item.Items.Remove(item.Items[i]);
											break;
										}
									}
								}
							}
						}
					}
				}
			}
		}
		private string GetMembers(CustomGroup c)
		{
			string result = string.Empty;
			string result2;
			if (c == null || c.Members == null)
			{
				result2 = result;
			}
			else
			{
				foreach (Staff item in c.Members)
				{
					result = result + item.Uid + ":";
				}
				result2 = result;
			}
			return result2;
		}
		private void TabItem_Loaded(object sender, RoutedEventArgs e)
		{
			this.InitUI();
			this.btnAddGroup.ContextMenu = null;
			this.RecentLinkListItem.InitService();
			this.SelfDepartmentItem.InitService();
		}
		private void InitUI()
		{
			this.imageService = ServiceUtil.Instance.ImageService;
			this.logger = ServiceUtil.Instance.Logger;
			this.dataService = ServiceUtil.Instance.DataService;
			this.sessionService = ServiceUtil.Instance.SessionService;
			DependencyObject d = VisualTreeHelper.GetChild(this.btnAddGroup, 0);
			Image img = LogicalTreeHelper.FindLogicalNode(d, "PART_icon") as Image;
			if (this.imageService != null)
			{
				img.Source = this.imageService.GetIcon(ImageTypeIcon.GroupAdd);
			}
		}
		private void OrgExpander_Expanded(object sender, RoutedEventArgs e)
		{
			this.FirstTreeView.Visibility = Visibility.Collapsed;
			this.DepartmentTree.Visibility = Visibility.Visible;
			this.OrganizationRow.Height = new GridLength(0.0);
			Grid.SetRow(this.OrgExpander, 0);
		}
		private void OrgExpander_Collapsed(object sender, RoutedEventArgs e)
		{
			this.DepartmentTree.Visibility = Visibility.Collapsed;
			this.OrganizationRow.Height = new GridLength(25.0);
			this.FirstTreeView.Visibility = Visibility.Visible;
			Grid.SetRow(this.OrgExpander, 1);
		}
		private void OrgExpander_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				this.OrgExpanderHandler();
			}
		}
		private void OrgExpanderHandler()
		{
			if (this.OrganizationRow.Height == new GridLength(25.0))
			{
				this.FirstTreeView.Visibility = Visibility.Collapsed;
				this.DepartmentTree.Visibility = Visibility.Visible;
				this.OrganizationRow.Height = new GridLength(0.0);
				Grid.SetRow(this.OrgExpander, 0);
				this.OrgExpander.IsExpanded = true;
			}
			else
			{
				this.DepartmentTree.Visibility = Visibility.Collapsed;
				this.OrganizationRow.Height = new GridLength(25.0);
				this.FirstTreeView.Visibility = Visibility.Visible;
				Grid.SetRow(this.OrgExpander, 1);
				this.OrgExpander.IsExpanded = false;
			}
		}
		private void GridHideBar_Initialized(object sender, System.EventArgs e)
		{
			this.GridHideBar.ToolTip = ((this.EnterpriseGroupRow.Height == new GridLength(25.0)) ? "显示企业内群" : "隐藏企业内群");
		}
		private void GridHideBar_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				this.EntGroupExpanderHandler();
			}
		}
		private void EntGroupExpanderHandler()
		{
			try
			{
				if (this.EnterpriseGroupRow.Height == new GridLength(25.0))
				{
					this.EnterpriseGroupRow.Height = new GridLength(136.0);
					this.GridHideBar.ToolTip = "隐藏企业内群";
					this.pathGridHide.Data = Geometry.Parse("M 0 0 L 4 4 L 8 0 Z");
				}
				else
				{
					this.EnterpriseGroupRow.Height = new GridLength(25.0);
					this.GridHideBar.ToolTip = "显示企业内群";
					this.pathGridHide.Data = Geometry.Parse("M 0 4 L 4 0 L 8 4 Z");
				}
			}
			catch (System.Exception)
			{
			}
		}
		private void btnAddGroup_Click(object sender, RoutedEventArgs e)
		{
			this.treEntGroupContextMenu.PlacementTarget = this.btnAddGroup;
			this.treEntGroupContextMenu.Placement = PlacementMode.Bottom;
			this.treEntGroupContextMenu.IsOpen = true;
		}
		private void treEntGroupMenuItem_Click(object sender, RoutedEventArgs e)
		{
			string text = (sender as MenuItem).DataContext.ToString();
			if (text != null)
			{
				if (!(text == "add"))
				{
					if (!(text == "update"))
					{
						if (!(text == "delete"))
						{
						}
					}
				}
				else
				{
					this.ShowManagerWindow();
				}
			}
		}
		private void ShowManagerWindow()
		{
			WindowModel.Instance.CreateOrAtcivateEntGroupManagerWindow(null, OperationType.Add);
		}
		private EntGroupManagerWindow GetManagerWindow(bool create = false)
		{
			return WindowModel.Instance.GroupManagerWindow;
		}
		private void RecentLinkListItem_Expanded(object sender, RoutedEventArgs e)
		{
			if (DataModel.Instance.RecentLinkInfoList.Count > 0)
			{
				this.RecentLinkProcessor();
			}
		}
		private void RecentLinkProcessor()
		{
			try
			{
				System.Collections.Generic.List<RecentLinkInfo> list = DataModel.Instance.RecentLinkInfoList;
				if (list != null && list.Count > 0)
				{
					base.Dispatcher.BeginInvoke(new EmployeeTabItem.AddRecentLinkDelegate(this.AddRecentLinkDelegateHandle), new object[]
					{
						list
					});
				}
			}
			catch (System.Exception e)
			{
				this.logger.Debug(e.ToString());
				this.logger.Error(e.ToString());
			}
		}
		private long[] StringArrayLongArray(string[] uids)
		{
			long[] userIds = new long[uids.Length];
			for (int i = 0; i < uids.Length; i++)
			{
				try
				{
					if (!string.IsNullOrEmpty(uids[i]))
					{
						userIds[i] = (long)((ulong)System.Convert.ToUInt32(uids[i]));
					}
					else
					{
						userIds[i] = 0L;
					}
				}
				catch (System.Exception)
				{
					userIds[i] = 0L;
				}
			}
			return userIds;
		}
		private void AddRecentLinkDelegateHandle(System.Collections.Generic.List<RecentLinkInfo> list)
		{
			try
			{
				this.RecentLinkListItem.AddRecentLinkList(IsMarkType.FELLOW, list);
				DataModel.Instance.RecentLinkInfoList.Clear();
			}
			catch (System.Exception e)
			{
				this.logger.Error(e.ToString());
			}
		}
		private void FirstTreeView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			TreeViewItem tvi = this.FirstTreeView.SelectedItem as TreeViewItem;
			if (tvi != null)
			{
				tvi.IsExpanded = !tvi.IsExpanded;
			}
		}
		private ContextMenu GetContextMenuForTree()
		{
			ContextMenu cMenu = new ContextMenu();
			MenuItem item = new MenuItem();
			item.Header = "添加分组";
			item.Click += new RoutedEventHandler(this.miAddGroup_Click);
			cMenu.Items.Add(item);
			return cMenu;
		}
		private void miAddGroup_Click(object sender, RoutedEventArgs e)
		{
			CustomGroupTreeViewItem item = new CustomGroupTreeViewItem();
			item.Style = (base.FindResource("TreeViewItemStyle") as Style);
			item.CreateCustomItemEvent += delegate(object s, CustomGroupTreeViewItem.CustomEventArgs ee)
			{
				if (ee.Item != null)
				{
					this.FirstTreeView.Items.Add(ee.Item);
					string id = CustomGroup.GetCustomGroupID();
					DataModel.Instance.CustomeGroupName.Add(id, new CustomGroup
					{
						GroupID = id,
						GroupName = ee.Item.HeaderText
					});
					this.customGroupManagerViewModel.CreateCustomGroup(this.sessionService.Uid, id, ee.Item.HeaderText);
				}
			};
			item.DeleteCustomItemEvent += delegate(object s, CustomGroupTreeViewItem.CustomEventArgs ee)
			{
				DataModel.Instance.CustomeGroupName.Remove(item.Tag.ToString());
				this.FirstTreeView.Items.Remove(item);
				this.customGroupManagerViewModel.DropCustomGroup((int)this.sessionService.Uid, item.Tag.ToString());
			};
			item.UpdateCustomItemEvent += delegate(object s, CustomGroupTreeViewItem.CustomEventArgs ee)
			{
				if (DataModel.Instance.CustomeGroupName.ContainsKey(item.Tag.ToString()))
				{
					DataModel.Instance.CustomeGroupName[item.Tag.ToString()].GroupName = item.HeaderText;
					this.customGroupManagerViewModel.UpdateCustomGroup((int)this.sessionService.Uid, item.Tag.ToString(), item.HeaderText);
				}
			};
			CustomGroup c = new CustomGroup
			{
				Admin = this.sessionService.Uid,
				GroupID = CustomGroup.GetCustomGroupID(),
				GroupName = item.HeaderText
			};
			item.DataContext = c;
			item.Tag = c.GroupID;
			this.FirstTreeView.Items.Add(item);
			DataModel.Instance.CustomeGroupName.Add(c.GroupID, c);
			this.customGroupManagerViewModel.CreateCustomGroup(this.sessionService.Uid, c.GroupID, item.HeaderText);
			this.SetFocus(item);
			item.IsSelected = true;
		}
		private void SetFocus(CustomGroupTreeViewItem item)
		{
			Grid grid = item.Header as Grid;
			if (grid != null)
			{
				if (grid.Children.Count > 1 && grid.Children[1] is TextBox)
				{
					TextBox textBox = grid.Children[1] as TextBox;
					textBox.Visibility = Visibility.Visible;
					textBox.SelectAll();
					textBox.Focus();
				}
			}
		}
		public void UpdateStaff(Staff staff)
		{
			foreach (TreeViewItem item in (System.Collections.IEnumerable)this.FirstTreeView.Items)
			{
				CustomGroupTreeViewItem customGroupTreeViewItem = item as CustomGroupTreeViewItem;
				if (customGroupTreeViewItem != null)
				{
					foreach (object cItem in (System.Collections.IEnumerable)item.Items)
					{
						TreeNodeCustomStaff treeNodeCustomStaff = cItem as TreeNodeCustomStaff;
						if (treeNodeCustomStaff != null)
						{
							treeNodeCustomStaff.UpdateInfo();
						}
					}
				}
			}
		}
		public void FirstTreeView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			base.Focus();
		}
		public void UpdateDepartmentStaffCount(int onlineStaff, int totalStaff)
		{
			this.tbkEmpCount.Text = string.Concat(new string[]
			{
				"[",
				onlineStaff.ToString(),
				"/",
				totalStaff.ToString(),
				"]"
			});
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/employeetabitem.xaml", UriKind.Relative);
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
        //        ((EmployeeTabItem)target).Loaded += new RoutedEventHandler(this.TabItem_Loaded);
        //        return;
        //    case 3:
        //        this.DepartmentRow = (RowDefinition)target;
        //        return;
        //    case 4:
        //        this.OrganizationRow = (RowDefinition)target;
        //        return;
        //    case 5:
        //        this.EnterpriseGroupRow = (RowDefinition)target;
        //        return;
        //    case 6:
        //        this.FirstTreeView = (TreeView)target;
        //        this.FirstTreeView.MouseLeftButtonUp += new MouseButtonEventHandler(this.FirstTreeView_MouseLeftButtonUp);
        //        return;
        //    case 7:
        //        this.RecentLinkListItem = (RecentLinkListItem)target;
        //        return;
        //    case 8:
        //        this.SelfDepartmentItem = (SelfDepartment)target;
        //        return;
        //    case 9:
        //        this.OrgExpander = (Expander)target;
        //        this.OrgExpander.Expanded += new RoutedEventHandler(this.OrgExpander_Expanded);
        //        this.OrgExpander.Collapsed += new RoutedEventHandler(this.OrgExpander_Collapsed);
        //        return;
        //    case 10:
        //        this.tbkEmpCount = (TextBlock)target;
        //        return;
        //    case 11:
        //        this.DepartmentTree = (DepartmentTreeView)target;
        //        return;
        //    case 12:
        //        this.GridHideBar = (StatusBar)target;
        //        this.GridHideBar.Initialized += new System.EventHandler(this.GridHideBar_Initialized);
        //        this.GridHideBar.MouseDown += new MouseButtonEventHandler(this.GridHideBar_MouseDown);
        //        return;
        //    case 13:
        //        this.btnAddGroup = (Button)target;
        //        this.btnAddGroup.Click += new RoutedEventHandler(this.btnAddGroup_Click);
        //        return;
        //    case 14:
        //        this.treEntGroupContextMenu = (ContextMenu)target;
        //        return;
        //    case 15:
        //        ((MenuItem)target).Click += new RoutedEventHandler(this.treEntGroupMenuItem_Click);
        //        return;
        //    case 16:
        //        ((MenuItem)target).Click += new RoutedEventHandler(this.treEntGroupMenuItem_Click);
        //        return;
        //    case 17:
        //        this.pathGridHide = (Path)target;
        //        this.pathGridHide.MouseDown += new MouseButtonEventHandler(this.GridHideBar_MouseDown);
        //        return;
        //    case 18:
        //        this.listEntGroup = (EnterpriseGroupListView)target;
        //        return;
        //    }
        //    this._contentLoaded = true;
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IStyleConnector.Connect(int connectionId, object target)
        //{
        //    if (connectionId == 2)
        //    {
        //        ((ContentPresenter)target).MouseDown += new MouseButtonEventHandler(this.OrgExpander_MouseDown);
        //    }
        //}
	}
}
