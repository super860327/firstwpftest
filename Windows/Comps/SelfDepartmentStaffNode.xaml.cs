using IDKin.IM.Core;
using IDKin.IM.Data;
using IDKin.IM.ImageService;
using IDKin.IM.Log;
using IDKin.IM.Windows.Comps.ChatTab;
using IDKin.IM.Windows.Util;
using IDKin.IM.Windows.View;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;
namespace IDKin.IM.Windows.Comps
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class SelfDepartmentStaffNode : UserControl//, IComponentConnector, IStyleConnector
	{
		private ILogger logger = ServiceUtil.Instance.Logger;
		private Staff staff = null;
		public Personalminicards Popup;
		private DispatcherTimer timer;
		private ISessionService sessionService = null;
		private IImageService imageService = ServiceUtil.Instance.ImageService;
		private IDataService dataService = ServiceUtil.Instance.DataService;
		private UserStatus status;
		private bool isBigHead;
		//internal WrapPanel wrapPanel;
		//internal Canvas canvas;
		//internal TreeNodeStaffHead imgFace;
		//internal Image imgFaceForeground;
		//internal TextBlock txtName;
		//internal StackPanel jobsStackPanel;
		////internal Image imgDepartmentManage;
		////internal TextBlock tbkJobs;
        ////private bool _contentLoaded;
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
		public bool IsBigHead
		{
			get
			{
				return this.isBigHead;
			}
			set
			{
				this.isBigHead = value;
			}
		}
		public SelfDepartmentStaffNode(Staff staff)
		{
			this.InitializeComponent();
			this.staff = staff;
			this.status = staff.Status;
			this.UpdateInfo();
			this.AddEventListenerHandler();
		}
		private void AddEventListenerHandler()
		{
			this.Popup = new Personalminicards();
			this.timer = new DispatcherTimer();
			this.timer.Interval = System.TimeSpan.FromMilliseconds(800.0);
			this.imgFace.InnerBorder.MouseEnter += new MouseEventHandler(this.ShowPersonalminicards);
			this.imgFace.InnerBorder.MouseLeave += new MouseEventHandler(this.ClosePersonalminicards);
			this.imgFace.InnerBorder.MouseDown += new MouseButtonEventHandler(this.imgFace_MouseDown);
		}
		public void ShowJodLogo()
		{
			if (this.staff != null)
			{
				switch (this.staff.Sex)
				{
				case Sex.Female:
					this.imgDepartmentManage.Source = this.imageService.GetIcon(ImageTypeIcon.DepartmentManagerFemale);
					break;
				case Sex.Male:
					this.imgDepartmentManage.Source = this.imageService.GetIcon(ImageTypeIcon.DepartmentManagerMale);
					break;
				case Sex.Unknow:
					this.imgDepartmentManage.Source = this.imageService.GetIcon(ImageTypeIcon.DepartmentManagerMale);
					break;
				}
			}
		}
		public void ShowJobName()
		{
			if (this.staff != null)
			{
				this.tbkJobs.Text = "部门经理";
			}
		}
		private void imgFace_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (this.Popup != null)
			{
				this.Popup.IsOpen = false;
			}
			this.timer.Stop();
		}
		public bool UpdateInfo()
		{
			bool update = false;
			this.txtName.Text = this.staff.Name;
			this.imgFace.bgHead.ImageSource = this.staff.HeaderImage42;
			this.imgFaceForeground.Source = this.staff.StatusIcon;
			if (this.status != this.staff.Status)
			{
				this.status = this.staff.Status;
				update = true;
			}
			return update;
		}
		private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			try
			{
				if (e.LeftButton == MouseButtonState.Pressed && this.staff.Uid != this.sessionService.Uid)
				{
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
			catch (System.Exception ex)
			{
				System.Console.WriteLine(ex.ToString());
				this.logger.Error(ex.ToString());
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
		private void OnOpenProfileHandler(object sender, RoutedEventArgs e)
		{
		}
		private void ShowPersonalminicards(object sender, MouseEventArgs e)
		{
			this.timer.Start();
			this.timer.Tick += delegate
			{
				this.Popup.PlacementTarget = this.imgFace;
				this.Popup.Placement = PlacementMode.Left;
				this.Popup.StaysOpen = true;
				this.Popup.IsOpen = true;
				this.timer.Stop();
			};
			this.Popup.InitData(this.staff);
		}
		private void ClosePersonalminicards(object sender, MouseEventArgs e)
		{
			if (this.Popup != null)
			{
				this.Popup.ClosePopup(e.GetPosition(this));
				this.timer.Stop();
			}
		}
		private void ShowBigHead()
		{
			this.wrapPanel.Orientation = Orientation.Vertical;
			this.wrapPanel.Height = 44.0;
			this.canvas.Height = 44.0;
			this.imgFace.Width = 42.0;
			this.imgFace.Height = 42.0;
			this.imgFaceForeground.SetValue(Canvas.LeftProperty, double.Parse("28"));
			this.imgFaceForeground.SetValue(Canvas.BottomProperty, double.Parse("2"));
			this.txtName.Margin = new Thickness(50.0, 2.5, 0.0, 0.0);
			this.jobsStackPanel.Margin = new Thickness(47.0, 2.5, 0.0, 2.5);
		}
		private void ShowSmallHead()
		{
			this.wrapPanel.Orientation = Orientation.Horizontal;
			this.wrapPanel.Height = 25.0;
			this.canvas.Height = 25.0;
			this.imgFace.Width = 23.0;
			this.imgFace.Height = 23.0;
			this.imgFaceForeground.SetValue(Canvas.LeftProperty, double.Parse("9"));
			this.imgFaceForeground.SetValue(Canvas.BottomProperty, double.Parse("2"));
			this.txtName.Margin = new Thickness(30.0, 0.0, 0.0, 0.0);
			this.jobsStackPanel.Margin = new Thickness(5.0, 0.0, 0.0, 0.0);
		}
		private void ShowOtherStaffSmallHead()
		{
			try
			{
				INWindow inWindow = this.dataService.INWindow as INWindow;
				if (inWindow != null)
				{
					System.Collections.Generic.ICollection<SelfDepartmentStaffNode> nodes = inWindow.Employee.SelfDepartmentItem.StaffNode.Values;
					foreach (SelfDepartmentStaffNode node in nodes)
					{
						if (node != null && node.staff.Uid != this.staff.Uid && node.IsBigHead)
						{
							node.IsBigHead = false;
							node.ShowSmallHead();
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				this.isBigHead = true;
				this.ShowBigHead();
				this.ShowOtherStaffSmallHead();
			}
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/selfdepartmentstaffnode.xaml", UriKind.Relative);
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
        //    case 2:
        //        this.wrapPanel = (WrapPanel)target;
        //        break;
        //    case 3:
        //        this.canvas = (Canvas)target;
        //        break;
        //    case 4:
        //        this.imgFace = (TreeNodeStaffHead)target;
        //        break;
        //    case 5:
        //        this.imgFaceForeground = (Image)target;
        //        break;
        //    case 6:
        //        this.txtName = (TextBlock)target;
        //        break;
        //    case 7:
        //        this.jobsStackPanel = (StackPanel)target;
        //        break;
        //    case 8:
        //        this.imgDepartmentManage = (Image)target;
        //        break;
        //    case 9:
        //        this.tbkJobs = (TextBlock)target;
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IStyleConnector.Connect(int connectionId, object target)
        //{
        //    if (connectionId == 1)
        //    {
        //        EventSetter eventSetter = new EventSetter();
        //        eventSetter.Event = UIElement.MouseDownEvent;
        //        eventSetter.Handler = new MouseButtonEventHandler(this.UserControl_MouseDown);
        //        ((Style)target).Setters.Add(eventSetter);
        //        eventSetter = new EventSetter();
        //        eventSetter.Event = Control.MouseDoubleClickEvent;
        //        eventSetter.Handler = new MouseButtonEventHandler(this.UserControl_MouseDoubleClick);
        //        ((Style)target).Setters.Add(eventSetter);
        //    }
        //}
	}
}
