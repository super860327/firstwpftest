using IDKin.IM.Core;
using IDKin.IM.Data;
using IDKin.IM.Log;
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
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
namespace IDKin.IM.Windows.Comps.MessageCenter
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public partial class MarkedTreeView : TreeView//, IComponentConnector
	{
		private ChatTabControlViewModel viewModel = new ChatTabControlViewModel();
		private ISessionService sessionService = ServiceUtil.Instance.SessionService;
		private IDataService dataService = ServiceUtil.Instance.DataService;
		private MessageCenterViewModel messageCenter = null;
		private System.Collections.Generic.IDictionary<long, TreeViewItem> departmentNode = new System.Collections.Generic.Dictionary<long, TreeViewItem>();
		private System.Collections.Generic.IDictionary<long, MsgCenterTreeNode> staffNode = new System.Collections.Generic.Dictionary<long, MsgCenterTreeNode>();
		private System.Collections.Generic.IDictionary<long, MsgCenterTreeNode> GroupNode = new System.Collections.Generic.Dictionary<long, MsgCenterTreeNode>();
		private System.Collections.Generic.IList<IDKin.IM.Core.Department> departlist = new System.Collections.Generic.List<IDKin.IM.Core.Department>();
		private System.Collections.Generic.ICollection<EntGroup> groups = null;
		private EntGroup group = null;
		private ILogger logger = ServiceUtil.Instance.Logger;
		private System.Collections.Generic.IList<IDKin.IM.Core.Department> GetClickdepart = new System.Collections.Generic.List<IDKin.IM.Core.Department>();
		//internal TreeViewItem WorkName;
		//internal TreeViewItem Victor;
		//internal TreeViewItem Provide;
		//internal TreeViewItem GroupName;
        ////private bool _contentLoaded;
		public MarkedTreeView()
		{
			try
			{
				this.InitializeComponent();
				this.WorkName.Items.Clear();
				this.GroupName.Items.Clear();
				this.ClearData();
				this.InitDate();
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		private void InitDate()
		{
			this.groups = this.dataService.GetEntGroupList();
			this.messageCenter = new MessageCenterViewModel();
		}
		public void ClearData()
		{
			try
			{
				this.staffNode.Clear();
				this.GroupNode.Clear();
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		public void AddMarkList(System.Collections.Generic.List<GetIsMark> getIsMark)
		{
			try
			{
				this.staffNode.Clear();
				this.departmentNode.Clear();
				this.departlist.Clear();
				System.Collections.Generic.ICollection<IDKin.IM.Core.Staff> stafflist = this.dataService.GetStaffList();
				IDKin.IM.Core.Staff staff = null;
				for (int i = 0; i < getIsMark.Count; i++)
				{
					foreach (IDKin.IM.Core.Staff st in stafflist)
					{
						if (st.Uid == getIsMark[i].id)
						{
							staff = st;
							break;
						}
					}
					if (staff != null)
					{
						System.Threading.Monitor.Enter(this.staffNode);
						if (!this.staffNode.ContainsKey(staff.Uid) && this.sessionService.Uid != staff.Uid)
						{
							MsgCenterTreeNode node = new MsgCenterTreeNode(staff, true);
							node.SessionService = this.sessionService;
							node.DataService = this.dataService;
							if (!this.staffNode.ContainsKey(staff.Uid))
							{
								this.staffNode.Add(staff.Uid, node);
								this.AddDepart(staff.Uid);
								this.departmentNode[staff.DepartmentId].Items.Add(node);
							}
						}
						System.Threading.Monitor.Exit(this.staffNode);
					}
					staff = null;
				}
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		public void AddGroupList(System.Collections.Generic.List<GetIsMark> getIsMark)
		{
			try
			{
				this.GroupName.Items.Clear();
				this.GroupNode.Clear();
				for (int i = 0; i < getIsMark.Count; i++)
				{
					foreach (EntGroup gp in this.groups)
					{
						if (gp.Gid == getIsMark[i].id)
						{
							this.group = gp;
							break;
						}
					}
					if (this.group != null)
					{
						if (!this.GroupNode.ContainsKey(this.group.Gid))
						{
							MsgCenterTreeNode node = new MsgCenterTreeNode(this.group, true);
							node.SessionService = this.sessionService;
							node.DataService = this.dataService;
							this.GroupNode.Add(this.group.Gid, node);
							this.GroupName.Items.Add(node);
						}
					}
					this.group = null;
				}
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		private void AddDepart(long id)
		{
			try
			{
				IDKin.IM.Core.Staff staff = this.dataService.GetStaff(id);
				if (staff != null)
				{
					System.Collections.Generic.List<IDKin.IM.Core.Department> departs = this.GetDepart(id);
					for (int i = departs.Count - 1; i >= 0; i--)
					{
						TreeViewItem tvi = new TreeViewItem();
						TextBlock nameBlock = new TextBlock();
						nameBlock.VerticalAlignment = VerticalAlignment.Center;
						nameBlock.Text = departs[i].Name;
						tvi.Header = new StackPanel
						{
							Height = 22.0,
							Orientation = Orientation.Horizontal,
							Children = 
							{
								nameBlock
							}
						};
						tvi.DataContext = departs[i];
						if (!this.departmentNode.ContainsKey(departs[i].Id))
						{
							if (departs[i].Pid == 1L && !this.departmentNode.ContainsKey(departs[i].Id))
							{
								this.WorkName.Items.Add(tvi);
							}
							else
							{
								this.departmentNode[departs[i].Pid].Items.Add(tvi);
							}
						}
						if (!this.departmentNode.ContainsKey(departs[i].Id))
						{
							this.departmentNode.Add(departs[i].Id, tvi);
							this.departlist.Add(departs[i]);
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		private System.Collections.Generic.List<IDKin.IM.Core.Department> GetDepart(long id)
		{
			System.Collections.Generic.List<IDKin.IM.Core.Department> listdepart = new System.Collections.Generic.List<IDKin.IM.Core.Department>();
			System.Collections.Generic.ICollection<IDKin.IM.Core.Department> Departs = this.dataService.GetDepartmentList();
			IDKin.IM.Core.Staff staff = this.dataService.GetStaff(id);
			if (staff != null)
			{
				IDKin.IM.Core.Department temp = this.dataService.GetDepartment(staff.DepartmentId);
				if (temp != null)
				{
					listdepart.Add(temp);
					while (temp != null)
					{
						temp = this.dataService.GetDepartment(temp.Pid);
						if (temp != null)
						{
							listdepart.Add(temp);
						}
					}
				}
			}
			return listdepart;
		}
		private void WorkName_Expanded(object sender, RoutedEventArgs e)
		{
			try
			{
				if (e.Source == this.WorkName)
				{
					WindowModel.Instance.MarkTreeView = this;
					WindowModel.Instance.Markpage.markedList.Items.Clear();
					WindowModel.Instance.MarkTreeView.WorkName.Items.Clear();
					this.messageCenter.SendMarkByType(this.sessionService.Uid, IsMarkType.FELLOW);
					WindowModel.Instance.MessageCenterWindow.ViewMsgFrame.NavigationService.Navigate(WindowModel.Instance.Markpage);
				}
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		private void GroupName_Expanded(object sender, RoutedEventArgs e)
		{
			try
			{
				System.Collections.Generic.List<long> list = this.GetGroupGid();
				WindowModel.Instance.MarkTreeView = this;
				WindowModel.Instance.Markpage.markedList.Items.Clear();
				this.messageCenter.SendGroupMarkByType(list, IsMarkType.GROUP, this.sessionService.Uid);
				WindowModel.Instance.MessageCenterWindow.ViewMsgFrame.NavigationService.Navigate(WindowModel.Instance.Markpage);
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		private void WorkName_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			try
			{
				if (e.Source == this.WorkName)
				{
					this.WorkName.IsExpanded = !this.WorkName.IsExpanded;
				}
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		private void GroupName_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			try
			{
				if (e.Source == this.GroupName)
				{
					this.GroupName.IsExpanded = !this.GroupName.IsExpanded;
				}
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
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
		private void Provide_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			try
			{
				WindowModel.Instance.Markpage.markedList.Items.Clear();
				WindowModel.Instance.MessageCenterWindow.ViewMsgFrame.NavigationService.Navigate(WindowModel.Instance.Markpage);
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		private void Victor_MouseButtonUp(object sender, System.EventArgs e)
		{
			try
			{
				WindowModel.Instance.Markpage.markedList.Items.Clear();
				WindowModel.Instance.MessageCenterWindow.ViewMsgFrame.NavigationService.Navigate(WindowModel.Instance.Markpage);
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		private void DepartTreeView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			try
			{
				TreeViewItem tvi = base.SelectedItem as TreeViewItem;
				if (tvi != null)
				{
					IDKin.IM.Core.Department department = tvi.DataContext as IDKin.IM.Core.Department;
					if (department != null)
					{
						WindowModel.Instance.Markpage.markedList.Items.Clear();
						if (department != null)
						{
							System.Collections.Generic.List<IDKin.IM.Core.Staff> addstafflist = new System.Collections.Generic.List<IDKin.IM.Core.Staff>();
							System.Collections.Generic.ICollection<MsgCenterTreeNode> getstafflist = this.staffNode.Values;
							foreach (MsgCenterTreeNode node in getstafflist)
							{
								IDKin.IM.Core.Staff staff = this.IsDepartStaff(node, department.Id);
								if (staff != null)
								{
									addstafflist.Add(staff);
								}
							}
							if (addstafflist.Count > 0)
							{
								WindowModel.Instance.Markpage.ShowDepartList(addstafflist);
							}
						}
						WindowModel.Instance.MessageCenterWindow.ViewMsgFrame.NavigationService.Navigate(WindowModel.Instance.Markpage);
						this.departmentNode[department.Id].IsExpanded = !this.departmentNode[department.Id].IsExpanded;
					}
				}
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		private System.Collections.Generic.IList<IDKin.IM.Core.Department> GetDepart(System.Collections.Generic.IList<long> id, System.Collections.Generic.IList<IDKin.IM.Core.Department> departList)
		{
			System.Collections.Generic.IList<long> ID = new System.Collections.Generic.List<long>();
			if (id.Count > 0)
			{
				foreach (long lg in id)
				{
					foreach (IDKin.IM.Core.Department dep in departList)
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
		private IDKin.IM.Core.Staff IsDepartStaff(MsgCenterTreeNode infom, long Departid)
		{
			IDKin.IM.Core.Staff staff = null;
			System.Collections.Generic.ICollection<MsgCenterTreeNode> stafflist = this.staffNode.Values;
			IDKin.IM.Core.Department Currentdepart = this.dataService.GetDepartment(Departid);
			IDKin.IM.Core.Staff result;
			foreach (MsgCenterTreeNode node in stafflist)
			{
				if (node.Staff.Uid == infom.Staff.Uid && node.Staff.DepartmentId == Departid)
				{
					staff = (result = node.Staff);
					return result;
				}
			}
			System.Collections.Generic.IList<long> id = new System.Collections.Generic.List<long>();
			this.GetClickdepart.Clear();
			id.Add(Departid);
			System.Collections.Generic.IList<IDKin.IM.Core.Department> depart = this.GetDepart(id, this.departlist);
			foreach (IDKin.IM.Core.Department dep in depart)
			{
				foreach (MsgCenterTreeNode node in stafflist)
				{
					if (node.Staff.Uid == infom.Staff.Uid && node.Staff.DepartmentId == dep.Id)
					{
						staff = (result = node.Staff);
						return result;
					}
				}
			}
			result = staff;
			return result;
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/messagecenter/markedtreeview.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        ((MarkedTreeView)target).MouseLeftButtonUp += new MouseButtonEventHandler(this.DepartTreeView_MouseLeftButtonUp);
        //        break;
        //    case 2:
        //        this.WorkName = (TreeViewItem)target;
        //        this.WorkName.Expanded += new RoutedEventHandler(this.WorkName_Expanded);
        //        this.WorkName.MouseLeftButtonUp += new MouseButtonEventHandler(this.WorkName_MouseLeftButtonUp);
        //        break;
        //    case 3:
        //        this.Victor = (TreeViewItem)target;
        //        this.Victor.MouseLeftButtonUp += new MouseButtonEventHandler(this.Victor_MouseButtonUp);
        //        break;
        //    case 4:
        //        this.Provide = (TreeViewItem)target;
        //        this.Provide.MouseLeftButtonUp += new MouseButtonEventHandler(this.Provide_MouseLeftButtonUp);
        //        break;
        //    case 5:
        //        this.GroupName = (TreeViewItem)target;
        //        this.GroupName.Expanded += new RoutedEventHandler(this.GroupName_Expanded);
        //        this.GroupName.MouseLeftButtonUp += new MouseButtonEventHandler(this.GroupName_MouseLeftButtonUp);
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
