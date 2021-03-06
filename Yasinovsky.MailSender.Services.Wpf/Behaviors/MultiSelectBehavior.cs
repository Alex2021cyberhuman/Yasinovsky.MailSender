using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Microsoft.Xaml.Behaviors;

namespace Yasinovsky.MailSender.Services.Wpf.Behaviors
{
    public abstract class MultiSelectBehavior<T> : Behavior<MultiSelector>
    {
        protected MultiSelector Selector => AssociatedObject as MultiSelector;
        protected IList SourceItems => Selector.SelectedItems;
        public ObservableCollection<T> SelectedItems
        {
            get => (ObservableCollection<T>)GetValue(SelectedItemsProperty);
            set => SetValue(SelectedItemsProperty, value);
        }

        // Using a DependencyProperty as the backing store for SelectedItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register(nameof(SelectedItems), 
                typeof(ObservableCollection<T>), 
                typeof(MultiSelectBehavior<T>),
                new PropertyMetadata(default(ObservableCollection<T>)));

        protected override void OnAttached()
        {
            base.OnAttached();
            SubscribeToEvents();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            UnsubscribeFromEvents();
        }

        private void SelectorOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UnsubscribeFromEvents();
            SelectedItems =
                new ObservableCollection<T>(SourceItems as List<T> ?? SourceItems.Cast<T>().ToList());
            SubscribeToEvents();
        }
        private void SelectedItemsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UnsubscribeFromEvents();
            SelectedItems =
                new ObservableCollection<T>(SourceItems as List<T> ?? SourceItems.Cast<T>().ToList());
            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            Selector.SelectionChanged += SelectorOnSelectionChanged;
            if (SelectedItems != null)
                SelectedItems.CollectionChanged += SelectedItemsOnCollectionChanged;
        }

        private void UnsubscribeFromEvents()
        {
            Selector.SelectionChanged -= SelectorOnSelectionChanged;
            if (SelectedItems != null)
                SelectedItems.CollectionChanged -= SelectedItemsOnCollectionChanged;
        }

        
    }
}