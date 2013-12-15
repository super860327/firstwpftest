using IDKin.IM.Core;
using IDKin.IM.ImageService;
using IDKin.IM.Windows.Util;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
namespace IDKin.IM.Windows.Comps
{
	public partial class StatusButton : Button, System.IDisposable
	{
		private IImageService imageService;
		private void Button_Initialized(object sender, System.EventArgs e)
		{
			base.ContextMenu = null;
		}
		public StatusButton()
		{
			this.InitializeComponent();
			this.InitUI();
		}
		private void InitUI()
		{
			this.imageService = ServiceUtil.Instance.ImageService;
			if (this.imageService != null)
			{
				this.miOnline.Icon = new Image
				{
					Source = this.imageService.GetStatusIcon(UserStatus.Online)
				};
				this.miAway.Icon = new Image
				{
					Source = this.imageService.GetStatusIcon(UserStatus.Away)
				};
				this.miDoNotDisturb.Icon = new Image
				{
					Source = this.imageService.GetStatusIcon(UserStatus.DoNotDisturb)
				};
				this.miBusy.Icon = new Image
				{
					Source = this.imageService.GetStatusIcon(UserStatus.Busy)
				};
				this.miOut.Icon = new Image
				{
					Source = this.imageService.GetStatusIcon(UserStatus.Out)
				};
				this.miMeeting.Icon = new Image
				{
					Source = this.imageService.GetStatusIcon(UserStatus.Meeting)
				};
				this.miHide.Icon = new Image
				{
					Source = this.imageService.GetStatusIcon(UserStatus.Hide)
				};
				this.miOffline.Icon = new Image
				{
					Source = this.imageService.GetStatusIcon(UserStatus.Offline)
				};
			}
		}
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			this.statusMenu.HasDropShadow = true;
			this.statusMenu.PlacementTarget = this;
			this.statusMenu.Placement = PlacementMode.Bottom;
			this.statusMenu.IsOpen = true;
		}
		public void Dispose()
		{
			this.imageService.RemoveStatusIcon(UserStatus.Online);
			this.imageService.RemoveStatusIcon(UserStatus.Away);
			this.imageService.RemoveStatusIcon(UserStatus.DoNotDisturb);
			this.imageService.RemoveStatusIcon(UserStatus.Busy);
			this.imageService.RemoveStatusIcon(UserStatus.Out);
			this.imageService.RemoveStatusIcon(UserStatus.Meeting);
			this.imageService.RemoveStatusIcon(UserStatus.Hide);
			this.imageService.RemoveStatusIcon(UserStatus.Offline);
		}
	}
}
