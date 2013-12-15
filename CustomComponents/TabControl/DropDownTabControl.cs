using System;
using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace IDKin.IM.CustomComponents.Controls.TabControl
{
	public class DropDownTabControl : System.Windows.Controls.TabControl
	{
		private ToggleButton toggleButton;
		private int START_INDEX = 0;
		static DropDownTabControl()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DropDownTabControl), new FrameworkPropertyMetadata(typeof(DropDownTabControl)));
		}
		public DropDownTabControl()
		{
			base.Loaded += new RoutedEventHandler(this.CustomTabControl_Loaded);
		}
		private void CustomTabControl_Loaded(object sender, RoutedEventArgs e)
		{
			this.ForceItemRender();
		}
		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			base.OnItemsChanged(e);
			this.ForceItemRender();
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.toggleButton = (base.Template.FindName("PART_DropDown", this) as ToggleButton);
			if (this.toggleButton != null)
			{
				ContextMenu contextMenu = new ContextMenu
				{
					PlacementTarget = this.toggleButton,
					Placement = PlacementMode.Bottom,
					Style = base.FindResource("DefaultContextMenuStyle") as Style
				};
				Binding b = new Binding
				{
					Source = this.toggleButton,
					Mode = BindingMode.TwoWay,
					Path = new PropertyPath(ToggleButton.IsCheckedProperty)
				};
				contextMenu.SetBinding(ContextMenu.IsOpenProperty, b);
				this.toggleButton.ContextMenu = contextMenu;
				this.toggleButton.Checked += new RoutedEventHandler(this.DropDownButton_Checked);
			}
		}
		private void DropDownButton_Checked(object sender, RoutedEventArgs e)
		{
			if (this.toggleButton != null)
			{
				this.toggleButton.ContextMenu.Items.Clear();
				this.toggleButton.ContextMenu.Placement = PlacementMode.Bottom;
				this.toggleButton.ContextMenu.IsOpen = true;
				int index = this.START_INDEX;
				if (base.Items != null)
				{
					foreach (TabItem item in (IEnumerable)base.Items)
					{
						if (item != null)
						{
							MenuItem mi;
							if (item.Tag.GetType().Equals(typeof(string)))
							{
								string header = item.Tag.ToString();
								mi = new MenuItem
								{
									Header = header,
									Tag = index++.ToString()
								};
							}
							else
							{
								MenuItem menuItem = (MenuItem)item.Tag;
								mi = new MenuItem
								{
									Icon = menuItem.Icon,
									Header = menuItem.Header,
									Tag = index++.ToString()
								};
							}
							if (mi != null)
							{
								this.toggleButton.ContextMenu.Items.Add(mi);
								mi.Click += new RoutedEventHandler(this.MenuItem_Click);
							}
						}
					}
				}
			}
		}
		private void MenuItem_Click(object sender, RoutedEventArgs e)
		{
			MenuItem mi = sender as MenuItem;
			if (mi != null)
			{
				TabItem tabItem = this.GetTabItem(int.Parse(mi.Tag.ToString()));
				if (tabItem != null)
				{
					tabItem.IsSelected = true;
					tabItem.Focus();
				}
			}
		}
		internal TabItem GetTabItem(int index)
		{
			TabItem result;
			if (BindingOperations.IsDataBound(this, ItemsControl.ItemsSourceProperty))
			{
				IList list = base.ItemsSource as IList;
				if (list != null)
				{
					result = (base.ItemContainerGenerator.ContainerFromItem(list[index]) as TabItem);
				}
				else
				{
					int i = 0;
					IEnumerator enumerator = base.ItemsSource.GetEnumerator();
					while (enumerator.MoveNext())
					{
						if (i == index)
						{
							result = (base.ItemContainerGenerator.ContainerFromItem(enumerator.Current) as TabItem);
							return result;
						}
						i++;
					}
					result = null;
				}
			}
			else
			{
				result = (base.Items[index] as TabItem);
			}
			return result;
		}
		private void ForceItemRender()
		{
			this.ForItemRenderForItems();
		}
		private void ForItemRenderForItems()
		{
			object selectedItem = base.SelectedItem;
			foreach (object item in (IEnumerable)base.Items)
			{
				TabItem tabItem = item as TabItem;
				if (tabItem == null)
				{
					tabItem = (base.ItemContainerGenerator.ContainerFromItem(item) as TabItem);
				}
				if (tabItem != null)
				{
					tabItem.IsSelected = true;
					base.UpdateLayout();
				}
			}
			if (selectedItem != null)
			{
				base.SelectedItem = selectedItem;
			}
		}
	}
}
