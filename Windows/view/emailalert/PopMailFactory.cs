using IDKin.IM.Core;
using System;
namespace IDKin.IM.Windows.View.EmailAlert
{
	public static class PopMailFactory
	{
		public static POP3 GetPopMail(EMailModal modal)
		{
			string text = modal.Text;
			POP3 pop;
			if (text != null)
			{
				if (text == "126.com" || text == "sunupcg.com" || text == "jbcidu.com" || text == "yahoo.com.cn")
				{
					pop = new POP3Normal(modal);
					return pop;
				}
				if (text == "gmail.com")
				{
					pop = new POP3Ssl(modal);
					return pop;
				}
			}
			pop = new POP3Normal(modal);
			return pop;
		}
	}
}
