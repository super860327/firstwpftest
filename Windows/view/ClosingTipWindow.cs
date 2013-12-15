using IDKin.IM.CustomComponents.Controls;
using IDKin.IM.Theme.Helper;
using IDKin.IM.Windows.Model;
using IDKin.IM.Windows.Properties;
using IDKin.IM.Windows.Util;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
namespace IDKin.IM.Windows.View
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class ClosingTipWindow : Window//, IComponentConnector
	{
        //internal ClosingTipWindow closingTipWindow;
        //internal Border OuterBorder;
        //internal Border InnerBorder;
        //internal Grid RootGrid;
        //internal StatusBar topBar;
        //internal Image imgIcon;
        //internal ImageButton btnClose;
        //internal RadioButton rboHide;
        //internal RadioButton rboExit;
        //internal CheckBox chkExitRemind;
        //private bool _contentLoaded;
		public ClosingTipWindow()
		{
			ThemeSwitcher.LoadSkin(ThemeEnum.Aero, this);
			this.InitializeComponent();
			this.rboHide.IsChecked = new bool?(Settings.Default.SystemSetup_Base_ExitHide);
			this.rboExit.IsChecked = new bool?(!Settings.Default.SystemSetup_Base_ExitHide);
			this.chkExitRemind.IsChecked = new bool?(Settings.Default.ExitRemind);
		}
		private void CloseHandler(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(false);
			base.Close();
		}
		private void SureHandler(object sender, RoutedEventArgs e)
		{
			Settings.Default.ExitRemind = this.chkExitRemind.IsChecked.Value;
			Settings.Default.SystemSetup_Base_ExitHide = this.rboHide.IsChecked.Value;
			Settings.Default.Save();
			if (Settings.Default.SystemSetup_Base_ExitHide)
			{
				base.DialogResult = new bool?(false);
				ServiceUtil.Instance.DataService.INWindow.WindowState = WindowState.Minimized;
			}
			else
			{
				INWindow inWindow = base.Owner as INWindow;
				if (inWindow != null)
				{
					inWindow.IsTaskClose = false;
				}
				base.DialogResult = new bool?(true);
				Application.Current.Shutdown(1000);
			}
			base.Close();
		}
		private void StatusBar_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				base.DragMove();
			}
		}
		private void closingTipWindow_Closing(object sender, CancelEventArgs e)
		{
			WindowModel.Instance.ClosingTipWindow = null;
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/view/closingtipwindow.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.closingTipWindow = (ClosingTipWindow)target;
        //        this.closingTipWindow.Closing += new CancelEventHandler(this.closingTipWindow_Closing);
        //        break;
        //    case 2:
        //        this.OuterBorder = (Border)target;
        //        break;
        //    case 3:
        //        this.InnerBorder = (Border)target;
        //        break;
        //    case 4:
        //        this.RootGrid = (Grid)target;
        //        break;
        //    case 5:
        //        this.topBar = (StatusBar)target;
        //        this.topBar.MouseLeftButtonDown += new MouseButtonEventHandler(this.StatusBar_MouseDown);
        //        break;
        //    case 6:
        //        this.imgIcon = (Image)target;
        //        break;
        //    case 7:
        //        this.btnClose = (ImageButton)target;
        //        this.btnClose.Click += new RoutedEventHandler(this.CloseHandler);
        //        break;
        //    case 8:
        //        this.rboHide = (RadioButton)target;
        //        break;
        //    case 9:
        //        this.rboExit = (RadioButton)target;
        //        break;
        //    case 10:
        //        this.chkExitRemind = (CheckBox)target;
        //        break;
        //    case 11:
        //        ((Button)target).Click += new RoutedEventHandler(this.SureHandler);
        //        break;
        //    case 12:
        //        ((Button)target).Click += new RoutedEventHandler(this.CloseHandler);
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
