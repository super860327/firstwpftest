using IDKin.IM.Communicate;
using IDKin.IM.Core;
using IDKin.IM.Core.Communicate;
using IDKin.IM.Core.Core;
using IDKin.IM.CustomComponents.Controls;
using IDKin.IM.Data;
using IDKin.IM.ImageService;
using IDKin.IM.Log;
using IDKin.IM.Protocol.Account;
using IDKin.IM.Protocol.Center;
using IDKin.IM.Protocol.Commons;
using IDKin.IM.Protocol.Cooperation;
using IDKin.IM.Protocol.Enterprise;
using IDKin.IM.Protocol.FileInfo;
using IDKin.IM.Windows.Comps;
using IDKin.IM.Windows.Comps.ChatTab;
using IDKin.IM.Windows.Model;
using IDKin.IM.Windows.Model.RecentLink;
using IDKin.IM.Windows.Properties;
using IDKin.IM.Windows.Util;
using IDKin.IM.Windows.View;
using IDKin.IM.Windows.View.Commons;
using IDKin.IM.Windows.View.EmailAlert;
using IDKin.IM.Windows.View.Pages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
namespace IDKin.IM.Windows.ViewModel
{
	public class INViewModel : DispatcherObject
	{
		private IConnection connection = null;
		private IFileService fileService = null;
		private IDataService dataService = null;
		private ISessionService sessionService = null;
		private IImageService imageService = null;
		private IUtilService utilService = null;
		private IWSClient wsClient = null;
		private ILogger logger = null;
		private INWindow inWindow = null;
		private DataModel dataModel = DataModel.Instance;
		private System.Timers.Timer keepliveTimer = null;
		private int oaMessageErrorCount = 0;
		public void InitService()
		{
			this.connection = ServiceUtil.Instance.Connection;
			this.fileService = ServiceUtil.Instance.FileService;
			this.dataService = ServiceUtil.Instance.DataService;
			this.sessionService = ServiceUtil.Instance.SessionService;
			this.imageService = ServiceUtil.Instance.ImageService;
			this.utilService = ServiceUtil.Instance.utilService;
			this.wsClient = ServiceUtil.Instance.WsClient;
			this.logger = ServiceUtil.Instance.Logger;
			this.inWindow = (this.dataService.INWindow as INWindow);
			this.AddEventListenerHandler();
		}
		private void AddEventListenerHandler()
		{
			this.connection.EventHandler.DepartmentListEvent = new DepartmentListHandler(this.DepartmentListEventHandle);
			this.connection.EventHandler.EntGroupListEvent = new EntGroupListHandler(this.EntGroupListEventHandle);
			this.connection.EventHandler.EntMessageEvent = new EntMessageHandler(this.EntMessageEvent);
			this.connection.EventHandler.EntGroupMessageEvent = new EntGroupMessageHandler(this.EntGroupMessageEvent);
			this.connection.EventHandler.EntGroupOpertaioneEvent = new EntGroupOperationHandler(this.EntGroupOperationEvent);
			this.connection.EventHandler.OffLineFileDataEvent = new OffLineFileDataHandler(this.FileDataRequestEvent);
			this.connection.EventHandler.CooperationStaffFileDownloadEvent = new CooperationStaffFileDownloadHandler(this.CooperationStaffFileDownloadEvent);
			this.connection.EventHandler.CooperationFileNoticeEvent = new CooperationFileNoticeHandler(this.CooperationFileNoticeEvent);
			this.connection.EventHandler.GroupFileDataEvent = new GroupFileDataHandler(this.GroupFileDataEvent);
			this.connection.EventHandler.AddRosterEvent = new AddRosterHandler(this.AddRosterEvent);
			this.connection.EventHandler.AddRosterAskEvent = new AddRosterAskHandler(this.AddRosterAskEvent);
			this.connection.EventHandler.RosterMessageEvent = new RosterMessageHandler(this.RosterMessageEvent);
			this.connection.EventHandler.RosterListEvent = new RosterListHandler(this.RosterListEvent);
			this.connection.EventHandler.NoticeEvent = new NoticeHandler(this.NoticeEvent);
			this.connection.EventHandler.LogOffDataEvent = new LogOffDataHandler(this.LogOffDataEvent);
			this.connection.EventHandler.OACircularEvent = new OACircularHandler(this.OACircularEvent);
			this.connection.EventHandler.OAMessageErrorEvent = new OAMessageErrorHandler(this.OAMessageErrorEvent);
			this.connection.EventHandler.OAScheduleEvent = new OAScheduleHanlder(this.OAScheduleEvent);
			this.connection.EventHandler.OAPlanEvent = new OAPlanHandler(this.OAPlanEvent);
			this.connection.EventHandler.StaffInfoEvent = new StaffInfoHandler(this.StaffInfoEvent);
			this.connection.EventHandler.CommonEvent = new CommonHandler(this.CommonEvent);
			this.connection.EventHandler.ContactEvent = new ContactHandler(this.ContactEvent);
			this.connection.EventHandler.CheckVersionUpdateEvent = new CheckVersionUpdateHandler(this.CheckVersionUpdateEvent);
			this.connection.EventHandler.CustomGroupEvent = new IDKin.IM.Communicate.CustomGroupHandler(this.GetCustomGroup);
			this.connection.EventHandler.EMailListEvent = new EMailListHandler(this.GetEmailList);
			this.connection.EventHandler.DepartmentBlockListEvent = new DepartmentBlockListHandler(this.DepartmentBlockListDataGetted);
			this.connection.EventHandler.CooperationStaffMessageEvent = new CooperationStaffMessageHandler(this.CooperationStaffMessageEvent);
			this.connection.EventHandler.CooperationCommonEvent = new CooperationCommonHandler(this.CooperationCommonEvent);
		}
		private void DepartmentBlockListDataGetted(DepartmentBlockListResponse response)
		{
			if (response != null)
			{
				if (response.deptBlockList != null)
				{
					this.sessionService.DeptBlockList.Clear();
					this.sessionService.DeptBlockList.AddRange(response.deptBlockList);
				}
			}
		}
		public bool ProfileUpdate(IDKin.IM.Core.Staff staff)
		{
			bool result;
			try
			{
				ProfileUpdateRequest request = new ProfileUpdateRequest();
				request.name = staff.Name;
				request.age = staff.Age;
				request.birthday = staff.Birthday;
				request.birthdayType = staff.BirthdayType;
				request.city = staff.City;
				request.country = staff.Country;
				request.email = staff.Email;
				request.job = staff.Job;
				request.mobile = staff.Mobile;
				request.name = staff.Name;
				request.nickname = staff.Nickname;
				request.province = staff.Province;
				request.school = staff.School;
				request.sex = (int)staff.Sex;
				request.signature = staff.Signature;
				request.telphone = staff.Telephone;
				request.uid = staff.Uid;
				request.img = staff.HeaderFileName;
				request.showScope = staff.ShowScope;
				request.description = staff.MyDescription;
				request.site = staff.MyHome;
				request.zodiac = staff.Zodiac;
				request.bloodType = staff.BloodType;
				request.constellation = staff.Constellation;
				request.extension = staff.Extension;
				this.connection = ServiceUtil.Instance.Connection;
				this.connection.Send(PacketType.PROFILE_UPDATE, request);
				result = true;
			}
			catch (System.Exception ex)
			{
				if (this.logger != null)
				{
					this.logger.Error(ex.ToString());
				}
				result = false;
			}
			return result;
		}
		private void CheckVersionUpdateEvent(CheckVersionResponse response)
		{
			if (response != null)
			{
				VersionType type = (VersionType)System.Enum.Parse(typeof(VersionType), response.type.ToString());
				UpdateItem item = new UpdateItem(response.url, response.name, response.updateInfo, type);
				if (WindowModel.Instance.IsOpenUpdateWindow())
				{
					UpdateWindow updateWindow = WindowModel.Instance.UpdateWindow;
					updateWindow.UpdateInfo(item);
				}
				else
				{
					if (type == VersionType.VERSION_UPDATE)
					{
						item.EndEvent = new EndEvent(this.UpdateVersionEnd);
						this.fileService.DownloadIDKinUpdatePackage(item);
					}
				}
			}
		}
		private void UpdateVersionEnd(string fileName, bool isCancel)
		{
			try
			{
				MessageBoxResult result = Alert.Show("是否需要更新到最新版本？", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Question);
				if (result == MessageBoxResult.OK && !string.IsNullOrEmpty(fileName))
				{
					Process.Start(fileName, "/sp- /silent /norestart");
					Application.Current.Shutdown(1000);
				}
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		public void GetAppUpdate(string jid, string version)
		{
			CheckVersionRequest request = new CheckVersionRequest();
			request.jid = jid;
			request.version = version;
			this.connection.Send(PacketType.CHECK_VERSION, request);
		}
		public void GetStaffInfo(long uid)
		{
			ProfileRequest request = new ProfileRequest();
			request.uid = uid;
			this.connection = ServiceUtil.Instance.Connection;
			this.connection.Send(PacketType.STAFF_INFO, request);
		}
		public void GetDepartmentList()
		{
			DepartmentListRequest request = new DepartmentListRequest();
			request.id = 0;
			request.pid = 0;
			this.connection.Send(PacketType.DEPARTMENT_LIST, request);
		}
		public void DepartmentListEventHandle(System.Collections.ObjectModel.Collection<DepartmentResponse> list)
		{
			try
			{
				if (list != null)
				{
					foreach (DepartmentResponse response in list)
					{
						if (response != null)
						{
							IDKin.IM.Core.Department department = new IDKin.IM.Core.Department();
							department.Id = response.id;
							department.Description = response.description;
							department.Name = response.name;
							department.Pid = response.pid;
							department.Uid = response.uid;
							this.dataService.AddDepartment(department);
							this.GetStaffList(department.Id);
							if (department.Id == this.sessionService.DepartmentId)
							{
								this.sessionService.DepartmentName = department.Name;
								this.inWindow.InitSelfDepartment();
							}
						}
					}
					this.ShowDepartment();
				}
			}
			catch (System.Exception e)
			{
				ServiceUtil.Instance.Logger.Error(e.ToString());
			}
		}
		private void ShowDepartment()
		{
			System.Collections.Generic.ICollection<IDKin.IM.Core.Department> departments = this.dataService.GetDepartmentList();
			if (departments != null && departments.Count > 0)
			{
				long currentRootId = 0L;
				foreach (IDKin.IM.Core.Department department in departments)
				{
					if (department.Pid == 0L)
					{
						currentRootId = department.Id;
						break;
					}
				}
				INWindow inWindow = this.dataService.INWindow as INWindow;
				if (inWindow != null)
				{
					inWindow.Employee.DepartmentTree.CurrentRootId = currentRootId;
					foreach (IDKin.IM.Core.Department department in departments)
					{
						if (inWindow != null && department.Pid != 0L)
						{
							inWindow.Employee.DepartmentTree.AddDepartment(department);
						}
					}
				}
				if (this.inWindow != null)
				{
					this.inWindow.Employee.DepartmentTree.UnParentDepartmentNodeProcessor();
				}
			}
		}
		private void GetStaffList(long departmentId)
		{
			StaffListRequest request = new StaffListRequest();
			request.department_id = departmentId;
			request.uid = this.sessionService.Uid;
			this.connection.Send(PacketType.STAFF_LIST, request);
		}
		private void NoticeEvent(NoticeResponse response)
		{
			if (response != null)
			{
				NotifyWindow notify = new NotifyWindow(new Notice
				{
					Date = response.date,
					From = response.from,
					Link = response.url,
					Message = response.message,
					MessageType = response.type,
					Title = response.title
				});
			}
		}
		private void RosterListEvent(RosterListResponse response)
		{
			try
			{
				if (response != null && response.user != null && response.user.Count > 0)
				{
					foreach (IDKin.IM.Protocol.Center.User user in response.user)
					{
						if (user != null)
						{
							Roster roster = new Roster();
							roster.Age = user.age;
							roster.Country = user.country;
							roster.Jid = user.jid;
							roster.Name = user.name;
							if (string.IsNullOrEmpty(user.name))
							{
								roster.Name = user.username;
							}
							roster.Nickname = user.nickname;
							roster.Province = user.province;
							roster.Signature = user.signature;
							roster.Status = (UserStatus)System.Enum.Parse(typeof(UserStatus), user.status.ToString());
							roster.Uid = user.uid;
							roster.UserName = user.username;
							if (this.dataService != null)
							{
								this.dataService.AddRoster(roster);
							}
							if (this.inWindow != null)
							{
								this.inWindow.FriendsList.AddRoster(roster);
							}
						}
					}
				}
			}
			catch (System.Exception e)
			{
				System.Console.WriteLine(e.ToString());
			}
		}
		private void ShowEntGroup()
		{
			System.Collections.Generic.ICollection<EntGroup> groups = this.dataService.GetEntGroupList();
			if (groups != null && groups.Count > 0)
			{
				foreach (EntGroup group in groups)
				{
					INWindow inWindow = this.dataService.INWindow as INWindow;
					if (inWindow != null)
					{
						inWindow.Employee.listEntGroup.AddEntGroup(group);
					}
				}
			}
		}
		private void ContactEvent(GetContactResponse response)
		{
			try
			{
				if (response != null && response.contact != null && response.contact.Count > 0)
				{
					System.Collections.Generic.List<RecentLinkInfo> list = this.dataModel.RecentLinkInfoList;
					foreach (Contact contact in response.contact)
					{
						if (contact != null)
						{
							list.Add(new RecentLinkInfo
							{
								Id = contact.id,
								RecentTime = contact.createTime,
								Type = (IsMarkType)System.Enum.Parse(typeof(IsMarkType), response.type.ToString())
							});
						}
					}
					list.Sort(new System.Comparison<RecentLinkInfo>(LocalDataUtil.Instance.CompareRecentLinkValue));
				}
			}
			catch (System.Exception e)
			{
				this.logger.Error(e.ToString());
			}
		}
		public void EntGroupListEventHandle(System.Collections.ObjectModel.Collection<GroupReadResponse> list)
		{
			try
			{
				if (list != null)
				{
					foreach (GroupReadResponse response in list)
					{
						if (response != null)
						{
							EntGroup group = new EntGroup();
							group.Admin = this.StringToLong(response.admin.Split(new char[]
							{
								':'
							}));
							group.Description = response.description;
							group.Gid = response.gid;
							group.Member = this.StringToLong(response.member.Split(new char[]
							{
								':'
							}));
							group.Name = response.name;
							group.AdminIcon = this.imageService.GetIcon(ImageTypeIcon.GroupIconAdmin);
							this.dataService.AddEntGroup(group);
						}
					}
					this.ShowEntGroup();
				}
			}
			catch (System.Exception e)
			{
				ServiceUtil.Instance.Logger.Error(e.ToString());
			}
		}
		public void GetGroupList()
		{
			GroupListRequest request = new GroupListRequest();
			request.uid = this.sessionService.Uid;
			this.connection.Send(PacketType.ENT_GROUP_LIST, request);
		}
		public void GetOfflineMessage(string jid)
		{
			OfflineDataRequest request = new OfflineDataRequest();
			request.jid = jid;
			this.connection.Send(PacketType.OFFLINE_DATA, request);
		}
		public void GetOADataByType(int type)
		{
			OAMessageRequest request = new OAMessageRequest();
			request.jid = this.sessionService.Jid;
			request.ticket = this.sessionService.Ticket;
			request.type = type;
			this.connection.Send(PacketType.OA_MESSAGE, request);
		}
		public void GetOAAllData()
		{
			OAMessageRequest request = new OAMessageRequest();
			request.jid = this.sessionService.Jid;
			request.ticket = this.sessionService.Ticket;
			request.type = 1;
			this.connection.Send(PacketType.OA_MESSAGE, request);
		}
		public void GetRosterList()
		{
			RosterListRequest request = new RosterListRequest();
			request.jid = this.sessionService.Jid;
			request.uid = this.sessionService.Uid;
			this.connection.Send(PacketType.ROSTER_LIST, request);
		}
		private void AddRosterAskEvent(RosterAddResponse response)
		{
			try
			{
				if (response != null)
				{
					this.AddRosterAskTip(new Message
					{
						MessageObjectType = MessageActorType.AddRosterAsk,
						MessageObject = response
					}, response.uid);
				}
			}
			catch (System.Exception ex)
			{
				ServiceUtil.Instance.Logger.Error(ex.ToString());
			}
		}
		private void ArgeeAndRoster(RosterAddResponse response)
		{
			if (response != null)
			{
				MessageBox.Show(response.user.name + "同意您添加为好友!", "提示");
				Roster roster = new Roster();
				roster.Uid = response.user.uid;
				roster.Jid = response.user.jid;
				roster.Name = response.user.name;
				roster.Nickname = response.user.nickname;
				roster.Status = (UserStatus)System.Enum.Parse(typeof(UserStatus), response.user.status.ToString());
				roster.Signature = response.user.signature;
				this.dataService.AddRoster(roster);
				this.inWindow.FriendsList.AddRoster(roster);
			}
		}
		private void RejectRoster(RosterAddResponse response)
		{
			MessageBox.Show(response.user.name + "拒绝您添加为好友!", "提示");
		}
		private void AddRosterEvent(RosterAddRequest request)
		{
			try
			{
				if (request != null && request.user != null)
				{
					this.AddRosterAskTip(new Message
					{
						MessageObjectType = MessageActorType.AddRoster,
						MessageObject = request
					}, request.uid);
				}
			}
			catch (System.Exception ex)
			{
				ServiceUtil.Instance.Logger.Error(ex.ToString());
			}
		}
		private void AddRosterAskTip(Message message, long uid)
		{
			this.dataModel.AddMessage(uid, message.MessageObjectType, message);
			NotifyIconUtil.Instance.SetFlashIcon(FlashIconType.MessageCenter);
			NotifyIconUtil.Instance.StartFlashing();
			if (this.IsAddMessageBox())
			{
				MessageBoxWindow mbw = this.dataModel.GetMessageBox();
				if (mbw == null)
				{
					mbw = new MessageBoxWindow();
					this.dataModel.SetMessageBox(mbw);
				}
				mbw.Refresh();
				mbw.Show();
			}
		}
		private void GroupFileDataEvent(GroupFileUploadResponse response)
		{
			try
			{
				if (response != null && response.fromUid != this.sessionService.Uid)
				{
					EntGroupTab item = this.dataService.GetEntGroupChatTab(response.gid) as EntGroupTab;
					if (item == null)
					{
						EntGroup group = this.dataService.GetEntGroup(response.gid);
						if (group != null)
						{
							item = new EntGroupTab(group);
							item.SetFlashingStyle();
							((INWindow)this.dataService.INWindow).ContentTab.Items.Add(item);
							this.dataService.AddEntGroupChatTab(group.Gid, item);
						}
					}
					if (item != null)
					{
						GroupChatTabControl tab = item.Content as GroupChatTabControl;
						if (tab != null)
						{
							tab.AddReceiveHttpFile(response);
							if (!this.IsCurrentItem(item))
							{
								item.SetFlashingStyle();
							}
						}
					}
					this.PickUpMessageProcessor();
					MessageBoxWindow mbw = this.dataModel.GetMessageBox();
					if (mbw != null)
					{
						mbw.Refresh();
					}
					if (this.inWindow.WindowState == WindowState.Minimized && item != null)
					{
						this.ActiveInWindow();
						this.SelectedTab(item);
					}
					else
					{
						this.ActiveInWindow();
					}
					this.FlashIconPorcessor();
				}
			}
			catch (System.Exception ex)
			{
				ServiceUtil.Instance.Logger.Error(ex.ToString());
			}
		}
		private void FileDataRequestEvent(OfflineFileResponse response)
		{
			INWindow inWindow = this.dataService.INWindow as INWindow;
			if (response != null && inWindow != null)
			{
				EntStaffTab item = this.dataService.GetStaffChatTab(response.fromUid) as EntStaffTab;
				if (item == null)
				{
					IDKin.IM.Core.Staff staff = this.dataService.GetStaff(response.fromUid);
					if (staff != null)
					{
						item = new EntStaffTab(staff);
						item.SetFlashingStyle();
						((INWindow)this.dataService.INWindow).ContentTab.Items.Add(item);
						this.dataService.AddStaffChatTab(staff.Uid, item);
					}
				}
				if (item != null)
				{
					PersonalChatTabControl tab = item.TabContent;
					if (tab != null)
					{
						tab.AddReceiveHttpFile(response);
						if (!this.IsCurrentItem(item))
						{
							item.SetFlashingStyle();
						}
					}
				}
				this.PickUpMessageProcessor();
				MessageBoxWindow mbw = this.dataModel.GetMessageBox();
				if (mbw != null)
				{
					mbw.Refresh();
				}
				if (inWindow.WindowState == WindowState.Minimized && item != null)
				{
					this.ActiveInWindow();
					this.SelectedTab(item);
				}
				else
				{
					this.ActiveInWindow();
				}
				this.FlashIconPorcessor();
			}
		}
		private void CooperationStaffFileDownloadEvent(CooperationFileDownloadResponse response)
		{
			try
			{
				INWindow inWindow = this.dataService.INWindow as INWindow;
				if (response != null && inWindow != null)
				{
					CoopStaffTab item = this.dataService.GetCooperationStaffChatTab(response.fromUid, response.projectId) as CoopStaffTab;
					if (item == null)
					{
						CooperationStaff staff = this.dataService.GetCooperationStaff(response.fromUid, response.projectId);
						CooperationProjectWrapper cooperationProjectWrapper = this.dataService.GetCooperationProjectWrapper(response.projectId);
						if (staff != null && cooperationProjectWrapper != null)
						{
							item = new CoopStaffTab(staff, cooperationProjectWrapper);
							item.SetFlashingStyle();
							((INWindow)this.dataService.INWindow).ContentTab.Items.Add(item);
							this.dataService.AddCooperationStaffChatTab(staff.Uid, staff.UnitedProjectid, item);
						}
					}
					if (item != null)
					{
						CoopStaffChatTabControl tab = item.TabContent;
						if (tab != null)
						{
							tab.AddReceiveHttpFile(response);
							if (!this.IsCurrentItem(item))
							{
								item.SetFlashingStyle();
							}
						}
					}
					this.PickUpMessageProcessor();
					MessageBoxWindow mbw = this.dataModel.GetMessageBox();
					if (mbw != null)
					{
						mbw.Refresh();
					}
					if (inWindow.WindowState == WindowState.Minimized && item != null)
					{
						this.ActiveInWindow();
						this.SelectedTab(item);
					}
					else
					{
						this.ActiveInWindow();
					}
					this.FlashIconPorcessor();
				}
			}
			catch (System.Exception)
			{
			}
		}
		private void CooperationFileNoticeEvent(CooperationFileNoticeResponse response)
		{
			try
			{
				if (response != null)
				{
					if (this.sessionService.Status != UserStatus.Offline)
					{
						this.CooperationStaffMessageEvent(new CooperationMessageResponse
						{
							from_jid = response.fromUid + "@null/null",
							message = response.fileName,
							project_id = response.projectId,
							type = 7
						});
					}
				}
			}
			catch (System.Exception)
			{
			}
		}
		private void FlashIconPorcessor()
		{
			System.Collections.Generic.List<Message> list2 = DataModel.Instance.GetLastMessage();
			if (list2 != null && list2.Count > 0)
			{
				Message message2 = list2[0];
				if (message2 != null)
				{
					if (message2.MessageObjectType == MessageActorType.EntStaff)
					{
						NotifyIconUtil.Instance.SetFlashIcon(FlashIconType.EntStaff);
					}
					if (message2.MessageObjectType == MessageActorType.EntGroup)
					{
						NotifyIconUtil.Instance.SetFlashIcon(FlashIconType.EntGroup);
					}
					if (message2.MessageObjectType == MessageActorType.Roster)
					{
						NotifyIconUtil.Instance.SetFlashIcon(FlashIconType.Roster);
					}
					if (message2.MessageObjectType == MessageActorType.AddRoster)
					{
						NotifyIconUtil.Instance.SetFlashIcon(FlashIconType.MessageCenter);
					}
					if (message2.MessageObjectType == MessageActorType.AddRosterAsk)
					{
						NotifyIconUtil.Instance.SetFlashIcon(FlashIconType.MessageCenter);
					}
				}
			}
			else
			{
				NotifyIconUtil.Instance.SetFlashIcon(FlashIconType.Default);
			}
		}
		private void PickUpMessageProcessor()
		{
			ItemCollection ic = this.inWindow.ContentTab.Items;
			foreach (TabItem item in (System.Collections.IEnumerable)ic)
			{
				if (item != null)
				{
					CloseableTabItem cti = item as CloseableTabItem;
					TabItemHeaderControl tabHeader = null;
					if (cti != null)
					{
						tabHeader = (cti.Header as TabItemHeaderControl);
					}
					PersonalChatTabControl pctc = item.Content as PersonalChatTabControl;
					GroupChatTabControl gctc = item.Content as GroupChatTabControl;
					FriendsChatTabControl fctc = item.Content as FriendsChatTabControl;
					CoopStaffChatTabControl coopStaffChatTabControl = item.Content as CoopStaffChatTabControl;
					if (pctc != null)
					{
						System.Collections.Generic.List<Message> list = this.dataModel.GetMessage(pctc.StaffId, MessageActorType.EntStaff);
						if (list != null)
						{
							if (tabHeader != null)
							{
								tabHeader.SetFlashingStyle();
							}
							this.ShowStaffNewMessage(pctc.StaffId, list);
						}
					}
					if (gctc != null)
					{
						System.Collections.Generic.List<Message> list = this.dataModel.GetMessage(gctc.GroupId, MessageActorType.EntGroup);
						if (list != null)
						{
							if (tabHeader != null)
							{
								tabHeader.SetFlashingStyle();
							}
							this.ShowGroupNewMessage(gctc.GroupId, list);
						}
					}
					if (fctc != null)
					{
						System.Collections.Generic.List<Message> list = this.dataModel.GetMessage(fctc.RosterId, MessageActorType.Roster);
						if (list != null)
						{
							if (tabHeader != null)
							{
								tabHeader.SetFlashingStyle();
							}
							this.ShowRosterNewMessage(fctc.RosterId, list);
						}
					}
					if (coopStaffChatTabControl != null)
					{
						System.Collections.Generic.List<Message> list = this.dataModel.GetCooperationStaffMessage(coopStaffChatTabControl.Uid, coopStaffChatTabControl.Projectid, MessageActorType.CooperationStaff);
						if (list != null)
						{
							if (tabHeader != null)
							{
								tabHeader.SetFlashingStyle();
							}
							this.ShowCooperationStaffNewMessage(coopStaffChatTabControl.Uid, coopStaffChatTabControl.Projectid, list);
						}
					}
				}
			}
		}
		private void ShowRosterNewMessage(long rosterId, System.Collections.Generic.List<Message> list)
		{
			if (list != null)
			{
				foreach (Message message in list)
				{
					this.RosterNewMessage(message);
				}
				DataModel.Instance.RemoveMessage(rosterId, MessageActorType.Roster);
			}
		}
		private void ShowGroupNewMessage(long groupId, System.Collections.Generic.List<Message> list)
		{
			if (list != null)
			{
				foreach (Message message in list)
				{
					this.GroupNewMessage(message);
				}
				DataModel.Instance.RemoveMessage(groupId, MessageActorType.EntGroup);
			}
		}
		private void ShowStaffNewMessage(long staffId, System.Collections.Generic.List<Message> list)
		{
			if (list != null)
			{
				foreach (Message message in list)
				{
					this.StaffNewMessage(message);
				}
				this.dataModel.RemoveMessage(staffId, MessageActorType.EntStaff);
			}
		}
		private void ShowCooperationStaffNewMessage(long staffId, string projectid, System.Collections.Generic.List<Message> list)
		{
			if (list != null)
			{
				foreach (Message message in list)
				{
					this.CooperationStaffNewMessage(message);
				}
				this.dataModel.RemoveCooperationStatffMessage(staffId, projectid, MessageActorType.CooperationStaff);
			}
		}
		private void RosterNewMessage(Message msg)
		{
			RosterTab item = this.dataService.GetRosterChatTab((long)((ulong)Jid.GetUid(msg.FromJid))) as RosterTab;
			if (item == null)
			{
				Roster roster = this.dataService.GetRoster((long)((ulong)Jid.GetUid(msg.FromJid)));
				if (roster != null)
				{
					item = new RosterTab(roster);
					item.SetDefaultStyle();
					((INWindow)this.dataService.INWindow).ContentTab.Items.Add(item);
					this.dataService.AddRosterChatTab(roster.Uid, item);
					((INWindow)this.dataService.INWindow).ContentTab.SelectedItem = item;
				}
			}
			FriendsChatTabControl tab = item.TabContent;
			if (tab != null)
			{
				tab.ChatComponent.AddMessageRoster(msg, false);
				tab.ChatComponent.inputMsgBox.Focus();
			}
		}
		private void GroupNewMessage(Message message)
		{
			EntGroupTab item = this.dataService.GetEntGroupChatTab(message.Gid) as EntGroupTab;
			if (item == null)
			{
				EntGroup group = this.dataService.GetEntGroup(message.Gid);
				if (group != null)
				{
					item = new EntGroupTab(group);
					item.SetDefaultStyle();
					((INWindow)this.dataService.INWindow).ContentTab.Items.Add(item);
					this.dataService.AddEntGroupChatTab(group.Gid, item);
					((INWindow)this.dataService.INWindow).ContentTab.SelectedItem = item;
				}
			}
			GroupChatTabControl tab = item.TabContent;
			if (tab != null)
			{
				tab.ChatComponent.AddMessageGroup(message, false);
				tab.ChatComponent.inputMsgBox.Focus();
			}
		}
		private void StaffNewMessage(Message message)
		{
			EntStaffTab item = this.dataService.GetStaffChatTab((long)((ulong)Jid.GetUid(message.FromJid))) as EntStaffTab;
			if (item == null)
			{
				IDKin.IM.Core.Staff staff = this.dataService.GetStaff((long)((ulong)Jid.GetUid(message.FromJid)));
				if (staff != null)
				{
					item = new EntStaffTab(staff);
					item.SetDefaultStyle();
					((INWindow)this.dataService.INWindow).ContentTab.Items.Add(item);
					this.dataService.AddStaffChatTab(staff.Uid, item);
					((INWindow)this.dataService.INWindow).ContentTab.SelectedItem = item;
				}
			}
			PersonalChatTabControl tab = item.TabContent;
			if (tab != null)
			{
				tab.ChatComponent.AddMessageStaff(message, false);
				tab.ChatComponent.inputMsgBox.Focus();
			}
		}
		private void CooperationStaffNewMessage(Message message)
		{
			CoopStaffTab item = this.dataService.GetCooperationStaffChatTab((long)((ulong)Jid.GetUid(message.FromJid)), message.ProjectId) as CoopStaffTab;
			if (item == null)
			{
				CooperationStaff staff = this.dataService.GetCooperationStaff((long)((ulong)Jid.GetUid(message.FromJid)), message.ProjectId);
				CooperationProjectWrapper cooperationProjectWrapper = this.dataService.GetCooperationProjectWrapper(message.ProjectId);
				if (staff != null && cooperationProjectWrapper != null)
				{
					item = new CoopStaffTab(staff, cooperationProjectWrapper);
					item.SetDefaultStyle();
					((INWindow)this.dataService.INWindow).ContentTab.Items.Add(item);
					this.dataService.AddCooperationStaffChatTab(staff.Uid, staff.UnitedProjectid, item);
					((INWindow)this.dataService.INWindow).ContentTab.SelectedItem = item;
				}
			}
			CoopStaffChatTabControl tab = item.TabContent;
			if (tab != null)
			{
				tab.ChatComponent.AddCooperationMessageStaff(message, false);
				tab.ChatComponent.inputMsgBox.Focus();
			}
		}
		private void GroupMessageCacheProcessor(GroupFileUploadResponse response, GroupChatTabControl tab)
		{
			System.Collections.Generic.List<Message> list = this.dataModel.GetMessage(response.gid, MessageActorType.EntGroup);
			if (list != null)
			{
				foreach (Message message in list)
				{
					tab.ChatComponent.AddMessageGroup(message, false);
				}
			}
			this.dataModel.RemoveMessage(response.gid, MessageActorType.EntGroup);
			MessageBoxWindow box = this.dataModel.GetMessageBox();
			if (box != null)
			{
				box.Refresh();
			}
		}
		private void StaffMessageCacheProcessor(OfflineFileResponse response, PersonalChatTabControl tab)
		{
			System.Collections.Generic.List<Message> list = this.dataModel.GetMessage(response.fromUid, MessageActorType.EntStaff);
			if (list != null)
			{
				foreach (Message message in list)
				{
					tab.ChatComponent.AddMessageStaff(message, false);
				}
			}
			DataModel.Instance.RemoveMessage(response.fromUid, MessageActorType.EntStaff);
			MessageBoxWindow box = this.dataModel.GetMessageBox();
			if (box != null)
			{
				box.Refresh();
			}
		}
		private void EntGroupOperationEvent(GroupOperationResponse response)
		{
			if (response != null)
			{
				switch (response.type)
				{
				case 1:
					this.EntGroupNoticeCreate(response);
					break;
				case 2:
					this.EntGroupNoticeUpdate(response);
					break;
				case 3:
					this.EntGroupNoticeDelete(response);
					break;
				case 5:
					this.EntGroupNoticeAddMember(response);
					break;
				case 6:
					this.EntGroupNoticeRemoveMember(response);
					break;
				case 7:
					this.EntGroupNoticeAddAdmin(response);
					break;
				case 8:
					this.EntGroupNoticeRemoveAdmin(response);
					break;
				case 9:
					this.EntGroupNoticeExit(response);
					break;
				}
			}
		}
		private void EntGroupNoticeRemoveMember(GroupOperationResponse response)
		{
			EntGroup group = this.dataService.GetEntGroup((long)((ulong)((uint)response.gid)));
			if (group != null)
			{
				long[] newMembers = this.StringToLong(response.member.Split(new char[]
				{
					':'
				}));
				if (-1 == this.FingGroupMember(newMembers, this.sessionService.Uid))
				{
					group.RemoveLocalMembers(newMembers);
					this.RemoveGroupMemberHandler(newMembers, group.Gid);
				}
				else
				{
					this.EntGroupNoticeDelete(response);
				}
			}
		}
		public void RemoveGroupMemberHandler(long[] deleMembers, long gid)
		{
			try
			{
				if (deleMembers.Length > 0)
				{
					EntGroup entGroup = this.dataService.GetEntGroup(gid);
					INWindow window = this.dataService.INWindow as INWindow;
					if (window != null)
					{
						window.Employee.listEntGroup.UpdateGroupInfo(entGroup);
					}
					EntGroupTab item = this.dataService.GetEntGroupChatTab(gid) as EntGroupTab;
					if (item != null)
					{
						GroupChatTabControl tab = item.TabContent;
						if (tab != null)
						{
							tab.GroupMemberControl.GroupMemberList.RemoveMembers(deleMembers);
							tab.ChatComponent.AddMessageNotice(this.GetGroupMemberNames(deleMembers).ToString() + "已被管理员删除!");
						}
					}
				}
			}
			catch (System.Exception e)
			{
				ServiceUtil.Instance.Logger.Error(e.ToString());
			}
		}
		private void EntGroupNoticeAddMember(GroupOperationResponse response)
		{
			EntGroup group = this.dataService.GetEntGroup(response.gid);
			if (group != null)
			{
				long[] newMembers = this.StringToLong(response.member.Split(new char[]
				{
					':'
				}));
				if (-1 == this.FingGroupMember(newMembers, this.sessionService.Uid))
				{
					group.AddLocalMembers(newMembers);
					this.AddEntGroupHandler(newMembers, group.Gid);
				}
			}
			else
			{
				this.EntGroupNoticeCreate(response);
			}
		}
		public void AddEntGroupHandler(long[] addMembers, long gid)
		{
			try
			{
				if (addMembers.Length > 0)
				{
					EntGroup entGroup = this.dataService.GetEntGroup(gid);
					INWindow window = this.dataService.INWindow as INWindow;
					if (window != null)
					{
						window.Employee.listEntGroup.UpdateGroupInfo(entGroup);
					}
					EntGroupTab item = this.dataService.GetEntGroupChatTab(gid) as EntGroupTab;
					if (item != null)
					{
						GroupChatTabControl tab = item.TabContent;
						if (tab != null)
						{
							tab.ChatComponent.AddMessageNotice(this.GetGroupMemberNames(addMembers).ToString() + "被管理员加入群");
							tab.GroupMemberControl.GroupMemberList.AddGroupMembers(addMembers);
						}
					}
				}
			}
			catch (System.Exception e)
			{
				ServiceUtil.Instance.Logger.Error(e.ToString());
			}
		}
		private int FingGroupMember(long[] members, long uid)
		{
			int result;
			for (int i = 0; i < members.Length; i++)
			{
				uint member = (uint)members[i];
				if ((ulong)member == (ulong)uid)
				{
					result = 1;
					return result;
				}
			}
			result = -1;
			return result;
		}
		private void EntGroupNoticeRemoveAdmin(GroupOperationResponse response)
		{
			EntGroup group = this.dataService.GetEntGroup(response.gid);
			if (group != null)
			{
				long[] newAdmin = this.StringToLong(response.member.Split(new char[]
				{
					':'
				}));
				group.DeleLocalAdmins(newAdmin);
				this.DeleAdminHandler(newAdmin, group.Gid);
			}
		}
		public void DeleAdminHandler(long[] deleAdmins, long gid)
		{
			try
			{
				if (deleAdmins.Length > 0)
				{
					EntGroupTab item = this.dataService.GetEntGroupChatTab(gid) as EntGroupTab;
					if (item != null)
					{
						GroupChatTabControl tab = item.TabContent;
						if (tab != null)
						{
							tab.DeleNewAdminHandler(deleAdmins);
							tab.ChatComponent.AddMessageNotice(this.GetGroupMemberNames(deleAdmins).ToString() + "被取消管理员");
						}
					}
				}
			}
			catch (System.Exception e)
			{
				ServiceUtil.Instance.Logger.Error(e.ToString());
			}
		}
		private void EntGroupNoticeAddAdmin(GroupOperationResponse response)
		{
			EntGroup group = this.dataService.GetEntGroup(response.gid);
			if (group != null)
			{
				long[] newAdmin = this.StringToLong(response.member.Split(new char[]
				{
					':'
				}));
				group.AddLocalAdmins(newAdmin);
				this.AddAdminHandler(newAdmin, group.Gid);
			}
		}
		public void AddAdminHandler(long[] admins, long gid)
		{
			try
			{
				if (admins.Length > 0)
				{
					EntGroupTab item = this.dataService.GetEntGroupChatTab(gid) as EntGroupTab;
					if (item != null)
					{
						GroupChatTabControl tab = item.TabContent;
						if (tab != null)
						{
							tab.AddNewAdminHandler(admins);
							tab.ChatComponent.AddMessageNotice(this.GetGroupMemberNames(admins).ToString() + "被提为管理员");
						}
					}
				}
			}
			catch (System.Exception e)
			{
				ServiceUtil.Instance.Logger.Error(e.ToString());
			}
		}
		public void AddNewAdminHandler(long[] admins)
		{
			try
			{
				if (admins.Length > 0)
				{
					for (int i = 0; i < admins.Length; i++)
					{
						long adm = admins[i];
					}
				}
			}
			catch (System.Exception e)
			{
				ServiceUtil.Instance.Logger.Error(e.ToString());
			}
		}
		private void EntGroupNoticeUpdate(GroupOperationResponse response)
		{
			EntGroup group = this.dataService.GetEntGroup(response.gid);
			if (group != null)
			{
				group.Name = response.name;
				group.Description = response.description;
				this.UpdateGroupHandler(group);
			}
		}
		public void UpdateGroupHandler(EntGroup group)
		{
			try
			{
				if (group != null)
				{
					INWindow window = this.dataService.INWindow as INWindow;
					if (window != null)
					{
						window.Employee.listEntGroup.UpdateGroupInfo(group);
					}
				}
			}
			catch (System.Exception e)
			{
				ServiceUtil.Instance.Logger.Error(e.ToString());
			}
		}
		private void EntGroupNoticeCreate(GroupOperationResponse response)
		{
			EntGroup group = this.dataService.GetEntGroup(response.gid);
			if (group == null)
			{
				group = new EntGroup();
				group.Gid = response.gid;
				group.AdminIcon = this.imageService.GetIcon(ImageTypeIcon.GroupIconAdmin);
				this.dataService.AddEntGroup(group);
			}
			group.Name = response.name;
			group.Description = response.description;
			group.Member = this.StringToLong(response.member.Split(new char[]
			{
				':'
			}));
			group.Admin = this.StringToLong(response.admin.Split(new char[]
			{
				':'
			}));
			this.CreateEntGroupHandler(group);
		}
		public void CreateEntGroupHandler(EntGroup group)
		{
			if (group != null)
			{
				INWindow window = this.dataService.INWindow as INWindow;
				if (window != null)
				{
					window.Employee.listEntGroup.AddEntGroup(group);
				}
			}
		}
		private void EntGroupNoticeExit(GroupOperationResponse vo)
		{
			EntGroup group = this.dataService.GetEntGroup((long)((ulong)((uint)vo.gid)));
			if (group != null)
			{
				long[] newMembers = this.StringToLong(vo.member.Split(new char[]
				{
					':'
				}));
				if (newMembers.Length == 1)
				{
					if (newMembers[0] != this.sessionService.Uid)
					{
						group.RemoveLocalMember(newMembers[0]);
						this.ExitEntGroupHandler(newMembers, group.Gid);
					}
					else
					{
						this.EntGroupNoticeDelete(vo);
					}
				}
			}
		}
		private void EntGroupNoticeDelete(GroupOperationResponse vo)
		{
			EntGroup group = this.dataService.GetEntGroup((long)((ulong)((uint)vo.gid)));
			if (group != null)
			{
				this.DeleGroupHandler(group);
			}
		}
		public void DeleGroupHandler(EntGroup group)
		{
			try
			{
				if (group != null)
				{
					EntGroupTab item = this.dataService.GetEntGroupChatTab(group.Gid) as EntGroupTab;
					if (item != null)
					{
						this.ItemGroupCloseTab(item, group.Gid);
					}
					INWindow window = this.dataService.INWindow as INWindow;
					if (window != null)
					{
						window.Employee.listEntGroup.DeleEntGroup(group);
					}
					this.dataService.RemoveEntGroup(group.Gid);
				}
			}
			catch (System.Exception e)
			{
				ServiceUtil.Instance.Logger.Error(e.ToString());
			}
		}
		public void ExitEntGroupHandler(long[] members, long gid)
		{
			try
			{
				if (members.Length == 1)
				{
					EntGroup entGroup = this.dataService.GetEntGroup(gid);
					INWindow inWindow = this.dataService.INWindow as INWindow;
					if (inWindow == null)
					{
						inWindow.Employee.listEntGroup.UpdateGroupInfo(entGroup);
					}
					EntGroupTab item = this.dataService.GetEntGroupChatTab(gid) as EntGroupTab;
					if (item != null)
					{
						GroupChatTabControl tab = item.TabContent;
						if (tab != null)
						{
							IDKin.IM.Core.Staff staff = this.dataService.GetStaff(members[0]);
							if (staff != null)
							{
								tab.ChatComponent.AddMessageNotice(staff.Name + "退出群!");
							}
						}
					}
				}
			}
			catch (System.Exception e)
			{
				ServiceUtil.Instance.Logger.Error(e.ToString());
			}
		}
		private void ItemGroupCloseTab(TabItem tabItem, long gid)
		{
			if (tabItem != null)
			{
				TabControl tabControl = tabItem.Parent as TabControl;
				if (tabControl != null)
				{
					tabControl.Items.Remove(tabItem);
					this.dataService.RemoveEntGroupChatTab(gid);
				}
			}
		}
		private void EntGroupMessageEvent(GroupMessageResponse response)
		{
			try
			{
				if (response != null && response.from_jid != this.sessionService.Jid && response.type == 0)
				{
					Message msg = new Message();
					MessageStyle ms = new MessageStyle(response.style);
					msg.CreateTime = response.date;
					msg.FromJid = response.from_jid;
					msg.MessageBlocks = this.utilService.MessageDecode(response.message);
					msg.Gid = response.gid;
					msg.Style = ms;
					msg.MessageObjectType = MessageActorType.EntGroup;
					if (this.IsNotifyFlashing(msg))
					{
						if (this.IsShieldMessage(msg))
						{
							INWindow inWindow = this.dataService.INWindow as INWindow;
							if (inWindow != null)
							{
								ItemGroup group = inWindow.Employee.listEntGroup.FindItemGroup(msg.Gid);
								if (group != null)
								{
									group.AddMessageCache(msg);
									group.UpdateShieldCount();
								}
							}
						}
						else
						{
							NotifyIconUtil.Instance.SetFlashIcon(FlashIconType.EntGroup);
							NotifyIconUtil.Instance.StartFlashing();
							this.dataModel.AddMessage(msg.Gid, MessageActorType.EntGroup, msg);
							if (this.IsAddMessageBox())
							{
								MessageBoxWindow mbw = this.dataModel.GetMessageBox();
								if (mbw == null)
								{
									mbw = new MessageBoxWindow();
									this.dataModel.SetMessageBox(mbw);
								}
								mbw.Refresh();
								mbw.Show();
							}
						}
					}
					else
					{
						this.GroupAddMessage(msg);
					}
				}
			}
			catch (System.Exception ex)
			{
				ServiceUtil.Instance.Logger.Error(ex.ToString());
			}
		}
		private void GroupAddMessage(Message message)
		{
			EntGroupTab item = this.dataService.GetEntGroupChatTab(message.Gid) as EntGroupTab;
			if (item != null)
			{
				GroupChatTabControl tab = item.TabContent;
				if (tab != null)
				{
					tab.ChatComponent.AddMessageGroup(message, false);
				}
			}
			INWindow inWindow = this.dataService.INWindow as INWindow;
			if (inWindow != null)
			{
				if (!this.IsCurrentItem(item))
				{
					TabItemHeaderControl header = item.Header as TabItemHeaderControl;
					if (header != null)
					{
						header.SetFlashingStyle();
					}
					this.TaskBarFlash();
				}
				else
				{
					if (!inWindow.IsActive)
					{
						this.TaskBarFlash();
					}
				}
			}
		}
		private bool IsCurrentItem(CloseableTabItem item)
		{
			object currentItem = ((INWindow)this.dataService.INWindow).ContentTab.SelectedItem;
			return item.Equals(currentItem);
		}
		private void EntMessageEvent(MessageResponse response)
		{
			try
			{
				if (response != null)
				{
					if (response.type != 3 && response.type != 6)
					{
						Message msg = new Message();
						MessageStyle ms = new MessageStyle(response.style);
						msg.CreateTime = response.date;
						msg.FromJid = response.from_jid;
						msg.MessageBlocks = this.utilService.MessageDecode(response.message);
						msg.MessageString = response.message;
						msg.Style = ms;
						msg.VOType = response.type;
						msg.MessageObjectType = MessageActorType.EntStaff;
						this.AutoReplyProcessor(msg);
						if (this.IsNotifyFlashing(msg))
						{
							NotifyIconUtil.Instance.SetFlashIcon(FlashIconType.EntStaff);
							NotifyIconUtil.Instance.StartFlashing();
							this.dataModel.AddMessage((long)((ulong)Jid.GetUid(msg.FromJid)), MessageActorType.EntStaff, msg);
							if (this.IsAddMessageBox())
							{
								MessageBoxWindow mbw = this.dataModel.GetMessageBox();
								if (mbw == null)
								{
									mbw = new MessageBoxWindow();
									this.dataModel.SetMessageBox(mbw);
								}
								mbw.Refresh();
								mbw.Show();
							}
						}
						else
						{
							this.StaffAddMessage(msg);
						}
						INWindow inWindow = this.dataService.INWindow as INWindow;
						if (inWindow != null)
						{
							inWindow.Employee.RecentLinkListItem.AddRecentLink(RecentLinkType.EntStaffChat, (long)((ulong)Jid.GetUid(msg.FromJid)));
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				System.Console.WriteLine(ex.ToString());
			}
		}
		private void CooperationStaffMessageEvent(CooperationMessageResponse response)
		{
			try
			{
				if (response != null)
				{
					if (response.type != 3 && response.type != 6)
					{
						Message msg = new Message();
						MessageStyle ms = new MessageStyle(response.style);
						msg.CreateTime = response.date;
						msg.FromJid = response.from_jid;
						msg.ProjectId = response.project_id;
						msg.MessageBlocks = this.utilService.MessageDecode(response.message);
						msg.MessageString = response.message;
						msg.Style = ms;
						msg.VOType = response.type;
						msg.MessageObjectType = MessageActorType.CooperationStaff;
						this.AutoReplyCooperationMessageProcessor(msg);
						if (this.IsNotifyFlashing(msg))
						{
							NotifyIconUtil.Instance.SetFlashIcon(FlashIconType.EntStaff);
							NotifyIconUtil.Instance.StartFlashing();
							this.dataModel.AddCooperationStatffMessage((long)((ulong)Jid.GetUid(msg.FromJid)), msg.ProjectId, MessageActorType.CooperationStaff, msg);
							if (this.IsAddMessageBox())
							{
								MessageBoxWindow mbw = this.dataModel.GetMessageBox();
								if (mbw == null)
								{
									mbw = new MessageBoxWindow();
									this.dataModel.SetMessageBox(mbw);
								}
								mbw.Refresh();
								mbw.Show();
							}
						}
						else
						{
							this.CooperationStaffAddMessage(msg);
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				System.Console.WriteLine(ex.ToString());
			}
		}
		private bool IsShieldMessage(Message msg)
		{
			bool result;
			if (msg != null && msg.Gid != 0L)
			{
				INWindow inWindow = this.dataService.INWindow as INWindow;
				if (inWindow != null)
				{
					ItemGroup group = inWindow.Employee.listEntGroup.FindItemGroup(msg.Gid);
					if (group != null)
					{
						if (group != null)
						{
							result = group.IsShield;
							return result;
						}
					}
				}
			}
			result = false;
			return result;
		}
		private bool IsNotifyFlashing(Message msg)
		{
			bool result;
			if (msg != null)
			{
				if (msg.MessageObjectType == MessageActorType.EntGroup)
				{
					result = (this.dataService.GetEntGroup(msg.Gid) != null && (this.dataService.GetEntGroupChatTab(msg.Gid) == null || this.IsMinimizedState()));
					return result;
				}
				if (msg.MessageObjectType == MessageActorType.EntStaff)
				{
					if (!string.IsNullOrEmpty(msg.FromJid) && this.dataService.GetStaff((long)((ulong)Jid.GetUid(msg.FromJid))) != null && (this.dataService.GetStaffChatTab((long)((ulong)Jid.GetUid(msg.FromJid))) == null || this.IsMinimizedState()))
					{
						result = true;
						return result;
					}
				}
				if (msg.MessageObjectType == MessageActorType.Roster)
				{
					if (!string.IsNullOrEmpty(msg.FromJid) && this.dataService.GetRoster((long)((ulong)Jid.GetUid(msg.FromJid))) != null && (this.dataService.GetRosterChatTab((long)((ulong)Jid.GetUid(msg.FromJid))) == null || this.IsMinimizedState()))
					{
						result = true;
						return result;
					}
				}
				if (msg.MessageObjectType == MessageActorType.CooperationStaff)
				{
					if (!string.IsNullOrEmpty(msg.FromJid) && this.dataService.GetCooperationStaff((long)((ulong)Jid.GetUid(msg.FromJid)), msg.ProjectId) != null && (this.dataService.GetCooperationStaffChatTab((long)((ulong)Jid.GetUid(msg.FromJid)), msg.ProjectId) == null || this.IsMinimizedState()))
					{
						result = true;
						return result;
					}
				}
			}
			result = false;
			return result;
		}
		private bool IsMinimizedState()
		{
			return this.inWindow.WindowState == WindowState.Minimized;
		}
		private void RosterMessageEvent(RosterMessage response)
		{
			try
			{
				if (response != null)
				{
					Message msg = new Message();
					MessageStyle ms = new MessageStyle(response.style);
					msg.CreateTime = response.date;
					msg.FromJid = response.fromJID;
					msg.MessageBlocks = this.utilService.MessageDecode(response.message);
					msg.Style = ms;
					msg.VOType = response.type;
					msg.MessageObjectType = MessageActorType.Roster;
					if (this.IsNotifyFlashing(msg))
					{
						NotifyIconUtil.Instance.SetFlashIcon(FlashIconType.Roster);
						NotifyIconUtil.Instance.StartFlashing();
						this.dataModel.AddMessage((long)((ulong)Jid.GetUid(msg.FromJid)), MessageActorType.Roster, msg);
						if (this.IsAddMessageBox())
						{
							MessageBoxWindow mbw = this.dataModel.GetMessageBox();
							if (mbw == null)
							{
								mbw = new MessageBoxWindow();
								this.dataModel.SetMessageBox(mbw);
							}
							mbw.Refresh();
							mbw.Show();
						}
					}
					else
					{
						this.RosterAddMessage(msg);
					}
				}
			}
			catch (System.Exception ex)
			{
				System.Console.WriteLine(ex.ToString());
			}
		}
		private void RosterAddMessage(Message msg)
		{
			if (msg != null && !string.IsNullOrEmpty(msg.FromJid))
			{
				RosterTab item = this.dataService.GetRosterChatTab((long)((ulong)Jid.GetUid(msg.FromJid))) as RosterTab;
				if (item != null)
				{
					FriendsChatTabControl tab = item.TabContent;
					if (tab != null)
					{
						tab.ChatComponent.AddMessageRoster(msg, false);
					}
					INWindow inWindow = this.dataService.INWindow as INWindow;
					if (inWindow != null)
					{
						if (!this.IsCurrentItem(item))
						{
							TabItemHeaderControl header = item.Header as TabItemHeaderControl;
							if (header != null)
							{
								header.SetFlashingStyle();
							}
							this.TaskBarFlash();
						}
						else
						{
							if (!inWindow.IsActive)
							{
								this.TaskBarFlash();
							}
						}
					}
				}
			}
		}
		private void CooperationStaffAddMessage(Message msg)
		{
			CoopStaffTab item = this.dataService.GetCooperationStaffChatTab((long)((ulong)Jid.GetUid(msg.FromJid)), msg.ProjectId) as CoopStaffTab;
			if (item != null)
			{
				CoopStaffChatTabControl tab = item.TabContent;
				if (tab != null)
				{
					tab.ChatComponent.AddCooperationMessageStaff(msg, false);
				}
				INWindow inWindow = this.dataService.INWindow as INWindow;
				if (inWindow != null)
				{
					if (!this.IsCurrentItem(item))
					{
						TabItemHeaderControl header = item.Header as TabItemHeaderControl;
						if (header != null)
						{
							header.SetFlashingStyle();
						}
						this.TaskBarFlash();
					}
					else
					{
						if (!inWindow.IsActive)
						{
							this.TaskBarFlash();
						}
					}
				}
			}
		}
		private void StaffAddMessage(Message msg)
		{
			EntStaffTab item = this.dataService.GetStaffChatTab((long)((ulong)Jid.GetUid(msg.FromJid))) as EntStaffTab;
			if (item != null)
			{
				PersonalChatTabControl tab = item.TabContent;
				if (tab != null)
				{
					tab.ChatComponent.AddMessageStaff(msg, false);
				}
				INWindow inWindow = this.dataService.INWindow as INWindow;
				if (inWindow != null)
				{
					if (!this.IsCurrentItem(item))
					{
						TabItemHeaderControl header = item.Header as TabItemHeaderControl;
						if (header != null)
						{
							header.SetFlashingStyle();
						}
						this.TaskBarFlash();
					}
					else
					{
						if (!inWindow.IsActive)
						{
							this.TaskBarFlash();
						}
					}
				}
			}
		}
		private void TaskBarFlash()
		{
			INWindow inWindow = this.dataService.INWindow as INWindow;
			if (inWindow != null)
			{
				FlashWindowHelper taskBarHelper = new FlashWindowHelper();
				taskBarHelper.Flash(8, 400.0, inWindow);
			}
		}
		private bool IsAddMessageBox()
		{
			return Settings.Default.SystemSetup_MessageNotify_MessageBoxShow;
		}
		private void AutoReplyCooperationMessageProcessor(Message msg)
		{
			if (this.sessionService.Status == UserStatus.Away)
			{
				if (Settings.Default.SystemSetup_AutoReply_Is)
				{
					if (msg.VOType == 0)
					{
						string txt = Settings.Default.SystemSetup_AutoReply_Message[Settings.Default.SystemSetup_AutoReply_Index];
						this.SendCooperationMessageAutoMessageReply(this.sessionService.Jid, msg.FromJid, txt, "", 1, msg.ProjectId);
					}
				}
			}
		}
		private void AutoReplyProcessor(Message msg)
		{
			if (this.sessionService.Status == UserStatus.Away)
			{
				if (Settings.Default.SystemSetup_AutoReply_Is)
				{
					if (msg.VOType == 0)
					{
						string txt = Settings.Default.SystemSetup_AutoReply_Message[Settings.Default.SystemSetup_AutoReply_Index];
						this.SendAutoMessageReply(this.sessionService.Jid, msg.FromJid, txt, "", 1);
					}
				}
			}
		}
		public void SendCooperationMessageAutoMessageReply(string fromJid, string toJid, string message, string style, int type, string projectid)
		{
			if (message != null)
			{
				CooperationMessageRequest request = new CooperationMessageRequest();
				request.from_jid = fromJid;
				request.to_jid = toJid;
				request.message = message;
				request.style = style;
				request.type = type;
				request.project_id = projectid;
				this.connection.Send(PacketType.COOPERATION_MESSAGE, request);
			}
		}
		public void SendAutoMessageReply(string fromJid, string toJid, string message, string style, int type)
		{
			if (message != null)
			{
				MessageRequest request = new MessageRequest();
				request.from_jid = fromJid;
				request.to_jid = toJid;
				request.message = message;
				request.style = style;
				request.type = type;
				this.connection.Send(PacketType.ENT_MESSAGE, request);
			}
		}
		public bool ChangeStatus(IDKin.IM.Core.Staff staff)
		{
			bool result;
			try
			{
				if (staff != null)
				{
					StaffNoticeRequest request = new StaffNoticeRequest();
					request.signature = staff.Signature;
					request.jid = staff.Jid;
					request.name = staff.Name;
					request.status = (int)staff.Status;
					request.img = staff.HeaderFileName;
					this.connection = ServiceUtil.Instance.Connection;
					this.connection.Send(PacketType.STAFF_NOTICE, request);
				}
				result = true;
			}
			catch (System.Exception ex)
			{
				if (this.logger != null)
				{
					this.logger.Error(ex.ToString());
				}
				result = false;
			}
			return result;
		}
		public void Disconnect()
		{
			this.connection.Disconnect();
		}
		public void LogOff()
		{
			LogOutRequest request = new LogOutRequest();
			request.type = 1;
			this.connection.Send(PacketType.LOGOUT, request);
		}
		public void SendKeepLive()
		{
			if (this.sessionService.IsLogin)
			{
				if (this.keepliveTimer == null)
				{
					this.keepliveTimer = new System.Timers.Timer(10000.0);
					this.keepliveTimer.Elapsed += new ElapsedEventHandler(this.SendKeepliveEventHandler);
				}
				this.keepliveTimer.Start();
				System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(this.GetTicket), null);
			}
			else
			{
				if (this.keepliveTimer != null)
				{
					this.keepliveTimer.Stop();
				}
			}
		}
		private void SendKeepliveEventHandler(object sender, ElapsedEventArgs e)
		{
			if (this.sessionService.IsLogin)
			{
				KeepLiveRequest request = new KeepLiveRequest();
				request.jid = this.sessionService.Jid;
				this.connection.Send(PacketType.KEEPLIVE, request);
				if (!this.sessionService.TicketAvailable)
				{
					System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(this.GetTicket), null);
				}
			}
			else
			{
				System.Timers.Timer timer = sender as System.Timers.Timer;
				timer.Stop();
				timer.Close();
				timer.Dispose();
				this.keepliveTimer = null;
			}
		}
		private void LogOffDataEvent(LogOutResponse response)
		{
			try
			{
				if (this.inWindow != null)
				{
					switch (response.type)
					{
					case 0:
						this.inWindow.RELogOn(response.message);
						break;
					case 2:
						this.inWindow.RELogOn("您的账号在其它地方登陆，被迫下线!");
						break;
					}
				}
			}
			catch (System.Exception ex)
			{
				if (this.logger != null)
				{
					this.logger.Error(ex.ToString());
				}
			}
		}
		private void OACircularEvent(CircularResponse response)
		{
			if (response != null && response.circular != null && response.circular.Count > 0)
			{
				this.inWindow.workDesktopControl.OANoticeTabItem.ClearData();
				this.inWindow.workDesktopControl.OANoticeTabItem.AddOANotice(response.circular);
			}
		}
		private void OAPlanEvent(OAPlanResponse response)
		{
			if (response != null && response.plan != null && response.plan.Count > 0)
			{
				this.inWindow.workDesktopControl.OAPlanTabItem.ClearData();
				this.inWindow.workDesktopControl.OAPlanTabItem.AddOAPlan(response.plan);
			}
		}
		private void OAScheduleEvent(OAWorkFlowResponse response)
		{
			if (response != null && response.workFlow != null && response.workFlow.Count > 0)
			{
				this.inWindow.workDesktopControl.OAWorkflowTabItem.ClearData();
				this.inWindow.workDesktopControl.OAWorkflowTabItem.AddOASchedule(response.workFlow);
			}
		}
		private void OAMessageErrorEvent(OAMessageFailedResponse response)
		{
			switch (response.type)
			{
			case 14:
				this.inWindow.workDesktopControl.OAPlanTabItem.ClearData();
				break;
			case 15:
				this.inWindow.workDesktopControl.OAWorkflowTabItem.ClearData();
				break;
			case 16:
				this.inWindow.workDesktopControl.OANoticeTabItem.ClearData();
				break;
			default:
				if (this.oaMessageErrorCount < 5)
				{
					System.Console.WriteLine(string.Concat(new object[]
					{
						"OAMessageErrorEvent = ",
						response.errorMsg,
						" type = ",
						response.type
					}));
					this.oaMessageErrorCount++;
					if (string.IsNullOrEmpty(this.sessionService.Ticket))
					{
						this.GetTicket("");
					}
					this.GetOADataByType(response.type);
				}
				break;
			}
		}
		private void CommonEvent(CommonResponse response)
		{
			if (response != null)
			{
				EntStaffTab item = this.dataService.GetStaffChatTab((long)((ulong)Jid.GetUid(response.from_jid))) as EntStaffTab;
				if (item == null)
				{
					IDKin.IM.Core.Staff staff = this.dataService.GetStaff((long)((ulong)Jid.GetUid(response.from_jid)));
					if (staff != null)
					{
						item = new EntStaffTab(staff);
						item.SetDefaultStyle();
						((INWindow)this.dataService.INWindow).ContentTab.Items.Add(item);
						this.dataService.AddStaffChatTab(staff.Uid, item);
					}
				}
				if (item != null)
				{
					this.ActiveInWindow();
					this.PickUpMessageProcessor();
					MessageBoxWindow mbw = this.dataModel.GetMessageBox();
					if (mbw != null)
					{
						mbw.Refresh();
					}
					this.FlashIconPorcessor();
					IDKin.IM.Core.Staff staff = this.dataService.GetStaff((long)((ulong)Jid.GetUid(response.from_jid)));
					if (staff != null)
					{
						item.TabContent.ChatComponent.AddMessageNotice(staff.Name + "给你发送了一个震动!");
						this.SelectedTab(item);
						item.TabContent.ChatComponent.Vibration(true);
					}
				}
			}
		}
		private void CooperationCommonEvent(CooperationCommonResponse response)
		{
			if (response != null)
			{
				CoopStaffTab item = this.dataService.GetCooperationStaffChatTab((long)((ulong)Jid.GetUid(response.from_jid)), response.projectID) as CoopStaffTab;
				if (item == null)
				{
					CooperationStaff staff = this.dataService.GetCooperationStaff((long)((ulong)Jid.GetUid(response.from_jid)), response.projectID);
					CooperationProjectWrapper cooperationProjectWrapper = this.dataService.GetCooperationProjectWrapper(response.projectID);
					if (staff != null && cooperationProjectWrapper != null)
					{
						item = new CoopStaffTab(staff, cooperationProjectWrapper);
						item.SetDefaultStyle();
						((INWindow)this.dataService.INWindow).ContentTab.Items.Add(item);
						this.dataService.AddCooperationStaffChatTab(staff.Uid, staff.UnitedProjectid, item);
					}
				}
				if (item != null)
				{
					this.ActiveInWindow();
					this.PickUpMessageProcessor();
					MessageBoxWindow mbw = this.dataModel.GetMessageBox();
					if (mbw != null)
					{
						mbw.Refresh();
					}
					this.FlashIconPorcessor();
					CooperationStaff staff = this.dataService.GetCooperationStaff((long)((ulong)Jid.GetUid(response.from_jid)), response.projectID);
					if (staff != null)
					{
						item.TabContent.ChatComponent.AddMessageNotice(staff.Name + "给你发送了一个震动!");
						this.SelectedTab(item);
						item.TabContent.ChatComponent.Vibration(true);
					}
				}
			}
		}
		private void StaffInfoEvent(ProfileResponse response)
		{
			if (response != null)
			{
				IDKin.IM.Core.Staff staff = this.dataService.GetStaff(response.uid);
				if (staff != null)
				{
					staff.Name = response.name;
					staff.Signature = response.signature;
					staff.Age = response.age;
					staff.Birthday = response.birthday;
					staff.BirthdayType = response.birthdayType;
					staff.BloodType = response.bloodType;
					staff.City = response.city;
					staff.Constellation = response.constellation;
					staff.Country = response.country;
					staff.CreateTime = response.createTime.ToString();
					staff.DepartmentId = response.department_id;
					staff.Email = response.email;
					staff.HeaderFileName = response.img;
					staff.Job = response.job;
					staff.Level = response.level.ToString();
					staff.Mobile = response.mobile;
					staff.MyDescription = response.description;
					staff.MyHome = response.site;
					staff.Nickname = response.nickname;
					staff.Province = response.province;
					staff.School = response.school;
					if (response.sex == 0)
					{
						staff.Sex = Sex.Female;
					}
					else
					{
						if (response.sex == 1)
						{
							staff.Sex = Sex.Male;
						}
						else
						{
							staff.Sex = Sex.Unknow;
						}
					}
					staff.ShowScope = response.showScope;
					staff.Telephone = response.telephone;
					staff.Uid = response.uid;
					staff.UserName = response.username;
					staff.Zodiac = response.zodiac;
					if (this.sessionService.Uid == response.uid)
					{
						SelfProfilePage selpage = WindowModel.Instance.GetSelfProfilePage();
						if (selpage != null)
						{
							selpage.InitStafInfo(staff);
						}
					}
					else
					{
						UserProfilePage page = WindowModel.Instance.GetUserProfilePage(staff.Uid);
						page.InitStafInfo(staff);
					}
				}
				System.Collections.Generic.ICollection<CooperationStaff> cooperationStaffs = this.dataService.GetCooperationStaff(response.uid);
				if (cooperationStaffs != null)
				{
					foreach (CooperationStaff item in cooperationStaffs)
					{
						item.SynchronizeTo(response);
					}
				}
			}
		}
		private void GetTicket(object obj)
		{
			try
			{
				this.wsClient.Url = this.dataService.ServerInfo.WebSsoOGet;
				this.wsClient.AddParams("username", this.sessionService.UserName);
				this.wsClient.AddParams("password", this.sessionService.Password);
				this.wsClient.AddParams("ticket", this.sessionService.Ticket);
				this.sessionService.Ticket = this.wsClient.GetTicket();
			}
			catch (System.Exception e)
			{
				this.logger.Error("ticket" + e.ToString());
			}
		}
		private long[] StringToLong(string[] strs)
		{
			long[] result;
			if (strs != null)
			{
				int arrayLength = 0;
				for (int j = 0; j < strs.Length; j++)
				{
					string s = strs[j];
					if (!string.IsNullOrEmpty(s))
					{
						arrayLength++;
					}
				}
				long[] tmp = new long[arrayLength];
				for (int i = 0; i < arrayLength; i++)
				{
					tmp[i] = (long)((ulong)uint.Parse(strs[i]));
				}
				result = tmp;
			}
			else
			{
				result = null;
			}
			return result;
		}
		private System.Text.StringBuilder GetGroupMemberNames(long[] members)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			for (int i = 0; i < members.Length; i++)
			{
				uint uid = (uint)members[i];
				IDKin.IM.Core.Staff staff = this.dataService.GetStaff((long)((ulong)uid));
				if (staff != null)
				{
					sb.Append(staff.Name);
					sb.Append(",");
				}
			}
			return sb;
		}
		private void ActiveInWindow()
		{
			INWindow inWindow = this.dataService.INWindow as INWindow;
			if (inWindow != null)
			{
				if (inWindow.WindowState == WindowState.Maximized)
				{
					inWindow.WindowState = WindowState.Maximized;
				}
				else
				{
					inWindow.WindowState = WindowState.Normal;
				}
				inWindow.Activate();
				inWindow.Show();
			}
		}
		private void SelectedTab(BaseTab baseTab)
		{
			INWindow inWindow = this.dataService.INWindow as INWindow;
			if (baseTab != null && inWindow != null)
			{
				inWindow.ContentTab.SelectedItem = baseTab;
				baseTab.SetDefaultStyle();
			}
		}
		public void GetCustomGroup()
		{
			CustomGroupRequest request = new CustomGroupRequest();
			request.uid = this.sessionService.Uid;
			this.connection.Send(PacketType.GET_CUSTOM_GROUP_LIST, request);
		}
		private void GetCustomGroup(CustomGroupResponse response)
		{
			System.Collections.Generic.ICollection<IDKin.IM.Core.CustomGroup> groups = null;
			if (response != null && response.customGroup != null && response.customGroup.Count > 0)
			{
				groups = new System.Collections.Generic.List<IDKin.IM.Core.CustomGroup>();
				foreach (IDKin.IM.Protocol.Enterprise.CustomGroup item in response.customGroup)
				{
					if (item.admin == this.sessionService.Uid)
					{
						groups.Add(new IDKin.IM.Core.CustomGroup
						{
							Admin = item.admin,
							GroupID = item.id,
							GroupName = item.name,
							Members = this.GetMembers(item.members)
						});
					}
				}
				this.ShowGroups(groups);
			}
			if (this.inWindow.Employee.FirstTreeView.ContextMenu != null)
			{
				this.inWindow.Employee.FirstTreeView.ContextMenu.Visibility = Visibility.Visible;
			}
		}
		private System.Collections.Generic.ICollection<IDKin.IM.Core.Staff> GetMembers(string members)
		{
			System.Collections.Generic.ICollection<IDKin.IM.Core.Staff> result2;
			if (string.IsNullOrEmpty(members))
			{
				result2 = null;
			}
			else
			{
				string[] result = members.Split(new char[]
				{
					':'
				});
				System.Collections.Generic.List<IDKin.IM.Core.Staff> lists = new System.Collections.Generic.List<IDKin.IM.Core.Staff>(result.Length);
				string[] array = result;
				for (int i = 0; i < array.Length; i++)
				{
					string s = array[i];
					if (!string.IsNullOrEmpty(s))
					{
						try
						{
							IDKin.IM.Core.Staff staff = this.dataService.GetStaff((long)int.Parse(s));
							if (staff != null)
							{
								lists.Add(staff);
							}
						}
						catch (System.Exception ex)
						{
							this.logger.Error(ex.Message);
						}
					}
				}
				result2 = lists;
			}
			return result2;
		}
		private void ShowGroups(System.Collections.Generic.ICollection<IDKin.IM.Core.CustomGroup> groups)
		{
			if (groups != null && groups.Count > 0)
			{
				CustomGroupManagerWindowViewModel viewModel = CustomGroupManagerWindowViewModel.GetInstance();
				DataModel.Instance.CustomeGroupName.Clear();
				foreach (IDKin.IM.Core.CustomGroup group in groups)
				{
					CustomGroupTreeViewItem item = new CustomGroupTreeViewItem();
					item.Style = (this.inWindow.FindResource("TreeViewItemStyle") as Style);
					item.DataContext = group;
					item.HeaderText = group.GroupName;
					item.Tag = group.GroupID;
					item.CreateCustomItemEvent += delegate(object s, CustomGroupTreeViewItem.CustomEventArgs e)
					{
						if (e.Item != null)
						{
							this.inWindow.Employee.FirstTreeView.Items.Add(e.Item);
							IDKin.IM.Core.CustomGroup c = e.Item.DataContext as IDKin.IM.Core.CustomGroup;
							if (c != null)
							{
								DataModel.Instance.CustomeGroupName.Add(c.GroupID, c);
								viewModel.CreateCustomGroup(this.sessionService.Uid, c.GroupID, e.Item.HeaderText);
							}
						}
					};
					item.DeleteCustomItemEvent += delegate(object s, CustomGroupTreeViewItem.CustomEventArgs e)
					{
						DataModel.Instance.CustomeGroupName.Remove(item.Tag.ToString());
						this.inWindow.Employee.FirstTreeView.Items.Remove(item);
						viewModel.DropCustomGroup((int)this.sessionService.Uid, item.Tag.ToString());
					};
					item.UpdateCustomItemEvent += delegate(object s, CustomGroupTreeViewItem.CustomEventArgs e)
					{
						if (DataModel.Instance.CustomeGroupName.ContainsKey(item.Tag.ToString()))
						{
							DataModel.Instance.CustomeGroupName[item.Tag.ToString()].GroupName = item.HeaderText;
							viewModel.UpdateCustomGroup((int)this.sessionService.Uid, item.Tag.ToString(), item.HeaderText);
						}
						DataModel.Instance.CustomeGroupName[item.Tag.ToString()].GroupName = item.HeaderText;
						viewModel.UpdateCustomGroup((int)this.sessionService.Uid, item.Tag.ToString(), item.HeaderText);
					};
					this.AddChildren(group, item);
					this.dataService.CustomGroups.Add(group);
					this.inWindow.Employee.FirstTreeView.Items.Add(item);
					DataModel.Instance.CustomeGroupName.Add(group.GroupID, group);
				}
			}
		}
		private void AddChildren(IDKin.IM.Core.CustomGroup group, CustomGroupTreeViewItem item)
		{
			try
			{
				if (group != null && group.Members != null && group.Members.Count != 0)
				{
					IOrderedEnumerable<IDKin.IM.Core.Staff> v = 
						from t in @group.Members
						orderby t.Status == UserStatus.Offline, t.Status == UserStatus.Hide, t.Status == UserStatus.Out, t.Status == UserStatus.DoNotDisturb, t.Status == UserStatus.Busy, t.Status == UserStatus.Meeting, t.Status == UserStatus.Away, t.Status == UserStatus.Online
						select t;
					foreach (IDKin.IM.Core.Staff staff in v)
					{
						TreeNodeCustomStaff node = new TreeNodeCustomStaff(staff);
						node.SessionService = this.sessionService;
						node.DataService = this.dataService;
						node.ContextMenu = this.GetContextMenuForStaff(node);
						item.Items.Add(node);
					}
				}
			}
			catch (System.Exception ex)
			{
				System.Console.WriteLine(ex.ToString());
			}
		}
		private ContextMenu GetContextMenuForStaff(TreeNodeCustomStaff treeitem)
		{
			ContextMenu cMenu = new ContextMenu();
			MenuItem item = new MenuItem();
			item.Header = "将联系人从该组移除";
			cMenu.Items.Add(item);
			item.Click += delegate(object s, RoutedEventArgs e)
			{
				CustomGroupTreeViewItem t = treeitem.Parent as CustomGroupTreeViewItem;
				if (t != null)
				{
					IDKin.IM.Core.CustomGroup c = t.DataContext as IDKin.IM.Core.CustomGroup;
					if (this.dataModel.CustomeGroupName.ContainsKey(c.GroupID))
					{
						CustomGroupManagerWindowViewModel vmodel = CustomGroupManagerWindowViewModel.GetInstance();
						this.dataModel.CustomeGroupName[c.GroupID].Members.Remove(treeitem.Staff);
						vmodel.DeleteMemberToCustomGroup((int)this.sessionService.Uid, c.GroupID, this.GetMembers(this.dataModel.CustomeGroupName[c.GroupID]));
						t.Items.Remove(treeitem);
					}
				}
			};
			return cMenu;
		}
		private string GetMembers(IDKin.IM.Core.CustomGroup c)
		{
			string result = string.Empty;
			string result2;
			if (c == null || c.Members == null)
			{
				result2 = result;
			}
			else
			{
				foreach (IDKin.IM.Core.Staff item in c.Members)
				{
					result = result + item.Uid + ":";
				}
				result2 = result;
			}
			return result2;
		}
		private void GetEmailList(EmailTimeResponse response)
		{
			DataModel.Instance.EmailList.Clear();
			if (response != null && response.emailTime.Count > 0)
			{
				System.Collections.Generic.List<EMailModal> localModals = this.GetEmailModals();
				foreach (EmailTime emailTime in response.emailTime)
				{
					EMailModal modal = new EMailModal();
					modal.MailID = emailTime.email;
					modal.EMailType = (EmailType)emailTime.emailType;
					if (localModals != null && localModals.Count > 0)
					{
						EMailModal modalTmp = localModals.Find((EMailModal m) => m.MailID == modal.MailID);
						if (modalTmp != null)
						{
							modal.PWD = modalTmp.PWD;
							modal.Server = modalTmp.Server;
							modal.Text = modalTmp.Text;
							modal.NewCount = modalTmp.NewCount;
							modal.Url = modalTmp.Url;
							modal.UID = modalTmp.UID;
						}
					}
					int time = 0;
					if (int.TryParse(emailTime.spanTime, out time))
					{
						modal.Span = time;
					}
					else
					{
						modal.Span = 5;
					}
					modal.Url = emailTime.url;
					modal.Server = emailTime.host;
					System.DateTime dt;
					if (System.DateTime.TryParse(emailTime.lastTime, out dt))
					{
						modal.LastUpdateTime = dt;
					}
					else
					{
						this.logger.Error(string.Format("邮件帐户:{0} 邮件ID:{1} 时间信息异常:{2}", modal.UID, modal.MailID, emailTime.lastTime));
					}
					modal.HasError = string.IsNullOrEmpty(modal.PWD);
					DataModel.Instance.EmailList.Add(modal);
				}
				this.InitialEmail();
			}
		}
		private void InitialEmail()
		{
			this.inWindow.UpdateMailCount();
			if (DataModel.Instance.EmailList != null && DataModel.Instance.EmailList.Count > 0)
			{
				foreach (EMailModal mode in DataModel.Instance.EmailList)
				{
					if (!mode.HasError)
					{
						Setting s = new Setting(mode);
						s.GetNewMailCountLoop();
					}
				}
			}
		}
		private System.Collections.Generic.List<EMailModal> GetEmailModals()
		{
			System.Collections.Generic.List<EMailModal> result;
			if (this.sessionService == null || string.IsNullOrEmpty(this.sessionService.DirLocalData) || !System.IO.File.Exists(System.IO.Path.Combine(this.sessionService.DirLocalData, "email.email")))
			{
				result = null;
			}
			else
			{
				System.Collections.Generic.List<EMailModal> modals = new System.Collections.Generic.List<EMailModal>();
				using (System.IO.FileStream stream = new System.IO.FileStream(System.IO.Path.Combine(this.sessionService.DirLocalData, "email.email"), System.IO.FileMode.Open))
				{
					System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
					while (stream.Position < stream.Length)
					{
						modals.Add(formatter.Deserialize(stream) as EMailModal);
					}
				}
				result = modals;
			}
			return result;
		}
	}
}
