using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
namespace IDKin.IM.CustomComponents.Controls
{
	public class PromptingTextBox : TextBox
	{
		private string prompt = "";
		private Brush promptColor = Brushes.LightGray;
		public string Prompt
		{
			get
			{
				return this.prompt;
			}
			set
			{
				this.prompt = value;
			}
		}
		public Brush PromptColor
		{
			get
			{
				return this.promptColor;
			}
			set
			{
				this.promptColor = value;
			}
		}
		static PromptingTextBox()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(PromptingTextBox), new FrameworkPropertyMetadata(typeof(PromptingTextBox)));
		}
		public PromptingTextBox()
		{
			base.GotFocus += new RoutedEventHandler(this.GotFocusHandler);
			base.LostFocus += new RoutedEventHandler(this.LostFocusHandler);
		}
		private void GotFocusHandler(object sender, RoutedEventArgs e)
		{
			TextBlock tbkPrompt = base.GetTemplateChild("PART_Prompt") as TextBlock;
			if (tbkPrompt != null)
			{
				tbkPrompt.Visibility = Visibility.Collapsed;
			}
		}
		private void LostFocusHandler(object sender, RoutedEventArgs e)
		{
			TextBlock tbkPrompt = base.GetTemplateChild("PART_Prompt") as TextBlock;
			if (tbkPrompt != null)
			{
				if (base.Text.Trim().Length != 0)
				{
					tbkPrompt.Visibility = Visibility.Collapsed;
				}
				else
				{
					tbkPrompt.Visibility = Visibility.Visible;
				}
			}
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			TextBlock tbkPrompt = base.GetTemplateChild("PART_Prompt") as TextBlock;
			if (tbkPrompt != null)
			{
				if (base.Text.Trim().Length != 0)
				{
					tbkPrompt.Visibility = Visibility.Collapsed;
				}
				else
				{
					tbkPrompt.Visibility = Visibility.Visible;
				}
				tbkPrompt.TextAlignment = TextAlignment.Left;
				tbkPrompt.TextWrapping = TextWrapping.NoWrap;
				tbkPrompt.TextTrimming = TextTrimming.WordEllipsis;
				tbkPrompt.Text = this.prompt;
				tbkPrompt.Foreground = this.promptColor;
			}
		}
	}
}
