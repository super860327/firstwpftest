using IDKin.IM.Communicate;
using IDKin.IM.Protocol.Commons;
using IDKin.IM.Protocol.Enterprise;
using IDKin.IM.Windows.Util;
using System;
namespace IDKin.IM.Windows.ViewModel
{
	public class CustomGroupManagerWindowViewModel
	{
		public class MemberToCustomGroupEventArgs
		{
			public string GroupId
			{
				get;
				set;
			}
			public string Uid
			{
				get;
				set;
			}
		}
		public delegate void MemberToCustomGroup(object sender, CustomGroupManagerWindowViewModel.MemberToCustomGroupEventArgs args);
		private IConnection connection;
		public static object obj = new object();
		private static CustomGroupManagerWindowViewModel Instance;
		public event CustomGroupManagerWindowViewModel.MemberToCustomGroup AddMemberToCustomGroupEvent;
		private CustomGroupManagerWindowViewModel()
		{
			this.connection = ServiceUtil.Instance.Connection;
		}
		public static CustomGroupManagerWindowViewModel GetInstance()
		{
			if (CustomGroupManagerWindowViewModel.Instance == null)
			{
				lock (CustomGroupManagerWindowViewModel.obj)
				{
					if (CustomGroupManagerWindowViewModel.Instance == null)
					{
						CustomGroupManagerWindowViewModel.Instance = new CustomGroupManagerWindowViewModel();
					}
				}
			}
			return CustomGroupManagerWindowViewModel.Instance;
		}
		public void CreateCustomGroup(long uid, string id, string name)
		{
			CustomGroupOperationRequest request = new CustomGroupOperationRequest();
			request.from_uid = (int)uid;
			request.id = id;
			request.members = string.Empty;
			request.name = name;
			request.operationType = 4;
			this.connection.Send(PacketType.CUSTOM_GROUP_OPERATION, request);
		}
		public void DropCustomGroup(int f_uid, string id)
		{
			CustomGroupOperationRequest request = new CustomGroupOperationRequest();
			request.from_uid = f_uid;
			request.id = id;
			request.operationType = 5;
			this.connection.Send(PacketType.CUSTOM_GROUP_OPERATION, request);
		}
		public void UpdateCustomGroup(int f_uid, string id, string name)
		{
			CustomGroupOperationRequest request = new CustomGroupOperationRequest();
			request.from_uid = f_uid;
			request.id = id;
			request.name = name;
			request.operationType = 3;
			this.connection.Send(PacketType.CUSTOM_GROUP_OPERATION, request);
		}
		public void AddMemberToCustomGroup(int f_uid, string id, string members, string staffId)
		{
			CustomGroupOperationRequest request = new CustomGroupOperationRequest();
			request.from_uid = f_uid;
			request.id = id;
			request.members = members;
			request.operationType = 1;
			this.connection.Send(PacketType.CUSTOM_GROUP_OPERATION, request);
			if (this.AddMemberToCustomGroupEvent != null)
			{
				this.AddMemberToCustomGroupEvent(this, new CustomGroupManagerWindowViewModel.MemberToCustomGroupEventArgs
				{
					GroupId = id,
					Uid = members
				});
			}
		}
		public void DeleteMemberToCustomGroup(int f_uid, string id, string members)
		{
			CustomGroupOperationRequest request = new CustomGroupOperationRequest();
			request.from_uid = f_uid;
			request.id = id;
			request.members = members;
			request.operationType = 2;
			this.connection.Send(PacketType.CUSTOM_GROUP_OPERATION, request);
		}
	}
}
