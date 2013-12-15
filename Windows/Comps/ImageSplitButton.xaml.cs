using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
namespace IDKin.IM.Windows.Comps
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class ImageSplitButton : Button//, IComponentConnector
	{
		public static readonly RoutedEvent ArrowClickEvent = EventManager.RegisterRoutedEvent("ArrowClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SplitButton));
		private ImageSource icon;
		//internal Image imgIcon;
		//internal Button btnArrow;
		//internal Path Arrow;
        ////private bool _contentLoaded;
		public event RoutedEventHandler ArrowClick
		{
			add
			{
				base.AddHandler(ImageSplitButton.ArrowClickEvent, value);
			}
			remove
			{
				base.RemoveHandler(ImageSplitButton.ArrowClickEvent, value);
			}
		}
		public ImageSource Icon
		{
			get
			{
				return this.icon;
			}
			set
			{
				this.icon = value;
				this.imgIcon.Source = this.icon;
			}
		}
		public ImageSplitButton()
		{
			this.InitializeComponent();
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			Button arrowButton = this.btnArrow;
			if (arrowButton != null)
			{
				arrowButton.Click += new RoutedEventHandler(this.ArrowButton_Click);
			}
		}
		private void ArrowButton_Click(object sender, RoutedEventArgs e)
		{
			base.RaiseEvent(new RoutedEventArgs(ImageSplitButton.ArrowClickEvent, this));
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/imagesplitbutton.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.imgIcon = (Image)target;
        //        break;
        //    case 2:
        //        this.btnArrow = (Button)target;
        //        break;
        //    case 3:
        //        this.Arrow = (Path)target;
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
