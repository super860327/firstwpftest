using IDKin.IM.Data;
using IDKin.IM.Protocol.Account;
using IDKin.IM.Protocol.Commons;
using IDKin.IM.Windows.Model;
using IDKin.IM.Windows.Util;
using IDKin.IM.Windows.ViewModel;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
namespace IDKin.IM.Windows.Comps.MessageCenter
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public partial class OANoticeListView : ListView, System.IDisposable//, IComponentConnector
	{
		private ISessionService sessionService = ServiceUtil.Instance.SessionService;
		private MessageCenterViewModel messageCenter = null;
		public System.Collections.Generic.List<NoticeRecord> listRecord = new System.Collections.Generic.List<NoticeRecord>();
		//internal ListViewItem lisWORKFLOW;
		//internal ListViewItem listPROMANAGER;
		//internal ListViewItem listNOTICE;
		//internal ListViewItem listSYSTEM;
		//internal ListViewItem listPLAN;
		//internal ListViewItem listDOC;
		//internal ListViewItem listDISCUSS;
		//internal ListViewItem listAPPROVE_RECORD;
		//private bool _contentLoaded;
		public OANoticeListView()
		{
			this.InitializeComponent();
			this.messageCenter = new MessageCenterViewModel();
		}
		public void OAtree(System.Collections.Generic.List<NoticeRecord> list, OAModuleType type)
		{
			OANoticeRecordPage page = WindowModel.Instance.OARecordPage;
			page.trgMessageTable.Rows.Clear();
			page.MessageCenter(list);
			WindowModel.Instance.MessageCenterWindow.ViewMsgFrame.NavigationService.Navigate(page);
		}
		private void lisWORKFLOW_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			WindowModel.Instance.OARecordPage.SetShowPage();
			WindowModel.Instance.OARecordPage.type = OAModuleType.OA_WORKFLOW_RECORD_TYPE;
			this.messageCenter.SendOANoticeRequest(this.sessionService.Uid, "0", 1, 10, OAModuleType.OA_WORKFLOW_RECORD_TYPE);
		}
		private void listPROMANAGER_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			WindowModel.Instance.OARecordPage.SetShowPage();
			WindowModel.Instance.OARecordPage.type = OAModuleType.OA_PROMANAGER_RECORD_TYPE;
			this.messageCenter.SendOANoticeRequest(this.sessionService.Uid, "0", 1, 10, OAModuleType.OA_PROMANAGER_RECORD_TYPE);
		}
		private void listNOTICE_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			WindowModel.Instance.OARecordPage.SetShowPage();
			WindowModel.Instance.OARecordPage.type = OAModuleType.OA_NOTICE_RECORD_TYPE;
			this.messageCenter.SendOANoticeRequest(this.sessionService.Uid, "0", 1, 10, OAModuleType.OA_DISCUSS_RECORD_TYPE);
		}
		private void listSYSTEM_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			WindowModel.Instance.OARecordPage.SetShowPage();
			WindowModel.Instance.OARecordPage.type = OAModuleType.OA_SYSTEM_RECORD_TYPE;
			this.messageCenter.SendOANoticeRequest(this.sessionService.Uid, "0", 1, 10, OAModuleType.OA_SYSTEM_RECORD_TYPE);
		}
		private void listPLAN_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			WindowModel.Instance.OARecordPage.SetShowPage();
			WindowModel.Instance.OARecordPage.type = OAModuleType.OA_PLAN_RECORD_TYPE;
			this.messageCenter.SendOANoticeRequest(this.sessionService.Uid, "0", 1, 10, OAModuleType.OA_PLAN_RECORD_TYPE);
		}
		private void listDOC_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			WindowModel.Instance.OARecordPage.SetShowPage();
			WindowModel.Instance.OARecordPage.type = OAModuleType.OA_DOC_RECORD_TYPE;
			this.messageCenter.SendOANoticeRequest(this.sessionService.Uid, "0", 1, 10, OAModuleType.OA_DOC_RECORD_TYPE);
		}
		private void listDISCUSS_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			WindowModel.Instance.OARecordPage.SetShowPage();
			WindowModel.Instance.OARecordPage.type = OAModuleType.OA_DISCUSS_RECORD_TYPE;
			this.messageCenter.SendOANoticeRequest(this.sessionService.Uid, "0", 1, 10, OAModuleType.OA_NOTICE_RECORD_TYPE);
		}
		private void listAPPROVE_RECORD_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			WindowModel.Instance.OARecordPage.SetShowPage();
			WindowModel.Instance.OARecordPage.type = OAModuleType.OA_APPROVE_RECORD_TYPE;
			this.messageCenter.SendOANoticeRequest(this.sessionService.Uid, "0", 1, 10, OAModuleType.OA_APPROVE_RECORD_TYPE);
		}
		public void Dispose()
		{
			this.listRecord.Clear();
			throw new System.NotImplementedException();
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/messagecenter/oanoticelistview.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.lisWORKFLOW = (ListViewItem)target;
        //        this.lisWORKFLOW.MouseLeftButtonUp += new MouseButtonEventHandler(this.lisWORKFLOW_MouseLeftButtonUp);
        //        break;
        //    case 2:
        //        this.listPROMANAGER = (ListViewItem)target;
        //        this.listPROMANAGER.MouseLeftButtonUp += new MouseButtonEventHandler(this.listPROMANAGER_MouseLeftButtonUp);
        //        break;
        //    case 3:
        //        this.listNOTICE = (ListViewItem)target;
        //        this.listNOTICE.MouseLeftButtonUp += new MouseButtonEventHandler(this.listNOTICE_MouseLeftButtonUp);
        //        break;
        //    case 4:
        //        this.listSYSTEM = (ListViewItem)target;
        //        this.listSYSTEM.MouseLeftButtonUp += new MouseButtonEventHandler(this.listSYSTEM_MouseLeftButtonUp);
        //        break;
        //    case 5:
        //        this.listPLAN = (ListViewItem)target;
        //        this.listPLAN.MouseLeftButtonUp += new MouseButtonEventHandler(this.listPLAN_MouseLeftButtonUp);
        //        break;
        //    case 6:
        //        this.listDOC = (ListViewItem)target;
        //        this.listDOC.MouseLeftButtonUp += new MouseButtonEventHandler(this.listDOC_MouseLeftButtonUp);
        //        break;
        //    case 7:
        //        this.listDISCUSS = (ListViewItem)target;
        //        this.listDISCUSS.MouseLeftButtonUp += new MouseButtonEventHandler(this.listDISCUSS_MouseLeftButtonUp);
        //        break;
        //    case 8:
        //        this.listAPPROVE_RECORD = (ListViewItem)target;
        //        this.listAPPROVE_RECORD.MouseLeftButtonUp += new MouseButtonEventHandler(this.listAPPROVE_RECORD_MouseLeftButtonUp);
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
