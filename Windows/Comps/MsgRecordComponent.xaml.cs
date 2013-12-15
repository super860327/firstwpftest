using IDKin.IM.Communicate;
using IDKin.IM.Core;
using IDKin.IM.Data;
using IDKin.IM.Protocol.Commons;
using IDKin.IM.Windows.Comps.FileTransport;
using IDKin.IM.Windows.Model;
using IDKin.IM.Windows.Util;
using IDKin.IM.Windows.ViewModel;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
namespace IDKin.IM.Windows.Comps
{
    public partial class MsgRecordComponent : UserControl//, IComponentConnector
    {
        private const int LIMIT = 10;
        private ChatTabControlViewModel viewModel = new ChatTabControlViewModel();
        private string Starttime;
        private string endtime;
        private int page = -1;
        private int total = 0;
        private MessageActorType type;
        private long id;
        private CooperationStaff coopStaff = null;
        private IDataService dataService;
        private ISessionService sessionService;
        private IFileService fileService = ServiceUtil.Instance.FileService;
        private bool tableRowColor = false;
        private ScrollViewer ViewMessageBoxScrollViewer = null;
        private MessageStyle ms = new MessageStyle();

        public int Total
        {
            get
            {
                return this.total;
            }
            set
            {
                this.total = value;
            }
        }
        public MessageActorType Type
        {
            set
            {
                this.type = value;
            }
        }
        public long Id
        {
            set
            {
                this.id = value;
            }
        }
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
        public string IconBase64
        {
            get;
            set;
        }
        public MsgRecordComponent()
        {
            this.InitializeComponent();
            this.InitUI();
            this.InitService();
        }
        private void InitUI()
        {
            this.Starttime = null;
            this.endtime = null;
        }
        private void InitService()
        {
            this.dataService = ServiceUtil.Instance.DataService;
            this.sessionService = ServiceUtil.Instance.SessionService;
            this.DataSelect.SelectedDateChanged += new System.EventHandler<SelectionChangedEventArgs>(this.DataSelect_SelectedDateChanged);
        }
        public void AddCooperationStaffMessageRecord(Message message)
        {
            try
            {
                this.AddCooperationStaffMessage(message);
            }
            catch (System.Exception e)
            {
                ServiceUtil.Instance.Logger.Error(e.ToString());
            }
        }
        public void AddMessageRecord(Message message)
        {
            try
            {
                this.AddMessage(message);
            }
            catch (System.Exception)
            {
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
        private void AddCooperationStaffMessage(Message message)
        {
            Block[] blocks = message.MessageBlocks;
            if (blocks != null && blocks.Length > 0)
            {
                CooperationStaff staff;
                if ((ulong)Jid.GetUid(message.FromJid) == (ulong)this.sessionService.Uid)
                {
                    staff = new CooperationStaff();
                    staff.Uid = this.sessionService.Uid;
                    staff.Name = this.sessionService.Name;
                }
                else
                {
                    staff = this.dataService.GetCooperationStaff((long)((ulong)Jid.GetUid(message.FromJid)), message.ProjectId);
                }
                if (staff != null)
                {
                    if (!string.IsNullOrEmpty(message.Url) && !string.IsNullOrEmpty(message.Icon) && !string.IsNullOrEmpty(message.FileName) && staff.Uid == this.sessionService.Uid)
                    {
                        System.IO.FileInfo file = new System.IO.FileInfo(message.FileName);
                        OpenFileControl openFile = new OpenFileControl(new FileItem(file)
                        {
                            FileName = message.Url + message.FileName,
                            IconBase64 = message.Icon
                        }, true);
                        if (openFile != null && !string.IsNullOrEmpty(staff.Name))
                        {
                            Paragraph p = new Paragraph();
                            FlowDocument doc = this.ViewMessageBoxViewer.Document;
                            TableRow tableRow = new TableRow();
                            TableCell tableCell = new TableCell();
                            tableRow.Cells.Add(tableCell);
                            tableCell.Blocks.Add(this.GetNameInfo(staff.Name, message.CreateTime, staff.Uid == this.sessionService.Uid));
                            p.Inlines.Add(openFile);
                            doc.Blocks.Add(p);
                            tableCell.Blocks.Add(doc.Blocks.LastBlock);
                            this.trgMessageTable.Rows.Add(tableRow);
                        }
                    }
                    else
                    {
                        TableRow tableRow = this.CreateTableRowCooperationStaff(message, blocks, staff);
                        this.trgMessageTable.Rows.Add(tableRow);
                        this.ScroolToEnd();
                    }
                }
                else
                {
                    TableRow tableRow = new TableRow();
                    TableCell tableCell = new TableCell();
                    tableRow.Cells.Add(tableCell);
                    tableCell.Blocks.Add(this.GetNameInfo("此人已离职", message.CreateTime, (long)int.Parse(message.FromJid.Substring(0, message.FromJid.IndexOf("@"))) == this.sessionService.Uid));
                    Block[] array = blocks;
                    for (int i = 0; i < array.Length; i++)
                    {
                        Paragraph block = (Paragraph)array[i];
                        this.SetParagraphStyle(block, message.Style);
                        tableCell.Blocks.Add(block);
                    }
                    this.trgMessageTable.Rows.Add(tableRow);
                }
            }
            this.DataSelect.Text = message.CreateTime.Substring(0, 10);
        }
        private void AddMessage(Message message)
        {
            Block[] blocks = message.MessageBlocks;
            if (blocks != null && blocks.Length > 0)
            {
                Staff staff = this.dataService.GetStaff((long)((ulong)Jid.GetUid(message.FromJid)));
                if (staff != null)
                {
                    if (!string.IsNullOrEmpty(message.Url) && !string.IsNullOrEmpty(message.Icon) && !string.IsNullOrEmpty(message.FileName) && staff.Uid == this.sessionService.Uid)
                    {
                        System.IO.FileInfo file = new System.IO.FileInfo(message.FileName);
                        OpenFileControl openFile = new OpenFileControl(new FileItem(file)
                        {
                            FileName = message.Url + message.FileName,
                            IconBase64 = message.Icon
                        }, true);
                        if (openFile != null && !string.IsNullOrEmpty(staff.Name))
                        {
                            Paragraph p = new Paragraph();
                            FlowDocument doc = this.ViewMessageBoxViewer.Document;
                            TableRow tableRow = new TableRow();
                            TableCell tableCell = new TableCell();
                            tableRow.Cells.Add(tableCell);
                            tableCell.Blocks.Add(this.GetNameInfo(staff.Name, message.CreateTime, staff.Uid == this.sessionService.Uid));
                            p.Inlines.Add(openFile);
                            doc.Blocks.Add(p);
                            tableCell.Blocks.Add(doc.Blocks.LastBlock);
                            this.trgMessageTable.Rows.Add(tableRow);
                        }
                    }
                    else
                    {
                        TableRow tableRow = this.CreateTableRow(message, blocks, staff);
                        this.trgMessageTable.Rows.Add(tableRow);
                        this.ScroolToEnd();
                    }
                }
                else
                {
                    TableRow tableRow = new TableRow();
                    TableCell tableCell = new TableCell();
                    tableRow.Cells.Add(tableCell);
                    tableCell.Blocks.Add(this.GetNameInfo("此人已离职", message.CreateTime, (long)int.Parse(message.FromJid.Substring(0, message.FromJid.IndexOf("@"))) == this.sessionService.Uid));
                    Block[] array = blocks;
                    for (int i = 0; i < array.Length; i++)
                    {
                        Paragraph block = (Paragraph)array[i];
                        this.SetParagraphStyle(block, message.Style);
                        tableCell.Blocks.Add(block);
                    }
                    this.trgMessageTable.Rows.Add(tableRow);
                }
            }
            this.DataSelect.Text = message.CreateTime.Substring(0, 10);
        }
        public TableRow CreateTableRowCooperationStaff(Message message, Block[] blocks, CooperationStaff staff)
        {
            TableRow tableRow = new TableRow();
            if (this.tableRowColor)
            {
                tableRow.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(221, 253, 214));
            }
            else
            {
                tableRow.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(241, 250, 255));
            }
            this.tableRowColor = !this.tableRowColor;
            TableCell tableCell = new TableCell();
            tableRow.Cells.Add(tableCell);
            tableCell.Blocks.Add(this.GetNameInfo(staff.Name, message.CreateTime, staff.Uid == this.sessionService.Uid));
            for (int i = 0; i < blocks.Length; i++)
            {
                Paragraph block = (Paragraph)blocks[i];
                this.SetParagraphStyle(block, message.Style);
                tableCell.Blocks.Add(block);
            }
            return tableRow;
        }
        public TableRow CreateTableRow(Message message, Block[] blocks, Staff staff)
        {
            TableRow tableRow = new TableRow();
            if (this.tableRowColor)
            {
                tableRow.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(221, 253, 214));
            }
            else
            {
                tableRow.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(241, 250, 255));
            }
            this.tableRowColor = !this.tableRowColor;
            TableCell tableCell = new TableCell();
            tableRow.Cells.Add(tableCell);
            tableCell.Blocks.Add(this.GetNameInfo(staff.Name, message.CreateTime, staff.Uid == this.sessionService.Uid));
            for (int i = 0; i < blocks.Length; i++)
            {
                Paragraph block = (Paragraph)blocks[i];
                this.SetParagraphStyle(block, message.Style);
                tableCell.Blocks.Add(block);
            }
            return tableRow;
        }
        private void ScroolToEnd()
        {
            if (this.ViewMessageBoxScrollViewer == null)
            {
                DependencyObject DO = this.ViewMessageBoxViewer;
                bool value = false;
                while (!value)
                {
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
                this.ViewMessageBoxScrollViewer.ScrollToBottom();
            }
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
                    para.FontFamily = new System.Windows.Media.FontFamily(this.ms.FontFamilyList[style.FontFamily]);
                }
                catch (System.IndexOutOfRangeException)
                {
                    style.FontFamily = 0;
                    para.FontFamily = new System.Windows.Media.FontFamily(this.ms.FontFamilyList[0]);
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
                para.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, style.FontColorR, style.FontColorG, style.FontColorB));
            }
        }
        private Paragraph GetNameInfo(string name, string createTime)
        {
            return this.GetNameInfo(name, createTime, false);
        }
        private Paragraph GetNameInfo(string name, string createTime, bool sender)
        {
            Run run = new Run(name + "  " + createTime);
            run.FontSize = 12.0;
            run.FontFamily = new System.Windows.Media.FontFamily("宋体");
            if (sender)
            {
                run.Foreground = System.Windows.Media.Brushes.Blue;
            }
            else
            {
                run.Foreground = System.Windows.Media.Brushes.Green;
            }
            return new Paragraph
            {
                Inlines = 
				{
					run
				}
            };
        }
        public void InitPage()
        {
            this.page = -1;
            this.total = 0;
        }
        public void Clear()
        {
            this.trgMessageTable.Rows.Clear();
        }
        public void ViewPageButton()
        {
            this.FirstPage.IsEnabled = false;
            this.PrePage.IsEnabled = false;
            this.LastPage.IsEnabled = false;
            this.NextPage.IsEnabled = false;
            decimal d = this.total / 10m;
            decimal max_page = System.Math.Ceiling(d);
            if (this.page == -1)
            {
                this.page = (int)max_page;
                this.textPage.Text = this.page.ToString();
            }
            this.tbkTotal.Text = "页/" + max_page.ToString() + "页";
            if (this.page == 1)
            {
                if (this.page * 10 < this.total)
                {
                    this.LastPage.IsEnabled = true;
                    this.NextPage.IsEnabled = true;
                }
            }
            else
            {
                if (this.page == max_page)
                {
                    this.FirstPage.IsEnabled = true;
                    this.PrePage.IsEnabled = true;
                }
                else
                {
                    if (this.page != 1 && this.page != max_page)
                    {
                        this.FirstPage.IsEnabled = true;
                        this.PrePage.IsEnabled = true;
                        this.LastPage.IsEnabled = true;
                        this.NextPage.IsEnabled = true;
                    }
                }
            }
        }
        private int GetPage(int page)
        {
            decimal d = this.total / 10m;
            decimal max_page = System.Math.Ceiling(d);
            return (int)max_page + 1 - page;
        }
        private byte[] GetColorBytes(string style)
        {
            char[] reg = new char[]
			{
				'#'
			};
            string[] s = style.Split(reg);
            byte[] bytes = new byte[4];
            if (s.Length == 4)
            {
                bytes[0] = byte.Parse(s[0]);
                bytes[1] = byte.Parse(s[1]);
                bytes[2] = byte.Parse(s[2]);
                bytes[3] = byte.Parse(s[3]);
            }
            return bytes;
        }
        public void ChatContentRecoredHandler(int page)
        {
            this.textPage.Text = page.ToString();
            page = this.GetPage(page);
            this.Clear();
            if (this.sessionService.IsDateSearch)
            {
                Staff staff = this.dataService.GetStaff(this.id);
                if (this.type == MessageActorType.EntStaff)
                {
                    if (staff != null)
                    {
                        this.viewModel.SendStaffDataSearchMessage(this.sessionService.Uid, staff.Uid, this.Starttime, page, 10, this.endtime, MessageRecordType.MESSAGE_CHAT_RECORD);
                    }
                }
                else
                {
                    if (this.type == MessageActorType.CooperationStaff)
                    {
                        if (this.coopStaff != null)
                        {
                            this.viewModel.SendCooperationMessageRecordRequestByDate(this.sessionService.Uid, this.coopStaff.Uid, this.coopStaff.ProjectId, this.Starttime, page, 10, this.endtime, MessageRecordType.MESSAGE_CHAT_RECORD);
                        }
                    }
                    else
                    {
                        EntGroup group = this.dataService.GetEntGroup(this.id);
                        if (group != null)
                        {
                            this.viewModel.SendGroupSearchRecord(this.sessionService.Uid, this.id, this.Starttime, page, 10, this.endtime, MessageRecordType.MESSAGE_CHAT_RECORD);
                        }
                    }
                }
            }
            else
            {
                if (this.type == MessageActorType.EntStaff)
                {
                    long from_uid = this.sessionService.Uid;
                    Staff staff = this.dataService.GetStaff(this.id);
                    if (staff != null)
                    {
                        this.viewModel.SendStaffMessageRecord(from_uid, staff.Uid, "0", page, 10, MessageRecordType.MESSAGE_CHAT_RECORD);
                    }
                }
                else
                {
                    if (this.type == MessageActorType.CooperationStaff)
                    {
                        if (this.coopStaff != null)
                        {
                            this.viewModel.SendCooperationMessageRecordRequest(this.sessionService.Uid, this.coopStaff.Uid, this.coopStaff.UnitedProjectid, this.Starttime, page, 10, MessageRecordType.MESSAGE_CHAT_RECORD);
                        }
                    }
                    else
                    {
                        EntGroup group = this.dataService.GetEntGroup(this.id);
                        if (group != null)
                        {
                            this.viewModel.SendGroupMessageRecord(group.Gid, "0", page, 10, this.sessionService.Uid, MessageRecordType.MESSAGE_CHAT_RECORD);
                        }
                    }
                }
            }
        }
        private void FirstPage_Click(object sender, RoutedEventArgs e)
        {
            if (this.page != 1)
            {
                this.page = 1;
                this.ChatContentRecoredHandler(this.page);
                this.sessionService.IsChatPanelRecord = true;
            }
        }
        private void PrePage_Click(object sender, RoutedEventArgs e)
        {
            if (this.page > 1)
            {
                this.page--;
                this.ChatContentRecoredHandler(this.page);
                this.sessionService.IsChatPanelRecord = true;
            }
        }
        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            decimal d = this.total / 10m;
            decimal max_page = System.Math.Ceiling(d);
            if (this.page < max_page)
            {
                this.page++;
                this.ChatContentRecoredHandler(this.page);
                this.sessionService.IsChatPanelRecord = true;
            }
        }
        private void LastPage_Click(object sender, RoutedEventArgs e)
        {
            decimal d = this.total / 10m;
            decimal max_page = System.Math.Ceiling(d);
            if (this.page != (int)max_page)
            {
                this.page = (int)max_page;
                this.ChatContentRecoredHandler(this.page);
                this.sessionService.IsChatPanelRecord = true;
            }
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            this.ViewMessageBox.Blocks.Clear();
        }
        private void topBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
        }
        private void textPage_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                decimal d = this.total / 10m;
                decimal max_page = System.Math.Ceiling(d);
                if (e.Key == System.Windows.Input.Key.Return)
                {
                    int page = int.Parse(this.textPage.Text);
                    if (page >= 1 && page <= (int)max_page)
                    {
                        this.ChatContentRecoredHandler(page);
                    }
                    else
                    {
                        this.textPage.Text = this.page.ToString();
                        MessageBox.Show("请输入正确的数字");
                    }
                }
            }
            catch
            {
                this.textPage.Text = this.page.ToString();
                MessageBox.Show("请输入正确的数字");
            }
        }
        public void setShowPage()
        {
            this.Clear();
            TableRow tableRow = new TableRow();
            TableCell tableCell = new TableCell();
            tableRow.Cells.Add(tableCell);
            tableCell.Blocks.Add(this.NoInfoRecord("无消息记录"));
            this.trgMessageTable.Rows.Add(tableRow);
            this.textPage.Text = "0";
            this.tbkTotal.Text = "页/0页";
        }
        private Paragraph NoInfoRecord(string name)
        {
            Run run = new Run("             " + name);
            run.FontSize = 14.0;
            run.FontFamily = new System.Windows.Media.FontFamily("宋体");
            run.Foreground = System.Windows.Media.Brushes.DarkGray;
            return new Paragraph
            {
                Inlines = 
				{
					run
				}
            };
        }
        private void DataSelect_SelectedDateChanged(object sender, System.EventArgs e)
        {
            if (this.DataSelect.IsDropDownOpen && this.DataSelect.IsInitialized)
            {
                this.InitUI();
                this.Clear();
                System.DateTime dt = this.DataSelect.SelectedDate.Value;
                string format = "yyyy-MM-dd";
                string time = dt.ToString(format, System.Globalization.DateTimeFormatInfo.InvariantInfo);
                System.Globalization.DateTimeFormatInfo info = new System.Globalization.DateTimeFormatInfo();
                System.Globalization.DateTimeStyles style = System.Globalization.DateTimeStyles.None;
                this.DataSelect.SelectedDate = new System.DateTime?(System.DateTime.Parse(time, info, style));
                this.Starttime = time + " 00:00:00";
                this.endtime = time + " 24:00:00";
                if (this.type == MessageActorType.EntStaff)
                {
                    Staff staff = this.dataService.GetStaff(this.id);
                    if (staff != null)
                    {
                        this.viewModel.SendStaffDataSearchMessage(this.sessionService.Uid, staff.Uid, this.Starttime, 0, 10, this.endtime, MessageRecordType.MESSAGE_CHAT_RECORD);
                        this.sessionService.IsDateSearch = true;
                    }
                }
                else
                {
                    if (this.type == MessageActorType.CooperationStaff)
                    {
                        if (this.coopStaff != null)
                        {
                            this.viewModel.SendCooperationMessageRecordRequestByDate(this.sessionService.Uid, this.coopStaff.Uid, this.coopStaff.ProjectId, this.Starttime, this.page, 10, this.endtime, MessageRecordType.MESSAGE_CHAT_RECORD);
                        }
                    }
                    else
                    {
                        EntGroup group = this.dataService.GetEntGroup(this.id);
                        if (group != null)
                        {
                            this.viewModel.SendGroupSearchRecord(this.sessionService.Uid, this.id, this.Starttime, 0, 10, this.endtime, MessageRecordType.MESSAGE_CHAT_RECORD);
                            this.sessionService.IsDateSearch = true;
                        }
                    }
                }
                this.InitPage();
                this.DataSelect.Text = this.endtime;
                this.DataSelect.IsDropDownOpen = false;
            }
        }
        private void ViewMoreRecord(object sender, MouseButtonEventArgs e)
        {
            Staff staff = this.dataService.GetStaff(this.id);
            EntGroup group = this.dataService.GetEntGroup(this.id);
            if (staff != null && this.type == MessageActorType.EntStaff)
            {
                if (!WindowModel.Instance.IsOpenMessageCenterWindow())
                {
                    WindowModel.Instance.MessageCenterWindow.Show();
                    WindowModel.Instance.MessageCenterWindow.Topmost = true;
                }
                else
                {
                    WindowModel.Instance.MessageCenterWindow.work.IsExpanded = true;
                    if (WindowModel.Instance.MessageCenterWindow.WindowState == WindowState.Minimized)
                    {
                        WindowModel.Instance.MessageCenterWindow.WindowState = WindowState.Normal;
                    }
                    WindowModel.Instance.MessageCenterWindow.Topmost = true;
                }
                WindowModel.Instance.MessageCenterWindow.workTreeView.NodeExpand(staff.Uid, true);
                this.viewModel.SendStaffMessageRecord(this.sessionService.Uid, staff.Uid, "0", 1, 10, MessageRecordType.MESSAGE_CENTER_RECORD);
                WindowModel.Instance.MsgRecordPage.InitData(staff.Uid, false, true, 0);
                WindowModel.Instance.MsgRecordPage.InitTitleName("与" + staff.Name + "消息记录");
                WindowModel.Instance.MessageCenterWindow.ViewMsgFrame.NavigationService.Navigate(WindowModel.Instance.MsgRecordPage);
            }
            else
            {
                if (group != null)
                {
                    if (!WindowModel.Instance.IsOpenMessageCenterWindow())
                    {
                        WindowModel.Instance.MessageCenterWindow.Show();
                        WindowModel.Instance.MessageCenterWindow.Topmost = true;
                    }
                    else
                    {
                        WindowModel.Instance.MessageCenterWindow.work.IsExpanded = true;
                        if (WindowModel.Instance.MessageCenterWindow.WindowState == WindowState.Minimized)
                        {
                            WindowModel.Instance.MessageCenterWindow.WindowState = WindowState.Normal;
                        }
                        WindowModel.Instance.MessageCenterWindow.Topmost = true;
                    }
                    WindowModel.Instance.MessageCenterWindow.workTreeView.NodeExpand(group.Gid, false);
                    this.viewModel.SendGroupMessageRecord(group.Gid, "0", 1, 10, this.sessionService.Uid, MessageRecordType.MESSAGE_CENTER_RECORD);
                    WindowModel.Instance.MsgRecordPage.InitData(group.Gid, false, false, 0);
                    WindowModel.Instance.MsgRecordPage.InitTitleName("与" + group.Name + "群消息记录");
                    WindowModel.Instance.MessageCenterWindow.ViewMsgFrame.NavigationService.Navigate(WindowModel.Instance.MsgRecordPage);
                }
            }
        }
    }
}
