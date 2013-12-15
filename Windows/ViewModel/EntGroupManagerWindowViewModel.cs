using IDKin.IM.Communicate;
using IDKin.IM.Protocol.Commons;
using IDKin.IM.Protocol.Enterprise;
using IDKin.IM.Windows.Util;
using System;
namespace IDKin.IM.Windows.ViewModel
{
	public class EntGroupManagerWindowViewModel
	{
		private IConnection connection = ServiceUtil.Instance.Connection;
		public void AdminRemove(long gid, string member)
		{
			GroupOperationRequsest request = new GroupOperationRequsest();
			request.gid = gid;
			request.member = member;
			request.type = 8;
			this.connection.Send(PacketType.ENT_GROUP_OPERATION, request);
		}
		public void AddAdmin(long gid, string member)
		{
			GroupOperationRequsest request = new GroupOperationRequsest();
			request.gid = gid;
			request.member = member;
			request.type = 7;
			this.connection.Send(PacketType.ENT_GROUP_OPERATION, request);
		}
		public void Create(long uid, string name, string description, string member)
		{
			GroupOperationRequsest request = new GroupOperationRequsest();
			request.uid = uid;
			request.name = name;
			request.description = description;
			request.member = member;
			request.type = 1;
			this.connection.Send(PacketType.ENT_GROUP_OPERATION, request);
		}
		public void Update(long gid, string name, string description, string member)
		{
			GroupOperationRequsest request = new GroupOperationRequsest();
			request.gid = gid;
			request.name = name;
			request.description = description;
			request.type = 2;
			request.member = member;
			this.connection.Send(PacketType.ENT_GROUP_OPERATION, request);
		}
		public void MemberRemove(long gid, string member)
		{
			GroupOperationRequsest request = new GroupOperationRequsest();
			request.gid = gid;
			request.member = member;
			request.type = 6;
			this.connection.Send(PacketType.ENT_GROUP_OPERATION, request);
		}
		public void AddMember(long gid, string member)
		{
			GroupOperationRequsest request = new GroupOperationRequsest();
			request.gid = gid;
			request.member = member;
			request.type = 5;
			this.connection.Send(PacketType.ENT_GROUP_OPERATION, request);
		}
	}
}
