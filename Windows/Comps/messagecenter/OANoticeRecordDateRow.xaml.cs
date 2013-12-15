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
	public partial class OANoticeRecordDateRow : TableRow//, IComponentConnector
	{
		//internal TextBlock tbkDate;
		//private bool _contentLoaded;
		public OANoticeRecordDateRow(string datatime)
		{
			this.InitializeComponent();
			this.tbkDate.Text = "日期：" + datatime;
		}
		public OANoticeRecordDateRow(string datatime, bool bn)
		{
			this.InitializeComponent();
			this.tbkDate.Text = datatime;
			this.tbkDate.FontSize = 14.0;
			this.tbkDate.FontWeight = FontWeights.Normal;
			this.tbkDate.Foreground = Brushes.Gray;
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/messagecenter/oanoticerecorddaterow.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    if (connectionId != 1)
        //    {
        //        this._contentLoaded = true;
        //    }
        //    else
        //    {
        //        this.tbkDate = (TextBlock)target;
        //    }
        //}
	}
}
