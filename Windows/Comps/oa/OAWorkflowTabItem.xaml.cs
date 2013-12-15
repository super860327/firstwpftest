using IDKin.IM.Protocol.Enterprise;
using IDKin.IM.Windows.Model.OA;
using IDKin.IM.Windows.Util;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
namespace IDKin.IM.Windows.Comps.OA
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public partial class OAWorkflowTabItem : TabItem//, IComponentConnector, IStyleConnector
	{
		//internal DataGrid WorkflowDataGrid;
		//private bool _contentLoaded;
		public OAWorkflowTabItem()
		{
			this.InitializeComponent();
		}
		private void TextBlockMouseEnter(object sender, MouseEventArgs e)
		{
			TextBlock text = e.Source as TextBlock;
			text.TextDecorations = TextDecorations.Underline;
			text.Foreground = Brushes.Blue;
			text.Cursor = Cursors.Hand;
		}
		private void TextBlockMouseLeave(object sender, MouseEventArgs e)
		{
			TextBlock text = e.Source as TextBlock;
			text.TextDecorations = null;
			text.Foreground = Brushes.Black;
			text.Cursor = Cursors.Hand;
		}
		private void TextBlockMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			TextBlock text = e.Source as TextBlock;
			if (text.Tag != null)
			{
				BrowserUtil.OpenNoticBrowserHandler(text.Tag.ToString());
			}
		}
		public void ClearData()
		{
			this.WorkflowDataGrid.ItemsSource = null;
		}
		public void AddOASchedule(System.Collections.Generic.List<WorkFlow> list)
		{
			if (list != null && list.Count > 0)
			{
				System.Collections.Generic.List<OAWorkflowData> workflowList = new System.Collections.Generic.List<OAWorkflowData>();
				foreach (WorkFlow workflow in list)
				{
					if (workflow != null)
					{
						workflowList.Add(new OAWorkflowData
						{
							Title = workflow.title,
							Important = workflow.isImportant ? Visibility.Visible : Visibility.Collapsed,
							Urgent = workflow.isUrgent ? Visibility.Visible : Visibility.Collapsed,
							Url = workflow.url,
							Username = workflow.send,
							Createtime = workflow.createTime,
							Project = workflow.projectName,
							Time = workflow.projectTime,
							Description = workflow.projectDes,
							Type = (workflow.type == 3) ? Visibility.Visible : Visibility.Collapsed
						});
					}
				}
				this.WorkflowDataGrid.ItemsSource = workflowList;
			}
		}
		private void ViewMoreHandler(object sender, MouseButtonEventArgs e)
		{
			BrowserUtil.OpenNoticBrowserHandler("eyJsbiI6Ii93b3JrZmxvdy90YXNrL2luZGV4In0=");
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/oa/oaworkflowtabitem.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.WorkflowDataGrid = (DataGrid)target;
        //        return;
        //    case 3:
        //        ((TextBlock)target).MouseLeftButtonDown += new MouseButtonEventHandler(this.ViewMoreHandler);
        //        return;
        //    }
        //    this._contentLoaded = true;
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IStyleConnector.Connect(int connectionId, object target)
        //{
        //    if (connectionId == 2)
        //    {
        //        EventSetter eventSetter = new EventSetter();
        //        eventSetter.Event = UIElement.MouseEnterEvent;
        //        eventSetter.Handler = new MouseEventHandler(this.TextBlockMouseEnter);
        //        ((Style)target).Setters.Add(eventSetter);
        //        eventSetter = new EventSetter();
        //        eventSetter.Event = UIElement.MouseLeaveEvent;
        //        eventSetter.Handler = new MouseEventHandler(this.TextBlockMouseLeave);
        //        ((Style)target).Setters.Add(eventSetter);
        //        eventSetter = new EventSetter();
        //        eventSetter.Event = UIElement.MouseLeftButtonDownEvent;
        //        eventSetter.Handler = new MouseButtonEventHandler(this.TextBlockMouseLeftButtonDown);
        //        ((Style)target).Setters.Add(eventSetter);
        //    }
        //}
	}
}
