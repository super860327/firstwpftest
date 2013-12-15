using IDKin.IM.Communicate;
using IDKin.IM.Core;
using IDKin.IM.CustomComponents.Controls;
using IDKin.IM.Data;
using IDKin.IM.ImageService;
using IDKin.IM.Log;
using IDKin.IM.Protocol.Commons;
using IDKin.IM.Util;
using IDKin.IM.Windows.Comps.FileTransport;
using IDKin.IM.Windows.Model.MessageCenter;
using IDKin.IM.Windows.Properties;
using IDKin.IM.Windows.Util;
using IDKin.IM.Windows.View;
using IDKin.IM.Windows.ViewModel;
using Microsoft.Win32;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
namespace IDKin.IM.Windows.Comps
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public partial class ChatComponent : System.Windows.Controls.UserControl, System.IDisposable//, IComponentConnector
	{
		public delegate void MsgRecordStatusDelegate(bool isChecked);
		public delegate void ScroolToEndDelegate();
		public class KeyCommand : ICommand
		{
			private System.Action<object> _executeDelegate;
			public event System.EventHandler CanExecuteChanged;
			public KeyCommand(System.Action<object> executeDelegate)
			{
				this._executeDelegate = executeDelegate;
			}
			public bool CanExecute(object parameter)
			{
				return true;
			}
			public void Execute(object parameter)
			{
				this._executeDelegate(parameter);
			}
		}
		private const string MessageTip = "提示:";
		private const int LIMIT = 10;
		public ChatComponent.MsgRecordStatusDelegate MsgRecordStatus;
		private ChatTabControlViewModel viewModel = new ChatTabControlViewModel();
		private string tempImageFile = string.Empty;
		private AnimatedImage tempAnimatedImage = null;
		private Image tempImage = null;
		private MessageStyle messageStyle = new MessageStyle();
		private ISessionService sessionService;
		private IDataService dataService;
		private IImageService imageService;
		private IFileService fileService;
		private Staff staff;
		private Roster roster;
		private EntGroup group;
		private ILogger logger;
		private FacePanel facePanel = null;
		private InputBinding ibEnter = null;
		private InputBinding ibCtrl = null;
		private SendStylePopup sendStylePopup = null;
		private FastReplyPopup popup = null;
		private IUtilService utilService = ServiceUtil.Instance.utilService;
		private ScrollViewer ViewMessageBoxScrollViewer = null;
		private int ContentMaxLength = 1000;
		private System.TimeSpan upVibrationTime;
		private CooperationStaff coopStaff = null;
	
		public CooperationStaff CoopStaff
		{
			get
			{
				return this.coopStaff;
			}
			set
			{
				this.coopStaff = value;
			}
		}
		public ChatComponent()
		{
			this.InitializeComponent();
		}
		private void InitUI()
		{
			this.imgFace.Source = this.imageService.GetToolBar(ImageTypeToolBar.Face);
			this.imgShake.Source = this.imageService.GetToolBar(ImageTypeToolBar.Shake);
			this.imgFont.Source = this.imageService.GetToolBar(ImageTypeToolBar.Font);
			this.btnFontB.Source = this.imageService.GetToolBar(ImageTypeToolBar.StyleB);
			this.btnFontI.Source = this.imageService.GetToolBar(ImageTypeToolBar.StyleI);
			this.btnFontU.Source = this.imageService.GetToolBar(ImageTypeToolBar.StyleU);
			this.InitFontStyle();
			this.ibCtrl = new InputBinding(new ChatComponent.KeyCommand(delegate(object x)
			{
				this.KeySendMessage();
			}), new KeyGesture(System.Windows.Input.Key.Return, ModifierKeys.Control));
			this.ibEnter = new InputBinding(new ChatComponent.KeyCommand(delegate(object x)
			{
				this.KeySendMessage();
			}), new KeyGesture(System.Windows.Input.Key.Return));
			this.imgHide.Source = this.imageService.GetIcon(ImageTypeIcon.LHide);
			this.btnGroupShield.ContextMenu = null;
			this.SetKeyEnter();
		}
		private void sendStylePopup_Closed(object sender, System.EventArgs e)
		{
			this.SetKeyEnter();
		}
		private void SetKeyEnter()
		{
			if (Settings.Default.SystemSetup_HotKey_SendMessage == 2)
			{
				this.inputMsgBox.InputBindings.Add(this.ibEnter);
				this.inputMsgBox.InputBindings.Remove(this.ibCtrl);
			}
			else
			{
				this.inputMsgBox.InputBindings.Remove(this.ibEnter);
				this.inputMsgBox.InputBindings.Add(this.ibCtrl);
			}
		}
		private void KeySendMessage()
		{
			try
			{
				if (!this.IsMessageEmpty())
				{
					this.SendMessageHandler();
				}
			}
			catch (System.Exception)
			{
				this.AddMessageNotice("发送聊天消息失败");
			}
		}
		public void Dispose()
		{
			this.imageService.RemoveToolBar(ImageTypeToolBar.Face);
			this.imageService.RemoveToolBar(ImageTypeToolBar.Shake);
			this.imageService.RemoveToolBar(ImageTypeToolBar.Font);
			this.imageService.RemoveToolBar(ImageTypeToolBar.StyleB);
			this.imageService.RemoveToolBar(ImageTypeToolBar.StyleI);
			this.imageService.RemoveToolBar(ImageTypeToolBar.StyleU);
			this.imageService.RemoveIcon(ImageTypeIcon.LHide);
		}
		private void InitFontStyle()
		{
			int count = 0;
			string[] fontFamilyList = this.messageStyle.FontFamilyList;
			for (int i = 0; i < fontFamilyList.Length; i++)
			{
				string fontFamily = fontFamilyList[i];
				ComboBoxItem cbi = new ComboBoxItem();
				cbi.Content = fontFamily;
				cbi.DataContext = count++;
				this.fontFamily.Items.Add(cbi);
			}
			this.fontFamily.SelectionChanged += new SelectionChangedEventHandler(this.fontFamily_SelectionChanged);
			count = 0;
			int[] fontSizeList = this.messageStyle.FontSizeList;
			for (int i = 0; i < fontSizeList.Length; i++)
			{
				int fontSize = fontSizeList[i];
				ComboBoxItem cbi = new ComboBoxItem();
				cbi.Content = fontSize;
				cbi.DataContext = count++;
				this.fontSize.Items.Add(cbi);
			}
			this.fontSize.SelectionChanged += new SelectionChangedEventHandler(this.fontSizeClick);
			this.boldButton.IsChecked = new bool?(Settings.Default.FontWeight.Equals(1));
			this.italicButton.IsChecked = new bool?(Settings.Default.FontStyle.Equals(1));
			this.fontSize.SelectedIndex = Settings.Default.FontSize;
			this.fontFamily.SelectedIndex = Settings.Default.FontFamily;
			this.SetFontWeight(Settings.Default.FontWeight.Equals(1));
			this.SetFontStyle(Settings.Default.FontStyle.Equals(1));
			this.SetFontSize(Settings.Default.FontSize);
			this.SetFontFamily(Settings.Default.FontFamily);
			this.SetFontColor(new byte[]
			{
				Settings.Default.FontColorA,
				Settings.Default.FontColorR,
				Settings.Default.FontColorG,
				Settings.Default.FontColorB
			});
		}
		public void InitData(Roster roster)
		{
			this.roster = roster;
			this.InitService();
		}
		public void InitData(Staff staff)
		{
			this.staff = staff;
			this.InitService();
		}
		public void InitData(EntGroup group)
		{
			this.group = group;
			this.InitService();
		}
		private void InitService()
		{
			this.sessionService = ServiceUtil.Instance.SessionService;
			this.dataService = ServiceUtil.Instance.DataService;
			this.imageService = ServiceUtil.Instance.ImageService;
			this.logger = ServiceUtil.Instance.Logger;
			this.fileService = ServiceUtil.Instance.FileService;
			this.viewModel.AddEventListenerHandler();
			this.InitUI();
		}
		private void ViewMsgBox_ContextMenuOpening(object sender, ContextMenuEventArgs e)
		{
			this.ViewerMenuItemImageSaveAs.IsEnabled = false;
			if (!string.IsNullOrEmpty(UIControlUtil.Instance.TempImagePath))
			{
				this.ViewerMenuItemImageSaveAs.IsEnabled = true;
				this.tempImageFile = UIControlUtil.Instance.TempImagePath;
				this.tempAnimatedImage = UIControlUtil.Instance.TempAnimatedImage;
			}
		}
		private void ViewerMenuItemImageSaveAs_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				this.ViewerMenuItemImageSaveAs.IsEnabled = false;
				if (this.tempImageFile != string.Empty)
				{
					string ext = System.IO.Path.GetExtension(this.tempImageFile);
					Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
					dialog.Filter = ext + "图片|*" + ext;
					dialog.FilterIndex = 0;
					dialog.RestoreDirectory = true;
					dialog.FileName = "未命名";
					if (dialog.ShowDialog() == true)
					{
						if (!System.IO.File.Exists(this.tempImageFile))
						{
							Uri uri = new Uri(this.tempImageFile);
							this.tempImageFile = uri.LocalPath;
							System.IO.File.Copy(this.tempImageFile, dialog.FileName, true);
						}
					}
					this.tempImageFile = string.Empty;
				}
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		private void InputMsgBox_ContextMenuOpening(object sender, ContextMenuEventArgs e)
		{
			this.MenuItemImageSaveAs.IsEnabled = false;
			this.tempImageFile = string.Empty;
			if (!string.IsNullOrEmpty(UIControlUtil.Instance.TempImagePath))
			{
				this.MenuItemImageSaveAs.IsEnabled = true;
				this.tempImageFile = UIControlUtil.Instance.TempImagePath;
				this.tempImage = UIControlUtil.Instance.TempImage;
				this.tempAnimatedImage = UIControlUtil.Instance.TempAnimatedImage;
			}
		}
		private void ShowColorDialog(object sender, RoutedEventArgs e)
		{
			ColorDialog colorDlg = new ColorDialog();
			colorDlg.ShowDialog();
			this.SetFontColor(new byte[]
			{
				255,
				colorDlg.Color.R,
				colorDlg.Color.G,
				colorDlg.Color.B
			});
		}
		private void MenuItemImageSaveAs_Click(object sender, RoutedEventArgs e)
		{
			this.MenuItemImageSaveAs.IsEnabled = false;
			if (this.tempImageFile != string.Empty)
			{
				string ext = System.IO.Path.GetExtension(this.tempImageFile);
				Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
				dialog.Filter = ext + "图片|*" + ext;
				dialog.FilterIndex = 0;
				dialog.RestoreDirectory = true;
				dialog.FileName = "未命名";
				if (dialog.ShowDialog() == true)
				{
					if (System.IO.File.Exists(this.tempImageFile))
					{
						System.IO.File.Copy(this.tempImageFile, dialog.FileName, true);
					}
				}
				this.tempImageFile = string.Empty;
			}
		}
		private void inputMsgBoxCommandCopy(object sender, ExecutedRoutedEventArgs e)
		{
			try
			{
				if (string.IsNullOrEmpty(UIControlUtil.Instance.TempImagePath))
				{
					TextRange tr = new TextRange(this.inputMsgBox.Selection.Start, this.inputMsgBox.Selection.End);
					if (!string.IsNullOrEmpty(tr.Text.Trim()))
					{
						System.Windows.Clipboard.SetText(tr.Text);
					}
					else
					{
						System.Windows.Clipboard.Clear();
						if (this.tempImageFile == "Ax012Face" && this.tempAnimatedImage != null)
						{
							System.Windows.Clipboard.SetText("Ax012Face" + this.tempAnimatedImage.DataContext);
							this.tempImageFile = string.Empty;
							this.tempAnimatedImage = null;
						}
					}
				}
				else
				{
					if (UIControlUtil.Instance.TempImagePath == "Ax012Face" && UIControlUtil.Instance.TempAnimatedImage != null)
					{
						System.Windows.Clipboard.Clear();
						System.Windows.Clipboard.SetText("Ax012Face" + UIControlUtil.Instance.TempAnimatedImage.DataContext);
					}
					else
					{
						if (UIControlUtil.Instance.TempImage != null)
						{
							BitmapSource bitmap = (BitmapSource)UIControlUtil.Instance.TempImage.Source;
							if (bitmap != null)
							{
								System.Windows.Clipboard.SetImage(bitmap);
							}
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		private void CommandCopy(object sender, ExecutedRoutedEventArgs e)
		{
			try
			{
				if (string.IsNullOrEmpty(UIControlUtil.Instance.TempImagePath))
				{
					TextRange tr = new TextRange(this.viewMsgBox.Selection.Start, this.viewMsgBox.Selection.End);
					if (!string.IsNullOrEmpty(tr.Text.Trim()))
					{
						System.Windows.Clipboard.SetText(tr.Text);
					}
					else
					{
						System.Windows.Clipboard.Clear();
						if (this.tempImageFile == "Ax012Face" && this.tempAnimatedImage != null)
						{
							System.Windows.Clipboard.SetText("Ax012Face" + this.tempAnimatedImage.DataContext);
							this.tempImageFile = string.Empty;
							this.tempAnimatedImage = null;
						}
					}
				}
				else
				{
					if (UIControlUtil.Instance.TempImagePath == "Ax012Face" && UIControlUtil.Instance.TempAnimatedImage != null)
					{
						System.Windows.Clipboard.Clear();
						System.Windows.Clipboard.SetText("Ax012Face" + UIControlUtil.Instance.TempAnimatedImage.DataContext);
					}
					else
					{
						if (UIControlUtil.Instance.TempImage != null)
						{
							BitmapSource bitmap = (BitmapSource)UIControlUtil.Instance.TempImage.Source;
							if (bitmap != null)
							{
								System.Windows.Clipboard.SetImage(bitmap);
							}
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		private void CommandPaste(object sender, ExecutedRoutedEventArgs e)
		{
			try
			{
				this.inputMsgBox.Selection.Text = string.Empty;
				if (System.Windows.Clipboard.ContainsText())
				{
					string text = System.Windows.Clipboard.GetText();
					if (!string.IsNullOrEmpty(text))
					{
						if (text.IndexOf("Ax012Face") != -1)
						{
							text = text.Substring("Ax012Face".Length);
							if (text.Length < 3)
							{
								int i = int.Parse(text);
								AnimatedImage newimage = this.imageService.GetAnimatebyIDImage(i);
								if (newimage != null)
								{
									newimage.DataContext = i;
									UIControlUtil.Instance.AnimateImageProcessor(newimage);
									this.InsertAnimatedImage(newimage);
								}
							}
						}
						else
						{
							this.inputMsgBox.Selection.Start.InsertTextInRun(text);
						}
					}
				}
				if (System.Windows.Clipboard.ContainsImage())
				{
					string filename = this.sessionService.DirImage + "ClipboardTempFile.png";
					System.IO.FileStream stream = new System.IO.FileStream(filename, System.IO.FileMode.Create);
					new PngBitmapEncoder
					{
						Frames = 
						{
							BitmapFrame.Create(System.Windows.Clipboard.GetImage())
						}
					}.Save(stream);
					stream.Flush();
					stream.Close();
					string newFileName = this.sessionService.DirImage + ChatComponent.GenerateFileName(filename);
					if (!System.IO.File.Exists(newFileName))
					{
						System.IO.File.Move(filename, newFileName);
					}
					System.IO.FileInfo info = new System.IO.FileInfo(newFileName);
					this.AddUpLoadHandle(info.Name);
					this.InsertImage(new Image
					{
						Stretch = Stretch.None,
						Source = new BitmapImage(new Uri(newFileName, UriKind.Relative))
					});
				}
				else
				{
					if (this.tempImage != null)
					{
						this.InsertImage(new Image
						{
							Stretch = Stretch.None,
							Source = new BitmapImage(new Uri(this.tempImage.Source.ToString(), UriKind.Relative))
						});
						this.tempImage = null;
					}
				}
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		private void btnSend_Click(object sender, RoutedEventArgs e)
		{
			try
			{
                //if (e.Source == this.btnSend)
                //{
                //    if (this.IsMessageEmpty())
                //    {
                //        this.popup = new FastReplyPopup();
                //        this.popup.Staff = this.staff;
                //        this.popup.Group = this.group;
                //        this.popup.Roster = this.roster;
                //        this.popup.StaysOpen = false;
                //        this.popup.PlacementTarget = (e.Source as System.Windows.Controls.Button);
                //        this.popup.Placement = PlacementMode.Bottom;
                //        this.popup.IsOpen = true;
                //    }
                //    else
                //    {
                //        this.SendMessageHandler();
                //    }
                //}
			}
			catch (System.Exception ex)
			{
				ServiceUtil.Instance.Logger.Error(ex.ToString());
				this.AddMessageNotice("发送聊天消息失败");
			}
		}
		public void SendMessageHandler()
		{
			if (this.sessionService.Status != UserStatus.Offline)
			{
				if (this.staff != null)
				{
					IDKin.IM.Core.Message[] messages = this.GetMessageStaff(true);
					IDKin.IM.Core.Message[] array = messages;
					for (int i = 0; i < array.Length; i++)
					{
						IDKin.IM.Core.Message message = array[i];
						if (message != null)
						{
							message.FromJid = this.sessionService.Jid;
							message.ToJid = this.staff.Jid;
							this.AddMessageStaff(message, true);
							this.viewModel.SendMessageStaff(message);
							INWindow inWindow = this.dataService.INWindow as INWindow;
							if (inWindow != null)
							{
								inWindow.Employee.RecentLinkListItem.AddRecentLink(RecentLinkType.EntStaffChat, this.staff.Uid);
							}
							this.AddMessageCenterChatInfo(this.staff.Name, this.staff.Uid, MessageCenterType.EntStaff, this.sessionService.CalculateSystemTime());
						}
					}
				}
				else
				{
					if (this.group != null)
					{
						IDKin.IM.Core.Message[] messages = this.GetMessageGroup(true);
						if (messages != null && messages.Length > 0)
						{
							IDKin.IM.Core.Message[] array = messages;
							for (int i = 0; i < array.Length; i++)
							{
								IDKin.IM.Core.Message message = array[i];
								message.FromJid = this.sessionService.Jid;
								message.Gid = (long)((ulong)((uint)this.group.Gid));
								this.AddMessageGroup(message, true);
								this.viewModel.SendMessageGroup(message);
								this.AddMessageCenterChatInfo(this.group.Name, this.group.Gid, MessageCenterType.EntGroup, this.sessionService.CalculateSystemTime());
							}
						}
					}
					else
					{
						if (this.roster != null)
						{
							IDKin.IM.Core.Message[] messages = this.GetMessageRoster(true);
							if (messages != null && messages.Length > 0)
							{
								IDKin.IM.Core.Message[] array = messages;
								for (int i = 0; i < array.Length; i++)
								{
									IDKin.IM.Core.Message message = array[i];
									message.FromJid = this.sessionService.Jid;
									message.ToJid = this.roster.Jid;
									this.AddMessageRoster(message, true);
									this.viewModel.SendMessageRoster(message);
								}
							}
						}
						else
						{
							if (this.coopStaff != null)
							{
								IDKin.IM.Core.Message[] messages = this.GetMessageCooperationStaff(true);
								if (messages != null && messages.Length > 0)
								{
									IDKin.IM.Core.Message[] array = messages;
									for (int i = 0; i < array.Length; i++)
									{
										IDKin.IM.Core.Message message = array[i];
										message.FromJid = this.sessionService.Jid;
										message.ToJid = this.coopStaff.Jid;
										message.ProjectId = this.coopStaff.UnitedProjectid;
										this.AddCooperationMessageStaff(message, true);
										this.viewModel.SendCooperationMessageRequest(message);
									}
								}
							}
						}
					}
				}
			}
			else
			{
				System.Windows.MessageBox.Show("离线状态下不能发送消息");
			}
			this.inputMsgBox.Focus();
		}
		private void AddMessageCenterChatInfo(string name, long id, MessageCenterType type, string lasttime)
		{
			if (!string.IsNullOrEmpty(name) && id > 0L && !string.IsNullOrEmpty(lasttime))
			{
				MessageCenterChatInfo messageCenterInfo = new MessageCenterChatInfo();
				messageCenterInfo.Name = name;
				messageCenterInfo.Id = id;
				messageCenterInfo.Type = type;
				messageCenterInfo.LastTime = lasttime;
				LocalDataUtil.Instance.AddChatInfo(messageCenterInfo);
			}
		}
		private string GetDepartmentName(long id)
		{
			Department department = this.dataService.GetDepartment(id);
			string result;
			if (department != null)
			{
				result = "(" + department.Name + ")";
			}
			else
			{
				result = "";
			}
			return result;
		}
		public void AddOpenFileControl(OpenFileControl openFile, string sendeName)
		{
			if (openFile != null && !string.IsNullOrEmpty(sendeName))
			{
				Paragraph p = new Paragraph();
				FlowDocument doc = this.viewMsgBox.Document;
				doc.Blocks.Add(this.GetNameInfo(sendeName, this.sessionService.CalculateSystemTime()));
				p.Inlines.Add(openFile);
				doc.Blocks.Add(p);
			}
		}
		public void AddMessageNotice(string message)
		{
			Paragraph paragraph = new Paragraph();
			paragraph.FontSize = 12.0;
			paragraph.FontFamily = new FontFamily("宋体");
			paragraph.Foreground = new SolidColorBrush(Color.FromRgb(80, 80, 80));
			paragraph.Inlines.Add(new Run("提示:" + message));
			this.viewMsgBox.Document.Blocks.Add(paragraph);
			this.ScroolToEnd();
		}
		private void AddMessageRosterChat(IDKin.IM.Core.Message message, bool send = false)
		{
			try
			{
				if (send)
				{
					message.MessageString = this.utilService.MessageEncode(message.MessageBlocks);
					Block[] blocks = this.utilService.MessageDecode(message.MessageString);
					if (blocks != null && blocks.Length > 0)
					{
						FlowDocument doc = this.viewMsgBox.Document;
						if (message.FromJid == this.sessionService.Jid)
						{
							doc.Blocks.Add(this.GetNameInfo(this.sessionService.Name, this.sessionService.CalculateSystemTime(), true));
						}
						else
						{
							doc.Blocks.Add(this.GetNameInfo(this.sessionService.Name, message.CreateTime));
						}
						Block[] array = blocks;
						for (int i = 0; i < array.Length; i++)
						{
							Paragraph block = (Paragraph)array[i];
							this.SetParagraphStyle(block, message.Style);
							doc.Blocks.Add(block);
						}
					}
				}
				else
				{
					Block[] blocks = message.MessageBlocks;
					if (blocks != null && blocks.Length > 0)
					{
						FlowDocument doc = this.viewMsgBox.Document;
						Roster roster = this.dataService.GetRoster((long)((ulong)Jid.GetUid(message.FromJid)));
						if (roster != null)
						{
							if (message.FromJid == this.sessionService.Jid)
							{
								doc.Blocks.Add(this.GetNameInfo(roster.Name, this.sessionService.CalculateSystemTime(), true));
							}
							else
							{
								doc.Blocks.Add(this.GetNameInfo(roster.Name, message.CreateTime));
							}
							Block[] array = blocks;
							for (int i = 0; i < array.Length; i++)
							{
								Paragraph block = (Paragraph)array[i];
								this.SetParagraphStyle(block, message.Style);
								doc.Blocks.Add(block);
							}
						}
					}
				}
				this.ScroolToEnd();
			}
			catch (System.ArgumentException ex)
			{
				if (this.logger != null)
				{
					this.logger.Error(ex);
				}
			}
		}
		private void AddMessageGroupChat(IDKin.IM.Core.Message message, bool send = false)
		{
			try
			{
				Block[] blocks;
				if (send)
				{
					message.MessageString = this.utilService.MessageEncode(message.MessageBlocks);
					blocks = this.utilService.MessageDecode(message.MessageString);
				}
				else
				{
					blocks = message.MessageBlocks;
				}
				if (blocks != null && blocks.Length > 0)
				{
					FlowDocument doc = this.viewMsgBox.Document;
					Staff staff = this.dataService.GetStaff((long)((ulong)Jid.GetUid(message.FromJid)));
					if (staff != null)
					{
						if (message.FromJid == this.sessionService.Jid)
						{
							doc.Blocks.Add(this.GetNameInfo(staff.Name + this.GetDepartmentName(staff.DepartmentId), this.sessionService.CalculateSystemTime(), true));
						}
						else
						{
							doc.Blocks.Add(this.GetNameInfo(staff.Name + this.GetDepartmentName(staff.DepartmentId), message.CreateTime));
						}
						Block[] array = blocks;
						for (int i = 0; i < array.Length; i++)
						{
							Paragraph block = (Paragraph)array[i];
							this.SetParagraphStyle(block, message.Style);
							doc.Blocks.Add(block);
						}
						this.ScroolToEnd();
					}
				}
			}
			catch (System.ArgumentException ex)
			{
				if (this.logger != null)
				{
					this.logger.Error(ex);
				}
			}
		}
		private void ScroolToEnd()
		{
			System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(this.ScroolToEntPool), null);
		}
		private void ScroolToEntPool(object obj)
		{
			System.Threading.Thread.Sleep(100);
			base.Dispatcher.BeginInvoke(new ChatComponent.ScroolToEndDelegate(this.ScroolToEndHandle), new object[0]);
		}
		private void ScroolToEndHandle()
		{
			try
			{
				if (this.ViewMessageBoxScrollViewer == null)
				{
					DependencyObject DO = this.viewMsgBox;
					bool value = false;
					while (!value)
					{
						if (VisualTreeHelper.GetChildrenCount(DO) <= 0)
						{
							break;
						}
						DO = VisualTreeHelper.GetChild(DO, 0);
						if (DO is ScrollViewer)
						{
							this.ViewMessageBoxScrollViewer = (DO as ScrollViewer);
							value = true;
						}
					}
				}
				if (this.ViewMessageBoxScrollViewer != null)
				{
					this.ViewMessageBoxScrollViewer.ScrollToEnd();
					this.ViewMessageBoxScrollViewer.ScrollToBottom();
					this.ViewMessageBoxScrollViewer.ScrollToEnd();
					this.ViewMessageBoxScrollViewer.ScrollToBottom();
				}
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		public void AddMessageRoster(IDKin.IM.Core.Message message, bool send = false)
		{
			if (message != null)
			{
				this.AddMessageRosterChat(message, send);
			}
		}
		public void AddMessageGroup(IDKin.IM.Core.Message message, bool send = false)
		{
			if (message != null)
			{
				if (message.VOType == 0)
				{
					this.AddMessageGroupChat(message, send);
				}
			}
		}
		public IDKin.IM.Core.Message[] GetMessageGroup(bool send = false)
		{
			FlowDocument doc = this.inputMsgBox.Document;
			Block[] blocks = new Block[doc.Blocks.Count];
			doc.Blocks.CopyTo(blocks, 0);
			bool empty = this.IsMessageEmpty();
			IDKin.IM.Core.Message[] messages = null;
			IDKin.IM.Core.Message[] result;
			if (!empty)
			{
				result = this.ChatContentMaxLengthProcessor(send, doc, blocks, ref messages);
			}
			else
			{
				result = messages;
			}
			return result;
		}
		private IDKin.IM.Core.Message[] GetMessageRoster(bool send = false)
		{
			FlowDocument doc = this.inputMsgBox.Document;
			Block[] blocks = new Block[doc.Blocks.Count];
			doc.Blocks.CopyTo(blocks, 0);
			bool empty = this.IsMessageEmpty();
			IDKin.IM.Core.Message[] messages = null;
			IDKin.IM.Core.Message[] result;
			if (!empty)
			{
				result = this.ChatContentMaxLengthProcessor(send, doc, blocks, ref messages);
			}
			else
			{
				result = null;
			}
			return result;
		}
		private IDKin.IM.Core.Message[] GetMessageCooperationStaff(bool send = false)
		{
			FlowDocument doc = this.inputMsgBox.Document;
			Block[] blocks = new Block[doc.Blocks.Count];
			doc.Blocks.CopyTo(blocks, 0);
			bool empty = this.IsMessageEmpty();
			IDKin.IM.Core.Message[] messages = null;
			IDKin.IM.Core.Message[] result;
			if (!empty)
			{
				result = this.ChatContentMaxLengthProcessor(send, doc, blocks, ref messages);
			}
			else
			{
				result = null;
			}
			return result;
		}
		private IDKin.IM.Core.Message[] GetMessageStaff(bool send = false)
		{
			FlowDocument doc = this.inputMsgBox.Document;
			Block[] blocks = new Block[doc.Blocks.Count];
			doc.Blocks.CopyTo(blocks, 0);
			bool empty = this.IsMessageEmpty();
			IDKin.IM.Core.Message[] messages = null;
			IDKin.IM.Core.Message[] result;
			if (!empty)
			{
				result = this.ChatContentMaxLengthProcessor(send, doc, blocks, ref messages);
			}
			else
			{
				result = null;
			}
			return result;
		}
		private IDKin.IM.Core.Message[] ChatContentMaxLengthProcessor(bool send, FlowDocument doc, Block[] blocks, ref IDKin.IM.Core.Message[] messages)
		{
			TextRange tr = new TextRange(this.inputMsgBox.Document.ContentStart, this.inputMsgBox.Document.ContentEnd);
			if (tr.Text.Length < this.ContentMaxLength)
			{
				messages = new IDKin.IM.Core.Message[1];
				messages[0] = new IDKin.IM.Core.Message();
				messages[0].MessageBlocks = blocks;
				messages[0].Style = this.messageStyle;
			}
			else
			{
				string[] contents = this.ContentToArray(tr.Text, this.ContentMaxLength);
				if (contents != null && contents.Length > 0)
				{
					messages = new IDKin.IM.Core.Message[contents.Length];
					for (int i = 0; i < contents.Length; i++)
					{
						messages[i] = new IDKin.IM.Core.Message();
						Paragraph[] tempParagraph = new Paragraph[]
						{
							new Paragraph()
						};
						tempParagraph[0].Inlines.Add(new Run(contents[i]));
						messages[i].MessageBlocks = tempParagraph;
						messages[i].Style = this.messageStyle;
					}
				}
			}
			if (send)
			{
				doc.Blocks.Clear();
			}
			return messages;
		}
		private string[] ContentToArray(string content, int length)
		{
			string[] contents;
			if (!string.IsNullOrEmpty(content) && length > 0)
			{
				double d = (double)content.Length / (double)length;
				double modelSum = System.Math.Ceiling(d);
				contents = new string[(int)modelSum];
				int i = 0;
				while ((double)i < modelSum)
				{
					if (i * length + length < content.Length)
					{
						contents[i] = content.Substring(i * length, length);
					}
					else
					{
						if (i * length < content.Length)
						{
							contents[i] = content.Substring(i * length);
						}
					}
					i++;
				}
			}
			else
			{
				contents = new string[]
				{
					content
				};
			}
			return contents;
		}
		private bool IsMessageEmpty()
		{
			bool empty = true;
			FlowDocument doc = this.inputMsgBox.Document;
			Block[] blocks = new Block[doc.Blocks.Count];
			doc.Blocks.CopyTo(blocks, 0);
			bool result;
			if (blocks.Length >= 1)
			{
				Block[] array = blocks;
				for (int i = 0; i < array.Length; i++)
				{
					Block block = array[i];
					Paragraph paragraph = block as Paragraph;
					if (paragraph != null)
					{
						foreach (Inline inline in paragraph.Inlines)
						{
							if (inline is Run)
							{
								TextRange tr = new TextRange(paragraph.ContentStart, paragraph.ContentEnd);
								if (tr.Text.Length > 0)
								{
									empty = (result = false);
									return result;
								}
							}
							if (inline is Span)
							{
								TextRange tr = new TextRange(paragraph.ContentStart, paragraph.ContentEnd);
								if (tr.Text.Length > 0)
								{
									empty = (result = false);
									return result;
								}
							}
							if (inline is InlineUIContainer)
							{
								InlineUIContainer ui = inline as InlineUIContainer;
								Image image = ui.Child as Image;
								System.Windows.Controls.UserControl imageControl = ui.Child as System.Windows.Controls.UserControl;
								if (image != null)
								{
									empty = (result = false);
									return result;
								}
								if (imageControl != null)
								{
									empty = (result = false);
									return result;
								}
							}
						}
					}
					else
					{
						BlockUIContainer uiContiainer = block as BlockUIContainer;
						if (uiContiainer != null)
						{
							result = false;
							return result;
						}
						Table table = block as Table;
						if (table != null && table.RowGroups.Count > 0)
						{
							result = false;
							return result;
						}
					}
				}
			}
			else
			{
				empty = true;
			}
			result = empty;
			return result;
		}
		public void AddMessageInputMsgBox(string message)
		{
			try
			{
				if (!string.IsNullOrEmpty(message))
				{
					Paragraph p = new Paragraph();
					Run run = new Run(message);
					p.Inlines.Add(run);
					this.SetParagraphStyle(p, this.messageStyle);
					this.inputMsgBox.Document.Blocks.Add(p);
				}
			}
			catch (System.Exception e)
			{
				this.logger.Error(e.ToString());
			}
		}
		public void AddMessageStaff(IDKin.IM.Core.Message message, bool send = false)
		{
			try
			{
				if (message != null)
				{
					if (message.VOType == 0)
					{
						this.AddMessageStaffChat(message, send);
					}
					if (message.VOType == 1)
					{
						this.AddMessageAutoReply(message);
					}
					if (message.VOType == 2)
					{
						message.MessageString = "对方已成功接收文件" + message.MessageString;
						this.AddMessageChatNotice(message);
					}
					if (message.VOType == 4)
					{
						message.MessageString = "对方文件" + message.MessageString + "接收失败";
						this.AddMessageChatNotice(message);
					}
					if (message.VOType == 6)
					{
					}
					if (message.VOType == 3)
					{
					}
					if (message.VOType == 5)
					{
						message.MessageString = "对方已拒绝接收您的文件" + message.MessageString;
						this.AddMessageChatNotice(message);
					}
				}
			}
			catch (System.Exception)
			{
				this.AddMessageNotice("发送聊天消息失败.");
			}
		}
		public void AddCooperationMessageStaff(IDKin.IM.Core.Message message, bool send = false)
		{
			try
			{
				if (message != null)
				{
					if (message.VOType == 0)
					{
						this.AddMessageCooperationStaffChat(message, send);
					}
					if (message.VOType == 1)
					{
						this.AddCooperationMessageAutoReply(message);
					}
					if (message.VOType == 2)
					{
						message.MessageString = "对方已成功接收文件" + message.MessageString;
						this.AddCooperationMessageChatNotice(message);
					}
					if (message.VOType == 4)
					{
						message.MessageString = "对方文件" + message.MessageString + "接收失败";
						this.AddCooperationMessageChatNotice(message);
					}
					if (message.VOType == 6)
					{
					}
					if (message.VOType == 3)
					{
					}
					if (message.VOType == 5)
					{
						message.MessageString = "对方已拒绝接收您的文件" + message.MessageString;
						this.AddCooperationMessageChatNotice(message);
					}
					if (message.VOType == 7)
					{
						message.MessageString = this.coopStaff.Name + "给你发送的文件:" + message.MessageString + "在传输中.请耐心等待!";
						this.AddCooperationMessageChatNotice(message);
					}
				}
			}
			catch (System.Exception)
			{
				this.AddMessageNotice("发送聊天消息失败.");
			}
		}
		private void AddMessageAutoReply(IDKin.IM.Core.Message message)
		{
			FlowDocument doc = this.viewMsgBox.Document;
			Staff staff = this.dataService.GetStaff((long)((ulong)Jid.GetUid(message.FromJid)));
			if (staff != null)
			{
				doc.Blocks.Add(this.GetNameInfo(staff.Name, message.CreateTime));
				Paragraph paragraph = new Paragraph();
				paragraph.FontSize = 12.0;
				paragraph.LineHeight = 15.0;
				paragraph.FontFamily = new FontFamily("宋体");
				paragraph.Foreground = new SolidColorBrush(Color.FromRgb(80, 80, 80));
				paragraph.Inlines.Add(new Run("提示:" + message.MessageString));
				this.viewMsgBox.Document.Blocks.Add(paragraph);
				this.ScroolToEnd();
			}
		}
		private void AddCooperationMessageAutoReply(IDKin.IM.Core.Message message)
		{
			FlowDocument doc = this.viewMsgBox.Document;
			CooperationStaff staff = this.dataService.GetCooperationStaff((long)((ulong)Jid.GetUid(message.FromJid)), message.ProjectId);
			if (staff != null)
			{
				doc.Blocks.Add(this.GetNameInfo(staff.Name, message.CreateTime));
				Paragraph paragraph = new Paragraph();
				paragraph.FontSize = 12.0;
				paragraph.LineHeight = 15.0;
				paragraph.FontFamily = new FontFamily("宋体");
				paragraph.Foreground = new SolidColorBrush(Color.FromRgb(80, 80, 80));
				paragraph.Inlines.Add(new Run("提示:" + message.MessageString));
				this.viewMsgBox.Document.Blocks.Add(paragraph);
				this.ScroolToEnd();
			}
		}
		private void AddMessageStaffChat(IDKin.IM.Core.Message message, bool send = false)
		{
			Block[] blocks;
			if (send)
			{
				message.MessageString = this.utilService.MessageEncode(message.MessageBlocks);
				blocks = this.utilService.MessageDecode(message.MessageString);
			}
			else
			{
				blocks = message.MessageBlocks;
			}
			if (blocks != null && blocks.Length > 0)
			{
				FlowDocument doc = this.viewMsgBox.Document;
				Staff staff = this.dataService.GetStaff((long)((ulong)Jid.GetUid(message.FromJid)));
				if (staff != null)
				{
					if (message.FromJid == this.sessionService.Jid)
					{
						doc.Blocks.Add(this.GetNameInfo(staff.Name, this.sessionService.CalculateSystemTime(), true));
					}
					else
					{
						doc.Blocks.Add(this.GetNameInfo(staff.Name, message.CreateTime));
					}
					Block[] array = blocks;
					for (int i = 0; i < array.Length; i++)
					{
						Paragraph block = (Paragraph)array[i];
						this.SetParagraphStyle(block, message.Style);
						doc.Blocks.Add(block);
					}
					this.ScroolToEnd();
				}
			}
		}
		private void SetParagraphStyle(Paragraph para, MessageStyle style)
		{
			try
			{
				para.FontSize = (double)this.messageStyle.FontSizeList[style.FontSize];
			}
			catch (System.IndexOutOfRangeException ex)
			{
				if (this.logger != null)
				{
					this.logger.Error(ex);
				}
				style.FontSize = 1;
				para.FontSize = 12.0;
			}
			try
			{
				para.FontFamily = new FontFamily(this.messageStyle.FontFamilyList[style.FontFamily]);
			}
			catch (System.IndexOutOfRangeException ex)
			{
				if (this.logger != null)
				{
					this.logger.Error(ex);
				}
				style.FontFamily = 0;
				para.FontFamily = new FontFamily(this.messageStyle.FontFamilyList[0]);
			}
			if (style.Bold == 1)
			{
				para.FontWeight = FontWeights.Bold;
			}
			else
			{
				para.FontWeight = FontWeights.Normal;
			}
			if (style.Italic == 1)
			{
				para.FontStyle = FontStyles.Italic;
			}
			else
			{
				para.FontStyle = FontStyles.Normal;
			}
			para.Foreground = new SolidColorBrush(Color.FromArgb(255, style.FontColorR, style.FontColorG, style.FontColorB));
		}
		public void InsertImage(Image image)
		{
			image.Stretch = Stretch.None;
			System.Windows.Controls.UserControl imageControl = UIControlUtil.Instance.CreateImageControl(image, this.sessionService.ChatViewMsgBoxWidth);
			new InlineUIContainer(imageControl, this.inputMsgBox.Selection.End);
			this.inputMsgBox.Focus();
		}
		public void InsertAnimatedImage(AnimatedImage animateImage)
		{
			try
			{
				if (animateImage != null)
				{
					animateImage.Stretch = Stretch.None;
					new InlineUIContainer(animateImage, this.inputMsgBox.Selection.End);
					this.inputMsgBox.Focus();
					this.inputMsgBox.ScrollToEnd();
				}
			}
			catch (System.Exception e)
			{
				this.logger.Error(e.ToString());
			}
		}
		private void AddMessageChatNotice(IDKin.IM.Core.Message message)
		{
			if (message != null)
			{
				Paragraph paragraph = new Paragraph();
				paragraph.FontSize = 12.0;
				paragraph.LineHeight = 15.0;
				paragraph.FontFamily = new FontFamily("宋体");
				paragraph.Foreground = new SolidColorBrush(Color.FromRgb(80, 80, 80));
				paragraph.Inlines.Add(new Run("提示:" + message.MessageString));
				this.viewMsgBox.Document.Blocks.Add(paragraph);
				this.ScroolToEnd();
			}
		}
		private void AddCooperationMessageChatNotice(IDKin.IM.Core.Message message)
		{
			if (message != null)
			{
				Paragraph paragraph = new Paragraph();
				paragraph.FontSize = 12.0;
				paragraph.LineHeight = 15.0;
				paragraph.FontFamily = new FontFamily("宋体");
				paragraph.Foreground = new SolidColorBrush(Color.FromRgb(80, 80, 80));
				paragraph.Inlines.Add(new Run("提示:" + message.MessageString));
				this.viewMsgBox.Document.Blocks.Add(paragraph);
				this.ScroolToEnd();
			}
		}
		private Paragraph GetNameInfo(string name, string createTime)
		{
			return this.GetNameInfo(name, createTime, false);
		}
		private Paragraph GetNameInfo(string name, string createTime, bool sender)
		{
			Run text = new Run
			{
				Text = name + "  " + createTime
			};
			text.FontSize = 12.0;
			text.FontFamily = new FontFamily("宋体");
			if (sender)
			{
				text.Foreground = Brushes.Blue;
			}
			else
			{
				text.Foreground = Brushes.Green;
			}
			return new Paragraph
			{
				Margin = new Thickness(5.0, 15.0, 5.0, 0.0),
				Inlines = 
				{
					text
				}
			};
		}
		private void ShowFacePanelHandler(object sender, RoutedEventArgs e)
		{
			if (this.facePanel == null)
			{
				this.facePanel = new FacePanel(this);
				this.facePanel.Height = 220.0;
				this.facePanel.Width = 380.0;
			}
			this.facePanel.PlacementTarget = this.btnFace;
			this.facePanel.Placement = PlacementMode.Top;
			this.facePanel.StaysOpen = false;
			this.facePanel.IsOpen = true;
		}
		private void SetFontHandler(object sender, RoutedEventArgs e)
		{
			if (this.FontRow.Height == new GridLength(0.0))
			{
				this.GridRowToolBar.Height = new GridLength(50.0);
				this.FontRow.Height = new GridLength(25.0);
				this.fontToolBar.Visibility = Visibility.Visible;
			}
			else
			{
				this.GridRowToolBar.Height = new GridLength(25.0);
				this.FontRow.Height = new GridLength(0.0);
				this.fontToolBar.Visibility = Visibility.Collapsed;
			}
		}
		private void SetFontColor(byte[] colorByte)
		{
			if (colorByte != null && colorByte.Length == 4)
			{
				this.inputMsgBox.Foreground = new SolidColorBrush(Color.FromArgb(255, colorByte[1], colorByte[2], colorByte[3]));
			}
			Settings.Default.FontColorA = 255;
			Settings.Default.FontColorR = colorByte[1];
			Settings.Default.FontColorG = colorByte[2];
			Settings.Default.FontColorB = colorByte[3];
			Settings.Default.Save();
			this.messageStyle.FontColor = string.Concat(new object[]
			{
				255,
				"#",
				colorByte[1],
				"#",
				colorByte[2],
				"#",
				colorByte[3]
			});
		}
		private void SetFontStyle(bool selected)
		{
			this.messageStyle.Italic = (selected ? 1 : 0);
			this.inputMsgBox.FontStyle = (selected ? FontStyles.Italic : FontStyles.Normal);
			Settings.Default.FontStyle = this.messageStyle.Italic;
			Settings.Default.Save();
		}
		private void SetFontWeight(bool selected)
		{
			this.messageStyle.Bold = (selected ? 1 : 0);
			this.inputMsgBox.FontWeight = (selected ? FontWeights.Bold : FontWeights.Normal);
			Settings.Default.FontWeight = this.messageStyle.Bold;
			Settings.Default.Save();
		}
		private void fontSizeClick(object sender, SelectionChangedEventArgs e)
		{
			ComboBoxItem cbiSize = this.fontSize.SelectedValue as ComboBoxItem;
			if (cbiSize != null)
			{
				this.SetFontSize((int)cbiSize.DataContext);
			}
		}
		private void SetFontSize(int fontSize)
		{
			this.messageStyle.FontSize = fontSize;
			if (this.inputMsgBox != null)
			{
				this.inputMsgBox.Document.FontSize = (double)this.messageStyle.FontSizeList[this.messageStyle.FontSize];
			}
			Settings.Default.FontSize = this.messageStyle.FontSize;
			Settings.Default.Save();
		}
		private void fontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (this.fontFamily.SelectedValue != null)
			{
				ComboBoxItem cbiFontFamily = this.fontFamily.SelectedValue as ComboBoxItem;
				if (cbiFontFamily != null)
				{
					this.SetFontFamily((int)cbiFontFamily.DataContext);
				}
			}
		}
		private void SetFontFamily(int fontFamily)
		{
			this.messageStyle.FontFamily = fontFamily;
			this.inputMsgBox.Document.FontFamily = new FontFamily(this.messageStyle.FontFamilyList[this.messageStyle.FontFamily]);
			Settings.Default.FontFamily = this.messageStyle.FontFamily;
			Settings.Default.Save();
		}
		private void boldButtonClick(object sender, RoutedEventArgs e)
		{
			this.SetFontWeight(this.boldButton.IsChecked.Equals(true));
		}
		private void italicButtonClick(object sender, RoutedEventArgs e)
		{
			this.SetFontStyle(this.italicButton.IsChecked.Equals(true));
		}
		private void underlineButtonClick(object sender, RoutedEventArgs e)
		{
		}
		private void btnMsgRecord_Click(object sender, RoutedEventArgs e)
		{
			if (this.btnMsgRecord.IsChecked == true)
			{
				this.sessionService.IsChatPanelRecord = true;
				this.PanelChangeColumn.Width = new GridLength(0.0, GridUnitType.Auto);
				this.MsgRecordComp.Visibility = Visibility.Visible;
				if (this.staff != null)
				{
					this.MsgRecordComp.Type = MessageActorType.EntStaff;
					this.MsgRecordComp.Id = this.staff.Uid;
					this.viewModel.SendStaffMessageRecord(this.sessionService.Uid, this.staff.Uid, "0", 1, 10, MessageRecordType.MESSAGE_CHAT_RECORD);
				}
				else
				{
					if (this.group != null)
					{
						this.MsgRecordComp.Type = MessageActorType.EntGroup;
						this.MsgRecordComp.Id = this.group.Gid;
						this.viewModel.SendGroupMessageRecord(this.group.Gid, "0", 1, 10, this.sessionService.Uid, MessageRecordType.MESSAGE_CHAT_RECORD);
					}
					else
					{
						if (this.coopStaff != null)
						{
							this.MsgRecordComp.Type = MessageActorType.CooperationStaff;
							this.MsgRecordComp.Id = this.coopStaff.Uid;
							this.MsgRecordComp.CoopStaff = this.coopStaff;
							this.viewModel.SendCooperationMessageRecordRequest(this.sessionService.Uid, this.coopStaff.Uid, this.coopStaff.UnitedProjectid, "0", 1, 10, MessageRecordType.MESSAGE_CHAT_RECORD);
						}
					}
				}
			}
			else
			{
				this.MsgRecordComp.Visibility = Visibility.Collapsed;
				this.MsgRecordComp.InitPage();
				this.MsgRecordComp.Clear();
			}
			if (this.MsgRecordStatus != null)
			{
				this.MsgRecordStatus(this.btnMsgRecord.IsChecked == true);
			}
			this.sessionService.IsDateSearch = false;
		}
		public void AddGroupMessageRecords(IDKin.IM.Core.Message[] messages, int total)
		{
			if (messages != null && this.MsgRecordComp != null)
			{
				this.MsgRecordComp.Clear();
				this.MsgRecordComp.Total = total;
				for (int i = messages.Length - 1; i >= 0; i--)
				{
					this.MsgRecordComp.AddMessageRecord(messages[i]);
				}
				this.MsgRecordComp.ViewPageButton();
			}
		}
		public void AddMessageRecords(IDKin.IM.Core.Message[] messages, int total)
		{
			try
			{
				if (messages != null && this.MsgRecordComp != null)
				{
					this.MsgRecordComp.Clear();
					this.MsgRecordComp.Total = total;
					for (int i = messages.Length - 1; i >= 0; i--)
					{
						this.MsgRecordComp.AddMessageRecord(messages[i]);
					}
					this.MsgRecordComp.ViewPageButton();
				}
			}
			catch (System.Exception e)
			{
				System.Console.WriteLine(e.ToString());
			}
		}
		public void AddCooperationStaffMessageRecords(IDKin.IM.Core.Message[] messages, int total)
		{
			try
			{
				if (messages != null && this.MsgRecordComp != null)
				{
					this.MsgRecordComp.Clear();
					this.MsgRecordComp.Total = total;
					for (int i = messages.Length - 1; i >= 0; i--)
					{
						this.MsgRecordComp.AddCooperationStaffMessageRecord(messages[i]);
					}
					this.MsgRecordComp.ViewPageButton();
				}
			}
			catch (System.Exception e)
			{
				System.Console.WriteLine(e.ToString());
			}
		}
		private void btnImage_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
				dialog.Filter = "图像文件（*.bmp;*.jpg;*.png;*.gif）|*.bmp;*.jpg;*.png;*.gif";
				dialog.FilterIndex = 0;
				dialog.RestoreDirectory = true;
				dialog.Multiselect = false;
				dialog.ShowReadOnly = true;
				if (dialog.ShowDialog() == true)
				{
					string filepath = dialog.FileName;
					string img = this.FileNameProcessor(filepath);
					if (!string.IsNullOrEmpty(img))
					{
						this.AddUpLoadHandle(img);
						Image image = new Image();
						string file = this.sessionService.DirImage + img;
						image.Source = new BitmapImage(new Uri(file, UriKind.Relative));
						this.InsertImage(image);
					}
				}
			}
			catch (System.NotSupportedException ex)
			{
				this.logger.Error(ex.ToString());
				System.Windows.MessageBox.Show("不能打开此图片");
			}
			catch (System.Exception ex2)
			{
				this.logger.Error(ex2.ToString());
			}
		}
		private void btnClearScreen_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				this.viewMsgBox.Document.Blocks.Clear();
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		private void btnShake_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (this.coopStaff != null)
				{
					this.SendCooperationVibration();
				}
				else
				{
					this.SendVibration();
				}
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		private void SendVibration()
		{
			if (this.staff.Status != UserStatus.Hide && this.staff.Status != UserStatus.Offline)
			{
				bool flag = 0 == 0;
				System.TimeSpan ts2 = new System.TimeSpan(System.DateTime.Now.Ticks);
				if (ts2.Subtract(this.upVibrationTime).Duration().TotalSeconds > 20.0)
				{
					this.AddMessageNotice("您发送了一个窗口抖动！");
					this.Vibration(false);
					if (this.staff != null)
					{
						this.viewModel.SendVibration(this.sessionService.Jid, this.staff.Jid);
					}
				}
				else
				{
					this.AddMessageNotice("请休息一下，喝口水，过于频繁的抖动会累着的！");
				}
			}
			else
			{
				this.AddMessageNotice("对方不在线，无法发送抖动！");
			}
		}
		private void SendCooperationVibration()
		{
			if (this.coopStaff.Status != UserStatus.Hide && this.coopStaff.Status != UserStatus.Offline)
			{
				bool flag = 0 == 0;
				System.TimeSpan ts2 = new System.TimeSpan(System.DateTime.Now.Ticks);
				if (ts2.Subtract(this.upVibrationTime).Duration().TotalSeconds > 20.0)
				{
					this.AddMessageNotice("您发送了一个窗口抖动！");
					this.Vibration(false);
					if (this.coopStaff != null)
					{
						this.viewModel.SendCooperationVibration(this.sessionService.Jid, this.coopStaff.Jid, this.coopStaff.UnitedProjectid);
					}
				}
				else
				{
					this.AddMessageNotice("请休息一下，喝口水，过于频繁的抖动会累着的！");
				}
			}
			else
			{
				this.AddMessageNotice("对方不在线，无法发送抖动！");
			}
		}
		public void Vibration(bool noCheck)
		{
			if (!noCheck)
			{
				this.upVibrationTime = new System.TimeSpan(System.DateTime.Now.Ticks);
			}
			INWindow inWindow = this.dataService.INWindow as INWindow;
			if (inWindow != null)
			{
				inWindow.Topmost = true;
				bool lab = true;
				for (int i = 0; i < 10; i++)
				{
					if (lab)
					{
						inWindow.Top += 5.0;
						inWindow.Left += 5.0;
						lab = false;
					}
					else
					{
						inWindow.Top -= 5.0;
						inWindow.Left -= 5.0;
						lab = true;
					}
					System.Threading.Thread.Sleep(40);
				}
				inWindow.Topmost = false;
			}
		}
		private void btnSend_PolygonClick(object sender, RoutedEventArgs e)
		{
			if (this.sendStylePopup == null)
			{
				this.sendStylePopup = new SendStylePopup();
				this.sendStylePopup.Width = 170.0;
				this.sendStylePopup.Height = 85.0;
				this.sendStylePopup.Closed += new System.EventHandler(this.sendStylePopup_Closed);
			}
            //this.sendStylePopup.PlacementTarget = this.btnSend;
			this.sendStylePopup.Placement = PlacementMode.Bottom;
			this.sendStylePopup.StaysOpen = false;
			this.sendStylePopup.IsOpen = true;
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
				if (this.staff != null || this.group != null)
				{
					this.fileService.UploadImage(fileName);
				}
				if (this.roster != null)
				{
					this.fileService.UploadImage(fileName, true);
				}
				if (this.coopStaff != null)
				{
					this.fileService.UploadImage(fileName, true);
				}
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
					name = ChatComponent.GenerateFileName(fileName);
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
				this.logger.Error(e.ToString());
			}
			return name;
		}
		public static string GenerateFileName(string fileName)
		{
			System.IO.FileStream file = null;
			string name = string.Empty;
			try
			{
				if (System.IO.File.Exists(fileName))
				{
					file = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
					name = ChatComponent.MD5Stream(file) + System.IO.Path.GetExtension(fileName);
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
		public static string MD5Stream(System.IO.Stream stream)
		{
			System.Security.Cryptography.MD5CryptoServiceProvider md5 = null;
			string resule = string.Empty;
			if (stream != null)
			{
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
			}
			stream.Close();
			return resule;
		}
		private void imgHide_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				this.ShowRightBoxHandler();
			}
		}
		private void ViewDrag_Click(object sender, System.Windows.DragEventArgs e)
		{
		}
		private void ShowRightBoxHandler()
		{
			if (this.PanelChangeColumn.Width == new GridLength(0.0))
			{
				this.imgHide.ToolTip = "隐藏侧边";
				this.imgHide.Source = this.imageService.GetIcon(ImageTypeIcon.LHide);
				this.PanelChangeColumn.Width = new GridLength(0.0, GridUnitType.Auto);
			}
			else
			{
				this.imgHide.ToolTip = "显示侧边";
				this.imgHide.Source = this.imageService.GetIcon(ImageTypeIcon.LShow);
				this.PanelChangeColumn.Width = new GridLength(0.0);
			}
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
						GroupChatTabControl gctc = item.Content as GroupChatTabControl;
						FriendsChatTabControl fctc = item.Content as FriendsChatTabControl;
						if (pctc != null && this.staff != null && pctc.StaffId == this.staff.Uid)
						{
							result = cti;
							return result;
						}
						if (gctc != null && this.group != null && gctc.GroupId == this.group.Gid)
						{
							result = cti;
							return result;
						}
						if (fctc != null && this.roster != null && fctc.RosterId == this.roster.Uid)
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
		private void HideBorder_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
		{
			DoubleAnimation animate = new DoubleAnimation();
			animate.FillBehavior = FillBehavior.HoldEnd;
			animate.From = new double?(0.0);
			animate.To = new double?(1.0);
			animate.Duration = new Duration(System.TimeSpan.FromSeconds(0.8));
			this.imgHide.BeginAnimation(UIElement.OpacityProperty, animate);
		}
		private void HideBorder_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
		{
			DoubleAnimation animate = new DoubleAnimation();
			animate.FillBehavior = FillBehavior.HoldEnd;
			animate.From = new double?(1.0);
			animate.To = new double?(0.0);
			animate.Duration = new Duration(System.TimeSpan.FromSeconds(0.8));
			this.imgHide.BeginAnimation(UIElement.OpacityProperty, animate);
		}
		private void btnGroupShield_Click(object sender, RoutedEventArgs e)
		{
			this.groupShieldMenu.HasDropShadow = true;
			this.groupShieldMenu.PlacementTarget = this.btnGroupShield;
			this.groupShieldMenu.Placement = PlacementMode.Bottom;
			this.groupShieldMenu.IsOpen = true;
		}
		private void menuShield_Click(object sender, RoutedEventArgs e)
		{
			if ((this.menuShield.IsChecked ? 1 : 0) != 0)
			{
				this.SaveShieldStatus();
			}
			else
			{
				this.CancelShileStatus();
			}
		}
		private void SaveShieldStatus()
		{
			string groups = Settings.Default.ShieldGroups;
			if (-1 == groups.IndexOf("#" + this.group.Gid + ";"))
			{
				groups = string.Concat(new object[]
				{
					groups,
					"#",
					this.group.Gid,
					";"
				});
				Settings.Default.ShieldGroups = groups;
				Settings.Default.Save();
			}
			this.SetItemGroupShield(true);
		}
		private void CancelShileStatus()
		{
			string groups = Settings.Default.ShieldGroups;
			groups = groups.Replace("#" + this.group.Gid + ";", string.Empty);
			Settings.Default.ShieldGroups = groups;
			Settings.Default.Save();
			this.SetItemGroupShield(false);
		}
		private void SetItemGroupShield(bool isShield)
		{
			INWindow inWindow = this.dataService.INWindow as INWindow;
			if (inWindow != null && this.group != null)
			{
				ItemGroup group = inWindow.Employee.listEntGroup.FindItemGroup(this.group.Gid);
				group.IsShield = isShield;
			}
		}
		public void ShowViewMsgTipGrid(string title, string url)
		{
			this.viewMsgTipGrid.Height = 30.0;
			this.viewMsgTipGrid.Visibility = Visibility.Visible;
			this.tbkMsgTip1.Text = "您正在通过协作与对方进行交流，所有聊天记录将自动保存至“";
			this.tbkMsgTip2.Text = title;
			this.tbkMsgTip2.Tag = url;
			this.tbkMsgTip3.Text = "”中。";
			this.tbkMsgTip2.MouseLeftButtonDown += new MouseButtonEventHandler(this.tbkMsgTip2_MouseLeftButtonDown);
		}
		private void tbkMsgTip2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			BrowserUtil.OpenHyperlinkHandler((sender as TextBlock).Tag.ToString());
		}
		public void InitData(CooperationStaff coopStaff)
		{
			this.coopStaff = coopStaff;
			this.InitService();
		}
		private void AddMessageCooperationStaffChat(IDKin.IM.Core.Message message, bool send = false)
		{
			try
			{
				if (send)
				{
					message.MessageString = this.utilService.MessageEncode(message.MessageBlocks);
					Block[] blocks = this.utilService.MessageDecode(message.MessageString);
					if (blocks != null && blocks.Length > 0)
					{
						FlowDocument doc = this.viewMsgBox.Document;
						if (message.FromJid == this.sessionService.Jid)
						{
							doc.Blocks.Add(this.GetNameInfo(this.sessionService.Name, this.sessionService.CalculateSystemTime(), true));
						}
						else
						{
							doc.Blocks.Add(this.GetNameInfo(this.sessionService.Name, message.CreateTime));
						}
						Block[] array = blocks;
						for (int i = 0; i < array.Length; i++)
						{
							Paragraph block = (Paragraph)array[i];
							this.SetParagraphStyle(block, message.Style);
							doc.Blocks.Add(block);
						}
					}
				}
				else
				{
					Block[] blocks = message.MessageBlocks;
					if (blocks != null && blocks.Length > 0)
					{
						FlowDocument doc = this.viewMsgBox.Document;
						CooperationStaff staff = this.dataService.GetCooperationStaff((long)((ulong)Jid.GetUid(message.FromJid)), message.ProjectId);
						if (staff != null)
						{
							if (message.FromJid == this.sessionService.Jid)
							{
								doc.Blocks.Add(this.GetNameInfo(staff.Name, this.sessionService.CalculateSystemTime(), true));
							}
							else
							{
								doc.Blocks.Add(this.GetNameInfo(staff.Name, message.CreateTime));
							}
							Block[] array = blocks;
							for (int i = 0; i < array.Length; i++)
							{
								Paragraph block = (Paragraph)array[i];
								this.SetParagraphStyle(block, message.Style);
								doc.Blocks.Add(block);
							}
						}
					}
				}
				this.ScroolToEnd();
			}
			catch (System.ArgumentException ex)
			{
				if (this.logger != null)
				{
					this.logger.Error(ex);
				}
			}
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/chatcomponent.xaml", UriKind.Relative);
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
        //        this.ChatColumn = (ColumnDefinition)target;
        //        break;
        //    case 2:
        //        this.HideColumn = (ColumnDefinition)target;
        //        break;
        //    case 3:
        //        this.PanelChangeColumn = (ColumnDefinition)target;
        //        break;
        //    case 4:
        //        this.GridRowToolBar = (RowDefinition)target;
        //        break;
        //    case 5:
        //        this.viewMsgTipGrid = (Grid)target;
        //        break;
        //    case 6:
        //        this.tbkMsgTip1 = (TextBlock)target;
        //        break;
        //    case 7:
        //        this.tbkMsgTip2 = (TextBlock)target;
        //        break;
        //    case 8:
        //        this.tbkMsgTip3 = (TextBlock)target;
        //        break;
        //    case 9:
        //        this.viewMsgBox = (FlowDocumentScrollViewer)target;
        //        this.viewMsgBox.ContextMenuOpening += new ContextMenuEventHandler(this.ViewMsgBox_ContextMenuOpening);
        //        break;
        //    case 10:
        //        this.menuViewMsgBox = (System.Windows.Controls.ContextMenu)target;
        //        break;
        //    case 11:
        //        this.ViewerMenuItemImageSaveAs = (System.Windows.Controls.MenuItem)target;
        //        this.ViewerMenuItemImageSaveAs.Click += new RoutedEventHandler(this.ViewerMenuItemImageSaveAs_Click);
        //        break;
        //    case 12:
        //        ((CommandBinding)target).Executed += new ExecutedRoutedEventHandler(this.CommandCopy);
        //        break;
        //    case 13:
        //        this.FontRow = (RowDefinition)target;
        //        break;
        //    case 14:
        //        this.fontToolBar = (System.Windows.Controls.Primitives.StatusBar)target;
        //        break;
        //    case 15:
        //        this.fontFamily = (System.Windows.Controls.ComboBox)target;
        //        break;
        //    case 16:
        //        this.fontSize = (System.Windows.Controls.ComboBox)target;
        //        break;
        //    case 17:
        //        this.boldButton = (ToggleButton)target;
        //        this.boldButton.Click += new RoutedEventHandler(this.boldButtonClick);
        //        break;
        //    case 18:
        //        this.btnFontB = (Image)target;
        //        break;
        //    case 19:
        //        this.italicButton = (ToggleButton)target;
        //        this.italicButton.Click += new RoutedEventHandler(this.italicButtonClick);
        //        break;
        //    case 20:
        //        this.btnFontI = (Image)target;
        //        break;
        //    case 21:
        //        this.underlineButton = (ToggleButton)target;
        //        this.underlineButton.Click += new RoutedEventHandler(this.underlineButtonClick);
        //        break;
        //    case 22:
        //        this.btnFontU = (Image)target;
        //        break;
        //    case 23:
        //        this.colorButton = (System.Windows.Controls.Button)target;
        //        this.colorButton.Click += new RoutedEventHandler(this.ShowColorDialog);
        //        break;
        //    case 24:
        //        this.btnFace = (System.Windows.Controls.Button)target;
        //        this.btnFace.Click += new RoutedEventHandler(this.ShowFacePanelHandler);
        //        break;
        //    case 25:
        //        this.imgFace = (Image)target;
        //        break;
        //    case 26:
        //        this.btnShake = (System.Windows.Controls.Button)target;
        //        this.btnShake.Click += new RoutedEventHandler(this.btnShake_Click);
        //        break;
        //    case 27:
        //        this.imgShake = (Image)target;
        //        break;
        //    case 28:
        //        this.btnImage = (System.Windows.Controls.Button)target;
        //        this.btnImage.Click += new RoutedEventHandler(this.btnImage_Click);
        //        break;
        //    case 29:
        //        this.togbtnFont = (ToggleButton)target;
        //        this.togbtnFont.Click += new RoutedEventHandler(this.SetFontHandler);
        //        break;
        //    case 30:
        //        this.imgFont = (Image)target;
        //        break;
        //    case 31:
        //        this.btnClearScreen = (System.Windows.Controls.Button)target;
        //        this.btnClearScreen.Click += new RoutedEventHandler(this.btnClearScreen_Click);
        //        break;
        //    case 32:
        //        this.btnGroupShield = (System.Windows.Controls.Button)target;
        //        this.btnGroupShield.Click += new RoutedEventHandler(this.btnGroupShield_Click);
        //        break;
        //    case 33:
        //        this.groupShieldMenu = (System.Windows.Controls.ContextMenu)target;
        //        break;
        //    case 34:
        //        this.menuShield = (System.Windows.Controls.MenuItem)target;
        //        this.menuShield.Click += new RoutedEventHandler(this.menuShield_Click);
        //        break;
        //    case 35:
        //        this.btnMsgRecord = (ToggleButton)target;
        //        this.btnMsgRecord.Click += new RoutedEventHandler(this.btnMsgRecord_Click);
        //        break;
        //    case 36:
        //        this.inputMsgBox = (System.Windows.Controls.RichTextBox)target;
        //        this.inputMsgBox.ContextMenuOpening += new ContextMenuEventHandler(this.InputMsgBox_ContextMenuOpening);
        //        break;
        //    case 37:
        //        this.MenuItemImageSaveAs = (System.Windows.Controls.MenuItem)target;
        //        this.MenuItemImageSaveAs.Click += new RoutedEventHandler(this.MenuItemImageSaveAs_Click);
        //        break;
        //    case 38:
        //        ((CommandBinding)target).Executed += new ExecutedRoutedEventHandler(this.CommandPaste);
        //        break;
        //    case 39:
        //        ((CommandBinding)target).Executed += new ExecutedRoutedEventHandler(this.inputMsgBoxCommandCopy);
        //        break;
        //    case 40:
        //        this.btnClose = (System.Windows.Controls.Button)target;
        //        break;
        //    case 41:
        //        this.btnSend = (SplitButton)target;
        //        break;
        //    case 42:
        //        this.HideBorder = (Border)target;
        //        this.HideBorder.MouseEnter += new System.Windows.Input.MouseEventHandler(this.HideBorder_MouseEnter);
        //        this.HideBorder.MouseLeave += new System.Windows.Input.MouseEventHandler(this.HideBorder_MouseLeave);
        //        break;
        //    case 43:
        //        this.imgHide = (Image)target;
        //        this.imgHide.MouseLeftButtonDown += new MouseButtonEventHandler(this.imgHide_MouseLeftButtonDown);
        //        break;
        //    case 44:
        //        this.PanelChangeContent = (Grid)target;
        //        break;
        //    case 45:
        //        this.MsgRecordComp = (MsgRecordComponent)target;
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
