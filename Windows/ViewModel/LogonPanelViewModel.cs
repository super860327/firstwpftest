using IDKin.IM.Communicate;
using IDKin.IM.Core;
using IDKin.IM.Data;
using IDKin.IM.ImageService;
using IDKin.IM.Log;
using IDKin.IM.Protocol.Account;
using IDKin.IM.Protocol.Commons;
using IDKin.IM.Protocol.Enterprise;
using IDKin.IM.Windows.BindingVO;
using IDKin.IM.Windows.Util;
using IDKin.IM.Windows.View;
using System;
using System.Windows.Threading;
namespace IDKin.IM.Windows.ViewModel
{
	public class LogonPanelViewModel : DispatcherObject
	{
		public LogOnMsg logOnMsg = new LogOnMsg();
		private IWSClient wsClient
		{
			get;
			set;
		}
		private ILogger logger
		{
			get;
			set;
		}
		private IConnection connection
		{
			get;
			set;
		}
		private IDataService dataService
		{
			get;
			set;
		}
		private ISessionService sessionService
		{
			get;
			set;
		}
		private IImageService imageService
		{
			get;
			set;
		}
		private IFileService fileService
		{
			get;
			set;
		}
		public LogonPanelViewModel()
		{
			this.connection = ServiceUtil.Instance.Connection;
			this.dataService = ServiceUtil.Instance.DataService;
			this.logger = ServiceUtil.Instance.Logger;
			this.sessionService = ServiceUtil.Instance.SessionService;
			this.wsClient = ServiceUtil.Instance.WsClient;
			this.imageService = ServiceUtil.Instance.ImageService;
			this.AddEventListenerHandler();
		}
		private void AddEventListenerHandler()
		{
			if (this.connection != null && this.connection.EventHandler != null)
			{
				this.connection.EventHandler.LogOnEvent = new LogOnHandler(this.LogOnEventHandle);
			}
		}
		public ServerInfo GetServerInfo(string server, int port)
		{
			ServerInfo result;
			try
			{
				this.wsClient.Url = string.Concat(new object[]
				{
					"http://",
					server,
					":",
					port,
					"?action=login"
				});
				this.wsClient.AddParams("action", "login");
				ServerInfo si = this.wsClient.GetServerInfo();
				result = si;
				return result;
			}
			catch (System.Exception e)
			{
				this.logger.Error(e.ToString());
			}
			result = null;
			return result;
		}
		public bool ConnectServer(ServerInfo serverInfo)
		{
			bool result;
			try
			{
				this.dataService.ServerInfo = serverInfo;
				result = this.connection.StartConnect(serverInfo.Host, serverInfo.Port);
				return result;
			}
			catch (System.Exception e)
			{
				this.logger.Error(e.ToString());
			}
			result = false;
			return result;
		}
		public void Login(ServerInfo serverInfo, string username, string password, int status)
		{
			try
			{
				LogOnReuqest request = new LogOnReuqest();
				request.username = username;
				request.password = password;
				request.status = status;
				request.server = serverInfo.Name;
				request.resource = "pc";
				request.version = AppUtil.Instance.AppVersion();
				this.connection.Send(PacketType.LOG_ON, request);
			}
			catch (System.Exception e)
			{
				this.logger.Error(e.ToString());
			}
		}
		private void LogOnEventHandle(LogOnResponse response)
		{
			try
			{
				if (response != null)
				{
					switch (response.code)
					{
					case 0:
					{
						LogonWindow logonWindow = this.dataService.LoginWindow as LogonWindow;
						if (logonWindow != null)
						{
							logonWindow.LogonPanel.LogonButtonEnable();
						}
						this.logOnMsg.Message = "验证失败，用户名不存在！";
						this.connection.Disconnect();
						break;
					}
					case 1:
						this.logOnMsg.Message = "验证成功，正在获取数据...";
						this.InitUserInfo(response);
						this.dataService.ServerInfo.AESKey = System.Convert.FromBase64String(response.key);
						this.EnterINWindow();
						break;
					case 2:
					{
						LogonWindow logonWindow2 = this.dataService.LoginWindow as LogonWindow;
						if (logonWindow2 != null)
						{
							logonWindow2.LogonPanel.LogonButtonEnable();
						}
						this.logOnMsg.Message = "验证失败，密码错误！";
						this.connection.Disconnect();
						break;
					}
					}
				}
			}
			catch (System.Exception e)
			{
				this.logger.Error(e.ToString());
			}
		}
		private void EnterINWindow()
		{
			if (this.dataService.LoginWindow != null)
			{
				this.dataService.LoginWindow.Close();
				this.dataService.RemoveWindow(DataService.WindowType.Login);
			}
			this.dataService.INWindow = new INWindow();
			if (this.dataService.INWindow != null)
			{
				INWindow inWindow = this.dataService.INWindow as INWindow;
				if (inWindow != null)
				{
					inWindow.Show();
					LogonWindow logonWindow2 = this.dataService.LoginWindow as LogonWindow;
					if (logonWindow2 != null)
					{
						logonWindow2.LogonPanel.StopAnimation();
					}
					inWindow.InitData();
					this.sessionService.IsEnable = true;
					SystemWindow sysWindow = this.dataService.SystemWindow as SystemWindow;
					sysWindow.optionItem.Enabled = this.sessionService.IsEnable;
					sysWindow.aboutItem.Enabled = this.sessionService.IsEnable;
					sysWindow.logoutItem.Enabled = this.sessionService.IsEnable;
				}
			}
		}
		private void InitUserInfo(LogOnResponse response)
		{
			if (response != null)
			{
				this.sessionService.DepartmentId = response.department_id;
				this.sessionService.Uid = response.uid;
				this.sessionService.Jid = response.jid;
				this.sessionService.Name = response.name;
				this.sessionService.NickName = response.nickname;
				this.sessionService.Actor = response.level;
				this.sessionService.ServerTimeStamp = response.serverTimeStamp;
				this.sessionService.HeaderFileName = response.img;
				this.sessionService.Sex = (Sex)System.Enum.Parse(typeof(Sex), response.sex.ToString());
				this.sessionService.Signature = response.signature;
				this.sessionService.Status = (UserStatus)System.Enum.Parse(typeof(UserStatus), response.status.ToString());
				Logger.Jid = response.jid;
			}
		}
		private void GetStaffList(long departmentId)
		{
			StaffListRequest request = new StaffListRequest();
			request.department_id = departmentId;
			request.uid = this.sessionService.Uid;
			this.connection.Send(PacketType.STAFF_LIST, request);
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
	}
}
