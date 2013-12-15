using IDKin.IM.Core;
using IDKin.IM.Windows.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
namespace IDKin.IM.Windows.View.EmailAlert
{
	public class POP3Normal : POP3
	{
		protected System.IO.Stream ns;
		private object objLock = new object();
		private object objLock2 = new object();
		public POP3Normal(EMailModal eMailModal)
		{
			base.EMailModal = eMailModal;
			base.POPServer = base.EMailModal.Server;
			base.User = base.EMailModal.MailID;
			base.Pwd = base.EMailModal.PWD;
			if (eMailModal.Server == "mail.idkin.net")
			{
				base.User = base.EMailModal.MailID.Substring(0, eMailModal.MailID.IndexOf('@'));
			}
		}
		public override string TestConnect()
		{
			string result = string.Empty;
			TcpClient tcpClient = null;
			try
			{
				tcpClient = new TcpClient(base.POPServer, 110);
				this.ns = tcpClient.GetStream();
				base.sr = new System.IO.StreamReader(this.ns);
				string readuser = string.Empty;
				string readpwd = string.Empty;
				this.ns = tcpClient.GetStream();
				base.sr = new System.IO.StreamReader(this.ns);
				base.sr.ReadLine();
				string input = "user " + base.User + "\r\n";
				byte[] outbytes = System.Text.Encoding.ASCII.GetBytes(input.ToCharArray());
				this.ns.Write(outbytes, 0, outbytes.Length);
				readuser = base.sr.ReadLine();
				if (readuser == null)
				{
					readuser = "readuser 返回值空";
				}
				input = "pass " + base.Pwd + "\r\n";
				outbytes = System.Text.Encoding.ASCII.GetBytes(input.ToCharArray());
				this.ns.Write(outbytes, 0, outbytes.Length);
				readpwd = base.sr.ReadLine();
				if (readpwd == null)
				{
					readpwd = "readpwd 返回值空";
				}
				if (readuser.IndexOf("-ERR") != -1 || readpwd.IndexOf("-ERR") != -1)
				{
					if (readuser.IndexOf("pop") != -1)
					{
						result = "没有申请pop权限，请申请！";
					}
					else
					{
						result = "用户名密码有误,请重试!";
					}
				}
				input = "quit\r\n";
				outbytes = System.Text.Encoding.ASCII.GetBytes(input.ToCharArray());
				this.ns.Write(outbytes, 0, outbytes.Length);
				readpwd = base.sr.ReadLine();
			}
			catch (SocketException es)
			{
				result = string.Format("网络错误：未找到服务器", new object[0]);
				ServiceUtil.Instance.Logger.Error(es.ToString());
			}
			catch (System.Exception ex)
			{
				result = string.Format("未知错误{0}", ex.Message);
				ServiceUtil.Instance.Logger.Error(ex.ToString());
			}
			finally
			{
				if (this.ns != null)
				{
					this.ns.Dispose();
				}
				if (base.sr != null)
				{
					base.sr.Dispose();
				}
				if (tcpClient != null)
				{
					tcpClient.Close();
				}
			}
			return result;
		}
		public override void Disconnect()
		{
			string input = "quit\r\n";
			byte[] outbytes = System.Text.Encoding.ASCII.GetBytes(input.ToCharArray());
			this.ns.Write(outbytes, 0, outbytes.Length);
			this.ns.Close();
		}
		public override int GetMailCount(System.DateTime dateTime)
		{
			int result = 0;
			int result2;
			if (string.IsNullOrEmpty(base.POPServer))
			{
				result2 = -1;
			}
			else
			{
				lock (this.objLock)
				{
					int c = 0;
					string[] ids = this.ReadLocal();
					System.Collections.Generic.List<string> NewIds = this.GetAllNew();
					if (ids != null && NewIds != null)
					{
						foreach (string s in NewIds)
						{
							if (!ids.Contains(s))
							{
								c++;
							}
						}
					}
					result = c;
				}
				result2 = result;
			}
			return result2;
		}
		public override System.DateTime GetLastestDateTime()
		{
			System.DateTime result2;
			if (string.IsNullOrEmpty(base.POPServer))
			{
				result2 = System.DateTime.MaxValue;
			}
			else
			{
				System.DateTime result;
				lock (this.objLock2)
				{
					System.DateTime LastestDateTime = System.DateTime.MinValue;
					TcpClient tcpClient = null;
					try
					{
						tcpClient = new TcpClient(base.POPServer, 110);
						string readuser = string.Empty;
						string readpwd = string.Empty;
						this.ns = tcpClient.GetStream();
						base.sr = new System.IO.StreamReader(this.ns);
						base.sr.ReadLine();
						string input = "user " + base.User + "\r\n";
						byte[] outbytes = System.Text.Encoding.ASCII.GetBytes(input.ToCharArray());
						this.ns.Write(outbytes, 0, outbytes.Length);
						readuser = base.sr.ReadLine();
						input = "pass " + base.Pwd + "\r\n";
						outbytes = System.Text.Encoding.ASCII.GetBytes(input.ToCharArray());
						this.ns.Write(outbytes, 0, outbytes.Length);
						readpwd = base.sr.ReadLine();
						if (readuser.IndexOf("-ERR") != -1 || readpwd.IndexOf("-ERR") != -1)
						{
							base.EMailModal.HasError = true;
							result2 = System.DateTime.MaxValue;
							return result2;
						}
						input = "stat \r\n";
						outbytes = System.Text.Encoding.ASCII.GetBytes(input.ToCharArray());
						this.ns.Write(outbytes, 0, outbytes.Length);
						string thisResponse = base.sr.ReadLine();
						if (thisResponse.Substring(0, 4) == "-ERR")
						{
							result2 = System.DateTime.MaxValue;
							return result2;
						}
						string[] tmpArray = thisResponse.Split(new char[]
						{
							' '
						});
						int countmail = System.Convert.ToInt32(tmpArray[1]);
						for (int i = 1; i <= countmail; i++)
						{
							input = string.Format("top {0} 0\r\n", i);
							outbytes = System.Text.Encoding.ASCII.GetBytes(input.ToCharArray());
							this.ns.Write(outbytes, 0, outbytes.Length);
							thisResponse = string.Empty;
							while (thisResponse != null && !thisResponse.StartsWith("Date:", System.StringComparison.OrdinalIgnoreCase))
							{
								thisResponse = base.sr.ReadLine();
							}
							if (thisResponse != null)
							{
								if (thisResponse.Length < 4 || !(thisResponse.Substring(0, 4) == "-ERR"))
								{
									try
									{
										string str = thisResponse.Substring(5);
										int index = str.IndexOf('(');
										if (index > 0)
										{
											str = str.Remove(index);
										}
										System.DateTime dateTime = System.DateTime.Parse(str);
										if (thisResponse.Length > 6 && LastestDateTime.CompareTo(dateTime) <= 0)
										{
											LastestDateTime = dateTime;
										}
									}
									catch (System.FormatException)
									{
									}
									catch (System.Exception ex)
									{
										ServiceUtil.Instance.Logger.Error(ex.ToString());
									}
								}
							}
						}
						this.Disconnect();
					}
					catch (System.Exception e)
					{
						ServiceUtil.Instance.Logger.Error(e.ToString());
					}
					finally
					{
						if (this.ns != null)
						{
							this.ns.Dispose();
						}
						if (base.sr != null)
						{
							base.sr.Dispose();
						}
						if (tcpClient != null)
						{
							tcpClient.Close();
						}
					}
					result = LastestDateTime;
				}
				result2 = result;
			}
			return result2;
		}
		public override void GetAll()
		{
			string result = string.Empty;
			string input = string.Empty;
			string readuser = string.Empty;
			string readpwd = string.Empty;
			TcpClient tcpClient = new TcpClient(base.POPServer, 110);
			this.ns = tcpClient.GetStream();
			base.sr = new System.IO.StreamReader(this.ns);
			base.sr.ReadLine();
			try
			{
				input = "user " + base.User + "\r\n";
				byte[] outbytes = System.Text.Encoding.ASCII.GetBytes(input.ToCharArray());
				this.ns.Write(outbytes, 0, outbytes.Length);
				readuser = base.sr.ReadLine();
				input = "pass " + base.Pwd + "\r\n";
				outbytes = System.Text.Encoding.ASCII.GetBytes(input.ToCharArray());
				this.ns.Write(outbytes, 0, outbytes.Length);
				readpwd = base.sr.ReadLine();
				if (readuser.IndexOf("-ERR") != -1 || readpwd.IndexOf("-ERR") != -1)
				{
				}
				input = "uidl\r\n";
				outbytes = System.Text.Encoding.ASCII.GetBytes(input.ToCharArray());
				this.ns.Write(outbytes, 0, outbytes.Length);
				string thisResponse = base.sr.ReadLine();
				System.Collections.Generic.List<string> ids;
				if (thisResponse == "+OK")
				{
					ids = new System.Collections.Generic.List<string>();
					while ((thisResponse = base.sr.ReadLine()) != ".")
					{
						if (!string.IsNullOrEmpty(thisResponse) && thisResponse.Split(new char[]
						{
							' '
						}).Length > 1 && thisResponse != ".")
						{
							ids.Add(thisResponse.Split(new char[]
							{
								' '
							})[1]);
						}
					}
				}
				else
				{
					ids = new System.Collections.Generic.List<string>(int.Parse(thisResponse.Split(new char[]
					{
						' '
					})[1]) + 1);
					while ((thisResponse = base.sr.ReadLine()) != ".")
					{
						ids.Add(thisResponse.Split(new char[]
						{
							' '
						})[1]);
					}
				}
				System.IO.File.WriteAllText(base.EMailModal.MailID + ".txt", string.Empty);
				System.IO.File.AppendAllLines(base.EMailModal.MailID + ".txt", ids);
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}
		public override System.Collections.Generic.List<string> GetAllNew()
		{
			System.Collections.Generic.List<string> ids = null;
			string result = string.Empty;
			string input = string.Empty;
			string readuser = string.Empty;
			string readpwd = string.Empty;
			TcpClient tcpClient = new TcpClient(base.POPServer, 110);
			this.ns = tcpClient.GetStream();
			base.sr = new System.IO.StreamReader(this.ns);
			base.sr.ReadLine();
			try
			{
				input = "user " + base.User + "\r\n";
				byte[] outbytes = System.Text.Encoding.ASCII.GetBytes(input.ToCharArray());
				this.ns.Write(outbytes, 0, outbytes.Length);
				readuser = base.sr.ReadLine();
				input = "pass " + base.Pwd + "\r\n";
				outbytes = System.Text.Encoding.ASCII.GetBytes(input.ToCharArray());
				this.ns.Write(outbytes, 0, outbytes.Length);
				readpwd = base.sr.ReadLine();
				if (readuser.IndexOf("-ERR") != -1 || readpwd.IndexOf("-ERR") != -1)
				{
				}
				input = "uidl\r\n";
				outbytes = System.Text.Encoding.ASCII.GetBytes(input.ToCharArray());
				this.ns.Write(outbytes, 0, outbytes.Length);
				string thisResponse = base.sr.ReadLine();
				if (thisResponse == "+OK")
				{
					ids = new System.Collections.Generic.List<string>();
					while ((thisResponse = base.sr.ReadLine()) != ".")
					{
						if (!string.IsNullOrEmpty(thisResponse) && thisResponse.Split(new char[]
						{
							' '
						}).Length > 1 && thisResponse != ".")
						{
							ids.Add(thisResponse.Split(new char[]
							{
								' '
							})[1]);
						}
					}
				}
				else
				{
					ids = new System.Collections.Generic.List<string>(int.Parse(thisResponse.Split(new char[]
					{
						' '
					})[1]) + 1);
					while ((thisResponse = base.sr.ReadLine()) != ".")
					{
						ids.Add(thisResponse.Split(new char[]
						{
							' '
						})[1]);
					}
				}
			}
			catch (System.Exception ex)
			{
				System.Console.WriteLine(ex.ToString());
			}
			return ids;
		}
		public override string[] ReadLocal()
		{
			string[] result;
			if (System.IO.File.Exists(base.EMailModal.MailID + ".txt"))
			{
				result = System.IO.File.ReadAllLines(base.EMailModal.MailID + ".txt");
			}
			else
			{
				result = null;
			}
			return result;
		}
	}
}
