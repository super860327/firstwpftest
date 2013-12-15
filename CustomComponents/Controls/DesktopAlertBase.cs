using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
namespace IDKin.IM.CustomComponents.Controls
{
	public class DesktopAlertBase : Window
	{
		private DoubleAnimation fadeInAnimation;
		private DoubleAnimation fadeOutAnimation;
		private DispatcherTimer activeTimer;
		private bool isAutoClose = true;
		private bool isDragMove = true;
		public bool IsAutoClose
		{
			get
			{
				return this.isAutoClose;
			}
			set
			{
				this.isAutoClose = value;
			}
		}
		public bool IsDragMove
		{
			get
			{
				return this.isDragMove;
			}
			set
			{
				this.isDragMove = value;
			}
		}
		static DesktopAlertBase()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DesktopAlertBase), new FrameworkPropertyMetadata(typeof(DesktopAlertBase)));
		}
		public DesktopAlertBase()
		{
			base.Visibility = Visibility.Visible;
			base.Width = 350.0;
			base.Height = 75.0;
			base.ShowInTaskbar = false;
			base.WindowStyle = WindowStyle.None;
			base.ResizeMode = ResizeMode.NoResize;
			base.Topmost = true;
			base.ShowActivated = false;
			base.AllowsTransparency = true;
			base.BorderThickness = new Thickness(1.0);
			base.BorderBrush = Brushes.Transparent;
			base.Background = Brushes.Transparent;
			this.fadeInAnimation = new DoubleAnimation();
			this.fadeInAnimation.From = new double?(0.0);
			this.fadeInAnimation.To = new double?(1.0);
			this.fadeInAnimation.Duration = new Duration(TimeSpan.Parse("0:0:0.25"));
			this.fadeOutAnimation = new DoubleAnimation();
			this.fadeOutAnimation.To = new double?(0.0);
			this.fadeOutAnimation.Duration = new Duration(TimeSpan.Parse("0:0:0.25"));
			base.Loaded += new RoutedEventHandler(this.DesktopAlertBase_Loaded);
		}
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonDown(e);
			if (this.isDragMove)
			{
				base.DragMove();
			}
		}
		public override void OnApplyTemplate()
		{
			ButtonBase closeButton = base.Template.FindName("PART_CloseButton", this) as ButtonBase;
			if (closeButton != null)
			{
				if (Thread.CurrentThread.CurrentCulture.Equals(CultureInfo.GetCultureInfo("zh-CN")))
				{
					closeButton.ToolTip = "关闭";
				}
				else
				{
					closeButton.ToolTip = "close";
				}
				closeButton.Click += new RoutedEventHandler(this.closeButton_Click);
			}
		}
		private void DesktopAlertBase_Loaded(object sender, RoutedEventArgs e)
		{
			Rect workAreaRectangle = SystemParameters.WorkArea;
			base.Left = workAreaRectangle.Right - base.Width - base.BorderThickness.Right;
			base.Top = workAreaRectangle.Bottom - base.Height - base.BorderThickness.Bottom;
			this.fadeInAnimation.Completed += new EventHandler(this.fadeInAnimation_Completed);
			base.BeginAnimation(UIElement.OpacityProperty, this.fadeInAnimation);
		}
		private void fadeInAnimation_Completed(object sender, EventArgs e)
		{
			this.activeTimer = new DispatcherTimer();
			this.activeTimer.Interval = TimeSpan.Parse("0:0:10");
			this.activeTimer.Tick += delegate
			{
				this.FadeOut();
			};
			if (this.isAutoClose)
			{
				this.activeTimer.Start();
			}
		}
		private void FadeOut()
		{
			this.fadeOutAnimation.Completed += delegate
			{
				base.Close();
			};
			base.BeginAnimation(UIElement.OpacityProperty, this.fadeOutAnimation, HandoffBehavior.SnapshotAndReplace);
		}
		private void closeButton_Click(object sender, RoutedEventArgs e)
		{
			this.activeTimer.Stop();
			this.FadeOut();
		}
	}
}
