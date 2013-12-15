using IDKin.IM.Core;
using IDKin.IM.Data;
using IDKin.IM.ImageService;
using IDKin.IM.Windows.Util;
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
    public partial class EntGroupTab : BaseTab//, IComponentConnector
    {
        private EntGroup group = null;
        private ISessionService sessionService = ServiceUtil.Instance.SessionService;
        private IImageService imageService = ServiceUtil.Instance.ImageService;
        ////private bool _contentLoaded;
        public GroupChatTabControl TabContent
        {
            get
            {
                return base.Content as GroupChatTabControl;
            }
            set
            {
                base.Content = value;
            }
        }
        public EntGroupTab(EntGroup group)
        {
            if (group != null)
            {
                this.InitializeComponent();
                this.group = group;
                this.TabHeader.Label = group.Name;
                this.TabHeader.imgIcon.Source = this.imageService.GetIcon(ImageTypeIcon.Group);
                base.Tag = new MenuItem
                {
                    Icon = new Image
                    {
                        Width = 16.0,
                        Height = 16.0,
                        Source = this.imageService.GetIcon(ImageTypeIcon.Group)
                    },
                    Header = group.Name
                };
                this.TabContent = new GroupChatTabControl(group);
                base.SetFocus2DesktopButton();
                this.AddEventListenerHandler();
            }
        }
        private void AddEventListenerHandler()
        {
            base.CloseTab += new RoutedEventHandler(this.EntGroupTab_CloseTab);
            if (this.TabContent != null && this.TabContent.ChatComponent != null)
            {
                this.TabContent.ChatComponent.btnClose.Click += new RoutedEventHandler(this.btnClose_Click);
            }
        }
        private void EntGroupTab_CloseTab(object sender, RoutedEventArgs e)
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
                        this.dataService.RemoveEntGroupChatTab(this.group.Gid);
                    }
                }
            }
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (this.CloseTabTip())
            {
                INWindow inWin = this.dataService.INWindow as INWindow;
                inWin.ContentTab.Items.Remove(this);
                this.dataService.RemoveEntGroupChatTab(this.group.Gid);
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
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/chattab/entgrouptab.xaml", UriKind.Relative);
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
