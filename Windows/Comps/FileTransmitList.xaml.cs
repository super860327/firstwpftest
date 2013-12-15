using IDKin.IM.Data;
using IDKin.IM.Windows.Properties;
using IDKin.IM.Windows.Util;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Markup;
namespace IDKin.IM.Windows.Comps
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class FileTransmitList : Border//, IComponentConnector
	{
		private ISessionService sessionService = ServiceUtil.Instance.SessionService;
		//internal System.Windows.Controls.ListBox fileList;
		//internal UniformGrid splPanel;
		//internal System.Windows.Controls.Button buttonAllAccept;
		//internal System.Windows.Controls.Button buttonAllSaveAs;
		//internal System.Windows.Controls.Button buttonAllRecpt;
		//private bool _contentLoaded;
		private string fileDir
		{
			get
			{
				return Settings.Default.SystemSetup_FileTransport_SaveDir;
			}
		}
		public FileTransmitList()
		{
			this.InitializeComponent();
		}
		private void buttonAllAccept_Click(object sender, RoutedEventArgs e)
		{
			if (this.hasReceivedFile())
			{
				System.Array items = this.CopyFileListItems();
				System.Collections.Generic.List<NewFileListItem> temNewFileListItems = new System.Collections.Generic.List<NewFileListItem>();
				foreach (object obj in items)
				{
					NewFileListItem item = obj as NewFileListItem;
					if (item != null)
					{
						temNewFileListItems.Add(item);
					}
				}
				this.MakeSameNameFile(temNewFileListItems);
				int existedFileCount = this.GetExistedFileCount(this.fileDir, temNewFileListItems);
				MessageBoxResult result = MessageBoxResult.None;
				if (existedFileCount != 0)
				{
					result = System.Windows.MessageBox.Show(string.Format("接收文件目录:{0},一共存在({1})个同名文件.是:全部覆盖,否：自动命名", this.fileDir, existedFileCount), "友情提示", MessageBoxButton.YesNoCancel);
					if (result == MessageBoxResult.Cancel)
					{
						return;
					}
				}
				this.MakeSameNameFile(temNewFileListItems);
				System.Collections.Generic.List<string> processingFileNames = new System.Collections.Generic.List<string>();
				foreach (object obj in temNewFileListItems)
				{
					NewFileListItem item = obj as NewFileListItem;
					if (item != null)
					{
						item.SaveAs(this.fileDir, this.GetExistedFileCount(this.fileDir, temNewFileListItems), processingFileNames, result);
					}
				}
			}
		}
		private int GetExistedFileCount(string selectedPath, System.Collections.Generic.List<NewFileListItem> items)
		{
			int count = 0;
			foreach (NewFileListItem item in items)
			{
				if (!item.AutoRename && !item.IsProcessing)
				{
					if (this.FileExists(selectedPath + item.FileName))
					{
						count++;
					}
				}
			}
			return count;
		}
		private bool FileExists(string filePath)
		{
			return System.IO.File.Exists(filePath);
		}
		private bool HasSameNameFile(System.Collections.Generic.List<NewFileListItem> items)
		{
			bool result;
			for (int i = 0; i < items.Count; i++)
			{
				for (int j = i + 1; j < items.Count; j++)
				{
					if (items[i].FileName == items[j].FileName)
					{
						result = true;
						return result;
					}
				}
			}
			result = false;
			return result;
		}
		private void MakeSameNameFile(System.Collections.Generic.List<NewFileListItem> items)
		{
			for (int i = 0; i < items.Count; i++)
			{
				for (int j = i + 1; j < items.Count; j++)
				{
					if (items[i].FileName == items[j].FileName)
					{
						items[j].AutoRename = true;
					}
				}
			}
		}
		private void buttonAllSaveAs_Click(object sender, RoutedEventArgs e)
		{
			if (this.hasReceivedFile())
			{
				System.Array items = this.CopyFileListItems();
				System.Collections.Generic.List<NewFileListItem> temNewFileListItems = new System.Collections.Generic.List<NewFileListItem>();
				foreach (object obj in items)
				{
					NewFileListItem item = obj as NewFileListItem;
					if (item != null)
					{
						temNewFileListItems.Add(item);
					}
				}
				FolderBrowserDialog fdb = new FolderBrowserDialog();
				fdb.SelectedPath = this.sessionService.SaveAsFilePath;
				DialogResult dr = fdb.ShowDialog();
				if (DialogResult.OK == dr && fdb.SelectedPath != string.Empty)
				{
					this.MakeSameNameFile(temNewFileListItems);
					int existedFileCount = this.GetExistedFileCount(fdb.SelectedPath + "\\", temNewFileListItems);
					MessageBoxResult result = MessageBoxResult.None;
					if (existedFileCount != 0)
					{
						result = System.Windows.MessageBox.Show(string.Format("接收文件目录:{0},一共存在({1})个同名文件.是:全部覆盖,否：自动命名", fdb.SelectedPath + "\\", existedFileCount), "友情提示", MessageBoxButton.YesNoCancel);
						if (result == MessageBoxResult.Cancel)
						{
							return;
						}
					}
					this.MakeSameNameFile(temNewFileListItems);
					System.Collections.Generic.List<string> processingFileNames = new System.Collections.Generic.List<string>();
					foreach (object obj in temNewFileListItems)
					{
						NewFileListItem item = obj as NewFileListItem;
						if (item != null)
						{
							item.SaveAs(fdb.SelectedPath + "\\", this.GetExistedFileCount(fdb.SelectedPath + "\\", temNewFileListItems), processingFileNames, result);
						}
					}
					this.sessionService.SaveAsFilePath = fdb.SelectedPath + "\\";
				}
			}
		}
		private void buttonAllRecpt_Click(object sender, RoutedEventArgs e)
		{
			if (this.hasReceivedFile())
			{
				System.Array items = this.CopyFileListItems();
				foreach (object obj in items)
				{
					NewFileListItem item = obj as NewFileListItem;
					if (item != null)
					{
						item.Refuse();
					}
				}
			}
		}
		private bool hasReceivedFile()
		{
			bool result;
			for (int i = 0; i < this.fileList.Items.Count; i++)
			{
				NewFileListItem item = this.fileList.Items[i] as NewFileListItem;
				if (item != null && item.FileInfo == null)
				{
					result = true;
					return result;
				}
			}
			result = false;
			return result;
		}
		private NewFileListItem[] CopyFileListItems()
		{
			NewFileListItem[] items = new NewFileListItem[this.fileList.Items.Count];
			for (int i = 0; i < this.fileList.Items.Count; i++)
			{
				items[i] = (this.fileList.Items[i] as NewFileListItem);
			}
			return items;
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/filetransmitlist.xaml", UriKind.Relative);
        //        System.Windows.Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.fileList = (System.Windows.Controls.ListBox)target;
        //        break;
        //    case 2:
        //        this.splPanel = (UniformGrid)target;
        //        break;
        //    case 3:
        //        this.buttonAllAccept = (System.Windows.Controls.Button)target;
        //        this.buttonAllAccept.Click += new RoutedEventHandler(this.buttonAllAccept_Click);
        //        break;
        //    case 4:
        //        this.buttonAllSaveAs = (System.Windows.Controls.Button)target;
        //        this.buttonAllSaveAs.Click += new RoutedEventHandler(this.buttonAllSaveAs_Click);
        //        break;
        //    case 5:
        //        this.buttonAllRecpt = (System.Windows.Controls.Button)target;
        //        this.buttonAllRecpt.Click += new RoutedEventHandler(this.buttonAllRecpt_Click);
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
