using IDKin.IM.Communicate;
using IDKin.IM.Core;
using IDKin.IM.CustomComponents.Controls;
using IDKin.IM.CustomComponents.Controls.TabControl;
using IDKin.IM.Data;
using IDKin.IM.ImageService;
using IDKin.IM.Log;
using IDKin.IM.Protocol.Account;
using IDKin.IM.Protocol.Enterprise;
using IDKin.IM.Theme.Helper;
using IDKin.IM.Windows.Comps;
using IDKin.IM.Windows.Comps.ChatTab;
using IDKin.IM.Windows.Helper;
using IDKin.IM.Windows.Model;
using IDKin.IM.Windows.Properties;
using IDKin.IM.Windows.Util;
using IDKin.IM.Windows.View.Commons;
using IDKin.IM.Windows.View.EmailAlert;
using IDKin.IM.Windows.ViewModel;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
namespace IDKin.IM.Windows.View
{
    public partial class INWindow : BaseWindow
    {
        private delegate void UpdateGroupAllMemberDelegate();
        private bool isTaskClose = true;
        private ChatTabControlViewModel chatTabControlViewModel = new ChatTabControlViewModel();
        private IDataService dataService = ServiceUtil.Instance.DataService;
        private ISessionService sessionService = ServiceUtil.Instance.SessionService;
        private IWSClient wsClient = ServiceUtil.Instance.WsClient;
        private ILogger logger = ServiceUtil.Instance.Logger;
        private IConnection connection = ServiceUtil.Instance.Connection;
        private IFileService fileService = ServiceUtil.Instance.FileService;
        private IImageService imageService = ServiceUtil.Instance.ImageService;
        private IUtilService utilService = ServiceUtil.Instance.utilService;
        private INViewModel inViewModel = null;
        private PopupList popupList;
        private Timer getAppUpdateTimer;
        private Timer getOfflineMessageTimer;
        private int DepartmentStaffAllReceivedCount = 0;
        private Timer updateGroupAllMemberTimer;
        private string approval = "{ln:\"/workflow/approval\"}";
        private string item = "{ln:\"/views/project/list_project.html?tabs=1\"}";
        private string cooperate_with = "{ln:\"/workflow/task/index\"}";
        private string plan = "{ln:\"/plan/calendar\"}";
        private string gis = "{ln:\"/document/file\"}";
        private string forum = "{ln:\"/forum\"}";
        private string gallery = "{ln:\"/\"}";
        private DispatcherTimer dockTimer = null;
        private double dockWindowHeight = 0.0;

