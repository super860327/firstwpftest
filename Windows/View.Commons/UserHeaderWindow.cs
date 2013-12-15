using IDKin.IM.Communicate;
using IDKin.IM.Core;
using IDKin.IM.CustomComponents.Controls;
using IDKin.IM.Data;
using IDKin.IM.ImageService;
using IDKin.IM.Log;
using IDKin.IM.Theme.Helper;
using IDKin.IM.Windows.Util;
using Microsoft.Win32;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
namespace IDKin.IM.Windows.View.Commons
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public partial class UserHeaderWindow : Window//, IComponentConnector
	{
		public delegate void NewHeadImageDelegate(string file);
		public UserHeaderWindow.NewHeadImageDelegate NewHeadImage;
		private ISessionService sessionService = ServiceUtil.Instance.SessionService;
		private IImageService imageService = ServiceUtil.Instance.ImageService;
		private ILogger logger = ServiceUtil.Instance.Logger;
		private CanvasCustom imageCanvas = new CanvasCustom();
		private int size = 350;
		private double imageScale = 1.0;
		private double pixelScale = 1.0;
		private IFileService fileService = ServiceUtil.Instance.FileService;
		private int mouseDropDirection = -1;
        //internal UserHeaderWindow userHeaderWindow;
        //internal Border OuterBorder;
        //internal Border InnerBorder;
        //internal StatusBar topBar;
        //internal ImageButton btnMin;
        //internal ImageButton btnClose;
        //internal Border leftMenuBorder;
        //internal System.Windows.Controls.Image img40;
        //internal System.Windows.Controls.Image img110;
        //internal Button btnSelectFile;
        //internal Canvas ImageBox;
        //internal System.Windows.Controls.Image img;
        //internal System.Windows.Shapes.Rectangle rectangleBox;
        //private bool _contentLoaded;
		public UserHeaderWindow(Staff staff)
		{
			ThemeSwitcher.LoadSkin(ThemeEnum.Aero, this);
			this.InitializeComponent();
			this.InitUI();
			this.imageCanvas.Width = 200.0;
			this.imageCanvas.Height = 200.0;
			this.imageCanvas.Visibility = Visibility.Hidden;
			this.imageCanvas.CacheImage = new CanvasCustom.CacheImageDelegate(this.CacheImageHandler);
			this.ImageBox.Children.Add(this.imageCanvas);
			this.size = (int)this.ImageBox.Height;
			this.img40.Source = staff.HeaderImage;
			this.img110.Source = staff.HeaderImage;
			this.img.Width = (double)this.size;
			this.img.Height = (double)this.size;
			this.img.Source = staff.HeaderImage;
		}
		private void InitUI()
		{
			this.leftMenuBorder.Loaded += delegate(object sender, RoutedEventArgs e)
			{
				this.leftMenuBorder.Background = new ImageBrush
				{
					ImageSource = this.imageService.GetBackground(ImageTypeBackground.LeftMenu)
				};
			};
			this.leftMenuBorder.Unloaded += delegate(object sender, RoutedEventArgs e)
			{
				this.imageService.RemoveBackground(ImageTypeBackground.LeftMenu);
			};
		}
		private void btnSelectFile_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				OpenFileDialog dialog = new OpenFileDialog();
				dialog.Filter = "图片文件类型(*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png";
				dialog.FilterIndex = 0;
				dialog.RestoreDirectory = true;
				dialog.Multiselect = false;
				dialog.ShowReadOnly = true;
				if (dialog.ShowDialog() == true)
				{
					Uri fileUri = new Uri(dialog.FileName, UriKind.RelativeOrAbsolute);
					BitmapImage bi = new BitmapImage(new Uri(dialog.FileName, UriKind.RelativeOrAbsolute));
					this.pixelScale = (double)bi.PixelWidth / bi.Width;
					this.pixelScale = (double)bi.PixelHeight / bi.Height;
					if (bi.Width > bi.Height)
					{
						this.imageScale = bi.Width / (double)this.size;
						this.img.Width = (double)this.size;
						this.img.Height = (double)this.size * bi.Height / bi.Width;
						double top = ((double)this.size - this.img.Height) / 2.0;
						this.img.Margin = new Thickness(0.0, top, 0.0, top);
						this.imageCanvas.MoveArea = new Thickness(0.0, top, this.ImageBox.Width, this.ImageBox.Height - top);
						this.rectangleBox.Width = (double)this.size;
						this.rectangleBox.Height = this.img.Height;
						this.rectangleBox.Margin = new Thickness(0.0, top, 0.0, top);
					}
					else
					{
						this.imageScale = bi.Height / (double)this.size;
						this.img.Height = (double)this.size;
						this.img.Width = (double)this.size * bi.Width / bi.Height;
						double left = ((double)this.size - this.img.Width) / 2.0;
						this.img.Margin = new Thickness(left, 0.0, left, 0.0);
						this.imageCanvas.MoveArea = new Thickness(left, 0.0, this.ImageBox.Width - left, this.ImageBox.Height);
						this.rectangleBox.Height = (double)this.size;
						this.rectangleBox.Width = this.img.Width;
						this.rectangleBox.Margin = new Thickness(left, 0.0, left, 0.0);
					}
					this.img.Source = bi;
					this.CompsSizeProcessor();
					this.CompsCenterProcessor();
					ImageBrush ib = new ImageBrush();
					this.imageCanvas.Background = ib;
					this.imageCanvas.Visibility = Visibility.Visible;
					this.CacheImageHandler(this.imageCanvas.Margin.Left - this.imageCanvas.MoveArea.Left, this.imageCanvas.Margin.Top - this.imageCanvas.MoveArea.Top, this.imageCanvas.Width, this.imageCanvas.Height);
				}
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		private void CompsSizeProcessor()
		{
			if (this.img.Width < this.imageCanvas.Width)
			{
				this.imageCanvas.Width = this.img.Width;
			}
			if (this.img.Height < this.imageCanvas.Height)
			{
				this.imageCanvas.Height = this.img.Height;
			}
			if (this.imageCanvas.Width > this.imageCanvas.Height)
			{
				this.imageCanvas.Width = this.imageCanvas.Height;
			}
			else
			{
				if (this.imageCanvas.Width < this.imageCanvas.Height)
				{
					this.imageCanvas.Height = this.imageCanvas.Width;
				}
			}
		}
		private void CompsCenterProcessor()
		{
			this.imageCanvas.Margin = new Thickness(this.rectangleBox.Margin.Left + this.rectangleBox.Width / 2.0 - this.imageCanvas.Width / 2.0, this.rectangleBox.Margin.Top + this.rectangleBox.Height / 2.0 - this.imageCanvas.Height / 2.0, 0.0, 0.0);
		}
		private void Sure_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (this.img110.Source != null)
				{
					string file = this.SaveImage();
					if (!string.IsNullOrEmpty(file))
					{
						this.UploadImage(file);
						if (this.NewHeadImage != null)
						{
							this.NewHeadImage(file);
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
			base.Close();
		}
		private void Cancel_Click(object sender, RoutedEventArgs e)
		{
			base.Close();
		}
		private void UploadImage(string file)
		{
			System.IO.FileInfo info = new System.IO.FileInfo(file);
			this.fileService.UploadHeader(info.Name);
		}
		private string MD5Stream(System.IO.Stream stream)
		{
			System.Security.Cryptography.MD5CryptoServiceProvider md5 = null;
			string resule = string.Empty;
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
			return resule;
		}
		private string SaveImage()
		{
			System.Threading.Monitor.Enter(this);
			string tmp = this.sessionService.DirHeader + "SnippingScreenTempFile.png";
			System.IO.FileStream fs = new System.IO.FileStream(tmp, System.IO.FileMode.Create);
			new PngBitmapEncoder
			{
				Frames = 
				{
					BitmapFrame.Create((BitmapSource)this.img110.Source)
				}
			}.Save(fs);
			fs.Position = 0L;
			string md5 = this.MD5Stream(fs);
			fs.Flush();
			fs.Close();
			string file = "";
			if (md5 != string.Empty)
			{
				file = this.sessionService.DirHeader + md5 + ".png";
				this.ThumbnailImageProcessor(tmp, file);
				System.IO.File.Delete(tmp);
			}
			System.Threading.Monitor.Exit(this);
			return file;
		}
		private void ThumbnailImageProcessor(string fullName, string md5FullName)
		{
			try
			{
				System.Drawing.Image image = System.Drawing.Image.FromFile(fullName);
				if (image != null)
				{
					image = this.GetReducedImage(image, 110, 110);
					if (!System.IO.File.Exists(md5FullName))
					{
						System.IO.FileStream fs = new System.IO.FileStream(md5FullName, System.IO.FileMode.Create);
						image.Save(fs, ImageFormat.Png);
						fs.Position = 0L;
						fs.Flush();
						fs.Close();
						image.Dispose();
					}
				}
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		private System.Drawing.Image GetReducedImage(System.Drawing.Image image, int width, int height)
		{
			System.Drawing.Image result;
			if (image != null)
			{
				try
				{
					System.Drawing.Image.GetThumbnailImageAbort callb = new System.Drawing.Image.GetThumbnailImageAbort(this.ThumbnailCallback);
					System.Drawing.Image reducedImage = image.GetThumbnailImage(width, height, callb, System.IntPtr.Zero);
					image.Dispose();
					result = reducedImage;
					return result;
				}
				catch (System.Exception)
				{
				}
			}
			result = null;
			return result;
		}
		private bool ThumbnailCallback()
		{
			return false;
		}
		public System.Drawing.Image BitmapImageToBitmap(BitmapImage bitmapImage)
		{
			System.IO.MemoryStream ms = null;
			System.Drawing.Image result;
			try
			{
				ms = new System.IO.MemoryStream();
				ms = (bitmapImage.StreamSource as System.IO.MemoryStream);
				ms.Position = 0L;
				result = System.Drawing.Image.FromStream(ms);
				return result;
			}
			catch (System.Exception e)
			{
				ServiceUtil.Instance.Logger.Error(e.ToString());
				if (ms != null)
				{
					ms.Dispose();
				}
			}
			result = null;
			return result;
		}
		public BitmapImage BitmapToBitmapImage(System.Drawing.Image bit)
		{
			BitmapImage result;
			if (bit == null)
			{
				result = null;
			}
			else
			{
				BitmapImage bi = null;
				System.IO.MemoryStream ms = null;
				try
				{
					bi = new BitmapImage();
					ms = new System.IO.MemoryStream();
					bi.BeginInit();
					bi.StreamSource = ms;
					bit.Save(bi.StreamSource, ImageFormat.Gif);
					bi.EndInit();
				}
				catch
				{
					if (bi != null)
					{
						bi = null;
					}
					if (ms != null)
					{
						ms.Dispose();
					}
				}
				result = bi;
			}
			return result;
		}
		public void CacheImageHandler(double x, double y, double width, double height)
		{
			try
			{
				if (width > 0.0 && base.Height > 0.0)
				{
					int pointX = (int)(x * this.imageScale * this.pixelScale);
					int pointY = (int)(y * this.imageScale * this.pixelScale);
					int imageWidth = (int)(width * this.imageScale * this.pixelScale);
					int imageHeight = (int)(height * this.imageScale * this.pixelScale);
					if (pointX + imageWidth > ((BitmapImage)this.img.Source).PixelWidth)
					{
						imageWidth = ((BitmapImage)this.img.Source).PixelWidth - pointX;
						imageHeight = imageWidth;
					}
					if (pointY + imageHeight > ((BitmapImage)this.img.Source).PixelHeight)
					{
						imageHeight = ((BitmapImage)this.img.Source).PixelHeight - pointY;
						imageWidth = imageHeight;
					}
					CroppedBitmap cb = new CroppedBitmap((BitmapImage)this.img.Source, new Int32Rect(pointX, pointY, imageWidth, imageHeight));
					this.img40.Source = cb;
					this.img110.Source = cb;
				}
			}
			catch (System.Exception e)
			{
				this.logger.Error(e.ToString());
			}
		}
		private void Window_MouseMove(object sender, MouseEventArgs e)
		{
			try
			{
				if (this.mouseDropDirection != -1)
				{
					this.ChangeImageCanvasSize(this.mouseDropDirection, e);
					this.CrossBorderProcessor();
				}
				else
				{
					int direction = this.CheckMousrPoint(e);
					this.ChangeMouseCursors(direction);
				}
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		private void Window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.mouseDropDirection = this.CheckMousrPoint(e);
		}
		private void Window_MouseUp(object sender, MouseButtonEventArgs e)
		{
			if (this.mouseDropDirection != -1)
			{
				this.CacheImageHandler(this.imageCanvas.Margin.Left - this.imageCanvas.MoveArea.Left, this.imageCanvas.Margin.Top - this.imageCanvas.MoveArea.Top, this.imageCanvas.Width, this.imageCanvas.Height);
			}
			this.mouseDropDirection = -1;
			this.ChangeMouseCursors(this.mouseDropDirection);
		}
		private void ChangeMouseCursors(int direction)
		{
			switch (direction)
			{
			case -1:
				base.Cursor = Cursors.Arrow;
				break;
			case 1:
				base.Cursor = Cursors.SizeNWSE;
				break;
			case 2:
				base.Cursor = Cursors.SizeNESW;
				break;
			case 3:
				base.Cursor = Cursors.SizeNWSE;
				break;
			case 4:
				base.Cursor = Cursors.SizeNESW;
				break;
			}
		}
		private void CrossBorderProcessor()
		{
			for (int i = 0; i < 3; i++)
			{
				if (this.imageCanvas.Margin.Top + this.imageCanvas.Height > this.imageCanvas.MoveArea.Bottom)
				{
					this.imageCanvas.Margin = new Thickness(this.imageCanvas.Margin.Left, this.imageCanvas.Margin.Top, this.imageCanvas.Margin.Right, this.imageCanvas.MoveArea.Bottom);
					this.imageCanvas.Height = this.imageCanvas.MoveArea.Bottom - this.imageCanvas.Margin.Top;
					this.imageCanvas.Width = this.imageCanvas.Height;
				}
				else
				{
					if (this.imageCanvas.Margin.Left + this.imageCanvas.Width > this.imageCanvas.MoveArea.Right)
					{
						this.imageCanvas.Margin = new Thickness(this.imageCanvas.Margin.Left, this.imageCanvas.Margin.Top, this.imageCanvas.MoveArea.Right, this.imageCanvas.Margin.Bottom);
						this.imageCanvas.Width = this.imageCanvas.MoveArea.Right - this.imageCanvas.Margin.Left;
						this.imageCanvas.Height = this.imageCanvas.Width;
					}
					else
					{
						if (this.imageCanvas.Margin.Left < this.imageCanvas.MoveArea.Left)
						{
							this.imageCanvas.Margin = new Thickness(this.imageCanvas.MoveArea.Left, this.imageCanvas.Margin.Top, this.imageCanvas.Margin.Right, this.imageCanvas.Margin.Bottom);
							this.imageCanvas.Width = this.imageCanvas.Margin.Right - this.imageCanvas.MoveArea.Left;
							this.imageCanvas.Height = this.imageCanvas.Width;
						}
						else
						{
							if (this.imageCanvas.Margin.Top >= this.imageCanvas.MoveArea.Top)
							{
								break;
							}
							this.imageCanvas.Margin = new Thickness(this.imageCanvas.Margin.Left, this.imageCanvas.MoveArea.Top, this.imageCanvas.Margin.Right, this.imageCanvas.Margin.Bottom);
							this.imageCanvas.Height = this.imageCanvas.Margin.Bottom - this.imageCanvas.MoveArea.Top;
							this.imageCanvas.Width = this.imageCanvas.Height;
						}
					}
				}
			}
		}
		private int CheckMousrPoint(MouseEventArgs e)
		{
			int result;
			try
			{
				double x = e.GetPosition(this.imageCanvas).X;
				double y = e.GetPosition(this.imageCanvas).Y;
				System.Windows.Point mousePoint = new System.Windows.Point(x, y);
				if (this.imageCanvas.LeftTop.Contains(mousePoint))
				{
					result = 1;
					return result;
				}
				if (this.imageCanvas.RightTop.Contains(mousePoint))
				{
					result = 2;
					return result;
				}
				if (this.imageCanvas.RightBottom.Contains(mousePoint))
				{
					result = 3;
					return result;
				}
				if (this.imageCanvas.LeftBottom.Contains(mousePoint))
				{
					result = 4;
					return result;
				}
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
			result = -1;
			return result;
		}
		private void ChangeImageCanvasSize(int direction, MouseEventArgs e)
		{
			try
			{
				double x = e.GetPosition(this.imageCanvas).X;
				double y = e.GetPosition(this.imageCanvas).Y;
				System.Windows.Point mousePoint = new System.Windows.Point(x, y);
				double tempWidth = x;
				if (direction == 1)
				{
					double left = this.imageCanvas.Margin.Left;
					this.imageCanvas.Margin = new Thickness(this.imageCanvas.Margin.Left + x, this.imageCanvas.Margin.Top + y, left + this.imageCanvas.Width, this.imageCanvas.Margin.Bottom);
					this.imageCanvas.Width = this.imageCanvas.Margin.Right - this.imageCanvas.Margin.Left;
					this.imageCanvas.Height = this.imageCanvas.Width;
				}
				else
				{
					if (direction == 2)
					{
						this.imageCanvas.Margin = new Thickness(this.imageCanvas.Margin.Left, this.imageCanvas.Margin.Top + y, this.imageCanvas.Margin.Left + x, this.imageCanvas.Margin.Bottom);
						this.imageCanvas.Width = this.imageCanvas.Margin.Right - this.imageCanvas.Margin.Left;
						this.imageCanvas.Height = this.imageCanvas.Width;
					}
					else
					{
						if (direction == 3)
						{
							this.imageCanvas.Margin = new Thickness(this.imageCanvas.Margin.Left, this.imageCanvas.Margin.Top, this.imageCanvas.Margin.Left + this.imageCanvas.Width, this.imageCanvas.Margin.Top + this.imageCanvas.Height);
							this.imageCanvas.Width = tempWidth;
							this.imageCanvas.Height = tempWidth;
						}
						else
						{
							if (direction == 4)
							{
								this.imageCanvas.Margin = new Thickness(this.imageCanvas.Margin.Left + x, this.imageCanvas.Margin.Top, this.imageCanvas.Margin.Right, this.imageCanvas.Margin.Top + y);
								this.imageCanvas.Width = this.imageCanvas.Margin.Bottom - this.imageCanvas.Margin.Top;
								this.imageCanvas.Height = this.imageCanvas.Width;
							}
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				this.logger.Error(ex.ToString());
			}
		}
		private void MinHandler(object sender, RoutedEventArgs e)
		{
			base.WindowState = WindowState.Minimized;
		}
		private void CloseHandler(object sender, RoutedEventArgs e)
		{
			base.Close();
		}
		private void DragMoveHandler(object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				base.DragMove();
			}
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/commons/userheaderwindow.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.userHeaderWindow = (UserHeaderWindow)target;
        //        this.userHeaderWindow.MouseMove += new MouseEventHandler(this.Window_MouseMove);
        //        this.userHeaderWindow.MouseDown += new MouseButtonEventHandler(this.Window_MouseDown);
        //        this.userHeaderWindow.MouseUp += new MouseButtonEventHandler(this.Window_MouseUp);
        //        break;
        //    case 2:
        //        this.OuterBorder = (Border)target;
        //        break;
        //    case 3:
        //        this.InnerBorder = (Border)target;
        //        break;
        //    case 4:
        //        this.topBar = (StatusBar)target;
        //        this.topBar.MouseDown += new MouseButtonEventHandler(this.DragMoveHandler);
        //        break;
        //    case 5:
        //        this.btnMin = (ImageButton)target;
        //        this.btnMin.Click += new RoutedEventHandler(this.MinHandler);
        //        break;
        //    case 6:
        //        this.btnClose = (ImageButton)target;
        //        this.btnClose.Click += new RoutedEventHandler(this.CloseHandler);
        //        break;
        //    case 7:
        //        this.leftMenuBorder = (Border)target;
        //        break;
        //    case 8:
        //        this.img40 = (System.Windows.Controls.Image)target;
        //        break;
        //    case 9:
        //        this.img110 = (System.Windows.Controls.Image)target;
        //        break;
        //    case 10:
        //        this.btnSelectFile = (Button)target;
        //        this.btnSelectFile.Click += new RoutedEventHandler(this.btnSelectFile_Click);
        //        break;
        //    case 11:
        //        this.ImageBox = (Canvas)target;
        //        break;
        //    case 12:
        //        this.img = (System.Windows.Controls.Image)target;
        //        break;
        //    case 13:
        //        this.rectangleBox = (System.Windows.Shapes.Rectangle)target;
        //        break;
        //    case 14:
        //        ((Button)target).Click += new RoutedEventHandler(this.Sure_Click);
        //        break;
        //    case 15:
        //        ((Button)target).Click += new RoutedEventHandler(this.Cancel_Click);
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
