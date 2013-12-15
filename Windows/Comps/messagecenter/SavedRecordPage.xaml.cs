using IDKin.IM.Theme.Helper;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
namespace IDKin.IM.Windows.Comps.MessageCenter
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public partial class SavedRecordPage : Page//, IComponentConnector
	{
		//internal ListView SavedList;
		//internal Button FirstPage;
		//internal Button PrePage;
		//internal TextBox textPage;
		//internal TextBlock tbkTotal;
		//internal Button NextPage;
		//internal Button LastPage;
		//internal FlowDocumentScrollViewer ViewMessageBoxViewer;
		//internal FlowDocument ViewMessageBox;
		//internal TableRowGroup trgMessageTable;
		//private bool _contentLoaded;
		public SavedRecordPage()
		{
			ThemeSwitcher.LoadSkin(ThemeEnum.Aero, this);
			this.InitializeComponent();
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/messagecenter/savedrecordpage.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.SavedList = (ListView)target;
        //        break;
        //    case 2:
        //        this.FirstPage = (Button)target;
        //        break;
        //    case 3:
        //        this.PrePage = (Button)target;
        //        break;
        //    case 4:
        //        this.textPage = (TextBox)target;
        //        break;
        //    case 5:
        //        this.tbkTotal = (TextBlock)target;
        //        break;
        //    case 6:
        //        this.NextPage = (Button)target;
        //        break;
        //    case 7:
        //        this.LastPage = (Button)target;
        //        break;
        //    case 8:
        //        this.ViewMessageBoxViewer = (FlowDocumentScrollViewer)target;
        //        break;
        //    case 9:
        //        this.ViewMessageBox = (FlowDocument)target;
        //        break;
        //    case 10:
        //        this.trgMessageTable = (TableRowGroup)target;
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	
    }
}
