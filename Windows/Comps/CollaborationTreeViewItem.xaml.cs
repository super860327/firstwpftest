using IDKin.IM.Core.Core;
using IDKin.IM.Protocol.Cooperation;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
namespace IDKin.IM.Windows.Comps
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class CollaborationTreeViewItem : UserControl//, IComponentConnector
	{
		private CooperationProject cooperationProject = null;
		private CooperationProjectWrapper cooperationProjectWrapper = null;
		//internal Image imgIcon;
		//internal TextBlock tbkHeader;
		//internal TextBlock tbkCompany;
        ////private bool _contentLoaded;
		public CollaborationTreeViewItem(CooperationProject cooperationProject)
		{
			this.InitializeComponent();
			this.cooperationProject = cooperationProject;
			this.cooperationProjectWrapper = new CooperationProjectWrapper(cooperationProject);
			if (this.cooperationProjectWrapper.IsSender)
			{
				this.imgIcon.Source = this.SetIcon("/IDKin.IM.Windows;component/Resources/Icon/senderIcon.png");
				this.imgIcon.Width = 16.0;
				this.imgIcon.Height = 16.0;
			}
			else
			{
				this.imgIcon.Source = this.SetIcon("/IDKin.IM.Windows;component/Resources/Icon/receiveIcon.png");
				this.imgIcon.Width = 16.0;
				this.imgIcon.Height = 16.0;
			}
			base.DataContext = this.cooperationProjectWrapper;
		}
		private BitmapImage SetIcon(string uri)
		{
			BitmapImage icon = new BitmapImage();
			icon.BeginInit();
			icon.UriSource = new Uri(uri, UriKind.RelativeOrAbsolute);
			icon.EndInit();
			return icon;
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/collaboration/collaborationtreeviewitem.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.imgIcon = (Image)target;
        //        break;
        //    case 2:
        //        this.tbkHeader = (TextBlock)target;
        //        break;
        //    case 3:
        //        this.tbkCompany = (TextBlock)target;
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
