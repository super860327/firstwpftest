using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
namespace IDKin.IM.Windows.Comps
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public partial class ImageControl : UserControl//, IComponentConnector
	{
        ////private bool _contentLoaded;
		protected override bool IsEnabledCore
		{
			get
			{
				return true;
			}
		}
		public ImageControl(Image image)
		{
			this.InitializeComponent();
			base.BorderBrush = Brushes.Red;
			this.AddEventListenerHandler();
		}
		private void AddEventListenerHandler()
		{
			base.MouseEnter += new MouseEventHandler(this.ImageControl_MouseEnter);
			base.MouseLeave += new MouseEventHandler(this.ImageControl_MouseLeave);
		}
		private void ImageControl_MouseEnter(object sender, MouseEventArgs e)
		{
			this.ShowBorder();
		}
		private void ImageControl_MouseLeave(object sender, MouseEventArgs e)
		{
			this.HidenBorder();
		}
		private void ShowBorder()
		{
			base.BorderThickness = new Thickness(2.0);
		}
		private void HidenBorder()
		{
			base.BorderThickness = new Thickness(0.0);
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/imagecontrol.xaml", UriKind.Relative);
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
