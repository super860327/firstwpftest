using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
namespace IDKin.IM.Windows.Comps.OA.CurrentWork
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public partial class DynamicWorkItemTwelve : TableRow//, IComponentConnector
	{
		//internal TextBlock tbkDate;
		//internal TextBlock tbkfromName;
		//internal TextBlock tbkAfterNameOne;
		//internal TextBlock tbkAfterNameTwo;
		//internal TextBlock tbkAfterNameThree;
		//internal TextBlock tbkAfterNameFour;
		//internal TextBlock tbkAfterNameFive;
		//internal TextBlock tbkAfterNameSix;
		//internal TextBlock tbkAfterNameSeven;
		//internal TextBlock tbkAfterNameEight;
		//internal TextBlock tbkAfterNameNine;
		//internal TextBlock tbkAfterNameTen;
        ////private bool _contentLoaded;
		public DynamicWorkItemTwelve()
		{
			this.InitializeComponent();
		}
		public DynamicWorkItemTwelve(string date, string fromName, string afterNameOne, string afterNameTwo, string afterNameThree, string afterNameFour, string afterNameFive, string afterNameSix, string afterNameSeven, string afterNameEight, string afterNameNine, string afterNameTen, string url)
		{
			this.InitializeComponent();
			this.tbkDate.Text = date;
			this.tbkfromName.Text = fromName;
			this.tbkAfterNameOne.Text = afterNameOne;
			this.tbkAfterNameTwo.Text = afterNameTwo;
			this.tbkAfterNameThree.Text = afterNameThree;
			this.tbkAfterNameFour.Text = afterNameFour;
			this.tbkAfterNameFive.Text = afterNameFive;
			this.tbkAfterNameSix.Text = afterNameSix;
			this.tbkAfterNameSeven.Text = afterNameSeven;
			this.tbkAfterNameEight.Text = afterNameEight;
			this.tbkAfterNameNine.Text = afterNameNine;
			this.tbkAfterNameTen.Text = afterNameTen;
		}
		public void UseStyle(DynamicWorkItemTwelveStyle style)
		{
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/oa/currentwork/dynamicworkitemtwelve.xaml", UriKind.Relative);
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
        //        this.tbkAfterNameFour = (TextBlock)target;
        //        break;
        //    case 7:
        //        this.tbkAfterNameFive = (TextBlock)target;
        //        break;
        //    case 8:
        //        this.tbkAfterNameSix = (TextBlock)target;
        //        break;
        //    case 9:
        //        this.tbkAfterNameSeven = (TextBlock)target;
        //        break;
        //    case 10:
        //        this.tbkAfterNameEight = (TextBlock)target;
        //        break;
        //    case 11:
        //        this.tbkAfterNameNine = (TextBlock)target;
        //        break;
        //    case 12:
        //        this.tbkAfterNameTen = (TextBlock)target;
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
