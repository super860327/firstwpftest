using IDKin.IM.Core;
using IDKin.IM.Log;
using IDKin.IM.Windows.Model;
using IDKin.IM.Windows.Util;
using IDKin.IM.Windows.View;
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
    public partial class EnterpriseGroupListView : ListView//, IComponentConnector
	{
		private ILogger logger = ServiceUtil.Instance.Logger;
		//internal EnterpriseGroupListView listEntGroup;
		//internal ContextMenu entGroupContextMenu;
        ////private bool _contentLoaded;
		public EnterpriseGroupListView()
		{
			this.InitializeComponent();
		}
		public void UpdateGroupInfo(EntGroup group)
		{
			if (group != null)
			{
				ItemGroup ig = this.FindItemGroup(group);
				if (ig != null)
				{
					ig.ChangeGroupName(group.Name, true);
				}
			}
		}
		public void DeleEntGroup(EntGroup group)
		{
			try
			{
				if (group != null)
				{
					ItemGroup ig = this.FindItemGroup(group);
					if (ig != null)
					{
						this.listEntGroup.Items.Remove(ig);
					}
				}
			}
			catch (System.Exception ex)
			{
				if (this.logger != null)
				{
					this.logger.Error(ex);
				}
			}
		}
		public void AddEntGroup(EntGroup group)
		{
			try
			{
				if (group != null)
				{
					if (null == this.FindItemGroup(group))
					{
						ItemGroup ig = new ItemGroup(group);
						this.listEntGroup.Items.Add(ig);
					}
				}
			}
			catch (System.Exception ex)
			{
				if (this.logger != null)
				{
					this.logger.Error(ex);
				}
			}
		}
		public ItemGroup FindItemGroup(long gid)
		{
			ItemGroup result;
			foreach (ItemGroup ig in (System.Collections.IEnumerable)this.listEntGroup.Items)
			{
				if (ig.Gid == gid)
				{
					result = ig;
					return result;
				}
			}
			result = null;
			return result;
		}
		public ItemGroup FindItemGroup(EntGroup group)
		{
			ItemGroup result;
			foreach (ItemGroup ig in (System.Collections.IEnumerable)this.listEntGroup.Items)
			{
				if (ig.Gid == group.Gid)
				{
					result = ig;
					return result;
				}
			}
			result = null;
			return result;
		}
		public void Clear()
		{
		}
		private void treEntGroupMenuItem_Click(object sender, RoutedEventArgs e)
		{
			string text = (sender as MenuItem).DataContext.ToString();
			if (text != null)
			{
				if (!(text == "add"))
				{
					if (!(text == "update"))
					{
						if (!(text == "delete"))
						{
						}
					}
				}
				else
				{
					this.ShowManagerWindow();
				}
			}
		}
		private void ShowManagerWindow()
		{
			WindowModel.Instance.CreateOrAtcivateEntGroupManagerWindow(null, OperationType.Add);
		}
		private EntGroupManagerWindow GetManagerWindow(bool create = false)
		{
			return WindowModel.Instance.GroupManagerWindow;
		}
        //[System.Diagnostics.DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Uri resourceLocater = new Uri("/IDKin.IM.Windows;component/comps/enterprisegrouplistview.xaml", UriKind.Relative);
        //        Application.LoadComponent(this, resourceLocater);
        //    }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never), System.Diagnostics.DebuggerNonUserCode]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //    case 1:
        //        this.listEntGroup = (EnterpriseGroupListView)target;
        //        break;
        //    case 2:
        //        this.entGroupContextMenu = (ContextMenu)target;
        //        break;
        //    case 3:
        //        ((MenuItem)target).Click += new RoutedEventHandler(this.treEntGroupMenuItem_Click);
        //        break;
        //    case 4:
        //        ((MenuItem)target).Click += new RoutedEventHandler(this.treEntGroupMenuItem_Click);
        //        break;
        //    default:
        //        this._contentLoaded = true;
        //        break;
        //    }
        //}
	}
}
