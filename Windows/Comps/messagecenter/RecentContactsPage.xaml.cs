using IDKin.IM.Data;
using IDKin.IM.Log;
using IDKin.IM.Theme.Helper;
using IDKin.IM.Windows.Model.MessageCenter;
using IDKin.IM.Windows.Util;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
namespace IDKin.IM.Windows.Comps.MessageCenter
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public partial class RecentContactsPage : Page//, IComponentConnector
	{
		private RecentContactsItem item = null;
		private ISessionService sessionService = ServiceUtil.Instance.SessionService;
		private ILogger logger = ServiceUtil.Instance.Logger;
		private System.Collections.Generic.IDictionary<long, MessageCenterChatInfo> center = new System.Collections.Generic.Dictionary<long, MessageCenterChatInfo>();
		//internal ListView recentList;
        ////private bool _contentLoaded;
		public RecentContactsPage()
		{
			ThemeSwitcher.LoadSkin(ThemeEnum.Aero, this);
			this.InitializeComponent();
		}
		public void InitStaff()
		{
			try
			{
				this.recentList.Items.Clear();
				this.center.Clear();
				System.Collections.Generic.List<MessageCenterChatInfo> listmessage = LocalDataUtil.Instance.GetAllChatInfoBySort();
				if (listmessage != null && listmessage.Count > 0)
				{
					for (int i = listmessage.Count - 1; i >= 0; i--)
					{
						if (listmessage[i] != null && !this.center.ContainsKey(listmessage[i].Id))
						{
							this.item = new RecentContactsItem(listmessage[i], false, false);
							if (this.item.IsValue)
							{
								this.center.Add(listmessage[i].Id, listmessage[i]);
								if (this.sessionService.Uid != listmessage[i].Id)
								{
									this.recentList.Items.Add(this.item);
								}
							}
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		public void InitGroup()
		{
			try
			{
				this.recentList.Items.Clear();
				this.center.Clear();
				System.Collections.Generic.List<MessageCenterChatInfo> listmessage = LocalDataUtil.Instance.GetAllChatInfoBySort();
				if (listmessage != null && listmessage.Count > 0)
				{
					for (int i = listmessage.Count - 1; i >= 0; i--)
					{
						if (listmessage[i] != null && !this.center.ContainsKey(listmessage[i].Id))
						{
							this.item = new RecentContactsItem(listmessage[i], true, false);
							if (this.item.IsValue)
							{
								this.center.Add(listmessage[i].Id, listmessage[i]);
								if (this.sessionService.Uid != listmessage[i].Id)
								{
									this.recentList.Items.Add(this.item);
								}
							}
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		public void InitData()
		{
			try
			{
				this.recentList.Items.Clear();
				this.center.Clear();
				System.Collections.Generic.List<MessageCenterChatInfo> listmessage = LocalDataUtil.Instance.GetAllChatInfoBySort();
				if (listmessage != null && listmessage.Count > 0)
				{
					for (int i = listmessage.Count - 1; i >= 0; i--)
					{
						if (listmessage[i] != null && !this.center.ContainsKey(listmessage[i].Id))
						{
							this.item = new RecentContactsItem(listmessage[i], false, true);
							if (this.item.IsValue)
							{
								this.center.Add(listmessage[i].Id, listmessage[i]);
								if (this.sessionService.Uid != listmessage[i].Id)
								{
									this.recentList.Items.Add(this.item);
								}
							}
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/messagecenter/recentcontactspage.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    if (connectionId != 1)
        //    {
        //        this._contentLoaded = true;
        //    }
        //    else
        //    {
        //        this.recentList = (ListView)target;
        //    }
        //}
	
    }
}
