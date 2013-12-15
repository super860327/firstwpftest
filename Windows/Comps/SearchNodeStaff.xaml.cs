using IDKin.IM.Core;
using IDKin.IM.Data;
using IDKin.IM.Windows.Comps.ChatTab;
using IDKin.IM.Windows.Util;
using IDKin.IM.Windows.View;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
namespace IDKin.IM.Windows.Comps
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class SearchNodeStaff : UserControl//, IComponentConnector
	{
		private IDataService dataService = ServiceUtil.Instance.DataService;
		private ISessionService sessionService = ServiceUtil.Instance.SessionService;
		private Staff staff = null;
		private UserStatus status;
		//internal WrapPanel wrapPanel;
		//internal Canvas canvas;
		//internal Image imgFace;
		//internal Image imgFaceForeground;
		//internal TextBlock txtName;
        ////private bool _contentLoaded;
		public Staff Staff
		{
			get
			{
				return this.staff;
			}
		}
		public UserStatus Status
		{
			get
			{
				return this.staff.Status;
			}
			set
			{
				this.staff.Status = value;
			}
		}
		public SearchNodeStaff(Staff staff)
		{
			this.InitializeComponent();
			this.staff = staff;
			this.status = staff.Status;
			this.SetSizeHandler();
			this.UpdateInfo();
		}
		private void SetSizeHandler()
		{
		}
		public bool UpdateInfo()
		{
			bool update = false;
			this.txtName.Text = this.staff.Name + " (" + this.staff.UserName + ")";
			this.imgFace.Source = this.staff.HeaderImage;
			this.imgFaceForeground.Source = this.staff.StatusIcon;
			if (this.status != this.staff.Status)
			{
				this.status = this.staff.Status;
				update = true;
			}
			return update;
		}
		private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed && this.staff.Uid != this.sessionService.Uid)
			{
				INWindow window = this.dataService.INWindow as INWindow;
				if (window != null)
				{
					window.tbxSearch.Text = this.staff.Name;
				}
				EntStaffTab item = this.dataService.GetStaffChatTab(this.staff.Uid) as EntStaffTab;
				if (item != null)
				{
					((INWindow)this.dataService.INWindow).ContentTab.SelectedItem = item;
				}
				else
				{
					item = new EntStaffTab(this.staff);
					item.SetDefaultStyle();
					((INWindow)this.dataService.INWindow).ContentTab.Items.Add(item);
					this.dataService.AddStaffChatTab(this.staff.Uid, item);
					((INWindow)this.dataService.INWindow).ContentTab.SelectedItem = item;
				}
			}
		}
		private void Item_CloseTab(object sender, RoutedEventArgs e)
		{
			TabItem tabItem = e.Source as TabItem;
			if (tabItem != null)
			{
				TabControl tabControl = tabItem.Parent as TabControl;
				if (tabControl != null)
				{
					tabControl.Items.Remove(tabItem);
					this.dataService.RemoveStaffChatTab(this.staff.Uid);
				}
			}
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/searchnodestaff.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        ((SearchNodeStaff)target).MouseLeftButtonDown += new MouseButtonEventHandler(this.UserControl_MouseLeftButtonDown);
        //        break;
        //    case 2:
        //        this.wrapPanel = (WrapPanel)target;
        //        break;
        //    case 3:
        //        this.canvas = (Canvas)target;
        //        break;
        //    case 4:
        //        this.imgFace = (Image)target;
        //        break;
        //    case 5:
        //        this.imgFaceForeground = (Image)target;
        //        break;
        //    case 6:
        //        this.txtName = (TextBlock)target;
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
