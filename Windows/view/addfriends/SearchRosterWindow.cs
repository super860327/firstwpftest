using IDKin.IM.CustomComponents.Controls;
using IDKin.IM.Data;
using IDKin.IM.Theme.Helper;
using IDKin.IM.Windows.Model;
using IDKin.IM.Windows.Util;
using IDKin.IM.Windows.ViewModel;
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
    public partial class SearchRosterWindow : Window//, IComponentConnector
	{
		private ISessionService sessionService = ServiceUtil.Instance.SessionService;
		private RosterViewModel viewModel = new RosterViewModel();
        //internal SearchRosterWindow searchRosterWindow;
        //internal Border OuterBorder;
        //internal Border InnerBorder;
        //internal StatusBar topBar;
        //internal Image imgIcon;
        //internal ImageButton btnMin;
        //internal ImageButton btnClose;
        //internal TabControl tab;
        //internal TabItem tabItem1;
        //internal RadioButton rbtByin;
        //internal RadioButton rbtByCondition;
        //internal TextBlock tbkInid;
        //internal TextBox tbxInid;
        //internal Canvas conditionCanvas;
        //internal ComboBox comboBox1;
        //internal TextBox comboBox2;
        //internal TextBox textBox1;
        //internal ComboBox comboBox3;
        //internal TextBox textBox2;
        //internal TextBox textBox3;
        //internal ComboBox comboBox4;
        //internal ComboBox textBox4;
        //internal ComboBox textBox5;
        //internal TabItem tabItem2;
        //internal TabItem tabItem3;
        //internal Button btnSearch;
        //internal Button btnCancel;
        //private bool _contentLoaded;
		public SearchRosterWindow()
		{
			ThemeSwitcher.LoadSkin(ThemeEnum.Aero, this);
			this.InitializeComponent();
			this.InitUI();
			this.AddEventListener();
		}
		private void InitUI()
		{
			base.WindowStartupLocation = WindowStartupLocation.CenterScreen;
		}
		private void AddEventListener()
		{
			this.btnSearch.Click += new RoutedEventHandler(this.btnSearch_Click);
			this.btnCancel.Click += new RoutedEventHandler(this.btnCancel_Click);
			base.Closing += new CancelEventHandler(this.SearchRosterWindow_Closing);
		}
		public void btnSearch_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				base.ShowInTaskbar = false;
				base.Visibility = Visibility.Collapsed;
				long rosterUid = ValidationUtil.ChangeUid(this.tbxInid.Text);
				if (rosterUid != 0L)
				{
					WindowModel.Instance.ResultWindow.Owner = this;
					WindowModel.Instance.ResultWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
					WindowModel.Instance.ResultWindow.Show();
					this.viewModel.SearchRosterByUid(rosterUid, this.sessionService.Jid);
				}
			}
			catch (System.Exception ex)
			{
				ServiceUtil.Instance.Logger.Error(ex.ToString());
			}
		}
		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{
			base.Close();
		}
		private void SearchRosterWindow_Closing(object sender, CancelEventArgs e)
		{
			WindowModel.Instance.SearchWindow = null;
		}
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonDown(e);
			base.DragMove();
		}
		private void btnMin_Click(object sender, RoutedEventArgs e)
		{
			base.WindowState = WindowState.Minimized;
		}
		private void btnClose_Click(object sender, RoutedEventArgs e)
		{
			base.Close();
		}
		private void RadioButton_Checked(object sender, RoutedEventArgs e)
		{
			if (this.rbtByin != null && this.rbtByCondition != null)
			{
				if (e.Source == this.rbtByin)
				{
					this.conditionCanvas.Visibility = Visibility.Collapsed;
					this.tbkInid.Visibility = Visibility.Visible;
					this.tbxInid.Visibility = Visibility.Visible;
				}
				if (e.Source == this.rbtByCondition)
				{
					this.tbkInid.Visibility = Visibility.Collapsed;
					this.tbxInid.Visibility = Visibility.Collapsed;
					this.conditionCanvas.Visibility = Visibility.Visible;
				}
			}
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/view/addfriends/searchrosterwindow.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.searchRosterWindow = (SearchRosterWindow)target;
        //        break;
        //    case 2:
        //        this.OuterBorder = (Border)target;
        //        break;
        //    case 3:
        //        this.InnerBorder = (Border)target;
        //        break;
        //    case 4:
        //        this.topBar = (StatusBar)target;
        //        break;
        //    case 5:
        //        this.imgIcon = (Image)target;
        //        break;
        //    case 6:
        //        this.btnMin = (ImageButton)target;
        //        this.btnMin.Click += new RoutedEventHandler(this.btnMin_Click);
        //        break;
        //    case 7:
        //        this.btnClose = (ImageButton)target;
        //        this.btnClose.Click += new RoutedEventHandler(this.btnClose_Click);
        //        break;
        //    case 8:
        //        this.tab = (TabControl)target;
        //        break;
        //    case 9:
        //        this.tabItem1 = (TabItem)target;
        //        break;
        //    case 10:
        //        this.rbtByin = (RadioButton)target;
        //        this.rbtByin.Checked += new RoutedEventHandler(this.RadioButton_Checked);
        //        break;
        //    case 11:
        //        this.rbtByCondition = (RadioButton)target;
        //        this.rbtByCondition.Checked += new RoutedEventHandler(this.RadioButton_Checked);
        //        break;
        //    case 12:
        //        this.tbkInid = (TextBlock)target;
        //        break;
        //    case 13:
        //        this.tbxInid = (TextBox)target;
        //        break;
        //    case 14:
        //        this.conditionCanvas = (Canvas)target;
        //        break;
        //    case 15:
        //        this.comboBox1 = (ComboBox)target;
        //        break;
        //    case 16:
        //        this.comboBox2 = (TextBox)target;
        //        break;
        //    case 17:
        //        this.textBox1 = (TextBox)target;
        //        break;
        //    case 18:
        //        this.comboBox3 = (ComboBox)target;
        //        break;
        //    case 19:
        //        this.textBox2 = (TextBox)target;
        //        break;
        //    case 20:
        //        this.textBox3 = (TextBox)target;
        //        break;
        //    case 21:
        //        this.comboBox4 = (ComboBox)target;
        //        break;
        //    case 22:
        //        this.textBox4 = (ComboBox)target;
        //        break;
        //    case 23:
        //        this.textBox5 = (ComboBox)target;
        //        break;
        //    case 24:
        //        this.tabItem2 = (TabItem)target;
        //        break;
        //    case 25:
        //        this.tabItem3 = (TabItem)target;
        //        break;
        //    case 26:
        //        this.btnSearch = (Button)target;
        //        break;
        //    case 27:
        //        this.btnCancel = (Button)target;
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
