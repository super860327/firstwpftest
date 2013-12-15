using IDKin.IM.Core;
using IDKin.IM.Data;
using IDKin.IM.Windows.Util;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
namespace IDKin.IM.Windows.Comps
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class GroupMemberControl : UserControl//, IComponentConnector
	{
		private ISessionService sessionService = ServiceUtil.Instance.SessionService;
		private IDataService dataService = ServiceUtil.Instance.DataService;
		private EntGroup group;
		//internal RowDefinition GroupDescriptionRow;
		//internal RowDefinition SearchRow;
		//internal RowDefinition GroupMemberListRow;
		//internal StatusBar GroupDescriptionBar;
		//internal TextBox groupDescription;
		//internal StatusBar GroupMemberListBar;
		//internal TextBlock tbkGroupCount;
		//internal IconButton btnSearch;
		//internal TextBox tbxSearch;
		//internal GroupMemberListView GroupMemberList;
        ////private bool _contentLoaded;
		public GroupMemberControl(EntGroup group)
		{
			try
			{
				this.InitializeComponent();
				if (group != null)
				{
					this.group = group;
					this.groupDescription.Text = group.Description;
					this.GroupMemberList.Gid = this.group.Gid;
					if (group.Member != null)
					{
						long[] member = group.Member;
						for (int i = 0; i < member.Length; i++)
						{
							uint uid = (uint)member[i];
							this.GroupMemberList.AddGroupMember(this.dataService.GetStaff((long)((ulong)uid)));
						}
						this.GroupMemberList.SortGroupMember();
					}
				}
			}
			catch (System.Exception)
			{
			}
		}
		private void btnSearch_Click(object sender, RoutedEventArgs e)
		{
			if (this.SearchRow.Height == new GridLength(22.0))
			{
				this.SearchRow.Height = new GridLength(44.0);
				this.tbxSearch.Visibility = Visibility.Visible;
				this.tbxSearch.Focus();
			}
			else
			{
				this.SearchRow.Height = new GridLength(22.0);
				this.tbxSearch.Visibility = Visibility.Collapsed;
			}
		}
		private void StatusBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (e.Source == this.GroupDescriptionBar)
			{
				if (this.GroupDescriptionRow.Height == new GridLength(0.0))
				{
					this.GroupDescriptionRow.Height = new GridLength(166.0);
					this.groupDescription.Visibility = Visibility.Visible;
					this.GroupMemberListRow.Height = new GridLength(1.0, GridUnitType.Star);
					this.GroupMemberList.Visibility = Visibility.Visible;
				}
				else
				{
					this.GroupDescriptionRow.Height = new GridLength(0.0);
					this.groupDescription.Visibility = Visibility.Collapsed;
					this.GroupMemberListRow.Height = new GridLength(1.0, GridUnitType.Star);
					this.GroupMemberList.Visibility = Visibility.Visible;
				}
			}
			if (e.Source == this.GroupMemberListBar)
			{
				if (this.GroupMemberListRow.Height == new GridLength(0.0))
				{
					this.GroupMemberListRow.Height = new GridLength(1.0, GridUnitType.Star);
					this.GroupMemberList.Visibility = Visibility.Visible;
					this.GroupDescriptionRow.Height = new GridLength(166.0);
					this.groupDescription.Visibility = Visibility.Visible;
				}
				else
				{
					this.GroupMemberListRow.Height = new GridLength(0.0);
					this.GroupMemberList.Visibility = Visibility.Collapsed;
					this.GroupDescriptionRow.Height = new GridLength(1.0, GridUnitType.Star);
					this.groupDescription.Visibility = Visibility.Visible;
				}
			}
		}
		public void UpdateGroupStaffCount(int onlineStaff, int totalStaff)
		{
			this.tbkGroupCount.Text = string.Concat(new string[]
			{
				"[",
				onlineStaff.ToString(),
				"/",
				totalStaff.ToString(),
				"]"
			});
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/groupmembercontrol.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        ////[System.Diagnostics.DebuggerNonUserCode]
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
        //        this.GroupDescriptionRow = (RowDefinition)target;
        //        break;
        //    case 2:
        //        this.SearchRow = (RowDefinition)target;
        //        break;
        //    case 3:
        //        this.GroupMemberListRow = (RowDefinition)target;
        //        break;
        //    case 4:
        //        this.GroupDescriptionBar = (StatusBar)target;
        //        this.GroupDescriptionBar.MouseLeftButtonDown += new MouseButtonEventHandler(this.StatusBar_MouseLeftButtonDown);
        //        break;
        //    case 5:
        //        this.groupDescription = (TextBox)target;
        //        break;
        //    case 6:
        //        this.GroupMemberListBar = (StatusBar)target;
        //        this.GroupMemberListBar.MouseLeftButtonDown += new MouseButtonEventHandler(this.StatusBar_MouseLeftButtonDown);
        //        break;
        //    case 7:
        //        this.tbkGroupCount = (TextBlock)target;
        //        break;
        //    case 8:
        //        this.btnSearch = (IconButton)target;
        //        break;
        //    case 9:
        //        this.tbxSearch = (TextBox)target;
        //        break;
        //    case 10:
        //        this.GroupMemberList = (GroupMemberListView)target;
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
