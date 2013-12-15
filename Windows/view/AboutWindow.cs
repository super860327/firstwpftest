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
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
namespace IDKin.IM.Windows.View
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class AboutWindow : Window//, IComponentConnector
	{
        //internal AboutWindow aboutWindow;
        //internal Border OuterBorder;
        //internal Border InnerBorder;
        //internal Grid RootGrid;
        //internal StatusBar topBar;
        //internal Image imgIcon;
        //internal ImageButton btnClose;
        //internal TextBlock tbkVersion;
        //internal RichTextBox tbxChangelog;
        //private bool _contentLoaded;
		public AboutWindow()
		{
			ThemeSwitcher.LoadSkin(ThemeEnum.Aero, this);
			this.InitializeComponent();
			this.InitDataHandler();
		}
		private void InitDataHandler()
		{
			TextBlock expr_07 = this.tbkVersion;
			expr_07.Text += AppUtil.Instance.AppVersion();
			this.SetChangelog(IDKin.IM.Windows.Properties.Resources.Changelog);
		}
		private void SetChangelog(string content)
		{
			Paragraph paragraph = new Paragraph();
			paragraph.Inlines.Add(new Run(content));
			this.tbxChangelog.Document.Blocks.Add(paragraph);
		}
		private void GoWebSiteHandler(object sender, MouseButtonEventArgs e)
		{
			BrowserUtil.OpenHyperlinkHandler("http://www.idkin.com");
		}
		private void CloseHandler(object sender, RoutedEventArgs e)
		{
			base.Close();
		}
		private void aboutWindow_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				base.Close();
			}
		}
		private void aboutWindow_Closing(object sender, CancelEventArgs e)
		{
			WindowModel.Instance.AboutWindow = null;
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/view/aboutwindow.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.aboutWindow = (AboutWindow)target;
        //        this.aboutWindow.KeyDown += new KeyEventHandler(this.aboutWindow_KeyDown);
        //        this.aboutWindow.Closing += new CancelEventHandler(this.aboutWindow_Closing);
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
        //        break;
        //    case 6:
        //        this.imgIcon = (Image)target;
        //        break;
        //    case 7:
        //        this.btnClose = (ImageButton)target;
        //        this.btnClose.Click += new RoutedEventHandler(this.CloseHandler);
        //        break;
        //    case 8:
        //        this.tbkVersion = (TextBlock)target;
        //        break;
        //    case 9:
        //        ((TextBlock)target).MouseLeftButtonDown += new MouseButtonEventHandler(this.GoWebSiteHandler);
        //        break;
        //    case 10:
        //        this.tbxChangelog = (RichTextBox)target;
        //        break;
        //    case 11:
        //        ((Button)target).Click += new RoutedEventHandler(this.CloseHandler);
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
