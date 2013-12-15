using IDKin.IM.Windows.Properties;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
namespace IDKin.IM.Windows.View
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public partial class UsageGuideWindow : Window//, IComponentConnector
	{
        //internal Button btnOK;
        //internal Button btnCancel;
        //private bool _contentLoaded;
		public static bool ShowUsageGuideWindow
		{
			get
			{
				return Settings.Default.ShowUsageGuideWindow;
			}
		}
		public UsageGuideWindow()
		{
			this.InitializeComponent();
		}
		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{
			base.Close();
		}
		private void btnOK_Click(object sender, RoutedEventArgs e)
		{
			new UsageGuideWindowSub
			{
				Owner = base.Owner
			}.ShowDialog();
			base.Close();
		}
		private void Window_Closed(object sender, System.EventArgs e)
		{
			Settings.Default.ShowUsageGuideWindow = false;
			Settings.Default.Save();
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/view/usageguidewindow.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        ((UsageGuideWindow)target).Closed += new System.EventHandler(this.Window_Closed);
        //        break;
        //    case 2:
        //        this.btnOK = (Button)target;
        //        this.btnOK.Click += new RoutedEventHandler(this.btnOK_Click);
        //        break;
        //    case 3:
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
