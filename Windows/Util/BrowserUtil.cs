using IDKin.IM.Communicate;
using IDKin.IM.Data;
using IDKin.IM.Log;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows;
namespace IDKin.IM.Windows.Util
{
	public class BrowserUtil
	{
		private static ILogger logger = ServiceUtil.Instance.Logger;
		public static void OpenNoticBrowserHandler(string str)
		{
			try
			{
				ISessionService sessionService = ServiceUtil.Instance.SessionService;
				IDataService dataService = ServiceUtil.Instance.DataService;
				if (sessionService.TicketAvailable)
				{
					string param = "ck=" + sessionService.Ticket + "&rd=" + str;
					string url = dataService.ServerInfo.WebSsoRequest + System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(param));
					BrowserUtil.OpenSupportSite(url);
				}
				else
				{
					System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(BrowserUtil.GetTicketNotic), str);
				}
			}
			catch
			{
			}
		}
		private static void GetTicketNotic(object obj)
		{
			try
			{
				IWSClient wsClient = ServiceUtil.Instance.WsClient;
				IDataService dataService = ServiceUtil.Instance.DataService;
				ISessionService sessionService = ServiceUtil.Instance.SessionService;
				System.Threading.Monitor.Enter(BrowserUtil.logger);
				for (int i = 0; i < 1; i++)
				{
					wsClient.Url = dataService.ServerInfo.WebSsoOGet;
					wsClient.AddParams("username", sessionService.UserName);
					wsClient.AddParams("password", sessionService.Password);
					wsClient.AddParams("ticket", sessionService.Ticket);
					sessionService.Ticket = wsClient.GetTicket();
					if (!string.IsNullOrEmpty(sessionService.Ticket))
					{
						break;
					}
				}
				if (sessionService.TicketAvailable && obj != null)
				{
					string param = "ck=" + sessionService.Ticket + "&rd=" + obj.ToString();
					string url = dataService.ServerInfo.WebSsoRequest + System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(param));
					BrowserUtil.OpenSupportSite(url);
				}
				else
				{
					MessageBox.Show("无法取到Ticket!");
				}
				System.Threading.Monitor.Exit(BrowserUtil.logger);
			}
			catch (System.Exception)
			{
			}
		}
		public static void OpenBrowserHandler(string str)
		{
			try
			{
				ISessionService sessionService = ServiceUtil.Instance.SessionService;
				IDataService dataService = ServiceUtil.Instance.DataService;
				if (sessionService.TicketAvailable)
				{
					string param = "ck=" + sessionService.Ticket + "&rd=" + System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(str));
					string url = dataService.ServerInfo.WebSsoRequest + System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(param));
					BrowserUtil.OpenSupportSite(url);
				}
				else
				{
					System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(BrowserUtil.GetTicketBrowser), str);
				}
			}
			catch
			{
			}
		}
		public static void OpenGalleryHandler(string str)
		{
			try
			{
				ISessionService sessionService = ServiceUtil.Instance.SessionService;
				IDataService dataService = ServiceUtil.Instance.DataService;
				if (sessionService.TicketAvailable)
				{
					string param = "ck=" + sessionService.Ticket + "&rd=" + System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(str));
					string url = dataService.ServerInfo.TuKu + System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(param));
					BrowserUtil.OpenSupportSite(url);
				}
				else
				{
					System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(BrowserUtil.GetTicketTuKuBrowser), str);
				}
			}
			catch
			{
			}
		}
		private static void GetTicketTuKuBrowser(object obj)
		{
			try
			{
				IWSClient wsClient = ServiceUtil.Instance.WsClient;
				IDataService dataService = ServiceUtil.Instance.DataService;
				ISessionService sessionService = ServiceUtil.Instance.SessionService;
				System.Threading.Monitor.Enter(BrowserUtil.logger);
				for (int i = 0; i < 1; i++)
				{
					wsClient.Url = dataService.ServerInfo.WebSsoOGet;
					wsClient.AddParams("username", sessionService.UserName);
					wsClient.AddParams("password", sessionService.Password);
					wsClient.AddParams("ticket", sessionService.Ticket);
					sessionService.Ticket = wsClient.GetTicket();
					if (!string.IsNullOrEmpty(sessionService.Ticket))
					{
						break;
					}
				}
				if (sessionService.TicketAvailable && obj != null)
				{
					string param = "ck=" + sessionService.Ticket + "&rd=" + System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(obj.ToString()));
					string url = dataService.ServerInfo.TuKu + System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(param));
					BrowserUtil.OpenSupportSite(url);
				}
				else
				{
					MessageBox.Show("无法取到Ticket!");
				}
				System.Threading.Monitor.Exit(BrowserUtil.logger);
			}
			catch (System.Exception e)
			{
				BrowserUtil.logger.Error(e.ToString());
			}
		}
		private static void GetTicketBrowser(object obj)
		{
			try
			{
				IWSClient wsClient = ServiceUtil.Instance.WsClient;
				IDataService dataService = ServiceUtil.Instance.DataService;
				ISessionService sessionService = ServiceUtil.Instance.SessionService;
				System.Threading.Monitor.Enter(BrowserUtil.logger);
				for (int i = 0; i < 1; i++)
				{
					wsClient.Url = dataService.ServerInfo.WebSsoOGet;
					wsClient.AddParams("username", sessionService.UserName);
					wsClient.AddParams("password", sessionService.Password);
					wsClient.AddParams("ticket", sessionService.Ticket);
					sessionService.Ticket = wsClient.GetTicket();
					if (!string.IsNullOrEmpty(sessionService.Ticket))
					{
						break;
					}
				}
				if (sessionService.TicketAvailable && obj != null)
				{
					string param = "ck=" + sessionService.Ticket + "&rd=" + System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(obj.ToString()));
					string url = dataService.ServerInfo.WebSsoRequest + System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(param));
					BrowserUtil.OpenSupportSite(url);
				}
				else
				{
					MessageBox.Show("无法取到Ticket!");
				}
				System.Threading.Monitor.Exit(BrowserUtil.logger);
			}
			catch (System.Exception e)
			{
				BrowserUtil.logger.Error(e.ToString());
			}
		}
		public static void OpenHyperlinkHandler(string url)
		{
			try
			{
				BrowserUtil.OpenSupportSite(url);
			}
			catch
			{
			}
		}
		private static string GetSystemDefaultBrowser()
		{
			string name = string.Empty;
			Microsoft.Win32.RegistryKey regKey = null;
			try
			{
				regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey("http\\shell\\open\\command", false);
				name = regKey.GetValue(null).ToString().ToLower().Replace(string.Concat('"'), "");
				if (!name.EndsWith("exe"))
				{
					name = name.Substring(0, name.LastIndexOf(".exe") + 4);
				}
				else
				{
					name = "iexplore";
				}
			}
			catch (System.Exception)
			{
				name = string.Format("温馨提示:\n满足下面的条件即可访问:\n1、已经安装了浏览器\n2、设置了默认浏览器", new object[0]);
			}
			finally
			{
				if (regKey != null)
				{
					regKey.Close();
				}
			}
			return name;
		}
		private static bool OpenSupportSite(string url)
		{
			string browser = BrowserUtil.GetSystemDefaultBrowser();
			bool result;
			if (!browser.Contains("温馨提示"))
			{
				try
				{
					new Process
					{
						StartInfo = 
						{
							FileName = browser,
							Arguments = url
						}
					}.Start();
				}
				catch (Win32Exception e)
				{
					if (e.NativeErrorCode == 2)
					{
						Process.Start("iexplore", url);
					}
					result = false;
					return result;
				}
				result = true;
			}
			else
			{
				MessageBox.Show(string.Format("{0}", browser), "提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
				result = false;
			}
			return result;
		}
	}
}
