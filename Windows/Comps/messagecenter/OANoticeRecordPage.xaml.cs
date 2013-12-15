using IDKin.IM.Communicate;
using IDKin.IM.Data;
using IDKin.IM.Protocol.Account;
using IDKin.IM.Protocol.Commons;
using IDKin.IM.Theme.Helper;
using IDKin.IM.Windows.Util;
using IDKin.IM.Windows.ViewModel;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
namespace IDKin.IM.Windows.Comps.MessageCenter
{
	public partial class OANoticeRecordPage : Page//, IComponentConnector
	{
		private INViewModel inViewModel = new INViewModel();
		private ChatTabControlViewModel viewModel = new ChatTabControlViewModel();
		private IUtilService utilService = ServiceUtil.Instance.utilService;
		private IDataService dataService = ServiceUtil.Instance.DataService;
		private ISessionService sessionService = ServiceUtil.Instance.SessionService;
		private MessageCenterViewModel messageCenter = null;
		public int ShowPage;
		public int MaxPage;
		public OAModuleType type = OAModuleType.OA_GETALL_RECORD;
		private string startTime;
		private string endTime;
		private bool DateSearch = false;
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
		public OANoticeRecordPage()
		{
			ThemeSwitcher.LoadSkin(ThemeEnum.Aero, this);
			this.InitializeComponent();
			this.ShowPage = 1;
			this.MaxPage = 0;
			this.startTime = null;
			this.endTime = null;
			this.textPage.Text = this.ShowPage.ToString();
			this.tbkTotal.Text = "页/" + this.MaxPage.ToString() + "页";
			this.messageCenter = new MessageCenterViewModel();
		}
		public void Clear()
		{
			this.trgMessageTable.Rows.Clear();
		}
		public void NoRecordInit()
		{
			this.tbkTotal.Text = "页/0页";
			this.textPage.Text = "0";
			this.trgMessageTable.Rows.Clear();
			string str = "                              无消息提醒   ";
			OANoticeRecordDateRow row = new OANoticeRecordDateRow(str, true);
			row.Foreground = Brushes.Gray;
			row.FontSize = 14.0;
			this.trgMessageTable.Rows.Add(row);
		}
		public void SetShowPage(NoticeRecordResponse response)
		{
			if (this.type == OAModuleType.OA_GETALL_RECORD && this.MaxPage == 0)
			{
				if (response.noticeRecord[0].total % 10 == 0)
				{
					this.MaxPage = response.noticeRecord[0].total / 10;
				}
				else
				{
					this.MaxPage = response.noticeRecord[0].total / 10 + 1;
				}
				this.tbkTotal.Text = "页/" + this.MaxPage.ToString() + "页";
			}
			else
			{
				if (response.noticeRecord[0].total % 10 == 0)
				{
					this.MaxPage = response.noticeRecord[0].total / 10;
				}
				else
				{
					this.MaxPage = response.noticeRecord[0].total / 10 + 1;
				}
				this.tbkTotal.Text = "页/" + this.MaxPage.ToString() + "页";
			}
			this.DateSearch = false;
		}
		public void SetShowPage()
		{
			this.DateSearch = false;
			this.ShowPage = 1;
			this.textPage.Text = this.ShowPage.ToString();
		}
		public void MessageCenter(System.Collections.Generic.List<NoticeRecord> records)
		{
			try
			{
				this.Clear();
				if (records != null && records.Count > 0)
				{
					string Data = null;
					foreach (NoticeRecord record in records)
					{
						if (record != null)
						{
							if (Data == null || !Data.Equals(record.createTime.Substring(0, 10)))
							{
								OANoticeRecordDateRow datarow = new OANoticeRecordDateRow(record.createTime.Substring(0, 10));
								this.trgMessageTable.Rows.Add(datarow);
								OANoticeRecordItem row = new OANoticeRecordItem(record.createTime, record.message, record.title, record.url);
								this.trgMessageTable.Rows.Add(row);
								Data = record.createTime.Substring(0, 10);
							}
							else
							{
								OANoticeRecordItem row = new OANoticeRecordItem(record.createTime, record.message, record.title, record.url);
								this.trgMessageTable.Rows.Add(row);
							}
						}
					}
					this.DateSelect.Text = Data;
				}
			}
			catch (System.Exception e)
			{
				System.Console.WriteLine(e.ToString());
			}
		}
		private void FirstPage_Click(object sender, RoutedEventArgs e)
		{
			if (int.Parse(this.textPage.Text) != 1)
			{
				this.ShowPage = 1;
				this.textPage.Text = this.ShowPage.ToString();
				this.SendPageEvent(this.ShowPage);
				this.tbkTotal.Text = "页/" + this.MaxPage.ToString() + "页";
			}
		}
		private void PrePage_Click(object sender, RoutedEventArgs e)
		{
			if (int.Parse(this.textPage.Text) > 1)
			{
				this.ShowPage--;
				this.textPage.Text = this.ShowPage.ToString();
				this.SendPageEvent(this.ShowPage);
				this.tbkTotal.Text = "页/" + this.MaxPage.ToString() + "页";
			}
		}
		private void NextPage_Click(object sender, RoutedEventArgs e)
		{
			if (int.Parse(this.textPage.Text) < this.MaxPage)
			{
				this.ShowPage++;
				this.textPage.Text = this.ShowPage.ToString();
				this.SendPageEvent(this.ShowPage);
				this.tbkTotal.Text = "页/" + this.MaxPage.ToString() + "页";
			}
		}
		private void LastPage_Click(object sender, RoutedEventArgs e)
		{
			if (int.Parse(this.textPage.Text) != this.MaxPage)
			{
				this.ShowPage = this.MaxPage;
				this.textPage.Text = this.ShowPage.ToString();
				this.SendPageEvent(this.ShowPage);
				this.tbkTotal.Text = "页/" + this.MaxPage.ToString() + "页";
			}
		}
		private void SendPageEvent(int page)
		{
			if (this.DateSearch)
			{
				this.messageCenter.SendOADataSearch(this.sessionService.Uid, this.startTime, page, 10, this.type, this.endTime);
			}
			else
			{
				this.messageCenter.SendOANoticeRequest(this.sessionService.Uid, "0", page, 10, this.type);
			}
		}
		private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
		{
			if (this.DateSelect.IsDropDownOpen && this.DateSelect.Text != this.startTime)
			{
				this.DateSearch = true;
				this.trgMessageTable.Rows.Clear();
				System.DateTime dt = this.DateSelect.SelectedDate.Value;
				string format = "yyyy-MM-dd";
				string time = dt.ToString(format, System.Globalization.DateTimeFormatInfo.InvariantInfo);
				System.Globalization.DateTimeFormatInfo info = new System.Globalization.DateTimeFormatInfo();
				System.Globalization.DateTimeStyles style = System.Globalization.DateTimeStyles.None;
				this.DateSelect.SelectedDate = new System.DateTime?(System.DateTime.Parse(time, info, style));
				this.startTime = time + " 00:00:00";
				this.endTime = time + " 24:00:00";
				this.messageCenter.SendOADataSearch(this.sessionService.Uid, this.startTime, 0, 10, this.type, this.endTime);
			}
		}
		private void textPage_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == System.Windows.Input.Key.Return)
			{
				try
				{
					int page = int.Parse(this.textPage.Text.Trim());
					if (page > 0 && page < this.MaxPage)
					{
						this.ShowPage = page;
						this.SendPageEvent(page);
					}
					else
					{
						this.textPage.Text = this.ShowPage.ToString();
						MessageBox.Show("请输入正确的数字");
					}
				}
				catch (System.Exception)
				{
					this.textPage.Text = this.ShowPage.ToString();
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
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/messagecenter/oanoticerecordpage.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.ViewMessageBoxViewer = (FlowDocumentScrollViewer)target;
        //        break;
        //    case 2:
        //        this.ViewMessageBox = (FlowDocument)target;
        //        break;
        //    case 3:
        //        this.trgMessageTable = (TableRowGroup)target;
        //        break;
        //    case 4:
        //        this.DateSelect = (DatePicker)target;
        //        this.DateSelect.SelectedDateChanged += new System.EventHandler<SelectionChangedEventArgs>(this.DatePicker_SelectedDateChanged);
        //        break;
        //    case 5:
        //        this.FirstPage = (Button)target;
        //        this.FirstPage.Click += new RoutedEventHandler(this.FirstPage_Click);
        //        break;
        //    case 6:
        //        this.PrePage = (Button)target;
        //        this.PrePage.Click += new RoutedEventHandler(this.PrePage_Click);
        //        break;
        //    case 7:
        //        this.textPage = (TextBox)target;
        //        this.textPage.KeyUp += new KeyEventHandler(this.textPage_KeyUp);
        //        break;
        //    case 8:
        //        this.tbkTotal = (TextBlock)target;
        //        break;
        //    case 9:
        //        this.NextPage = (Button)target;
        //        this.NextPage.Click += new RoutedEventHandler(this.NextPage_Click);
        //        break;
        //    case 10:
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
