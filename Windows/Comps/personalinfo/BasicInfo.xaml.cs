using IDKin.IM.Core;
using IDKin.IM.Windows.Util;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
namespace IDKin.IM.Windows.Comps.PersonalInfo
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public partial class BasicInfo : Canvas//, IComponentConnector
	{
		//internal TextBlock tbkUserName;
		//internal TextBlock tbkInID;
		//internal TextBlock tbkGrade;
		//internal TextBox tbxNickName;
		//internal TextBox tbxRealName;
		//internal TextBox Signate;
		//internal ComboBox cbxSex;
		//internal DatePicker datePicker;
		//internal ComboBox cbxAge;
		//internal ComboBox cbxZodiac;
		//internal ComboBox cbxConstellation;
		//internal ComboBox cbxBloodType;
		//internal AddressComponent address;
        ////private bool _contentLoaded;
		public BasicInfo()
		{
			this.InitializeComponent();
			this.InitUI();
		}
		public void SaveInfo(Staff staff)
		{
			try
			{
				if (staff != null)
				{
					staff.Constellation = this.cbxConstellation.Text;
					staff.Zodiac = this.cbxZodiac.Text;
					staff.BloodType = this.cbxBloodType.Text;
					string temp = this.cbxSex.Text;
					Sex sex;
					if (temp.Equals("男"))
					{
						sex = Sex.Male;
					}
					else
					{
						if (temp.Equals("女"))
						{
							sex = Sex.Female;
						}
						else
						{
							sex = Sex.Unknow;
						}
					}
					staff.Sex = sex;
					staff.Sex = sex;
					staff.Country = this.address.cmbCountry.Text;
					staff.Province = this.address.cmbProvince.Text;
					staff.City = this.address.cmbCity.Text;
					staff.Age = int.Parse(this.cbxAge.Text.ToString());
					staff.Name = this.tbxRealName.Text;
					staff.Signature = this.Signate.Text;
					staff.Nickname = this.tbxNickName.Text;
					staff.Birthday = this.datePicker.Text;
				}
			}
			catch (System.Exception ex)
			{
				ServiceUtil.Instance.Logger.Error(ex.ToString());
			}
		}
		public void SetInfo(Staff staff)
		{
			try
			{
				if (staff != null)
				{
					if (staff.Constellation == "null")
					{
						this.cbxConstellation.Text = "";
					}
					else
					{
						this.cbxConstellation.Text = staff.Constellation;
					}
					if (staff.Zodiac == "null")
					{
						this.cbxZodiac.Text = "";
					}
					else
					{
						this.cbxZodiac.Text = staff.Zodiac;
					}
					if (staff.BloodType == "null")
					{
						this.cbxBloodType.Text = "";
					}
					else
					{
						this.cbxBloodType.Text = staff.BloodType;
					}
					if (staff.UserName == "null")
					{
						this.tbkUserName.Text = "";
					}
					else
					{
						this.tbkUserName.Text = staff.UserName;
					}
					this.tbkInID.Text = staff.Uid.ToString();
					this.cbxAge.Text = staff.Age.ToString();
					this.datePicker.Text = staff.Birthday;
					if (staff.Sex == Sex.Female)
					{
						this.cbxSex.Text = "女";
					}
					else
					{
						if (staff.Sex == Sex.Male)
						{
							this.cbxSex.Text = "男";
						}
						else
						{
							this.cbxSex.Text = "保密";
						}
					}
					if (staff.Name == "null")
					{
						this.tbxRealName.Text = "";
					}
					else
					{
						this.tbxRealName.Text = staff.Name;
					}
					if (staff.Nickname == "null")
					{
						this.tbxNickName.Text = "";
					}
					else
					{
						this.tbxNickName.Text = staff.Nickname;
					}
					this.datePicker.Text = staff.BirthdayType.ToString();
					if (staff.Signature == "null")
					{
						this.Signate.Text = "";
					}
					else
					{
						this.Signate.Text = staff.Signature;
					}
					if (staff.Country == "null")
					{
						this.address.cmbCountry.Text = "";
					}
					else
					{
						this.address.cmbCountry.Text = staff.Country;
					}
					if (staff.Province == "null")
					{
						this.address.cmbProvince.Text = "";
					}
					else
					{
						this.address.cmbProvince.Text = staff.Province;
					}
					if (staff.City == "null")
					{
						this.address.cmbCity.Text = "";
					}
					else
					{
						this.address.cmbCity.Text = staff.City;
					}
				}
			}
			catch (System.Exception e)
			{
				ServiceUtil.Instance.Logger.Error(e.ToString());
			}
		}
		private void InitUI()
		{
			try
			{
				this.InitAge();
				this.InitZodiac();
				this.InitConstellation();
				this.InitBloodType();
				this.InitSex();
			}
			catch (System.Exception e)
			{
				ServiceUtil.Instance.Logger.Error(e.ToString());
			}
		}
		private void InitAge()
		{
			for (int i = 1; i <= 119; i++)
			{
				ComboBoxItem item = new ComboBoxItem();
				item.Content = i.ToString();
				this.cbxAge.Items.Add(item);
			}
		}
		private void InitZodiac()
		{
			string[] items = new string[]
			{
				" ",
				"鼠",
				"牛",
				"虎",
				"兔",
				"龙",
				"蛇",
				"马",
				"羊",
				"猴",
				"鸡",
				"狗",
				"猪"
			};
			string[] array = items;
			for (int i = 0; i < array.Length; i++)
			{
				string itemText = array[i];
				ComboBoxItem item = new ComboBoxItem();
				item.Content = itemText;
				this.cbxZodiac.Items.Add(item);
			}
		}
		private void InitConstellation()
		{
			string[] items = new string[]
			{
				" ",
				"白羊座",
				"金牛座",
				"双子座",
				"巨蟹座",
				"狮子座",
				"处女座",
				"天秤座",
				"天蝎座",
				"射手座",
				"摩羯座",
				"水瓶座",
				"双鱼座"
			};
			string[] array = items;
			for (int i = 0; i < array.Length; i++)
			{
				string itemText = array[i];
				ComboBoxItem item = new ComboBoxItem();
				item.Content = itemText;
				this.cbxConstellation.Items.Add(item);
			}
		}
		private void InitBloodType()
		{
			string[] items = new string[]
			{
				" ",
				"A型",
				"B型",
				"O型",
				"AB型",
				"其它"
			};
			string[] array = items;
			for (int i = 0; i < array.Length; i++)
			{
				string itemText = array[i];
				ComboBoxItem item = new ComboBoxItem();
				item.Content = itemText;
				this.cbxBloodType.Items.Add(item);
			}
		}
		private void InitSex()
		{
			string[] Sex = new string[]
			{
				"",
				"男",
				"女",
				"保密"
			};
			string[] array = Sex;
			for (int i = 0; i < array.Length; i++)
			{
				string sex = array[i];
				ComboBoxItem item = new ComboBoxItem();
				item.Content = sex;
				this.cbxSex.Items.Add(item);
			}
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/personalinfo/basicinfo.xaml", UriKind.Relative);
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
        //        this.tbkUserName = (TextBlock)target;
        //        break;
        //    case 2:
        //        this.tbkInID = (TextBlock)target;
        //        break;
        //    case 3:
        //        this.tbkGrade = (TextBlock)target;
        //        break;
        //    case 4:
        //        this.tbxNickName = (TextBox)target;
        //        break;
        //    case 5:
        //        this.tbxRealName = (TextBox)target;
        //        break;
        //    case 6:
        //        this.Signate = (TextBox)target;
        //        break;
        //    case 7:
        //        this.cbxSex = (ComboBox)target;
        //        break;
        //    case 8:
        //        this.datePicker = (DatePicker)target;
        //        break;
        //    case 9:
        //        this.cbxAge = (ComboBox)target;
        //        break;
        //    case 10:
        //        this.cbxZodiac = (ComboBox)target;
        //        break;
        //    case 11:
        //        this.cbxConstellation = (ComboBox)target;
        //        break;
        //    case 12:
        //        this.cbxBloodType = (ComboBox)target;
        //        break;
        //    case 13:
        //        this.address = (AddressComponent)target;
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
