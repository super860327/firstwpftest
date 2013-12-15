using IDKin.IM.Theme.Helper;
using IDKin.IM.Windows.BindingVO;
using IDKin.IM.Windows.Comps;
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
    public partial class RequestResultWindow : Window//, IComponentConnector
	{
		public RequestResultVO resultVO = null;
        //internal RequestResultWindow requestResultWindow;
        //internal Border OuterBorder;
        //internal Border InnerBorder;
        //internal StatusBar topBar;
        //internal Image imgIcon;
        //internal CloseButton btnClose;
        //private bool _contentLoaded;
		public RequestResultWindow()
		{
			ThemeSwitcher.LoadSkin(ThemeEnum.Aero, this);
			this.InitializeComponent();
		}
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonDown(e);
			base.DragMove();
		}
		private void btnClose_Click(object sender, RoutedEventArgs e)
		{
			base.Close();
		}
		private void MouseLeftButtonUpHandler(object sender, MouseButtonEventArgs e)
		{
			base.Close();
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/view/addfriends/requestresultwindow.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[System.Diagnostics.DebuggerNonUserCode]
        //internal System.Delegate _CreateDelegate(System.Type delegateType, string handler)
        //{
        //    return System.Delegate.CreateDelegate(delegateType, this, handler);
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.requestResultWindow = (RequestResultWindow)target;
        //        this.requestResultWindow.MouseLeftButtonUp += new MouseButtonEventHandler(this.MouseLeftButtonUpHandler);
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
        //        this.btnClose = (CloseButton)target;
        //        break;
        //    case 7:
        //        ((Button)target).Click += new RoutedEventHandler(this.btnClose_Click);
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
