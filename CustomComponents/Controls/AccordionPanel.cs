using System;
using System.Windows;
using System.Windows.Controls;
namespace IDKin.IM.CustomComponents.Controls
{
	public class AccordionPanel : Panel
	{
		public static readonly DependencyProperty ChildToFillProperty = DependencyProperty.Register("ChildToFill", typeof(UIElement), typeof(AccordionPanel), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure));
		public UIElement ChildToFill
		{
			get
			{
				return (UIElement)base.GetValue(AccordionPanel.ChildToFillProperty);
			}
			set
			{
				base.SetValue(AccordionPanel.ChildToFillProperty, value);
			}
		}
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			UIElementCollection internalChildren = base.InternalChildren;
			int count = internalChildren.Count;
			int childToFillIndex = (this.ChildToFill == null) ? (count - 1) : internalChildren.IndexOf(this.ChildToFill);
			double y = 0.0;
			Rect rectForFill = new Rect(0.0, 0.0, arrangeSize.Width, arrangeSize.Height);
			if (childToFillIndex != -1)
			{
				for (int i = 0; i < childToFillIndex + 1; i++)
				{
					UIElement element = internalChildren[i];
					if (element != null)
					{
						Size desiredSize = element.DesiredSize;
						Rect finalRect = new Rect(0.0, y, Math.Max(0.0, arrangeSize.Width), Math.Max(0.0, arrangeSize.Height - y));
						if (i < childToFillIndex)
						{
							finalRect.Height = desiredSize.Height;
							y += desiredSize.Height;
							element.Arrange(finalRect);
						}
						else
						{
							rectForFill = finalRect;
						}
					}
				}
				y = 0.0;
				for (int i = count - 1; i > childToFillIndex; i--)
				{
					UIElement element = internalChildren[i];
					if (element != null)
					{
						Size desiredSize = element.DesiredSize;
						Rect finalRect = new Rect(0.0, arrangeSize.Height - y - desiredSize.Height, Math.Max(0.0, arrangeSize.Width), Math.Max(0.0, desiredSize.Height));
						element.Arrange(finalRect);
						y += desiredSize.Height;
					}
				}
				rectForFill.Height -= y;
				base.InternalChildren[childToFillIndex].Arrange(rectForFill);
			}
			return arrangeSize;
		}
		protected override Size MeasureOverride(Size constraint)
		{
			UIElementCollection internalChildren = base.InternalChildren;
			double num = 0.0;
			double num2 = 0.0;
			double num3 = 0.0;
			double num4 = 0.0;
			int index = 0;
			int count = internalChildren.Count;
			while (index < count)
			{
				UIElement element = internalChildren[index];
				if (element != null)
				{
					Size availableSize = new Size(Math.Max(0.0, constraint.Width - num3), Math.Max(0.0, constraint.Height - num4));
					element.Measure(availableSize);
					Size desiredSize = element.DesiredSize;
					num = Math.Max(num, num3 + desiredSize.Width);
					num4 += desiredSize.Height;
				}
				index++;
			}
			num = Math.Max(num, num3);
			return new Size(num, Math.Max(num2, num4));
		}
	}
}
