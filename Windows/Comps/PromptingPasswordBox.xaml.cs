using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
namespace IDKin.IM.Windows.Comps
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class PromptingPasswordBox : UserControl//, IComponentConnector
	{
		private string password = "";
		private string prompt = "";
		private Brush promptColor = Brushes.LightGray;
		//internal PasswordBox txtPassword;
		//internal TextBlock tbkPrompt;
        ////private bool _contentLoaded;
		public string Password
		{
			get
			{
				return this.txtPassword.Password;
			}
			set
			{
				this.password = value;
			}
		}
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
		public PromptingPasswordBox()
		{
			this.InitializeComponent();
			this.txtPassword.Loaded += new RoutedEventHandler(this.txtPassword_Loaded);
			this.txtPassword.GotFocus += new RoutedEventHandler(this.GotFocusHandler);
			this.txtPassword.LostFocus += new RoutedEventHandler(this.LostFocusHandler);
		}
		private void txtPassword_Loaded(object sender, RoutedEventArgs e)
		{
			if (this.tbkPrompt != null)
			{
				if (this.txtPassword.Password.Trim().Length != 0)
				{
					this.tbkPrompt.Visibility = Visibility.Collapsed;
				}
				else
				{
					this.tbkPrompt.Visibility = Visibility.Visible;
				}
				this.tbkPrompt.TextAlignment = TextAlignment.Left;
				this.tbkPrompt.TextWrapping = TextWrapping.NoWrap;
				this.tbkPrompt.TextTrimming = TextTrimming.WordEllipsis;
				this.tbkPrompt.Text = this.prompt;
				this.tbkPrompt.Foreground = this.promptColor;
			}
		}
		private void GotFocusHandler(object sender, RoutedEventArgs e)
		{
			if (this.tbkPrompt != null)
			{
				this.tbkPrompt.Visibility = Visibility.Collapsed;
			}
		}
		private void LostFocusHandler(object sender, RoutedEventArgs e)
		{
			if (this.tbkPrompt != null)
			{
				if (this.txtPassword.Password.Trim().Length != 0)
				{
					this.tbkPrompt.Visibility = Visibility.Collapsed;
				}
				else
				{
					this.tbkPrompt.Visibility = Visibility.Visible;
				}
			}
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/promptingpasswordbox.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.txtPassword = (PasswordBox)target;
        //        break;
        //    case 2:
        //        this.tbkPrompt = (TextBlock)target;
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
