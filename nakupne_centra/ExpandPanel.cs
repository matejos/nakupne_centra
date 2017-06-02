﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace nakupne_centra
{
    public sealed class ExpandPanel : ContentControl
    {
        public ExpandPanel()
        {
            this.DefaultStyleKey = typeof(ExpandPanel);
        }

        public void PanelLoaded()
        {
            ReturnExpandState();
        }

        private bool _useTransitions = true;
        private VisualState _collapsedState;
        private Windows.UI.Xaml.Controls.Primitives.ToggleButton toggleExpander;
        private FrameworkElement contentElement;

        public static readonly DependencyProperty HeaderContentProperty =
        DependencyProperty.Register("HeaderContent", typeof(string),
        typeof(ExpandPanel), new PropertyMetadata("X"));

        public static readonly DependencyProperty IsExpandedProperty =
        DependencyProperty.Register("IsExpanded", typeof(bool),
        typeof(ExpandPanel), new PropertyMetadata(false));

        public static readonly DependencyProperty CornerRadiusProperty =
        DependencyProperty.Register("CornerRadius", typeof(CornerRadius),
        typeof(ExpandPanel), null);

        public string HeaderContent
        {
            get { return (string)GetValue(HeaderContentProperty); }
            set { SetValue(HeaderContentProperty, value); }
        }

        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); SaveExpandState(value); }
        }

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        private void changeVisualState(bool useTransitions)
        {
            if (IsExpanded)
            {
                if (contentElement != null)
                {
                    contentElement.Visibility = Visibility.Visible;
                }
                VisualStateManager.GoToState(this, "Expanded", useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, "Collapsed", useTransitions);
                _collapsedState = (VisualState)GetTemplateChild("Collapsed");
                if (_collapsedState == null)
                {
                    if (contentElement != null)
                    {
                        contentElement.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            toggleExpander = (Windows.UI.Xaml.Controls.Primitives.ToggleButton)
                GetTemplateChild("ExpandCollapseButton");
            if (toggleExpander != null)
            {
                toggleExpander.Click += ClickExpander;
            }
            contentElement = (FrameworkElement)GetTemplateChild("Content");
            if (contentElement != null)
            {
                _collapsedState = (VisualState)GetTemplateChild("Collapsed");
                if ((_collapsedState != null) && (_collapsedState.Storyboard != null))
                {
                    _collapsedState.Storyboard.Completed += (object sender, object e) =>
                    {
                        contentElement.Visibility = IsExpanded ? Visibility.Visible : Visibility.Collapsed;
                    };
                }
            }
            changeVisualState(false);
        }

        private void ClickExpander(object sender, RoutedEventArgs e)
        {
            IsExpanded = !IsExpanded;
            toggleExpander.IsChecked = IsExpanded;
            changeVisualState(_useTransitions);
        }

        private void SaveExpandState(bool value)
        {
            (App.Current as App).CategoryExpanded[HeaderContent] = value;
        }

        private void ReturnExpandState()
        {
            if ((App.Current as App).CategoryExpanded.ContainsKey(HeaderContent))
            {
                if ((App.Current as App).CategoryExpanded[HeaderContent])
                {
                    ClickExpander(null, null);
                }
            }
            else
            {
                (App.Current as App).CategoryExpanded.Add(HeaderContent, false);
            }
        }
    }
}
