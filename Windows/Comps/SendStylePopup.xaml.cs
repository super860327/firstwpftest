using IDKin.IM.Windows.Properties;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
namespace IDKin.IM.Windows.Comps
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class SendStylePopup : Popup//, IComponentConnector
	{
		//internal Menu sendTypeMenu;
		//internal MenuItem enterMenu;
		//internal MenuItem ctrlMenu;
        ////private bool _contentLoaded;
		public SendStylePopup()
		{
			this.InitializeComponent();
			if (Settings.Default.SystemSetup_HotKey_SendMessage == 2)
			{
				this.enterMenu.IsChecked = true;
			}
			else
			{
				this.ctrlMenu.IsChecked = true;
			}
		}
		private void enterMenu_Click(object sender, RoutedEventArgs e)
		{
			if (!this.enterMenu.IsChecked)
			{
				this.ctrlMenu.IsChecked = false;
				this.enterMenu.IsChecked = true;
				Settings.Default.SystemSetup_HotKey_SendMessage = 2;
				Settings.Default.Save();
			}
		}
		private void ctrlMenu_Click(object sender, RoutedEventArgs e)
		{
			if (!this.ctrlMenu.IsChecked)
			{
				this.ctrlMenu.IsChecked = true;
				this.enterMenu.IsChecked = false;
				Settings.Default.SystemSetup_HotKey_SendMessage = 1;
				Settings.Default.Save();
			}
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/sendstylepopup.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.sendTypeMenu = (Menu)target;
        //        break;
        //    case 2:
        //        this.enterMenu = (MenuItem)target;
        //        this.enterMenu.Click += new RoutedEventHandler(this.enterMenu_Click);
        //        break;
        //    case 3:
        //        this.ctrlMenu = (MenuItem)target;
        //        this.ctrlMenu.Click += new RoutedEventHandler(this.ctrlMenu_Click);
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
