using IDKin.IM.Communicate;
using IDKin.IM.Data;
using IDKin.IM.Protocol.Account;
using IDKin.IM.Protocol.Commons;
using IDKin.IM.Protocol.Enterprise;
using IDKin.IM.Windows.Model;
using IDKin.IM.Windows.Util;
using System;
using System.Collections.Generic;
namespace IDKin.IM.Windows.ViewModel
{
	public class MessageCenterViewModel
	{
		private IConnection connection = ServiceUtil.Instance.Connection;
		private IUtilService utilService = ServiceUtil.Instance.utilService;
		private ISessionService sessionService = ServiceUtil.Instance.SessionService;
		private IDataService dataService = ServiceUtil.Instance.DataService;
		public System.Collections.Generic.List<NoticeRecord> listAll = new System.Collections.Generic.List<NoticeRecord>();
		public MessageCenterViewModel()
		{
			this.AddEventListenerHandler();
		}
		public void AddEventListenerHandler()
		{
			this.connection.EventHandler.GetMarkListEvent = new GetMarkListHandler(this.GetMarkListEvent);
			this.connection.EventHandler.NoticeRecordEvent = new NoticeRecordHandler(this.NoticeRecordEvent);
		}
		public void SendOANoticeRequest(long fromUid, string startTime, int page, int limit, OAModuleType type)
		{
			NoticeRecordRequest request = new NoticeRecordRequest();
			request.uid = fromUid;
			request.limit = limit;
			request.page = page;
			request.startTime = startTime;
			request.type = (int)type;
			this.connection.Send(PacketType.NOTICE_RECORD, request);
		}
		public void SendOADataSearch(long fromUid, string startTime, int page, int limit, OAModuleType type, string endTime)
		{
			NoticeRecordRequest request = new NoticeRecordRequest();
			request.uid = fromUid;
			request.limit = limit;
			request.page = page;
			request.startTime = startTime;
			request.type = (int)type;
			request.endTime = endTime;
			request.ByTime = true;
			this.connection.Send(PacketType.NOTICE_RECORD, request);
		}
		public void SendStaffMessageAddMark(long recordId, bool isMark, IsMarkType type, long fromID, long toID)
		{
			MessageMarkRequest request = new MessageMarkRequest();
			request.id = recordId;
			request.isMark = isMark;
			request.type = (int)type;
			request.from_uid = fromID;
			request.to_uid = toID;
			this.connection.Send(PacketType.ENT_MESSAGE_MARK, request);
		}
		public void SendGroupMessageAddMark(long recordId, bool isMark, IsMarkType type, long fromID, long toID)
		{
			MessageMarkRequest request = new MessageMarkRequest();
			request.id = recordId;
			request.isMark = isMark;
			request.type = (int)type;
			request.from_uid = fromID;
			request.gid = toID;
			this.connection.Send(PacketType.ENT_MESSAGE_MARK, request);
		}
		public void SendStaffMessageRecordMark(long fromUid, long toUid, string startTime, int page, int limit, MessageRecordType type)
		{
			MessageRecordRequest request = new MessageRecordRequest();
			request.from_uid = fromUid;
			request.to_uid = toUid;
			request.startTime = startTime;
			request.page = page;
			request.limit = limit;
			request.entMessageRecordType = (int)type;
			request.type = 1;
			this.connection.Send(PacketType.ENT_MESSAGE_RECORD, request);
		}
		public void SendStaffDateSearchMark(long fromUid, long toUid, string startTime, int page, int limit, string endtime, MessageRecordType type)
		{
			MessageRecordRequest request = new MessageRecordRequest();
			request.from_uid = fromUid;
			request.to_uid = toUid;
			request.startTime = startTime;
			request.page = page;
			request.limit = limit;
			request.endTime = endtime;
			request.entMessageRecordType = (int)type;
			request.type = 1;
			this.connection.Send(PacketType.ENT_MESSAGE_RECORD, request);
		}
		public void SendGroupMessageRecordMark(long Gid, string starttime, int page, int limit, long fromID, MessageRecordType type)
		{
			GroupMessageRecordRequest request = new GroupMessageRecordRequest();
			request.gid = Gid;
			request.startTime = starttime;
			request.page = page;
			request.limit = limit;
			request.from_uid = fromID;
			request.groupMessageRecordType = (int)type;
			request.type = 1;
			this.connection.Send(PacketType.ENT_GROUP_MESSAGE_RECORD, request);
		}
		public void SendGroupDateSearchMark(long Gid, string starttime, int page, int limit, long fromID, string endtime, MessageRecordType type)
		{
			GroupMessageRecordRequest request = new GroupMessageRecordRequest();
			request.gid = Gid;
			request.startTime = starttime;
			request.page = page;
			request.limit = limit;
			request.from_uid = fromID;
			request.endTime = endtime;
			request.groupMessageRecordType = (int)type;
			request.type = 1;
			this.connection.Send(PacketType.ENT_GROUP_MESSAGE_RECORD, request);
		}
		public void SendMarkByType(long fromUid, IsMarkType type)
		{
			GetIsMarkReuqest request = new GetIsMarkReuqest();
			request.uid = fromUid;
			request.type = (int)type;
			this.connection.Send(PacketType.GET_ISMARK_LIST, request);
		}
		public void SendGroupMarkByType(System.Collections.Generic.List<long> list, IsMarkType type, long uid)
		{
			GetIsMarkReuqest request = new GetIsMarkReuqest();
			foreach (long lg in list)
			{
				request.gid.Add(lg);
			}
			request.type = (int)type;
			request.uid = uid;
			this.connection.Send(PacketType.GET_ISMARK_LIST, request);
		}
		private void GetMarkListEvent(GetIsMarkResponse response)
		{
			if (response != null)
			{
				switch (response.type)
				{
				case 1:
					WindowModel.Instance.Markpage.ShowStaffList(response.getIsMark);
					WindowModel.Instance.MarkTreeView.AddMarkList(response.getIsMark);
					break;
				case 2:
					WindowModel.Instance.Markpage.ShowGroupList(response.getIsMark);
					WindowModel.Instance.MarkTreeView.AddGroupList(response.getIsMark);
					break;
				}
			}
		}
		private void NoticeRecordEvent(NoticeRecordResponse response)
		{
			if (response.noticeRecord.Count != 0)
			{
				WindowModel.Instance.OARecordPage.SetShowPage(response);
			}
			else
			{
				MessageCenterViewModel.SetNoRecord();
			}
			if (response.noticeRecord.Count != 0)
			{
				switch (response.type)
				{
				case 0:
					WindowModel.Instance.OATreeView.OAtree(response.noticeRecord, OAModuleType.OA_GETALL_RECORD);
					break;
				case 5:
					WindowModel.Instance.OATreeView.OAtree(response.noticeRecord, OAModuleType.OA_WORKFLOW_RECORD_TYPE);
					break;
				case 6:
					WindowModel.Instance.OATreeView.OAtree(response.noticeRecord, OAModuleType.OA_PROMANAGER_RECORD_TYPE);
					break;
				case 7:
					WindowModel.Instance.OATreeView.OAtree(response.noticeRecord, OAModuleType.OA_NOTICE_RECORD_TYPE);
					break;
				case 8:
					WindowModel.Instance.OATreeView.OAtree(response.noticeRecord, OAModuleType.OA_SYSTEM_RECORD_TYPE);
					break;
				case 9:
					WindowModel.Instance.OATreeView.OAtree(response.noticeRecord, OAModuleType.OA_PLAN_RECORD_TYPE);
					break;
				case 10:
					WindowModel.Instance.OATreeView.OAtree(response.noticeRecord, OAModuleType.OA_DOC_RECORD_TYPE);
					break;
				case 11:
					WindowModel.Instance.OATreeView.OAtree(response.noticeRecord, OAModuleType.OA_DISCUSS_RECORD_TYPE);
					break;
				case 12:
					WindowModel.Instance.OATreeView.OAtree(response.noticeRecord, OAModuleType.OA_APPROVE_RECORD_TYPE);
					break;
				}
			}
		}
		private static void SetNoRecord()
		{
			WindowModel.Instance.OARecordPage.NoRecordInit();
		}
	}
}
