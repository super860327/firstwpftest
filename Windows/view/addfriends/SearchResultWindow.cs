using IDKin.IM.CustomComponents.Controls;
using IDKin.IM.Theme.Helper;
using IDKin.IM.Windows.Model;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
namespace IDKin.IM.Windows.View.AddFriends
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class SearchResultWindow : Window//, IComponentConnector
	{
        //internal Border OuterBorder;
        //internal Border InnerBorder;
        //internal StatusBar topBar;
        //internal Image imgIcon;
        //internal ImageButton btnMin;
        //internal ImageButton btnClose;
        //internal ListView resultList;
        //internal Button btnBack;
        //internal Button btnCancel;
        //private bool _contentLoaded;
		public SearchResultWindow()
		{
			ThemeSwitcher.LoadSkin(ThemeEnum.Aero, this);
			this.InitializeComponent();
			this.InitUI();
			this.AddEventListener();
		}
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonDown(e);
			base.DragMove();
		}
		private void InitUI()
		{
		}
		private void AddEventListener()
		{
			base.Closing += new CancelEventHandler(this.SearchResultWindow_Closing);
			this.btnBack.Click += new RoutedEventHandler(this.btnBack_Click);
			this.btnCancel.Click += new RoutedEventHandler(this.btnCancel_Click);
		}
		private void btnBack_Click(object sender, RoutedEventArgs e)
		{
			base.Close();
			this.GoBackHandler();
		}
		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{
			base.Close();
		}
		private void SearchResultWindow_Closing(object sender, CancelEventArgs e)
		{
			WindowModel.Instance.ResultWindow = null;
			this.GoBackHandler();
		}
		private void GoBackHandler()
		{
			WindowModel.Instance.SearchWindow.ShowInTaskbar = true;
			WindowModel.Instance.SearchWindow.Visibility = Visibility.Visible;
		}
		private void btnMin_Click(object sender, RoutedEventArgs e)
		{
			base.WindowState = WindowState.Minimized;
		}
		private void btnClose_Click(object sender, RoutedEventArgs e)
		{
			base.Close();
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/view/addfriends/searchresultwindow.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.OuterBorder = (Border)target;
        //        break;
        //    case 2:
        //        this.InnerBorder = (Border)target;
        //        break;
        //    case 3:
        //        this.topBar = (StatusBar)target;
        //        break;
        //    case 4:
        //        this.imgIcon = (Image)target;
        //        break;
        //    case 5:
        //        this.btnMin = (ImageButton)target;
        //        this.btnMin.Click += new RoutedEventHandler(this.btnMin_Click);
        //        break;
        //    case 6:
        //        this.btnClose = (ImageButton)target;
        //        this.btnClose.Click += new RoutedEventHandler(this.btnClose_Click);
        //        break;
        //    case 7:
        //        this.resultList = (ListView)target;
        //        break;
        //    case 8:
        //        this.btnBack = (Button)target;
        //        break;
        //    case 9:
        //        this.btnCancel = (Button)target;
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
