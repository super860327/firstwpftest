using IDKin.IM.Core;
using IDKin.IM.CustomComponents.Controls;
using IDKin.IM.Data;
using IDKin.IM.Protocol.Commons;
using IDKin.IM.Theme.Helper;
using IDKin.IM.Windows.Comps;
using IDKin.IM.Windows.Comps.MessageCenter;
using IDKin.IM.Windows.Model;
using IDKin.IM.Windows.Util;
using IDKin.IM.Windows.ViewModel;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
namespace IDKin.IM.Windows.View
{
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class MessageCenterWindow : BaseWindow, System.IDisposable//, IComponentConnector
    {
        private ISessionService sessionService = ServiceUtil.Instance.SessionService;
        private IDataService dataService = ServiceUtil.Instance.DataService;
        private MessageCenterViewModel messageCenter = null;
        private bool first = false;
        private bool IsMarkNotice = false;
        private System.Collections.Generic.ICollection<EntGroup> groups = null;

        public MessageCenterWindow()
        {
            ThemeSwitcher.LoadSkin(ThemeEnum.Aero, this);
            this.InitializeComponent();
            this.InitEventHandler();
            this.messageCenter = new MessageCenterViewModel();
            this.groups = this.dataService.GetEntGroupList();
        }
        private void InitEventHandler()
        {
            base.Closing += new CancelEventHandler(this.Window_Closing);
            base.Loaded += new RoutedEventHandler(this.Window_Loaded);
            base.ContentRendered += new System.EventHandler(this.Window_ContentRendered);
            this.work.Expanded += (new RoutedEventHandler(this.ExpandedHandler));
            this.friends.Expanded += (new RoutedEventHandler(this.ExpandedHandler));
            this.marked.Expanded += (new RoutedEventHandler(this.ExpandedHandler));
            this.OANotice.Expanded += (new RoutedEventHandler(this.ExpandedHandler));
            this.btnWork.Click += new RoutedEventHandler(this.Button_Click);
            this.btnMark.Click += new RoutedEventHandler(this.Button_Click);
            this.btnOANotice.Click += new RoutedEventHandler(this.Button_Click);
            base.LostFocus += new RoutedEventHandler(this.Window_LostFocus);
        }
        private void Window_LostFocus(object sender, System.EventArgs e)
        {
            base.Topmost = false;
        }
        private void Window_ContentRendered(object sender, System.EventArgs e)
        {
            if (base.Topmost)
            {
                base.Topmost = false;
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.work.IsExpanded = true;
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            WindowModel.Instance.Markpage = null;
            WindowModel.Instance.MarkTreeView = null;
            WindowModel.Instance.MsgRecordPage = null;
            WindowModel.Instance.OARecordPage = null;
            WindowModel.Instance.OATreeView = null;
            WindowModel.Instance.RecentPage = null;
            WindowModel.Instance.MessageCenterWindow = null;
        }
        private void WindowMaxHandler()
        {
            if (base.WindowState == WindowState.Normal)
            {
                base.WindowState = WindowState.Maximized;
                this.btnMax.NormalImage = "/IDKin.IM.Windows;component/Resources/Image/restore.png";
                this.btnMax.HoverImage = "/IDKin.IM.Windows;component/Resources/Image/restoreOver.png";
                this.btnMax.PressedImage = "/IDKin.IM.Windows;component/Resources/Image/restorePress.png";
            }
            else
            {
                base.WindowState = WindowState.Normal;
                this.btnMax.NormalImage = "/IDKin.IM.Windows;component/Resources/Image/max.png";
                this.btnMax.HoverImage = "/IDKin.IM.Windows;component/Resources/Image/maxOver.png";
                this.btnMax.PressedImage = "/IDKin.IM.Windows;component/Resources/Image/maxPress.png";
            }
        }
        private void StatusBar_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.WindowMaxHandler();
        }
        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            base.WindowState = WindowState.Minimized;
        }
        private void btnMax_Click(object sender, RoutedEventArgs e)
        {
            this.WindowMaxHandler();
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            System.GC.Collect();
            base.Close();
        }
        private void ExpandedHandler(object sender, RoutedEventArgs e)
        {
            this.ExpandedMethod(e);
        }
        private void ExpandedMethod(RoutedEventArgs e)
        {
            if (e.Source == this.work)
            {
                this.ExpandWork();
            }
            if (e.Source == this.friends)
            {
                this.ExpandFriends();
            }
            if (e.Source == this.marked)
            {
                this.ExpandMarked();
            }
            if (e.Source == this.OANotice)
            {
                this.ExpandOANotice();
            }
        }
        public void ExpandWork()
        {
            this.friends.IsExpanded = false;
            this.marked.IsExpanded = false;
            this.OANotice.IsExpanded = false;
            this.row0.Height = new GridLength(1.0, GridUnitType.Star);
            this.row1.Height = new GridLength(1.0, GridUnitType.Auto);
            this.row2.Height = new GridLength(1.0, GridUnitType.Auto);
            this.row3.Height = new GridLength(1.0, GridUnitType.Auto);
            if (this.first)
            {
                WindowModel.Instance.RecentPage.InitData();
                this.ViewMsgFrame.NavigationService.Navigate(WindowModel.Instance.RecentPage);
            }
            else
            {
                this.first = true;
            }
            this.IsMarkNotice = false;
        }
        private void ExpandFriends()
        {
            this.work.IsExpanded = false;
            this.marked.IsExpanded = false;
            this.OANotice.IsExpanded = false;
            this.row0.Height = new GridLength(1.0, GridUnitType.Auto);
            this.row1.Height = new GridLength(1.0, GridUnitType.Star);
            this.row2.Height = new GridLength(1.0, GridUnitType.Auto);
            this.row3.Height = new GridLength(1.0, GridUnitType.Auto);
            WindowModel.Instance.MsgRecordPage.trgMessageTable.Rows.Clear();
            this.ViewMsgFrame.NavigationService.Navigate(WindowModel.Instance.MsgRecordPage);
            WindowModel.Instance.MsgRecordPage.titleName.Text = "";
        }
        private void ExpandMarked()
        {
            this.friends.IsExpanded = false;
            this.work.IsExpanded = false;
            this.OANotice.IsExpanded = false;
            this.row0.Height = new GridLength(1.0, GridUnitType.Auto);
            this.row1.Height = new GridLength(1.0, GridUnitType.Auto);
            this.row2.Height = new GridLength(1.0, GridUnitType.Star);
            this.row3.Height = new GridLength(1.0, GridUnitType.Auto);
            if (!this.IsMarkNotice)
            {
                WindowModel.Instance.Markpage.markedList.Items.Clear();
                WindowModel.Instance.MarkTreeView.WorkName.Items.Clear();
                WindowModel.Instance.MarkTreeView.GroupName.Items.Clear();
                WindowModel.Instance.Markpage.InitData();
                this.ViewMsgFrame.NavigationService.Navigate(WindowModel.Instance.Markpage);
                this.IsMarkNotice = true;
            }
        }
        private void ExpandOANotice()
        {
            this.friends.IsExpanded = false;
            this.marked.IsExpanded = false;
            this.work.IsExpanded = false;
            this.row0.Height = new GridLength(1.0, GridUnitType.Auto);
            this.row1.Height = new GridLength(1.0, GridUnitType.Auto);
            this.row2.Height = new GridLength(1.0, GridUnitType.Auto);
            this.row3.Height = new GridLength(1.0, GridUnitType.Star);
            WindowModel.Instance.OARecordPage.SetShowPage();
            WindowModel.Instance.OARecordPage.trgMessageTable.Rows.Clear();
            WindowModel.Instance.OARecordPage.type = OAModuleType.OA_GETALL_RECORD;
            this.messageCenter.SendOANoticeRequest(this.sessionService.Uid, "0", 1, 10, OAModuleType.OA_GETALL_RECORD);
            this.ViewMsgFrame.NavigationService.Navigate(WindowModel.Instance.OARecordPage);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (e.Source == this.btnWork)
            {
                if (this.work.IsExpanded)
                {
                    WindowModel.Instance.RecentPage.InitData();
                    this.ViewMsgFrame.NavigationService.Navigate(WindowModel.Instance.RecentPage);
                }
            }
            if (e.Source == this.btnMark)
            {
                if (this.IsMarkNotice)
                {
                    WindowModel.Instance.Markpage.markedList.Items.Clear();
                    WindowModel.Instance.MarkTreeView.WorkName.Items.Clear();
                    WindowModel.Instance.Markpage.InitData();
                    this.ViewMsgFrame.NavigationService.Navigate(WindowModel.Instance.Markpage);
                    this.IsMarkNotice = true;
                }
            }
            if (e.Source == this.btnOANotice)
            {
                WindowModel.Instance.OARecordPage.SetShowPage();
                WindowModel.Instance.OARecordPage.trgMessageTable.Rows.Clear();
                WindowModel.Instance.OARecordPage.type = OAModuleType.OA_GETALL_RECORD;
                this.messageCenter.SendOANoticeRequest(this.sessionService.Uid, "0", 1, 10, OAModuleType.OA_GETALL_RECORD);
                this.IsMarkNotice = false;
            }
        }
        public void Dispose()
        {
            this.groups.Clear();
            throw new System.NotImplementedException();
        }
    }
}
