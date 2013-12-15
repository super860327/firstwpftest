using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
namespace IDKin.IM.Windows.Util
{
	public static class ControlBox
	{
		private static class NativeMethods
		{
			[System.Runtime.InteropServices.DllImport("user32.dll")]
			internal static extern int GetWindowLong(System.IntPtr hWnd, int index);
			[System.Runtime.InteropServices.DllImport("user32.dll")]
			internal static extern int SetWindowLong(System.IntPtr hWnd, int index, int newLong);
		}
		private const int Style = -16;
		private const int ExtStyle = -20;
		private const int MaximizeBox = 65536;
		private const int MinimizeBox = 131072;
		private const int ContextHelp = 1024;
		public static readonly DependencyProperty HasHelpButtonProperty = DependencyProperty.RegisterAttached("HasHelpButton", typeof(bool), typeof(ControlBox), new UIPropertyMetadata(false, new PropertyChangedCallback(ControlBox.OnControlBoxChanged)));
		public static readonly DependencyProperty HasMaximizeButtonProperty = DependencyProperty.RegisterAttached("HasMaximizeButton", typeof(bool), typeof(ControlBox), new UIPropertyMetadata(true, new PropertyChangedCallback(ControlBox.OnControlBoxChanged)));
		public static readonly DependencyProperty HasMinimizeButtonProperty = DependencyProperty.RegisterAttached("HasMinimizeButton", typeof(bool), typeof(ControlBox), new UIPropertyMetadata(true, new PropertyChangedCallback(ControlBox.OnControlBoxChanged)));
		[AttachedPropertyBrowsableForType(typeof(Window))]
		public static bool GetHasHelpButton(Window element)
		{
			return (bool)element.GetValue(ControlBox.HasHelpButtonProperty);
		}
		[AttachedPropertyBrowsableForType(typeof(Window))]
		public static void SetHasHelpButton(Window element, bool value)
		{
			element.SetValue(ControlBox.HasHelpButtonProperty, value);
		}
		[AttachedPropertyBrowsableForType(typeof(Window))]
		public static bool GetHasMaximizeButton(Window element)
		{
			return (bool)element.GetValue(ControlBox.HasMaximizeButtonProperty);
		}
		[AttachedPropertyBrowsableForType(typeof(Window))]
		public static void SetHasMaximizeButton(Window element, bool value)
		{
			element.SetValue(ControlBox.HasMaximizeButtonProperty, value);
		}
		[AttachedPropertyBrowsableForType(typeof(Window))]
		public static bool GetHasMinimizeButton(Window element)
		{
			return (bool)element.GetValue(ControlBox.HasMinimizeButtonProperty);
		}
		[AttachedPropertyBrowsableForType(typeof(Window))]
		public static void SetHasMinimizeButton(Window element, bool value)
		{
			element.SetValue(ControlBox.HasMinimizeButtonProperty, value);
		}
		private static void OnControlBoxChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Window window = d as Window;
			if (window != null)
			{
				System.IntPtr hWnd = new WindowInteropHelper(window).Handle;
				if (hWnd == System.IntPtr.Zero)
				{
					window.SourceInitialized += new System.EventHandler(ControlBox.OnWindowSourceInitialized);
				}
				else
				{
					ControlBox.UpdateStyle(window, hWnd);
					ControlBox.UpdateExtendedStyle(window, hWnd);
				}
			}
		}
		private static void OnWindowSourceInitialized(object sender, System.EventArgs e)
		{
			Window window = (Window)sender;
			System.IntPtr hWnd = new WindowInteropHelper(window).Handle;
			ControlBox.UpdateStyle(window, hWnd);
			ControlBox.UpdateExtendedStyle(window, hWnd);
			window.SourceInitialized -= new System.EventHandler(ControlBox.OnWindowSourceInitialized);
		}
		private static void UpdateStyle(Window window, System.IntPtr hWnd)
		{
			int style = ControlBox.NativeMethods.GetWindowLong(hWnd, -16);
			if (ControlBox.GetHasMaximizeButton(window))
			{
				style |= 65536;
			}
			else
			{
				style &= -65537;
			}
			if (ControlBox.GetHasMinimizeButton(window))
			{
				style |= 131072;
			}
			else
			{
				style &= -131073;
			}
			ControlBox.NativeMethods.SetWindowLong(hWnd, -16, style);
		}
		private static void UpdateExtendedStyle(Window window, System.IntPtr hWnd)
		{
			int style = ControlBox.NativeMethods.GetWindowLong(hWnd, -20);
			if (ControlBox.GetHasHelpButton(window))
			{
				style |= 1024;
			}
			else
			{
				style &= 1025;
			}
			ControlBox.NativeMethods.SetWindowLong(hWnd, -20, style);
		}
	}
}
