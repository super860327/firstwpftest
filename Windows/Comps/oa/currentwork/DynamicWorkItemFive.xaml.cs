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
	public partial class DynamicWorkItemFive : TableRow//, IComponentConnector
	{
		private string url = string.Empty;
		//internal TextBlock tbkDate;
		//internal TextBlock tbkfromName;
		//internal TextBlock tbkAfterNameOne;
		//internal TextBlock tbkAfterNameTwo;
		//internal TextBlock tbkAfterNameThree;
        ////private bool _contentLoaded;
		public DynamicWorkItemFive()
		{
			this.InitializeComponent();
		}
		public void UseStyle(DynamicWorkItemFiveStyle style)
		{
			switch (style)
			{
			case DynamicWorkItemFiveStyle.FourHyperLink:
				this.tbkAfterNameTwo.Style = (base.FindResource("TitleStyle") as Style);
				this.tbkAfterNameTwo.TextTrimming = TextTrimming.WordEllipsis;
				this.tbkAfterNameTwo.HorizontalAlignment = HorizontalAlignment.Left;
				this.tbkAfterNameTwo.MouseLeftButtonDown += delegate
				{
					BrowserUtil.OpenHyperlinkHandler(this.url);
				};
				break;
			case DynamicWorkItemFiveStyle.LastHyperLinkSpectial:
				this.tbkAfterNameTwo.TextTrimming = TextTrimming.WordEllipsis;
				this.tbkAfterNameTwo.HorizontalAlignment = HorizontalAlignment.Left;
				this.tbkAfterNameTwo.Foreground = new SolidColorBrush(Color.FromRgb(0, 109, 131));
				this.tbkAfterNameThree.Style = (base.FindResource("TitleStyle") as Style);
				this.tbkAfterNameThree.TextTrimming = TextTrimming.WordEllipsis;
				this.tbkAfterNameThree.HorizontalAlignment = HorizontalAlignment.Left;
				this.tbkAfterNameThree.MouseLeftButtonDown += delegate
				{
					BrowserUtil.OpenHyperlinkHandler(this.url);
				};
				break;
			case DynamicWorkItemFiveStyle.LastHyperLink:
				this.tbkAfterNameThree.Style = (base.FindResource("TitleStyle") as Style);
				this.tbkAfterNameThree.TextTrimming = TextTrimming.WordEllipsis;
				this.tbkAfterNameThree.HorizontalAlignment = HorizontalAlignment.Left;
				this.tbkAfterNameThree.MouseLeftButtonDown += delegate
				{
					BrowserUtil.OpenHyperlinkHandler(this.url);
				};
				break;
			}
		}
		public DynamicWorkItemFive(string date, string fromName, string afterNameOne, string afterNameTwo, string afterNameThree) : this(date, fromName, afterNameOne, afterNameTwo, afterNameThree, null)
		{
		}
		public DynamicWorkItemFive(string date, string fromName, string afterNameOne, string afterNameTwo, string afterNameThree, string url)
		{
			this.InitializeComponent();
			this.tbkDate.Text = date;
			this.tbkfromName.Text = fromName;
			this.tbkAfterNameOne.Text = afterNameOne;
			this.tbkAfterNameTwo.Text = afterNameTwo;
			this.tbkAfterNameThree.Text = afterNameThree;
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/oa/currentwork/dynamicworkitemfive.xaml", UriKind.Relative);
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
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
