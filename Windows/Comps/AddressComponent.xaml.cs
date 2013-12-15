using IDKin.IM.Core;
using IDKin.IM.Windows.Properties;
using IDKin.IM.Windows.Util;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
namespace IDKin.IM.Windows.Comps
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class AddressComponent : UserControl//, IComponentConnector
	{
		public class AddressPoco
		{
			public string Code = null;
			public string Name = null;
		}
		private System.Collections.Hashtable Proviences = new System.Collections.Hashtable();
		private System.Collections.Hashtable Citys = new System.Collections.Hashtable();
		public static string cbCountry = null;
		public static string cbProvience = null;
		public static string cbCity = null;
        ////internal Canvas content;
        ////internal ComboBox cmbCountry;
        ////internal ComboBox cmbProvince;
        ////internal ComboBox cmbCity;
        ////private bool _contentLoaded;
		public AddressComponent()
		{
			this.InitializeComponent();
			AddressComponent.AddressPoco ap = new AddressComponent.AddressPoco();
			ap.Code = "100000";
			ap.Name = "中国";
			ComboBoxItem cbi = new ComboBoxItem();
			cbi.Content = ap.Name;
			cbi.DataContext = ap;
			this.cmbCountry.Items.Add(cbi);
			this.InitAddressData();
		}
		public void Init(Staff staff)
		{
			this.cmbCountry.Text = staff.Country;
			this.cmbCity.Text = staff.City;
			this.cmbProvince.Text = staff.Province;
		}
		private void cmbCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.cmbProvince.IsEditable = true;
			ComboBoxItem cbiCountry = this.cmbCountry.SelectedItem as ComboBoxItem;
			AddressComponent.AddressPoco coutryAP = cbiCountry.DataContext as AddressComponent.AddressPoco;
			System.Collections.Generic.List<AddressComponent.AddressPoco> provienceList = this.Proviences[coutryAP.Code] as System.Collections.Generic.List<AddressComponent.AddressPoco>;
			if (provienceList != null)
			{
				this.cmbProvince.Items.Clear();
				foreach (AddressComponent.AddressPoco ap in provienceList)
				{
					ComboBoxItem cbiProvience = new ComboBoxItem();
					cbiProvience.Content = ap.Name;
					cbiProvience.DataContext = ap;
					this.cmbProvince.Items.Add(cbiProvience);
				}
				AddressComponent.cbCountry = this.cmbCountry.Text;
			}
		}
		private void cmbProvince_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.cmbCity.IsEditable = true;
			ComboBoxItem cbiProvience = this.cmbProvince.SelectedItem as ComboBoxItem;
			if (cbiProvience != null)
			{
				AddressComponent.AddressPoco proviencesAP = cbiProvience.DataContext as AddressComponent.AddressPoco;
				if (proviencesAP != null)
				{
					System.Collections.Generic.List<AddressComponent.AddressPoco> cityList = this.Citys[proviencesAP.Code] as System.Collections.Generic.List<AddressComponent.AddressPoco>;
					if (cityList != null)
					{
						this.cmbCity.Items.Clear();
						foreach (AddressComponent.AddressPoco ap in cityList)
						{
							ComboBoxItem cbiCity = new ComboBoxItem();
							cbiCity.DataContext = ap;
							cbiCity.Content = ap.Name;
							this.cmbCity.Items.Add(cbiCity);
						}
						AddressComponent.cbProvience = this.cmbProvince.Text;
					}
				}
			}
		}
		private void InitUI()
		{
			this.InitCountry();
		}
		private void InitCountry()
		{
			string[] items = new string[]
			{
				" ",
				"中国"
			};
			string[] array = items;
			for (int i = 0; i < array.Length; i++)
			{
				string itemText = array[i];
				ComboBoxItem item = new ComboBoxItem();
				item.Content = itemText;
				this.cmbCountry.Items.Add(item);
			}
		}
		private void InitProvince(string provice)
		{
		}
		private void InitCity(string city)
		{
		}
		private System.Collections.Generic.List<string> LoadAddressTxt()
		{
			System.Collections.Generic.List<string> result;
			try
			{
				System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
				string s = IDKin.IM.Windows.Properties.Resources.chinese;
				char[] reg = new char[]
				{
					'\r',
					'\n'
				};
				string[] ss = s.Split(reg);
				for (int i = 0; i < ss.Length; i++)
				{
					if (!"".Equals(ss[i]))
					{
						list.Add(ss[i]);
					}
				}
				result = list;
				return result;
			}
			catch (System.Exception e)
			{
				MessageBox.Show(e.ToString());
			}
			result = null;
			return result;
		}
		private void InitAddressData()
		{
			System.Collections.Generic.List<string> list = this.LoadAddressTxt();
			if (list != null)
			{
				char[] reg = new char[]
				{
					'#'
				};
				for (int i = 0; i < list.Count; i++)
				{
					AddressComponent.AddressPoco address = new AddressComponent.AddressPoco();
					string[] s = list[i].Split(reg);
					if (s.Length == 2)
					{
						address.Code = s[0];
						address.Name = s[1];
						char[] values = address.Code.ToCharArray();
						int countValue = 0;
						for (int j = 2; j < values.Length; j++)
						{
							countValue += int.Parse(values[j].ToString());
						}
						if (countValue == 0)
						{
							if (!this.Proviences.ContainsKey("100000"))
							{
								this.Proviences.Add("100000", new System.Collections.Generic.List<AddressComponent.AddressPoco>());
							}
							System.Collections.Generic.List<AddressComponent.AddressPoco> listTemp = this.Proviences["100000"] as System.Collections.Generic.List<AddressComponent.AddressPoco>;
							listTemp.Add(address);
						}
						else
						{
							countValue = 0;
							for (int z = 4; z < values.Length; z++)
							{
								countValue += int.Parse(values[z].ToString());
							}
							if (countValue == 0)
							{
								string provienceCode = this.getProvinceCode(int.Parse(address.Code)).ToString();
								if (!this.Citys.ContainsKey(provienceCode))
								{
									this.Citys.Add(provienceCode, new System.Collections.Generic.List<AddressComponent.AddressPoco>());
								}
								System.Collections.Generic.List<AddressComponent.AddressPoco> cityList = this.Citys[provienceCode] as System.Collections.Generic.List<AddressComponent.AddressPoco>;
								cityList.Add(address);
							}
						}
					}
				}
				System.Console.WriteLine("InitAddressData");
			}
		}
		private int getProvinceCode(int code)
		{
			int _code = code / 10000;
			return _code * 10000;
		}
		public string[] GetAddress()
		{
			return new string[]
			{
				this.cmbCountry.Text,
				this.cmbProvince.Text,
				this.cmbCity.Text
			};
		}
		public void ShowAddress(string country, string province, string city)
		{
			try
			{
				if ("100000".Equals(country))
				{
					this.cmbCountry.Text = "中国";
				}
				System.Collections.Generic.List<AddressComponent.AddressPoco> provienceList = this.Proviences["100000"] as System.Collections.Generic.List<AddressComponent.AddressPoco>;
				if (provienceList != null)
				{
					AddressComponent.AddressPoco apProvience = null;
					foreach (AddressComponent.AddressPoco ap in provienceList)
					{
						if (ap.Code.Equals(province))
						{
							apProvience = ap;
						}
					}
					if (apProvience != null)
					{
						this.cmbProvince.Text = apProvience.Name;
						System.Collections.Generic.List<AddressComponent.AddressPoco> cityList = this.Citys[province] as System.Collections.Generic.List<AddressComponent.AddressPoco>;
						AddressComponent.AddressPoco apCity = null;
						foreach (AddressComponent.AddressPoco ap in cityList)
						{
							if (ap.Code.Equals(city))
							{
								apCity = ap;
							}
						}
						if (apCity != null)
						{
							this.cmbCity.Text = apCity.Name;
						}
					}
				}
			}
			catch (System.Exception e)
			{
				ServiceUtil.Instance.Logger.Error(e.ToString());
			}
		}
		public string GetAddressCode(object obj)
		{
			string result;
			if (obj == null)
			{
				result = "";
			}
			else
			{
				ComboBoxItem cbi = obj as ComboBoxItem;
				AddressComponent.AddressPoco ap = cbi.DataContext as AddressComponent.AddressPoco;
				result = ap.Code;
			}
			return result;
		}
		public void Dispose()
		{
			this.Proviences.Clear();
			this.Citys.Clear();
		}
		private void cmbProvince_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
		{
			this.cmbCity.IsEditable = true;
			ComboBoxItem cbiProvience = this.cmbProvince.SelectedItem as ComboBoxItem;
			if (cbiProvience != null)
			{
				AddressComponent.AddressPoco proviencesAP = cbiProvience.DataContext as AddressComponent.AddressPoco;
				if (proviencesAP != null)
				{
					System.Collections.Generic.List<AddressComponent.AddressPoco> cityList = this.Citys[proviencesAP.Code] as System.Collections.Generic.List<AddressComponent.AddressPoco>;
					this.cmbCity.Items.Clear();
					if (cityList != null)
					{
						foreach (AddressComponent.AddressPoco ap in cityList)
						{
							ComboBoxItem cbiCity = new ComboBoxItem();
							cbiCity.DataContext = ap;
							cbiCity.Content = ap.Name;
							this.cmbCity.Items.Add(cbiCity);
						}
					}
				}
			}
		}
		private void cmbCountry_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
		{
			this.cmbProvince.IsEditable = true;
			ComboBoxItem cbiCountry = this.cmbCountry.SelectedItem as ComboBoxItem;
			AddressComponent.AddressPoco coutryAP = cbiCountry.DataContext as AddressComponent.AddressPoco;
			System.Collections.Generic.List<AddressComponent.AddressPoco> provienceList = this.Proviences[coutryAP.Code] as System.Collections.Generic.List<AddressComponent.AddressPoco>;
			if (provienceList != null)
			{
				this.cmbProvince.Items.Clear();
				foreach (AddressComponent.AddressPoco ap in provienceList)
				{
					ComboBoxItem cbiProvience = new ComboBoxItem();
					cbiProvience.Content = ap.Name;
					cbiProvience.DataContext = ap;
					this.cmbProvince.Items.Add(cbiProvience);
				}
			}
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/addresscomponent.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.content = (Canvas)target;
        //        break;
        //    case 2:
        //        this.cmbCountry = (ComboBox)target;
        //        this.cmbCountry.SelectionChanged += new SelectionChangedEventHandler(this.cmbCountry_SelectionChanged_1);
        //        break;
        //    case 3:
        //        this.cmbProvince = (ComboBox)target;
        //        this.cmbProvince.SelectionChanged += new SelectionChangedEventHandler(this.cmbProvince_SelectionChanged_1);
        //        break;
        //    case 4:
        //        this.cmbCity = (ComboBox)target;
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
