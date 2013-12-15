using IDKin.IM.Communicate;
using IDKin.IM.Core;
using IDKin.IM.Core.Core;
using IDKin.IM.Data;
using IDKin.IM.ImageService;
using IDKin.IM.Log;
using IDKin.IM.Windows.Comps.ChatTab;
using IDKin.IM.Windows.Model;
using IDKin.IM.Windows.Util;
using IDKin.IM.Windows.View;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
namespace IDKin.IM.Windows.Comps
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class CollaborationNodeStaff : UserControl//, IComponentConnector
	{
		private CooperationStaff staff = null;
		private CooperationProjectWrapper cooperationProjectWrapper = null;
		private ISessionService sessionService = null;
		private ILogger logger = null;
		private IImageService imageService = null;
		private IDataService dataService = null;
		private IFileService fileService = null;
		private bool isBigHead;
		//internal WrapPanel wrapPanel;
		//internal Canvas canvas;
		//internal TreeNodeStaffHead imgFace;
		//internal Image imgFaceForeground;
		//internal TextBlock tbkName;
		//internal TextBlock tbkRole;
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
		public CooperationStaff Staff
		{
			get
			{
				return this.staff;
			}
		}
		public string UnitedProjectId
		{
			get;
			set;
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
		public CollaborationNodeStaff(CooperationStaff staff, CooperationProjectWrapper cooperationProjectWrapper)
		{
			this.InitializeComponent();
			this.logger = ServiceUtil.Instance.Logger;
			this.imageService = ServiceUtil.Instance.ImageService;
			this.sessionService = ServiceUtil.Instance.SessionService;
			this.dataService = ServiceUtil.Instance.DataService;
			this.fileService = ServiceUtil.Instance.FileService;
			this.cooperationProjectWrapper = cooperationProjectWrapper;
			this.staff = staff;
			this.staff_CooperationStaffUpdated(this.staff);
			base.DataContext = this.staff;
			this.staff.CooperationStaffUpdated += new CooperationStaffUpdatedHandler(this.staff_CooperationStaffUpdated);
			this.AddEventListenerHandler();
		}
		private void staff_CooperationStaffUpdated(CooperationStaff cooperationStaff)
		{
			base.Dispatcher.BeginInvoke(new CooperationStaffUpdatedHandler(this.UpdateView), new object[]
			{
				cooperationStaff
			});
		}
		private void UpdateView(CooperationStaff cooperationStaff)
		{
			try
			{
				this.imageService.SetCooperationStaffHeaderImage(cooperationStaff);
				this.imageService.SetCooperationStaffStatusIcon(cooperationStaff);
				this.imageService.SetCooperationStaffHeaderImageOnline(cooperationStaff);
				this.fileService.DownloadCooperationStaffHeader(cooperationStaff);
				this.imgFace.bgHead.ImageSource = cooperationStaff.HeaderImage42;
				TreeViewItem cooperationProjectNode = this.dataService.GetCooperationProjectTvi(cooperationStaff.UnitedProjectid);
				if (cooperationProjectNode != null)
				{
					cooperationProjectNode.Items.SortDescriptions.Clear();
					cooperationProjectNode.Items.SortDescriptions.Add(new SortDescription("Tag", ListSortDirection.Ascending));
				}
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		private void AddEventListenerHandler()
		{
			base.MouseDoubleClick += new MouseButtonEventHandler(this.UserControl_MouseDoubleClick);
			base.MouseDown += new MouseButtonEventHandler(this.CollaborationNodeStaff_MouseDown);
		}
		private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			try
			{
				if (e.LeftButton == MouseButtonState.Pressed && this.staff.Uid != this.sessionService.Uid)
				{
					CoopStaffTab item = this.dataService.GetCooperationStaffChatTab(this.staff.Uid, this.staff.UnitedProjectid) as CoopStaffTab;
					if (item != null)
					{
						((INWindow)this.dataService.INWindow).ContentTab.SelectedItem = item;
					}
					else
					{
						item = new CoopStaffTab(this.staff, this.cooperationProjectWrapper);
						item.SetDefaultStyle();
						((INWindow)this.dataService.INWindow).ContentTab.Items.Add(item);
						this.dataService.AddCooperationStaffChatTab(this.staff.Uid, this.staff.UnitedProjectid, item);
						((INWindow)this.dataService.INWindow).ContentTab.SelectedItem = item;
						this.MessageProcessor(item);
					}
				}
			}
			catch (System.Exception)
			{
			}
		}
		private void MessageProcessor(CoopStaffTab item)
		{
			if (item != null)
			{
				System.Collections.Generic.List<Message> list = DataModel.Instance.GetCooperationStaffMessage(this.staff.Uid, this.staff.UnitedProjectid, MessageActorType.CooperationStaff);
				if (list != null && list.Count > 0)
				{
					foreach (Message message in list)
					{
						item.TabContent.ChatComponent.AddCooperationMessageStaff(message, false);
						item.TabContent.ChatComponent.inputMsgBox.Focus();
					}
				}
				DataModel.Instance.RemoveCooperationStatffMessage(this.staff.Uid, this.staff.UnitedProjectid, MessageActorType.CooperationStaff);
				MessageBoxWindow mbw = DataModel.Instance.GetMessageBox();
				if (mbw != null)
				{
					mbw.Refresh();
				}
			}
		}
		public void ShowBigHead()
		{
			this.wrapPanel.Orientation = Orientation.Vertical;
			this.wrapPanel.Height = 44.0;
			this.canvas.Height = 44.0;
			this.imgFace.Width = 42.0;
			this.imgFace.Height = 42.0;
			this.imgFaceForeground.SetValue(Canvas.LeftProperty, double.Parse("28"));
			this.imgFaceForeground.SetValue(Canvas.BottomProperty, double.Parse("2"));
			this.tbkName.Margin = new Thickness(50.0, 2.5, 0.0, 0.0);
			this.tbkRole.Margin = new Thickness(47.0, 2.5, 0.0, 2.5);
			this.isBigHead = true;
		}
		public void ShowSmallHead()
		{
			this.wrapPanel.Orientation = Orientation.Horizontal;
			this.wrapPanel.Height = 25.0;
			this.canvas.Height = 25.0;
			this.imgFace.Width = 23.0;
			this.imgFace.Height = 23.0;
			this.imgFaceForeground.SetValue(Canvas.LeftProperty, double.Parse("9"));
			this.imgFaceForeground.SetValue(Canvas.BottomProperty, double.Parse("2"));
			this.tbkName.Margin = new Thickness(30.0, 0.0, 0.0, 0.0);
			this.tbkRole.Margin = new Thickness(5.0, 0.0, 0.0, 0.0);
			this.isBigHead = false;
		}
		private void ShowOtherStaffSmallHead()
		{
			try
			{
				System.Collections.Generic.ICollection<TreeViewItem> cooperationStafffListTvi = this.dataService.GetCooperationStafffListTvi();
				if (cooperationStafffListTvi != null)
				{
					foreach (TreeViewItem item in cooperationStafffListTvi)
					{
						if (item != null)
						{
							if (item.Header != null)
							{
								CollaborationNodeStaff temNodeCooperationStaff = item.Header as CollaborationNodeStaff;
								if (temNodeCooperationStaff != null)
								{
									if (temNodeCooperationStaff != this && temNodeCooperationStaff.isBigHead)
									{
										temNodeCooperationStaff.ShowSmallHead();
									}
								}
							}
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		private void CollaborationNodeStaff_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
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
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/collaboration/collaborationnodestaff.xaml", UriKind.Relative);
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
        //        this.wrapPanel = (WrapPanel)target;
        //        break;
        //    case 2:
        //        this.canvas = (Canvas)target;
        //        break;
        //    case 3:
        //        this.imgFace = (TreeNodeStaffHead)target;
        //        break;
        //    case 4:
        //        this.imgFaceForeground = (Image)target;
        //        break;
        //    case 5:
        //        this.tbkName = (TextBlock)target;
        //        break;
        //    case 6:
        //        this.tbkRole = (TextBlock)target;
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
