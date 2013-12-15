using IDKin.IM.Data;
using IDKin.IM.Windows.Comps.OA;
using IDKin.IM.Windows.Util;
using IDKin.IM.Windows.View.Pages;
using IDKin.IM.Windows.ViewModel;
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
    public partial class WorkDesktopControl : UserControl//, IComponentConnector
	{
		private ISessionService sessionService;
		private INViewModel inViewModel;
		private SelfProfilePage selPage;
		private bool isChecked = false;
		//internal TabControl tabControl;
		//internal OAWorkflowTabItem OAWorkflowTabItem;
		//internal OAPlanTabItem OAPlanTabItem;
		//internal OANoticeTabItem OANoticeTabItem;
		//internal TabItem profireItem;
		//internal Frame selfProfileFrame;
        ////private bool _contentLoaded;
		public SelfProfilePage SelPage
		{
			get
			{
				return this.selPage;
			}
		}
		public WorkDesktopControl()
		{
			this.InitializeComponent();
			base.Loaded += new RoutedEventHandler(this.WorkDesktopControl_Loaded);
		}
		private void WorkDesktopControl_Loaded(object sender, RoutedEventArgs e)
		{
			this.InitUI();
			this.InitData();
		}
		private void InitUI()
		{
			this.selPage = new SelfProfilePage();
		}
		private void InitData()
		{
			this.inViewModel = new INViewModel();
			this.sessionService = ServiceUtil.Instance.SessionService;
		}
		public void ClearData()
		{
			this.OAWorkflowTabItem.WorkflowDataGrid.Items.Clear();
			this.OAPlanTabItem.PlanDataGrid.Items.Clear();
			this.OANoticeTabItem.NoticeDataGrid.Items.Clear();
		}
		private void ChatTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (this.tabControl.SelectedIndex == 3)
			{
				if (!this.isChecked && this.selPage != null)
				{
					this.isChecked = true;
					this.selfProfileFrame.NavigationService.Navigate(this.SelPage);
					this.inViewModel.GetStaffInfo(this.sessionService.Uid);
				}
			}
			else
			{
				this.isChecked = false;
			}
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/workdesktopcontrol.xaml", UriKind.Relative);
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
        //        this.tabControl = (TabControl)target;
        //        this.tabControl.SelectionChanged += new SelectionChangedEventHandler(this.ChatTab_SelectionChanged);
        //        break;
        //    case 2:
        //        this.OAWorkflowTabItem = (OAWorkflowTabItem)target;
        //        break;
        //    case 3:
        //        this.OAPlanTabItem = (OAPlanTabItem)target;
        //        break;
        //    case 4:
        //        this.OANoticeTabItem = (OANoticeTabItem)target;
        //        break;
        //    case 5:
        //        this.profireItem = (TabItem)target;
        //        break;
        //    case 6:
        //        this.selfProfileFrame = (Frame)target;
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
