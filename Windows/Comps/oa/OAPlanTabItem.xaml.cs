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
	public partial class OAPlanTabItem : TabItem//, IComponentConnector, IStyleConnector
	{
		//internal DataGrid PlanDataGrid;
        ////private bool _contentLoaded;
		public OAPlanTabItem()
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
			this.PlanDataGrid.ItemsSource = null;
		}
		public void AddOAPlan(System.Collections.Generic.List<Plan> list)
		{
			if (list != null && list.Count > 0)
			{
				System.Collections.Generic.List<OAPlanData> noticeList = new System.Collections.Generic.List<OAPlanData>();
				foreach (Plan plan in list)
				{
					if (plan != null)
					{
						noticeList.Add(new OAPlanData
						{
							Title = plan.title,
							FullTitle = plan.fullTitle,
							Time = plan.time,
							NextRemind = plan.next_remind,
							Url = plan.url
						});
					}
				}
				this.PlanDataGrid.ItemsSource = noticeList;
			}
		}
		private void ViewMoreHandler(object sender, MouseButtonEventArgs e)
		{
			BrowserUtil.OpenNoticBrowserHandler("eyJsbiI6Ii9wbGFuL2NhbGVuZGFyIn0=");
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/oa/oaplantabitem.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.PlanDataGrid = (DataGrid)target;
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
