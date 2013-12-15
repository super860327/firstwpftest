using IDKin.IM.Communicate;
using IDKin.IM.Core;
using IDKin.IM.Data;
using IDKin.IM.Protocol.Commons;
using IDKin.IM.Theme.Helper;
using IDKin.IM.Windows.Model;
using IDKin.IM.Windows.Util;
using IDKin.IM.Windows.ViewModel;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
namespace IDKin.IM.Windows.Comps.MessageCenter
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public partial class MsgRecordPage : Page//, IComponentConnector
	{
		private long ID;
		private bool IsMark;
		private bool IsStaff;
		private int TotalRecord;
		private ChatTabControlViewModel viewModel = new ChatTabControlViewModel();
		private ISessionService sessionService = ServiceUtil.Instance.SessionService;
		private IUtilService utilService = ServiceUtil.Instance.utilService;
		private MessageCenterViewModel messageCenter = new MessageCenterViewModel();
		private bool IsDataSearch;
		private string Starttime = null;
		private string endtime = null;
		private int Showpage = 0;
		private int TotalPage = 0;
		//internal TextBlock titleName;
		//internal FlowDocumentScrollViewer ViewMessageBoxViewer;
		//internal FlowDocument ViewMessageBox;
		//internal TableRowGroup trgMessageTable;
		//internal DatePicker DateSelect;
		//internal Button FirstPage;
		//internal Button PrePage;
		//internal TextBox textPage;
		//internal TextBlock tbkTotal;
		//internal Button NextPage;
		//internal Button LastPage;
        ////private bool _contentLoaded;
		public MsgRecordPage()
		{
			ThemeSwitcher.LoadSkin(ThemeEnum.Aero, this);
			this.InitializeComponent();
			this.ID = 0L;
			this.IsMark = false;
			this.IsStaff = false;
			this.TotalRecord = 0;
			this.Showpage = 0;
			this.TotalPage = 0;
			this.IsDataSearch = false;
		}
		private int GetPage(int page)
		{
			return this.TotalPage + 1 - this.Showpage;
		}
		public void InitData(long id, bool isMark, bool isstaff, int CurrenPage)
		{
			this.trgMessageTable.Rows.Clear();
			this.ID = id;
			this.IsMark = isMark;
			this.IsStaff = isstaff;
			this.Showpage = CurrenPage;
			this.IsDataSearch = false;
		}
		public void InitTitleName(string name)
		{
			this.titleName.Text = name;
		}
		public void NORecord()
		{
			this.titleName.Text = "";
			this.tbkTotal.Text = "页/0页";
			this.textPage.Text = "0";
		}
		private void NextPage_Click(object sender, RoutedEventArgs e)
		{
			if (int.Parse(this.textPage.Text) < this.TotalPage)
			{
				this.Showpage++;
				this.textPage.Text = this.Showpage.ToString();
				this.PageHandle(this.Showpage);
			}
		}
		private void LastPage_Click(object sender, RoutedEventArgs e)
		{
			if (int.Parse(this.textPage.Text) != this.TotalPage)
			{
				this.Showpage = this.TotalPage;
				this.textPage.Text = this.Showpage.ToString();
				this.PageHandle(this.Showpage);
			}
		}
		private void PrePage_Click(object sender, RoutedEventArgs e)
		{
			if (int.Parse(this.textPage.Text) > 1)
			{
				this.Showpage--;
				this.textPage.Text = this.Showpage.ToString();
				this.PageHandle(this.Showpage);
			}
		}
		private void FirstPage_Click(object sender, RoutedEventArgs e)
		{
			if (int.Parse(this.textPage.Text) != 1)
			{
				this.Showpage = 1;
				this.textPage.Text = this.Showpage.ToString();
				this.PageHandle(this.Showpage);
			}
		}
		public void PageHandle(int Page)
		{
			this.trgMessageTable.Rows.Clear();
			WindowModel.Instance.MsgRecordPage = this;
			int page = this.GetPage(Page);
			if (this.IsDataSearch)
			{
				if (this.IsMark)
				{
					if (this.IsStaff)
					{
						this.messageCenter.SendStaffDateSearchMark(this.sessionService.Uid, this.ID, this.Starttime, page, 10, this.endtime, MessageRecordType.MESSAGE_CENTER_RECORD);
					}
					else
					{
						this.messageCenter.SendGroupDateSearchMark(this.ID, this.Starttime, page, 10, this.sessionService.Uid, this.endtime, MessageRecordType.MESSAGE_CENTER_RECORD);
					}
				}
				else
				{
					if (this.IsStaff)
					{
						this.viewModel.SendStaffSearchRecord(this.sessionService.Uid, this.ID, this.Starttime, page, 10, this.endtime, MessageRecordType.MESSAGE_CENTER_RECORD);
					}
					else
					{
						this.viewModel.SendGroupSearchRecord(this.sessionService.Uid, this.ID, this.Starttime, page, 10, this.endtime, MessageRecordType.MESSAGE_CENTER_RECORD);
					}
				}
			}
			else
			{
				if (this.IsMark)
				{
					if (this.IsStaff)
					{
						this.messageCenter.SendStaffMessageRecordMark(this.sessionService.Uid, this.ID, "0", page, 10, MessageRecordType.MESSAGE_CENTER_RECORD);
					}
					else
					{
						this.messageCenter.SendGroupMessageRecordMark(this.ID, "0", page, 10, this.sessionService.Uid, MessageRecordType.MESSAGE_CENTER_RECORD);
					}
				}
				else
				{
					if (this.IsStaff)
					{
						this.viewModel.SendStaffMessageRecord(this.sessionService.Uid, this.ID, "0", page, 10, MessageRecordType.MESSAGE_CENTER_RECORD);
					}
					else
					{
						this.viewModel.SendGroupMessageRecord(this.ID, "0", page, 10, this.sessionService.Uid, MessageRecordType.MESSAGE_CENTER_RECORD);
					}
				}
			}
		}
		private void SetRecordPageParam()
		{
			if (this.TotalRecord % 10 == 0)
			{
				this.TotalPage = this.TotalRecord / 10;
			}
			else
			{
				this.TotalPage = this.TotalRecord / 10 + 1;
			}
			this.tbkTotal.Text = "页/" + this.TotalPage.ToString() + "页";
			if (this.Showpage == 0)
			{
				this.Showpage = this.TotalPage;
				this.textPage.Text = this.Showpage.ToString();
			}
		}
		public void MessageCenterRecordStaff(Message[] messages, Staff staff, int total)
		{
			bool color = true;
			this.TotalRecord = total;
			this.trgMessageTable.Rows.Clear();
			int i;
			for (i = messages.Length - 1; i >= 0; i--)
			{
				if (messages[i].MessageBlocks != null && messages[i].MessageBlocks.Length > 0)
				{
					MsgRecordItem row = new MsgRecordItem(messages[i], staff);
					if (color)
					{
						row.Background = new SolidColorBrush(Color.FromRgb(241, 250, 255));
						color = false;
					}
					else
					{
						row.Background = new SolidColorBrush(Color.FromRgb(221, 253, 214));
						color = true;
					}
					WindowModel.Instance.MsgRecordPage.trgMessageTable.Rows.Add(row);
				}
			}
			this.SetRecordPageParam();
			string st = messages[i + 1].CreateTime.Substring(0, 10);
			this.DateSelect.Text = st.ToString();
		}
		public void MessageCenterRecordGroup(Message[] messages, EntGroup group, int total)
		{
			bool color = true;
			this.TotalRecord = total;
			this.trgMessageTable.Rows.Clear();
			int i;
			for (i = messages.Length - 1; i >= 0; i--)
			{
				if (messages[i].MessageBlocks != null && messages[i].MessageBlocks.Length != 0)
				{
					MsgRecordItem row = new MsgRecordItem(messages[i], group);
					if (color)
					{
						row.Background = new SolidColorBrush(Color.FromRgb(241, 250, 255));
						color = false;
					}
					else
					{
						row.Background = new SolidColorBrush(Color.FromRgb(221, 253, 214));
						color = true;
					}
					WindowModel.Instance.MsgRecordPage.trgMessageTable.Rows.Add(row);
				}
			}
			this.SetRecordPageParam();
			this.InitTitleName("在" + group.Name + "的消息记录");
			string st = messages[i + 1].CreateTime.Substring(0, 10);
			this.DateSelect.Text = st;
		}
		private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
		{
			if (this.DateSelect.IsInitialized && this.DateSelect.IsDropDownOpen)
			{
				System.DateTime dt = this.DateSelect.SelectedDate.Value;
				string format = "yyyy-MM-dd";
				string time = dt.ToString(format, System.Globalization.DateTimeFormatInfo.InvariantInfo);
				System.Globalization.DateTimeFormatInfo info = new System.Globalization.DateTimeFormatInfo();
				System.Globalization.DateTimeStyles style = System.Globalization.DateTimeStyles.None;
				this.DateSelect.SelectedDate = new System.DateTime?(System.DateTime.Parse(time, info, style));
				this.Starttime = time + " 00:00:00";
				this.endtime = time + " 24:00:00";
				this.Showpage = 0;
				if (this.IsMark)
				{
					if (this.IsStaff)
					{
						this.messageCenter.SendStaffDateSearchMark(this.sessionService.Uid, this.ID, this.Starttime, 0, 10, this.endtime, MessageRecordType.MESSAGE_CENTER_RECORD);
						this.IsDataSearch = true;
					}
					else
					{
						this.messageCenter.SendGroupDateSearchMark(this.ID, this.Starttime, 0, 10, this.sessionService.Uid, this.endtime, MessageRecordType.MESSAGE_CENTER_RECORD);
						this.IsDataSearch = true;
					}
				}
				else
				{
					if (this.IsStaff)
					{
						this.viewModel.SendStaffSearchRecord(this.sessionService.Uid, this.ID, this.Starttime, 0, 10, this.endtime, MessageRecordType.MESSAGE_CENTER_RECORD);
						this.IsDataSearch = true;
					}
					else
					{
						this.viewModel.SendGroupSearchRecord(this.sessionService.Uid, this.ID, this.Starttime, 0, 10, this.endtime, MessageRecordType.MESSAGE_CENTER_RECORD);
						this.IsDataSearch = true;
					}
				}
				this.DateSelect.IsDropDownOpen = false;
			}
		}
		public void ClearPage()
		{
			this.trgMessageTable.Rows.Clear();
			TableRow tableRow = new TableRow();
			TableCell tableCell = new TableCell();
			tableRow.Cells.Add(tableCell);
			Block[] blocks;
			if (this.IsMark)
			{
				blocks = this.utilService.MessageDecode("[{T:                                                无已标记记录}]");
			}
			else
			{
				blocks = this.utilService.MessageDecode("[{T:                                               无消息记录}]");
			}
			MsgRecordItem row = new MsgRecordItem(blocks);
			row.Background = Brushes.Transparent;
			row.Foreground = Brushes.Gray;
			this.trgMessageTable.Rows.Add(row);
			this.TotalRecord = 0;
			this.TotalRecord = 0;
			this.Showpage = 0;
			this.tbkTotal.Text = "页/0页";
			this.textPage.Text = "0";
		}
		private bool IsDigital(string text)
		{
			string str = "^[0-9]*$";
			Regex regex = new Regex(str);
			return regex.IsMatch(text);
		}
		private void textPage_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == System.Windows.Input.Key.Return)
			{
				try
				{
					int page = int.Parse(this.textPage.Text.Trim());
					if (page > 0 && page < this.TotalPage)
					{
						this.Showpage = page;
						this.PageHandle(page);
					}
					else
					{
						this.textPage.Text = this.Showpage.ToString();
						MessageBox.Show("请输入正确的数字");
					}
				}
				catch (System.Exception)
				{
					this.textPage.Text = this.Showpage.ToString();
					MessageBox.Show("请输入正确的数字");
				}
			}
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/messagecenter/msgrecordpage.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.titleName = (TextBlock)target;
        //        break;
        //    case 2:
        //        this.ViewMessageBoxViewer = (FlowDocumentScrollViewer)target;
        //        break;
        //    case 3:
        //        this.ViewMessageBox = (FlowDocument)target;
        //        break;
        //    case 4:
        //        this.trgMessageTable = (TableRowGroup)target;
        //        break;
        //    case 5:
        //        this.DateSelect = (DatePicker)target;
        //        this.DateSelect.SelectedDateChanged += new System.EventHandler<SelectionChangedEventArgs>(this.DatePicker_SelectedDateChanged);
        //        break;
        //    case 6:
        //        this.FirstPage = (Button)target;
        //        this.FirstPage.Click += new RoutedEventHandler(this.FirstPage_Click);
        //        break;
        //    case 7:
        //        this.PrePage = (Button)target;
        //        this.PrePage.Click += new RoutedEventHandler(this.PrePage_Click);
        //        break;
        //    case 8:
        //        this.textPage = (TextBox)target;
        //        this.textPage.KeyUp += new KeyEventHandler(this.textPage_KeyUp);
        //        break;
        //    case 9:
        //        this.tbkTotal = (TextBlock)target;
        //        break;
        //    case 10:
        //        this.NextPage = (Button)target;
        //        this.NextPage.Click += new RoutedEventHandler(this.NextPage_Click);
        //        break;
        //    case 11:
        //        this.LastPage = (Button)target;
        //        this.LastPage.Click += new RoutedEventHandler(this.LastPage_Click);
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
