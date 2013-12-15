using IDKin.IM.Core;
using IDKin.IM.Windows.View;
using IDKin.IM.Windows.View.Commons;
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
    public partial class EntStaffTab : BaseTab//, IComponentConnector
    {
        private Staff staff = null;
        //private bool _contentLoaded;
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
        public PersonalChatTabControl TabContent
        {
            get
            {
                return base.Content as PersonalChatTabControl;
            }
            set
            {
                base.Content = value;
            }
        }
        public EntStaffTab(Staff staff)
        {
            if (staff != null)
            {
                this.InitializeComponent();
                this.staff = staff;
                this.TabHeader.Label = staff.Name;
                this.TabHeader.imgIcon.Source = staff.HeaderImage;
                base.Tag = new MenuItem
                {
                    Icon = new Image
                    {
                        Width = 16.0,
                        Height = 16.0,
                        Source = staff.HeaderImage
                    },
                    Header = staff.Name
                };
                this.TabContent = new PersonalChatTabControl(staff);
                base.SetFocus2DesktopButton();
                this.AddEventListenerHandler();
            }
        }
        public void UpdateInfo()
        {
            this.TabHeader.Label = this.staff.Name;
            this.TabHeader.imgIcon.Source = this.staff.HeaderImage;
            base.Tag = new MenuItem
            {
                Icon = new Image
                {
                    Width = 16.0,
                    Height = 16.0,
                    Source = this.staff.HeaderImage
                },
                Header = this.staff.Name
            };
        }
        private void AddEventListenerHandler()
        {
            base.CloseTab += new RoutedEventHandler(this.EntStaffTab_CloseTab);
            if (this.TabContent != null && this.TabContent.ChatComponent != null)
            {
                this.TabContent.ChatComponent.btnClose.Click += new RoutedEventHandler(this.btnClose_Click);
            }
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (this.CloseTabTip())
            {
                INWindow inWin = this.dataService.INWindow as INWindow;
                inWin.ContentTab.Items.Remove(this);
                this.dataService.RemoveStaffChatTab(this.staff.Uid);
            }
        }
        private void EntStaffTab_CloseTab(object sender, RoutedEventArgs e)
        {
            if (this.CloseTabTip())
            {
                TabItem tabItem = e.Source as TabItem;
                if (tabItem != null)
                {
                    TabControl tabControl = tabItem.Parent as TabControl;
                    if (tabControl != null)
                    {
                        tabControl.Items.Remove(tabItem);
                        this.dataService.RemoveStaffChatTab(this.staff.Uid);
                    }
                }
            }
        }
        private bool CloseTabTip()
        {
            return this.TabContent.FileList.fileList.Items.Count == 0 || Alert.Show("文件正在传输：确定要关闭吗?", "提示", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes;
        }
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/chattab/entstafftab.xaml", UriKind.Relative);
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
