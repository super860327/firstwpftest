using IDKin.IM.Communicate;
using IDKin.IM.Protocol.Commons;
using IDKin.IM.Protocol.Enterprise;
using IDKin.IM.Windows.Util;
using System;
namespace IDKin.IM.Windows.ViewModel
{
	public class EmailManagerViewModel
	{
		private IConnection connection;
		public EmailManagerViewModel()
		{
			this.connection = ServiceUtil.Instance.Connection;
		}
		public void AddEmail(long uid, string id, int mailType, string strHost, string url, string lasttime, string spantime)
		{
			EmailTimeRequest request = new EmailTimeRequest();
			request.email = id;
			request.uid = uid;
			request.spanTime = spantime;
			request.lastTime = lasttime;
			request.emailType = mailType;
			request.url = url;
			request.host = strHost;
			request.type = 1;
			this.connection.Send(PacketType.EMAIL_TIME, request);
		}
		public void DeleteEmail(long uid, string id)
		{
			EmailTimeRequest request = new EmailTimeRequest();
			request.email = id;
			request.uid = uid;
			request.type = 3;
			this.connection.Send(PacketType.EMAIL_TIME, request);
		}
		public void UpdateEmail(long uid, string id, int mailType, string spantime, string lastTime)
		{
			EmailTimeRequest request = new EmailTimeRequest();
			request.email = id;
			request.uid = uid;
			request.spanTime = spantime;
			request.emailType = mailType;
			request.lastTime = lastTime;
			request.type = 2;
			this.connection.Send(PacketType.EMAIL_TIME, request);
		}
	}
}
