using IDKin.IM.Core;
using IDKin.IM.CustomComponents.Controls;
using IDKin.IM.Data;
using IDKin.IM.Theme.Helper;
using IDKin.IM.Windows.Comps;
using IDKin.IM.Windows.Model;
using IDKin.IM.Windows.Util;
using IDKin.IM.Windows.ViewModel;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
namespace IDKin.IM.Windows.View
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class AddMemberCustomGroupWindow : Window//, IComponentConnector
	{
		private CustomGroupManagerWindowViewModel viewModel;
		private string groupId;
        //internal Border OuterBorder;
        //internal Border InnerBorder;
        //internal StatusBar topBar;
        //internal Image imgIcon;
        //internal ImageButton btnMin;
        //internal ComboBox cboxDeptList;
        //internal ListBox lstBoxFrom;
        //internal Label label2;
        //internal ListBox lstBoxTo;
        //internal TextBlock lblDeleteAll;
        //internal TextBlock lblAddAll;
        //internal Button btnOk;
        //internal Button btnClose;
        //private bool _contentLoaded;
		public AddMemberCustomGroupWindow(string groupid)
		{
			ThemeSwitcher.LoadSkin(ThemeEnum.Aero, this);
			this.InitializeComponent();
			this.groupId = groupid;
			this.viewModel = CustomGroupManagerWindowViewModel.GetInstance();
			this.cboxDeptList.SelectionChanged += new SelectionChangedEventHandler(this.cboxDeptList_SelectionChanged);
			this.InitialDeptList();
			this.btnClose.Click += delegate(object s, RoutedEventArgs e)
			{
				base.Close();
			};
			this.InitialToBox(groupid);
		}
		private void InitialDeptList()
		{
			DataService d = ServiceUtil.Instance.DataService as DataService;
			System.Collections.Generic.ICollection<Department> depts = d.GetDepartmentList();
			this.cboxDeptList.Items.Clear();
			foreach (Department item in depts)
			{
				if (item.Pid != 0L)
				{
					this.cboxDeptList.Items.Add(item);
				}
			}
			if (this.cboxDeptList.Items.Count > 0)
			{
				this.cboxDeptList.SelectedIndex = 0;
			}
		}
		private void InitialToBox(string groupid)
		{
			if (DataModel.Instance.CustomeGroupName.ContainsKey(groupid))
			{
				CustomGroup item = DataModel.Instance.CustomeGroupName[groupid];
				if (item.GroupID == groupid && item.Members != null && item.Members.Count > 0)
				{
					foreach (Staff staff in item.Members)
					{
						if (staff != null && !this.IsExist(staff))
						{
							CustomMemberItem citem = new CustomMemberItem(CustomMemberType.Delete);
							citem.DataContext = staff;
							citem.tbkAccount.Text = staff.Name;
							citem.imgHead.Source = staff.HeaderImage;
							citem.ItemDelete += new System.EventHandler(this.item_ItemDelete);
							this.lstBoxTo.Items.Add(citem);
						}
					}
				}
			}
		}
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonDown(e);
			base.DragMove();
		}
		private void cboxDeptList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.lstBoxFrom.Items.Clear();
			Department dept = (e.Source as ComboBox).SelectedValue as Department;
			if (dept != null)
			{
				DataService d = ServiceUtil.Instance.DataService as DataService;
				System.Collections.Generic.IEnumerable<CustomMemberItem> v = 
					from s in d.GetStaffList()
					where s.DepartmentId == dept.Id
					select this.GetMember(s);
				foreach (CustomMemberItem item in v)
				{
					this.lstBoxFrom.Items.Add(item);
				}
			}
		}
		private CustomMemberItem GetMember(Staff s)
		{
			CustomMemberItem item = new CustomMemberItem(CustomMemberType.Add);
			item.DataContext = s;
			item.imgHead.Source = s.HeaderImage;
			item.tbkAccount.Text = s.Name;
			item.ItemAdd += new System.EventHandler(this.item_ItemAdd);
			return item;
		}
		private void item_ItemAdd(object sender, System.EventArgs e)
		{
			if (this.lstBoxFrom.SelectedItems.Count > 0)
			{
				foreach (CustomMemberItem customMemberItem in this.lstBoxFrom.SelectedItems)
				{
					this.AddOneStaff(customMemberItem);
				}
			}
		}
		private void AddOneStaff(CustomMemberItem customMemberItem)
		{
			Staff staff = customMemberItem.DataContext as Staff;
			if (staff != null && !this.IsExist(staff) && ServiceUtil.Instance.SessionService.Uid != staff.Uid)
			{
				CustomMemberItem item = new CustomMemberItem(CustomMemberType.Delete);
				item.DataContext = staff;
				item.tbkAccount.Text = staff.Name;
				item.imgHead.Source = staff.HeaderImage;
				item.ItemDelete += new System.EventHandler(this.item_ItemDelete);
				this.lstBoxTo.Items.Add(item);
			}
		}
		private void item_ItemDelete(object sender, System.EventArgs e)
		{
			if (this.lstBoxTo.SelectedItems.Count > 0)
			{
				for (int i = this.lstBoxTo.SelectedItems.Count - 1; i >= 0; i--)
				{
					this.lstBoxTo.Items.Remove(this.lstBoxTo.SelectedItems[i]);
				}
			}
		}
		private bool IsExist(Staff staff)
		{
			bool result;
			foreach (ListBoxItem item in (System.Collections.IEnumerable)this.lstBoxTo.Items)
			{
				Staff s = item.DataContext as Staff;
				if (staff.Uid == s.Uid)
				{
					result = true;
					return result;
				}
			}
			result = false;
			return result;
		}
		private void btnRemove_Click(object sender, RoutedEventArgs e)
		{
			if (this.lstBoxTo.SelectedItems.Count > 0)
			{
				for (int i = this.lstBoxTo.SelectedItems.Count - 1; i >= 0; i--)
				{
					this.lstBoxTo.Items.Remove(this.lstBoxTo.SelectedItems[i]);
				}
			}
		}
		private void btnAddAll_Click(object sender, RoutedEventArgs e)
		{
			foreach (Staff staff in (System.Collections.IEnumerable)this.lstBoxFrom.Items)
			{
				if (!this.IsExist(staff))
				{
					ListBoxItem item = new ListBoxItem();
					item.DataContext = staff;
					item.Content = staff.Name;
					this.lstBoxTo.Items.Add(item);
				}
			}
		}
		private void btnRemoveAll_Click(object sender, RoutedEventArgs e)
		{
			this.lstBoxTo.Items.Clear();
		}
		private void btnOk_Click(object sender, RoutedEventArgs e)
		{
			if (!DataModel.Instance.CustomeGroupName.ContainsKey(this.groupId))
			{
				base.Close();
			}
			else
			{
				CustomGroup group = DataModel.Instance.CustomeGroupName[this.groupId];
				if (group != null)
				{
					if (group.Members == null)
					{
						group.Members = new System.Collections.Generic.List<Staff>();
					}
					else
					{
						group.Members.Clear();
					}
					System.Text.StringBuilder sb = new System.Text.StringBuilder();
					if (this.lstBoxTo.Items.Count != 0)
					{
						foreach (ListBoxItem item in (System.Collections.IEnumerable)this.lstBoxTo.Items)
						{
							Staff staff = item.DataContext as Staff;
							if (staff != null && !group.Members.Contains(staff) && staff.Uid != ServiceUtil.Instance.SessionService.Uid)
							{
								group.Members.Add(staff);
								sb.Append(staff.Uid + ":");
							}
						}
					}
					this.Addmember2Group(sb.ToString());
				}
				base.Close();
			}
		}
		private void Addmember2Group(string members)
		{
			if (DataModel.Instance.CustomeGroupName.ContainsKey(this.groupId))
			{
				CustomGroup group = DataModel.Instance.CustomeGroupName[this.groupId];
				if (group != null)
				{
					if (group.Members != null)
					{
						System.Text.StringBuilder strAll = new System.Text.StringBuilder();
						foreach (Staff s in group.Members)
						{
							if (s.Uid != ServiceUtil.Instance.SessionService.Uid)
							{
								strAll.Append(s.Uid + ":");
							}
						}
						CustomGroupManagerWindowViewModel.GetInstance().AddMemberToCustomGroup((int)ServiceUtil.Instance.SessionService.Uid, this.groupId, strAll.ToString(), members);
					}
				}
			}
		}
		private void ImageButton_Click(object sender, RoutedEventArgs e)
		{
			base.Close();
		}
		private void btnMin_Click(object sender, RoutedEventArgs e)
		{
			base.WindowState = WindowState.Minimized;
		}
		private void lblDeleteAll_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				if (this.lstBoxTo.Items.Count > 0)
				{
					this.lstBoxTo.Items.Clear();
				}
			}
		}
		public void UpdateMember(Staff staff)
		{
			for (int i = 0; i < this.lstBoxFrom.Items.Count; i++)
			{
				Staff staffNew = (this.lstBoxFrom.Items[i] as CustomMemberItem).DataContext as Staff;
				if (staffNew != null && staff.Uid == staffNew.Uid)
				{
					this.lstBoxFrom.Items.RemoveAt(i);
					CustomMemberItem item = new CustomMemberItem(CustomMemberType.Add);
					item.DataContext = staff;
					item.imgHead.Source = staff.HeaderImage;
					item.tbkAccount.Text = staff.Name;
					item.ItemAdd += new System.EventHandler(this.item_ItemAdd);
					this.lstBoxFrom.Items.Insert(i, item);
					break;
				}
			}
			for (int i = 0; i < this.lstBoxTo.Items.Count; i++)
			{
				Staff staffNew = (this.lstBoxTo.Items[i] as CustomMemberItem).DataContext as Staff;
				if (staffNew != null && staff.Uid == staffNew.Uid)
				{
					this.lstBoxTo.Items.RemoveAt(i);
					CustomMemberItem item = new CustomMemberItem(CustomMemberType.Add);
					item.DataContext = staff;
					item.imgHead.Source = staff.HeaderImage;
					item.tbkAccount.Text = staff.Name;
					item.ItemAdd += new System.EventHandler(this.item_ItemAdd);
					this.lstBoxTo.Items.Insert(i, item);
					break;
				}
			}
		}
		private void lblAddAll_MouseDown(object sender, MouseButtonEventArgs e)
		{
			foreach (CustomMemberItem item in (System.Collections.IEnumerable)this.lstBoxFrom.Items)
			{
				this.AddOneStaff(item);
			}
		}
	}
}
