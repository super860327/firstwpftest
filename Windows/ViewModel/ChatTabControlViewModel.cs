using IDKin.IM.Communicate;
using IDKin.IM.Core;
using IDKin.IM.Data;
using IDKin.IM.Log;
using IDKin.IM.Protocol.Account;
using IDKin.IM.Protocol.Center;
using IDKin.IM.Protocol.Commons;
using IDKin.IM.Protocol.Cooperation;
using IDKin.IM.Protocol.Enterprise;
using IDKin.IM.Windows.Comps;
using IDKin.IM.Windows.Comps.ChatTab;
using IDKin.IM.Windows.Model;
using IDKin.IM.Windows.Util;
using IDKin.IM.Windows.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;
namespace IDKin.IM.Windows.ViewModel
{
	public class ChatTabControlViewModel : DispatcherObject
	{
		public delegate void EntMessageRecordDelegate(System.Collections.ObjectModel.Collection<MessageRecord> records);
		public delegate void EntGroupMessageRecordDelegate(System.Collections.ObjectModel.Collection<GroupMessageRecord> records);
		private const string SendFailFileMsg = "文件";
		private const string SendFailFileMsg2 = "发送失败!";
		private const string SendingFileMsg = "正在发送文件";
		private const string ReceivedFileMsg = "正在接收文件";
		private const string ReceivedFileRequestMsg = "收到【";
		private const string ReceivedFileRequestMsg2 = "】的文件请求";
		private const string ReceivedStaffFileRequestMsg = "收到文件请求";
		private const string ReceivedSuccessFileMsg = "成功接收";
		private const string ReceivedSuccessFileMsg2 = "的文件:";
		private const string ReceivedFailFileMsg = "的文件";
		private const string ReceivedFailFileMsg2 = "接收失败!";
		private const string RefuseFileMsg = "您已拒绝接收文件";
		private IConnection connection = ServiceUtil.Instance.Connection;
		private IUtilService utilService = ServiceUtil.Instance.utilService;
		private ISessionService sessionService = ServiceUtil.Instance.SessionService;
		private IDataService dataService = ServiceUtil.Instance.DataService;
		private ILogger logger = ServiceUtil.Instance.Logger;
		private MessageStyle ms = new MessageStyle();
		public void AddEventListenerHandler()
		{
			this.connection.EventHandler.EntMessageRecordEvent = new EntMessageRecordHandler(this.EntMessageRecordEvent);
			this.connection.EventHandler.EntGroupMessageRecordEvent = new EntGroupMessageRecordHandler(this.EntGroupMessageRecordEvent);
			this.connection.EventHandler.CooperationStaffMessageRecordEvent = new CooperationStaffMessageRecordHandler(this.CooperationStaffMessageRecordEvent);
		}
		public void SendVibration(string fromJid, string toJid)
		{
			if (ValidationUtil.Jid(fromJid) && ValidationUtil.Jid(toJid))
			{
				CommonRequest request = new CommonRequest();
				request.from_jid = fromJid;
				request.to_jid = toJid;
				request.type = 1;
				this.connection.Send(PacketType.COMMON, request);
			}
		}
		public void SendCooperationVibration(string fromJid, string toJid, string projectid)
		{
			if (ValidationUtil.Jid(fromJid) && ValidationUtil.Jid(toJid))
			{
				CooperationCommonRequest request = new CooperationCommonRequest();
				request.from_jid = fromJid;
				request.to_jid = toJid;
				request.projectID = projectid;
				request.type = 1;
				this.connection.Send(PacketType.COOPERATION_COMMON, request);
			}
		}
		public void SendCooperationMessageRequest(Message message)
		{
			if (message != null)
			{
				CooperationMessageRequest request = new CooperationMessageRequest();
				request.from_jid = message.FromJid;
				request.to_jid = message.ToJid;
				request.project_id = message.ProjectId;
				request.message = this.utilService.MessageEncode(message.MessageBlocks);
				request.style = message.Style.ToString();
				request.type = message.VOType;
				this.connection.Send(PacketType.COOPERATION_MESSAGE, request);
				this.logger.Debug(request.to_jid);
			}
		}
		public void SendMessageStaff(Message message)
		{
			if (message != null)
			{
				MessageRequest request = new MessageRequest();
				request.from_jid = message.FromJid;
				request.to_jid = message.ToJid;
				request.message = this.utilService.MessageEncode(message.MessageBlocks);
				request.style = message.Style.ToString();
				request.type = message.VOType;
				this.connection.Send(PacketType.ENT_MESSAGE, request);
			}
		}
		public void SendMessageRoster(Message message)
		{
			if (message != null)
			{
				RosterMessage request = new RosterMessage();
				request.fromJID = message.FromJid;
				request.toJID = message.ToJid;
				request.message = this.utilService.MessageEncode(message.MessageBlocks);
				request.style = message.Style.ToString();
				request.type = message.VOType;
				this.connection.Send(PacketType.ROSTER_MESSAGE, request);
			}
		}
		public void SendMessageGroup(Message message)
		{
			if (message != null)
			{
				GroupMessageRequest request = new GroupMessageRequest();
				request.from_jid = message.FromJid;
				request.gid = message.Gid;
				request.message = this.utilService.MessageEncode(message.MessageBlocks);
				request.style = message.Style.ToString();
				this.connection.Send(PacketType.ENT_GROUP_MESSAGE, request);
			}
		}
		public void SendStaffMessageRecord(long fromUid, long toUid, string startTime, int page, int limit, MessageRecordType type)
		{
			MessageRecordRequest request = new MessageRecordRequest();
			request.from_uid = fromUid;
			request.to_uid = toUid;
			request.startTime = startTime;
			request.page = page;
			request.limit = limit;
			request.entMessageRecordType = (int)type;
			this.connection.Send(PacketType.ENT_MESSAGE_RECORD, request);
		}
		public void SendCooperationMessageRecordRequest(long fromUid, long toUid, string projectid, string startTime, int page, int limit, MessageRecordType type)
		{
			CooperationMessageRecordRequest request = new CooperationMessageRecordRequest();
			request.from_uid = fromUid;
			request.project_id = projectid;
			request.to_uid = toUid;
			request.fromJid = this.sessionService.Jid;
			request.startTime = startTime;
			request.page = page;
			request.limit = limit;
			request.messageRecordType = (int)type;
			request.type = 0;
			this.connection.Send(PacketType.COOPERATION_MESSAGE_RECORD, request);
		}
		public void SendCooperationMessageRecordRequestByDate(long fromUid, long toUid, long projecid, string startTime, int page, int limit, string endTime, MessageRecordType type)
		{
			MessageRecordRequest request = new MessageRecordRequest();
			request.from_uid = fromUid;
			request.to_uid = toUid;
			request.startTime = startTime;
			request.page = page;
			request.endTime = endTime;
			request.limit = limit;
			request.entMessageRecordType = (int)type;
			request.type = 2;
			this.connection.Send(PacketType.ENT_MESSAGE_RECORD, request);
		}
		public void SendStaffDataSearchMessage(long fromUid, long toUid, string startTime, int page, int limit, string endTime, MessageRecordType type)
		{
			CooperationMessageRecordRequest request = new CooperationMessageRecordRequest();
			request.from_uid = fromUid;
			request.to_uid = toUid;
			request.startTime = startTime;
			request.page = page;
			request.endTime = endTime;
			request.limit = limit;
			this.connection.Send(PacketType.COOPERATION_MESSAGE_RECORD, request);
		}
		public void SendGroupMessageRecord(long gid, string time, int page, int limit, long fromID, MessageRecordType type)
		{
			GroupMessageRecordRequest request = new GroupMessageRecordRequest();
			request.gid = gid;
			request.startTime = time;
			request.page = page;
			request.limit = limit;
			request.from_uid = fromID;
			request.groupMessageRecordType = (int)type;
			this.connection.Send(PacketType.ENT_GROUP_MESSAGE_RECORD, request);
		}
		public void SendStaffSearchRecord(long fromUid, long toUid, string startTime, int page, int limit, string endTime, MessageRecordType type)
		{
			MessageRecordRequest request = new MessageRecordRequest();
			request.from_uid = fromUid;
			request.to_uid = toUid;
			request.startTime = startTime;
			request.page = page;
			request.limit = limit;
			request.endTime = endTime;
			request.entMessageRecordType = (int)type;
			request.type = 2;
			this.connection.Send(PacketType.ENT_MESSAGE_RECORD, request);
		}
		public void SendGroupSearchRecord(long fromgID, long gid, string startTime, int page, int limit, string endTime, MessageRecordType type)
		{
			GroupMessageRecordRequest request = new GroupMessageRecordRequest();
			request.gid = gid;
			request.from_uid = fromgID;
			request.startTime = startTime;
			request.page = page;
			request.limit = limit;
			request.endTime = endTime;
			request.groupMessageRecordType = (int)type;
			request.type = 2;
			this.connection.Send(PacketType.ENT_GROUP_MESSAGE_RECORD, request);
		}
		public void EntMessageRecordEvent(MessageRecordResponse records)
		{
			try
			{
				if (records.messageRecords != null && records.messageRecords.Count > 0)
				{
					if (records.entMessageRecordType == 4)
					{
						this.EntStaffMessageRecordMessageCenter(records.messageRecords);
					}
					else
					{
						if (records.entMessageRecordType == 3)
						{
							this.EntStaffMessageRecordChatPanel(records.messageRecords);
						}
					}
				}
				else
				{
					if (records.messageRecords.Count == 0)
					{
						if (records.entMessageRecordType == 4)
						{
							WindowModel.Instance.MsgRecordPage.ClearPage();
						}
						else
						{
							if (records.entMessageRecordType == 3)
							{
								INWindow inWindow = this.dataService.INWindow as INWindow;
								if (inWindow != null)
								{
									EntStaffTab pctc = inWindow.ContentTab.SelectedItem as EntStaffTab;
									EntGroupTab gctc = inWindow.ContentTab.SelectedItem as EntGroupTab;
									RosterTab rost = inWindow.ContentTab.SelectedItem as RosterTab;
									if (pctc != null)
									{
										pctc.TabContent.ChatComponent.MsgRecordComp.setShowPage();
									}
									if (gctc != null)
									{
										gctc.TabContent.ChatComponent.MsgRecordComp.setShowPage();
									}
									if (rost != null)
									{
										rost.TabContent.ChatComponent.MsgRecordComp.setShowPage();
									}
								}
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
		public void CooperationStaffMessageRecordEvent(CooperationMessageRecordResponse response)
		{
			try
			{
				if (response != null || response.cooperationMessageRecord.Count != 0)
				{
					this.DisplayCooperationMessageRecordToChatPanel(response.cooperationMessageRecord, response.total);
				}
				else
				{
					if (response.cooperationMessageRecord.Count == 0)
					{
						if (response.messageRecordType == 3)
						{
							INWindow inWindow = this.dataService.INWindow as INWindow;
							if (inWindow != null)
							{
								EntStaffTab pctc = inWindow.ContentTab.SelectedItem as EntStaffTab;
								EntGroupTab gctc = inWindow.ContentTab.SelectedItem as EntGroupTab;
								RosterTab rost = inWindow.ContentTab.SelectedItem as RosterTab;
								CoopStaffTab coopStaffTab = inWindow.ContentTab.SelectedItem as CoopStaffTab;
								if (pctc != null)
								{
									pctc.TabContent.ChatComponent.MsgRecordComp.setShowPage();
								}
								if (gctc != null)
								{
									gctc.TabContent.ChatComponent.MsgRecordComp.setShowPage();
								}
								if (rost != null)
								{
									rost.TabContent.ChatComponent.MsgRecordComp.setShowPage();
								}
								if (coopStaffTab != null)
								{
									coopStaffTab.TabContent.ChatComponent.MsgRecordComp.setShowPage();
								}
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
		private void EntStaffMessageRecordMessageCenter(System.Collections.Generic.List<MessageRecord> records)
		{
			try
			{
				if (records.Count > 0)
				{
					Message[] messages = new Message[records.Count];
					MessageStyle ms = null;
					for (int i = 0; i < messages.Length; i++)
					{
						ms = this.StaffMessageRecordProcessor(records, ms, i);
						messages[i] = new Message();
						messages[i].FromJid = records[i].from_uid + "@null/null";
						messages[i].CreateTime = records[i].createTime;
						messages[i].MessageBlocks = this.utilService.MessageDecode(records[i].message);
						messages[i].Style = ms;
						messages[i].RecordId = records[i].id;
						messages[i].IsMark = records[i].isMark;
						messages[i].Gid = records[i].from_uid;
						messages[i].FileName = records[i].fileName;
						messages[i].Icon = records[i].icon;
						messages[i].Url = records[i].url;
						if (this.sessionService.Uid == records[i].to_uid)
						{
							messages[i].ToJid = records[i].from_uid.ToString();
						}
						else
						{
							messages[i].ToJid = records[i].to_uid.ToString();
						}
						messages[i].Style = ms;
					}
					IDKin.IM.Core.Staff staff = this.dataService.GetStaff((long)((ulong)Jid.GetUid(messages[0].FromJid)));
					if (staff != null && WindowModel.Instance.IsOpenMessageCenterWindow())
					{
						WindowModel.Instance.MsgRecordPage.MessageCenterRecordStaff(messages, staff, records[0].total);
						System.GC.Collect();
					}
				}
				else
				{
					WindowModel.Instance.MsgRecordPage.ClearPage();
				}
			}
			catch (System.Exception e)
			{
				System.Console.WriteLine(e.ToString());
			}
		}
		private void SetParagraphStyle(Paragraph para, MessageStyle style)
		{
			if (para != null && style != null)
			{
				try
				{
					para.FontSize = (double)this.ms.FontSizeList[style.FontSize];
				}
				catch (System.IndexOutOfRangeException e)
				{
					ServiceUtil.Instance.Logger.Error(e.ToString());
					style.FontSize = 1;
					para.FontSize = 12.0;
				}
				try
				{
					para.FontFamily = new FontFamily(this.ms.FontFamilyList[style.FontFamily]);
				}
				catch (System.IndexOutOfRangeException e)
				{
					ServiceUtil.Instance.Logger.Error(e.ToString());
					style.FontFamily = 0;
					para.FontFamily = new FontFamily(this.ms.FontFamilyList[0]);
				}
				if (style.Bold == 1)
				{
					para.FontWeight = FontWeights.Bold;
				}
				else
				{
					para.FontWeight = FontWeights.Normal;
				}
				if (style.Italic == 1)
				{
					para.FontStyle = FontStyles.Italic;
				}
				else
				{
					para.FontStyle = FontStyles.Normal;
				}
				para.Foreground = new SolidColorBrush(Color.FromArgb(255, style.FontColorR, style.FontColorG, style.FontColorB));
			}
		}
		private void EntStaffMessageRecordChatPanel(System.Collections.Generic.List<MessageRecord> records)
		{
			long staffUid;
			if (records[0].from_uid != this.sessionService.Uid)
			{
				staffUid = records[0].from_uid;
			}
			else
			{
				staffUid = records[0].to_uid;
			}
			Message[] messages = new Message[records.Count];
			MessageStyle ms = null;
			for (int i = 0; i < messages.Length; i++)
			{
				ms = this.StaffMessageRecordProcessor(records, ms, i);
				messages[i] = new Message();
				messages[i].FromJid = records[i].from_uid + "@null/null";
				messages[i].CreateTime = records[i].createTime;
				messages[i].FileName = records[i].fileName;
				messages[i].Icon = records[i].icon;
				messages[i].Url = records[i].url;
				messages[i].toUid = records[i].to_uid;
				if (this.sessionService.Uid == records[i].to_uid)
				{
					messages[i].ToJid = records[i].from_uid.ToString();
				}
				else
				{
					messages[i].ToJid = records[i].to_uid.ToString();
				}
				messages[i].MessageBlocks = this.utilService.MessageDecode(records[i].message);
				messages[i].Style = ms;
			}
			EntStaffTab item = this.dataService.GetStaffChatTab(staffUid) as EntStaffTab;
			if (item != null)
			{
				PersonalChatTabControl tab = item.TabContent;
				if (tab != null)
				{
					tab.ChatComponent.AddMessageRecords(messages, records[0].total);
				}
			}
			System.GC.Collect();
		}
		private void DisplayCooperationMessageRecordToChatPanel(System.Collections.Generic.List<CooperationMessageRecord> records, int total)
		{
			string projectid = records[0].project_id;
			long staffUid;
			if (records[0].from_uid != this.sessionService.Uid)
			{
				staffUid = records[0].from_uid;
			}
			else
			{
				staffUid = records[0].to_uid;
			}
			Message[] messages = new Message[records.Count];
			MessageStyle ms = null;
			for (int i = 0; i < messages.Length; i++)
			{
				ms = this.CooperationMessageRecordProcessor(records, ms, i);
				messages[i] = new Message();
				messages[i].FromJid = records[i].from_uid + "@null/null";
				messages[i].CreateTime = records[i].createTime;
				messages[i].FileName = records[i].fileName;
				messages[i].Icon = records[i].icon;
				messages[i].Url = records[i].url;
				messages[i].toUid = records[i].to_uid;
				messages[i].ProjectId = records[i].project_id;
				if (this.sessionService.Uid == records[i].to_uid)
				{
					messages[i].ToJid = records[i].from_uid.ToString();
				}
				else
				{
					messages[i].ToJid = records[i].to_uid.ToString();
				}
				messages[i].MessageBlocks = this.utilService.MessageDecode(records[i].message);
				messages[i].Style = ms;
			}
			CoopStaffTab item = this.dataService.GetCooperationStaffChatTab(staffUid, projectid) as CoopStaffTab;
			if (item != null)
			{
				CoopStaffChatTabControl tab = item.TabContent;
				if (tab != null)
				{
					tab.ChatComponent.AddCooperationStaffMessageRecords(messages, total);
				}
			}
		}
		public MessageStyle StaffMessageRecordProcessor(System.Collections.Generic.List<MessageRecord> records, MessageStyle ms, int i)
		{
			switch (records[i].type)
			{
			case 0:
				ms = new MessageStyle(records[i].style);
				break;
			case 1:
				ms = new MessageStyle(records[i].style);
				break;
			case 2:
				ms = this.StaffEndFileDownloadMessageRecord(records, ms, i);
				break;
			case 3:
				ms = this.StaffErrorFileUpLoadMessageRecord(records, ms, i);
				break;
			case 4:
				ms = this.StaffErrorFileDownloadMessageRecord(records, ms, i);
				break;
			case 5:
				ms = this.StaffReJectFileDownloadMessagRecord(records, ms, i);
				break;
			case 6:
				ms = this.StaffEndFileUpLoadMessageRecord(records, ms, i);
				break;
			}
			if (records[i].type == 0)
			{
				ms = new MessageStyle(records[i].style);
			}
			return ms;
		}
		public MessageStyle CooperationMessageRecordProcessor(System.Collections.Generic.List<CooperationMessageRecord> records, MessageStyle ms, int i)
		{
			switch (records[i].type)
			{
			case 0:
				ms = new MessageStyle(records[i].style);
				break;
			case 1:
				ms = new MessageStyle(records[i].style);
				break;
			case 2:
				ms = this.CooperationStaffEndFileDownloadMessageRecord(records, ms, i);
				break;
			case 3:
				ms = this.CooperationStaffErrorFileUpLoadMessageRecord(records, ms, i);
				break;
			case 4:
				ms = this.CooperationStaffErrorFileDownloadMessageRecord(records, ms, i);
				break;
			case 5:
				ms = this.CooperationStaffReJectFileDownloadMessagRecord(records, ms, i);
				break;
			case 6:
				ms = this.CooperationStaffEndFileUpLoadMessageRecord(records, ms, i);
				break;
			}
			if (records[i].type == 0)
			{
				ms = new MessageStyle(records[i].style);
			}
			return ms;
		}
		private MessageStyle CooperationStaffEndFileDownloadMessageRecord(System.Collections.Generic.List<CooperationMessageRecord> records, MessageStyle ms, int i)
		{
			if (records[i].from_uid == this.sessionService.Uid)
			{
				string staffEndFileDownload = "成功接收文件" + records[i].message;
				records[i].message = "[{T:" + staffEndFileDownload + "}]";
			}
			else
			{
				string staffEndFileDownload = "成功接收文件" + records[i].message;
				IDKin.IM.Core.Staff staff = this.dataService.GetStaff(records[i].from_uid);
				if (staff != null)
				{
					records[i].message = "[{T:" + staffEndFileDownload + "}]";
				}
			}
			records[i].style = "0;0;0;255#255#0#0;1";
			return new MessageStyle(records[i].style);
		}
		private MessageStyle StaffEndFileDownloadMessageRecord(System.Collections.Generic.List<MessageRecord> records, MessageStyle ms, int i)
		{
			if (records[i].from_uid == this.sessionService.Uid)
			{
				string staffEndFileDownload = "成功接收文件" + records[i].message;
				records[i].message = "[{T:" + staffEndFileDownload + "}]";
			}
			else
			{
				string staffEndFileDownload = "成功接收文件" + records[i].message;
				IDKin.IM.Core.Staff staff = this.dataService.GetStaff(records[i].from_uid);
				if (staff != null)
				{
					records[i].message = "[{T:" + staffEndFileDownload + "}]";
				}
			}
			records[i].style = "0;0;0;255#255#0#0;1";
			return new MessageStyle(records[i].style);
		}
		private MessageStyle CooperationStaffErrorFileDownloadMessageRecord(System.Collections.Generic.List<CooperationMessageRecord> records, MessageStyle ms, int i)
		{
			if (records[i].from_uid == this.sessionService.Uid)
			{
				string staffErrorFileDownload = "文件" + records[i].message + "接收失败";
				records[i].message = "[{T:" + staffErrorFileDownload + "}]";
			}
			else
			{
				string staffErrorFileDownload = "接收文件" + records[i].message + "失败";
				IDKin.IM.Core.Staff staff = this.dataService.GetStaff(records[i].from_uid);
				if (staff != null)
				{
					records[i].message = "[{T:" + staffErrorFileDownload + "}]";
				}
			}
			records[i].style = "0;0;0;255#255#0#0;1";
			return new MessageStyle(records[i].style);
		}
		private MessageStyle StaffErrorFileDownloadMessageRecord(System.Collections.Generic.List<MessageRecord> records, MessageStyle ms, int i)
		{
			if (records[i].from_uid == this.sessionService.Uid)
			{
				string staffErrorFileDownload = "文件" + records[i].message + "接收失败";
				records[i].message = "[{T:" + staffErrorFileDownload + "}]";
			}
			else
			{
				string staffErrorFileDownload = "接收文件" + records[i].message + "失败";
				IDKin.IM.Core.Staff staff = this.dataService.GetStaff(records[i].from_uid);
				if (staff != null)
				{
					records[i].message = "[{T:" + staffErrorFileDownload + "}]";
				}
			}
			records[i].style = "0;0;0;255#255#0#0;1";
			return new MessageStyle(records[i].style);
		}
		private MessageStyle CooperationStaffErrorFileUpLoadMessageRecord(System.Collections.Generic.List<CooperationMessageRecord> records, MessageStyle ms, int i)
		{
			if (records[i].from_uid == this.sessionService.Uid)
			{
				records[i].message = "[{T:" + records[i].message + "}]";
			}
			records[i].style = "0;0;0;255#255#0#0;1";
			return new MessageStyle(records[i].style);
		}
		private MessageStyle StaffErrorFileUpLoadMessageRecord(System.Collections.Generic.List<MessageRecord> records, MessageStyle ms, int i)
		{
			if (records[i].from_uid == this.sessionService.Uid)
			{
				records[i].message = "[{T:" + records[i].message + "}]";
			}
			records[i].style = "0;0;0;255#255#0#0;1";
			return new MessageStyle(records[i].style);
		}
		private MessageStyle CooperationStaffEndFileUpLoadMessageRecord(System.Collections.Generic.List<CooperationMessageRecord> records, MessageStyle ms, int i)
		{
			if (records[i].from_uid == this.sessionService.Uid)
			{
				records[i].message = "[{T:" + records[i].message + "}]";
			}
			records[i].style = "0;0;0;255#255#0#0;1";
			return new MessageStyle(records[i].style);
		}
		private MessageStyle StaffEndFileUpLoadMessageRecord(System.Collections.Generic.List<MessageRecord> records, MessageStyle ms, int i)
		{
			if (records[i].from_uid == this.sessionService.Uid)
			{
				records[i].message = "[{T:" + records[i].message + "}]";
			}
			records[i].style = "0;0;0;255#255#0#0;1";
			return new MessageStyle(records[i].style);
		}
		private MessageStyle CooperationStaffReJectFileDownloadMessagRecord(System.Collections.Generic.List<CooperationMessageRecord> records, MessageStyle ms, int i)
		{
			if (records[i].from_uid == this.sessionService.Uid)
			{
				string staffReJectFileDownload = "您已拒绝接收文件" + records[i].message;
				records[i].message = "[{T:" + staffReJectFileDownload + "}]";
			}
			else
			{
				string staffReJectFileDownload = "已拒绝接收您的文件" + records[i].message;
				IDKin.IM.Core.Staff staff = this.dataService.GetStaff(records[i].from_uid);
				if (staff != null)
				{
					records[i].message = "[{T:" + staffReJectFileDownload + "}]";
				}
			}
			records[i].style = "0;0;0;255#255#0#0;1";
			return new MessageStyle(records[i].style);
		}
		private MessageStyle StaffReJectFileDownloadMessagRecord(System.Collections.Generic.List<MessageRecord> records, MessageStyle ms, int i)
		{
			if (records[i].from_uid == this.sessionService.Uid)
			{
				string staffReJectFileDownload = "您已拒绝接收文件" + records[i].message;
				records[i].message = "[{T:" + staffReJectFileDownload + "}]";
			}
			else
			{
				string staffReJectFileDownload = "已拒绝接收您的文件" + records[i].message;
				IDKin.IM.Core.Staff staff = this.dataService.GetStaff(records[i].from_uid);
				if (staff != null)
				{
					records[i].message = "[{T:" + staffReJectFileDownload + "}]";
				}
			}
			records[i].style = "0;0;0;255#255#0#0;1";
			return new MessageStyle(records[i].style);
		}
		private void EntMessageRecordEventHandle(System.Collections.ObjectModel.Collection<MessageRecord> records)
		{
		}
		private void EntGroupMessageRecordEvent(GroupMessageRecordResponse response)
		{
			try
			{
				if (response.GroupMessageRecord != null)
				{
					System.Collections.Generic.List<GroupMessageRecord> records = response.GroupMessageRecord;
					Message[] messages = new Message[response.GroupMessageRecord.Count];
					MessageStyle ms = null;
					for (int i = 0; i < messages.Length; i++)
					{
						ms = this.GroupMessageRecordProcessor(records, ms, i);
						messages[i] = new Message();
						messages[i].FromJid = records[i].from_uid + "@null/null";
						messages[i].CreateTime = records[i].createTime;
						messages[i].ToJid = records[i].gid.ToString();
						messages[i].MessageBlocks = this.utilService.MessageDecode(records[i].message);
						messages[i].Style = ms;
						messages[i].RecordId = records[i].id;
						messages[i].IsMark = records[i].isMark;
						messages[i].FileName = records[i].fileName;
						messages[i].Icon = records[i].icon;
						messages[i].Url = records[i].url;
						messages[i].Style = ms;
					}
					if (records.Count != 0 && response.groupMessageRecordType == 4)
					{
						if (WindowModel.Instance.IsOpenMessageCenterWindow())
						{
							EntGroup group = this.dataService.GetEntGroup(long.Parse(messages[0].ToJid));
							WindowModel.Instance.MsgRecordPage.MessageCenterRecordGroup(messages, group, records[0].total);
							System.GC.Collect();
						}
					}
					else
					{
						if (records.Count != 0 && response.groupMessageRecordType == 3)
						{
							EntGroupTab item = this.dataService.GetEntGroupChatTab(records[0].gid) as EntGroupTab;
							if (item != null)
							{
								GroupChatTabControl tab = item.TabContent;
								if (tab != null)
								{
									tab.ChatComponent.AddGroupMessageRecords(messages, records[0].total);
									System.GC.Collect();
								}
							}
						}
						else
						{
							if (records.Count == 0)
							{
								if (response.GroupMessageRecord.Count == 0 && response.groupMessageRecordType == 4)
								{
									WindowModel.Instance.MsgRecordPage.ClearPage();
								}
								else
								{
									if (response.GroupMessageRecord.Count == 0 && response.groupMessageRecordType == 3)
									{
										INWindow inWindow = this.dataService.INWindow as INWindow;
										if (inWindow != null)
										{
											EntGroupTab gctc = inWindow.ContentTab.SelectedItem as EntGroupTab;
											if (gctc != null)
											{
												gctc.TabContent.ChatComponent.MsgRecordComp.setShowPage();
											}
										}
									}
								}
							}
						}
					}
				}
			}
			catch (System.Exception e)
			{
				this.logger.Error(e.ToString());
			}
		}
		private MessageStyle GroupMessageRecordProcessor(System.Collections.Generic.List<GroupMessageRecord> records, MessageStyle ms, int i)
		{
			switch (records[i].type)
			{
			case 0:
				ms = new MessageStyle(records[i].style);
				break;
			case 2:
				records[i].message = "[{T:" + records[i].message + "}]";
				ms = this.SetRedColorStyle(records, ms, i);
				break;
			case 3:
				if (records[i].from_uid == this.sessionService.Uid)
				{
					records[i].message = "[{T:" + records[i].message + "}]";
				}
				ms = this.SetRedColorStyle(records, ms, i);
				break;
			case 4:
				records[i].message = "[{T:" + records[i].message + "}]";
				ms = this.SetRedColorStyle(records, ms, i);
				break;
			case 5:
				records[i].message = "[{T:" + records[i].message + "}]";
				ms = this.SetRedColorStyle(records, ms, i);
				break;
			case 6:
				if (records[i].from_uid == this.sessionService.Uid)
				{
					records[i].message = "[{T:" + records[i].message + "}]";
				}
				ms = this.SetRedColorStyle(records, ms, i);
				break;
			}
			return ms;
		}
		private MessageStyle SetRedColorStyle(System.Collections.Generic.List<GroupMessageRecord> records, MessageStyle ms, int i)
		{
			records[i].style = "0;0;0;255#255#0#0;1";
			return new MessageStyle(records[i].style);
		}
		private void EntGroupMessageRecordEventHandle(System.Collections.ObjectModel.Collection<GroupMessageRecord> records)
		{
		}
	}
}
