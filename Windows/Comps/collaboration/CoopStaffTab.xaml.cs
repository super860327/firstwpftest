using IDKin.IM.Communicate;
using IDKin.IM.Core;
using IDKin.IM.Core.Core;
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
    public partial class CoopStaffTab : BaseTab//, IComponentConnector
    {
        private CooperationStaff staff = null;
        private IImageService imageService = null;
        private IFileService fileService = null;
        private CooperationProjectWrapper cooperationProjectWrapper = null;
        //private bool _contentLoaded;
        public CooperationStaff Staff
        {
            get
            {
                return this.staff;
            }
            set
            {
                this.staff = value;
                this.TabContent.Staff = this.staff;
                this.staff.CooperationStaffUpdated -= new CooperationStaffUpdatedHandler(this.staff_CooperationStaffUpdated);
                this.staff.CooperationStaffUpdated += new CooperationStaffUpdatedHandler(this.staff_CooperationStaffUpdated);
                this.staff_CooperationStaffUpdated(this.staff);
            }
        }
        public CooperationProjectWrapper CooperationProjectWrapper
        {
            get
            {
                return this.cooperationProjectWrapper;
            }
            set
            {
                this.cooperationProjectWrapper = value;
                this.TabContent.CooperationProjectWrapper = this.cooperationProjectWrapper;
            }
        }
        public CoopStaffChatTabControl TabContent
        {
            get
            {
                return base.Content as CoopStaffChatTabControl;
            }
            set
            {
                base.Content = value;
            }
        }
        private void staff_CooperationStaffUpdated(CooperationStaff cooperationStaff)
        {
            base.Dispatcher.BeginInvoke(new CooperationStaffUpdatedHandler(this.UpdateView), new object[]
			{
				cooperationStaff
			});
        }
        private void UpdateView(CooperationStaff cooperationStaff)
        {
            try
            {
                this.imageService.SetCooperationStaffHeaderImage(cooperationStaff);
                this.imageService.SetCooperationStaffStatusIcon(cooperationStaff);
                this.imageService.SetCooperationStaffHeaderImageOnline(cooperationStaff);
                this.fileService.DownloadCooperationStaffHeader(cooperationStaff);
                base.TabHead.Icon = cooperationStaff.HeaderImage42;
                this.TabHeader.Label = cooperationStaff.Name;
            }
            catch (System.Exception ex)
            {
                this.logger.Error(ex.ToString());
            }
        }
        public CoopStaffTab(CooperationStaff cooperationStaff, CooperationProjectWrapper cooperationProjectWrapper)
        {
            if (cooperationStaff != null)
            {
                this.InitializeComponent();
                this.imageService = ServiceUtil.Instance.ImageService;
                this.fileService = ServiceUtil.Instance.FileService;
                this.TabHeader.Label = cooperationStaff.Name;
                this.TabHeader.imgIcon.Source = cooperationStaff.HeaderImage;
                base.Tag = new MenuItem
                {
                    Icon = new Image
                    {
                        Width = 16.0,
                        Height = 16.0,
                        Source = cooperationStaff.HeaderImage
                    },
                    Header = cooperationStaff.Name
                };
                this.TabContent = new CoopStaffChatTabControl(cooperationStaff, cooperationProjectWrapper);
                this.Staff = cooperationStaff;
                this.CooperationProjectWrapper = cooperationProjectWrapper;
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
            base.CloseTab += new RoutedEventHandler(this.CoopStaffTab_CloseTab);
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
                this.dataService.RemoveCooperationStaffChatTab(this.staff.Uid, this.staff.UnitedProjectid);
            }
        }
        private void CoopStaffTab_CloseTab(object sender, RoutedEventArgs e)
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
                        this.dataService.RemoveCooperationStaffChatTab(this.staff.Uid, this.staff.UnitedProjectid);
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
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/collaboration/coopstafftab.xaml", UriKind.Relative);
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
        //    this._contentLoaded = true;
        //}
    }
}
