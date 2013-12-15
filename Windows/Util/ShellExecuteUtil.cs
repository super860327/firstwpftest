using System;
using System.Runtime.InteropServices;
namespace IDKin.IM.Windows.Util
{
	public class ShellExecuteUtil
	{
		public enum ShowCommands
		{
			SW_HIDE,
			SW_SHOWNORMAL,
			SW_NORMAL = 1,
			SW_SHOWMINIMIZED,
			SW_SHOWMAXIMIZED,
			SW_MAXIMIZE = 3,
			SW_SHOWNOACTIVATE,
			SW_SHOW,
			SW_MINIMIZE,
			SW_SHOWMINNOACTIVE,
			SW_SHOWNA,
			SW_RESTORE,
			SW_SHOWDEFAULT,
			SW_FORCEMINIMIZE,
			SW_MAX = 11
		}
		[System.Runtime.InteropServices.DllImport("shell32.dll")]
		public static extern System.IntPtr ShellExecute(System.IntPtr hwnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, ShellExecuteUtil.ShowCommands nShowCmd);
	}
}
