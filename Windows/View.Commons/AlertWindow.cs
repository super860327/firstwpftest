using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
namespace IDKin.IM.Windows.View.Commons
{
	public partial class AlertWindow : Window//, IComponentConnector
	{
        //internal TextBlock tbkCaption;
        //internal Button btnClose;
        //internal Image imgIcon;
        //internal TextBlock tbkMessage;
        //internal StackPanel buttonsStackPanel;
        //internal Button btnOk;
        //internal Button btnCancel;
        //internal Button btnYes;
        //internal Button btnNo;
		public AlertWindow()
		{
			this.InitializeComponent();
			this.Init();
		}
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonDown(e);
			base.DragMove();
		}
		private void Init()
		{
			base.WindowStyle = WindowStyle.None;
			base.AllowsTransparency = true;
			base.Topmost = true;
			base.ShowInTaskbar = false;
			base.ShowActivated = false;
			base.ResizeMode = ResizeMode.NoResize;
			base.Background = new SolidColorBrush(Colors.Transparent);
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
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/commons/alertwindow.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.tbkCaption = (TextBlock)target;
        //        break;
        //    case 2:
        //        this.btnClose = (Button)target;
        //        this.btnClose.Click += new RoutedEventHandler(this.btnClose_Click);
        //        break;
        //    case 3:
        //        this.imgIcon = (Image)target;
        //        break;
        //    case 4:
        //        this.tbkMessage = (TextBlock)target;
        //        break;
        //    case 5:
        //        this.buttonsStackPanel = (StackPanel)target;
        //        break;
        //    case 6:
        //        this.btnOk = (Button)target;
        //        break;
        //    case 7:
        //        this.btnCancel = (Button)target;
        //        break;
        //    case 8:
        //        this.btnYes = (Button)target;
        //        break;
        //    case 9:
        //        this.btnNo = (Button)target;
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
