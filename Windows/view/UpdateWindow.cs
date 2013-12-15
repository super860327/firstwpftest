using IDKin.IM.Communicate;
using IDKin.IM.Core.Communicate;
using IDKin.IM.Data;
using IDKin.IM.Log;
using IDKin.IM.Protocol.Commons;
using IDKin.IM.Theme.Helper;
using IDKin.IM.Windows.Model;
using IDKin.IM.Windows.Util;
using IDKin.IM.Windows.ViewModel;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Deployment.Application;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
namespace IDKin.IM.Windows.View
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class UpdateWindow : Window//, IComponentConnector
	{
		private delegate void ChangeMsgDelgate(string msg);
		private UpdateItem updateItem;
		private ISessionService sessionService = null;
		private IFileService fileService = null;
		private ILogger logger = null;
		private bool debug = true;
        //internal UpdateWindow updateWindow;
        //internal Image imgIcon;
        //internal TextBlock lblMsg;
        //internal ProgressBar progressBar;
        //internal TextBlock lblVer;
        //internal Button btnCheckUpdate;
        //internal Button btnYes;
        //internal Button btnNo;
        //internal Button btnExit;
        //private bool _contentLoaded;
		public UpdateItem UpdateItem
		{
			get
			{
				return this.updateItem;
			}
			set
			{
				this.updateItem = value;
			}
		}
		public UpdateWindow()
		{
			ThemeSwitcher.LoadSkin(ThemeEnum.Aero, this);
			this.InitializeComponent();
			if (ApplicationDeployment.IsNetworkDeployed || this.debug)
			{
				this.lblVer.Text = "当前版本:" + AppUtil.Instance.AppVersion();
			}
			else
			{
				this.lblMsg.Text = "内部开发版禁止更新！";
				this.lblVer.Text = "当前版本：内部开发版";
				this.btnCheckUpdate.IsEnabled = false;
				this.btnCheckUpdate.Content = "禁止更新";
			}
			this.sessionService = ServiceUtil.Instance.SessionService;
			this.fileService = ServiceUtil.Instance.FileService;
			this.logger = ServiceUtil.Instance.Logger;
		}
		public void UpdateInfo(UpdateItem item)
		{
			if (item != null)
			{
				this.updateItem = item;
				if (item.Type == VersionType.VERSION_NONE)
				{
					this.lblMsg.Text = "已经是最新版本，不需要更新";
					this.btnCheckUpdate.Visibility = Visibility.Collapsed;
					this.btnExit.Visibility = Visibility.Visible;
				}
				if (item.Type == VersionType.VERSION_UPDATE)
				{
					this.lblMsg.Text = "检测到最近版本，是否需要更新";
					this.btnCheckUpdate.Visibility = Visibility.Collapsed;
					this.btnYes.Visibility = Visibility.Visible;
					this.btnNo.Visibility = Visibility.Visible;
				}
				if (item.Type == VersionType.VERSION_REFUSE)
				{
					this.lblMsg.Text = "服务器拒绝更新";
				}
			}
		}
		private void Window_Closing(object sender, CancelEventArgs e)
		{
			WindowModel.Instance.UpdateWindow = null;
		}
		private void CheckUpdate_Click(object sender, RoutedEventArgs e)
		{
			if (ApplicationDeployment.IsNetworkDeployed || this.debug)
			{
				this.btnCheckUpdate.IsEnabled = false;
				this.Update();
			}
		}
		private void updateWindow_ContentRendered(object sender, System.EventArgs e)
		{
			if (ApplicationDeployment.IsNetworkDeployed || this.debug)
			{
				this.Update();
			}
		}
		private void Update()
		{
			this.lblMsg.Text = "正在检查是否有更新...";
			try
			{
				INViewModel inViewModel = new INViewModel();
				inViewModel.InitService();
				inViewModel.GetAppUpdate(this.sessionService.Jid, AppUtil.Instance.AppUpdateVerson());
			}
			catch (System.InvalidOperationException)
			{
				this.lblMsg.Text = "更新发生错误,请重试!";
				this.btnCheckUpdate.Content = "重新检查";
				this.btnCheckUpdate.IsEnabled = true;
			}
		}
		private void btnYes_Click(object sender, RoutedEventArgs e)
		{
			this.btnYes.Visibility = Visibility.Collapsed;
			this.btnNo.Visibility = Visibility.Collapsed;
			this.btnExit.Visibility = Visibility.Visible;
			this.btnYes.IsEnabled = false;
			this.btnNo.IsEnabled = false;
			try
			{
				this.lblMsg.Text = "正在下载更新中,请稍候!";
				this.progressBar.Value = 40.0;
				this.updateItem.EndEvent = new EndEvent(this.DownloadEndHandle);
				this.fileService.DownloadIDKinUpdatePackage(this.updateItem);
				this.progressBar.Value = 60.0;
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}
		private void DownloadEndHandle(string fileName, bool isCancel)
		{
			try
			{
				this.progressBar.Value = 100.0;
				Process.Start(fileName, "/sp- /silent /norestart");
				Application.Current.Shutdown(1000);
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		private void btnNo_Click(object sender, RoutedEventArgs e)
		{
			base.Close();
		}
		private void btnExit_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				base.Close();
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/view/updatewindow.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.updateWindow = (UpdateWindow)target;
        //        this.updateWindow.Closing += new CancelEventHandler(this.Window_Closing);
        //        this.updateWindow.ContentRendered += new System.EventHandler(this.updateWindow_ContentRendered);
        //        break;
        //    case 2:
        //        this.imgIcon = (Image)target;
        //        break;
        //    case 3:
        //        this.lblMsg = (TextBlock)target;
        //        break;
        //    case 4:
        //        this.progressBar = (ProgressBar)target;
        //        break;
        //    case 5:
        //        this.lblVer = (TextBlock)target;
        //        break;
        //    case 6:
        //        this.btnCheckUpdate = (Button)target;
        //        this.btnCheckUpdate.Click += new RoutedEventHandler(this.CheckUpdate_Click);
        //        break;
        //    case 7:
        //        this.btnYes = (Button)target;
        //        this.btnYes.Click += new RoutedEventHandler(this.btnYes_Click);
        //        break;
        //    case 8:
        //        this.btnNo = (Button)target;
        //        this.btnNo.Click += new RoutedEventHandler(this.btnNo_Click);
        //        break;
        //    case 9:
        //        this.btnExit = (Button)target;
        //        this.btnExit.Click += new RoutedEventHandler(this.btnExit_Click);
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
