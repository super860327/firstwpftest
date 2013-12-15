using IDKin.IM.Data;
using IDKin.IM.Windows.Util;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
namespace IDKin.IM.Windows.Comps
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class ScreenshotPopup : Popup//, IComponentConnector
	{
		private ISessionService sessionService = ServiceUtil.Instance.SessionService;
		//internal RadioButton rbtnCutShow;
		//internal RadioButton rbtnCutHide;
        ////private bool _contentLoaded;
		public ScreenshotPopup()
		{
			this.InitializeComponent();
			this.rbtnCutShow.IsChecked = new bool?(!this.sessionService.IsCutScreenHidenWindow);
			this.rbtnCutHide.IsChecked = new bool?(this.sessionService.IsCutScreenHidenWindow);
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/screenshotpopup.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.rbtnCutShow = (RadioButton)target;
        //        break;
        //    case 2:
        //        this.rbtnCutHide = (RadioButton)target;
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
