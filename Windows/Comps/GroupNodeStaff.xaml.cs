using IDKin.IM.Core;
using IDKin.IM.Data;
using IDKin.IM.ImageService;
using IDKin.IM.Windows.Comps.ChatTab;
using IDKin.IM.Windows.Util;
using IDKin.IM.Windows.View;
using IDKin.IM.Windows.ViewModel;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
namespace IDKin.IM.Windows.Comps
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class GroupNodeStaff : ListViewItem//, IComponentConnector
	{
		private const int MEMBER_NAME_LENGTH_MAX = 8;
		private EntGroupManagerWindowViewModel viewModel = new EntGroupManagerWindowViewModel();
		private ISessionService sessionService = ServiceUtil.Instance.SessionService;
		private IDataService dataService = ServiceUtil.Instance.DataService;
		private IImageService imageService = ServiceUtil.Instance.ImageService;
		private long gid;
		private Staff staff;
		//internal MenuItem AddAdmin;
		//internal MenuItem DeleAdmin;
		//internal Image imgFaceManager;
		//internal TreeNodeStaffHead imgFace;
		//internal Image imgFace_foreground;
		//internal TextBlock tbkName;
		//private bool _contentLoaded;
		public long Gid
		{
			get
			{
				return this.gid;
			}
			set
			{
				this.gid = value;
			}
		}
		public UserStatus Status
		{
			get
			{
				return this.staff.Status;
			}
		}
		public Staff Staff
		{
			get
			{
				return this.staff;
			}
			set
			{
				this.staff = value;
			}
		}
		public GroupNodeStaff(long gid, Staff staff)
		{
			try
			{
				this.InitializeComponent();
				if (staff != null)
				{
					this.gid = gid;
					this.staff = staff;
					this.tbkName.Text = this.GetName(staff);
					this.imgFace.Background = new ImageBrush
					{
						ImageSource = staff.HeaderImage
					};
					this.imgFace_foreground.Source = staff.StatusIcon;
					EntGroup entGroup = this.dataService.GetEntGroup(gid);
					if (entGroup != null)
					{
						if (entGroup.IsAdmin(staff.Uid))
						{
							this.imgFaceManager.Source = entGroup.AdminIcon;
						}
						if (staff.Uid != this.sessionService.Uid && entGroup.IsAdmin(this.sessionService.Uid))
						{
							if (!entGroup.IsAdmin(staff.Uid))
							{
								this.AddAdminContextMenu();
							}
							else
							{
								this.DeleAdminContextMenu();
							}
						}
					}
				}
			}
			catch (System.Exception e)
			{
				ServiceUtil.Instance.Logger.Error(e.ToString());
			}
		}
		public void DeleAdminImage()
		{
			this.imgFaceManager.Source = null;
		}
		public void ClearAdminContextMenu()
		{
			this.DeleAdmin.IsEnabled = false;
			this.AddAdmin.IsEnabled = false;
		}
		public void DeleAdminContextMenu()
		{
			this.DeleAdmin.IsEnabled = true;
			this.AddAdmin.IsEnabled = false;
		}
		public void AddAdminContextMenu()
		{
			this.AddAdmin.IsEnabled = true;
			this.DeleAdmin.IsEnabled = false;
		}
		public void AddAdminImage()
		{
			EntGroup entGroup = this.dataService.GetEntGroup(this.gid);
			if (entGroup != null)
			{
				this.imgFaceManager.Source = entGroup.AdminIcon;
			}
		}
		private string GetName(Staff staff)
		{
			string memberName = "";
			if (staff != null)
			{
				memberName = this.ChangNameLength(staff.Name) + this.GetDepartmentName(staff.DepartmentId);
			}
			return memberName;
		}
		private string GetDepartmentName(long id)
		{
			Department department = this.dataService.GetDepartment(id);
			string result;
			if (department != null)
			{
				result = "(" + department.Name + ")";
			}
			else
			{
				result = "";
			}
			return result;
		}
		private string ChangNameLength(string name)
		{
			string result;
			if (name == null)
			{
				result = "";
			}
			else
			{
				if (name.Length > 8)
				{
					name = name.Substring(0, 8);
				}
				result = name;
			}
			return result;
		}
		public bool UpdateInfo()
		{
			this.tbkName.Text = this.GetName(this.staff);
			this.imgFace.Background = new ImageBrush
			{
				ImageSource = this.staff.HeaderImage
			};
			this.imgFace_foreground.Source = this.staff.StatusIcon;
			return true;
		}
		private Image getStatusIcon(uint status)
		{
			return null;
		}
		private Image GetAdminIcon()
		{
			return new Image
			{
				Source = this.imageService.GetIcon(ImageTypeIcon.GroupIconAdmin)
			};
		}
		private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (this.staff.Uid != this.sessionService.Uid)
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
				catch (System.Exception)
				{
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
		private void Admin_Click(object sender, RoutedEventArgs e)
		{
			EntGroup group = this.dataService.GetEntGroup(this.gid);
			if (group != null)
			{
				this.viewModel.AddAdmin(this.gid, this.staff.Uid + ":");
			}
		}
		private void DeleAdmin_Click(object sender, RoutedEventArgs e)
		{
			EntGroup group = this.dataService.GetEntGroup(this.gid);
			if (group != null)
			{
				this.viewModel.AdminRemove(this.gid, this.staff.Uid + ":");
			}
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/groupnodestaff.xaml", UriKind.Relative);
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
        //        ((GroupNodeStaff)target).MouseDoubleClick += new MouseButtonEventHandler(this.UserControl_MouseDoubleClick);
        //        break;
        //    case 2:
        //        this.AddAdmin = (MenuItem)target;
        //        this.AddAdmin.Click += new RoutedEventHandler(this.Admin_Click);
        //        break;
        //    case 3:
        //        this.DeleAdmin = (MenuItem)target;
        //        this.DeleAdmin.Click += new RoutedEventHandler(this.DeleAdmin_Click);
        //        break;
        //    case 4:
        //        this.imgFaceManager = (Image)target;
        //        break;
        //    case 5:
        //        this.imgFace = (TreeNodeStaffHead)target;
        //        break;
        //    case 6:
        //        this.imgFace_foreground = (Image)target;
        //        break;
        //    case 7:
        //        this.tbkName = (TextBlock)target;
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
