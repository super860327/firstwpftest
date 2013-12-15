using IDKin.IM.Core;
using IDKin.IM.Data;
using IDKin.IM.Log;
using IDKin.IM.Windows.Comps.ChatTab;
using IDKin.IM.Windows.Model;
using IDKin.IM.Windows.Properties;
using IDKin.IM.Windows.Util;
using IDKin.IM.Windows.View;
using System;
using System.CodeDom.Compiler;
using System.Collections.Specialized;
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
    public partial class FastReplyPopup : Popup//, IComponentConnector, IStyleConnector
	{
		private ILogger logger = ServiceUtil.Instance.Logger;
		private IDataService dataService = ServiceUtil.Instance.DataService;
		private EntGroup group = null;
		private Staff staff = null;
		private Roster roster = null;
		//internal ListBox listBox;
        ////private bool _contentLoaded;
		public EntGroup Group
		{
			get
			{
				return this.group;
			}
			set
			{
				this.group = value;
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
		public Roster Roster
		{
			get
			{
				return this.roster;
			}
			set
			{
				this.roster = value;
			}
		}
		public FastReplyPopup()
		{
			this.InitializeComponent();
			this.InitUI();
			this.listBox.SelectedIndex = Settings.Default.FastResponseIndex;
		}
		private void InitUI()
		{
			StringCollection sc = Settings.Default.SystemSetup_FastReply_Message;
			int size = sc.Count;
			this.listBox.Items.Clear();
			for (int i = 0; i < size; i++)
			{
				ListBoxItem item = new ListBoxItem();
				item.Content = sc[i];
				this.listBox.Items.Add(item);
			}
			this.listBox.SelectedIndex = Settings.Default.FastResponseIndex;
		}
		private void ListBoxItem_MouseUp(object sender, MouseButtonEventArgs e)
		{
			try
			{
				ListBoxItem source = this.listBox.SelectedItem as ListBoxItem;
				System.Console.WriteLine("source.Content.ToString().Trim()" + source.Content.ToString().Trim());
				source.IsSelected = true;
				string message = source.Content.ToString().Trim();
				this.AddMessage(message);
				base.IsOpen = false;
				Settings.Default.FastResponseIndex = this.listBox.SelectedIndex;
				Settings.Default.Save();
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		private void AddMessage(string message)
		{
			if (!string.IsNullOrEmpty(message))
			{
				if (this.staff != null)
				{
					EntStaffTab item = this.dataService.GetStaffChatTab(this.staff.Uid) as EntStaffTab;
					if (item != null)
					{
						item.TabContent.ChatComponent.AddMessageInputMsgBox(message);
						item.TabContent.ChatComponent.SendMessageHandler();
					}
				}
				if (this.group != null)
				{
					EntGroupTab item2 = this.dataService.GetEntGroupChatTab(this.group.Gid) as EntGroupTab;
					if (item2 != null)
					{
						item2.TabContent.ChatComponent.AddMessageInputMsgBox(message);
						item2.TabContent.ChatComponent.SendMessageHandler();
					}
				}
				if (this.roster != null)
				{
					RosterTab item3 = this.dataService.GetRosterChatTab(this.roster.Uid) as RosterTab;
					if (item3 != null)
					{
						item3.TabContent.ChatComponent.AddMessageInputMsgBox(message);
						item3.TabContent.ChatComponent.SendMessageHandler();
					}
				}
			}
		}
		private void AddItemHandler(object sender, RoutedEventArgs e)
		{
			try
			{
				SystemSettingWindow systemSetting = WindowModel.Instance.SystemSettingWindow;
				systemSetting.Show();
				systemSetting.Activate();
				systemSetting.rdoReply.IsChecked = new bool?(true);
				base.IsOpen = false;
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/fastreplypopup.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 2:
        //        this.listBox = (ListBox)target;
        //        break;
        //    case 3:
        //        ((Button)target).Click += new RoutedEventHandler(this.AddItemHandler);
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IStyleConnector.Connect(int connectionId, object target)
        //{
        //    if (connectionId == 1)
        //    {
        //        EventSetter eventSetter = new EventSetter();
        //        eventSetter.Event = UIElement.MouseUpEvent;
        //        eventSetter.Handler = new MouseButtonEventHandler(this.ListBoxItem_MouseUp);
        //        ((Style)target).Setters.Add(eventSetter);
        //    }
        //}
	}
}
