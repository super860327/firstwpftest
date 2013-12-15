using IDKin.IM.Data;
using IDKin.IM.Windows.Util;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
namespace IDKin.IM.Windows.Comps.MessageCenter
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public partial class OANoticeRecordItem : TableRow//, IComponentConnector
	{
		private ISessionService sessionService = ServiceUtil.Instance.SessionService;
		private string link;
		public string url;
		//internal TextBlock tbkTime;
		//internal TextBlock tbkMsg;
		//internal Hyperlink hlMessageTitle;
		//internal TextBlock txtTitle;
        ////private bool _contentLoaded;
		public OANoticeRecordItem(string createTime, string message, string title, string url)
		{
			this.InitializeComponent();
			this.tbkTime.Text = createTime.Substring(10, 9);
			this.tbkMsg.Text = message;
			this.txtTitle.Text = "\"" + title + "\"";
			this.link = url;
			this.InitEvent();
		}
		public OANoticeRecordItem(string message)
		{
			this.InitializeComponent();
			this.tbkMsg.Text = message.Trim();
			base.FontSize = 14.0;
			base.Foreground = Brushes.Gray;
		}
		private void InitEvent()
		{
			this.hlMessageTitle.Click += new RoutedEventHandler(this.hlMessageTitle_Click);
		}
		private void hlMessageTitle_Click(object sender, System.EventArgs e)
		{
			try
			{
				BrowserUtil.OpenNoticBrowserHandler(this.link);
			}
			catch (System.Exception)
			{
			}
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/messagecenter/oanoticerecorditem.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.tbkTime = (TextBlock)target;
        //        break;
        //    case 2:
        //        this.tbkMsg = (TextBlock)target;
        //        break;
        //    case 3:
        //        this.hlMessageTitle = (Hyperlink)target;
        //        break;
        //    case 4:
        //        this.txtTitle = (TextBlock)target;
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
