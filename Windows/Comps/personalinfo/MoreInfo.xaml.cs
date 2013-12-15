using IDKin.IM.Core;
using IDKin.IM.Data;
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
	public partial class MoreInfo : UserControl//, IComponentConnector
	{
		private ISessionService sessionService = ServiceUtil.Instance.SessionService;
		//internal ComboBox cbxShowScope;
		//internal TextBox tbxMobile;
		//internal TextBox tbxPhone;
		//internal TextBox tbxExtension;
		//internal TextBox tbxEmail;
		//internal ComboBox cbxProfessional;
		//internal TextBox tbxSchool;
		//internal TextBox tbxMyHome;
		//internal TextBox tbxMyDescription;
		//private bool _contentLoaded;
		public MoreInfo()
		{
			this.InitializeComponent();
			this.InitScope();
			this.InitJob();
		}
		private void InitScope()
		{
			string[] Sex = new string[]
			{
				"公开",
				"保密"
			};
			string[] array = Sex;
			for (int i = 0; i < array.Length; i++)
			{
				string sex = array[i];
				ComboBoxItem item = new ComboBoxItem();
				item.Content = sex;
				this.cbxShowScope.Items.Add(item);
			}
			this.cbxShowScope.SelectedIndex = 1;
		}
		private void InitJob()
		{
			string[] Job = new string[]
			{
				"在校学生",
				"经营管理类",
				"客户服务类",
				"销售类",
				"项目管理类",
				"市场/公关/媒介类",
				"质量管理类",
				"人力资源类",
				"行政/后勤类",
				"财务/审计/统计类",
				"咨询顾问类",
				"计算机/网络/技术类",
				"翻译类",
				"电气/能源/动力类",
				"个体经营/商业零售类",
				"金融类（银行/基金/证券/期货/投资）",
				"教育/培训类",
				"贸易/物流/采购/运输类",
				"法律类",
				"建筑/房地产/装饰保修/物业管理类",
				"生物/制药/化工/环保类",
				"酒店/餐饮/旅游/服务类",
				"科研类",
				"工厂生产类",
				"技工类",
				"机械/仪器仪表类",
				"公务员类",
				"美术/设计/创意类",
				"退休"
			};
			string[] array = Job;
			for (int i = 0; i < array.Length; i++)
			{
				string job = array[i];
				ComboBoxItem item = new ComboBoxItem();
				item.Content = job;
				this.cbxProfessional.Items.Add(item);
			}
		}
		public void SetInfo(Staff staff)
		{
			try
			{
				if (staff.ShowScope == 1)
				{
					this.cbxShowScope.Text = "公开";
				}
				else
				{
					this.cbxShowScope.Text = "保密";
				}
				if (staff.Mobile == "null")
				{
					this.tbxMobile.Text = "";
				}
				else
				{
					this.tbxMobile.Text = staff.Mobile;
				}
				if (staff.Telephone == "null")
				{
					this.tbxPhone.Text = "";
				}
				else
				{
					this.tbxPhone.Text = staff.Telephone;
				}
				if (staff.Email == "null")
				{
					this.tbxEmail.Text = "";
				}
				else
				{
					this.tbxEmail.Text = staff.Email;
				}
				if (staff.Job == "null")
				{
					this.cbxProfessional.Text = "";
				}
				else
				{
					this.cbxProfessional.Text = staff.Job;
				}
				if (staff.School == "null")
				{
					this.tbxSchool.Text = "";
				}
				else
				{
					this.tbxSchool.Text = staff.School;
				}
				if (staff.MyHome == "null")
				{
					this.tbxMyHome.Text = "";
				}
				else
				{
					this.tbxMyHome.Text = staff.MyHome;
				}
				if (staff.MyDescription == "null")
				{
					this.tbxMyDescription.Text = "";
				}
				else
				{
					this.tbxMyDescription.Text = staff.MyDescription;
				}
				if (staff.Extension == "null")
				{
					this.tbxExtension.Text = "";
				}
				else
				{
					this.tbxExtension.Text = staff.Extension;
				}
			}
			catch (System.Exception e)
			{
				ServiceUtil.Instance.Logger.Error(e.ToString());
			}
		}
		public void SaveInfo(Staff staff)
		{
			try
			{
				staff.Email = this.tbxEmail.Text;
				staff.Mobile = this.tbxMobile.Text;
				staff.MyDescription = this.tbxMyDescription.Text;
				staff.MyHome = this.tbxMyHome.Text;
				staff.Telephone = this.tbxPhone.Text;
				staff.School = this.tbxSchool.Text;
				staff.Job = this.cbxProfessional.Text;
				staff.Extension = this.tbxExtension.Text;
				string temp = this.cbxShowScope.Text;
				if (temp == "公开")
				{
					staff.ShowScope = 1;
				}
				else
				{
					if (temp == "保密")
					{
						staff.ShowScope = 2;
					}
				}
			}
			catch (System.Exception e)
			{
				ServiceUtil.Instance.Logger.Error(e.ToString());
			}
		}
	}
}
