using System;
using System.Windows.Forms;
namespace IDKin.IM.Windows.Util
{
	public class HookEventArgs : System.EventArgs
	{
		public Keys Key;
		public bool Alt;
		public bool Control;
		public bool Shift;
		public HookEventArgs(uint keyCode)
		{
			this.Key = (Keys)keyCode;
			this.Alt = ((System.Windows.Forms.Control.ModifierKeys & Keys.Alt) != Keys.None);
			this.Control = ((System.Windows.Forms.Control.ModifierKeys & Keys.Control) != Keys.None);
			this.Shift = ((System.Windows.Forms.Control.ModifierKeys & Keys.Shift) != Keys.None);
		}
	}
}
