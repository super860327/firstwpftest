using IDKin.IM.Windows.Comps.OA.CurrentWork;
using IDKin.IM.Windows.ViewModel;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
namespace IDKin.IM.Windows.Comps.OA
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public partial class OACurrentWorkControl : UserControl//, IComponentConnector
	{
		private bool canLoadData = false;
		//internal TabControl tbcDynamicWork;
		//internal TabItem tabItemWorkCooperation;
		//internal OAAllCurrentWork crwWorkCooperation;
		//internal TabItem tabItemInsideNotice;
		//internal OAAllCurrentWork crwInsideNotice;
		//internal TabItem tabItemProjectManagement;
		//internal OAAllCurrentWork crwProjectManagement;
		//internal TabItem tabItemDocumentManagement;
		//internal OAAllCurrentWork crwDocumentManagement;
		//internal TabItem tabItemWorkPlan;
		//internal OAAllCurrentWork crwWorkPlan;
		//internal TabItem tabItemInsideDiscussion;
		//internal OAAllCurrentWork crwInsideDiscussion;
		//internal TabItem tabItemSystemManagement;
		//internal OAAllCurrentWork crwSystemManagement;
        ////private bool _contentLoaded;
		public bool CanLoadData
		{
			get
			{
				return this.canLoadData;
			}
			set
			{
				this.canLoadData = value;
				if (this.canLoadData)
				{
					this.tbcDynamicWork_SelectionChanged(null, null);
				}
			}
		}
		public OACurrentWorkControl()
		{
			this.InitializeComponent();
		}
		private void tbcDynamicWork_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (this.CanLoadData)
			{
				if (this.tbcDynamicWork.SelectedIndex != -1)
				{
					TabItem ti = this.tbcDynamicWork.SelectedItem as TabItem;
					if (ti != null)
					{
						if (ti.Content != null)
						{
							OAAllCurrentWork oAAllCurrentWork = ti.Content as OAAllCurrentWork;
							if (oAAllCurrentWork != null)
							{
								AllBaseDynamicWorkObjViewModel vm = oAAllCurrentWork.AllBaseDynamicWorkObjViewModel;
								if (vm != null)
								{
									if (!vm.GetMoreDataCalled)
									{
										vm.GetMoreData();
									}
								}
							}
						}
					}
				}
			}
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/oa/oacurrentworkcontrol.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[System.Diagnostics.DebuggerNonUserCode]
        ////internal System.Delegate _CreateDelegate(System.Type delegateType, string handler)
        //{
        //    return System.Delegate.CreateDelegate(delegateType, this, handler);
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.tbcDynamicWork = (TabControl)target;
        //        this.tbcDynamicWork.SelectionChanged += new SelectionChangedEventHandler(this.tbcDynamicWork_SelectionChanged);
        //        break;
        //    case 2:
        //        this.tabItemWorkCooperation = (TabItem)target;
        //        break;
        //    case 3:
        //        this.crwWorkCooperation = (OAAllCurrentWork)target;
        //        break;
        //    case 4:
        //        this.tabItemInsideNotice = (TabItem)target;
        //        break;
        //    case 5:
        //        this.crwInsideNotice = (OAAllCurrentWork)target;
        //        break;
        //    case 6:
        //        this.tabItemProjectManagement = (TabItem)target;
        //        break;
        //    case 7:
        //        this.crwProjectManagement = (OAAllCurrentWork)target;
        //        break;
        //    case 8:
        //        this.tabItemDocumentManagement = (TabItem)target;
        //        break;
        //    case 9:
        //        this.crwDocumentManagement = (OAAllCurrentWork)target;
        //        break;
        //    case 10:
        //        this.tabItemWorkPlan = (TabItem)target;
        //        break;
        //    case 11:
        //        this.crwWorkPlan = (OAAllCurrentWork)target;
        //        break;
        //    case 12:
        //        this.tabItemInsideDiscussion = (TabItem)target;
        //        break;
        //    case 13:
        //        this.crwInsideDiscussion = (OAAllCurrentWork)target;
        //        break;
        //    case 14:
        //        this.tabItemSystemManagement = (TabItem)target;
        //        break;
        //    case 15:
        //        this.crwSystemManagement = (OAAllCurrentWork)target;
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	
    }
}
