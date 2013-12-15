using IDKin.IM.Communicate;
using IDKin.IM.Data;
using IDKin.IM.Protocol.Account;
using IDKin.IM.Protocol.Commons;
using IDKin.IM.Protocol.Cooperation;
using IDKin.IM.Protocol.Enterprise;
using IDKin.IM.Windows.Util;
using System;
namespace IDKin.IM.Windows.ViewModel
{
	public class NewFileListItemViewModel
	{
		private ISessionService sessionService = null;
		private IDataService dataService = null;
		private IConnection connection = null;
		public NewFileListItemViewModel()
		{
			this.InitService();
		}
		private void InitService()
		{
			this.sessionService = ServiceUtil.Instance.SessionService;
			this.dataService = ServiceUtil.Instance.DataService;
			this.connection = ServiceUtil.Instance.Connection;
		}
		public void SendGroupUpDownFile(string fromJid, long gid, string message, string style, int type)
		{
			GroupMessageRequest request = new GroupMessageRequest();
			request.from_jid = fromJid;
			request.gid = gid;
			request.message = message;
			request.style = style;
			request.type = type;
			this.connection.Send(PacketType.ENT_GROUP_MESSAGE, request);
		}
		public void SendGroupDownLoadFile(string fromJid, long gid, string message, string style, int type, string filename, string icon, string urL)
		{
			GroupMessageRequest request = new GroupMessageRequest();
			request.from_jid = fromJid;
			request.gid = gid;
			request.message = message;
			request.style = style;
			request.type = type;
			request.fileName = filename;
			request.url = urL;
			request.icon = icon;
			this.connection.Send(PacketType.ENT_GROUP_MESSAGE, request);
		}
		public void SendStaffUpDownNotice(string fromJid, string toJid, string message, string style, int type)
		{
			MessageRequest request = new MessageRequest();
			request.from_jid = fromJid;
			request.to_jid = toJid;
			request.message = message;
			request.style = style;
			request.type = type;
			this.connection.Send(PacketType.ENT_MESSAGE, request);
		}
		public void SendCooperationStaffUpDownNotice(string fromJid, string toJid, string projectid, string message, string style, int type)
		{
			CooperationMessageRequest request = new CooperationMessageRequest();
			request.from_jid = fromJid;
			request.to_jid = toJid;
			request.project_id = projectid;
			request.message = message;
			request.style = style;
			request.type = type;
			this.connection.Send(PacketType.COOPERATION_MESSAGE, request);
		}
		public void SendStaffDownLoadNotice(string fromJid, string toJid, string message, string style, int type, string icon, string filename, string url)
		{
			MessageRequest request = new MessageRequest();
			request.from_jid = fromJid;
			request.to_jid = toJid;
			request.message = message;
			request.style = style;
			request.type = type;
			request.icon = icon;
			request.fileName = filename;
			request.url = url;
			this.connection.Send(PacketType.ENT_MESSAGE, request);
		}
		public void SendCooperationStaffDownLoadNotice(string fromJid, string toJid, string projectid, string message, string style, int type, string icon, string filename, string url)
		{
			CooperationMessageRequest request = new CooperationMessageRequest();
			request.from_jid = fromJid;
			request.to_jid = toJid;
			request.project_id = projectid;
			request.message = message;
			request.style = style;
			request.type = type;
			request.icon = icon;
			request.fileName = filename;
			request.url = url;
			this.connection.Send(PacketType.COOPERATION_MESSAGE, request);
		}
		public void SendCooperationFileNoticeRequest(long fromUid, long toUid, string toJid, string projectid, string fileID, string iconString, string fileName, long fileSize)
		{
			CooperationFileNoticeRequest request = new CooperationFileNoticeRequest();
			request.from_uid = fromUid;
			request.to_uid = toUid;
			request.to_jid = toJid;
			request.projectId = projectid;
			request.fileID = fileID;
			request.iconString = iconString;
			request.fileName = fileName;
			request.fileSize = fileSize;
			this.connection.Send(PacketType.COOPERATION_NOTICE_FILE, request);
		}
		public void SendOffLineFile(long fromUid, long toUid, string fileID, string iconString, string fileName, long fileSize)
		{
			OffLineFileUploadRequest request = new OffLineFileUploadRequest();
			request.from_uid = fromUid;
			request.to_uid = toUid;
			request.fileID = fileID;
			request.iconString = iconString;
			request.fileName = fileName;
			request.fileSize = fileSize;
			this.connection.Send(PacketType.OFFLINE_FILE_UPLOAD, request);
		}
		public void SendGroupFile(long fromUid, long gid, string fileID, string iconString, string name)
		{
			GroupFileUploadRequest request = new GroupFileUploadRequest();
			request.fileID = fileID;
			request.from_uid = fromUid;
			request.gid = gid;
			request.iconString = iconString;
			request.filename = name;
			this.connection.Send(PacketType.ENT_GROUP_FILE_UPLOAD, request);
		}
	}
}
