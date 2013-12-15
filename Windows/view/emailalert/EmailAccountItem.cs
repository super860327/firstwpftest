using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
namespace IDKin.IM.Windows.View.EmailAlert
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class EmailAccountItem : ListBoxItem//, IComponentConnector
	{
        //internal TextBlock tbkAccount;
        //internal TextBlock tbkCount;
        //internal Button btnDelete;
        //private bool _contentLoaded;
		public event System.EventHandler ItemDelete;
		public EmailAccountItem(EmailAccountItemType type)
		{
			this.InitializeComponent();
			switch (type)
			{
			case EmailAccountItemType.Type1:
				this.btnDelete.Visibility = Visibility.Collapsed;
				break;
			case EmailAccountItemType.type2:
				this.tbkCount.Visibility = Visibility.Collapsed;
				break;
			}
			this.btnDelete.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(this.btnDelete_MouseLeftButtonDown);
		}
		private void btnDelete_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (this.ItemDelete != null)
			{
				base.IsSelected = true;
				this.ItemDelete(sender, null);
			}
		}
		public void AccoutValid()
		{
			this.tbkAccount.Foreground = Brushes.Black;
		}
		public void AccoutInvalid()
		{
			this.tbkAccount.Foreground = Brushes.Red;
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/view/emailalert/emailaccountitem.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.tbkAccount = (TextBlock)target;
        //        break;
        //    case 2:
        //        this.tbkCount = (TextBlock)target;
        //        break;
        //    case 3:
        //        this.btnDelete = (Button)target;
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
