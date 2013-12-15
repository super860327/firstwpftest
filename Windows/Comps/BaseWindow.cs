using IDKin.IM.Windows.Helper;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
namespace IDKin.IM.Windows.Comps
{
	public class BaseWindow : Window
	{
		private HwndSource hwndSource;
		public BaseWindow()
		{
			base.SourceInitialized += delegate(object sender, System.EventArgs e)
			{
				this.hwndSource = (PresentationSource.FromVisual((Visual)sender) as HwndSource);
				this.hwndSource.AddHook(new HwndSourceHook(Win32Helper.WindowProc));
			};
		}
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			try
			{
				base.OnMouseLeftButtonDown(e);
				base.DragMove();
			}
			catch (System.Exception)
			{
			}
		}
	}
}
