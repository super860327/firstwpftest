using IDKin.IM.CustomComponents.Controls;
using IDKin.IM.Data;
using IDKin.IM.Log;
using IDKin.IM.Windows.Util;
using IDKin.IM.Windows.View;
using System;
namespace IDKin.IM.Windows.Comps.ChatTab
{
	public class BaseTab : CloseableTabItem
	{
		protected TabItemHeaderControl TabHeader = new TabItemHeaderControl();
		protected IDataService dataService = null;
		protected ILogger logger = null;
		public TabItemHeaderControl TabHead
		{
			get
			{
				return this.TabHeader;
			}
		}
		public BaseTab()
		{
			base.Header = this.TabHeader;
			this.InitService();
		}
		private void InitService()
		{
			this.dataService = ServiceUtil.Instance.DataService;
			this.logger = ServiceUtil.Instance.Logger;
		}
		public void SetDefaultStyle()
		{
			this.TabHeader.SetNormalStyle();
		}
		public void SetFlashingStyle()
		{
			this.TabHeader.SetFlashingStyle();
		}
		protected void SetFocus2DesktopButton()
		{
			INWindow inWindow = this.dataService.INWindow as INWindow;
			if (inWindow != null)
			{
				inWindow.btnApproval.Focus();
			}
		}
	}
}
