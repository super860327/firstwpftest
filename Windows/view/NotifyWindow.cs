using IDKin.IM.Core;
using IDKin.IM.CustomComponents.Controls;
using IDKin.IM.Windows.Util;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
namespace IDKin.IM.Windows.View
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class NotifyWindow : DesktopAlertBase//, IComponentConnector
	{
		private string link;
        //internal TextBlock tbkMsg;
        //internal TextBlock tbkTitle;
        //internal TextBlock tbkView;
        //private bool _contentLoaded;
		public NotifyWindow(Notice notice)
		{
			this.InitializeComponent();
			this.tbkMsg.Text = notice.Message;
			this.tbkTitle.Text = notice.Title;
			this.link = notice.Link;
			this.InitEvent(notice.MessageType);
		}
		private void InitEvent(int type)
		{
			if (type == 2)
			{
				this.tbkTitle.MouseLeftButtonDown += new MouseButtonEventHandler(this.ViewLoggerNoticeHandler);
				this.tbkView.MouseLeftButtonDown += new MouseButtonEventHandler(this.ViewLoggerNoticeHandler);
			}
			else
			{
				this.tbkTitle.MouseLeftButtonDown += new MouseButtonEventHandler(this.ViewWorkServicesHandler);
				this.tbkView.MouseLeftButtonDown += new MouseButtonEventHandler(this.ViewWorkServicesHandler);
			}
		}
		private void ViewLoggerNoticeHandler(object sender, MouseButtonEventArgs e)
		{
			try
			{
				BrowserUtil.OpenHyperlinkHandler(this.link);
			}
			catch (System.Exception)
			{
			}
			finally
			{
				base.Close();
			}
		}
		private void ViewWorkServicesHandler(object sender, MouseButtonEventArgs e)
		{
			try
			{
				BrowserUtil.OpenNoticBrowserHandler(this.link);
			}
			catch (System.Exception)
			{
			}
			finally
			{
				base.Close();
			}
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/view/notifywindow.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.tbkMsg = (TextBlock)target;
        //        break;
        //    case 2:
        //        this.tbkTitle = (TextBlock)target;
        //        break;
        //    case 3:
        //        this.tbkView = (TextBlock)target;
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
