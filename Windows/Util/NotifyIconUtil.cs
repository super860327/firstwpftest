using IDKin.IM.Core;
using IDKin.IM.Data;
using IDKin.IM.Windows.Model;
using IDKin.IM.Windows.Properties;
using IDKin.IM.Windows.View;
using System;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;
namespace IDKin.IM.Windows.Util
{
	public class NotifyIconUtil
	{
		private IDataService dataService = null;
		private NotifyIcon notifyIcon = null;
		private static NotifyIconUtil instance = null;
		private Icon NotifyIcon = Resources.notifyIcon;
		private Icon NotifyIconEmpty = Resources.notifyIcon_empty;
		private System.Timers.Timer flashingTimer = null;
		private bool IsFlashing = false;
		private bool flashingFlag = false;
		public static NotifyIconUtil Instance
		{
			get
			{
				if (NotifyIconUtil.instance == null)
				{
					NotifyIconUtil.instance = new NotifyIconUtil();
				}
				return NotifyIconUtil.instance;
			}
		}
		private NotifyIconUtil()
		{
			this.dataService = ServiceUtil.Instance.DataService;
			if (this.dataService != null)
			{
				SystemWindow window = this.dataService.SystemWindow as SystemWindow;
				if (window != null)
				{
					this.notifyIcon = window.NotifyIcon;
				}
			}
		}
		public void SetFlashIcon(FlashIconType type)
		{
			switch (type)
			{
			case FlashIconType.EntGroup:
				this.NotifyIcon = Resources.groupHeaderIcon;
				break;
			case FlashIconType.EntStaff:
				this.NotifyIcon = Resources.notifyIcon;
				break;
			case FlashIconType.MessageCenter:
				this.NotifyIcon = Resources.speaker;
				break;
			case FlashIconType.Roster:
				this.NotifyIcon = Resources.notifyIcon;
				break;
			case FlashIconType.Default:
				this.NotifyIcon = Resources.notifyIcon;
				break;
			}
		}
		public void StartFlashing()
		{
			if (!this.IsFlashing)
			{
				this.IsFlashing = true;
				this.flashingTimer = new System.Timers.Timer(500.0);
				this.flashingTimer.Elapsed += new ElapsedEventHandler(this.FlashingHandler);
				this.flashingTimer.AutoReset = true;
				this.flashingTimer.Enabled = true;
			}
		}
		private void FlashingHandler(object sender, ElapsedEventArgs e)
		{
			try
			{
				if (!DataModel.Instance.HasMessage())
				{
					this.flashingTimer.Stop();
					this.IsFlashing = false;
					this.SetFlashIcon(FlashIconType.Default);
					this.notifyIcon.Icon = this.NotifyIcon;
					this.flashingTimer.Dispose();
					this.flashingTimer.Close();
					this.flashingTimer = null;
				}
				else
				{
					if (this.flashingFlag)
					{
						this.notifyIcon.Icon = this.NotifyIconEmpty;
						this.flashingFlag = false;
					}
					else
					{
						this.notifyIcon.Icon = this.NotifyIcon;
						this.flashingFlag = true;
					}
				}
			}
			catch (System.Exception ex)
			{
				System.Console.WriteLine("NotifyIcon FlashingHandler Exception {0}", ex.ToString());
			}
		}
		public void StopFlashing()
		{
		}
	}
}
