using CSharpWin;
using IDKin.IM.Communicate;
using IDKin.IM.Core;
using IDKin.IM.CustomComponents.Controls;
using IDKin.IM.Data;
using IDKin.IM.ImageService;
using IDKin.IM.Log;
using IDKin.IM.Protocol.Account;
using IDKin.IM.Protocol.DynamicWork;
using IDKin.IM.Protocol.Enterprise;
using IDKin.IM.Util;
using IDKin.IM.Windows.Comps.OA;
using IDKin.IM.Windows.Properties;
using IDKin.IM.Windows.Util;
using IDKin.IM.Windows.View;
using IDKin.IM.Windows.View.Pages;
using IDKin.IM.Windows.ViewModel;
using Microsoft.Win32;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
namespace IDKin.IM.Windows.Comps
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class PersonalChatTabControl : System.Windows.Controls.UserControl//, IComponentConnector
	{
		private delegate void ShowCutDelegate();
		private delegate void HideCutDelegate();
		private const long MAX_FILE_LENGTH = 209715200L;
		public ISessionService sessionService = ServiceUtil.Instance.SessionService;
		private IDataService dataService = ServiceUtil.Instance.DataService;
		private IImageService imageService = ServiceUtil.Instance.ImageService;
		public IShowPanel IShowPanel = new IShowPanel();
		public FileTransmitList FileList = new FileTransmitList();
		private IFileService fileService = ServiceUtil.Instance.FileService;
		private ILogger logger = ServiceUtil.Instance.Logger;
		private ScreenshotPopup popup = null;
		private UserProfilePage profilePage = new UserProfilePage();
		private SelfProfilePage selPage = new SelfProfilePage();
		private IDKin.IM.Core.Staff staff;
		private INViewModel inViewModel = new INViewModel();
		//internal ImageBrush userHead;
		//internal TextBlock tbkName;
		//internal TextBlock tbkID;
		//internal TextBlock tbkSignature;
		//internal System.Windows.Controls.ToolTip toolTip;
		//internal TextBlock tbkToolTip;
		//internal System.Windows.Controls.TabControl ChatTab;
		//internal ChatComponent ChatComponent;
		//internal OACurrentWorkControl OACurrentWork;
		//internal TabItem profireItem;
		//internal Frame userProfileFrame;
		//internal TabItem tabItemPerson;
        ////private bool _contentLoaded;
		public UserProfilePage ProfilePage
		{
			get
			{
				return this.profilePage;
			}
		}
		public SelfProfilePage SelPage
		{
			get
			{
				return this.selPage;
			}
		}
		public long StaffId
		{
			get
			{
				long result;
				if (this.staff != null)
				{
					result = this.staff.Uid;
				}
				else
				{
					result = 0L;
				}
				return result;
			}
		}
		public PersonalChatTabControl(IDKin.IM.Core.Staff staff)
		{
			if (staff != null)
			{
				this.InitializeComponent();
				this.staff = staff;
				this.userHead.ImageSource = staff.HeaderImage42;
				this.tbkName.Text = staff.Name;
				this.tbkID.Text = "(" + staff.Uid + ")";
				this.tbkSignature.Text = staff.Signature;
				this.tbkToolTip.Text = staff.Signature;
				this.ChatComponent.InitData(staff);
				this.ChatComponent.btnGroupShield.Visibility = Visibility.Collapsed;
				this.ChatComponent.PanelChangeContent.Children.Add(this.FileList);
				this.ChatComponent.PanelChangeContent.Children.Add(this.IShowPanel);
				this.ChatComponent.MsgRecordStatus = new ChatComponent.MsgRecordStatusDelegate(this.MsgRecordStatusHandle);
				this.ChatComponent.viewMsgBox.PreviewDragEnter += new System.Windows.DragEventHandler(this.TextBox_PreviewDragEnter);
				this.ChatComponent.viewMsgBox.PreviewDragOver += new System.Windows.DragEventHandler(this.TextBox_PreviewDragEnter);
				this.ChatComponent.viewMsgBox.PreviewDrop += new System.Windows.DragEventHandler(this.UserControl_Drop);
				this.ChatComponent.inputMsgBox.PreviewDragEnter += new System.Windows.DragEventHandler(this.TextBox_PreviewDragEnter);
				this.ChatComponent.inputMsgBox.PreviewDragOver += new System.Windows.DragEventHandler(this.TextBox_PreviewDragEnter);
				this.ChatComponent.inputMsgBox.PreviewDrop += new System.Windows.DragEventHandler(this.UserControl_Drop);
				this.ShowIShowPanel();
				this.userProfileFrame.NavigationService.Navigate(this.profilePage);
				this.OACurrentWork.crwDocumentManagement.AllBaseDynamicWorkObjViewModel = new AllBaseDynamicWorkObjViewModel(DynamicWorkModuleType.DOCUMENT_MANAGEMENT, this.staff, this.OACurrentWork.crwDocumentManagement);
				this.OACurrentWork.crwInsideDiscussion.AllBaseDynamicWorkObjViewModel = new AllBaseDynamicWorkObjViewModel(DynamicWorkModuleType.INSIDE_DISCUSSION, this.staff, this.OACurrentWork.crwInsideDiscussion);
				this.OACurrentWork.crwInsideNotice.AllBaseDynamicWorkObjViewModel = new AllBaseDynamicWorkObjViewModel(DynamicWorkModuleType.INSIDE_NOTICE, this.staff, this.OACurrentWork.crwInsideNotice);
				this.OACurrentWork.crwProjectManagement.AllBaseDynamicWorkObjViewModel = new AllBaseDynamicWorkObjViewModel(DynamicWorkModuleType.PROJECT_MANAGEMENT, this.staff, this.OACurrentWork.crwProjectManagement);
				this.OACurrentWork.crwSystemManagement.AllBaseDynamicWorkObjViewModel = new AllBaseDynamicWorkObjViewModel(DynamicWorkModuleType.SYSTEM_MANAGEMENT, this.staff, this.OACurrentWork.crwSystemManagement);
				this.OACurrentWork.crwWorkCooperation.AllBaseDynamicWorkObjViewModel = new AllBaseDynamicWorkObjViewModel(DynamicWorkModuleType.WORK_COOPERATION, this.staff, this.OACurrentWork.crwWorkCooperation);
				this.OACurrentWork.crwWorkPlan.AllBaseDynamicWorkObjViewModel = new AllBaseDynamicWorkObjViewModel(DynamicWorkModuleType.WORK_PLAN, this.staff, this.OACurrentWork.crwWorkPlan);
			}
		}
		private void DepartmentBlockListDataGetted(DepartmentBlockListResponse response)
		{
			if (response != null)
			{
				if (response.deptBlockList != null)
				{
					this.sessionService.DeptBlockList.Clear();
					this.sessionService.DeptBlockList.AddRange(response.deptBlockList);
				}
			}
		}
		public void UpdateInfo()
		{
			this.userHead.ImageSource = this.staff.HeaderImage;
		}
		private void TextBox_PreviewDragEnter(object sender, System.Windows.DragEventArgs e)
		{
			e.Effects = System.Windows.DragDropEffects.Copy;
			e.Handled = true;
		}
		private void MsgRecordStatusHandle(bool isChecked)
		{
			if (isChecked)
			{
				this.ShowMsgRecordComp();
			}
			else
			{
				if (this.FileList.fileList.Items.Count > 0)
				{
					this.ShowFileList();
				}
				else
				{
					this.ShowIShowPanel();
				}
			}
		}
		private void UserControl_Drop(object sender, System.Windows.DragEventArgs e)
		{
			try
			{
				if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop))
				{
					string[] fileArray = (string[])e.Data.GetData(System.Windows.DataFormats.FileDrop);
					if (this.sessionService.Status == UserStatus.Offline)
					{
						this.ChatComponent.AddMessageNotice("您现在处于离线状态，不能发送文件！");
						return;
					}
					this.AddSendFileHandle(fileArray);
				}
				else
				{
					if (e.Data.GetDataPresent(System.Windows.DataFormats.Bitmap))
					{
						AnimatedImage animagteImage = this.imageService.GetAnimatebyIDImage(this.sessionService.AnimatedImageId);
						if (animagteImage != null)
						{
							animagteImage.Stretch = Stretch.None;
							animagteImage.DataContext = this.sessionService.AnimatedImageId;
							new InlineUIContainer(animagteImage, this.ChatComponent.inputMsgBox.Selection.End);
							this.ChatComponent.inputMsgBox.Focus();
							this.ChatComponent.inputMsgBox.SelectionChanged += new RoutedEventHandler(this.inputMsgBox_SelectionChanged);
						}
					}
					else
					{
						string dropData = e.Data.GetData(System.Windows.DataFormats.Text) as string;
						if (dropData != null && string.IsNullOrEmpty(dropData.Trim()) && !string.IsNullOrEmpty(UIControlUtil.Instance.DropImagePath))
						{
							System.Windows.Controls.Image image = new System.Windows.Controls.Image();
							Uri uri = new Uri(UIControlUtil.Instance.DropImagePath);
							image.Source = new BitmapImage(new Uri(uri.LocalPath, UriKind.Relative));
							this.InsertImage(image);
						}
					}
				}
				UIControlUtil.Instance.DropImagePath = string.Empty;
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		private void inputMsgBox_SelectionChanged(object sender, RoutedEventArgs e)
		{
			try
			{
				System.Windows.Controls.RichTextBox inputTextBox = this.ChatComponent.inputMsgBox;
				if (inputTextBox != null)
				{
					inputTextBox.SelectionChanged -= new RoutedEventHandler(this.inputMsgBox_SelectionChanged);
					BlockCollection bc = inputTextBox.Document.Blocks;
					if (bc != null && bc.Count > 0)
					{
						Block[] blocks = new Block[bc.Count];
						bc.CopyTo(blocks, 0);
						Block[] array = blocks;
						for (int i = 0; i < array.Length; i++)
						{
							Paragraph paragraph = (Paragraph)array[i];
							if (paragraph != null)
							{
								Inline[] inlines = new Inline[paragraph.Inlines.Count];
								paragraph.Inlines.CopyTo(inlines, 0);
								Inline[] array2 = inlines;
								for (int j = 0; j < array2.Length; j++)
								{
									Inline inline = array2[j];
									if (inline is InlineUIContainer)
									{
										InlineUIContainer ui = inline as InlineUIContainer;
										System.Windows.Controls.Image image = ui.Child as System.Windows.Controls.Image;
										if (image != null)
										{
											if (image.Source != null && !System.IO.File.Exists(image.Source.ToString()))
											{
												paragraph.Inlines.Remove(inline);
											}
										}
									}
								}
							}
						}
					}
				}
			}
			catch (System.InvalidCastException ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		public void AddReceiveHttpFile(OfflineFileResponse response)
		{
			this.ShowFileList();
			NewFileListItem item = new NewFileListItem(response.filename, response.id, response.size, this, this.staff, response.iconString);
			this.FileList.fileList.Items.Add(item);
			item.EndEvent = new NewFileListItem.EndEventDelegate(this.FileEndEventHandle);
		}
		private void AddSendFileItem(System.IO.FileInfo fileInfo)
		{
			if (fileInfo != null)
			{
				if (this.FindFileListItem(fileInfo.FullName) == null)
				{
					this.ShowFileList();
					NewFileListItem newFileItem = new NewFileListItem(fileInfo, this, this.staff);
					this.FileList.fileList.Items.Add(newFileItem);
					newFileItem.EndEvent = new NewFileListItem.EndEventDelegate(this.FileEndEventHandle);
					newFileItem.Send();
				}
			}
		}
		private NewFileListItem FindFileListItem(string fullName)
		{
			NewFileListItem result;
			if (!string.IsNullOrEmpty(fullName))
			{
				ItemCollection ic = this.FileList.fileList.Items;
				if (ic != null && ic.Count > 0)
				{
					foreach (NewFileListItem item in (System.Collections.IEnumerable)ic)
					{
						if (item != null && item.Item != null && item.Item.Info != null && item.Item.Info.FullName == fullName)
						{
							result = item;
							return result;
						}
					}
				}
			}
			result = null;
			return result;
		}
		private void FileEndEventHandle(NewFileListItem item)
		{
			try
			{
				if (item != null)
				{
					this.FileList.fileList.Items.Remove(item);
					if (this.FileList.fileList.Items.Count == 0)
					{
						if (this.ChatComponent.btnMsgRecord.IsChecked == true)
						{
							this.ShowMsgRecordComp();
						}
						else
						{
							this.ShowIShowPanel();
						}
					}
				}
			}
			catch (System.Exception e)
			{
				this.logger.Error(e.ToString());
			}
		}
		public void ShowIShowPanel()
		{
			if (this.FileList != null && this.IShowPanel != null && this.ChatComponent.MsgRecordComp != null)
			{
				this.IShowPanel.Visibility = Visibility.Visible;
				this.FileList.Visibility = Visibility.Collapsed;
				this.ChatComponent.MsgRecordComp.Visibility = Visibility.Collapsed;
			}
			this.CountChatPanelWidth(400);
		}
		public void ShowMsgRecordComp()
		{
			if (this.FileList != null && this.IShowPanel != null && this.ChatComponent.MsgRecordComp != null)
			{
				this.IShowPanel.Visibility = Visibility.Collapsed;
				this.FileList.Visibility = Visibility.Collapsed;
				this.ChatComponent.MsgRecordComp.Visibility = Visibility.Visible;
			}
			this.CountChatPanelWidth(520);
		}
		public void ShowFileList()
		{
			if (this.FileList != null && this.IShowPanel != null && this.ChatComponent.MsgRecordComp != null)
			{
				this.IShowPanel.Visibility = Visibility.Collapsed;
				this.FileList.Visibility = Visibility.Visible;
				this.ChatComponent.MsgRecordComp.Visibility = Visibility.Collapsed;
			}
			this.CountChatPanelWidth(445);
		}
		private void CountChatPanelWidth(int value)
		{
			INWindow inWindow = this.dataService.INWindow as INWindow;
			if (inWindow != null)
			{
				this.sessionService.ChatViewMsgBoxWidth = inWindow.Width - (double)value;
			}
		}
		private void btnSendFile_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (this.sessionService.Status == UserStatus.Offline)
				{
					this.ChatComponent.AddMessageNotice("您现在处于离线状态，不能发送文件！");
				}
				else
				{
					Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
					dialog.Filter = "所有文件|*";
					dialog.RestoreDirectory = true;
					dialog.Multiselect = true;
					dialog.ShowReadOnly = true;
					if (dialog.ShowDialog() == true)
					{
						string[] fileArray = dialog.FileNames;
						this.AddSendFileHandle(fileArray);
					}
				}
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		private void AddSendFileHandle(string[] fileArray)
		{
			if (fileArray != null && fileArray.Length > 0)
			{
				int i = 0;
				while (i < fileArray.Length)
				{
					string filePath = fileArray[i];
					System.IO.FileInfo fileInfo = new System.IO.FileInfo(filePath);
					if (ValidationUtil.IsSharePath(fileInfo.FullName))
					{
						this.ChatComponent.AddMessageNotice("目前暂时不支持发送共享文件!");
					}
					else
					{
						if (this.staff == null)
						{
							break;
						}
						if (this.sessionService.DeptBlockList.Contains(this.staff.DepartmentId))
						{
							IDKin.IM.Core.Department dep = this.dataService.GetDepartment(this.staff.DepartmentId);
							string depname = (dep == null) ? string.Empty : dep.Name;
							this.ChatComponent.AddMessageNotice(string.Concat(new string[]
							{
								"您没有权限向\"",
								depname,
								"\"发送文件,\"",
								fileInfo.Name,
								"\"发送失败"
							}));
						}
						else
						{
							if (fileInfo.Length / 10L <= 209715200L)
							{
								if (fileInfo.Length == 0L)
								{
									this.ChatComponent.AddMessageNotice("您发的文件" + fileInfo.Name + "内容为0KB, 发送失败");
								}
								else
								{
									this.AddSendFileItem(fileInfo);
								}
							}
							else
							{
								this.ChatComponent.AddMessageNotice("您发的文件" + fileInfo.Name + "已经超过最大容量2G, 发送失败");
							}
						}
					}
					IL_193:
					i++;
					continue;
					goto IL_193;
				}
			}
		}
		private void btnMsgBox_Click(object sender, RoutedEventArgs e)
		{
			System.Console.WriteLine("btnMsgBox_Click");
		}
		private void btnScreenshot_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (this.sessionService.IsAllowCut)
				{
					DependencyObject d = VisualTreeHelper.GetChild(this.tabItemPerson, 0);
					System.Windows.Controls.Button button = LogicalTreeHelper.FindLogicalNode(d, "btnTabItem") as System.Windows.Controls.Button;
					DependencyObject d2 = VisualTreeHelper.GetChild(button, 0);
					ImageSplitButton splitButton = LogicalTreeHelper.FindLogicalNode(d2, "btnScreenshot") as ImageSplitButton;
					if (e.Source == splitButton)
					{
						if (this.sessionService.IsCutScreenHidenWindow)
						{
							this.sessionService.IsAllowCut = false;
							this.CutScreenHidenWindowProcessor();
							this.StartCaptureImage();
							this.CutScreenEndShowWindow();
						}
						else
						{
							this.sessionService.IsAllowCut = false;
							this.StartCaptureImage();
							this.CutScreenEndShowWindow();
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		private void btnScreenshot_ArrowClick(object sender, RoutedEventArgs e)
		{
			this.popup = new ScreenshotPopup();
			this.popup.StaysOpen = false;
			this.popup.PlacementTarget = (e.Source as System.Windows.Controls.Button);
			this.popup.Placement = PlacementMode.Bottom;
			this.popup.IsOpen = true;
			this.popup.rbtnCutShow.Click += new RoutedEventHandler(this.rbtnCutShow_Click);
			this.popup.rbtnCutHide.Click += new RoutedEventHandler(this.rbtnCutHide_Click);
		}
		private void rbtnCutShow_Click(object sender, RoutedEventArgs e)
		{
			if (this.popup.rbtnCutShow.IsChecked == true)
			{
				this.popup.IsOpen = false;
				System.Threading.Thread.Sleep(50);
				System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(this.ShowCutPool));
			}
		}
		private void ShowCutPool(object obj)
		{
			System.Threading.Thread.Sleep(100);
			base.Dispatcher.BeginInvoke(new PersonalChatTabControl.ShowCutDelegate(this.ShowCutHandle), new object[0]);
		}
		private void ShowCutHandle()
		{
			this.sessionService.IsCutScreenHidenWindow = false;
			Settings.Default.IsCutScreenHidenWindow = this.sessionService.IsCutScreenHidenWindow;
			Settings.Default.Save();
			this.sessionService.IsAllowCut = false;
			this.StartCaptureImage();
			this.CutScreenEndShowWindow();
		}
		private void rbtnCutHide_Click(object sender, RoutedEventArgs e)
		{
			if (this.popup.rbtnCutHide.IsChecked == true)
			{
				this.popup.IsOpen = false;
				System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(this.HideCutPool));
			}
		}
		private void HideCutPool(object obj)
		{
			System.Threading.Thread.Sleep(100);
			base.Dispatcher.BeginInvoke(new PersonalChatTabControl.HideCutDelegate(this.HideCutHandler), new object[0]);
		}
		private void HideCutHandler()
		{
			this.sessionService.IsCutScreenHidenWindow = true;
			Settings.Default.IsCutScreenHidenWindow = this.sessionService.IsCutScreenHidenWindow;
			Settings.Default.Save();
			this.sessionService.IsAllowCut = false;
			this.CutScreenHidenWindowProcessor();
			this.StartCaptureImage();
			this.CutScreenEndShowWindow();
		}
		public void StartCaptureImage()
		{
			try
			{
				CaptureImageTool capture = new CaptureImageTool();
				if (capture.ShowDialog() == DialogResult.OK)
				{
					System.Drawing.Image image = capture.Image;
					string file = this.SaveImage(image);
					this.SnippingScreenHandler(file);
				}
				capture.Dispose();
				this.sessionService.IsAllowCut = true;
			}
			catch (System.Exception e)
			{
				this.logger.Error(e.ToString());
			}
		}
		private void CutScreenEndShowWindow()
		{
			if (this.sessionService.IsCutScreenHidenWindow)
			{
				INWindow inWindow = this.dataService.INWindow as INWindow;
				if (inWindow != null)
				{
					inWindow.WindowState = WindowState.Normal;
				}
			}
		}
		private void CutScreenHidenWindowProcessor()
		{
			if (this.sessionService.IsCutScreenHidenWindow)
			{
				INWindow inWindow = this.dataService.INWindow as INWindow;
				if (inWindow != null)
				{
					inWindow.WindowState = WindowState.Minimized;
				}
			}
		}
		private void CancelSnippingScreenHandler()
		{
		}
		public void SnippingScreenHandler(string filepath)
		{
			string img = this.FileNameProcessor(filepath);
			if (!string.IsNullOrEmpty(img))
			{
				this.AddUpLoadHandle(img);
				System.Windows.Controls.Image image = new System.Windows.Controls.Image();
				string file = this.sessionService.DirImage + img;
				image.Source = new BitmapImage(new Uri(file, UriKind.Relative));
				this.InsertImage(image);
			}
		}
		public string FileNameProcessor(string fileName)
		{
			string file = string.Empty;
			string name = "";
			try
			{
				if (!string.IsNullOrEmpty(fileName) && System.IO.File.Exists(fileName))
				{
					name = this.GenerateFileName(fileName);
					if (!string.IsNullOrEmpty(name))
					{
						file = this.sessionService.DirImage + name;
						if (!System.IO.File.Exists(file))
						{
							System.IO.File.Copy(fileName, file);
						}
					}
				}
			}
			catch (System.Exception e)
			{
				ServiceUtil.Instance.Logger.Error(e.ToString());
			}
			return name;
		}
		public string GenerateFileName(string fileName)
		{
			System.IO.FileStream file = null;
			string name = string.Empty;
			try
			{
				if (System.IO.File.Exists(fileName))
				{
					file = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
					name = this.MD5Stream(file) + System.IO.Path.GetExtension(fileName);
				}
			}
			catch (System.Exception e)
			{
				ServiceUtil.Instance.Logger.Error(e.ToString());
			}
			finally
			{
				if (file != null)
				{
					file.Dispose();
					file = null;
				}
			}
			return name;
		}
		private void AddUpLoadHandle(string fileName)
		{
			System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(this.AddUpLoadImage), fileName);
		}
		public void AddUpLoadImage(object obj)
		{
			string fileName = obj as string;
			if (!string.IsNullOrEmpty(fileName))
			{
				this.fileService.UploadImage(fileName);
			}
		}
		public void InsertImage(System.Windows.Controls.Image image)
		{
			image.Stretch = Stretch.None;
			System.Windows.Controls.UserControl imageControl = UIControlUtil.Instance.CreateImageControl(image, this.sessionService.ChatViewMsgBoxWidth);
			new InlineUIContainer(imageControl, this.ChatComponent.inputMsgBox.Selection.End);
			this.ChatComponent.inputMsgBox.Focus();
		}
		private string SaveImage(System.Drawing.Image image)
		{
			string file = "";
			if (image != null)
			{
				System.IO.MemoryStream ms = new System.IO.MemoryStream();
				image.Save(ms, ImageFormat.Png);
				BitmapImage bitmap = new BitmapImage();
				bitmap.BeginInit();
				bitmap.StreamSource = ms;
				bitmap.EndInit();
				string tmp = this.sessionService.DirImage + "SnippingScreenTempFile.jpg";
				System.IO.FileStream fs = new System.IO.FileStream(tmp, System.IO.FileMode.Create);
				new PngBitmapEncoder
				{
					Frames = 
					{
						BitmapFrame.Create(bitmap)
					}
				}.Save(fs);
				fs.Position = 0L;
				string md5 = this.MD5Stream(fs);
				fs.Flush();
				fs.Close();
				if (md5 != string.Empty)
				{
					file = this.sessionService.DirImage + md5 + ".png";
					if (!System.IO.File.Exists(file))
					{
						System.IO.File.Move(tmp, file);
					}
				}
			}
			return file;
		}
		private string MD5Stream(System.IO.Stream stream)
		{
			System.Security.Cryptography.MD5CryptoServiceProvider md5 = null;
			string resule = string.Empty;
			try
			{
				md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
				byte[] hash = md5.ComputeHash(stream);
				resule = System.BitConverter.ToString(hash);
				resule = resule.Replace("-", "");
			}
			finally
			{
				if (md5 != null)
				{
					md5.Dispose();
				}
			}
			return resule;
		}
		private void btnSendImg_Click(object sender, RoutedEventArgs e)
		{
			System.Console.WriteLine("btnSendImg_Click");
		}
		private void btnGift_Click(object sender, RoutedEventArgs e)
		{
			System.Console.WriteLine("btnGift_Click");
		}
		private CloseableTabItem FindChatTab()
		{
			INWindow inWindow = this.dataService.INWindow as INWindow;
			CloseableTabItem result;
			if (inWindow != null)
			{
				ItemCollection ic = inWindow.ContentTab.Items;
				foreach (TabItem item in (System.Collections.IEnumerable)ic)
				{
					if (item != null)
					{
						CloseableTabItem cti = item as CloseableTabItem;
						if (cti != null)
						{
							TabItemHeaderControl tabHeader = cti.Header as TabItemHeaderControl;
						}
						PersonalChatTabControl pctc = item.Content as PersonalChatTabControl;
						if (pctc != null && this.staff != null && pctc.StaffId == this.staff.Uid)
						{
							result = cti;
							return result;
						}
					}
				}
			}
			result = null;
			return result;
		}
		private void ChatTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (this.staff != null && this.ChatTab.SelectedIndex == 3)
			{
				this.inViewModel.GetStaffInfo(this.staff.Uid);
			}
			if (this.ChatTab.SelectedIndex == 2)
			{
				this.OACurrentWork.CanLoadData = true;
			}
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/personalchattabcontrol.xaml", UriKind.Relative);
        //        System.Windows.Application.LoadComponent(this, resourceLocater);
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
        //        this.userHead = (ImageBrush)target;
        //        break;
        //    case 2:
        //        this.tbkName = (TextBlock)target;
        //        break;
        //    case 3:
        //        this.tbkID = (TextBlock)target;
        //        break;
        //    case 4:
        //        this.tbkSignature = (TextBlock)target;
        //        break;
        //    case 5:
        //        this.toolTip = (System.Windows.Controls.ToolTip)target;
        //        break;
        //    case 6:
        //        this.tbkToolTip = (TextBlock)target;
        //        break;
        //    case 7:
        //        this.ChatTab = (System.Windows.Controls.TabControl)target;
        //        this.ChatTab.SelectionChanged += new SelectionChangedEventHandler(this.ChatTab_SelectionChanged);
        //        break;
        //    case 8:
        //        this.ChatComponent = (ChatComponent)target;
        //        break;
        //    case 9:
        //        this.OACurrentWork = (OACurrentWorkControl)target;
        //        break;
        //    case 10:
        //        this.profireItem = (TabItem)target;
        //        break;
        //    case 11:
        //        this.userProfileFrame = (Frame)target;
        //        break;
        //    case 12:
        //        this.tabItemPerson = (TabItem)target;
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
