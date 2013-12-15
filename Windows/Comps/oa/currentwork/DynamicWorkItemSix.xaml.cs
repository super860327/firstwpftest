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
namespace IDKin.IM.Windows.Comps.OA.CurrentWork
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public partial class DynamicWorkItemSix : TableRow//, IComponentConnector
	{
		private string urlFour = string.Empty;
		private string urlSix = string.Empty;
		//internal TextBlock tbkDate;
		//internal TextBlock tbkfromName;
		//internal TextBlock tbkAfterNameOne;
		//internal TextBlock tbkAfterNameTwo;
		//internal TextBlock tbkAfterNameThree;
		//internal TextBlock tbkAfterToNameFour;
		//private bool _contentLoaded;
		public DynamicWorkItemSix()
		{
			this.InitializeComponent();
		}
		public DynamicWorkItemSix(string date, string fromName, string afterNameOne, string afterNameTwo, string afterNameThree, string afterNameFour) : this(date, fromName, afterNameOne, afterNameTwo, afterNameThree, afterNameFour, null)
		{
		}
		public DynamicWorkItemSix(string date, string fromName, string afterNameOne, string afterNameTwo, string afterNameThree, string afterNameFour, string url)
		{
			this.InitializeComponent();
			this.tbkDate.Text = date;
			this.tbkfromName.Text = fromName;
			this.tbkAfterNameOne.Text = afterNameOne;
			this.tbkAfterNameTwo.Text = afterNameTwo;
			this.tbkAfterNameThree.Text = afterNameThree;
			this.tbkAfterToNameFour.Text = afterNameFour;
			this.tbkAfterNameTwo.Tag = url;
		}
		public void UseStyle(DynamicWorkItemSixStyle style)
		{
			switch (style)
			{
			case DynamicWorkItemSixStyle.FourLastHyperLink:
				this.tbkAfterNameTwo.TextTrimming = TextTrimming.WordEllipsis;
				this.tbkAfterNameTwo.HorizontalAlignment = HorizontalAlignment.Left;
				this.tbkAfterNameTwo.Style = (base.FindResource("TitleStyle") as Style);
				this.tbkAfterNameTwo.MouseLeftButtonDown += delegate
				{
					BrowserUtil.OpenHyperlinkHandler(this.urlFour);
				};
				this.tbkAfterToNameFour.TextTrimming = TextTrimming.WordEllipsis;
				this.tbkAfterToNameFour.HorizontalAlignment = HorizontalAlignment.Left;
				this.tbkAfterToNameFour.Style = (base.FindResource("TitleStyle") as Style);
				this.tbkAfterToNameFour.MouseLeftButtonDown += delegate
				{
					BrowserUtil.OpenHyperlinkHandler(this.urlSix);
				};
				break;
			case DynamicWorkItemSixStyle.LastHyperLink:
				this.tbkAfterNameTwo.Foreground = new SolidColorBrush(Color.FromRgb(0, 109, 131));
				this.tbkAfterToNameFour.TextTrimming = TextTrimming.WordEllipsis;
				this.tbkAfterToNameFour.HorizontalAlignment = HorizontalAlignment.Left;
				this.tbkAfterToNameFour.Style = (base.FindResource("TitleStyle") as Style);
				this.tbkAfterToNameFour.MouseLeftButtonDown += delegate
				{
					BrowserUtil.OpenHyperlinkHandler(this.urlSix);
				};
				break;
			}
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/oa/currentwork/dynamicworkitemsix.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.tbkDate = (TextBlock)target;
        //        break;
        //    case 2:
        //        this.tbkfromName = (TextBlock)target;
        //        break;
        //    case 3:
        //        this.tbkAfterNameOne = (TextBlock)target;
        //        break;
        //    case 4:
        //        this.tbkAfterNameTwo = (TextBlock)target;
        //        break;
        //    case 5:
        //        this.tbkAfterNameThree = (TextBlock)target;
        //        break;
        //    case 6:
        //        this.tbkAfterToNameFour = (TextBlock)target;
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
