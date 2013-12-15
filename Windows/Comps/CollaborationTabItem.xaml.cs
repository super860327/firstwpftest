using IDKin.IM.Core;
using IDKin.IM.Core.Core;
using IDKin.IM.Data;
using IDKin.IM.Protocol.Cooperation;
using IDKin.IM.Windows.Util;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
namespace IDKin.IM.Windows.Comps
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial  class CollaborationTabItem : TabItem//, IComponentConnector
	{
		private IDataService dataService = null;
        ////internal TextBlock tbkTip;
        ////internal TreeView TreeRoot;
        ////private bool _contentLoaded;
		public CollaborationTabItem()
		{
			this.InitializeComponent();
			this.dataService = ServiceUtil.Instance.DataService;
			this.Clear();
		}
		private void TabItem_Loaded(object sender, RoutedEventArgs e)
		{
			this.tbkTip.Text = "温馨提示：在协作里与人的聊天记录将会保存至对应项目的\"协作方交流\"中，协作双方所有成员均可查阅。";
		}
		private void InitUI()
		{
			this.TreeRoot.Style = (base.TryFindResource("TreeViewStyle") as Style);
		}
		public void Clear()
		{
			this.TreeRoot.Items.Clear();
			this.tbkTip.Visibility = Visibility.Visible;
			this.TreeRoot.Visibility = Visibility.Collapsed;
		}
		public void Add(CooperationProjectResponse resp)
		{
			this.TreeRoot.Visibility = Visibility.Visible;
			this.tbkTip.Visibility = Visibility.Collapsed;
			foreach (CooperationProject item in resp.cooperationProject)
			{
				CooperationProjectWrapper cooperationProjectWrapper = new CooperationProjectWrapper(item);
				if (!this.dataService.ContainsCooperationProjectWrapper(cooperationProjectWrapper.UnitedProjecid))
				{
					TreeViewItem cooperationProjectNode = new TreeViewItem();
					cooperationProjectNode.Style = (base.TryFindResource("TreeViewItemStyle") as Style);
					CollaborationTreeViewItem uscCooperationProject = new CollaborationTreeViewItem(item);
					cooperationProjectNode.Header = uscCooperationProject;
					this.TreeRoot.Items.Add(cooperationProjectNode);
					this.dataService.AddCooperationProjectWrapper(cooperationProjectWrapper);
					this.dataService.AddCooperationProjectTvi(cooperationProjectWrapper.UnitedProjecid, cooperationProjectNode);
				}
			}
		}
		public void Add(CooperationDockingResponse resp)
		{
			TreeViewItem parentCooperationProjectNode = null;
			CooperationProjectWrapper cooperationProjectWrapper = null;
			foreach (CooperationDocking item in resp.cooperationDocking)
			{
				if (parentCooperationProjectNode == null)
				{
					parentCooperationProjectNode = this.dataService.GetCooperationProjectTvi(item.projectId);
					if (parentCooperationProjectNode == null)
					{
						continue;
					}
				}
				if (cooperationProjectWrapper == null)
				{
					cooperationProjectWrapper = this.dataService.GetCooperationProjectWrapper(item.projectId);
					if (cooperationProjectWrapper == null)
					{
						continue;
					}
				}
				CooperationStaff cooperationStaff = new CooperationStaff(item);
				if (!this.dataService.ContainsCooperationStaff(cooperationStaff.Uid, cooperationStaff.UnitedProjectid))
				{
					this.dataService.AddCooperationStaff(cooperationStaff);
					CollaborationNodeStaff uscCollaborationNodeStaff = new CollaborationNodeStaff(cooperationStaff, cooperationProjectWrapper);
					TreeViewItem subCooperationStaffNode = new TreeViewItem();
					subCooperationStaffNode.Tag = cooperationStaff;
					subCooperationStaffNode.Style = (base.TryFindResource("TreeViewItemStyle") as Style);
					subCooperationStaffNode.Header = uscCollaborationNodeStaff;
					parentCooperationProjectNode.Items.Add(subCooperationStaffNode);
					this.dataService.AddCooperationStaffTvi(cooperationStaff.Uid, cooperationStaff.UnitedProjectid, subCooperationStaffNode);
				}
			}
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/collaboration/collaborationtabitem.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        ((CollaborationTabItem)target).Loaded += new RoutedEventHandler(this.TabItem_Loaded);
        //        break;
        //    case 2:
        //        this.tbkTip = (TextBlock)target;
        //        break;
        //    case 3:
        //        this.TreeRoot = (TreeView)target;
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
