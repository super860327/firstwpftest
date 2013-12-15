using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Shapes;
namespace IDKin.IM.Windows.Comps
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class SplitButton : Button//, IComponentConnector
	{
		public static readonly RoutedEvent PolygonClickEvent = EventManager.RegisterRoutedEvent("PolygonClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SplitButton));
		//internal TextBlock tbkLabel;
		//internal TextBlock tbkVSplit;
		//internal Button btnPolygon;
		//internal Path Arrow;
        ////private bool _contentLoaded;
		public event RoutedEventHandler PolygonClick
		{
			add
			{
				base.AddHandler(SplitButton.PolygonClickEvent, value);
			}
			remove
			{
				base.RemoveHandler(SplitButton.PolygonClickEvent, value);
			}
		}
		public SplitButton()
		{
			this.InitializeComponent();
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			Button polygonButton = this.btnPolygon;
			if (polygonButton != null)
			{
				polygonButton.Click += new RoutedEventHandler(this.polygonButton_Click);
			}
		}
		private void polygonButton_Click(object sender, RoutedEventArgs e)
		{
			base.RaiseEvent(new RoutedEventArgs(SplitButton.PolygonClickEvent, this));
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/splitbutton.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.tbkLabel = (TextBlock)target;
        //        break;
        //    case 2:
        //        this.tbkVSplit = (TextBlock)target;
        //        break;
        //    case 3:
        //        this.btnPolygon = (Button)target;
        //        break;
        //    case 4:
        //        this.Arrow = (Path)target;
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
