using IDKin.IM.Core;
using IDKin.IM.CustomComponents.Controls;
using IDKin.IM.Data;
using IDKin.IM.Theme.Helper;
using IDKin.IM.Windows.Model;
using IDKin.IM.Windows.Util;
using IDKin.IM.Windows.ViewModel;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
namespace IDKin.IM.Windows.View
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class EntGroupManagerWindow : Window//, IComponentConnector
	{
		private IDataService dataService = ServiceUtil.Instance.DataService;
		private ISessionService sessionService = ServiceUtil.Instance.SessionService;
		private EntGroupManagerWindowViewModel viewModel = new EntGroupManagerWindowViewModel();
		private EntGroup group = null;
        //internal Border OuterBorder;
        //internal Border InnerBorder;
        //internal StatusBar topBar;
        //internal Image imgIcon;
        //internal ImageButton btnMin;
        //internal ImageButton btnClose;
        //internal TextBox txtName;
        //internal TextBox txtDescription;
        //internal ListBox lstMember;
        //internal Button btnSelect;
        //internal Button btnRemove;
        //internal ComboBox cboCategory;
        //internal ListBox lstStaff;
        //internal Button btnAddAll;
        //internal Button btnRemoveAll;
        //internal Button btnEnter;
        //internal Button btnCancel;
        //private bool _contentLoaded;
		public EntGroupManagerWindow()
		{
			ThemeSwitcher.LoadSkin(ThemeEnum.Aero, this);
			this.InitializeComponent();
			this.InitData();
			base.Closed += new System.EventHandler(this.EntGroupManagerWindow_Closed);
		}
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonDown(e);
			base.DragMove();
		}
		public EntGroupManagerWindow(EntGroup group)
		{
			if (group != null)
			{
				ThemeSwitcher.LoadSkin(ThemeEnum.Aero, this);
				this.InitializeComponent();
				this.group = group;
				this.txtName.Text = group.Name;
				this.txtDescription.Text = group.Description;
				long[] memebers = this.group.Member;
				if (memebers != null && memebers.Length > 0)
				{
					long[] array = memebers;
					for (int i = 0; i < array.Length; i++)
					{
						long member = array[i];
						Staff staff = this.dataService.GetStaff(member);
						if (staff != null)
						{
							this.AddListMemberItem(staff);
						}
					}
				}
				this.InitData();
			}
			base.Closed += new System.EventHandler(this.EntGroupManagerWindow_Closed);
		}
		private void EntGroupManagerWindow_Closed(object sender, System.EventArgs e)
		{
			if (this.group != null)
			{
				WindowModel.Instance.RemoveEntGroupManagerWindow(this.group.Gid);
			}
			else
			{
				WindowModel.Instance.RemoveEntGroupManagerWindow(-1L);
			}
		}
		private void AddListMemberItem(Staff staff)
		{
			if (staff != null)
			{
				ListBoxItem lbi = new ListBoxItem();
				lbi.Content = staff.Name;
				lbi.DataContext = staff;
				this.lstMember.Items.Add(lbi);
			}
		}
		private void InitData()
		{
			System.Collections.Generic.ICollection<Department> departmentCollection = this.dataService.GetDepartmentList();
			if (departmentCollection != null)
			{
				foreach (Department department in departmentCollection)
				{
					ComboBoxItem cbi = new ComboBoxItem();
					cbi.Content = department.Name;
					cbi.DataContext = department.Id;
					this.cboCategory.Items.Add(cbi);
				}
				System.Collections.Generic.ICollection<Staff> staffCollection = this.dataService.GetStaffList();
				if (staffCollection != null)
				{
					foreach (Staff staff in staffCollection)
					{
						if (staff.Uid != this.sessionService.Uid)
						{
							ListBoxItem lbi = new ListBoxItem();
							lbi.Content = staff.Name;
							lbi.DataContext = staff;
							this.lstStaff.Items.Add(lbi);
						}
					}
					this.cboCategory.SelectionChanged += new SelectionChangedEventHandler(this.cboCategory_SelectionChanged);
				}
			}
		}
		private void btnOK_Click(object sender, RoutedEventArgs e)
		{
			if (this.txtName.Text.Equals("") || this.txtDescription.Text.Equals(""))
			{
				MessageBox.Show("请填写群名称 群简介(群公告)!", "提示");
			}
			else
			{
				if (this.lstMember.Items.Count == 0)
				{
					MessageBox.Show("请选择群成员", "提示");
				}
				else
				{
					if (this.group != null)
					{
						this.UpdateGroup();
					}
					else
					{
						this.CreateGroup();
					}
					base.Close();
				}
			}
		}
		private void CreateGroup()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			foreach (ListBoxItem i in (System.Collections.IEnumerable)this.lstMember.Items)
			{
				Staff vo = i.DataContext as Staff;
				if (vo.Uid != this.sessionService.Uid)
				{
					sb.Append(vo.Uid);
					sb.Append(":");
				}
			}
			this.viewModel.Create(this.sessionService.Uid, this.txtName.Text, this.txtDescription.Text, sb.ToString());
		}
		private void UpdateGroup()
		{
			if (!this.txtName.Text.Equals(this.group.Name) || !this.txtDescription.Text.Equals(this.group.Description))
			{
				this.viewModel.Update(this.group.Gid, this.txtName.Text, this.txtDescription.Text, "");
			}
			string deleteGroupMembers = this.FindDeleteGroupMembers();
			string addGroupMembers = this.FindAddGroupMembers();
			if (!string.IsNullOrEmpty(deleteGroupMembers))
			{
				this.viewModel.MemberRemove(this.group.Gid, deleteGroupMembers);
			}
			if (!string.IsNullOrEmpty(addGroupMembers))
			{
				this.viewModel.AddMember(this.group.Gid, addGroupMembers);
			}
		}
		private string FindDeleteGroupMembers()
		{
			bool hasMember = false;
			System.Collections.Generic.List<uint> deleteMembers = new System.Collections.Generic.List<uint>();
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			long[] member = this.group.Member;
			for (int i = 0; i < member.Length; i++)
			{
				uint uid = (uint)member[i];
				hasMember = false;
				foreach (ListBoxItem item in (System.Collections.IEnumerable)this.lstMember.Items)
				{
					Staff staff = item.DataContext as Staff;
					if (staff != null && staff.Uid == (long)((ulong)uid))
					{
						hasMember = true;
						break;
					}
				}
				if (!hasMember)
				{
					sb.Append(uid);
					sb.Append(":");
				}
			}
			return sb.ToString();
		}
		private string FindAddGroupMembers()
		{
			System.Collections.Generic.List<uint> addMembers = new System.Collections.Generic.List<uint>();
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			foreach (ListBoxItem item in (System.Collections.IEnumerable)this.lstMember.Items)
			{
				bool hasMember = false;
				Staff staff = item.DataContext as Staff;
				long[] member = this.group.Member;
				for (int i = 0; i < member.Length; i++)
				{
					uint uid = (uint)member[i];
					if (staff != null && staff.Uid == (long)((ulong)uid))
					{
						hasMember = true;
						break;
					}
				}
				if (!hasMember)
				{
					sb.Append(staff.Uid);
					sb.Append(":");
				}
			}
			return sb.ToString();
		}
		private void cboCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.lstStaff.Items.Clear();
			ComboBoxItem cbi = (sender as ComboBox).SelectedValue as ComboBoxItem;
			long depid = (long)((ulong)uint.Parse(cbi.DataContext.ToString()));
			System.Collections.Generic.ICollection<Staff> staffCollection = this.dataService.GetStaffList();
			if (depid == 0L)
			{
				foreach (Staff staff in staffCollection)
				{
					if (staff.Uid != this.sessionService.Uid)
					{
						ListBoxItem lbi = new ListBoxItem();
						lbi.Content = staff.Name;
						lbi.DataContext = staff;
						this.lstStaff.Items.Add(lbi);
					}
				}
			}
			else
			{
				foreach (Staff staff in staffCollection)
				{
					if (staff.DepartmentId == depid && staff.Uid != this.sessionService.Uid)
					{
						ListBoxItem lbi = new ListBoxItem();
						lbi.Content = staff.Name;
						lbi.DataContext = staff;
						this.lstStaff.Items.Add(lbi);
					}
				}
			}
		}
		private void btnSelect_Click(object sender, RoutedEventArgs e)
		{
			this.AddContact();
		}
		private void AddContact()
		{
			System.Collections.IList list = this.lstStaff.SelectedItems;
			foreach (ListBoxItem lbi in list)
			{
				Staff staff = lbi.DataContext as Staff;
				if (staff != null)
				{
					bool exist = false;
					foreach (ListBoxItem i in (System.Collections.IEnumerable)this.lstMember.Items)
					{
						Staff v = i.DataContext as Staff;
						if (v != null && v.Uid == staff.Uid)
						{
							exist = true;
							break;
						}
					}
					if (!exist)
					{
						ListBoxItem lb = new ListBoxItem();
						lb.Content = staff.Name;
						lb.DataContext = staff;
						this.lstMember.Items.Add(lb);
					}
				}
			}
		}
		private void btnRemove_Click(object sender, RoutedEventArgs e)
		{
			this.RemoveContact();
		}
		private void RemoveContact()
		{
			System.Collections.Generic.List<ListBoxItem> list = new System.Collections.Generic.List<ListBoxItem>();
			foreach (ListBoxItem lbi in (System.Collections.IEnumerable)this.lstMember.Items)
			{
				if (lbi.IsSelected)
				{
					list.Add(lbi);
				}
			}
			foreach (ListBoxItem lbi in list)
			{
				if (!this.IsUserItem(lbi))
				{
					this.lstMember.Items.Remove(lbi);
				}
			}
		}
		private void btnAddAll_Click(object sender, RoutedEventArgs e)
		{
			System.Collections.IList list = this.lstStaff.Items;
			foreach (ListBoxItem lbi in list)
			{
				Staff vo = lbi.DataContext as Staff;
				bool exist = false;
				foreach (ListBoxItem i in (System.Collections.IEnumerable)this.lstMember.Items)
				{
					Staff v = i.DataContext as Staff;
					if (v.Uid == vo.Uid)
					{
						exist = true;
						break;
					}
				}
				if (!exist)
				{
					ListBoxItem lb = new ListBoxItem();
					lb.Content = vo.Name;
					lb.DataContext = vo;
					this.lstMember.Items.Add(lb);
				}
			}
		}
		private void btnRemoveAll_Click(object sender, RoutedEventArgs e)
		{
			System.Collections.Generic.List<ListBoxItem> list = new System.Collections.Generic.List<ListBoxItem>();
			foreach (ListBoxItem lbi in (System.Collections.IEnumerable)this.lstMember.Items)
			{
				list.Add(lbi);
			}
			foreach (ListBoxItem lbi in list)
			{
				if (!this.IsUserItem(lbi))
				{
					this.lstMember.Items.Remove(lbi);
				}
			}
		}
		private bool IsUserItem(ListBoxItem lbi)
		{
			bool result;
			if (lbi != null)
			{
				Staff staff = lbi.DataContext as Staff;
				if (staff != null && staff.Uid == this.sessionService.Uid)
				{
					result = true;
					return result;
				}
			}
			result = false;
			return result;
		}
		private void btnClose_Click(object sender, RoutedEventArgs e)
		{
			base.Close();
		}
		private void btnMin_Click(object sender, RoutedEventArgs e)
		{
			this.MinHandler();
		}
		private void MinHandler()
		{
			if (base.WindowState == WindowState.Normal)
			{
				base.WindowState = WindowState.Minimized;
			}
			else
			{
				base.WindowState = WindowState.Normal;
			}
		}
		private void Window_Closed(object sender, System.EventArgs e)
		{
			WindowModel.Instance.GroupManagerWindow = null;
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/view/entgroupmanagerwindow.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        ((EntGroupManagerWindow)target).Closed += new System.EventHandler(this.Window_Closed);
        //        break;
        //    case 2:
        //        this.OuterBorder = (Border)target;
        //        break;
        //    case 3:
        //        this.InnerBorder = (Border)target;
        //        break;
        //    case 4:
        //        this.topBar = (StatusBar)target;
        //        break;
        //    case 5:
        //        this.imgIcon = (Image)target;
        //        break;
        //    case 6:
        //        this.btnMin = (ImageButton)target;
        //        this.btnMin.Click += new RoutedEventHandler(this.btnMin_Click);
        //        break;
        //    case 7:
        //        this.btnClose = (ImageButton)target;
        //        this.btnClose.Click += new RoutedEventHandler(this.btnClose_Click);
        //        break;
        //    case 8:
        //        this.txtName = (TextBox)target;
        //        break;
        //    case 9:
        //        this.txtDescription = (TextBox)target;
        //        break;
        //    case 10:
        //        this.lstMember = (ListBox)target;
        //        break;
        //    case 11:
        //        this.btnSelect = (Button)target;
        //        this.btnSelect.Click += new RoutedEventHandler(this.btnSelect_Click);
        //        break;
        //    case 12:
        //        this.btnRemove = (Button)target;
        //        this.btnRemove.Click += new RoutedEventHandler(this.btnRemove_Click);
        //        break;
        //    case 13:
        //        this.cboCategory = (ComboBox)target;
        //        break;
        //    case 14:
        //        this.lstStaff = (ListBox)target;
        //        break;
        //    case 15:
        //        this.btnAddAll = (Button)target;
        //        this.btnAddAll.Click += new RoutedEventHandler(this.btnAddAll_Click);
        //        break;
        //    case 16:
        //        this.btnRemoveAll = (Button)target;
        //        this.btnRemoveAll.Click += new RoutedEventHandler(this.btnRemoveAll_Click);
        //        break;
        //    case 17:
        //        this.btnEnter = (Button)target;
        //        this.btnEnter.Click += new RoutedEventHandler(this.btnOK_Click);
        //        break;
        //    case 18:
        //        this.btnCancel = (Button)target;
        //        this.btnCancel.Click += new RoutedEventHandler(this.btnClose_Click);
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
