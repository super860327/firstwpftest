using System;
using System.Windows;
using System.Windows.Controls;
namespace IDKin.IM.CustomComponents.Controls
{
	[TemplatePart(Name = "PART_ItemsHost", Type = typeof(AccordionPanel))]
	public class Accordion : ItemsControl
	{
		private AccordionPanel _itemsHost;
		public static readonly DependencyProperty ExpandedItemProperty;
		public object ExpandedItem
		{
			get
			{
				return base.GetValue(Accordion.ExpandedItemProperty);
			}
			set
			{
				base.SetValue(Accordion.ExpandedItemProperty, value);
			}
		}
		private static void OnExpandedItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			Accordion shelf = sender as Accordion;
			if (shelf != null)
			{
				shelf.OnExpandedItemChanged(e.OldValue, e.NewValue);
			}
		}
		protected virtual void OnExpandedItemChanged(object oldValue, object newValue)
		{
			AccordionItem oldItem = base.ItemContainerGenerator.ContainerFromItem(oldValue) as AccordionItem;
			AccordionItem newItem = base.ItemContainerGenerator.ContainerFromItem(newValue) as AccordionItem;
			if (oldItem != null)
			{
				oldItem.IsExpanded = false;
			}
			if (newItem != null)
			{
				if (this._itemsHost != null)
				{
					this._itemsHost.ChildToFill = newItem;
				}
			}
		}
		static Accordion()
		{
			Accordion.ExpandedItemProperty = DependencyProperty.Register("ExpandedItem", typeof(object), typeof(Accordion), new UIPropertyMetadata(null, new PropertyChangedCallback(Accordion.OnExpandedItemChanged)));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Accordion), new FrameworkPropertyMetadata(typeof(Accordion)));
		}
		protected override void ClearContainerForItemOverride(DependencyObject element, object item)
		{
			base.ClearContainerForItemOverride(element, item);
		}
		protected override DependencyObject GetContainerForItemOverride()
		{
			return new AccordionItem();
		}
		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is AccordionItem;
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this._itemsHost = (base.GetTemplateChild("PART_ItemsHost") as AccordionPanel);
		}
	}
}
