using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
namespace IDKin.IM.Windows.Comps
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class CustomMemberItem : ListBoxItem//, IComponentConnector
	{
		private Button iconButton = null;
		private int iconType = 0;
		//internal Image imgHead;
		//internal TextBlock tbkAccount;
		//private bool _contentLoaded;
		public event System.EventHandler ItemDelete;
		public event System.EventHandler ItemAdd;
		public CustomMemberItem(CustomMemberType type)
		{
			this.InitializeComponent();
			this.iconType = (int)type;
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.iconButton = (base.Template.FindName("PART_IconButton", this) as Button);
			if (this.iconButton != null)
			{
				switch (this.iconType)
				{
				case 0:
					base.Tag = new Image
					{
						Source = this.SetButtonIcon("/IDKin.IM.Windows;component/Resources/Icon/addIcon.png")
					};
					this.iconButton.ToolTip = "添加";
					this.iconButton.Click += new RoutedEventHandler(this.AddItemHandler);
					break;
				case 1:
					base.Tag = new Image
					{
						Source = this.SetButtonIcon("/IDKin.IM.Windows;component/Resources/Icon/deleteIcon.png")
					};
					this.iconButton.ToolTip = "删除";
					this.iconButton.Click += new RoutedEventHandler(this.DeleteItemHandler);
					break;
				}
			}
		}
		private void AddItemHandler(object sender, RoutedEventArgs e)
		{
			if (this.ItemAdd != null)
			{
				base.IsSelected = true;
				this.ItemAdd(sender, null);
			}
		}
		private void DeleteItemHandler(object sender, RoutedEventArgs e)
		{
			if (this.ItemDelete != null)
			{
				base.IsSelected = true;
				this.ItemDelete(sender, null);
			}
		}
		private BitmapImage SetButtonIcon(string uri)
		{
			BitmapImage icon = new BitmapImage();
			icon.BeginInit();
			icon.UriSource = new Uri(uri, UriKind.RelativeOrAbsolute);
			icon.EndInit();
			return icon;
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/custommemberitem.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.imgHead = (Image)target;
        //        break;
        //    case 2:
        //        this.tbkAccount = (TextBlock)target;
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
