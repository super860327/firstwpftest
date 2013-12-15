using IDKin.IM.Core;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
namespace IDKin.IM.Windows.Comps
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class FriendsChatTabControl : UserControl//, IComponentConnector
	{
		private Roster roster;
		private long rosterId;
		//internal TabControl ChatTab;
		//internal ChatComponent ChatComponent;
		//private bool _contentLoaded;
		public long RosterId
		{
			get
			{
				return this.rosterId;
			}
		}
		public FriendsChatTabControl(Roster roster)
		{
			if (roster != null)
			{
				this.InitializeComponent();
				this.roster = roster;
				this.rosterId = roster.Uid;
				this.ChatComponent.InitData(roster);
				this.ChatComponent.MsgRecordComp.Visibility = Visibility.Collapsed;
			}
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/friendschattabcontrol.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.ChatTab = (TabControl)target;
        //        break;
        //    case 2:
        //        this.ChatComponent = (ChatComponent)target;
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
