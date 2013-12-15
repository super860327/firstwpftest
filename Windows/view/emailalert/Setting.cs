using IDKin.IM.Core;
using IDKin.IM.Windows.Model;
using IDKin.IM.Windows.Util;
using System;
using System.Collections.Generic;
using System.Threading;
namespace IDKin.IM.Windows.View.EmailAlert
{
	public class Setting
	{
		private static System.Collections.Generic.List<EMailModal> emailList = DataModel.Instance.EmailList;
		private System.Threading.Thread thread;
		private INWindow iNWindow;
		public EMailModal Modal
		{
			get;
			set;
		}
		public Setting(EMailModal modal)
		{
			this.Modal = modal;
			this.iNWindow = (ServiceUtil.Instance.DataService.INWindow as INWindow);
		}
		public void GetNewMailCountLoop()
		{
			if (this.Modal != null)
			{
				this.thread = new System.Threading.Thread(new System.Threading.ThreadStart(this.GetNewMailCount));
				this.thread.IsBackground = true;
				this.thread.Start();
			}
		}
		public void GetNewMailCount()
		{
			if (this.Modal != null && !string.IsNullOrEmpty(this.Modal.Server) && !string.IsNullOrEmpty(this.Modal.MailID) && this.Modal.MailID.Contains("@"))
			{
				try
				{
					POP3 pop3 = PopMailFactory.GetPopMail(this.Modal);
					while (true)
					{
						if (!ServiceUtil.Instance.SessionService.IsLogin)
						{
							System.Threading.Thread.CurrentThread.Abort();
						}
						if (this.Modal != null && pop3 != null && !string.IsNullOrEmpty(this.Modal.Server) && !string.IsNullOrEmpty(this.Modal.MailID) && Setting.emailList.Contains(this.Modal))
						{
							try
							{
								int count = pop3.GetMailCount(this.Modal.LastUpdateTime);
								if (count >= 0)
								{
									this.Modal.NewCount = count;
									this.iNWindow.UpdateMailCount();
								}
							}
							catch (System.Exception ex)
							{
								ServiceUtil.Instance.Logger.Error(ex.ToString());
							}
							System.Threading.Thread.Sleep(this.Modal.Span * 1000 * 60);
						}
						else
						{
							System.Threading.Thread.CurrentThread.Abort();
						}
					}
				}
				catch (System.Threading.ThreadAbortException)
				{
				}
				catch (System.Exception ex)
				{
					ServiceUtil.Instance.Logger.Error(ex.ToString());
				}
			}
		}
	}
}
