using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace IDKin.IM.CustomComponents.Controls
{
	public class AccordionItem : HeaderedContentControl
	{
		public static readonly DependencyProperty IsExpandedProperty;
		public static RoutedEvent ExpandedEvent;
		public static RoutedEvent CollapsedEvent;
		public static RoutedCommand ExpandCommand;
		public event RoutedEventHandler Expanded
		{
			add
			{
				base.AddHandler(AccordionItem.ExpandedEvent, value);
			}
			remove
			{
				base.RemoveHandler(AccordionItem.ExpandedEvent, value);
			}
		}
		public event RoutedEventHandler Collapsed
		{
			add
			{
				base.AddHandler(AccordionItem.CollapsedEvent, value);
			}
			remove
			{
				base.RemoveHandler(AccordionItem.CollapsedEvent, value);
			}
		}
		public bool IsExpanded
		{
			get
			{
				return (bool)base.GetValue(AccordionItem.IsExpandedProperty);
			}
			set
			{
				base.SetValue(AccordionItem.IsExpandedProperty, value);
			}
		}
		public Accordion ParentAccordion
		{
			get
			{
				return ItemsControl.ItemsControlFromItemContainer(this) as Accordion;
			}
		}
		private static void OnIsExpandedChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			AccordionItem item = sender as AccordionItem;
			if (item != null)
			{
				item.OnIsExpandedChanged(e);
			}
		}
		protected virtual void OnIsExpandedChanged(DependencyPropertyChangedEventArgs e)
		{
			bool newValue = (bool)e.NewValue;
			if (newValue)
			{
				this.OnExpanded();
			}
			else
			{
				this.OnCollapsed();
			}
		}
		protected virtual void OnExpanded()
		{
			Accordion parentAccordion = this.ParentAccordion;
			if (parentAccordion != null)
			{
				parentAccordion.ExpandedItem = this;
			}
			base.RaiseEvent(new RoutedEventArgs(AccordionItem.ExpandedEvent, this));
		}
		protected virtual void OnCollapsed()
		{
			base.RaiseEvent(new RoutedEventArgs(AccordionItem.CollapsedEvent, this));
		}
		private static void OnExecuteExpand(object sender, ExecutedRoutedEventArgs e)
		{
			AccordionItem item = sender as AccordionItem;
			if (!item.IsExpanded)
			{
				item.IsExpanded = true;
			}
		}
		private static void CanExecuteExpand(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (sender is AccordionItem);
		}
		static AccordionItem()
		{
			AccordionItem.IsExpandedProperty = DependencyProperty.Register("IsExpanded", typeof(bool), typeof(AccordionItem), new PropertyMetadata(false, new PropertyChangedCallback(AccordionItem.OnIsExpandedChanged)));
			AccordionItem.ExpandedEvent = EventManager.RegisterRoutedEvent("Expanded", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(AccordionItem));
			AccordionItem.CollapsedEvent = EventManager.RegisterRoutedEvent("Collapsed", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(AccordionItem));
			AccordionItem.ExpandCommand = new RoutedCommand("Expand", typeof(AccordionItem));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(AccordionItem), new FrameworkPropertyMetadata(typeof(AccordionItem)));
			CommandBinding expandCommandBinding = new CommandBinding(AccordionItem.ExpandCommand, new ExecutedRoutedEventHandler(AccordionItem.OnExecuteExpand), new CanExecuteRoutedEventHandler(AccordionItem.CanExecuteExpand));
			CommandManager.RegisterClassCommandBinding(typeof(AccordionItem), expandCommandBinding);
		}
	}
}
