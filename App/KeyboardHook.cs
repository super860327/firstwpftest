using IDKin.IM.Windows.Util;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
namespace IN.Helper
{
	public class KeyboardHook
	{
		private enum HookType
		{
			WH_JOURNALRECORD,
			WH_JOURNALPLAYBACK,
			WH_KEYBOARD,
			WH_GETMESSAGE,
			WH_CALLWNDPROC,
			WH_CBT,
			WH_SYSMSGFILTER,
			WH_MOUSE,
			WH_HARDWARE,
			WH_DEBUG,
			WH_SHELL,
			WH_FOREGROUNDIDLE,
			WH_CALLWNDPROCRET,
			WH_KEYBOARD_LL,
			WH_MOUSE_LL
		}
		public struct KBDLLHOOKSTRUCT
		{
			public uint vkCode;
			public uint scanCode;
			public uint flags;
			public uint time;
			public IntPtr extraInfo;
		}
		private delegate int HookProc(int code, IntPtr wParam, ref KeyboardHook.KBDLLHOOKSTRUCT lParam);
		public delegate void HookEventHandler(object sender, HookEventArgs e);
		private KeyboardHook.HookType _hookType = KeyboardHook.HookType.WH_KEYBOARD_LL;
		private IntPtr _hookHandle = IntPtr.Zero;
		private KeyboardHook.HookProc _hookFunction;
		public event KeyboardHook.HookEventHandler KeyDown;
		public event KeyboardHook.HookEventHandler KeyUp;
		[DllImport("user32.dll")]
		private static extern IntPtr SetWindowsHookEx(KeyboardHook.HookType code, KeyboardHook.HookProc func, IntPtr instance, int threadID);
		[DllImport("user32.dll")]
		private static extern int UnhookWindowsHookEx(IntPtr hook);
		[DllImport("user32.dll")]
		private static extern int CallNextHookEx(IntPtr hook, int code, IntPtr wParam, ref KeyboardHook.KBDLLHOOKSTRUCT lParam);
		public KeyboardHook()
		{
			this._hookFunction = new KeyboardHook.HookProc(this.HookCallback);
			this.Install();
		}
		~KeyboardHook()
		{
			this.Uninstall();
		}
		private int HookCallback(int code, IntPtr wParam, ref KeyboardHook.KBDLLHOOKSTRUCT lParam)
		{
			if (code < 0)
			{
				return KeyboardHook.CallNextHookEx(this._hookHandle, code, wParam, ref lParam);
			}
			if ((lParam.flags & 128u) != 0u && this.KeyUp != null)
			{
				this.KeyUp(this, new HookEventArgs(lParam.vkCode));
			}
			if ((lParam.flags & 128u) == 0u && this.KeyDown != null)
			{
				this.KeyDown(this, new HookEventArgs(lParam.vkCode));
			}
			return KeyboardHook.CallNextHookEx(this._hookHandle, code, wParam, ref lParam);
		}
		private void Install()
		{
			if (this._hookHandle != IntPtr.Zero)
			{
				return;
			}
			Module[] list = Assembly.GetExecutingAssembly().GetModules();
			Debug.Assert(list != null && list.Length > 0);
			this._hookHandle = KeyboardHook.SetWindowsHookEx(this._hookType, this._hookFunction, Marshal.GetHINSTANCE(list[0]), 0);
		}
		private void Uninstall()
		{
			if (this._hookHandle != IntPtr.Zero)
			{
				KeyboardHook.UnhookWindowsHookEx(this._hookHandle);
				this._hookHandle = IntPtr.Zero;
			}
		}
	}
}
