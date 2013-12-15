using IDKin.IM.Communicate;
using IDKin.IM.Protocol.Commons;
using IDKin.IM.Protocol.Enterprise;
using IDKin.IM.Windows.Util;
using System;
namespace IDKin.IM.Windows.ViewModel
{
	public class ItemGroupViewModel
	{
		private IConnection connection = null;
		public ItemGroupViewModel()
		{
			this.InitService();
		}
		private void InitService()
		{
			this.connection = ServiceUtil.Instance.Connection;
		}
		public void Exit(long uid, long gid)
		{
			GroupOperationRequsest request = new GroupOperationRequsest();
			request.gid = gid;
			request.member = uid + ":";
			request.uid = uid;
			request.type = 9;
			this.connection.Send(PacketType.ENT_GROUP_OPERATION, request);
		}
		public void Delete(long gid)
		{
			GroupOperationRequsest request = new GroupOperationRequsest();
			request.gid = gid;
			request.type = 3;
			this.connection.Send(PacketType.ENT_GROUP_OPERATION, request);
		}
	}
}
