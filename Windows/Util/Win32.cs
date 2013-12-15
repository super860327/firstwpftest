using System;
using System.Runtime.InteropServices;
namespace IDKin.IM.Windows.Util
{
	internal static class Win32
	{
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		public static extern bool FlashWindow(System.IntPtr hwnd, bool bInvert);
	}
}
