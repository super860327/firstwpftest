using Microsoft.Win32;
using System;
using System.IO;
using System.Net;
using System.Reflection;
namespace IDKin.IM.Windows.Util
{
	public class AppUtil
	{
		private static AppUtil instance = null;
		public static string FilePath = System.IO.Path.GetFullPath(System.Reflection.Assembly.GetEntryAssembly().GetName().Name) + ".exe";
		private string version = "1.5.5";
		public static AppUtil Instance
		{
			get
			{
				if (AppUtil.instance == null)
				{
					AppUtil.instance = new AppUtil();
				}
				return AppUtil.instance;
			}
			set
			{
				AppUtil.instance = value;
			}
		}
		public string AppUpdateVerson()
		{
			return this.version;
		}
		public virtual string AppVersion()
		{
			string result;
			try
			{
				result = this.version + "(Beta)";
			}
			catch (System.Exception ex)
			{
				ServiceUtil.Instance.Logger.Error(ex.ToString());
				result = "内部开发版";
			}
			return result;
		}
		public virtual void StartAtLogon(string filePath, bool isAutoRun)
		{
			Microsoft.Win32.RegistryKey reg = null;
			try
			{
				if (!System.IO.File.Exists(filePath))
				{
					throw new System.Exception("该文件不存在！");
				}
				string keyName = System.IO.Path.GetFileName(System.Reflection.Assembly.GetEntryAssembly().GetName().CodeBase);
				reg = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
				if (reg == null)
				{
					reg = Microsoft.Win32.Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
				}
				if (isAutoRun)
				{
					reg.SetValue(keyName, filePath);
				}
				else
				{
					reg.SetValue(keyName, false);
				}
			}
			catch (System.Exception e)
			{
				System.Console.WriteLine("StartAtLogon():" + e.ToString());
			}
			finally
			{
				if (reg != null)
				{
					reg.Close();
				}
			}
		}
		public string LocalIP()
		{
			string result;
			try
			{
				IPHostEntry ipHost = Dns.Resolve(Dns.GetHostName());
				if (ipHost.AddressList.Length > 0)
				{
					result = ipHost.AddressList[0].ToString();
					return result;
				}
			}
			catch
			{
			}
			result = "";
			return result;
		}
	}
}