        //private bool _contentLoaded;
        public bool IsTaskClose
        {
            get
            {
                return this.isTaskClose;
            }
            set
            {
                this.isTaskClose = value;
            }
        }
        public INWindow()
        {
            ThemeSwitcher.LoadSkin(ThemeEnum.Aero, this);
            this.InitializeComponent();
            this.InitService();
            this.InitHideTimer();
            this.InitDataHandler();
            this.AddEventListenerHandler();
        }
        private void AddEventListenerHandler()
        {
            try
            {
                this.connection.EventHandler.StaffListEvent = new StaffListHandler(this.StaffListEventHandle);
                this.connection.EventHandler.StaffNoticeEvent = new StaffNoticeHandler(this.StaffNoticeEvent);
                this.connection.EventHandler.SocketErrorEvent = new SocketErrorHandler(this.SocketErrorEvent);
                this.btnStatus.miOnline.Click -= new RoutedEventHandler(this.StatusMenuItem_Click);
                this.btnStatus.miAway.Click -= new RoutedEventHandler(this.StatusMenuItem_Click);
                this.btnStatus.miBusy.Click -= new RoutedEventHandler(this.StatusMenuItem_Click);
                this.btnStatus.miDoNotDisturb.Click -= new RoutedEventHandler(this.StatusMenuItem_Click);
                this.btnStatus.miOut.Click -= new RoutedEventHandler(this.StatusMenuItem_Click);
                this.btnStatus.miHide.Click -= new RoutedEventHandler(this.StatusMenuItem_Click);
                this.btnStatus.miMeeting.Click -= new RoutedEventHandler(this.StatusMenuItem_Click);
                this.btnStatus.miOffline.Click -= new RoutedEventHandler(this.StatusMenuItem_Click);
                this.btnStatus.miOnline.Click += new RoutedEventHandler(this.StatusMenuItem_Click);
                this.btnStatus.miAway.Click += new RoutedEventHandler(this.StatusMenuItem_Click);
                this.btnStatus.miBusy.Click += new RoutedEventHandler(this.StatusMenuItem_Click);
                this.btnStatus.miDoNotDisturb.Click += new RoutedEventHandler(this.StatusMenuItem_Click);
                this.btnStatus.miOut.Click += new RoutedEventHandler(this.StatusMenuItem_Click);
                this.btnStatus.miHide.Click += new RoutedEventHandler(this.StatusMenuItem_Click);
                this.btnStatus.miMeeting.Click += new RoutedEventHandler(this.StatusMenuItem_Click);
                this.btnStatus.miOffline.Click += new RoutedEventHandler(this.StatusMenuItem_Click);
                this.btnApproval.Loaded += delegate(object sender, RoutedEventArgs e)
                {
                    this.btnApproval.spnlContent.Orientation = Orientation.Vertical;
                    this.btnApproval.imgContent.Width = 22.0;
                    this.btnApproval.imgContent.Height = 22.0;
                    this.btnApproval.Icon = this.imageService.GetOA(ImageTypeOA.OfficePurchasing);
                };
                this.btnApproval.Unloaded += delegate(object sender, RoutedEventArgs e)
                {
                    this.imageService.RemoveOA(ImageTypeOA.OfficePurchasing);
                };
                this.btnItem.Loaded += delegate(object sender, RoutedEventArgs e)
                {
                    this.btnItem.spnlContent.Orientation = Orientation.Vertical;
                    this.btnItem.imgContent.Width = 22.0;
                    this.btnItem.imgContent.Height = 22.0;
                    this.btnItem.Icon = this.imageService.GetOA(ImageTypeOA.Project);
                };
                this.btnItem.Unloaded += delegate(object sender, RoutedEventArgs e)
                {
                    this.imageService.RemoveOA(ImageTypeOA.Project);
                };
                this.btnCooperate.Loaded += delegate(object sender, RoutedEventArgs e)
                {
                    this.btnCooperate.spnlContent.Orientation = Orientation.Vertical;
                    this.btnCooperate.imgContent.Width = 22.0;
                    this.btnCooperate.imgContent.Height = 22.0;
                    this.btnCooperate.Icon = this.imageService.GetOA(ImageTypeOA.Cooperate);
                };
                this.btnCooperate.Unloaded += delegate(object sender, RoutedEventArgs e)
                {
                    this.imageService.RemoveOA(ImageTypeOA.Cooperate);
                };
                this.btnPlan.Loaded += delegate(object sender, RoutedEventArgs e)
                {
                    this.btnPlan.spnlContent.Orientation = Orientation.Vertical;
                    this.btnPlan.imgContent.Width = 22.0;
                    this.btnPlan.imgContent.Height = 22.0;
                    this.btnPlan.Icon = this.imageService.GetOA(ImageTypeOA.Plan);
                };
                this.btnPlan.Unloaded += delegate(object sender, RoutedEventArgs e)
                {
                    this.imageService.RemoveOA(ImageTypeOA.Cooperate);
                };
                this.btnGis.Loaded += delegate(object sender, RoutedEventArgs e)
                {
                    this.btnGis.spnlContent.Orientation = Orientation.Vertical;
                    this.btnGis.imgContent.Width = 22.0;
                    this.btnGis.imgContent.Height = 22.0;
                    this.btnGis.Icon = this.imageService.GetOA(ImageTypeOA.Document);
                };
                this.btnGis.Unloaded += delegate(object sender, RoutedEventArgs e)
                {
                    this.imageService.RemoveOA(ImageTypeOA.Document);
                };
                this.btnForum.Loaded += delegate(object sender, RoutedEventArgs e)
                {
                    this.btnForum.spnlContent.Orientation = Orientation.Vertical;
                    this.btnForum.imgContent.Width = 22.0;
                    this.btnForum.imgContent.Height = 22.0;
                    this.btnForum.Icon = this.imageService.GetOA(ImageTypeOA.Forum);
                };
                this.btnForum.Unloaded += delegate(object sender, RoutedEventArgs e)
                {
                    this.imageService.RemoveOA(ImageTypeOA.Forum);
                };
                this.btnGallery.Loaded += delegate(object sender, RoutedEventArgs e)
                {
                    this.btnGallery.spnlContent.Orientation = Orientation.Vertical;
                    this.btnGallery.imgContent.Width = 22.0;
                    this.btnGallery.imgContent.Height = 22.0;
                    this.btnGallery.Icon = this.imageService.GetOA(ImageTypeOA.Gallery);
                };
                this.btnGallery.Unloaded += delegate(object sender, RoutedEventArgs e)
                {
                    this.imageService.RemoveOA(ImageTypeOA.Gallery);
                };
                base.StateChanged += new System.EventHandler(this.INWindow_StateChanged);
            }
            catch (System.Exception ex)
            {
                this.logger.Error(ex.ToString());
            }
        }
        private void INWindow_StateChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (base.WindowState != WindowState.Minimized)
                {
                    base.ShowInTaskbar = true;
                    this.sessionService.ChatViewMsgBoxWidth = base.Width - 400.0;
                }
                else
                {
                    base.ShowInTaskbar = false;
                }
            }
            catch (System.Exception ex)
            {
                this.logger.Error(ex.ToString());
            }
        }
        private void StatusMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MenuItem mi = sender as MenuItem;
                Image image = mi.Icon as Image;
                UserStatus status = (UserStatus)System.Enum.Parse(typeof(UserStatus), mi.DataContext.ToString());
                if (this.sessionService.Status != status)
                {
                    if (image != null)
                    {
                        this.btnStatus.IconContent.Source = image.Source;
                    }
                    Settings.Default.Status = (int)status;
                    IDKin.IM.Core.Staff staff = this.dataService.GetStaff(this.sessionService.Uid);
                    if (staff == null)
                    {
                        staff = this.CreateSelfStaff(status);
                    }
                    staff.Status = status;
                    this.tbkStatus.Text = this.GetStatusLabel(status);
                    Settings.Default.Save();
                    this.imageService.SetStatusIcon(staff);
                    this.imageService.SetHeaderImage(staff);
                    this.userHead.ImageSource = staff.HeaderImage;
                    this.sessionService.HeaderImage = staff.HeaderImage;
                    this.Employee.DepartmentTree.UpdateStaff(staff);
                    UserStatus preUserStatus = this.sessionService.Status;
                    this.inViewModel.ChangeStatus(staff);
                    this.sessionService.Status = status;
                    if (preUserStatus == UserStatus.Offline && status != UserStatus.Offline)
                    {
                        this.NotOffLineProcessor();
                    }
                    if (preUserStatus != UserStatus.Offline && status == UserStatus.Offline)
                    {
                        this.OffLineProcessor();
                    }
                }
            }
            catch (System.Exception ex)
            {
                if (this.logger != null)
                {
                    this.logger.Error(ex);
                }
            }
        }
        private void NotOffLineProcessor()
        {
            this.ClearUIData();
            this.ClearServiceData();
            this.InitData();
            this.ShowNotifyIcon();
            this.userHead.ImageSource = this.sessionService.HeaderImage;
            this.UpdataGroupAllMemberTimer();
        }
        private void ClearServiceData()
        {
            this.dataService.ClearORGData();
            DataModel.Instance.ClearAll();
            WindowModel.Instance.ClearAll();
        }
        private void ClearUIData()
        {
            this.Employee.RecentLinkListItem.Items.Clear();
            this.Employee.RecentLinkListItem.StaffNode.Clear();
            this.Employee.DepartmentTree.Clear();
            this.Employee.SelfDepartmentItem.Clear();
            this.FriendsList.Items.Clear();
            this.Employee.listEntGroup.Items.Clear();
            this.Employee.ClearCustomGroup();
            this.Collaboration.Clear();
        }
        private void OffLineProcessor()
        {
            System.Collections.Generic.ICollection<IDKin.IM.Core.Staff> staffs = this.dataService.GetStaffList();
            if (staffs != null)
            {
                foreach (IDKin.IM.Core.Staff staff in staffs)
                {
                    if (staff != null)
                    {
                        staff.Status = UserStatus.Offline;
                        this.imageService.SetStatusIcon(staff);
                        this.imageService.SetHeaderImage(staff);
                        this.imageService.SetHeaderImageOnline(staff);
                        this.Employee.DepartmentTree.UpdateStaff(staff);
                        this.Employee.SelfDepartmentItem.UpdateStaff(staff);
                        this.Employee.UpdateStaff(staff);
                        this.Employee.UpdateDepartmentStaffCount(0, this.dataService.GetEntStaffTotalCount());
                        this.EntGroupMemberUpdate(staff);
                    }
                }
                if (DataModel.Instance.EmailList != null && DataModel.Instance.EmailList.Count > 0)
                {
                    DataModel.Instance.EmailList.Clear();
                }
            }
            IDKin.IM.Core.Staff selfStaff = this.dataService.GetStaff(this.sessionService.Uid);
            this.sessionService.HeaderImage = selfStaff.HeaderImage;
            this.userHead.ImageSource = this.sessionService.HeaderImage;
            this.ShowNotifyIconGray();
            System.Collections.Generic.ICollection<CooperationStaff> cooperationStaff = this.dataService.GetCooperationStafffList();
            foreach (CooperationStaff item in cooperationStaff)
            {
                item.Status = UserStatus.Offline;
            }
        }
        public void InitData()
        {
            try
            {
                this.InitVM();
                this.GetEntGroupList();
                this.GetDepartmentList();
                this.InitService();
                this.inViewModel.SendKeepLive();
                this.ShowNotifyIcon();
                this.btnStatus.IconContent.Source = this.GetStatusIcon();
                this.tbkStatus.Text = this.GetStatusLabel(this.sessionService.Status);
                this.tbkName.Text = this.sessionService.Name;
                this.txtSignature.Text = this.sessionService.Signature;
                IDKin.IM.Core.Staff staff = this.CreateSelfStaff(this.sessionService.Status);
                this.imageService.SetHeaderImage(staff);
                this.userHead.ImageSource = staff.HeaderImage;
                this.sessionService.HeaderImage = staff.HeaderImage;
                this.GroupActorProcessor();
                this.SearchPopupList();
                this.GetOfflineMessage();
                this.GetAppUpdate();
                this.tbkInVersion.Text = " " + AppUtil.Instance.AppVersion();
                this.sessionService.SaveAsFilePath = Settings.Default.SystemSetup_FileTransport_SaveDir;
                this.sessionService.IsCutScreenHidenWindow = Settings.Default.IsCutScreenHidenWindow;
                this.sessionService.ChatViewMsgBoxWidth = base.Width - 400.0;
            }
            catch (System.Exception ex)
            {
                this.logger.Error(ex.ToString());
            }
        }
        private void GetCustomGroup()
        {
            this.inViewModel.GetCustomGroup();
        }
        private IDKin.IM.Core.Staff CreateSelfStaff(UserStatus status)
        {
            return new IDKin.IM.Core.Staff
            {
                HeaderFileName = this.sessionService.HeaderFileName,
                Sex = this.sessionService.Sex,
                Status = status,
                Uid = this.sessionService.Uid
            };
        }
        private void GetDepartmentList()
        {
            this.inViewModel.GetDepartmentList();
        }
        private void ShowNotifyIconGray()
        {
            SystemWindow sysWindow = this.dataService.SystemWindow as SystemWindow;
            if (sysWindow != null)
            {
                sysWindow.NotifyIcon.Icon = IDKin.IM.Windows.Properties.Resources.notifyIcon_gray;
            }
        }
        private void ShowNotifyIcon()
        {
            SystemWindow sysWindow = this.dataService.SystemWindow as SystemWindow;
            if (sysWindow != null)
            {
                sysWindow.NotifyIcon.Icon = IDKin.IM.Windows.Properties.Resources.notifyIcon;
            }
        }
        private void GetEntGroupList()
        {
            this.inViewModel.GetGroupList();
        }
        private void GetAppUpdate()
        {
            try
            {
                this.getAppUpdateTimer = new Timer(5000.0);
                this.getAppUpdateTimer.Elapsed += new ElapsedEventHandler(this.GetAppUpdateTimeHanlder);
                this.getAppUpdateTimer.AutoReset = true;
                this.getAppUpdateTimer.Enabled = true;
            }
            catch (System.Exception ex)
            {
                this.logger.Error(ex.ToString());
            }
        }
        private void GetAppUpdateTimeHanlder(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (this.getAppUpdateTimer != null)
                {
                    this.getAppUpdateTimer.BeginInit();
                    this.getAppUpdateTimer.Stop();
                    this.getAppUpdateTimer.Dispose();
                    this.getAppUpdateTimer.Close();
                    this.inViewModel.GetAppUpdate(this.sessionService.Jid, AppUtil.Instance.AppUpdateVerson());
                    this.GetCustomGroup();
                    CooperationProjectViewModel.GetInstance().SendCooperationProjectRequest();
                    this.getAppUpdateTimer.EndInit();
                    this.getAppUpdateTimer = null;
                }
            }
            catch (System.Exception ex)
            {
                this.logger.Error(ex.ToString());
            }
        }
        private void GetOfflineMessage()
        {
            try
            {
                this.getOfflineMessageTimer = new Timer(2000.0);
                this.getOfflineMessageTimer.Elapsed += new ElapsedEventHandler(this.GetOfflineMessageHandler);
                this.getOfflineMessageTimer.AutoReset = true;
                this.getOfflineMessageTimer.Enabled = true;
            }
            catch (System.Exception ex)
            {
                this.logger.Error(ex.ToString());
            }
        }
        private void GetOfflineMessageHandler(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (this.getOfflineMessageTimer != null)
                {
                    this.getOfflineMessageTimer.BeginInit();
                    this.getOfflineMessageTimer.Stop();
                    this.getOfflineMessageTimer.Dispose();
                    this.getOfflineMessageTimer.Close();
                    this.GetOfflineMessageHandle();
                    this.GetRosterListHandle();
                    this.GetOADataHandle();
                    this.getOfflineMessageTimer.EndInit();
                    this.getOfflineMessageTimer = null;
                }
            }
            catch (System.Exception ex)
            {
                this.logger.Error(ex.ToString());
            }
        }
        private void GetOfflineMessageHandle()
        {
            this.inViewModel.GetOfflineMessage(this.sessionService.Jid);
        }
        private void GetRosterListHandle()
        {
            this.inViewModel.GetRosterList();
        }
        private void GetOADataHandle()
        {
            this.inViewModel.GetOAAllData();
        }
        private void SearchPopupList()
        {
            this.popupList = new PopupList();
            this.popupList.MinWidth = 200.0;
            this.tbxSearch.TextChanged += new TextChangedEventHandler(this.tbxSearch_TextChanged);
            this.popupList.lbSuggestion.SelectionChanged += new SelectionChangedEventHandler(this.lbSuggestion_SelectionChanged);
        }
        private void lbSuggestion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.popupList.lbSuggestion.ItemsSource != null)
                {
                    this.popupList.IsOpen = false;
                    this.popupList.lbSuggestion.Visibility = Visibility.Collapsed;
                    this.tbxSearch.TextChanged -= new TextChangedEventHandler(this.tbxSearch_TextChanged);
                    if (this.popupList.lbSuggestion.SelectedIndex != -1)
                    {
                        this.popupList.IsOpen = false;
                        this.popupList.lbSuggestion.Visibility = Visibility.Collapsed;
                    }
                    this.tbxSearch.TextChanged += new TextChangedEventHandler(this.tbxSearch_TextChanged);
                }
            }
            catch (System.Exception ex)
            {
                this.logger.Error(ex.ToString());
            }
        }
        private void GroupActorProcessor()
        {
            if (this.sessionService.Actor != 9)
            {
                this.Employee.listEntGroup.ContextMenu = null;
                this.Employee.btnAddGroup.IsEnabled = false;
                this.Employee.treEntGroupContextMenu = null;
            }
        }
        public void InitSelfDepartment()
        {
            try
            {
                this.Employee.SelfDepartmentItem.Header = this.sessionService.DepartmentName;
                this.Employee.SelfDepartmentItem.Visibility = Visibility.Visible;
            }
            catch (System.Exception ex)
            {
                this.logger.Error(ex.ToString());
            }
        }
        private ImageSource GetStatusIcon()
        {
            return this.imageService.GetStatusIcon(this.sessionService.Status);
        }
        private string GetStatusLabel(UserStatus status)
        {
            string statusLabel = UserStatusLabel.GetUserStatusLabel((int)status);
            return "[" + statusLabel + "]";
        }
        private void InitService()
        {
            try
            {
                this.Employee.DepartmentTree.SessionService = this.sessionService;
                this.Employee.DepartmentTree.DataService = this.dataService;
                this.AddEventListenerHandler();
                this.inViewModel = new INViewModel();
                this.inViewModel.InitService();
            }
            catch (System.Exception ex)
            {
                this.logger.Error(ex.ToString());
            }
        }
        private void InitVM()
        {
        }
        private void StaffNoticeEvent(StaffNoticeResponse response)
        {
            if (response != null)
            {
                IDKin.IM.Core.Staff staff = this.dataService.GetStaff((long)((ulong)Jid.GetUid(response.jid)));
                if (staff != null)
                {
                    staff.Status = (UserStatus)System.Enum.Parse(typeof(UserStatus), response.status.ToString());
                    staff.Signature = response.signature;
                    staff.Jid = response.jid;
                    staff.DepartmentId = (long)((ulong)((uint)response.department_id));
                    staff.HeaderFileName = response.img;
                    staff.Name = response.name;
                    if (string.IsNullOrEmpty(staff.Name))
                    {
                        this.logger.Debug("员工状态更新时，员工名字为空，请及时通知IDKin团队 " + staff.Uid);
                    }
                    this.fileService.DownloadHeader(staff);
                    this.StaffInfoUpdate(staff.Uid);
                    this.Employee.UpdateDepartmentStaffCount(this.dataService.GetEntStaffOnlineCount(), this.dataService.GetEntStaffTotalCount());
                }
            }
        }
        private void StaffListEventHandle(System.Collections.ObjectModel.Collection<IDKin.IM.Protocol.Enterprise.Staff> list)
        {
            if (list != null && list.Count > 0)
            {
                IDKin.IM.Core.Staff staff = null;
                foreach (IDKin.IM.Protocol.Enterprise.Staff s in list)
                {
                    staff = new IDKin.IM.Core.Staff();
                    staff.DepartmentId = s.department_id;
                    staff.Jid = s.jid;
                    staff.Name = s.name;
                    if (string.IsNullOrEmpty(staff.Name))
                    {
                        this.logger.Debug("获取用户列表时，员工名字为空，请及时通知IDKin团队 " + staff.Uid);
                    }
                    staff.Signature = s.signature;
                    staff.Status = (UserStatus)System.Enum.Parse(typeof(UserStatus), s.status.ToString());
                    staff.Uid = s.uid;
                    staff.UserName = s.username;
                    staff.HeaderFileName = s.img;
                    staff.Sex = (Sex)System.Enum.Parse(typeof(Sex), s.sex.ToString());
                    staff.Telephone = s.telephone;
                    staff.Mobile = s.mobile;
                    staff.Email = s.email;
                    staff.Extension = s.extension;
                    staff.ShowScope = s.showScope;
                    staff.InfoUpdate = new StaffInfoUpdate(this.StaffInfoUpdate);
                    this.imageService.SetHeaderImage(staff);
                    this.imageService.SetStatusIcon(staff);
                    this.imageService.SetHeaderImageOnline(staff);
                    if (this.dataService.GetDepartment(staff.DepartmentId) != null)
                    {
                        this.dataService.AddStaff(staff);
                    }
                    this.Employee.DepartmentTree.AddStaff(staff);
                    this.fileService.DownloadHeader(staff);
                    if (staff.DepartmentId == this.sessionService.DepartmentId)
                    {
                        this.Employee.SelfDepartmentItem.AddStaff(staff);
                        this.AddSelfDepartmentJobsLogo(staff);
                    }
                }
                this.Employee.DepartmentTree.SortAllDepartment();
                this.Employee.SelfDepartmentItem.SortAllDepartment();
                this.Employee.UpdateDepartmentStaffCount(this.dataService.GetEntStaffOnlineCount(), this.dataService.GetEntStaffTotalCount());
                if (staff != null)
                {
                    this.AddJobsLogo(staff.DepartmentId);
                }
            }
        }
        private void AddSelfDepartmentJobsLogo(IDKin.IM.Core.Staff staff)
        {
            IDKin.IM.Core.Department department = this.dataService.GetDepartment(staff.DepartmentId);
            if (staff.Uid == department.Uid && this.Employee.SelfDepartmentItem.StaffNode.ContainsKey(staff.Uid))
            {
                SelfDepartmentStaffNode node = this.Employee.SelfDepartmentItem.StaffNode[staff.Uid];
                if (node != null)
                {
                    node.ShowJodLogo();
                    node.ShowJobName();
                }
            }
        }
        private void AddJobsLogo(long departmentId)
        {
            IDKin.IM.Core.Department department = this.dataService.GetDepartment(departmentId);
            if (department != null)
            {
                IDKin.IM.Core.Staff staff = this.dataService.GetStaff(department.Uid);
                if (staff != null && this.Employee.DepartmentTree.StaffNode.ContainsKey(staff.Uid))
                {
                    TreeNodeStaff node = this.Employee.DepartmentTree.StaffNode[staff.Uid];
                    if (node != null)
                    {
                        node.ShowJodLogo();
                        node.ShowJobName();
                    }
                }
            }
        }
        private bool IsDepartmentStaffAllReceived()
        {
            this.DepartmentStaffAllReceivedCount++;
            return this.DepartmentStaffAllReceivedCount >= this.dataService.GetDepartmentList().Count;
        }
        private void StaffInfoUpdate(long uid)
        {
            IDKin.IM.Core.Staff staff = this.dataService.GetStaff(uid);
            if (staff != null)
            {
                this.imageService.SetStatusIcon(staff);
                this.imageService.SetHeaderImage(staff);
                this.imageService.SetHeaderImageOnline(staff);
                if (this.sessionService.Uid == staff.Uid)
                {
                    this.userHead.ImageSource = staff.HeaderImage;
                }
                this.Employee.DepartmentTree.UpdateStaff(staff);
                this.Employee.RecentLinkListItem.UpdateStaffStaus(staff);
                EntStaffTab staffTab = this.FindEntStaffTab(uid);
                if (staffTab != null)
                {
                    staffTab.UpdateInfo();
                }
                PersonalChatTabControl chatTab = this.FindStaffChatTab(uid);
                if (chatTab != null)
                {
                    chatTab.UpdateInfo();
                }
                this.Employee.SelfDepartmentItem.UpdateStaff(staff);
                this.EntGroupMemberUpdate(staff);
                this.UpdateCustomGroup(staff);
            }
        }
        private void UpdateCustomGroup(IDKin.IM.Core.Staff staff)
        {
            if (this.Employee.FirstTreeView != null && this.Employee.FirstTreeView.Items.Count != 0)
            {
                foreach (TreeViewItem item in (System.Collections.IEnumerable)this.Employee.FirstTreeView.Items)
                {
                    CustomGroupTreeViewItem customGroupTreeViewItem = item as CustomGroupTreeViewItem;
                    if (customGroupTreeViewItem != null)
                    {
                        if (customGroupTreeViewItem.Items.Count > 0)
                        {
                            this.SortTheStaff(staff, customGroupTreeViewItem);
                        }
                        if (customGroupTreeViewItem.addMemberCustomGroupWindow != null)
                        {
                            customGroupTreeViewItem.addMemberCustomGroupWindow.UpdateMember(staff);
                        }
                    }
                }
            }
        }
        private void SortTheStaff(IDKin.IM.Core.Staff staff, CustomGroupTreeViewItem customGroupTreeViewItem)
        {
            for (int i = 0; i < customGroupTreeViewItem.Items.Count; i++)
            {
                TreeNodeCustomStaff cItem = customGroupTreeViewItem.Items[i] as TreeNodeCustomStaff;
                if (cItem.Staff.Uid == staff.Uid)
                {
                    cItem.UpdateInfo();
                    System.Collections.Generic.List<TreeNodeCustomStaff> lists = new System.Collections.Generic.List<TreeNodeCustomStaff>(customGroupTreeViewItem.Items.Count);
                    for (int j = 0; j < customGroupTreeViewItem.Items.Count; j++)
                    {
                        TreeNodeCustomStaff childItem = customGroupTreeViewItem.Items[j] as TreeNodeCustomStaff;
                        lists.Add(childItem);
                    }
                    customGroupTreeViewItem.Items.Clear();
                    IOrderedEnumerable<TreeNodeCustomStaff> v =
                        from t in lists
                        orderby t.Staff.Status == UserStatus.Offline, t.Status == UserStatus.Hide, t.Status == UserStatus.Out, t.Status == UserStatus.DoNotDisturb, t.Status == UserStatus.Busy, t.Status == UserStatus.Meeting, t.Status == UserStatus.Away, t.Status == UserStatus.Online
                        select t;
                    foreach (TreeNodeCustomStaff treeNodeCustomStaff in v)
                    {
                        customGroupTreeViewItem.Items.Add(treeNodeCustomStaff);
                    }
                    break;
                }
            }
        }
        private void UpdataGroupAllMemberTimer()
        {
            this.updateGroupAllMemberTimer = new Timer(5000.0);
            this.updateGroupAllMemberTimer.Elapsed += new ElapsedEventHandler(this.UpdataGroupAllMemberTimerHandle);
            this.updateGroupAllMemberTimer.AutoReset = true;
            this.updateGroupAllMemberTimer.Enabled = true;
        }
        private void UpdataGroupAllMemberTimerHandle(object sender, ElapsedEventArgs e)
        {
            if (this.updateGroupAllMemberTimer != null)
            {
                this.updateGroupAllMemberTimer.Stop();
                this.updateGroupAllMemberTimer.Dispose();
                this.updateGroupAllMemberTimer.Close();
                this.updateGroupAllMemberTimer = null;
            }
            base.Dispatcher.BeginInvoke(new INWindow.UpdateGroupAllMemberDelegate(this.EntGroupChatTabAllMemberUpdate), new object[0]);
        }
        private void EntGroupChatTabAllMemberUpdate()
        {
            foreach (TabItem item in this.dataService.GetEntGroupChatTabs().Values)
            {
                CloseableTabItem closeItem = item as CloseableTabItem;
                if (closeItem != null)
                {
                    GroupChatTabControl tab = closeItem.Content as GroupChatTabControl;
                    if (tab != null)
                    {
                        tab.UpdateGroupAllMember();
                    }
                }
            }
        }
        private void EntGroupMemberUpdate(IDKin.IM.Core.Staff staff)
        {
            foreach (TabItem item in this.dataService.GetEntGroupChatTabs().Values)
            {
                CloseableTabItem closeItem = item as CloseableTabItem;
                if (closeItem != null)
                {
                    GroupChatTabControl tab = closeItem.Content as GroupChatTabControl;
                    if (tab != null)
                    {
                        tab.UpdateGroupMemberListHandler(staff);
                    }
                }
            }
        }
        private PersonalChatTabControl FindStaffChatTab(long uid)
        {
            ItemCollection ic = this.ContentTab.Items;
            PersonalChatTabControl result;
            foreach (TabItem item in (System.Collections.IEnumerable)ic)
            {
                if (item != null)
                {
                    PersonalChatTabControl pctc = item.Content as PersonalChatTabControl;
                    if (pctc != null && pctc.StaffId == uid)
                    {
                        result = pctc;
                        return result;
                    }
                }
            }
            result = null;
            return result;
        }
        private EntStaffTab FindEntStaffTab(long uid)
        {
            ItemCollection ic = this.ContentTab.Items;
            EntStaffTab result;
            foreach (TabItem item in (System.Collections.IEnumerable)ic)
            {
                if (item != null)
                {
                    EntStaffTab staffTab = item as EntStaffTab;
                    if (staffTab != null)
                    {
                        PersonalChatTabControl pctc = staffTab.TabContent;
                        if (pctc != null && pctc.StaffId == uid)
                        {
                            result = staffTab;
                            return result;
                        }
                    }
                }
            }
            result = null;
            return result;
        }
        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            base.WindowState = WindowState.Minimized;
        }
        private void btnMax_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.WindowMaxHandler();
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
                if (Settings.Default.ExitRemind)
                {
                    if (Settings.Default.SystemSetup_Base_ExitHide)
                    {
                        base.WindowState = WindowState.Minimized;
                    }
                    else
                    {
                        Application.Current.Shutdown(1001);
                    }
                }
                else
                {
                    ClosingTipWindow closingTip = WindowModel.Instance.ClosingTipWindow;
                    closingTip.Owner = this;
                    closingTip.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    if (closingTip.ShowDialog().Value)
                    {
                    }
                }
            }
            catch (System.Exception ex)
            {
                this.logger.Error(ex.ToString());
            }
        }
        private void topBar_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.WindowMaxHandler();
            }
            catch (System.Exception ex)
            {
                this.logger.Error(ex.ToString());
            }
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
                if (base.Top <= 0.0 || base.Left <= 5.0 || base.Left + base.Width >= SystemParameters.WorkArea.Width)
                {
                    base.Left = (SystemParameters.WorkArea.Width - base.Width) / 2.0;
                    base.Top = (SystemParameters.WorkArea.Height - base.Height) / 2.0;
                }
                this.btnMax.NormalImage = "/IDKin.IM.Windows;component/Resources/Image/max.png";
                this.btnMax.HoverImage = "/IDKin.IM.Windows;component/Resources/Image/maxOver.png";
                this.btnMax.PressedImage = "/IDKin.IM.Windows;component/Resources/Image/maxPress.png";
            }
        }
        private void AddFriendHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                WindowModel.Instance.SearchWindow.Show();
            }
            catch (System.Exception ex)
            {
                this.logger.Error(ex.ToString());
            }
        }
        private void lblSignature_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    this.lblSignature.Visibility = Visibility.Collapsed;
                    this.txtSignature.Text = this.lblSignature.Text;
                    this.txtSignature.Visibility = Visibility.Visible;
                    this.txtSignature.IsEnabled = true;
                    this.txtSignature.Focus();
                    this.txtSignature.SelectAll();
                    base.MouseDown += new MouseButtonEventHandler(this.BaseWindow_MouseDown);
                }
            }
            catch (System.Exception ex)
            {
                this.logger.Error(ex.ToString());
            }
        }
        private void txtSignature_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return)
                {
                    this.SetSignature();
                }
            }
            catch (System.Exception ex)
            {
                this.logger.Error(ex.ToString());
            }
        }
        private void txtSignature_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                this.SetSignature();
                this.txtSignature.IsEnabled = false;
                base.MouseDown -= new MouseButtonEventHandler(this.BaseWindow_MouseDown);
                base.MouseDown -= new MouseButtonEventHandler(this.BaseWindow_MouseDown);
                base.MouseDown -= new MouseButtonEventHandler(this.BaseWindow_MouseDown);
            }
            catch (System.Exception ex)
            {
                this.logger.Error(ex.ToString());
            }
        }
        private void BaseWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.btnApproval.Focus();
            }
            catch (System.Exception ex)
            {
                this.logger.Error(ex.ToString());
            }
        }
        private void SetSignature()
        {
            if (this.sessionService.Signature.Equals(this.txtSignature.Text))
            {
                this.txtSignature.Visibility = Visibility.Hidden;
                this.lblSignature.Visibility = Visibility.Visible;
            }
            else
            {
                if (string.IsNullOrEmpty(this.txtSignature.Text))
                {
                    this.txtSignature.Text = "输入个性签名";
                }
                string txt = this.txtSignature.Text;
                this.lblSignature.Text = txt;
                this.lblSignature.ToolTip = this.lblSignature.Text;
                this.txtSignature.Visibility = Visibility.Hidden;
                this.lblSignature.Visibility = Visibility.Visible;
                this.sessionService.Signature = this.txtSignature.Text;
                IDKin.IM.Core.Staff staff = this.dataService.GetStaff(this.sessionService.Uid);
                if (staff != null)
                {
                    staff.Signature = this.sessionService.Signature;
                    this.inViewModel.ChangeStatus(staff);
                }
            }
        }
        private void GotoItem(object sender, RoutedEventArgs e)
        {
            try
            {
                BrowserUtil.OpenBrowserHandler(this.item);
            }
            catch (System.Exception ex)
            {
                this.logger.Error(ex.ToString());
            }
        }
        private void GotoCooperate_with(object sender, RoutedEventArgs e)
        {
            BrowserUtil.OpenBrowserHandler(this.cooperate_with);
        }
        private void GotoPlan(object sender, RoutedEventArgs e)
        {
            BrowserUtil.OpenBrowserHandler(this.plan);
        }
        private void GotoGis(object sender, RoutedEventArgs e)
        {
            BrowserUtil.OpenBrowserHandler(this.gis);
        }
        private void GotoForum(object sender, RoutedEventArgs e)
        {
            BrowserUtil.OpenBrowserHandler(this.forum);
        }
        private void GotoApproval(object sender, RoutedEventArgs e)
        {
            BrowserUtil.OpenBrowserHandler(this.approval);
        }
        private void GotoGallery(object sender, RoutedEventArgs e)
        {
            Alert.Show("专业图库正在测试中，您可以通过访问：\nhttp://tuku.idkin.com参与测试，感谢\n您的支持！", "提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }
        private void btnMenu_Initialized(object sender, System.EventArgs e)
        {
            base.ContextMenu = null;
        }
        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            this.mainMenu.HasDropShadow = true;
            this.mainMenu.PlacementTarget = this.btnMenu;
            this.mainMenu.Placement = PlacementMode.Bottom;
            this.mainMenu.IsOpen = true;
        }
        private void ChangeUser_Click(object sender, RoutedEventArgs e)
        {
            this.ChangeUserHandler();
        }
        public void ChangeUserHandler()
        {
            try
            {
                this.RELogOn("");
            }
            catch (System.Exception e)
            {
                this.logger.Error(e.ToString());
            }
        }
        public void RELogOn(string message)
        {
            try
            {
                if (this.sessionService.IsLogin)
                {
                    this.CloseWindows();
                    if (DataModel.Instance.EmailList != null && DataModel.Instance.EmailList.Count > 0)
                    {
                        DataModel.Instance.EmailList.Clear();
                    }
                    this.isTaskClose = false;
                    this.sessionService.IsReLogOn = true;
                    this.sessionService.IsLogin = false;
                    this.sessionService.IsEnable = false;
                    this.sessionService.Ticket = string.Empty;
                    this.sessionService.Uid = 0L;
                    this.dataService.ClearAll();
                    DataModel.Instance.ClearAll();
                    WindowModel.Instance.ClearAll();
                    this.ClearAll();
                    base.Close();
                    NotifyIconUtil.Instance.SetFlashIcon(FlashIconType.Default);
                    LogonWindow logonWindow = new LogonWindow();
                    this.dataService.LoginWindow = logonWindow;
                    logonWindow.Show();
                    this.ShowNotifyIconGray();
                    this.connection.EventHandler.SocketErrorEvent = null;
                    logonWindow.AddMessage(message);
                    this.inViewModel.LogOff();
                    this.inViewModel.Disconnect();
                    SystemWindow sysWindow = this.dataService.SystemWindow as SystemWindow;
                    sysWindow.optionItem.Enabled = this.sessionService.IsEnable;
                    sysWindow.aboutItem.Enabled = this.sessionService.IsEnable;
                    sysWindow.logoutItem.Enabled = this.sessionService.IsEnable;
                }
            }
            catch (System.Exception ex)
            {
                if (this.logger != null)
                {
                    this.logger.Error(ex.ToString());
                }
            }
        }
        private void CloseWindows()
        {
            MessageBoxWindow mbw = DataModel.Instance.GetMessageBox();
            if (mbw != null)
            {
                mbw.Close();
            }
        }
        private void ClearAll()
        {
            if (this.userHead != null)
            {
                this.userHead.ImageSource = null;
            }
            if (this.ContentTab != null)
            {
                this.ContentTab.Items.Clear();
            }
            if (this.WorkTabControl != null)
            {
                this.WorkTabControl.Items.Clear();
            }
            this.Collaboration.Clear();
        }
        private void SystemSettingHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                SystemSettingWindow systemSetting = WindowModel.Instance.SystemSettingWindow;
                systemSetting.Show();
                systemSetting.Activate();
            }
            catch (System.Exception ex)
            {
                this.logger.Error(ex.ToString());
            }
        }
        private void CheckUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UpdateWindow updateWindow = WindowModel.Instance.UpdateWindow;
                updateWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                updateWindow.ShowDialog();
            }
            catch (System.Exception ex)
            {
                this.logger.Error(ex.ToString());
            }
        }
        private void OnAboutClick(object sender, RoutedEventArgs e)
        {
            try
            {
                AboutWindow about = WindowModel.Instance.AboutWindow;
                about.Owner = this;
                about.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                DoubleAnimation animate = new DoubleAnimation();
                animate.FillBehavior = FillBehavior.HoldEnd;
                animate.From = new double?(0.0);
                animate.To = new double?(0.5);
                animate.Duration = new Duration(System.TimeSpan.FromSeconds(0.8));
                this.RootBorder.Visibility = Visibility.Visible;
                this.RootBorder.Background.BeginAnimation(Brush.OpacityProperty, animate);
                about.ShowDialog();
                this.RootBorder.Visibility = Visibility.Collapsed;
            }
            catch (System.Exception ex)
            {
                this.logger.Error(ex.ToString());
            }
        }
        private void OnExitClick(object sender, RoutedEventArgs e)
        {
            try
            {
                this.inViewModel.LogOff();
                Application.Current.Shutdown(1002);
            }
            catch (System.Exception ex)
            {
                this.logger.Error(ex.ToString());
            }
        }
        private void tbxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                this.SearchRequirement();
            }
            catch (System.Exception ex)
            {
                this.logger.Error(ex.ToString());
            }
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.SearchRequirement();
            }
            catch (System.Exception ex)
            {
                this.logger.Error(ex.ToString());
            }
        }
        private void SearchRequirement()
        {
            System.Collections.Generic.List<SearchNodeStaff> nodes = SearchUtil.Instance.SearchStaffNode(this.tbxSearch.Text);
            if (nodes != null)
            {
                this.popupList.IsOpen = true;
                this.popupList.PlacementTarget = this.tbxSearch;
                this.popupList.Placement = PlacementMode.Bottom;
                this.popupList.lbSuggestion.ItemsSource = nodes;
                this.popupList.lbSuggestion.Visibility = Visibility.Visible;
            }
            else
            {
                this.popupList.IsOpen = false;
                this.popupList.lbSuggestion.Visibility = Visibility.Collapsed;
            }
        }
        private void SocketErrorEvent()
        {
            try
            {
                this.RELogOn("与服务器断开连接，5秒后自动重连...");
                LogonWindow logonWindow = this.dataService.LoginWindow as LogonWindow;
                if (logonWindow != null)
                {
                    logonWindow.LogonPanel.ReLogOnByTime();
                }
            }
            catch (System.Exception ex)
            {
                if (this.logger != null)
                {
                    this.logger.Error(ex.ToString());
                }
            }
        }
        private void BaseWindow_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                if (this.isTaskClose)
                {
                    e.Cancel = true;
                    this.InWindowExitHandler();
                }
            }
            catch (System.Exception ex)
            {
                this.logger.Error(ex.ToString());
            }
        }
        private void InWindowExitHandler()
        {
            if (Settings.Default.ExitRemind)
            {
                if (Settings.Default.SystemSetup_Base_ExitHide)
                {
                    base.WindowState = WindowState.Minimized;
                }
                else
                {
                    Application.Current.Shutdown(1001);
                }
            }
            else
            {
                ClosingTipWindow closingTip = WindowModel.Instance.ClosingTipWindow;
                closingTip.Owner = this;
                closingTip.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                closingTip.ShowDialog();
            }
        }
        private void ContentTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BaseTab baseTab = this.ContentTab.SelectedItem as BaseTab;
            if (baseTab != null)
            {
                baseTab.SetDefaultStyle();
            }
        }
        private void IconButton_Click(object sender, RoutedEventArgs e)
        {
            IconButton button = sender as IconButton;
            if (this.LeftColumn.Width == new GridLength(178.0))
            {
                this.LeftColumn.Width = new GridLength(0.0);
                this.ContentTab.Margin = new Thickness(3.0, 4.0, 4.0, 4.0);
                this.ContentTab.BorderThickness = new Thickness(0.0, 0.0, 0.0, 0.0);
                button.Icon = new BitmapImage(new Uri("/IDKin.IM.Windows;component/Resources/Image/leftShow.png", UriKind.RelativeOrAbsolute));
                button.ToolTip = "显示用户列表";
            }
            else
            {
                this.LeftColumn.Width = new GridLength(178.0);
                this.ContentTab.Margin = new Thickness(5.0, 4.0, 4.0, 4.0);
                this.ContentTab.BorderThickness = new Thickness(1.0, 0.0, 0.0, 0.0);
                button.Icon = new BitmapImage(new Uri("/IDKin.IM.Windows;component/Resources/Image/leftHide.png", UriKind.RelativeOrAbsolute));
                button.ToolTip = "隐藏用户列表";
            }
        }
        private void InitHideTimer()
        {
            this.dockTimer = new DispatcherTimer();
            this.dockTimer.IsEnabled = false;
            this.dockTimer.Interval = System.TimeSpan.FromMilliseconds(200.0);
            this.dockTimer.Tick += new System.EventHandler(this.DockTimerAutoHide_Tick);
            this.dockTimer.Start();
        }
        private void DockTimerAutoHide_Tick(object sender, System.EventArgs e)
        {
            AutoHideHelper.SideHideOrShow(this, ref this.dockWindowHeight, this.dockTimer);
        }
        private void InitDataHandler()
        {
            this.sessionService.IsAllowCut = true;
        }
        private void OpenMessageCenter(object sender, RoutedEventArgs e)
        {
            try
            {
                if (WindowModel.Instance.IsOpenMessageCenterWindow())
                {
                    if (WindowModel.Instance.MessageCenterWindow.WindowState == WindowState.Minimized)
                    {
                        WindowModel.Instance.MessageCenterWindow.WindowState = WindowState.Normal;
                    }
                    WindowModel.Instance.MessageCenterWindow.Topmost = true;
                }
                else
                {
                    WindowModel.Instance.MessageCenterWindow.Show();
                }
            }
            catch (System.Exception ex)
            {
                this.logger.Error(ex.ToString());
            }
        }
        private void BaseWindow_ContentRendered(object sender, System.EventArgs e)
        {
            if (UsageGuideWindow.ShowUsageGuideWindow)
            {
                DoubleAnimation animate = new DoubleAnimation();
                animate.FillBehavior = FillBehavior.HoldEnd;
                animate.From = new double?(0.0);
                animate.To = new double?(0.5);
                animate.Duration = new Duration(System.TimeSpan.FromSeconds(0.8));
                this.RootBorder.Visibility = Visibility.Visible;
                this.RootBorder.Background.BeginAnimation(Brush.OpacityProperty, animate);
                new UsageGuideWindow
                {
                    Owner = this
                }.ShowDialog();
                this.RootBorder.Visibility = Visibility.Collapsed;
            }
        }
        private void ViewEmailClick(object sender, RoutedEventArgs e)
        {
            ViewEmailAlertPopup emailPopup = new ViewEmailAlertPopup();
            emailPopup.PlacementTarget = (e.Source as Button);
            emailPopup.Placement = PlacementMode.Bottom;
            emailPopup.IsOpen = true;
            emailPopup.StaysOpen = false;
        }
        public void UpdateMailCount()
        {
            if (DataModel.Instance.EmailList != null && DataModel.Instance.EmailList.Count > 0)
            {
                int count = 0;
                bool hasError = false;
                foreach (EMailModal item in DataModel.Instance.EmailList)
                {
                    hasError = item.HasError;
                    count += item.NewCount;
                }
                //base.Dispatcher.BeginInvoke(delegate
                //{
                //    if (hasError)
                //    {
                //        this.imgEmail.Source = new BitmapImage(new Uri("pack://application:,,,/IDKin.IM.Windows;component/Resources/Icon/emailAccountInvalidIcon.png"));
                //    }
                //    else
                //    {
                //        this.imgEmail.Source = new BitmapImage(new Uri("pack://application:,,,/IDKin.IM.Windows;component/Resources/Icon/emailIcon.png"));
                //    }
                //    if (this.sessionService != null && this.sessionService.Status != UserStatus.Offline)
                //    {
                //        this.tblockMailCount.Text = string.Format("({0})", count);
                //    }
                //}, null);
            }
            else
            {
                //base.Dispatcher.BeginInvoke(delegate
                //{
                //    this.imgEmail.Source = new BitmapImage(new Uri("pack://application:,,,/IDKin.IM.Windows;component/Resources/Icon/emailIcon.png"));
                //    if (this.sessionService != null && this.sessionService.Status != UserStatus.Offline)
                //    {
                //        this.tblockMailCount.Text = "(0)";
                //    }
                //}, null);
            }
        }
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/view/inwindow.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[System.Diagnostics.DebuggerNonUserCode]
        //internal System.Delegate _CreateDelegate(System.Type delegateType, string handler)
        //{
        //    return System.Delegate.CreateDelegate(delegateType, this, handler);
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.OuterBorder = (Border)target;
        //        return;
        //    case 2:
        //        this.InnerBorder = (Border)target;
        //        return;
        //    case 3:
        //        this.RootGrid = (Grid)target;
        //        return;
        //    case 4:
        //        this.RootBorder = (Border)target;
        //        return;
        //    case 5:
        //        this.topBar = (StatusBar)target;
        //        this.topBar.MouseDoubleClick += new MouseButtonEventHandler(this.topBar_MouseDoubleClick);
        //        return;
        //    case 6:
        //        this.btnIsTop = (ToggleButton)target;
        //        return;
        //    case 7:
        //        this.btnMenu = (Button)target;
        //        this.btnMenu.Initialized += new System.EventHandler(this.btnMenu_Initialized);
        //        this.btnMenu.Click += new RoutedEventHandler(this.btnMenu_Click);
        //        return;
        //    case 8:
        //        this.mainMenu = (ContextMenu)target;
        //        return;
        //    case 9:
        //        ((MenuItem)target).Click += new RoutedEventHandler(this.ChangeUser_Click);
        //        return;
        //    case 10:
        //        ((MenuItem)target).Click += new RoutedEventHandler(this.SystemSettingHandler);
        //        return;
        //    case 11:
        //        ((MenuItem)target).Click += new RoutedEventHandler(this.CheckUpdate_Click);
        //        return;
        //    case 12:
        //        ((MenuItem)target).Click += new RoutedEventHandler(this.OnAboutClick);
        //        return;
        //    case 13:
        //        ((MenuItem)target).Click += new RoutedEventHandler(this.OnExitClick);
        //        return;
        //    case 14:
        //        this.btnTheme = (Button)target;
        //        return;
        //    case 15:
        //        this.btnMin = (ImageButton)target;
        //        this.btnMin.Click += new RoutedEventHandler(this.btnMin_Click);
        //        return;
        //    case 16:
        //        this.btnMax = (ImageButton)target;
        //        this.btnMax.Click += new RoutedEventHandler(this.btnMax_Click);
        //        return;
        //    case 17:
        //        this.btnClose = (ImageButton)target;
        //        this.btnClose.Click += new RoutedEventHandler(this.btnClose_Click);
        //        return;
        //    case 18:
        //        this.bdUserHead = (Border)target;
        //        return;
        //    case 19:
        //        this.userHead = (ImageBrush)target;
        //        return;
        //    case 20:
        //        this.tbkName = (TextBlock)target;
        //        return;
        //    case 21:
        //        this.btnStatus = (StatusButton)target;
        //        return;
        //    case 22:
        //        this.tbkStatus = (TextBlock)target;
        //        return;
        //    case 23:
        //        this.lblSignature = (TextBlock)target;
        //        this.lblSignature.MouseDown += new MouseButtonEventHandler(this.lblSignature_MouseDown);
        //        return;
        //    case 24:
        //        this.txtSignature = (TextBox)target;
        //        this.txtSignature.KeyDown += new KeyEventHandler(this.txtSignature_KeyDown);
        //        this.txtSignature.LostFocus += new RoutedEventHandler(this.txtSignature_LostFocus);
        //        return;
        //    case 25:
        //        this.btnApproval = (LinkButton)target;
        //        return;
        //    case 26:
        //        this.btnCooperate = (LinkButton)target;
        //        return;
        //    case 27:
        //        this.btnItem = (LinkButton)target;
        //        return;
        //    case 28:
        //        this.btnPlan = (LinkButton)target;
        //        return;
        //    case 29:
        //        this.btnGis = (LinkButton)target;
        //        return;
        //    case 30:
        //        this.btnForum = (LinkButton)target;
        //        return;
        //    case 31:
        //        this.btnGallery = (LinkButton)target;
        //        return;
        //    case 32:
        //        ((Button)target).Click += new RoutedEventHandler(this.ViewEmailClick);
        //        return;
        //    case 33:
        //        this.imgEmail = (Image)target;
        //        return;
        //    case 34:
        //        this.tblockMailCount = (TextBlock)target;
        //        return;
        //    case 35:
        //        this.tbxSearch = (PromptingTextBox)target;
        //        this.tbxSearch.TextChanged += new TextChangedEventHandler(this.tbxSearch_TextChanged);
        //        return;
        //    case 36:
        //        this.btnSearch = (IconButton)target;
        //        return;
        //    case 37:
        //        this.LeftColumn = (ColumnDefinition)target;
        //        return;
        //    case 38:
        //        this.WorkTabControl = (TabControl)target;
        //        return;
        //    case 39:
        //        this.Employee = (EmployeeTabItem)target;
        //        return;
        //    case 40:
        //        this.Collaboration = (CollaborationTabItem)target;
        //        return;
        //    case 41:
        //        this.Customer = (CustomerTabItem)target;
        //        return;
        //    case 42:
        //        this.Suppliers = (SuppliersTabItem)target;
        //        return;
        //    case 43:
        //        this.FriendsList = (FriendsList)target;
        //        return;
        //    case 45:
        //        this.ContentGrid = (Grid)target;
        //        return;
        //    case 46:
        //        this.ContentTab = (DropDownTabControl)target;
        //        this.ContentTab.SelectionChanged += new SelectionChangedEventHandler(this.ContentTab_SelectionChanged);
        //        return;
        //    case 47:
        //        this.workDesktopItem = (TabItem)target;
        //        return;
        //    case 48:
        //        this.workDesktopControl = (WorkDesktopControl)target;
        //        return;
        //    case 49:
        //        this.btnMessageCenter = (IconButton)target;
        //        return;
        //    case 50:
        //        this.tbkInLabel = (TextBlock)target;
        //        return;
        //    case 51:
        //        this.tbkInVersion = (TextBlock)target;
        //        return;
        //    case 52:
        //        this.tbkAdvertisement = (TextBlock)target;
        //        return;
        //    }
        //    this._contentLoaded = true;
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IStyleConnector.Connect(int connectionId, object target)
        //{
        //    if (connectionId == 44)
        //    {
        //        ((Button)target).Click += new RoutedEventHandler(this.AddFriendHandler);
        //    }
        //}
    }
}
