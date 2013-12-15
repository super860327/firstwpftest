using IDKin.IM.Core;
using IDKin.IM.Windows.Util;
using System;
using System.Collections.Generic;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
namespace IDKin.IM.Windows.View.EmailAlert
{
	public class POP3Ssl : POP3
	{
		protected SslStream instream;
		private object objlock = new object();
		private object objLock2 = new object();
		public POP3Ssl(EMailModal eMailModal)
		{
			base.EMailModal = eMailModal;
			base.POPServer = eMailModal.Server;
			base.User = eMailModal.MailID;
			base.Pwd = eMailModal.PWD;
		}
		public override string TestConnect()
		{
			string result = string.Empty;
			TcpClient tcpClient = null;
			try
			{
				tcpClient = new TcpClient(base.POPServer, 995);
				this.instream = new SslStream(tcpClient.GetStream(), false, new RemoteCertificateValidationCallback(this.ValidateServerCertificate), new LocalCertificateSelectionCallback(this.SelectLocalCertificate));
				byte[] inbytes = null;
				byte[] outbytes = null;
				string input = string.Empty;
				string output = string.Empty;
				string strResult = this.CheckLogin(inbytes, outbytes, input, output);
				if (!string.IsNullOrEmpty(strResult))
				{
					result = strResult;
				}
				this.Disconnect();
			}
			catch (SocketException)
			{
				result = "找不到服务器！";
			}
			catch (System.Exception ex)
			{
				result = ex.ToString();
			}
			finally
			{
				if (this.instream != null)
				{
					this.instream.Close();
				}
				if (tcpClient != null)
				{
					tcpClient.Close();
				}
			}
			return result;
		}
		private bool ValidateServerCertificate(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			bool result;
			if (sslPolicyErrors == SslPolicyErrors.None)
			{
				result = true;
			}
			else
			{
				System.Console.WriteLine("Certificate error: {0}", sslPolicyErrors);
				result = false;
			}
			return result;
		}
		private System.Security.Cryptography.X509Certificates.X509Certificate SelectLocalCertificate(object sender, string targetHost, X509CertificateCollection localCertificates, System.Security.Cryptography.X509Certificates.X509Certificate remoteCertificate, string[] acceptableIssuers)
		{
			System.Security.Cryptography.X509Certificates.X509Certificate result;
			if (acceptableIssuers != null && acceptableIssuers.Length > 0 && localCertificates != null && localCertificates.Count > 0)
			{
				foreach (System.Security.Cryptography.X509Certificates.X509Certificate certificate in localCertificates)
				{
					string issuer = certificate.Issuer;
					if (System.Array.IndexOf<string>(acceptableIssuers, issuer) != -1)
					{
						result = certificate;
						return result;
					}
				}
			}
			if (localCertificates != null && localCertificates.Count > 0)
			{
				result = localCertificates[0];
			}
			else
			{
				result = null;
			}
			return result;
		}
		public override void Disconnect()
		{
			byte[] bytes = new byte[1024];
			this.instream.Write(System.Text.Encoding.ASCII.GetBytes("QUIT \r\n"));
			bytes = new byte[1024];
			this.instream.Read(bytes, 0, bytes.Length);
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
				lock (this.objlock)
				{
					System.DateTime LastestDateTime = System.DateTime.MinValue;
					TcpClient tcpClient = null;
					try
					{
						tcpClient = new TcpClient(base.POPServer, 995);
						this.instream = new SslStream(tcpClient.GetStream(), false, new RemoteCertificateValidationCallback(this.ValidateServerCertificate), new LocalCertificateSelectionCallback(this.SelectLocalCertificate));
						byte[] inbytes = null;
						byte[] outbytes = null;
						string input = string.Empty;
						string strRead = string.Empty;
						string strResult = this.CheckLogin(inbytes, outbytes, input, strRead);
						if (!string.IsNullOrEmpty(strResult))
						{
							result2 = System.DateTime.MaxValue;
							return result2;
						}
						int countmail = this.GetMailCount(inbytes, outbytes, input, strRead);
						for (int i = 1; i <= countmail; i++)
						{
							outbytes = new byte[1024];
							input = string.Format("top {0} 0\r\n", i);
							inbytes = System.Text.Encoding.ASCII.GetBytes(input.ToCharArray());
							this.instream.Write(inbytes, 0, inbytes.Length);
							strRead = string.Empty;
							this.instream.Read(outbytes, 0, outbytes.Length);
							strRead = System.Text.Encoding.ASCII.GetString(outbytes);
							if (strRead.IndexOf("date:", System.StringComparison.CurrentCultureIgnoreCase) > -1)
							{
								string s = strRead.Substring(strRead.IndexOf("date:", System.StringComparison.CurrentCultureIgnoreCase));
								int _index = s.IndexOf("\r\n", 0);
								if (_index >= 0)
								{
									s = s.Substring(0, _index);
								}
								try
								{
									string str = s.Substring(5);
									int index = str.IndexOf('(');
									if (index > 0)
									{
										str = str.Remove(index);
									}
									System.DateTime dateTime = System.DateTime.Parse(str);
									if (s.Length > 6 && LastestDateTime.CompareTo(dateTime) <= 0)
									{
										LastestDateTime = dateTime;
									}
								}
								catch (System.FormatException ex)
								{
									ServiceUtil.Instance.Logger.Error(ex.ToString());
								}
								catch (System.Exception ex2)
								{
									ServiceUtil.Instance.Logger.Error(ex2.ToString());
								}
							}
						}
						this.Disconnect();
					}
					catch (System.Exception ex2)
					{
						ServiceUtil.Instance.Logger.Error(ex2.ToString());
					}
					finally
					{
						if (this.instream != null)
						{
							this.instream.Dispose();
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
				lock (this.objLock2)
				{
					int countTmp = 0;
					TcpClient tcpClient = null;
					try
					{
						tcpClient = new TcpClient(base.POPServer, 995);
						this.instream = new SslStream(tcpClient.GetStream(), false, new RemoteCertificateValidationCallback(this.ValidateServerCertificate), new LocalCertificateSelectionCallback(this.SelectLocalCertificate));
						byte[] inbytes = null;
						byte[] outbytes = null;
						string strInput = null;
						string strOutput = null;
						string strResult = this.CheckLogin(inbytes, outbytes, strInput, strOutput);
						if (!string.IsNullOrEmpty(strResult))
						{
							result2 = -1;
							return result2;
						}
						int countmail = this.GetMailCount(inbytes, outbytes, strInput, strOutput);
						if (countmail == -1)
						{
							result2 = -1;
							return result2;
						}
						System.Collections.Generic.List<int> listError = new System.Collections.Generic.List<int>();
						for (int i = 1; i <= countmail; i++)
						{
							strInput = string.Format("top {0} 0\r\n", i);
							inbytes = System.Text.Encoding.ASCII.GetBytes(strInput.ToCharArray());
							this.instream.Write(inbytes, 0, inbytes.Length);
							outbytes = new byte[1024];
							this.instream.Read(outbytes, 0, outbytes.Length);
							strOutput = System.Text.Encoding.ASCII.GetString(outbytes);
							if (string.IsNullOrEmpty(strOutput))
							{
								listError.Add(i);
							}
							else
							{
								if (strOutput.Length >= 4 && strOutput.Substring(0, 4) == "-ERR")
								{
									listError.Add(i);
								}
								else
								{
									if (strOutput.IndexOf("date:", System.StringComparison.CurrentCultureIgnoreCase) > -1)
									{
										string strTime = strOutput.Substring(strOutput.IndexOf("date:", System.StringComparison.CurrentCultureIgnoreCase));
										int _index = strTime.IndexOf("\r\n", 0);
										if (_index >= 0)
										{
											strTime = strTime.Substring(0, _index);
										}
										string str = string.Empty;
										try
										{
											if (strTime.Length > 5)
											{
												str = strTime.Substring(5);
												int index = str.IndexOf('(');
												if (index > 0)
												{
													str = str.Remove(index);
												}
												if (System.DateTime.Parse(str).CompareTo(dateTime) > 0)
												{
													countTmp++;
												}
											}
										}
										catch (System.FormatException)
										{
											countTmp += this.GetCountByDate(str, dateTime);
										}
									}
								}
							}
						}
						if (countTmp != -1 && countmail != -1 && listError.Count > 0)
						{
							foreach (int i in listError)
							{
								int kk = this.GetCount(i, dateTime);
								if (kk != -1)
								{
									countTmp += this.GetCount(i, dateTime);
								}
							}
						}
						this.Disconnect();
					}
					catch (System.Exception ex)
					{
						countTmp = -1;
						ServiceUtil.Instance.Logger.Error(ex.ToString());
					}
					finally
					{
						if (this.instream != null)
						{
							this.instream.Dispose();
						}
						if (tcpClient != null)
						{
							tcpClient.Close();
						}
					}
					result = countTmp;
				}
				result2 = result;
			}
			return result2;
		}
		private int GetCountByDate(string strTime, System.DateTime dateTime)
		{
			int count = 0;
			if (strTime.Length > 5 && strTime.IndexOf("date:", 5, System.StringComparison.CurrentCultureIgnoreCase) > -1)
			{
				string str = strTime.Substring(strTime.IndexOf("date:", 5, System.StringComparison.CurrentCultureIgnoreCase));
				int _index = str.IndexOf("\r\n", 0);
				if (_index >= 0)
				{
					str = str.Substring(0, _index);
				}
				int index = str.IndexOf('(');
				if (index > 0)
				{
					str = str.Remove(index);
				}
				try
				{
					if (System.DateTime.Parse(str).CompareTo(dateTime) > 0)
					{
						count++;
					}
				}
				catch (System.FormatException)
				{
					count = this.GetCountByDate(str, dateTime);
				}
			}
			return count;
		}
		private int GetCount(int i, System.DateTime dateTime)
		{
			int countTmp = 0;
			int j = 0;
			while (j < 5)
			{
				string strInput = string.Format("top {0} 0\r\n", i);
				byte[] inbytes = System.Text.Encoding.ASCII.GetBytes(strInput.ToCharArray());
				this.instream.Write(inbytes, 0, inbytes.Length);
				byte[] outbytes = new byte[1024];
				this.instream.Read(outbytes, 0, outbytes.Length);
				string strOutput = System.Text.Encoding.ASCII.GetString(outbytes);
				if (string.IsNullOrEmpty(strOutput))
				{
					j++;
				}
				else
				{
					if (strOutput.Length < 4 || !(strOutput.Substring(0, 4) == "-ERR"))
					{
						if (strOutput.IndexOf("date:", System.StringComparison.CurrentCultureIgnoreCase) > -1)
						{
							string s = strOutput.Substring(strOutput.IndexOf("date:", System.StringComparison.CurrentCultureIgnoreCase));
							int _index = s.IndexOf("\r\n", 0);
							if (_index >= 0)
							{
								s = s.Substring(0, _index);
							}
							try
							{
								if (s.Length >= 6)
								{
									string str = s.Substring(5);
									int index = str.IndexOf('(');
									if (index > 0)
									{
										str = str.Remove(index);
									}
									if (System.DateTime.Parse(str).CompareTo(dateTime) > 0)
									{
										countTmp++;
									}
								}
							}
							catch (System.FormatException)
							{
							}
						}
						break;
					}
					j++;
				}
			}
			return countTmp;
		}
		private int GetMailCount(byte[] inbytes, byte[] outbytes, string strInput, string strOutput)
		{
			strInput = "stat \r\n";
			inbytes = System.Text.Encoding.ASCII.GetBytes(strInput.ToCharArray());
			this.instream.Write(inbytes, 0, inbytes.Length);
			outbytes = new byte[1024];
			this.instream.Read(outbytes, 0, outbytes.Length);
			strOutput = System.Text.Encoding.ASCII.GetString(outbytes);
			int result;
			if (strOutput.Substring(0, 4) == "-ERR")
			{
				result = -1;
			}
			else
			{
				string[] tmpArray = strOutput.Split(new char[]
				{
					' '
				});
				int countmail = 0;
				try
				{
					if (tmpArray.Length > 1)
					{
						countmail = System.Convert.ToInt32(tmpArray[1]);
					}
				}
				catch (System.FormatException)
				{
					countmail = -1;
				}
				result = countmail;
			}
			return result;
		}
		private string CheckLogin(byte[] inbytes, byte[] outbytes, string input, string strRead)
		{
			string result = string.Empty;
			try
			{
				this.instream.AuthenticateAsClient(base.POPServer);
				outbytes = new byte[1024];
				this.instream.Read(outbytes, 0, outbytes.Length);
				strRead = System.Text.Encoding.ASCII.GetString(outbytes);
				input = "user " + base.User + "\r\n";
				inbytes = System.Text.Encoding.ASCII.GetBytes(input.ToCharArray());
				this.instream.Write(inbytes, 0, inbytes.Length);
				outbytes = new byte[1024];
				this.instream.Read(outbytes, 0, outbytes.Length);
				strRead = System.Text.Encoding.ASCII.GetString(outbytes);
				input = "pass " + base.Pwd + "\r\n";
				inbytes = System.Text.Encoding.ASCII.GetBytes(input.ToCharArray());
				this.instream.Write(inbytes, 0, inbytes.Length);
				outbytes = new byte[1024];
				this.instream.Read(outbytes, 0, outbytes.Length);
				strRead = System.Text.Encoding.ASCII.GetString(outbytes);
				if (strRead.IndexOf("-ERR") != -1)
				{
					base.EMailModal.HasError = true;
					result = "用户名密码有误！";
				}
				else
				{
					base.EMailModal.HasError = false;
				}
			}
			catch (System.Exception ex)
			{
				result = ex.ToString();
			}
			return result;
		}
		public override void GetAll()
		{
		}
		public override System.Collections.Generic.List<string> GetAllNew()
		{
			return null;
		}
		public override string[] ReadLocal()
		{
			return null;
		}
	}
}
