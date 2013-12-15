using IDKin.IM.Core;
using IDKin.IM.Data;
using IDKin.IM.Windows.Model;
using IDKin.IM.Windows.Util;
using IDKin.IM.Windows.View;
using IDKin.IM.Windows.ViewModel;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
namespace IDKin.IM.Windows.Comps
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class CustomGroupTreeViewItem : TreeViewItem//, IComponentConnector
	{
		public class CustomEventArgs : System.EventArgs
		{
			public CustomGroupTreeViewItem Item
			{
				get;
				set;
			}
		}
		private CustomGroupManagerWindowViewModel customGroupManagerViewModel;
		private IDataService dataService;
		private ISessionService sessionService;
		private INWindow inWindow;
		public AddMemberCustomGroupWindow addMemberCustomGroupWindow;
		//internal TextBlock tBlock;
		//internal TextBox tb;
        ////private bool _contentLoaded;
		public event CustomGroupHandler CreateCustomItemEvent;
		public event CustomGroupHandler DeleteCustomItemEvent;
		public event CustomGroupHandler UpdateCustomItemEvent;
		public string HeaderText
		{
			get
			{
				return this.tBlock.Text;
			}
			set
			{
				this.tBlock.Text = value;
				this.tb.Text = value;
			}
		}
		public CustomGroupTreeViewItem()
		{
			this.InitializeComponent();
			this.customGroupManagerViewModel = CustomGroupManagerWindowViewModel.GetInstance();
			this.tBlock.ContextMenu = this.GetContextMenu();
			this.tBlock.Text = "新建分组";
			this.sessionService = ServiceUtil.Instance.SessionService;
			this.dataService = ServiceUtil.Instance.DataService;
			this.inWindow = (this.dataService.INWindow as INWindow);
			this.tb.LostFocus += new RoutedEventHandler(this.LostFocus_HandleEvent);
			this.tb.KeyUp += delegate(object s, KeyEventArgs ee)
			{
				if (ee.Key == Key.Return)
				{
					this.TextBoxLostFocus();
				}
			};
			this.tb.SelectAll();
			base.MouseRightButtonDown += new MouseButtonEventHandler(this.CustomGroupTreeViewItem_MouseRightButtonDown);
			base.Loaded += new RoutedEventHandler(this.CustomGroupTreeViewItem_Loaded);
		}
		private void LostFocus_HandleEvent(object sender, RoutedEventArgs e)
		{
			this.TextBoxLostFocus();
		}
		private void TextBoxLostFocus()
		{
			if (string.IsNullOrWhiteSpace(this.tb.Text) && base.Items.Count > 0)
			{
                //base.Dispatcher.BeginInvoke(delegate
                //{
                //    this.tb.Focus();
                //}, new object[0]);
			}
			else
			{
				if (string.IsNullOrWhiteSpace(this.tb.Text) && base.Items.Count == 0)
				{
					if (this.DeleteCustomItemEvent != null)
					{
						this.DeleteCustomItemEvent(this, null);
					}
				}
				else
				{
					this.NewCustomGroup();
				}
			}
		}
		private void CustomGroupTreeViewItem_Loaded(object sender, RoutedEventArgs e)
		{
			this.tb.Focus();
		}
		private void NewCustomGroup()
		{
			this.tBlock.Text = this.tBlock.Text.Trim();
			this.tb.Text = this.tBlock.Text;
			this.tb.Visibility = Visibility.Hidden;
			this.UpdateCustomItemEvent(this, null);
		}
		private void CustomGroupTreeViewItem_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
			TreeNodeCustomStaff node = e.Source as TreeNodeCustomStaff;
			if (node != null)
			{
				node.Focus();
				e.Handled = true;
			}
			else
			{
				CustomGroupTreeViewItem customGroupTreeViewItem = sender as CustomGroupTreeViewItem;
				if (customGroupTreeViewItem != null)
				{
					customGroupTreeViewItem.Focus();
					e.Handled = true;
				}
			}
		}
		private ContextMenu GetContextMenu()
		{
			ContextMenu cMenu = new ContextMenu();
			MenuItem item = new MenuItem();
			item.Header = "重命名";
			item.Click += new RoutedEventHandler(this.miRename_Click);
			cMenu.Items.Add(item);
			item = new MenuItem();
			item.Header = "添加分组";
			item.Click += new RoutedEventHandler(this.miAddGroup_Click);
			cMenu.Items.Add(item);
			item = new MenuItem();
			item.Header = "删除分组";
			item.Click += new RoutedEventHandler(this.miDelGroup);
			cMenu.Items.Add(item);
			item = new MenuItem();
			item.Header = "添加联系人";
			item.Click += new RoutedEventHandler(this.miAdd_Click);
			cMenu.Items.Add(item);
			return cMenu;
		}
		private void miRename_Click(object sender, RoutedEventArgs e)
		{
			this.tb.Visibility = Visibility.Visible;
			this.tb.Focus();
			this.tb.SelectAll();
		}
		private void miAdd_Click(object sender, RoutedEventArgs e)
		{
			if (base.DataContext != null)
			{
				CustomGroup c = base.DataContext as CustomGroup;
				if (c != null)
				{
					this.addMemberCustomGroupWindow = new AddMemberCustomGroupWindow(c.GroupID);
					this.addMemberCustomGroupWindow.Owner = ServiceUtil.Instance.DataService.INWindow;
					this.addMemberCustomGroupWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
					this.addMemberCustomGroupWindow.ShowDialog();
				}
			}
		}
		private void miAddGroup_Click(object sender, RoutedEventArgs ee)
		{
			CustomGroupTreeViewItem item = new CustomGroupTreeViewItem();
			item.Style = (base.FindResource("TreeViewItemStyle") as Style);
			string id = CustomGroup.GetCustomGroupID();
			CustomGroup cc = new CustomGroup
			{
				Admin = ServiceUtil.Instance.SessionService.Uid,
				GroupID = id,
				GroupName = item.HeaderText
			};
			item.DataContext = cc;
			item.Tag = id;
			CustomGroupManagerWindowViewModel viewModel = CustomGroupManagerWindowViewModel.GetInstance();
			item.CreateCustomItemEvent += delegate(object s, CustomGroupTreeViewItem.CustomEventArgs e)
			{
				if (e.Item != null)
				{
					this.inWindow.Employee.FirstTreeView.Items.Add(e.Item);
					CustomGroup c = e.Item.DataContext as CustomGroup;
					if (c != null)
					{
						DataModel.Instance.CustomeGroupName.Add(c.GroupID, c);
						viewModel.CreateCustomGroup(this.sessionService.Uid, c.GroupID, e.Item.HeaderText);
					}
				}
			};
			item.DeleteCustomItemEvent += delegate(object s, CustomGroupTreeViewItem.CustomEventArgs e)
			{
				DataModel.Instance.CustomeGroupName.Remove(item.Tag.ToString());
				this.inWindow.Employee.FirstTreeView.Items.Remove(item);
				viewModel.DropCustomGroup((int)this.sessionService.Uid, item.Tag.ToString());
			};
			item.UpdateCustomItemEvent += delegate(object s, CustomGroupTreeViewItem.CustomEventArgs e)
			{
				if (DataModel.Instance.CustomeGroupName.ContainsKey(item.Tag.ToString()))
				{
					DataModel.Instance.CustomeGroupName[item.Tag.ToString()].GroupName = item.HeaderText;
					viewModel.UpdateCustomGroup((int)this.sessionService.Uid, item.Tag.ToString(), item.HeaderText);
				}
			};
			if (this.CreateCustomItemEvent != null)
			{
				this.CreateCustomItemEvent(this, new CustomGroupTreeViewItem.CustomEventArgs
				{
					Item = item
				});
			}
			this.SetFocus(item);
			item.IsSelected = true;
		}
		private void miDelGroup(object sender, RoutedEventArgs e)
		{
			if (base.Items.Count > 0)
			{
				MessageBoxResult result = MessageBox.Show("删除此分组将会同时删除组内的联系人，您确认要删除吗？", "确认自定义分组删除", MessageBoxButton.OKCancel, MessageBoxImage.Question);
				if (result == MessageBoxResult.OK)
				{
					if (this.DeleteCustomItemEvent != null)
					{
						this.DeleteCustomItemEvent(this, null);
					}
				}
			}
			else
			{
				if (this.DeleteCustomItemEvent != null)
				{
					this.DeleteCustomItemEvent(this, null);
				}
			}
		}
		private void SetFocus(CustomGroupTreeViewItem item)
		{
			Grid grid = item.Header as Grid;
			if (grid != null)
			{
				if (grid.Children.Count > 1 && grid.Children[1] is TextBox)
				{
					TextBox textBox = grid.Children[1] as TextBox;
					textBox.Visibility = Visibility.Visible;
					textBox.SelectAll();
					textBox.Focus();
				}
			}
		}
		private void CreateGroup(string name, string desc)
		{
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/customgrouptreeviewitem.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.tBlock = (TextBlock)target;
        //        break;
        //    case 2:
        //        this.tb = (TextBox)target;
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
