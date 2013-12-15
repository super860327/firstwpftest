using IDKin.IM.Theme.Helper;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
namespace IDKin.IM.Windows.View.Commons
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public partial class PromptWindow : Window//, IComponentConnector
	{
		public PromptWindowEnter Enter;
        //internal Label lblTitle;
        //internal TextBox txtContext;
        //internal Button btnEnter;
        //internal Button btnCancel;
        //private bool _contentLoaded;
		public PromptWindow()
		{
			ThemeSwitcher.LoadSkin(ThemeEnum.Aero, this);
			this.InitializeComponent();
			this.txtContext.Focus();
		}
		private void btnEnter_Click(object sender, RoutedEventArgs e)
		{
			if (this.txtContext.Text.Length > 200)
			{
				MessageBox.Show("内容只能在100个字以内!", "提示", MessageBoxButton.OK, MessageBoxImage.Exclamation);
			}
			else
			{
				if (this.txtContext.Text.Length == 0)
				{
					MessageBox.Show("内容不能为空!", "提示", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				}
				else
				{
					base.Close();
					if (this.Enter != null)
					{
						this.Enter(this.txtContext.Text);
					}
				}
			}
		}
		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{
			base.Close();
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/commons/promptwindow.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.lblTitle = (Label)target;
        //        break;
        //    case 2:
        //        this.txtContext = (TextBox)target;
        //        break;
        //    case 3:
        //        this.btnEnter = (Button)target;
        //        this.btnEnter.Click += new RoutedEventHandler(this.btnEnter_Click);
        //        break;
        //    case 4:
        //        this.btnCancel = (Button)target;
        //        this.btnCancel.Click += new RoutedEventHandler(this.btnCancel_Click);
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
