using System;
using System.Runtime.InteropServices;
using System.Windows;
namespace IDKin.IM.Windows.Helper
{
	public class Win32Helper
	{
		internal struct MINMAXINFO
		{
			public Win32Helper.POINT ptReserved;
			public Win32Helper.POINT ptMaxSize;
			public Win32Helper.POINT ptMaxPosition;
			public Win32Helper.POINT ptMinTrackSize;
			public Win32Helper.POINT ptMaxTrackSize;
		}
		[System.Runtime.InteropServices.StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		internal class MONITORINFO
		{
			public int cbSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(Win32Helper.MONITORINFO));
			public Win32Helper.RECT rcMonitor;
			public Win32Helper.RECT rcWork;
			public int dwFlags;
		}
		internal struct RECT
		{
			public int left;
			public int top;
			public int right;
			public int bottom;
			public static readonly Win32Helper.RECT Empty;
			public int Width
			{
				get
				{
					return System.Math.Abs(this.right - this.left);
				}
			}
			public int Height
			{
				get
				{
					return this.bottom - this.top;
				}
			}
			public bool IsEmpty
			{
				get
				{
					return this.left >= this.right || this.top >= this.bottom;
				}
			}
			public RECT(int left, int top, int right, int bottom)
			{
				this.left = left;
				this.top = top;
				this.right = right;
				this.bottom = bottom;
			}
			public RECT(Win32Helper.RECT rcSrc)
			{
				this.left = rcSrc.left;
				this.top = rcSrc.top;
				this.right = rcSrc.right;
				this.bottom = rcSrc.bottom;
			}
			public override string ToString()
			{
				string result;
				if (this == Win32Helper.RECT.Empty)
				{
					result = "RECT {Empty}";
				}
				else
				{
					result = string.Concat(new object[]
					{
						"RECT { left : ",
						this.left,
						" / top : ",
						this.top,
						" / right : ",
						this.right,
						" / bottom : ",
						this.bottom,
						" }"
					});
				}
				return result;
			}
			public override bool Equals(object obj)
			{
				return obj is Rect && this == (Win32Helper.RECT)obj;
			}
			public override int GetHashCode()
			{
				return this.left.GetHashCode() + this.top.GetHashCode() + this.right.GetHashCode() + this.bottom.GetHashCode();
			}
			public static bool operator ==(Win32Helper.RECT rect1, Win32Helper.RECT rect2)
			{
				return rect1.left == rect2.left && rect1.top == rect2.top && rect1.right == rect2.right && rect1.bottom == rect2.bottom;
			}
			public static bool operator !=(Win32Helper.RECT rect1, Win32Helper.RECT rect2)
			{
				return !(rect1 == rect2);
			}
		}
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
		public static int GWL_EXSTYLE = -20;
		public static int WS_EX_LAYERED = 524288;
		public static int WS_EX_TRANSPARENT = 32;
		public static void WmGetMinMaxInfo(System.IntPtr hwnd, System.IntPtr lParam)
		{
			Win32Helper.MINMAXINFO mmi = (Win32Helper.MINMAXINFO)System.Runtime.InteropServices.Marshal.PtrToStructure(lParam, typeof(Win32Helper.MINMAXINFO));
			int MONITOR_DEFAULTTONEAREST = 2;
			System.IntPtr monitor = Win32Helper.MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);
			if (monitor != System.IntPtr.Zero)
			{
				Win32Helper.MONITORINFO monitorInfo = new Win32Helper.MONITORINFO();
				Win32Helper.GetMonitorInfo(monitor, monitorInfo);
				Win32Helper.RECT rcWorkArea = monitorInfo.rcWork;
				Win32Helper.RECT rcMonitorArea = monitorInfo.rcMonitor;
				mmi.ptMaxPosition.x = System.Math.Abs(rcWorkArea.left - rcMonitorArea.left);
				mmi.ptMaxPosition.y = System.Math.Abs(rcWorkArea.top - rcMonitorArea.top);
				mmi.ptMaxSize.x = System.Math.Abs(rcWorkArea.right - rcWorkArea.left);
				mmi.ptMaxSize.y = System.Math.Abs(rcWorkArea.bottom - rcWorkArea.top);
			}
			System.Runtime.InteropServices.Marshal.StructureToPtr(mmi, lParam, true);
		}
		[System.Runtime.InteropServices.DllImport("user32")]
		internal static extern bool GetMonitorInfo(System.IntPtr hMonitor, Win32Helper.MONITORINFO lpmi);
		[System.Runtime.InteropServices.DllImport("User32")]
		internal static extern System.IntPtr MonitorFromWindow(System.IntPtr handle, int flags);
		[System.Runtime.InteropServices.DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool GetCursorPos(out Win32Helper.POINT pt);
		[System.Runtime.InteropServices.DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int GetWindowLong(System.IntPtr hWnd, int nIndex);
		[System.Runtime.InteropServices.DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int SetWindowLong(System.IntPtr hWnd, int nIndex, int newVal);
		[System.Runtime.InteropServices.DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern System.IntPtr SendMessage(System.IntPtr hWnd, uint Msg, System.IntPtr wParam, System.IntPtr lParam);
		public static System.IntPtr WindowProc(System.IntPtr hwnd, int msg, System.IntPtr wParam, System.IntPtr lParam, ref bool handled)
		{
			if (msg == 36)
			{
				Win32Helper.WmGetMinMaxInfo(hwnd, lParam);
				handled = true;
			}
			return (System.IntPtr)0;
		}
	}
}
