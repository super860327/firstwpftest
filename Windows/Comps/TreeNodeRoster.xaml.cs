using IDKin.IM.Core;
using IDKin.IM.Data;
using IDKin.IM.Windows.Comps.ChatTab;
using IDKin.IM.Windows.Util;
using IDKin.IM.Windows.View;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
namespace IDKin.IM.Windows.Comps
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public  partial  class TreeNodeRoster : ListViewItem//, IComponentConnector
	{
		private ISessionService sessionService = ServiceUtil.Instance.SessionService;
		private IDataService dataService = ServiceUtil.Instance.DataService;
		private Roster roster;
		private UserStatus status;
		//internal WrapPanel wrapPanel;
		//internal Canvas canvas;
		//internal TreeNodeStaffHead imgFace;
		//internal Image imgFaceForeground;
		//internal TextBlock txtName;
		//internal TextBlock txtSignagrue;
        ////private bool _contentLoaded;
		public TreeNodeRoster(Roster roster)
		{
			try
			{
				this.InitializeComponent();
				this.roster = roster;
				this.status = roster.Status;
				this.UpdateInfo();
				this.AddEventListenerHandler();
			}
			catch (System.Exception ex)
			{
				System.Console.WriteLine(ex.ToString());
			}
		}
		private void AddEventListenerHandler()
		{
			base.MouseDoubleClick += new MouseButtonEventHandler(this.UserControl_MouseDoubleClick);
		}
		public bool UpdateInfo()
		{
			bool result;
			try
			{
				bool update = false;
				this.txtName.Text = this.roster.Name;
				this.txtSignagrue.Text = this.roster.Signature;
				this.imgFace.bgHead.ImageSource = this.roster.HeaderImage;
				this.imgFaceForeground.Source = this.roster.StatusIcon;
				if (this.status != this.roster.Status)
				{
					this.status = this.roster.Status;
					update = true;
				}
				result = update;
			}
			catch (System.Exception ex)
			{
				System.Console.WriteLine(ex.ToString());
				result = false;
			}
			return result;
		}
		private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			try
			{
				if (e.LeftButton == MouseButtonState.Pressed && this.roster.Uid != this.sessionService.Uid)
				{
					RosterTab item = this.dataService.GetRosterChatTab(this.roster.Uid) as RosterTab;
					if (item != null)
					{
						((INWindow)this.dataService.INWindow).ContentTab.SelectedItem = item;
					}
					else
					{
						item = new RosterTab(this.roster);
						item.SetDefaultStyle();
						((INWindow)this.dataService.INWindow).ContentTab.Items.Add(item);
						this.dataService.AddRosterChatTab(this.roster.Uid, item);
						((INWindow)this.dataService.INWindow).ContentTab.SelectedItem = item;
					}
				}
			}
			catch (System.Exception)
			{
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
					this.dataService.RemoveRosterChatTab(this.roster.Uid);
				}
			}
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/treenoderoster.xaml", UriKind.Relative);
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
        //        this.txtName = (TextBlock)target;
        //        break;
        //    case 6:
        //        this.txtSignagrue = (TextBlock)target;
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
