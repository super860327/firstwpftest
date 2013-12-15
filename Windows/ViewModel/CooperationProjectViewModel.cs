using IDKin.IM.Communicate;
using IDKin.IM.Core;
using IDKin.IM.Core.Core;
using IDKin.IM.Data;
using IDKin.IM.Protocol.Commons;
using IDKin.IM.Protocol.Cooperation;
using IDKin.IM.Windows.Comps.ChatTab;
using IDKin.IM.Windows.Model;
using IDKin.IM.Windows.Util;
using IDKin.IM.Windows.View;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
namespace IDKin.IM.Windows.ViewModel
{
	public class CooperationProjectViewModel
	{
		private IConnection connection;
		private DataModel dataModel = null;
		private ISessionService sessionService = null;
		private IDataService dataService = null;
		private static readonly object obj = new object();
		private static CooperationProjectViewModel Instance;
		private System.Collections.Generic.List<string> projectids = new System.Collections.Generic.List<string>();
		public INWindow INWindow
		{
			get
			{
				return ServiceUtil.Instance.DataService.INWindow as INWindow;
			}
		}
		private CooperationProjectViewModel()
		{
			this.connection = ServiceUtil.Instance.Connection;
			this.dataModel = DataModel.Instance;
			this.sessionService = ServiceUtil.Instance.SessionService;
			this.dataService = ServiceUtil.Instance.DataService;
			this.connection.EventHandler.CooperationProjectDataGettedEvent = new CooperationProjectDataGettedHandler(this.OnCooperationProjectDataGetted);
			this.connection.EventHandler.CooperationDockingDataGettedEvent = new CooperationDockingDataGettedHandler(this.OnCooperationDockingDataGetted);
			this.connection.EventHandler.CooperationDockingStutasDataGettedEvent = new CooperationDockingStutasDataGettedHandler(this.OnCooperationDockingStutasDataGetted);
			this.connection.EventHandler.CooperationNoticeResponseEvent = new CooperationNoticeResponseHandler(this.CooperationNoticeResponseEvent);
		}
		public static CooperationProjectViewModel GetInstance()
		{
			if (CooperationProjectViewModel.Instance == null)
			{
				lock (CooperationProjectViewModel.obj)
				{
					if (CooperationProjectViewModel.Instance == null)
					{
						CooperationProjectViewModel.Instance = new CooperationProjectViewModel();
					}
				}
			}
			return CooperationProjectViewModel.Instance;
		}
		public void SendCooperationProjectRequest()
		{
			if (this.sessionService != null && this.connection != null)
			{
				CooperationProjectRequest req = new CooperationProjectRequest();
				req.uid = this.sessionService.Uid;
				this.connection.Send(PacketType.COOPERATION_PROJECT, req);
			}
		}
		public void SendCooperationDockingRequest(CooperationProjectWrapper cooperationProjectWrapper)
		{
			if (this.sessionService != null && this.connection != null && cooperationProjectWrapper != null)
			{
				CooperationDockingRequest req = new CooperationDockingRequest();
				req.Uid = this.sessionService.Uid;
				req.ProjectID = cooperationProjectWrapper.UnitedProjecid;
				req.requestProjectID = cooperationProjectWrapper.CooperationProject.ProjectID;
				req.jid = this.sessionService.Jid;
				this.connection.Send(PacketType.COOPERATION_DOCKING, req);
			}
		}
		private void CloseCooperationTab()
		{
			System.Collections.Generic.ICollection<TabItem> staffChatTabs = this.dataService.GetCooperationStaffChatTabList();
			System.Collections.Generic.List<TabItem> temTabItemList = new System.Collections.Generic.List<TabItem>(staffChatTabs);
			for (int i = temTabItemList.Count - 1; i >= 0; i--)
			{
				CoopStaffTab temCoopStaffTab = temTabItemList[i] as CoopStaffTab;
				if (temCoopStaffTab != null)
				{
					CooperationStaff temCooperationStaff = this.dataService.GetCooperationStaff(temCoopStaffTab.Staff.Uid, temCoopStaffTab.Staff.UnitedProjectid);
					if (temCooperationStaff == null)
					{
						this.dataService.RemoveCooperationStaffChatTab(temCoopStaffTab.Staff.Uid, temCoopStaffTab.Staff.UnitedProjectid);
						if (this.INWindow.ContentTab.Items.Contains(temCoopStaffTab))
						{
							this.INWindow.ContentTab.Items.Remove(temCoopStaffTab);
						}
					}
					else
					{
						temCoopStaffTab.Staff = temCooperationStaff;
						CooperationProjectWrapper temCooperationProjectWrapper = this.dataService.GetCooperationProjectWrapper(temCooperationStaff.UnitedProjectid);
						if (temCooperationProjectWrapper != null)
						{
							temCoopStaffTab.CooperationProjectWrapper = temCooperationProjectWrapper;
						}
					}
				}
			}
		}
		public void OnCooperationProjectDataGetted(CooperationProjectResponse res)
		{
			try
			{
				this.dataService.ClearCooperation();
				this.INWindow.Collaboration.Clear();
				if (res == null || res.cooperationProject == null || res.cooperationProject.Count == 0)
				{
					this.SendCooperationOfflineDataRequest();
					this.CloseCooperationTab();
				}
				else
				{
					this.INWindow.Collaboration.Add(res);
					this.projectids.Clear();
					foreach (CooperationProjectWrapper item in this.dataService.GetCooperationProjectList())
					{
						this.projectids.Add(item.UnitedProjecid);
					}
					if (res.cooperationProject.Count > 0)
					{
						ServiceUtil.Instance.Logger.Debug(res.cooperationProject[res.cooperationProject.Count - 1].from_ProjectID + "." + res.cooperationProject[res.cooperationProject.Count - 1].to_ProjectID);
					}
					foreach (CooperationProjectWrapper item in this.dataService.GetCooperationProjectList())
					{
						this.SendCooperationDockingRequest(item);
					}
				}
			}
			catch (System.Exception ex)
			{
				ServiceUtil.Instance.Logger.Error(ex);
			}
		}
		public void OnCooperationDockingDataGetted(CooperationDockingResponse res)
		{
			try
			{
				if (res != null && res.cooperationDocking != null)
				{
					if (this.projectids.Contains(res.projectID))
					{
						this.projectids.Remove(res.projectID);
					}
					this.INWindow.Collaboration.Add(res);
					if (this.projectids.Count == 0)
					{
						this.SendCooperationOfflineDataRequest();
						this.CloseCooperationTab();
					}
				}
			}
			catch (System.Exception ex)
			{
				ServiceUtil.Instance.Logger.Error(ex);
			}
		}
		public void OnCooperationDockingStutasDataGetted(CooperationDockingStutasResponse res)
		{
			try
			{
				if (res == null || res.cooperationDockingStutas != null)
				{
					foreach (CooperationDockingStutas item in res.cooperationDockingStutas)
					{
						System.Collections.Generic.List<CooperationStaff> staffs = this.dataService.GetCooperationStaff(item.uid);
						foreach (CooperationStaff sta in staffs)
						{
							sta.SynchronizeTo(item);
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				ServiceUtil.Instance.Logger.Error(ex);
			}
		}
		private void SendCooperationOfflineDataRequest()
		{
			CooperationOfflineDataRequest request = new CooperationOfflineDataRequest();
			request.jid = this.sessionService.Jid;
			this.connection.Send(PacketType.COOPERATION_OFFLINE, request);
		}
		private void CooperationNoticeResponseEvent(CooperationNoticeResponse response)
		{
			try
			{
				if (response != null)
				{
					System.Collections.Generic.List<CooperationStaff> lists = this.dataService.GetCooperationStaff(response.uid);
					if (lists != null)
					{
						foreach (CooperationStaff item in lists)
						{
							item.SynchronizeTo(response);
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				ServiceUtil.Instance.Logger.Error(ex);
			}
		}
	}
}
