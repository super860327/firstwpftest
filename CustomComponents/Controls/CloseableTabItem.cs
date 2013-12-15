using System;
using System.Windows;
using System.Windows.Controls;
namespace IDKin.IM.CustomComponents.Controls
{
	public class CloseableTabItem : TabItem
	{
		public enum Langs
		{
			zh_CN = 1,
			en_US
		}
		public static readonly RoutedEvent CloseTabEvent;
		private CloseableTabItem.Langs lang;
		public event RoutedEventHandler CloseTab
		{
			add
			{
				base.AddHandler(CloseableTabItem.CloseTabEvent, value);
			}
			remove
			{
				base.RemoveHandler(CloseableTabItem.CloseTabEvent, value);
			}
		}
		public CloseableTabItem.Langs Lang
		{
			get
			{
				return this.lang;
			}
			set
			{
				this.lang = value;
			}
		}
		static CloseableTabItem()
		{
			CloseableTabItem.CloseTabEvent = EventManager.RegisterRoutedEvent("CloseTab", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CloseableTabItem));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(CloseableTabItem), new FrameworkPropertyMetadata(typeof(CloseableTabItem)));
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			Button closeButton = base.GetTemplateChild("PART_Close") as Button;
			if (closeButton != null)
			{
				switch (this.lang)
				{
				case CloseableTabItem.Langs.zh_CN:
					closeButton.ToolTip = "关闭";
					break;
				case CloseableTabItem.Langs.en_US:
					closeButton.ToolTip = "close";
					break;
				default:
					closeButton.ToolTip = "关闭";
					break;
				}
				closeButton.Click += new RoutedEventHandler(this.closeButton_Click);
			}
		}
		private void closeButton_Click(object sender, RoutedEventArgs e)
		{
			base.RaiseEvent(new RoutedEventArgs(CloseableTabItem.CloseTabEvent, this));
		}
	}
}
