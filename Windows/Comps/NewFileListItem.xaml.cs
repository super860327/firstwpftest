using IDKin.IM.Communicate;
using IDKin.IM.Core;
using IDKin.IM.Data;
using IDKin.IM.Log;
using IDKin.IM.Windows.Comps.FileTransport;
using IDKin.IM.Windows.Properties;
using IDKin.IM.Windows.Util;
using IDKin.IM.Windows.ViewModel;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
namespace IDKin.IM.Windows.Comps
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public partial class NewFileListItem : ListBoxItem//, IComponentConnector
	{
		public delegate void EndEventDelegate(NewFileListItem item);
		private const string SendSuccessFileMsg = "成功发送文件";
		private const string SendFailFileMsg = "文件";
		private const string SendFailFileMsg2 = "发送失败!";
		private const string SendingFileMsg = "正在发送文件";
		private const string ReceivedFileMsg = "正在接收文件";
		private const string ReceivedFileRequestMsg = "收到【";
		private const string ReceivedFileRequestMsg2 = "】的文件请求";
		private const string ReceivedStaffFileRequestMsg = "收到文件请求";
		private const string ReceivedSuccessFileMsg = "成功接收";
		private const string ReceivedSuccessFileMsg2 = "的文件:";
		private const string ReceivedFailFileMsg = "的文件";
		private const string ReceivedFailFileMsg2 = "接收失败!";
		private const string RefuseFileMsg = "您已拒绝接收文件";
		public NewFileListItem.EndEventDelegate EndEvent;
		private NewFileListItemViewModel viewModel = new NewFileListItemViewModel();
		private IFileService fileService = ServiceUtil.Instance.FileService;
		private ISessionService sessionService = ServiceUtil.Instance.SessionService;
		private ILogger logger = ServiceUtil.Instance.Logger;
		private GroupChatTabControl groupChatTab;
		private EntGroup group;
		private PersonalChatTabControl staffChatTab;
		private CoopStaffChatTabControl coopStaffChatTab;
		private CooperationStaff coopStaff = null;
		private Staff staff;
		private string SaveDirectory;
		private System.IO.FileInfo fileInfo;
		private FileItem item;
		private string userName;
		private bool autoRename = false;
		private bool isProcessing = false;
		private string fileName;
		private string _unit = "KB";
		private string _speedUnit = "B/s";
		private long _upSize = 0L;
		//internal System.Windows.Controls.Image imgIcon;
		//internal TextBlock tbkMsg;
		//internal TextBlock tbkFilename;
		//internal System.Windows.Controls.ProgressBar progressBar;
		//internal TextBlock tbkSpeed;
		//internal TextBlock tbkSize;
		//internal System.Windows.Controls.Button btnAccept;
		//internal System.Windows.Controls.Button btnSaveAs;
		//internal System.Windows.Controls.Button btnRefuse;
		//internal System.Windows.Controls.Button btnCancel;
        ////private bool _contentLoaded;
		public System.IO.FileInfo FileInfo
		{
			get
			{
				return this.fileInfo;
			}
		}
		public bool AutoRename
		{
			get
			{
				return this.autoRename;
			}
			set
			{
				this.autoRename = value;
			}
		}
		public bool IsProcessing
		{
			get
			{
				return this.isProcessing;
			}
			set
			{
				this.isProcessing = value;
			}
		}
		public FileItem Item
		{
			get
			{
				return this.item;
			}
		}
		public string IconBase64
		{
			get;
			set;
		}
		public string FileName
		{
			get
			{
				return this.fileName;
			}
		}
		private string fileDir
		{
			get
			{
				return Settings.Default.SystemSetup_FileTransport_SaveDir;
			}
		}
		public NewFileListItem(System.IO.FileInfo fileInfo, GroupChatTabControl groupChatTab, EntGroup group)
		{
			try
			{
				this.InitializeComponent();
				if (fileInfo != null && groupChatTab != null && group != null)
				{
					this.fileInfo = fileInfo;
					this.groupChatTab = groupChatTab;
					this.group = group;
					this.item = new FileItem(fileInfo);
					this.item.FromUid = this.sessionService.Uid;
					this.item.Gid = this.group.Gid;
					this.item.FileService = this.fileService;
					this.item.ProcessEvent = new ProcessEvent(this.ProcessEventHandle);
					this.item.EndEvent = new EndEvent(this.EndEventHandle);
					this.item.ErrorEvent = new ErrorEvent(this.ErrorEventHandle);
					this.item.StopEvent = new StopEvent(this.StopEventHandle);
					this.progressBar.Maximum = (double)fileInfo.Length;
					this.imgIcon.Source = this.IconToBitmap(Icon.ExtractAssociatedIcon(fileInfo.FullName));
					this.IconBase64 = this.IconToBase64(Icon.ExtractAssociatedIcon(fileInfo.FullName));
					this.item.IconBase64 = this.IconBase64;
					this.tbkFilename.Text = fileInfo.Name.Trim();
					this.fileName = fileInfo.Name.Trim();
					this.tbkMsg.Text = "正在发送文件";
					this.tbkSize.Text = this.GetLength(fileInfo.Length);
					this.ShowCancelButton();
				}
			}
			catch (System.Exception e)
			{
				ServiceUtil.Instance.Logger.Error(e.ToString());
			}
		}
		public NewFileListItem(System.IO.FileInfo fileInfo, PersonalChatTabControl staffChatTab, Staff staff)
		{
			try
			{
				this.InitializeComponent();
				if (fileInfo != null && staffChatTab != null && staff != null)
				{
					this.fileInfo = fileInfo;
					this.staffChatTab = staffChatTab;
					this.staff = staff;
					this.item = new FileItem(fileInfo);
					this.item.FromUid = this.sessionService.Uid;
					this.item.ToJid = this.staff.Jid;
					this.item.ToUid = this.staff.Uid;
					this.item.FileService = this.fileService;
					this.item.ProcessEvent = new ProcessEvent(this.ProcessEventHandle);
					this.item.EndEvent = new EndEvent(this.EndEventHandle);
					this.item.ErrorEvent = new ErrorEvent(this.ErrorEventHandle);
					this.item.StopEvent = new StopEvent(this.StopEventHandle);
					this.progressBar.Maximum = (double)fileInfo.Length;
					this.imgIcon.Source = this.IconToBitmap(Icon.ExtractAssociatedIcon(fileInfo.FullName));
					this.IconBase64 = this.IconToBase64(Icon.ExtractAssociatedIcon(fileInfo.FullName));
					this.item.IconBase64 = this.IconBase64;
					this.tbkFilename.Text = fileInfo.Name.Trim();
					this.fileName = fileInfo.Name.Trim();
					this.tbkMsg.Text = "正在发送文件";
					this.tbkSize.Text = this.GetLength(fileInfo.Length);
					this.ShowCancelButton();
				}
			}
			catch (System.Exception e)
			{
				this.logger.Error(e.ToString());
			}
		}
		public NewFileListItem(System.IO.FileInfo fileInfo, CoopStaffChatTabControl coopStaffChatTab, CooperationStaff coopStaff)
		{
			try
			{
				this.InitializeComponent();
				if (fileInfo != null && coopStaffChatTab != null && coopStaff != null)
				{
					this.fileInfo = fileInfo;
					this.coopStaffChatTab = coopStaffChatTab;
					this.coopStaff = coopStaff;
					this.item = new FileItem(fileInfo);
					this.item.IsToCenter = true;
					this.item.Projectid = coopStaff.UnitedProjectid;
					this.item.FromUid = this.sessionService.Uid;
					this.item.ToJid = coopStaff.Jid;
					this.item.ToUid = coopStaff.Uid;
					this.item.FileService = this.fileService;
					this.item.ProcessEvent = new ProcessEvent(this.ProcessEventHandle);
					this.item.EndEvent = new EndEvent(this.EndEventHandle);
					this.item.ErrorEvent = new ErrorEvent(this.ErrorEventHandle);
					this.item.StopEvent = new StopEvent(this.StopEventHandle);
					this.progressBar.Maximum = (double)fileInfo.Length;
					this.imgIcon.Source = this.IconToBitmap(Icon.ExtractAssociatedIcon(fileInfo.FullName));
					this.IconBase64 = this.IconToBase64(Icon.ExtractAssociatedIcon(fileInfo.FullName));
					this.item.IconBase64 = this.IconBase64;
					this.tbkFilename.Text = fileInfo.Name.Trim();
					this.fileName = fileInfo.Name.Trim();
					this.tbkMsg.Text = "正在发送文件";
					this.tbkSize.Text = this.GetLength(fileInfo.Length);
					this.ShowCancelButton();
				}
			}
			catch (System.Exception e)
			{
				this.logger.Error(e.ToString());
			}
		}
		public NewFileListItem(string fileName, string id, long size, CoopStaffChatTabControl coopStaffChatTab, CooperationStaff coopStaff, string iconBase64)
		{
			if (coopStaffChatTab != null && coopStaff != null)
			{
				this.InitializeComponent();
				this.coopStaffChatTab = coopStaffChatTab;
				this.coopStaff = coopStaff;
				this.item = new FileItem(id, this.fileDir + fileName);
				this.item.FileService = this.fileService;
				this.item.ProcessEvent = new ProcessEvent(this.ProcessEventHandle);
				this.item.EndEvent = new EndEvent(this.EndEventHandle);
				this.item.ErrorEvent = new ErrorEvent(this.ErrorEventHandle);
				this.item.IconBase64 = iconBase64;
				this.IconBase64 = iconBase64;
				this.imgIcon.Source = this.IconDecode(this.IconBase64);
				this.progressBar.Maximum = (double)size;
				this.tbkFilename.Text = fileName;
				this.fileName = fileName;
				this.tbkMsg.Text = "收到文件请求";
				this.tbkSize.Text = this.GetLength(size);
				this.ShowAcceptButton();
			}
		}
		public NewFileListItem(string fileName, string id, long size, GroupChatTabControl groupChatTab, EntGroup group, string iconBase64, string userName)
		{
			if (groupChatTab != null && group != null)
			{
				this.InitializeComponent();
				this.groupChatTab = groupChatTab;
				this.group = group;
				this.item = new FileItem(id, this.fileDir + fileName);
				this.item.FileService = this.fileService;
				this.item.ProcessEvent = new ProcessEvent(this.ProcessEventHandle);
				this.item.EndEvent = new EndEvent(this.EndEventHandle);
				this.item.ErrorEvent = new ErrorEvent(this.ErrorEventHandle);
				this.item.IconBase64 = iconBase64;
				this.userName = userName;
				this.IconBase64 = iconBase64;
				this.imgIcon.Source = this.IconDecode(this.IconBase64);
				this.progressBar.Maximum = (double)size;
				this.tbkFilename.Text = fileName;
				this.fileName = fileName;
				this.tbkMsg.Text = "收到【" + this.userName + "】的文件请求";
				this.tbkSize.Text = this.GetLength(size);
				this.ShowAcceptButton();
			}
		}
		public NewFileListItem(string fileName, string id, long size, PersonalChatTabControl staffChatTab, Staff staff, string iconBase64)
		{
			if (staffChatTab != null && staff != null)
			{
				this.InitializeComponent();
				this.staffChatTab = staffChatTab;
				this.staff = staff;
				this.item = new FileItem(id, this.fileDir + fileName);
				this.item.FileService = this.fileService;
				this.item.ProcessEvent = new ProcessEvent(this.ProcessEventHandle);
				this.item.EndEvent = new EndEvent(this.EndEventHandle);
				this.item.ErrorEvent = new ErrorEvent(this.ErrorEventHandle);
				this.item.IconBase64 = iconBase64;
				this.IconBase64 = iconBase64;
				this.imgIcon.Source = this.IconDecode(this.IconBase64);
				this.progressBar.Maximum = (double)size;
				this.tbkFilename.Text = fileName;
				this.fileName = fileName;
				this.tbkMsg.Text = "收到文件请求";
				this.tbkSize.Text = this.GetLength(size);
				this.ShowAcceptButton();
			}
		}
		public void Download()
		{
			if (this.item != null)
			{
				this.isProcessing = true;
				System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(this.DownloadHandle));
			}
		}
		private void DownloadHandle(object obj)
		{
			this.item.Start();
		}
		public void Send()
		{
			if (this.item != null)
			{
				this.item.Start();
			}
		}
		public BitmapImage IconDecode(string base64)
		{
			BitmapImage bi = null;
			if (!string.IsNullOrEmpty(base64))
			{
				try
				{
					bi = new BitmapImage();
					byte[] array = System.Convert.FromBase64String(base64);
					bi.BeginInit();
					bi.StreamSource = new System.IO.MemoryStream(array);
					bi.EndInit();
				}
				catch (System.Exception ex)
				{
					ServiceUtil.Instance.Logger.Error(ex.ToString());
				}
			}
			return bi;
		}
		public BitmapImage IconToBitmap(Icon icon)
		{
			Bitmap bitmap = null;
			BitmapImage bi = null;
			BitmapImage result;
			if (icon == null)
			{
				result = null;
			}
			else
			{
				try
				{
					bitmap = icon.ToBitmap();
					System.IO.MemoryStream ms = new System.IO.MemoryStream();
					bitmap.Save(ms, ImageFormat.Png);
					bi = new BitmapImage();
					bi.BeginInit();
					bi.StreamSource = ms;
					bi.EndInit();
				}
				finally
				{
					if (bitmap != null)
					{
						bitmap.Dispose();
					}
				}
				result = bi;
			}
			return result;
		}
		private void ErrorEventFileIsReadOnly()
		{
			if (this.staff != null)
			{
				if (this.fileInfo != null)
				{
					string staffErrorFileIsReadOnly = "文件" + this.fileName + "被设为只读，文件发送失败";
					this.staffChatTab.ChatComponent.AddMessageNotice(staffErrorFileIsReadOnly);
					this.viewModel.SendStaffUpDownNotice(this.sessionService.Jid, this.staff.Jid, staffErrorFileIsReadOnly, "", 3);
				}
			}
			if (this.coopStaff != null)
			{
				if (this.fileInfo != null)
				{
					string staffErrorFileIsReadOnly = "文件" + this.fileName + "被设为只读，文件发送失败";
					this.staffChatTab.ChatComponent.AddMessageNotice(staffErrorFileIsReadOnly);
					this.viewModel.SendCooperationStaffUpDownNotice(this.sessionService.Jid, this.coopStaff.Jid, this.coopStaff.UnitedProjectid, staffErrorFileIsReadOnly, "", 3);
				}
			}
			if (this.group != null)
			{
				if (this.fileInfo != null)
				{
					string groupErrorFileIsReadOnly = "文件" + this.fileName + "被设为只读，文件发送失败";
					this.groupChatTab.ChatComponent.AddMessageNotice(groupErrorFileIsReadOnly);
					this.viewModel.SendGroupUpDownFile(this.sessionService.Jid, this.group.Gid, groupErrorFileIsReadOnly, "", 3);
				}
			}
			this.CloseFileItem();
		}
		private void StopEventHandle(string id)
		{
			this.CloseFileItem();
		}
		private void ErrorEventHandle(string id, string message)
		{
			if (this.staff != null)
			{
				if (this.fileInfo != null)
				{
					string staffErrorFileUpload = "文件" + this.fileName + "发送失败";
					this.staffChatTab.ChatComponent.AddMessageNotice(staffErrorFileUpload);
					this.viewModel.SendStaffUpDownNotice(this.sessionService.Jid, this.staff.Jid, staffErrorFileUpload, "", 3);
				}
				else
				{
					string staffErrorFileDownload = "文件" + this.fileName + "接收失败";
					this.staffChatTab.ChatComponent.AddMessageNotice(staffErrorFileDownload);
					this.viewModel.SendStaffDownLoadNotice(this.sessionService.Jid, this.staff.Jid, this.fileName, "", 4, null, null, null);
				}
			}
			if (this.coopStaff != null)
			{
				if (this.fileInfo != null)
				{
					string staffErrorFileUpload = "文件" + this.fileName + "发送失败";
					this.coopStaffChatTab.ChatComponent.AddMessageNotice(staffErrorFileUpload);
					this.viewModel.SendCooperationStaffUpDownNotice(this.sessionService.Jid, this.coopStaff.Jid, this.coopStaff.UnitedProjectid, staffErrorFileUpload, "", 3);
				}
				else
				{
					string staffErrorFileDownload = "文件" + this.fileName + "接收失败";
					this.coopStaffChatTab.ChatComponent.AddMessageNotice(staffErrorFileDownload);
					this.viewModel.SendCooperationStaffDownLoadNotice(this.sessionService.Jid, this.coopStaff.Jid, this.coopStaff.UnitedProjectid, this.fileName, "", 4, null, null, null);
				}
			}
			if (this.group != null)
			{
				if (this.fileInfo != null)
				{
					string groupErorFileUpload = "文件" + this.fileName + "发送失败";
					this.groupChatTab.ChatComponent.AddMessageNotice("文件" + this.fileName + "文件");
					this.viewModel.SendGroupUpDownFile(this.sessionService.Jid, this.group.Gid, groupErorFileUpload, "", 3);
				}
				else
				{
					string groupErrorFileDownload = this.userName + "的文件" + this.fileName + "接收失败";
					this.groupChatTab.ChatComponent.AddMessageNotice(groupErrorFileDownload);
					this.viewModel.SendGroupDownLoadFile(this.sessionService.Jid, this.group.Gid, groupErrorFileDownload, "", 4, null, null, null);
				}
			}
			this.CloseFileItem();
		}
		private void EndEventHandle(string id, bool isStop)
		{
			try
			{
				if (!isStop)
				{
					if (this.staff != null)
					{
						if (this.fileInfo != null)
						{
							string staffEndFileUpload = "文件" + this.fileName + "发送成功";
							this.viewModel.SendOffLineFile(this.sessionService.Uid, this.staff.Uid, id, this.IconBase64, this.fileName, this.fileInfo.Length);
							this.staffChatTab.ChatComponent.AddMessageNotice(staffEndFileUpload);
						}
						else
						{
							string staffEndFileDownload = "成功接收文件" + this.fileName;
							this.staffChatTab.ChatComponent.AddMessageNotice(staffEndFileDownload);
							if (this.SaveDirectory == null)
							{
								if (this.fileDir != null)
								{
									this.viewModel.SendStaffDownLoadNotice(this.sessionService.Jid, this.staff.Jid, this.fileName.Trim(), "", 2, this.IconBase64, this.fileName, this.fileDir);
								}
							}
							else
							{
								this.viewModel.SendStaffDownLoadNotice(this.sessionService.Jid, this.staff.Jid, this.fileName.Trim(), "", 2, this.IconBase64, this.fileName, this.SaveDirectory);
								this.SaveDirectory = null;
							}
							if (this.item != null)
							{
								OpenFileControl openFile = new OpenFileControl(this.item, true);
								this.staffChatTab.ChatComponent.AddOpenFileControl(openFile, this.staff.Name);
							}
						}
					}
					if (this.coopStaff != null)
					{
						if (this.fileInfo != null)
						{
							string staffEndFileUpload = "文件" + this.fileName + "发送成功";
							this.viewModel.SendCooperationFileNoticeRequest(this.sessionService.Uid, this.coopStaff.Uid, this.coopStaff.Jid, this.coopStaff.UnitedProjectid, id, this.IconBase64, this.fileName, this.fileInfo.Length);
							this.coopStaffChatTab.ChatComponent.AddMessageNotice(staffEndFileUpload);
						}
						else
						{
							string staffEndFileDownload = "成功接收文件" + this.fileName;
							this.coopStaffChatTab.ChatComponent.AddMessageNotice(staffEndFileDownload);
							if (this.SaveDirectory == null)
							{
								if (this.fileDir != null)
								{
									this.viewModel.SendCooperationStaffDownLoadNotice(this.sessionService.Jid, this.coopStaff.Jid, this.coopStaff.UnitedProjectid, this.fileName.Trim(), "", 2, this.IconBase64, this.fileName, this.fileDir);
								}
							}
							else
							{
								this.viewModel.SendCooperationStaffDownLoadNotice(this.sessionService.Jid, this.coopStaff.Jid, this.coopStaff.UnitedProjectid, this.fileName.Trim(), "", 2, this.IconBase64, this.fileName, this.SaveDirectory);
								this.SaveDirectory = null;
							}
							if (this.item != null)
							{
								OpenFileControl openFile = new OpenFileControl(this.item, true);
								this.coopStaffChatTab.ChatComponent.AddOpenFileControl(openFile, this.coopStaff.Name);
							}
						}
					}
					if (this.group != null)
					{
						if (this.fileInfo != null)
						{
							string groupEndFileUpload = "成功发送文件" + this.fileName + "至群";
							this.viewModel.SendGroupFile(this.sessionService.Uid, this.group.Gid, id, this.IconBase64, this.fileName);
							this.groupChatTab.ChatComponent.AddMessageNotice("成功发送文件" + this.fileName);
							this.viewModel.SendGroupUpDownFile(this.sessionService.Jid, this.group.Gid, groupEndFileUpload, "", 6);
						}
						else
						{
							string groupEndFileDownload = "成功接收" + this.userName + "的文件" + this.fileName;
							this.groupChatTab.ChatComponent.AddMessageNotice(groupEndFileDownload);
							if (this.SaveDirectory == null)
							{
								this.viewModel.SendGroupDownLoadFile(this.sessionService.Jid, this.group.Gid, groupEndFileDownload, "", 2, this.fileName, this.IconBase64, this.fileDir);
							}
							else
							{
								this.viewModel.SendGroupDownLoadFile(this.sessionService.Jid, this.group.Gid, groupEndFileDownload, "", 2, this.fileName, this.IconBase64, this.SaveDirectory);
								this.SaveDirectory = null;
							}
							if (this.item != null)
							{
								OpenFileControl openFile = new OpenFileControl(this.item, true);
								this.groupChatTab.ChatComponent.AddOpenFileControl(openFile, this.userName);
							}
						}
					}
				}
				else
				{
					if (this.staff != null)
					{
						if (this.fileInfo != null)
						{
							string staffStopFileUpload = "文件" + this.fileName + "发送失败";
							this.staffChatTab.ChatComponent.AddMessageNotice(staffStopFileUpload);
							this.viewModel.SendStaffUpDownNotice(this.sessionService.Jid, this.staff.Jid, staffStopFileUpload, "", 3);
						}
						else
						{
							string staffStopFileDownload = "文件" + this.fileName + "接收失败";
							this.staffChatTab.ChatComponent.AddMessageNotice(staffStopFileDownload);
							this.viewModel.SendStaffDownLoadNotice(this.sessionService.Jid, this.staff.Jid, this.fileName.Trim(), "", 4, null, null, null);
						}
					}
					if (this.coopStaff != null)
					{
						if (this.fileInfo != null)
						{
							string staffStopFileUpload = "文件" + this.fileName + "发送失败";
							this.coopStaffChatTab.ChatComponent.AddMessageNotice(staffStopFileUpload);
							this.viewModel.SendCooperationStaffUpDownNotice(this.sessionService.Jid, this.coopStaff.Jid, this.coopStaff.UnitedProjectid, staffStopFileUpload, "", 3);
						}
						else
						{
							string staffStopFileDownload = "文件" + this.fileName + "接收失败";
							this.coopStaffChatTab.ChatComponent.AddMessageNotice(staffStopFileDownload);
							this.viewModel.SendCooperationStaffDownLoadNotice(this.sessionService.Jid, this.coopStaff.Jid, this.coopStaff.UnitedProjectid, this.fileName.Trim(), "", 4, null, null, null);
						}
					}
					if (this.group != null)
					{
						if (this.fileInfo != null)
						{
							string groupStopFileUpload = "文件" + this.fileName + "发送失败";
							this.groupChatTab.ChatComponent.AddMessageNotice(groupStopFileUpload);
							this.viewModel.SendGroupUpDownFile(this.sessionService.Jid, this.group.Gid, groupStopFileUpload, "", 3);
						}
						else
						{
							string groupSotpFileDownload = string.Concat(new string[]
							{
								"接收",
								this.userName,
								"的文件",
								this.fileName,
								"失败"
							});
							this.groupChatTab.ChatComponent.AddMessageNotice(groupSotpFileDownload);
							this.viewModel.SendGroupDownLoadFile(this.sessionService.Jid, this.group.Gid, groupSotpFileDownload, "", 4, null, null, null);
						}
					}
				}
				this.CloseFileItem();
			}
			catch (System.Exception e)
			{
				System.Console.WriteLine("ddd " + e.ToString());
			}
		}
		private void ProcessEventHandle(string id, long size)
		{
			try
			{
				this.progressBar.Value = (double)size;
				this.UpdateSpeed(size);
			}
			catch (System.Exception ex)
			{
				System.Console.WriteLine(ex.ToString());
			}
		}
		private string GetLength(long size)
		{
			long part = 1024L;
			if (size >= 1048576L)
			{
				this._unit = "MB";
				part = 1048576L;
			}
			else
			{
				if (size >= 1073741824L)
				{
					this._unit = "GB";
					part = 1073741824L;
				}
			}
			return ((double)size / (double)part).ToString("0.00") + this._unit;
		}
		private void UpdateSpeed(long nowSize)
		{
			long size = nowSize - this._upSize;
			this._upSize = nowSize;
			long part = 1L;
			if (size < 1024L)
			{
				part = 1L;
				this._speedUnit = "B/s";
			}
			if (size >= 1024L)
			{
				part = 1024L;
				this._speedUnit = "KB/s";
			}
			if (size >= 1048576L)
			{
				part = 1048576L;
				this._speedUnit = "MB/s";
			}
			this.tbkSpeed.Text = "速度：" + ((double)size / (double)part).ToString("0.00") + this._speedUnit;
		}
		private void CloseFileItem()
		{
			if (this.EndEvent != null)
			{
				this.EndEvent(this);
			}
		}
		private void btnSaveAs_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				SaveFileDialog saveDialog = new SaveFileDialog();
				saveDialog.FileName = this.fileName;
				saveDialog.InitialDirectory = this.sessionService.SaveAsFilePath;
				DialogResult dr = saveDialog.ShowDialog();
				if (DialogResult.OK == dr)
				{
					System.IO.FileInfo info = new System.IO.FileInfo(saveDialog.FileName);
					this.sessionService.SaveAsFilePath = info.DirectoryName;
					this.SaveAs(saveDialog.FileName);
					string fileName = info.Name;
					this.SaveDirectory = saveDialog.FileName.Substring(0, saveDialog.FileName.IndexOf(fileName));
				}
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		public void SaveAs(string fileName)
		{
			if (!string.IsNullOrEmpty(this.fileDir) && this.fileInfo == null)
			{
				this.item.FileName = fileName;
				if (System.IO.File.Exists(this.item.FileName))
				{
					System.IO.FileStream fs = null;
					try
					{
						fs = System.IO.File.OpenWrite(this.item.FileName);
					}
					catch (System.Exception ex)
					{
						ServiceUtil.Instance.Logger.Error(ex.ToString());
						this.SaveAs(System.IO.Path.GetDirectoryName(this.item.FileName) + "\\" + this.GetUniqueFileName(System.IO.Path.GetDirectoryName(this.item.FileName) + "\\", this.item.FileName, new System.Collections.Generic.List<string>()));
						return;
					}
					finally
					{
						if (fs != null)
						{
							fs.Close();
						}
					}
				}
				this.item.FileName = fileName;
				this.ShowCancelButton();
				this.Download();
			}
		}
		public void SaveAs(string selectedPath, int existedFileCount, System.Collections.Generic.List<string> processingFileNames, MessageBoxResult mbs)
		{
			if (this.fileInfo == null && this.item != null)
			{
				this.item.FileName = selectedPath + this.fileName;
				this.CreateDirectory(selectedPath, string.Empty);
				if ((System.IO.File.Exists(this.item.FileName) || processingFileNames.Contains(this.item.FileName)) && !this.AutoRename)
				{
					if (mbs == MessageBoxResult.No)
					{
						this.SaveAs(selectedPath + this.GetUniqueFileName(selectedPath, this.item.FileName, processingFileNames));
					}
					else
					{
						System.IO.FileStream fs = null;
						try
						{
							fs = System.IO.File.OpenWrite(this.item.FileName);
						}
						catch (System.Exception ex)
						{
							ServiceUtil.Instance.Logger.Error(ex.ToString());
							this.SaveAs(selectedPath + this.GetUniqueFileName(selectedPath, this.item.FileName, processingFileNames));
							return;
						}
						finally
						{
							if (fs != null)
							{
								fs.Close();
							}
						}
						processingFileNames.Add(this.item.FileName);
						this.ShowCancelButton();
						this.Download();
					}
				}
				else
				{
					if (this.AutoRename)
					{
						this.SaveAs(selectedPath + this.GetUniqueFileName(selectedPath, this.item.FileName, processingFileNames));
					}
					else
					{
						processingFileNames.Add(this.item.FileName);
						this.ShowCancelButton();
						this.Download();
					}
				}
			}
		}
		private void btnAccept_Click(object sender, RoutedEventArgs e)
		{
			this.Accept();
		}
		public void Accept()
		{
			if (this.fileInfo == null && this.item != null)
			{
				this.item.FileName = this.fileDir + this.fileName;
				this.CreateDirectory(this.fileDir, string.Empty);
				if (System.IO.File.Exists(this.item.FileName))
				{
					MessageBoxResult result = System.Windows.MessageBox.Show("接收文件目录有同名文件，是否需要覆盖", "友情提示", MessageBoxButton.YesNoCancel);
					if (result == MessageBoxResult.Yes)
					{
						System.IO.FileStream fs = null;
						try
						{
							fs = System.IO.File.OpenWrite(this.item.FileName);
						}
						catch (System.Exception ex)
						{
							ServiceUtil.Instance.Logger.Error(ex.ToString());
							this.SaveAs(System.IO.Path.GetDirectoryName(this.item.FileName) + "\\" + this.GetUniqueFileName(System.IO.Path.GetDirectoryName(this.item.FileName) + "\\", this.item.FileName, new System.Collections.Generic.List<string>()));
							return;
						}
						finally
						{
							if (fs != null)
							{
								fs.Close();
							}
						}
						this.ShowCancelButton();
						this.Download();
					}
					else
					{
						if (result == MessageBoxResult.No)
						{
							SaveFileDialog saveDialog = new SaveFileDialog();
							saveDialog.FileName = this.fileName;
							saveDialog.InitialDirectory = this.fileDir;
							DialogResult dr = saveDialog.ShowDialog();
							if (DialogResult.OK == dr)
							{
								System.IO.FileInfo info = new System.IO.FileInfo(saveDialog.FileName);
								this.sessionService.SaveAsFilePath = info.DirectoryName;
								this.SaveAs(saveDialog.FileName);
							}
						}
					}
				}
				else
				{
					this.ShowCancelButton();
					this.Download();
				}
			}
		}
		private string GetUniqueFileName(string selectedPath, string fileName, System.Collections.Generic.List<string> processingFileNames)
		{
			string fileExtension = string.Empty;
			int fileSuffix = 1;
			string strFileSuffix = "({0})";
			string result;
			if (fileName != null)
			{
				string oldFileName = System.IO.Path.GetFileNameWithoutExtension(fileName);
				fileExtension = System.IO.Path.GetExtension(fileName);
				while (true)
				{
					fileName = selectedPath + oldFileName + string.Format(strFileSuffix, fileSuffix) + fileExtension;
					if (!System.IO.File.Exists(fileName) && !processingFileNames.Contains(fileName))
					{
						break;
					}
					fileSuffix++;
				}
				processingFileNames.Add(fileName);
				fileName = oldFileName + string.Format(strFileSuffix, fileSuffix) + fileExtension;
				result = fileName;
			}
			else
			{
				result = fileName;
			}
			return result;
		}
		private void CreateDirectory(string fileDir, string childDir)
		{
			System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(fileDir);
			if (!dirInfo.Exists)
			{
				System.IO.Directory.CreateDirectory(fileDir);
			}
		}
		public string IconToBase64(Icon icon)
		{
			string str = string.Empty;
			string result;
			if (icon == null)
			{
				result = str;
			}
			else
			{
				Bitmap bitmap = null;
				System.IO.MemoryStream ms = null;
				try
				{
					ms = new System.IO.MemoryStream();
					bitmap = icon.ToBitmap();
					bitmap.Save(ms, ImageFormat.Png);
					byte[] array = new byte[ms.Length];
					ms.Position = 0L;
					ms.Read(array, 0, (int)ms.Length);
					str = System.Convert.ToBase64String(array);
				}
				finally
				{
					if (ms != null)
					{
						ms.Dispose();
					}
					if (bitmap != null)
					{
						bitmap.Dispose();
					}
				}
				result = str;
			}
			return result;
		}
		private void btnRefuse_Click(object sender, RoutedEventArgs e)
		{
			this.Refuse();
		}
		public void Refuse()
		{
			if (this.fileInfo == null)
			{
				if (this.staff != null)
				{
					string staffRefuseFileDownload = "您已拒绝接收文件" + this.fileName;
					this.staffChatTab.ChatComponent.AddMessageNotice(staffRefuseFileDownload);
					this.viewModel.SendStaffUpDownNotice(this.sessionService.Jid, this.staff.Jid, this.fileName, "", 5);
				}
				if (this.coopStaff != null)
				{
					string staffRefuseFileDownload = "您已拒绝接收文件" + this.fileName;
					this.coopStaffChatTab.ChatComponent.AddMessageNotice(staffRefuseFileDownload);
					this.viewModel.SendCooperationStaffUpDownNotice(this.sessionService.Jid, this.coopStaff.Jid, this.coopStaff.UnitedProjectid, this.fileName, "", 5);
				}
				if (this.group != null)
				{
					string groupRefuseFileDownload = "已拒绝接收" + this.userName + "的文件" + this.fileName;
					this.groupChatTab.ChatComponent.AddMessageNotice(groupRefuseFileDownload);
					this.viewModel.SendGroupUpDownFile(this.sessionService.Jid, this.group.Gid, groupRefuseFileDownload, "", 5);
				}
				this.CloseFileItem();
			}
		}
		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{
			if (this.item != null)
			{
				this.item.Stop();
			}
		}
		private void ShowCancelButton()
		{
			this.btnAccept.Visibility = Visibility.Collapsed;
			this.btnCancel.Visibility = Visibility.Visible;
			this.btnRefuse.Visibility = Visibility.Collapsed;
			this.btnSaveAs.Visibility = Visibility.Collapsed;
		}
		private void ShowAcceptButton()
		{
			this.btnAccept.Visibility = Visibility.Visible;
			this.btnCancel.Visibility = Visibility.Collapsed;
			this.btnRefuse.Visibility = Visibility.Visible;
			this.btnSaveAs.Visibility = Visibility.Visible;
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/newfilelistitem.xaml", UriKind.Relative);
        //        System.Windows.Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.imgIcon = (System.Windows.Controls.Image)target;
        //        break;
        //    case 2:
        //        this.tbkMsg = (TextBlock)target;
        //        break;
        //    case 3:
        //        this.tbkFilename = (TextBlock)target;
        //        break;
        //    case 4:
        //        this.progressBar = (System.Windows.Controls.ProgressBar)target;
        //        break;
        //    case 5:
        //        this.tbkSpeed = (TextBlock)target;
        //        break;
        //    case 6:
        //        this.tbkSize = (TextBlock)target;
        //        break;
        //    case 7:
        //        this.btnAccept = (System.Windows.Controls.Button)target;
        //        this.btnAccept.Click += new RoutedEventHandler(this.btnAccept_Click);
        //        break;
        //    case 8:
        //        this.btnSaveAs = (System.Windows.Controls.Button)target;
        //        this.btnSaveAs.Click += new RoutedEventHandler(this.btnSaveAs_Click);
        //        break;
        //    case 9:
        //        this.btnRefuse = (System.Windows.Controls.Button)target;
        //        this.btnRefuse.Click += new RoutedEventHandler(this.btnRefuse_Click);
        //        break;
        //    case 10:
        //        this.btnCancel = (System.Windows.Controls.Button)target;
        //        this.btnCancel.Click += new RoutedEventHandler(this.btnCancel_Click);
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
