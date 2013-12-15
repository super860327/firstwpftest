using IDKin.IM.Core;
using IDKin.IM.Data;
using IDKin.IM.ImageService;
using IDKin.IM.Theme.Helper;
using IDKin.IM.Windows.Comps.PersonalInfo;
using IDKin.IM.Windows.Util;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
namespace IDKin.IM.Windows.View.Pages
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class UserProfilePage : Page//, IComponentConnector
	{
		private ISessionService sessionService = ServiceUtil.Instance.SessionService;
		private IImageService imageService = ServiceUtil.Instance.ImageService;
        //internal Border headBorder;
        //internal TextBlock tbkHead;
        //internal Border LiangTuBorder;
        //internal TextBlock tbkPoto;
        //internal ViewInfo viewInfo;
        //private bool _contentLoaded;
		public UserProfilePage()
		{
			ThemeSwitcher.LoadSkin(ThemeEnum.Aero, this);
			this.InitializeComponent();
		}
		public void InitStafInfo(Staff staff)
		{
			if (staff != null)
			{
				this.headBorder.Background = new ImageBrush
				{
					ImageSource = staff.HeaderImage
				};
				this.tbkHead.Text = staff.Name + "的头像";
				this.tbkPoto.Text = staff.Name + "的靓照";
				this.viewInfo.tbknickname.Text = staff.Nickname;
				this.viewInfo.tbkUsername.Text = staff.UserName;
				this.viewInfo.tbkrealname.Text = staff.Name;
				this.viewInfo.tbkkinID.Text = "(" + staff.Uid.ToString() + ")";
				if (staff.Job == "null")
				{
					this.viewInfo.tbkProfessional.Text = "";
				}
				else
				{
					this.viewInfo.tbkProfessional.Text = staff.Job;
				}
				this.viewInfo.tbkLevel.Text = null;
				if (staff.Sex == Sex.Female)
				{
					this.viewInfo.tbkSex.Text = "女";
				}
				else
				{
					if (staff.Sex == Sex.Male)
					{
						this.viewInfo.tbkSex.Text = "男";
					}
					else
					{
						this.viewInfo.tbkSex.Text = "保密";
					}
				}
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
				if (staff.Signature == "null")
				{
					this.viewInfo.tbksignature.Text = "";
				}
				else
				{
					this.viewInfo.tbksignature.Text = staff.Signature;
				}
				if (staff.Birthday == "null")
				{
					this.viewInfo.tbkBirthday.Text = "";
				}
				else
				{
					this.viewInfo.tbkBirthday.Text = staff.Birthday;
				}
				this.viewInfo.tbkAge.Text = staff.Age.ToString();
				if (staff.Zodiac == "null")
				{
					this.viewInfo.tbkZodiac.Text = "";
				}
				else
				{
					this.viewInfo.tbkZodiac.Text = staff.Zodiac;
				}
				if (staff.Constellation == "null")
				{
					this.viewInfo.tbkConstellation.Text = "";
				}
				else
				{
					this.viewInfo.tbkConstellation.Text = staff.Constellation;
				}
				if (staff.BloodType == "null")
				{
					this.viewInfo.tbkBloodType.Text = "";
				}
				else
				{
					this.viewInfo.tbkBloodType.Text = staff.BloodType;
				}
				if (staff.Country == "null")
				{
					this.viewInfo.tbkCountry.Text = "";
				}
				else
				{
					this.viewInfo.tbkCountry.Text = staff.Country;
				}
				if (staff.City == "null")
				{
					this.viewInfo.tbkCity.Text = "";
				}
				else
				{
					this.viewInfo.tbkCity.Text = staff.City;
				}
				if (staff.Province == "null")
				{
					this.viewInfo.tbkProvince.Text = "";
				}
				else
				{
					this.viewInfo.tbkProvince.Text = staff.Province;
				}
				if (staff.ShowScope == 1)
				{
					this.viewInfo.tbkMobile.Text = staff.Mobile;
					this.viewInfo.tbkPhone.Text = staff.Telephone;
					this.viewInfo.tbkExtension.Text = staff.Extension;
					this.viewInfo.tbkEmail.Text = staff.Email;
					if (staff.Job == "null")
					{
						this.viewInfo.tbkProfessional.Text = "";
					}
					else
					{
						this.viewInfo.tbkProfessional.Text = staff.Job;
					}
					if (staff.School == "null")
					{
						this.viewInfo.tbkSchool.Text = "";
					}
					else
					{
						this.viewInfo.tbkSchool.Text = staff.School;
					}
					if (staff.MyHome == "null")
					{
						this.viewInfo.tbkHome.Text = "";
					}
					else
					{
						this.viewInfo.tbkHome.Text = staff.MyHome;
					}
					if (staff.MyDescription == "null")
					{
						this.viewInfo.tbkDescription.Text = "";
					}
					else
					{
						this.viewInfo.tbkDescription.Text = staff.MyDescription;
					}
					if (staff.Extension == "null")
					{
						this.viewInfo.tbkExtension.Text = "";
					}
					else
					{
						this.viewInfo.tbkExtension.Text = staff.Extension;
					}
				}
				else
				{
					if (staff.ShowScope == 2)
					{
						this.viewInfo.tbkMobile.Text = null;
						this.viewInfo.tbkPhone.Text = null;
						this.viewInfo.tbkEmail.Text = null;
						this.viewInfo.tbkProfessional.Text = null;
						this.viewInfo.tbkSchool.Text = null;
						this.viewInfo.tbkHome.Text = null;
						this.viewInfo.tbkDescription.Text = null;
					}
				}
			}
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/view/pages/userprofilepage.xaml", UriKind.Relative);
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
        //        this.tbkHead = (TextBlock)target;
        //        break;
        //    case 3:
        //        this.LiangTuBorder = (Border)target;
        //        break;
        //    case 4:
        //        this.tbkPoto = (TextBlock)target;
        //        break;
        //    case 5:
        //        this.viewInfo = (ViewInfo)target;
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
