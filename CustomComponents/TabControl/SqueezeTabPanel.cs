using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace IDKin.IM.CustomComponents.Controls.TabControl
{
	public class SqueezeTabPanel : Panel
	{
		private double rowHeight;
		private double scaleFactor;
		static SqueezeTabPanel()
		{
			KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(SqueezeTabPanel), new FrameworkPropertyMetadata(KeyboardNavigationMode.Once));
			KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(SqueezeTabPanel), new FrameworkPropertyMetadata(KeyboardNavigationMode.Cycle));
		}
		protected override Size MeasureOverride(Size availableSize)
		{
			double width = 0.0;
			this.rowHeight = 0.0;
			foreach (UIElement element in base.Children)
			{
				element.Measure(availableSize);
				Size size = this.GetDesiredSizeLessMargin(element);
				this.rowHeight = Math.Max(this.rowHeight, size.Height);
				width += size.Width;
			}
			if (width > availableSize.Width)
			{
				this.scaleFactor = availableSize.Width / width;
				width = 0.0;
				foreach (UIElement element in base.Children)
				{
					element.Measure(new Size(element.DesiredSize.Width * this.scaleFactor, availableSize.Height));
					width += element.DesiredSize.Width;
				}
			}
			else
			{
				this.scaleFactor = 1.0;
			}
			return new Size(width, this.rowHeight);
		}
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			Point point = default(Point);
			foreach (UIElement element in base.Children)
			{
				Size size = element.DesiredSize;
				Size size2 = this.GetDesiredSizeLessMargin(element);
				Thickness margin = (Thickness)element.GetValue(FrameworkElement.MarginProperty);
				double width = size2.Width;
				if (element.DesiredSize.Width != size2.Width)
				{
					width = arrangeSize.Width - point.X;
				}
				element.Arrange(new Rect(point, new Size(Math.Min(width, size2.Width), this.rowHeight)));
				double leftRightMargin = Math.Max(0.0, -(margin.Left + margin.Right));
				point.X += size.Width + leftRightMargin * this.scaleFactor;
			}
			return arrangeSize;
		}
		private Size GetDesiredSizeLessMargin(UIElement element)
		{
			Thickness margin = (Thickness)element.GetValue(FrameworkElement.MarginProperty);
			return new Size
			{
				Height = Math.Max(0.0, element.DesiredSize.Height - (margin.Top + margin.Bottom)),
				Width = Math.Max(0.0, element.DesiredSize.Width - (margin.Left + margin.Right))
			};
		}
	}
}
