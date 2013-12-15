using IDKin.IM.Communicate;
using IDKin.IM.Core;
using IDKin.IM.Data;
using IDKin.IM.ImageService;
using IDKin.IM.Protocol.Commons;
using IDKin.IM.Windows.Comps.FileTransport;
using IDKin.IM.Windows.Util;
using IDKin.IM.Windows.ViewModel;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
namespace IDKin.IM.Windows.Comps.MessageCenter
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public partial class MsgRecordItem : TableRow, System.IDisposable//, IComponentConnector
	{
		public long ID = 0L;
		private IDataService dataService = ServiceUtil.Instance.DataService;
		private ISessionService sessionService = ServiceUtil.Instance.SessionService;
		private IImageService imageService = ServiceUtil.Instance.ImageService;
		private MessageCenterViewModel messageCenter = new MessageCenterViewModel();
		private Staff SelStaff = null;
		private Message message = null;
		private EntGroup group = null;
		private Staff staff = null;
		private bool MouseEvent;
		private MessageStyle ms = new MessageStyle();
		//internal Image imgHead;
		//internal FlowDocumentScrollViewer MessageContent;
		//internal Image imgIsMarked;
		//private bool _contentLoaded;
		public MsgRecordItem(Message message, EntGroup gp)
		{
			this.InitializeComponent();
			Staff fromStaff = this.dataService.GetStaff((long)((ulong)Jid.GetUid(message.FromJid)));
			if (!string.IsNullOrEmpty(message.Url) && !string.IsNullOrEmpty(message.Icon) && !string.IsNullOrEmpty(message.FileName) && fromStaff != null && fromStaff.Uid == this.sessionService.Uid)
			{
				System.IO.FileInfo file = new System.IO.FileInfo(message.FileName);
				this.MessageContent.Document.Blocks.Add(this.GetNameInfo(this.sessionService.Name, message.CreateTime, true));
				this.imgHead.Source = this.sessionService.HeaderImage;
				OpenFileControl openFile = new OpenFileControl(new FileItem(file)
				{
					FileName = message.Url + message.FileName,
					IconBase64 = message.Icon
				}, true);
				if (openFile != null && !string.IsNullOrEmpty(this.sessionService.Name))
				{
					Paragraph p = new Paragraph();
					FlowDocument doc = this.MessageContent.Document;
					p.Inlines.Add(openFile);
					doc.Blocks.Add(p);
				}
			}
			else
			{
				if (!string.IsNullOrEmpty(message.Url) && !string.IsNullOrEmpty(message.Icon) && !string.IsNullOrEmpty(message.FileName) && fromStaff == null)
				{
					System.IO.FileInfo file = new System.IO.FileInfo(message.FileName);
					this.MessageContent.Document.Blocks.Add(this.GetNameInfo("此人已离职", message.CreateTime, true));
					this.imgHead.Source = this.imageService.GetHeader(ImageTypeHeader.Gary48);
					OpenFileControl openFile = new OpenFileControl(new FileItem(file)
					{
						FileName = message.Url + message.FileName,
						IconBase64 = message.Icon
					}, true);
					if (openFile != null && !string.IsNullOrEmpty(this.sessionService.Name))
					{
						Paragraph p = new Paragraph();
						FlowDocument doc = this.MessageContent.Document;
						p.Inlines.Add(openFile);
						doc.Blocks.Add(p);
					}
				}
				else
				{
					if ((long)int.Parse(message.FromJid.Substring(0, message.FromJid.IndexOf("@"))) == this.sessionService.Uid)
					{
						this.MessageContent.Document.Blocks.Add(this.GetNameInfo(this.sessionService.Name, message.CreateTime, true));
						this.imgHead.Source = this.sessionService.HeaderImage;
					}
					else
					{
						if (fromStaff != null)
						{
							this.MessageContent.Document.Blocks.Add(this.GetNameInfo(fromStaff.Name, message.CreateTime, false));
							this.imgHead.Source = fromStaff.HeaderImageOnline;
						}
						else
						{
							this.MessageContent.Document.Blocks.Add(this.GetNameInfo("此人已离职", message.CreateTime, false));
							this.imgHead.Source = this.imageService.GetHeader(ImageTypeHeader.Gary48);
						}
					}
					Block[] messageBlocks = message.MessageBlocks;
					for (int i = 0; i < messageBlocks.Length; i++)
					{
						Paragraph block = (Paragraph)messageBlocks[i];
						this.SetParagraphStyle(block, message.Style);
						this.MessageContent.Document.Blocks.Add(block);
					}
				}
			}
			this.message = message;
			this.group = gp;
			this.MouseEvent = true;
			this.InitUI();
		}
		public MsgRecordItem(Message message, Staff staf)
		{
			this.InitializeComponent();
			this.SelStaff = this.dataService.GetStaff(message.Gid);
			if (!string.IsNullOrEmpty(message.Url) && !string.IsNullOrEmpty(message.Icon) && !string.IsNullOrEmpty(message.FileName) && this.SelStaff.Uid == this.sessionService.Uid && this.SelStaff != null)
			{
				System.IO.FileInfo file = new System.IO.FileInfo(message.FileName);
				this.MessageContent.Document.Blocks.Add(this.GetNameInfo(this.sessionService.Name, message.CreateTime, true));
				this.imgHead.Source = this.sessionService.HeaderImage;
				OpenFileControl openFile = new OpenFileControl(new FileItem(file)
				{
					FileName = message.Url + message.FileName,
					IconBase64 = message.Icon
				}, true);
				if (openFile != null && !string.IsNullOrEmpty(this.SelStaff.Name))
				{
					Paragraph p = new Paragraph();
					FlowDocument doc = this.MessageContent.Document;
					p.Inlines.Add(openFile);
					doc.Blocks.Add(p);
				}
				this.staff = this.dataService.GetStaff(long.Parse(message.ToJid));
				this.message = message;
			}
			else
			{
				if (this.SelStaff != null)
				{
					if (message.Gid == this.sessionService.Uid)
					{
						this.MessageContent.Document.Blocks.Add(this.GetNameInfo(this.sessionService.Name, message.CreateTime, true));
						this.imgHead.Source = this.sessionService.HeaderImage;
					}
					else
					{
						this.MessageContent.Document.Blocks.Add(this.GetNameInfo(this.SelStaff.Name, message.CreateTime, false));
						this.imgHead.Source = this.SelStaff.HeaderImageOnline;
					}
					if (message.MessageBlocks.Length != 0)
					{
						Block[] messageBlocks = message.MessageBlocks;
						for (int i = 0; i < messageBlocks.Length; i++)
						{
							Paragraph block = (Paragraph)messageBlocks[i];
							this.SetParagraphStyle(block, message.Style);
							this.MessageContent.Document.Blocks.Add(block);
						}
						this.staff = this.dataService.GetStaff(long.Parse(message.ToJid));
						this.message = message;
					}
					else
					{
						this.staff = null;
						this.message = null;
					}
				}
			}
			this.MouseEvent = true;
			this.InitUI();
		}
		public MsgRecordItem(Block[] blocks)
		{
			this.InitializeComponent();
			for (int i = 0; i < blocks.Length; i++)
			{
				Paragraph block = (Paragraph)blocks[i];
				block.FontSize = 14.0;
				this.imgHead.Source = null;
				this.MessageContent.Document.Blocks.Add(block);
			}
			this.MouseEvent = false;
		}
		private void InitUI()
		{
			this.ImageMarkedView();
			if (!this.message.IsMark)
			{
				this.imgIsMarked.Visibility = Visibility.Collapsed;
			}
		}
		private Paragraph GetNameInfo(string name, string createTime, bool sender)
		{
			Run run = new Run(name + "      " + createTime);
			run.FontSize = 12.0;
			run.FontFamily = new FontFamily("宋体");
			if (sender)
			{
				run.Foreground = Brushes.Blue;
			}
			else
			{
				run.Foreground = Brushes.Green;
			}
			return new Paragraph
			{
				Inlines = 
				{
					run
				}
			};
		}
		private void SetParagraphStyle(Paragraph para, MessageStyle style)
		{
			if (para != null && style != null)
			{
				try
				{
					para.FontSize = (double)this.ms.FontSizeList[style.FontSize];
				}
				catch (System.IndexOutOfRangeException)
				{
					style.FontSize = 1;
					para.FontSize = 12.0;
				}
				try
				{
					para.FontFamily = new FontFamily(this.ms.FontFamilyList[style.FontFamily]);
				}
				catch (System.IndexOutOfRangeException)
				{
					style.FontFamily = 0;
					para.FontFamily = new FontFamily(this.ms.FontFamilyList[0]);
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
		}
		private void ImageMarkedView()
		{
			if (this.message.IsMark)
			{
				this.imgIsMarked.Source = this.imageService.GetIcon(ImageTypeIcon.Marked);
			}
			else
			{
				this.imgIsMarked.Source = this.imageService.GetIcon(ImageTypeIcon.UnMarked);
			}
		}
		private void TableRow_MouseEnter(object sender, MouseEventArgs e)
		{
			if (this.MouseEvent && !this.message.IsMark)
			{
				this.imgIsMarked.Visibility = Visibility.Visible;
			}
		}
		private void TableRow_MouseLeave(object sender, MouseEventArgs e)
		{
			if (this.MouseEvent)
			{
				if (!this.message.IsMark)
				{
					this.imgIsMarked.Visibility = Visibility.Collapsed;
				}
			}
		}
		private void IsMarkedHandler(object sender, MouseButtonEventArgs e)
		{
			if (this.group != null)
			{
				if (!this.message.IsMark)
				{
					this.messageCenter.SendGroupMessageAddMark(this.message.RecordId, true, IsMarkType.GROUP, this.sessionService.Uid, this.group.Gid);
					this.message.IsMark = true;
				}
				else
				{
					this.messageCenter.SendGroupMessageAddMark(this.message.RecordId, false, IsMarkType.GROUP, this.sessionService.Uid, this.group.Gid);
					this.message.IsMark = false;
				}
			}
			else
			{
				if (!this.message.IsMark)
				{
					this.messageCenter.SendStaffMessageAddMark(this.message.RecordId, true, IsMarkType.FELLOW, this.sessionService.Uid, this.staff.Uid);
					this.message.IsMark = true;
				}
				else
				{
					this.messageCenter.SendStaffMessageAddMark(this.message.RecordId, false, IsMarkType.FELLOW, this.sessionService.Uid, this.staff.Uid);
					this.message.IsMark = false;
				}
			}
			this.ImageMarkedView();
		}
		public void Dispose()
		{
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/messagecenter/msgrecorditem.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        ((MsgRecordItem)target).MouseEnter += new MouseEventHandler(this.TableRow_MouseEnter);
        //        ((MsgRecordItem)target).MouseLeave += new MouseEventHandler(this.TableRow_MouseLeave);
        //        break;
        //    case 2:
        //        this.imgHead = (Image)target;
        //        break;
        //    case 3:
        //        this.MessageContent = (FlowDocumentScrollViewer)target;
        //        break;
        //    case 4:
        //        this.imgIsMarked = (Image)target;
        //        this.imgIsMarked.MouseLeftButtonDown += new MouseButtonEventHandler(this.IsMarkedHandler);
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
