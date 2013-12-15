using IDKin.IM.Core;
using System;
using System.Collections.Generic;
using System.IO;
namespace IDKin.IM.Windows.View.EmailAlert
{
	public abstract class POP3
	{
		public string POPServer
		{
			get;
			set;
		}
		public string User
		{
			get;
			set;
		}
		public string Pwd
		{
			get;
			set;
		}
		public System.IO.StreamReader sr
		{
			get;
			set;
		}
		public EMailModal EMailModal
		{
			get;
			set;
		}
		public abstract string TestConnect();
		public abstract void Disconnect();
		public abstract int GetMailCount(System.DateTime dateTime);
		public abstract System.DateTime GetLastestDateTime();
		public abstract void GetAll();
		public abstract System.Collections.Generic.List<string> GetAllNew();
		public abstract string[] ReadLocal();
	}
}
