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
	public partial class DynamicWorkItemFour : TableRow//, IComponentConnector
	{
		//internal ColumnDefinition column3;
		//internal TextBlock tbkDate;
		//internal TextBlock tbkfromName;
		//internal TextBlock tbkAfterNameOne;
		//internal TextBlock tbkAfterNameTwo;
        ////private bool _contentLoaded;
		public DynamicWorkItemFour()
		{
			this.InitializeComponent();
		}
		public void UseStyle(DynamicWorkItemFourStyle style)
		{
			switch (style)
			{
			case DynamicWorkItemFourStyle.None:
				this.column3.Width = new GridLength(1.0, GridUnitType.Auto);
				this.tbkAfterNameTwo.HorizontalAlignment = HorizontalAlignment.Left;
				this.tbkAfterNameTwo.Foreground = new SolidColorBrush(Color.FromRgb(0, 109, 131));
				break;
			case DynamicWorkItemFourStyle.LastHyperLink:
				this.column3.Width = new GridLength(1.0, GridUnitType.Star);
				this.tbkAfterNameTwo.Style = (base.FindResource("TitleStyle") as Style);
				this.tbkAfterNameTwo.MouseLeftButtonDown += delegate
				{
					BrowserUtil.OpenHyperlinkHandler(this.tbkAfterNameTwo.Tag.ToString());
				};
				break;
			}
		}
		public DynamicWorkItemFour(string date, string fromName, string afterNameOne, string afterNameTwo) : this(date, fromName, afterNameOne, afterNameTwo, string.Empty)
		{
		}
		public DynamicWorkItemFour(string date, string fromName, string afterNameOne, string afterNameTwo, string url)
		{
			this.InitializeComponent();
			this.tbkDate.Text = date;
			this.tbkfromName.Text = fromName;
			this.tbkAfterNameOne.Text = afterNameOne;
			this.tbkAfterNameTwo.Text = afterNameTwo;
			this.tbkAfterNameTwo.Tag = url;
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/oa/currentwork/dynamicworkitemfour.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.column3 = (ColumnDefinition)target;
        //        break;
        //    case 2:
        //        this.tbkDate = (TextBlock)target;
        //        break;
        //    case 3:
        //        this.tbkfromName = (TextBlock)target;
        //        break;
        //    case 4:
        //        this.tbkAfterNameOne = (TextBlock)target;
        //        break;
        //    case 5:
        //        this.tbkAfterNameTwo = (TextBlock)target;
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
