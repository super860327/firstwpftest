using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
namespace IDKin.IM.Windows.Comps
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class LinkButton : Button
	{
		private BitmapImage icon;
		private string label = "";
		//internal StackPanel spnlContent;
		//internal Image imgContent;
		//internal TextBlock tbkContent;
        ////private bool _contentLoaded;
		public BitmapImage Icon
		{
			get
			{
				return this.icon;
			}
			set
			{
				this.icon = value;
				this.imgContent.Source = this.icon;
			}
		}
		public string Label
		{
			get
			{
				return this.label;
			}
			set
			{
				this.label = value;
				this.tbkContent.Text = this.label;
			}
		}
		public LinkButton()
		{
			this.InitializeComponent();
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/linkbutton.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.spnlContent = (StackPanel)target;
        //        break;
        //    case 2:
        //        this.imgContent = (Image)target;
        //        break;
        //    case 3:
        //        this.tbkContent = (TextBlock)target;
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
