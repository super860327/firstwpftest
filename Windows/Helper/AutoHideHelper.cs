using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Threading;
namespace IDKin.IM.Windows.Helper
{
	public class AutoHideHelper
	{
		public struct POINT
		{
			public int x;
			public int y;
			public POINT(int x, int y)
			{
				this.x = x;
				this.y = y;
			}
		}
		public static void SideHideOrShow(Window dockableWindow, ref double dockWindowHeight, DispatcherTimer dockTimer)
		{
			AutoHideHelper.POINT p;
			if (AutoHideHelper.GetCursorPos(out p))
			{
				if (dockableWindow.WindowState != WindowState.Minimized)
				{
					dockTimer.Interval = System.TimeSpan.FromMilliseconds(1000.0);
					if ((double)p.x > dockableWindow.Left - 1.0 && (double)p.x < dockableWindow.Left + dockableWindow.ActualWidth && (double)p.y > dockableWindow.Top - 1.0 && (double)p.y < dockableWindow.Top + dockableWindow.ActualHeight)
					{
						if (dockableWindow.Top <= 0.0 && dockableWindow.Left > 5.0 && dockableWindow.Left < SystemParameters.WorkArea.Width - dockableWindow.Width)
						{
							dockableWindow.Top = 0.0;
							dockableWindow.Topmost = true;
						}
						else
						{
							if (dockableWindow.Left <= 0.0 && dockableWindow.Top <= 0.0)
							{
								dockableWindow.Top = 0.0;
								dockableWindow.Left = 0.0;
								dockableWindow.Topmost = true;
							}
							else
							{
								if (dockableWindow.Left + dockableWindow.Width >= SystemParameters.WorkArea.Width && dockableWindow.Top <= 0.0)
								{
									dockableWindow.Top = 0.0;
									dockableWindow.Left = SystemParameters.WorkArea.Width - dockableWindow.Width;
									dockableWindow.Topmost = true;
								}
								else
								{
									if (dockWindowHeight > 0.0)
									{
										dockWindowHeight = 0.0;
										dockableWindow.Topmost = false;
									}
								}
							}
						}
					}
					else
					{
						if (dockWindowHeight < 1.0)
						{
							dockWindowHeight = dockableWindow.Height;
						}
						if (dockableWindow.Top <= 4.0 && dockableWindow.Left > 5.0 && dockableWindow.Left < SystemParameters.WorkArea.Width - dockableWindow.Width)
						{
							dockableWindow.Top = 3.0 - dockableWindow.Height;
							if (dockableWindow.Left > 4.0)
							{
								if (dockableWindow.Left + dockableWindow.Width >= SystemParameters.WorkArea.Width - 4.0)
								{
								}
							}
						}
						else
						{
							if (dockableWindow.Left <= 4.0 && dockableWindow.Top <= 0.0)
							{
								dockableWindow.Top = 3.0 - dockableWindow.Height;
							}
							else
							{
								if (dockableWindow.Left + dockableWindow.Width >= SystemParameters.WorkArea.Width - 4.0 && dockableWindow.Top <= 0.0)
								{
									dockableWindow.Top = 3.0 - dockableWindow.Height;
								}
							}
						}
						dockTimer.Interval = System.TimeSpan.FromMilliseconds(200.0);
						dockableWindow.Topmost = false;
					}
				}
			}
		}
		[System.Runtime.InteropServices.DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool GetCursorPos(out AutoHideHelper.POINT pt);
	}
}
