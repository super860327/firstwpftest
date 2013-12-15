using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
namespace IDKin.IM.Windows.View.Commons
{
	public class Alert
	{
		protected MessageBoxResult MessageBoxResult = MessageBoxResult.None;
		private static string resourcesPath;
		private static Uri iconError;
		private static Uri iconExclamation;
		private static Uri iconInformation;
		private static Uri iconQuestion;
		protected AlertWindow Container
		{
			get;
			private set;
		}
		protected AlertData ContainerData
		{
			get;
			private set;
		}
		public static Uri IconError
		{
			get
			{
				return Alert.iconError;
			}
			set
			{
				Alert.iconError = value;
			}
		}
		public static Uri IconExclamation
		{
			get
			{
				return Alert.iconExclamation;
			}
			set
			{
				Alert.iconExclamation = value;
			}
		}
		public static Uri IconInformation
		{
			get
			{
				return Alert.iconInformation;
			}
			set
			{
				Alert.iconInformation = value;
			}
		}
		public static Uri IconQuestion
		{
			get
			{
				return Alert.iconQuestion;
			}
			set
			{
				Alert.iconQuestion = value;
			}
		}
		static Alert()
		{
			Alert.resourcesPath = "pack://application:,,,/IDKin.IM.Windows;component/Resources/MessageBoxIcon";
			Alert.iconInformation = new Uri(System.IO.Path.Combine(Alert.resourcesPath, "infomation.png"));
			Alert.iconExclamation = new Uri(System.IO.Path.Combine(Alert.resourcesPath, "warning.png"));
			Alert.iconError = new Uri(System.IO.Path.Combine(Alert.resourcesPath, "error.png"));
			Alert.iconQuestion = new Uri(System.IO.Path.Combine(Alert.resourcesPath, "question.png"));
		}
		public static MessageBoxResult Show(string messageText)
		{
			return Alert.Show(messageText, string.Empty, MessageBoxButton.OK);
		}
		public static MessageBoxResult Show(string messageText, string caption)
		{
			return Alert.Show(messageText, caption, MessageBoxButton.OK);
		}
		public static MessageBoxResult Show(string messageText, string caption, MessageBoxButton button)
		{
			return Alert.ShowCore(messageText, caption, button, MessageBoxImage.None);
		}
		public static MessageBoxResult Show(string messageText, string caption, MessageBoxButton button, MessageBoxImage image)
		{
			return Alert.ShowCore(messageText, caption, button, image);
		}
		private static MessageBoxResult ShowCore(string messageText, string caption, MessageBoxButton button, MessageBoxImage image)
		{
			Alert msgBox = new Alert();
			msgBox.InitializeAlert(messageText, caption, button, image);
			msgBox.Show();
			return msgBox.MessageBoxResult;
		}
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Button btn = e.Source as Button;
			MessageBoxResult messageResult = (MessageBoxResult)btn.Tag;
			this.MessageBoxResult = messageResult;
			this.Close();
		}
		protected void Show()
		{
			this.Container.ShowDialog();
		}
		protected void InitializeAlert(string text, string caption, MessageBoxButton button, MessageBoxImage image)
		{
			this.Container = this.CreateContainer();
			this.ContainerData = new AlertData();
			string okLabel = "Ok";
			string cancelLabel = "Cancel";
			string yesLabel = "Yes";
			string noLabel = "No";
			if (System.Threading.Thread.CurrentThread.CurrentCulture.Equals(System.Globalization.CultureInfo.GetCultureInfo("zh-CN")))
			{
				okLabel = "确定";
				cancelLabel = "取消";
				yesLabel = "是";
				noLabel = "否";
			}
			this.ContainerData.Message = text;
			this.ContainerData.Caption = caption;
			this.SetButton(button, okLabel, cancelLabel, yesLabel, noLabel);
			this.Container.imgIcon.Source = this.SetImageSource(image);
			foreach (UIElement element in this.Container.buttonsStackPanel.Children)
			{
				Button btn = element as Button;
				btn.Click += new RoutedEventHandler(this.Button_Click);
			}
			this.Container.DataContext = this.ContainerData;
		}
		private AlertWindow CreateContainer()
		{
			AlertWindow newWindow = new AlertWindow();
			FrameworkElement owner = Alert.ResolveOwner();
			if (owner != null)
			{
				newWindow.Owner = Window.GetWindow(owner);
			}
			newWindow.ShowInTaskbar = false;
			newWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
			return newWindow;
		}
		private static FrameworkElement ResolveOwner()
		{
			FrameworkElement owner = null;
			if (Application.Current != null)
			{
				foreach (Window w in Application.Current.Windows)
				{
					if (w.IsActive)
					{
						owner = w;
						break;
					}
				}
			}
			return owner;
		}
		private void SetButton(MessageBoxButton button, string okLabel, string cancelLabel, string yesLabel, string noLabel)
		{
			switch (button)
			{
			case MessageBoxButton.OK:
				this.Container.btnOk.Content = okLabel;
				this.Container.btnOk.Tag = MessageBoxResult.OK;
				this.Container.btnOk.Visibility = Visibility.Visible;
				break;
			case MessageBoxButton.OKCancel:
				this.Container.btnOk.Content = okLabel;
				this.Container.btnOk.Tag = MessageBoxResult.OK;
				this.Container.btnCancel.Content = cancelLabel;
				this.Container.btnCancel.Tag = MessageBoxResult.Cancel;
				this.Container.btnOk.Visibility = Visibility.Visible;
				this.Container.btnCancel.Visibility = Visibility.Visible;
				break;
			case MessageBoxButton.YesNoCancel:
				this.Container.btnYes.Content = yesLabel;
				this.Container.btnYes.Tag = MessageBoxResult.Yes;
				this.Container.btnNo.Content = noLabel;
				this.Container.btnNo.Tag = MessageBoxResult.No;
				this.Container.btnCancel.Content = cancelLabel;
				this.Container.btnCancel.Tag = MessageBoxResult.Cancel;
				this.Container.btnYes.Visibility = Visibility.Visible;
				this.Container.btnNo.Visibility = Visibility.Visible;
				this.Container.btnCancel.Visibility = Visibility.Visible;
				break;
			case MessageBoxButton.YesNo:
				this.Container.btnYes.Content = yesLabel;
				this.Container.btnYes.Tag = MessageBoxResult.Yes;
				this.Container.btnNo.Content = noLabel;
				this.Container.btnNo.Tag = MessageBoxResult.No;
				this.Container.btnYes.Visibility = Visibility.Visible;
				this.Container.btnNo.Visibility = Visibility.Visible;
				break;
			}
		}
		private ImageSource SetImageSource(MessageBoxImage image)
		{
			BitmapImage iconImage = new BitmapImage();
			iconImage.BeginInit();
			if (image <= MessageBoxImage.Hand)
			{
				if (image != MessageBoxImage.None)
				{
					if (image == MessageBoxImage.Hand)
					{
						iconImage.UriSource = Alert.iconError;
					}
				}
				else
				{
					iconImage.UriSource = new Uri("", UriKind.RelativeOrAbsolute);
					this.Container.imgIcon.Visibility = Visibility.Collapsed;
				}
			}
			else
			{
				if (image != MessageBoxImage.Question)
				{
					if (image != MessageBoxImage.Exclamation)
					{
						if (image == MessageBoxImage.Asterisk)
						{
							iconImage.UriSource = Alert.iconInformation;
						}
					}
					else
					{
						iconImage.UriSource = Alert.iconExclamation;
					}
				}
				else
				{
					iconImage.UriSource = Alert.iconQuestion;
				}
			}
			iconImage.DecodePixelWidth = 32;
			iconImage.EndInit();
			return iconImage;
		}
		private void Close()
		{
			this.Container.Close();
			this.Container = null;
		}
	}
}
