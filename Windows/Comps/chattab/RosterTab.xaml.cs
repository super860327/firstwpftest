using IDKin.IM.Core;
using IDKin.IM.Windows.View;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
namespace IDKin.IM.Windows.Comps.ChatTab
{
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class RosterTab : BaseTab//, IComponentConnector
    {
        private Roster roster = null;
        ////private bool _contentLoaded;
        public FriendsChatTabControl TabContent
        {
            get
            {
                return base.Content as FriendsChatTabControl;
            }
            set
            {
                base.Content = value;
            }
        }
        public RosterTab(Roster roster)
        {
            if (roster != null)
            {
                this.InitializeComponent();
                this.roster = roster;
                this.TabHeader.Label = roster.Name;
                this.TabHeader.imgIcon.Source = roster.HeaderImage;
                base.Tag = new MenuItem
                {
                    Icon = new Image
                    {
                        Source = roster.HeaderImage
                    },
                    Header = roster.Name
                };
                this.TabContent = new FriendsChatTabControl(roster);
                base.SetFocus2DesktopButton();
                this.AddEventListenerHandler();
            }
        }
        private void AddEventListenerHandler()
        {

            base.CloseTab += new RoutedEventHandler(this.RosterTab_CloseTab);
            if (this.TabContent != null && this.TabContent.ChatComponent != null)
            {
                this.TabContent.ChatComponent.btnClose.Click += new RoutedEventHandler(this.btnClose_Click);
            }
        }
        private void RosterTab_CloseTab(object sender, RoutedEventArgs e)
        {
            try
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
            catch (System.Exception ex)
            {
                this.logger.Error(ex.ToString());
            }
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                INWindow inWin = this.dataService.INWindow as INWindow;
                inWin.ContentTab.Items.Remove(this);
                this.dataService.RemoveRosterChatTab(this.roster.Uid);
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
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/chattab/rostertab.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    this._contentLoaded = true;
        //}
    }
}
