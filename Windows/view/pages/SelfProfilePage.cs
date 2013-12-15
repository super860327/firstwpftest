using IDKin.IM.Core;
using IDKin.IM.Data;
using IDKin.IM.ImageService;
using IDKin.IM.Log;
using IDKin.IM.Theme.Helper;
using IDKin.IM.Windows.Comps.PersonalInfo;
using IDKin.IM.Windows.Util;
using IDKin.IM.Windows.View.Commons;
using IDKin.IM.Windows.ViewModel;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
namespace IDKin.IM.Windows.View.Pages
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class SelfProfilePage : Page//, IComponentConnector
	{
		private ISessionService sessionService = ServiceUtil.Instance.SessionService;
		private IDataService dataService = ServiceUtil.Instance.DataService;
		private INViewModel inViewModel = new INViewModel();
		private ILogger logger = ServiceUtil.Instance.Logger;
		private IImageService imageService = ServiceUtil.Instance.ImageService;
		private Staff staff;
        //internal Border headBorder;
        //internal Button btnChange;
        //internal Border LiangTuBorder;
        //internal Button btnUpload;
        //internal StackPanel content;
        //internal BasicInfo basicInfo;
        //internal MoreInfo moreInfo;
        //internal Button btnSave;
        //private bool _contentLoaded;
		public SelfProfilePage()
		{
			ThemeSwitcher.LoadSkin(ThemeEnum.Aero, this);
			this.InitializeComponent();
		}
		public void InitStafInfo(Staff staff)
		{
			if (staff != null)
			{
				this.staff = staff;
				base.DataContext = staff;
				this.basicInfo.SetInfo(staff);
				this.moreInfo.SetInfo(staff);
				switch (staff.Sex)
				{
				case Sex.Female:
					this.LiangTuBorder.Background = new ImageBrush
					{
						ImageSource = this.imageService.GetHeader(ImageTypeHeader.LiangTuFemale)
					};
					break;
				case Sex.Male:
					this.LiangTuBorder.Background = new ImageBrush
					{
						ImageSource = this.imageService.GetHeader(ImageTypeHeader.LiangTuMale)
					};
					break;
				case Sex.Unknow:
					this.LiangTuBorder.Background = new ImageBrush
					{
						ImageSource = this.imageService.GetHeader(ImageTypeHeader.LiangTuMale)
					};
					break;
				default:
					this.LiangTuBorder.Background = new ImageBrush
					{
						ImageSource = this.imageService.GetHeader(ImageTypeHeader.LiangTuMale)
					};
					break;
				}
				this.headBorder.Background = new ImageBrush
				{
					ImageSource = staff.HeaderImage
				};
			}
		}
		private Staff GetSaveStaff()
		{
			Staff result;
			try
			{
				if (this.staff != null)
				{
					this.basicInfo.SaveInfo(this.staff);
					this.moreInfo.SaveInfo(this.staff);
					result = this.staff;
					return result;
				}
			}
			catch (System.Exception e)
			{
				this.logger.Error(e.ToString());
			}
			result = this.staff;
			return result;
		}
		private void btnChange_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (this.staff != null)
				{
					new UserHeaderWindow(this.staff)
					{
						NewHeadImage = new UserHeaderWindow.NewHeadImageDelegate(this.NewHeadImageHandler),
						Owner = this.dataService.INWindow,
						WindowStartupLocation = WindowStartupLocation.CenterOwner
					}.ShowDialog();
				}
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		private void NewHeadImageHandler(string file)
		{
			if (!string.IsNullOrEmpty(file))
			{
				this.headBorder.Background = new ImageBrush
				{
					ImageSource = new BitmapImage(new Uri(file, UriKind.Absolute))
				};
				this.staff.HeaderFileName = System.IO.Path.GetFileName(file);
				this.imageService.SetHeaderImage(this.staff);
				this.imageService.SetHeaderImageOnline(this.staff);
			}
		}
		private void btnUpload_Click(object sender, RoutedEventArgs e)
		{
		}
		private void btnSave_Click(object sender, RoutedEventArgs e)
		{
			Staff staff = this.GetSaveStaff();
			if (staff != null)
			{
				if (ValidationUtil.IsValid(this))
				{
					if (this.inViewModel.ChangeStatus(staff))
					{
						INWindow inWindow = this.dataService.INWindow as INWindow;
						if (inWindow != null)
						{
							inWindow.userHead.ImageSource = staff.HeaderImage;
							inWindow.tbkName.Text = staff.Name;
							inWindow.lblSignature.Text = staff.Signature;
						}
						if (!this.inViewModel.ProfileUpdate(staff))
						{
							Alert.Show("个人资料更新失败", "提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
						}
						else
						{
							Alert.Show("个人信息保存成功", "提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
						}
					}
					else
					{
						Alert.Show("个人状态更新失败包括,用户名，头象，状态，个人签名", "提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
					}
				}
			}
			else
			{
				Alert.Show("个人档案资料为空，保存失败", "提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
			}
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/view/pages/selfprofilepage.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[System.Diagnostics.DebuggerNonUserCode]
        //internal System.Delegate _CreateDelegate(System.Type delegateType, string handler)
        //{
        //    return System.Delegate.CreateDelegate(delegateType, this, handler);
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.headBorder = (Border)target;
        //        break;
        //    case 2:
        //        this.btnChange = (Button)target;
        //        this.btnChange.Click += new RoutedEventHandler(this.btnChange_Click);
        //        break;
        //    case 3:
        //        this.LiangTuBorder = (Border)target;
        //        break;
        //    case 4:
        //        this.btnUpload = (Button)target;
        //        break;
        //    case 5:
        //        this.content = (StackPanel)target;
        //        break;
        //    case 6:
        //        this.basicInfo = (BasicInfo)target;
        //        break;
        //    case 7:
        //        this.moreInfo = (MoreInfo)target;
        //        break;
        //    case 8:
        //        this.btnSave = (Button)target;
        //        this.btnSave.Click += new RoutedEventHandler(this.btnSave_Click);
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
