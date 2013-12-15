using IDKin.IM.Communicate;
using IDKin.IM.Core;
using IDKin.IM.Data;
using IDKin.IM.Log;
using IDKin.IM.Windows.Util;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Documents;
namespace IDKin.IM.Windows.Comps.MessageCenter
{
	internal class FileReadWrite
	{
		private ILogger logger = ServiceUtil.Instance.Logger;
		private IUtilService utilService = ServiceUtil.Instance.utilService;
		private IDataService dataService = ServiceUtil.Instance.DataService;
		private ISessionService sessionService = ServiceUtil.Instance.SessionService;
		private UtilService service = new UtilService();
		private string fileDirectory;
		private System.IO.FileStream fst;
		private int offset;
		private string temp;
		private byte[] data;
		private Staff staff;
		private EntGroup group;
		private MessageActorType type;
		public MessageActorType Type
		{
			set
			{
				this.type = value;
			}
		}
		public FileReadWrite()
		{
			this.InitData();
		}
		private void InitData()
		{
			this.fileDirectory = null;
			this.fst = null;
			this.offset = 0;
			this.temp = null;
			this.data = null;
			this.staff = null;
			this.group = null;
		}
		public void CreateFile(string name)
		{
			try
			{
				this.fst = new System.IO.FileStream(this.fileDirectory + "\\" + name, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite);
			}
			catch (System.IO.FileLoadException ex)
			{
				if (this.logger != null)
				{
					this.logger.Error(ex);
				}
			}
		}
		public void CreateDirctory(string path)
		{
			System.IO.Directory.CreateDirectory(path);
			this.fileDirectory = path;
		}
		public void OpenFile(string name)
		{
			try
			{
				this.fst = new System.IO.FileStream(name, System.IO.FileMode.Open, System.IO.FileAccess.Read);
			}
			catch (System.FieldAccessException ex)
			{
				if (this.logger != null)
				{
					this.logger.Error(ex);
				}
			}
			catch (System.IO.FileLoadException ex2)
			{
				if (this.logger != null)
				{
					this.logger.Error(ex2);
				}
			}
			catch (System.IO.FileNotFoundException ex3)
			{
				if (this.logger != null)
				{
					this.logger.Error(ex3);
				}
			}
		}
		public void WriteFileStream(Message message)
		{
			if (this.type == MessageActorType.EntStaff)
			{
				this.staff = this.dataService.GetStaff((long)((ulong)Jid.GetUid(message.FromJid)));
				if (this.staff != null)
				{
					this.temp = string.Concat(new string[]
					{
						"Staff$",
						this.staff.Uid.ToString(),
						"$",
						message.ToJid,
						"$",
						this.staff.Name,
						"$",
						message.CreateTime,
						"\r\n"
					});
					this.data = this.StringToBytes(this.temp);
					this.fst.Seek((long)this.offset, System.IO.SeekOrigin.Current);
					this.fst.Write(this.data, 0, this.data.Length);
					this.fst.Flush();
					message.MessageString = this.utilService.MessageEncode(message.MessageBlocks);
					this.WriteMessageToFile(message.MessageString);
				}
				this.staff = null;
				this.temp = null;
				this.data = null;
			}
			else
			{
				this.group = this.dataService.GetEntGroup(message.Gid);
				if (this.group != null)
				{
					this.temp = string.Concat(new string[]
					{
						"Group$",
						this.group.Gid.ToString(),
						"$",
						this.group.Name,
						"$",
						message.CreateTime,
						"\r\n"
					});
					this.data = this.StringToBytes(this.temp);
					this.fst.Write(this.data, this.offset, this.data.Length);
					this.fst.Flush();
					message.MessageString = this.utilService.MessageEncode(message.MessageBlocks);
					this.WriteMessageToFile(message.MessageString);
				}
				this.group = null;
				this.temp = null;
				this.data = null;
			}
		}
		public void ReadFile(string filePath)
		{
			Message[] message = new Message[10];
			System.IO.StreamReader reader = new System.IO.StreamReader(filePath);
			for (int i = 0; i < 10; i++)
			{
				this.temp = reader.ReadLine();
				char ch = '$';
				string[] str = this.temp.Split(new char[]
				{
					ch
				});
				if (str[0] == "Staff")
				{
					message[i] = new Message();
					message[i].FromJid = str[1];
					message[i].ToJid = str[2];
					message[i].CreateTime = str[4];
					this.temp = reader.ReadLine();
					string[] messages = this.temp.Split(new char[]
					{
						ch
					});
					if (messages.Length > 0)
					{
						int j = 0;
						while (j <= messages.Length - 1)
						{
							if (messages[j] != "")
							{
								string mark = messages[j].Trim().Substring(0, 2);
								string text = mark;
								if (text != null)
								{
									if (!(text == "I:"))
									{
										if (!(text == "F:"))
										{
											if (!(text == "T:"))
											{
												if (text == "L:")
												{
													message[i].MessageString = messages[j];
												}
											}
											else
											{
												message[i].MessageString = messages[j];
											}
										}
										else
										{
											message[i].MessageString = messages[j];
										}
									}
									else
									{
										this.temp = messages[j].Substring(2);
										byte[] imagebyte = this.StringConvertByte(this.temp);
										System.IO.MemoryStream ms = new System.IO.MemoryStream(imagebyte);
										try
										{
											Image image = Image.FromStream(ms);
										}
										catch (System.NullReferenceException ex)
										{
											if (this.logger != null)
											{
												this.logger.Error(ex);
											}
										}
										catch (System.NotSupportedException ex2)
										{
											if (this.logger != null)
											{
												this.logger.Error(ex2);
											}
										}
									}
								}
								IL_20B:
								j++;
								continue;
								goto IL_20B;
							}
							break;
						}
					}
				}
			}
		}
		private void WriteMessageToFile(string message)
		{
			Regex regexParagraph = new Regex("\\[(?<Paragraph>[\\s\\S]+?)\\]");
			MatchCollection matchsParagraph = regexParagraph.Matches(message);
			if (matchsParagraph != null)
			{
				Paragraph[] paragraphs = new Paragraph[matchsParagraph.Count];
				Regex regexText = new Regex("\\{(?<Text>[\\s\\S]+?)\\}");
				Regex regexT = new Regex("^T:");
				Regex regexI = new Regex("^I:");
				Regex regexL = new Regex("^L:");
				Regex regexF = new Regex("^F:");
				for (int i = 0; i < matchsParagraph.Count; i++)
				{
					Paragraph paragraph = new Paragraph();
					string p = matchsParagraph[i].Groups["Paragraph"].Value;
					MatchCollection matchsText = regexText.Matches(p);
					for (int x = 0; x < matchsText.Count; x++)
					{
						string text = matchsText[x].Groups["Text"].Value;
						text = this.service.DeEscapeSB(text);
						text = this.service.DeEscapeBB(text);
						if (regexT.IsMatch(text))
						{
							this.temp = "T:" + text.Substring(2) + "$";
							this.data = this.StringToBytes(this.temp);
							this.fst.Seek((long)this.offset, System.IO.SeekOrigin.Current);
							this.fst.Write(this.data, 0, this.data.Length);
							this.data = null;
						}
						else
						{
							if (regexI.IsMatch(text))
							{
								string file = text.Substring(2);
								string fileName = this.sessionService.DirImage + file;
								string name = FileReadWrite.GenerateFileName(file);
								this.fileDirectory += file;
								System.IO.File.Copy(fileName, this.fileDirectory, true);
								this.temp = "I:" + this.fileDirectory + "$";
								this.data = this.StringToBytes(this.temp);
								this.fst.Seek((long)this.offset, System.IO.SeekOrigin.Current);
								this.fst.Write(this.data, 0, this.data.Length);
								this.fileDirectory = this.fileDirectory.Substring(0, this.fileDirectory.IndexOf(file));
								this.data = null;
							}
							else
							{
								if (regexF.IsMatch(text))
								{
									this.temp = "F:" + text.Substring(2) + "$";
									this.data = this.StringToBytes(this.temp);
									this.fst.Seek((long)this.offset, System.IO.SeekOrigin.Current);
									this.fst.Write(this.data, 0, this.data.Length);
									this.data = null;
								}
								else
								{
									if (regexL.IsMatch(text))
									{
										this.temp = "L:" + text.Substring(2) + "$";
										this.data = this.StringToBytes(this.temp);
										this.fst.Seek((long)this.offset, System.IO.SeekOrigin.Current);
										this.fst.Write(this.data, 0, this.data.Length);
										this.data = null;
									}
								}
							}
						}
					}
				}
				this.temp = "\r\n";
				this.data = this.StringToBytes(this.temp);
				this.fst.Seek((long)this.offset, System.IO.SeekOrigin.Current);
				this.fst.Write(this.data, 0, this.data.Length);
				this.fst.Flush();
				this.data = null;
				this.temp = null;
			}
		}
		private byte[] ImageToBtye(Image image)
		{
			byte[] result;
			using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
			{
				if (image != null)
				{
					Bitmap bmp = new Bitmap(image.Width, image.Height);
					Graphics g = Graphics.FromImage(bmp);
					g.DrawImage(image, 0, 0, image.Width, image.Height);
					g.Dispose();
					bmp.Save(ms, image.RawFormat);
					bmp.Dispose();
				}
				result = ms.ToArray();
			}
			return result;
		}
		private Bitmap BytesToBitmap(byte[] bytes)
		{
			System.IO.MemoryStream stream = null;
			Bitmap result;
			try
			{
				stream = new System.IO.MemoryStream(bytes);
				result = new Bitmap(new Bitmap(stream));
				return result;
			}
			catch (System.ArgumentException ex)
			{
				if (this.logger != null)
				{
					this.logger.Error(ex);
				}
			}
			finally
			{
				stream.Close();
			}
			result = null;
			return result;
		}
		private byte[] ImageToBytes(Image image)
		{
			System.IO.MemoryStream stream = null;
			byte[] result;
			try
			{
				stream = new System.IO.MemoryStream();
				image.Save(stream, image.RawFormat);
				byte[] byteimage = new byte[stream.Length];
				byteimage = stream.ToArray();
				result = byteimage;
				return result;
			}
			catch (System.ArgumentException ex)
			{
				if (this.logger != null)
				{
					this.logger.Error(ex);
				}
			}
			finally
			{
				stream.Close();
			}
			result = null;
			return result;
		}
		private byte[] ImageDatebytes(string filePath)
		{
			byte[] result;
			if (!System.IO.File.Exists(filePath))
			{
				result = null;
			}
			else
			{
				Bitmap map = new Bitmap(Image.FromFile(filePath));
				using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
				{
					map.Save(ms, ImageFormat.Png);
					ms.Flush();
					byte[] mapbyte = ms.ToArray();
					result = mapbyte;
				}
			}
			return result;
		}
		private Bitmap GetImage(byte[] imageData)
		{
			Bitmap result;
			try
			{
				using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
				{
					ms.Write(imageData, 0, imageData.Length);
					Bitmap map = new Bitmap(ms);
					result = map;
					return result;
				}
			}
			catch (System.ArgumentException ex)
			{
				if (this.logger != null)
				{
					this.logger.Error(ex);
				}
			}
			result = null;
			return result;
		}
		private byte[] StringToBytes(string message)
		{
			return System.Text.Encoding.UTF8.GetBytes(message);
		}
		private string BtyeToString(byte[] message)
		{
			return System.Text.Encoding.UTF8.GetString(message);
		}
		public void CloseStream()
		{
			this.fst.Close();
		}
		private byte[] StringConvertByte(string str)
		{
			byte[] bytedata = new byte[str.Length];
			for (int i = 0; i < str.Length; i++)
			{
				bytedata[i] = (byte)str[i];
			}
			return bytedata;
		}
		public static string GenerateFileName(string fileName)
		{
			System.IO.FileStream file = null;
			string name = string.Empty;
			try
			{
				if (System.IO.File.Exists(fileName))
				{
					file = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
					name = FileReadWrite.MD5Stream(file) + System.IO.Path.GetExtension(fileName);
				}
			}
			catch (System.Exception e)
			{
				ServiceUtil.Instance.Logger.Error(e.ToString());
			}
			finally
			{
				if (file != null)
				{
					file.Dispose();
					file = null;
				}
			}
			return name;
		}
		public static string MD5Stream(System.IO.Stream stream)
		{
			System.Security.Cryptography.MD5CryptoServiceProvider md5 = null;
			string resule = string.Empty;
			if (stream != null)
			{
				try
				{
					md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
					byte[] hash = md5.ComputeHash(stream);
					resule = System.BitConverter.ToString(hash);
					resule = resule.Replace("-", "");
				}
				finally
				{
					if (md5 != null)
					{
						md5.Dispose();
					}
				}
			}
			stream.Close();
			return resule;
		}
	}
}
