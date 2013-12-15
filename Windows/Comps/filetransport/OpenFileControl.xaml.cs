using IDKin.IM.Communicate;
using IDKin.IM.Log;
using IDKin.IM.Windows.Util;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
namespace IDKin.IM.Windows.Comps.FileTransport
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public partial class OpenFileControl : UserControl//, IComponentConnector
	{
		private FileItem fileItem;
		private ILogger logger = ServiceUtil.Instance.Logger;
		private bool isSuccess = false;
		private string fileDirectoryName;
        ////internal Image FileIcon;
        ////internal TextBlock FileName;
        ////internal TextBlock OpenFile;
        ////internal TextBlock OpenFolder;
        ////private bool _contentLoaded;
		public OpenFileControl(FileItem fileItem, bool isSuccess)
		{
			this.InitializeComponent();
			try
			{
				if (fileItem != null)
				{
					this.fileItem = fileItem;
					this.isSuccess = isSuccess;
					System.IO.FileInfo info = new System.IO.FileInfo(fileItem.FileName);
					this.fileDirectoryName = info.DirectoryName;
					this.FileIcon.Source = this.IconDecode(this.fileItem.IconBase64);
					this.FileName.Text = info.Name;
					if (isSuccess)
					{
						this.OpenFile.Visibility = Visibility.Visible;
						this.OpenFolder.Visibility = Visibility.Visible;
					}
				}
			}
			catch (System.Exception e)
			{
				this.logger.Error(e.ToString());
			}
		}
		private void OpenFile_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				string filePath = this.fileItem.FileName;
				if (!System.IO.File.Exists(filePath))
				{
					MessageBox.Show("该文件已经不存在");
				}
				else
				{
					Process.Start(filePath);
				}
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		private void OpenFolder_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				string filePath = this.fileItem.FileName;
				if (!System.IO.File.Exists(filePath))
				{
					MessageBox.Show("该文件夹已经不存在");
				}
				else
				{
					string argument = "/select,\"" + filePath + "\"";
					Process.Start("explorer.exe", argument);
				}
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		private BitmapImage IconDecode(string base64)
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
					this.logger.Error(ex.ToString());
				}
			}
			return bi;
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/filetransport/openfilecontrol.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.FileIcon = (Image)target;
        //        break;
        //    case 2:
        //        this.FileName = (TextBlock)target;
        //        break;
        //    case 3:
        //        this.OpenFile = (TextBlock)target;
        //        break;
        //    case 4:
        //        ((Hyperlink)target).Click += new RoutedEventHandler(this.OpenFile_Click);
        //        break;
        //    case 5:
        //        this.OpenFolder = (TextBlock)target;
        //        break;
        //    case 6:
        //        ((Hyperlink)target).Click += new RoutedEventHandler(this.OpenFolder_Click);
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
