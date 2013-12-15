using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
namespace IDKin.IM.Windows.Comps
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class CustomProgressBar : ProgressBar//, IComponentConnector
	{
		//private bool _contentLoaded;
		public CustomProgressBar()
		{
			this.InitializeComponent();
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			TextBlock tbkIndicator = base.Template.FindName("PART_IndicatorText", this) as TextBlock;
			tbkIndicator.Text = base.Value.ToString() + "%";
			tbkIndicator.Margin = new Thickness(base.Value + 60.0, 0.0, 0.0, 0.0);
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/collaboration/customprogressbar.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    this._contentLoaded = true;
        //}
	}
}
