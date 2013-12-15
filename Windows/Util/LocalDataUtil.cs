using IDKin.IM.Core;
using IDKin.IM.Data;
using IDKin.IM.Log;
using IDKin.IM.Protocol.LocalData;
using IDKin.IM.Windows.Model.MessageCenter;
using IDKin.IM.Windows.Model.RecentLink;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
namespace IDKin.IM.Windows.Util
{
	public class LocalDataUtil
	{
		private ILogger logger = ServiceUtil.Instance.Logger;
		private ISessionService sessionService = ServiceUtil.Instance.SessionService;
		private static LocalDataUtil instance = null;
		public static LocalDataUtil Instance
		{
			get
			{
				if (LocalDataUtil.instance == null)
				{
					LocalDataUtil.instance = new LocalDataUtil();
				}
				return LocalDataUtil.instance;
			}
		}
		private LocalDataUtil()
		{
		}
		public void AddChatInfo(MessageCenterChatInfo messageCenterInfo)
		{
			if (messageCenterInfo != null)
			{
				ChatInfo chatInfo = new ChatInfo();
				chatInfo.name = messageCenterInfo.Name;
				chatInfo.lasttime = messageCenterInfo.LastTime;
				chatInfo.id = messageCenterInfo.Id;
				chatInfo.type = (int)messageCenterInfo.Type;
				ChatInfoList list = this.GetLocalChatInfoList();
				if (list != null && list.chatinfo != null)
				{
					ChatInfo info = this.FindChatInfoItem(list, chatInfo.id, chatInfo.type);
					if (info == null)
					{
						list.chatinfo.Add(chatInfo);
					}
					else
					{
						info.lasttime = chatInfo.lasttime;
						info.name = chatInfo.name;
					}
				}
				else
				{
					list = new ChatInfoList();
					list.chatinfo.Add(chatInfo);
				}
				this.SaveLocalChatInfoList(list);
			}
		}
		private ChatInfo FindChatInfoItem(ChatInfoList list, long id, int type)
		{
			ChatInfo result;
			foreach (ChatInfo info in list.chatinfo)
			{
				if (info != null)
				{
					if (info.type == type && info.id == id)
					{
						result = info;
						return result;
					}
				}
			}
			result = null;
			return result;
		}
		public void SetAllChatInfo(System.Collections.Generic.List<MessageCenterChatInfo> list)
		{
			if (list != null)
			{
				ChatInfoList chatInfoList = new ChatInfoList();
				foreach (MessageCenterChatInfo info in list)
				{
					if (info != null)
					{
						ChatInfo chatInfo = new ChatInfo();
						chatInfo.id = info.Id;
						chatInfo.name = info.Name;
						chatInfo.lasttime = info.LastTime;
						chatInfo.type = (int)info.Type;
						chatInfoList.chatinfo.Add(chatInfo);
					}
				}
				this.SaveLocalChatInfoList(chatInfoList);
			}
		}
		public System.Collections.Generic.List<MessageCenterChatInfo> GetAllChatInfoBySort()
		{
			System.Collections.Generic.List<MessageCenterChatInfo> result;
			try
			{
				System.Collections.Generic.List<MessageCenterChatInfo> messageChatInfoList = null;
				ChatInfoList chatInfolist = this.GetLocalChatInfoList();
				if (chatInfolist != null && chatInfolist.chatinfo != null)
				{
					messageChatInfoList = new System.Collections.Generic.List<MessageCenterChatInfo>();
					foreach (ChatInfo chatInfo in chatInfolist.chatinfo)
					{
						if (chatInfo != null)
						{
							messageChatInfoList.Add(new MessageCenterChatInfo
							{
								Id = chatInfo.id,
								Name = chatInfo.name,
								LastTime = chatInfo.lasttime,
								Type = (MessageCenterType)System.Enum.Parse(typeof(MessageCenterType), chatInfo.type.ToString())
							});
						}
					}
					messageChatInfoList.Sort(new System.Comparison<MessageCenterChatInfo>(this.CompareChatInfoValue));
					result = messageChatInfoList;
					return result;
				}
			}
			catch (System.Exception e)
			{
				ServiceUtil.Instance.Logger.Error(e.ToString());
			}
			result = null;
			return result;
		}
		public int CompareRecentLinkValue(IDKin.IM.Windows.Model.RecentLink.RecentLinkInfo infoX, IDKin.IM.Windows.Model.RecentLink.RecentLinkInfo infoY)
		{
			string x = infoX.RecentTime;
			string y = infoY.RecentTime;
			int result;
			if (x == null)
			{
				if (y == null)
				{
					result = 0;
				}
				else
				{
					result = -1;
				}
			}
			else
			{
				if (y == null)
				{
					result = 1;
				}
				else
				{
					int retval = x.Length.CompareTo(y.Length);
					if (retval != 0)
					{
						result = retval;
					}
					else
					{
						result = x.CompareTo(y);
					}
				}
			}
			return result;
		}
		private int CompareChatInfoValue(MessageCenterChatInfo infoX, MessageCenterChatInfo infoY)
		{
			string x = infoX.LastTime;
			string y = infoY.LastTime;
			int result;
			if (x == null)
			{
				if (y == null)
				{
					result = 0;
				}
				else
				{
					result = -1;
				}
			}
			else
			{
				if (y == null)
				{
					result = 1;
				}
				else
				{
					int retval = x.Length.CompareTo(y.Length);
					if (retval != 0)
					{
						result = retval;
					}
					else
					{
						result = x.CompareTo(y);
					}
				}
			}
			return result;
		}
		private RecentLinkInfoList GetLocalRecentLinkInfoList()
		{
			System.IO.FileStream fs = null;
			RecentLinkInfoList result;
			try
			{
				string filePath = this.sessionService.DirLocalData + "\\recentlinklist.properties";
				if (System.IO.File.Exists(filePath))
				{
					fs = new System.IO.FileStream(filePath, System.IO.FileMode.Open);
					RecentLinkInfoList recentLinkList = Serializer.Deserialize<RecentLinkInfoList>(fs);
					fs.Close();
					result = recentLinkList;
					return result;
				}
			}
			catch (System.Exception ex)
			{
				if (fs != null)
				{
					fs.Close();
				}
				this.logger.Error("LocalDataUtil = " + ex.ToString());
			}
			result = null;
			return result;
		}
		private ChatInfoList GetLocalChatInfoList()
		{
			System.IO.FileStream fs = null;
			ChatInfoList result;
			try
			{
				string filePath = this.sessionService.DirLocalData + "\\messagecenter.properties";
				if (System.IO.File.Exists(filePath))
				{
					fs = new System.IO.FileStream(filePath, System.IO.FileMode.Open);
					ChatInfoList chatInfoList = Serializer.Deserialize<ChatInfoList>(fs);
					fs.Close();
					result = chatInfoList;
					return result;
				}
			}
			catch (System.Exception ex)
			{
				if (fs != null)
				{
					fs.Close();
				}
				this.logger.Error("LocalDataUtil = " + ex.ToString());
			}
			result = null;
			return result;
		}
		private void SaveLocalChatInfoList(ChatInfoList list)
		{
			if (list != null)
			{
				System.IO.MemoryStream ms = new System.IO.MemoryStream();
				System.IO.FileStream fs = null;
				this.SerializeChatInfoList(ms, list);
				ms.Position = 0L;
				if (this.sessionService == null)
				{
					this.sessionService = ServiceUtil.Instance.SessionService;
				}
				string filePath = this.sessionService.DirLocalData + "\\messagecenter.properties";
				try
				{
					if (System.IO.File.Exists(filePath))
					{
						System.IO.File.Delete(filePath);
					}
					fs = new System.IO.FileStream(filePath, System.IO.FileMode.Create);
					ms.WriteTo(fs);
					ms.Flush();
					fs.Flush();
				}
				catch (System.Exception ex)
				{
					this.logger.Error("LocalDataUtil = " + ex.ToString());
				}
				finally
				{
					if (ms != null)
					{
						ms.Close();
					}
					if (fs != null)
					{
						fs.Close();
					}
				}
			}
		}
		private void SerializeChatInfoList(System.IO.MemoryStream ms, object vo)
		{
			Serializer.Serialize<ChatInfoList>(ms, (ChatInfoList)vo);
		}
		private void SerializeRecentLink(System.IO.MemoryStream ms, object vo)
		{
			Serializer.Serialize<RecentLinkInfoList>(ms, (RecentLinkInfoList)vo);
		}
	}
}
