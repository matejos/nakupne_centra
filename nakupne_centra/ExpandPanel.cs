using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources;
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
            if (!loaded)
            {
                ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView();
                string categoryCode = HeaderContent;
                string newHeaderContent = resourceLoader.GetString("Category" + categoryCode);
                if (newHeaderContent != "")
                {
                    HeaderContent = newHeaderContent;
                    var buttonBorder = (Border)GetTemplateChild("ButtonBorder");
                    buttonBorder.Style = (Style)App.Current.Resources["Category" + categoryCode + "Style"];
                }
                ReturnExpandState();
                loaded = true;
            }
        }

        private bool loaded = false;
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

        public static readonly DependencyProperty ButtonFillProperty =
        DependencyProperty.Register("ButtonFill", typeof(Style),
        typeof(ExpandPanel), null);

        public string HeaderContent
        {
            get { return (string)GetValue(HeaderContentProperty); }
            set { SetValue(HeaderContentProperty, value); }
        }

        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public Style ButtonFill
        {
            get { return (Style)GetValue(ButtonFillProperty); }
            set { SetValue(ButtonFillProperty, value); }
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
            ToggleExpand(!IsExpanded);
            SaveExpandState(IsExpanded);
        }

        public void ToggleExpand(bool expand)
        {
            IsExpanded = expand;
            toggleExpander.IsChecked = IsExpanded;
            changeVisualState(_useTransitions);
        }

        private void SaveExpandState(bool value)
        {
            (App.Current as App).CategoryExpanded[HeaderContent] = value;
        }

        public void ReturnExpandState()
        {
            if ((App.Current as App).CategoryExpanded.ContainsKey(HeaderContent))
            {
                if ((App.Current as App).CategoryExpanded[HeaderContent])
                {
                    ToggleExpand(true);
                }
                else
                {
                    ToggleExpand(false);
                }
            }
            else
            {
                (App.Current as App).CategoryExpanded.Add(HeaderContent, false);
            }
        }
    }
}
