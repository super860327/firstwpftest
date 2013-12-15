using IDKin.IM.Core;
using IDKin.IM.Windows.Util;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
namespace IDKin.IM.Windows.Comps
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class FriendsList : TreeView//, IComponentConnector
	{
		//internal TreeViewItem RosterList;
		//internal TreeViewItem StrangerList;
        ////private bool _contentLoaded;
		public FriendsList()
		{
			this.InitializeComponent();
		}
		public void AddRoster(Roster roster)
		{
			try
			{
				if (roster != null && this.FindRosterItem(roster.Uid) == null)
				{
					TreeNodeRoster node = new TreeNodeRoster(roster);
					node.DataContext = roster.Uid;
					this.RosterList.Items.Add(node);
				}
			}
			catch (System.Exception e)
			{
				System.Console.WriteLine(e.ToString());
			}
		}
		public ListViewItem FindRosterItem(long uid)
		{
			ListViewItem result;
			try
			{
				ItemCollection coll = this.RosterList.Items;
				if (coll != null)
				{
					foreach (TreeNodeRoster item in (System.Collections.IEnumerable)coll)
					{
						if (item.DataContext != null)
						{
							long userId = System.Convert.ToInt64(item.DataContext.ToString());
							if (userId == uid)
							{
								result = item;
								return result;
							}
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				ServiceUtil.Instance.Logger.Error(ex.ToString());
			}
			result = null;
			return result;
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/friendslist.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.RosterList = (TreeViewItem)target;
        //        break;
        //    case 2:
        //        this.StrangerList = (TreeViewItem)target;
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
