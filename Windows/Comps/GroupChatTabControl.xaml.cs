using CSharpWin;
using IDKin.IM.Communicate;
using IDKin.IM.Core;
using IDKin.IM.Data;
using IDKin.IM.ImageService;
using IDKin.IM.Log;
using IDKin.IM.Protocol.Enterprise;
using IDKin.IM.Util;
using IDKin.IM.Windows.Properties;
using IDKin.IM.Windows.Util;
using IDKin.IM.Windows.View;
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
    public partial class GroupChatTabControl : System.Windows.Controls.UserControl
    {
        private delegate void ShowCutDelegate();
        private delegate void HideCutDelegate();
        private const long MAX_FILE_LENGTH = 209715200L;
        public GroupMemberControl GroupMemberControl = null;
        public FileTransmitList FileList = new FileTransmitList();
        private EntGroup group;
        private ISessionService sessionService = ServiceUtil.Instance.SessionService;
        private IDataService dataService = ServiceUtil.Instance.DataService;
        private IFileService fileService = ServiceUtil.Instance.FileService;
        private IImageService imageService = ServiceUtil.Instance.ImageService;
        private ILogger logger = ServiceUtil.Instance.Logger;
        private ScreenshotPopup popup = null;
        //internal ImageBrush userHead;
        //internal IconButton btnSendFile;
        //internal IconButton btnMsgBox;
        //internal ImageSplitButton btnScreenshot;
        //internal IconButton btnSendImg;
        //internal IconButton btnGift;
        //internal ChatComponent ChatComponent;
        ////private bool _contentLoaded;
        public long GroupId
        {
            get
            {
                long result;
                if (this.group != null)
                {
                    result = this.group.Gid;
                }
                else
                {
                    result = 0L;
                }
                return result;
            }
        }
        public GroupChatTabControl(EntGroup group)
        {
            this.InitializeComponent();
            this.ChatComponent.InitData(group);
            this.ChatComponent.MsgRecordStatus = new ChatComponent.MsgRecordStatusDelegate(this.MsgRecordStatusHandle);
            this.ChatComponent.viewMsgBox.PreviewDragEnter += new System.Windows.DragEventHandler(this.TextBox_PreviewDragEnter);
            this.ChatComponent.viewMsgBox.PreviewDragOver += new System.Windows.DragEventHandler(this.TextBox_PreviewDragEnter);
            this.ChatComponent.viewMsgBox.PreviewDrop += new System.Windows.DragEventHandler(this.UserControl_Drop);
            this.ChatComponent.inputMsgBox.PreviewDragEnter += new System.Windows.DragEventHandler(this.TextBox_PreviewDragEnter);
            this.ChatComponent.inputMsgBox.PreviewDragOver += new System.Windows.DragEventHandler(this.TextBox_PreviewDragEnter);
            this.ChatComponent.inputMsgBox.PreviewDrop += new System.Windows.DragEventHandler(this.UserControl_Drop);
            this.group = group;
            this.InitUI();
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
                    this.ShowGroupMemberControl();
                }
            }
        }
        private void InitUI()
        {
            this.GroupMemberControl = new GroupMemberControl(this.group);
            this.GroupMemberControl.UpdateGroupStaffCount(this.GetGroupMemberOnlineCount(), this.GetGroupMemberTotalCount());
            this.ChatComponent.PanelChangeContent.Children.Add(this.GroupMemberControl);
            this.ChatComponent.PanelChangeContent.Children.Add(this.FileList);
            this.ChatComponent.btnShake.Visibility = Visibility.Collapsed;
            this.userHead.ImageSource = this.imageService.GetEntGroupImage40();
            this.ShowGroupMemberControl();
            string groups = Settings.Default.ShieldGroups;
            if (groups.IndexOf("#" + this.group.Gid + ";") != -1)
            {
                this.ChatComponent.menuShield.IsChecked = true;
            }
        }
        private int GetGroupMemberOnlineCount()
        {
            int count = 0;
            if (this.group != null && this.group.Member != null && this.group.Member.Length > 0)
            {
                long[] member = this.group.Member;
                for (int i = 0; i < member.Length; i++)
                {
                    long uid = member[i];
                    IDKin.IM.Core.Staff staff = this.dataService.GetStaff(uid);
                    if (staff != null && staff.Status != UserStatus.Offline && staff.Status != UserStatus.Hide)
                    {
                        count++;
                    }
                }
            }
            return count;
        }
        private int GetGroupMemberTotalCount()
        {
            int result;
            if (this.group != null && this.group.Member != null && this.group.Member.Length > 0)
            {
                result = this.group.Member.Length;
            }
            else
            {
                result = 0;
            }
            return result;
        }
        public void UpdateGroupAllMember()
        {
            if (this.GroupMemberControl != null)
            {
                this.ChatComponent.PanelChangeContent.Children.Remove(this.GroupMemberControl);
            }
            this.GroupMemberControl = new GroupMemberControl(this.group);
            this.ChatComponent.PanelChangeContent.Children.Add(this.GroupMemberControl);
        }
        public void UpdateGroupMemberListHandler(IDKin.IM.Core.Staff staff)
        {
            if (this.GroupMemberControl != null)
            {
                this.GroupMemberControl.GroupMemberList.UpdateGroupMember(staff);
                this.GroupMemberControl.UpdateGroupStaffCount(this.GetGroupMemberOnlineCount(), this.GetGroupMemberTotalCount());
            }
        }
        public void ShowGroupMemberControl()
        {
            if (this.FileList != null && this.GroupMemberControl != null && this.ChatComponent.MsgRecordComp != null)
            {
                this.GroupMemberControl.Visibility = Visibility.Visible;
                this.FileList.Visibility = Visibility.Collapsed;
                this.ChatComponent.MsgRecordComp.Visibility = Visibility.Collapsed;
            }
            this.CountChatPanelWidth(400);
        }
        public void ShowMsgRecordComp()
        {
            if (this.FileList != null && this.GroupMemberControl != null && this.ChatComponent.MsgRecordComp != null)
            {
                this.GroupMemberControl.Visibility = Visibility.Collapsed;
                this.FileList.Visibility = Visibility.Collapsed;
                this.ChatComponent.MsgRecordComp.Visibility = Visibility.Visible;
            }
            this.CountChatPanelWidth(520);
        }
        public void ShowFileList()
        {
            if (this.FileList != null && this.GroupMemberControl != null && this.ChatComponent.MsgRecordComp != null)
            {
                this.GroupMemberControl.Visibility = Visibility.Collapsed;
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
        public void DeleNewAdminHandler(long[] admins)
        {
            try
            {
                if (admins.Length > 0)
                {
                    for (int i = 0; i < admins.Length; i++)
                    {
                        long adm = admins[i];
                        this.GroupMemberControl.GroupMemberList.DeleAdmin(adm);
                    }
                }
            }
            catch (System.Exception e)
            {
                ServiceUtil.Instance.Logger.Error(e.ToString());
            }
        }
        public void AddNewAdminHandler(long[] admins)
        {
            try
            {
                if (admins.Length > 0)
                {
                    for (int i = 0; i < admins.Length; i++)
                    {
                        long adm = admins[i];
                        this.GroupMemberControl.GroupMemberList.AddAdmin(adm);
                    }
                }
            }
            catch (System.Exception e)
            {
                ServiceUtil.Instance.Logger.Error(e.ToString());
            }
        }
        public void AddReceiveHttpFile(GroupFileUploadResponse response)
        {
            this.ShowFileList();
            IDKin.IM.Core.Staff staff = this.dataService.GetStaff(response.fromUid);
            string userName = "";
            if (staff != null)
            {
                userName = staff.Name;
            }
            NewFileListItem item = new NewFileListItem(response.filename, response.file_Id, response.size, this, this.group, response.iconString, userName);
            this.FileList.fileList.Items.Add(item);
            item.EndEvent = new NewFileListItem.EndEventDelegate(this.FileEndEventHandle);
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
                            this.ShowGroupMemberControl();
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e.ToString());
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
                    if (System.Windows.MessageBox.Show("确认文件发送到群" + this.group.Name, "提示", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                    {
                        this.AddSendFileHandle(fileArray);
                    }
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
        private void AddSendFileItem(System.IO.FileInfo fileInfo)
        {
            if (fileInfo != null)
            {
                if (this.FindFileListItem(fileInfo.FullName) == null)
                {
                    this.ShowFileList();
                    NewFileListItem newFileItem = new NewFileListItem(fileInfo, this, this.group);
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
                IL_D7:
                    i++;
                    continue;
                    goto IL_D7;
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
                    if (e.Source == this.btnScreenshot)
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
            catch (System.Exception)
            {
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
            catch (System.Exception)
            {
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
        private void btnScreenshot_ArrowClick(object sender, RoutedEventArgs e)
        {
            this.popup = new ScreenshotPopup();
            this.popup.StaysOpen = false;
            this.popup.PlacementTarget = this.btnScreenshot;
            this.popup.Placement = PlacementMode.Bottom;
            this.popup.IsOpen = true;
            this.popup.rbtnCutHide.Click += new RoutedEventHandler(this.rbtnCutHide_Click);
            this.popup.rbtnCutShow.Click += new RoutedEventHandler(this.rbtnCutShow_Click);
        }
        private void rbtnCutShow_Click(object sender, RoutedEventArgs e)
        {
            if (this.popup.rbtnCutShow.IsChecked == true)
            {
                this.popup.IsOpen = false;
                System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(this.ShowCutPool));
            }
        }
        private void ShowCutPool(object obj)
        {
            System.Threading.Thread.Sleep(100);
            base.Dispatcher.BeginInvoke(new GroupChatTabControl.ShowCutDelegate(this.ShowCutHandle), new object[0]);
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
            base.Dispatcher.BeginInvoke(new GroupChatTabControl.HideCutDelegate(this.HideCutHandler), new object[0]);
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
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/groupchattabcontrol.xaml", UriKind.Relative);
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
        //        this.btnSendFile = (IconButton)target;
        //        break;
        //    case 3:
        //        this.btnMsgBox = (IconButton)target;
        //        break;
        //    case 4:
        //        this.btnScreenshot = (ImageSplitButton)target;
        //        break;
        //    case 5:
        //        this.btnSendImg = (IconButton)target;
        //        break;
        //    case 6:
        //        this.btnGift = (IconButton)target;
        //        break;
        //    case 7:
        //        this.ChatComponent = (ChatComponent)target;
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
    }
}
