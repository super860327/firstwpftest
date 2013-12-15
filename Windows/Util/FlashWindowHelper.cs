using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;
namespace IDKin.IM.Windows.Util
{
	public sealed class FlashWindowHelper
	{
		private DispatcherTimer _timer;
		private int _count = 0;
		private int _maxTimes = 0;
		private Window _window;
		public void Flash(int times, double millliseconds, Window window)
		{
			this._timer = new DispatcherTimer();
			this._maxTimes = times;
			this._timer.Interval = System.TimeSpan.FromMilliseconds(millliseconds);
			this._timer.Tick += new System.EventHandler(this.OnTick);
			this._window = window;
			this._timer.Start();
		}
		private void OnTick(object sender, System.EventArgs e)
		{
			if (++this._count < this._maxTimes)
			{
				Win32.FlashWindow(new WindowInteropHelper(this._window).Handle, this._count % 2 == 0);
			}
			else
			{
				this._timer.Stop();
			}
		}
	}
}
