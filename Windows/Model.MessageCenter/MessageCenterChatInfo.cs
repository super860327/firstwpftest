using IDKin.IM.Core;
using System;
namespace IDKin.IM.Windows.Model.MessageCenter
{
	public class MessageCenterChatInfo
	{
		public long Id
		{
			get;
			set;
		}
		public string Name
		{
			get;
			set;
		}
		public MessageCenterType Type
		{
			get;
			set;
		}
		public string LastTime
		{
			get;
			set;
		}
	}
}
