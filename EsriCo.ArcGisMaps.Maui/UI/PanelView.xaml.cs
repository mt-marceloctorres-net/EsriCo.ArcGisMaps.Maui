﻿using System.Diagnostics;

using EsriCo.ArcGisMaps.Maui.Extensions;
using EsriCo.ArcGisMaps.Maui.Model;

namespace EsriCo.ArcGisMaps.Maui.UI
{
  /// <summary>
  /// 
  /// </summary>
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class PanelView : ContentView
  {
    /// <summary>
    /// 
    /// </summary>
    public event EventHandler Closed;

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty IsModalProperty = BindableProperty.Create(
      nameof(IsModal),
      typeof(bool),
      typeof(PanelView),
      defaultValue: false);

    /// <summary>
    /// 
    /// </summary>
    public bool IsModal
    {
      get => (bool)GetValue(IsModalProperty);
      set => SetValue(IsModalProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty IsManagedProperty = BindableProperty.Create(
      nameof(IsManaged),
      typeof(bool),
      typeof(PanelView),
      propertyChanged: OnIsManagedPropertyChanged,
      defaultValue: true);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnIsManagedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
      if(bindable is PanelView panelView)
      {
        panelView.IsManaged = (bool)newValue;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public bool IsManaged
    {
      get => (bool)GetValue(IsManagedProperty);
      set => SetValue(IsManagedProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty CloseButtonImageProperty = BindableProperty.Create(
      nameof(CloseButtonImage),
      typeof(ImageSource),
      typeof(PanelView),
      propertyChanged: OnCloseButtonImageChanged);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    /// <exception cref="NotImplementedException"></exception>
    private static void OnCloseButtonImageChanged(BindableObject bindable, object oldValue, object newValue)
    {
      if(newValue == null && bindable is PanelView panelView)
      {
        var resourceId = GetCloseButtonImage();
        panelView.CloseButtonImage = ImageSource.FromStream(() => typeof(PanelView).Assembly.GetStreamEmbeddedResource(resourceId));
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public ImageSource CloseButtonImage
    {
      get => (ImageSource)GetValue(CloseButtonImageProperty);
      set => SetValue(CloseButtonImageProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static new readonly BindableProperty IsVisibleProperty = BindableProperty.Create(
      nameof(IsVisible),
      typeof(bool),
      typeof(PanelView),
      defaultBindingMode: BindingMode.TwoWay,
      defaultValue: true,
      propertyChanged: OnIsVisibleChanged);

    /// <summary>
    /// 
    /// </summary>
    public new bool IsVisible
    {
      get => (bool)GetValue(IsVisibleProperty);
      set => SetValue(IsVisibleProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnIsVisibleChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var newVisible = (bool)newValue;
      if(bindable is PanelView panelView)
      {
        panelView.SetVisible(newVisible);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="visible"></param>
    private void SetVisible(bool visible)
    {
      base.IsVisible = visible;
      Content.TranslationX = 0;
      Content.TranslationY = 0;
      if(visible && IsManaged)
      {
        EventAggregator.Current.GetEvent<PanelViewIsVisibleChangedEvent>().Publish(this);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty BorderThicknessProperty = BindableProperty.Create(
      nameof(BorderThickness),
      typeof(Thickness),
      typeof(PanelView),
      defaultValue: new Thickness(5));

    /// <summary>
    /// 
    /// </summary>
    public Thickness BorderThickness
    {
      get => (Thickness)GetValue(BorderThicknessProperty);
      set => SetValue(BorderThicknessProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty HeaderContentProperty = BindableProperty.Create(
      nameof(HeaderContent),
      typeof(View),
      typeof(PanelView),
      propertyChanged: OnHeaderContentChanged);

    /// <summary>
    /// 
    /// </summary>
    public View HeaderContent
    {
      get => (View)GetValue(HeaderContentProperty);
      set => SetValue(HeaderContentProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnHeaderContentChanged(BindableObject bindable, object oldValue, object newValue)
    {
      if(!ReferenceEquals(newValue, bindable))
      {
        var panelView = (PanelView)bindable;
        var newView = (View)newValue;

        panelView.HeaderContentView.Content = newView;
      }
      else
      {
        Console.WriteLine(bindable);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty IsHeaderVisibleProperty = BindableProperty.Create(
      nameof(IsHeaderVisible),
      typeof(bool),
      typeof(PanelView),
      defaultValue: true);

    /// <summary>
    /// 
    /// </summary>
    public bool IsHeaderVisible
    {
      get => (bool)GetValue(IsHeaderVisibleProperty);
      set => SetValue(IsHeaderVisibleProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty IsFooterVisibleProperty = BindableProperty.Create(
      nameof(IsFooterVisible),
      typeof(bool),
      typeof(PanelView),
      defaultValue: true);

    /// <summary>
    /// 
    /// </summary>
    public bool IsFooterVisible
    {
      get => (bool)GetValue(IsFooterVisibleProperty);
      set => SetValue(IsFooterVisibleProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty FooterContentProperty = BindableProperty.Create(
      nameof(FooterContent),
      typeof(View),
      typeof(PanelView),
      propertyChanged: OnFooterContentChanged);

    /// <summary>
    /// 
    /// </summary>
    public View FooterContent
    {
      get => (View)GetValue(FooterContentProperty);
      set => SetValue(FooterContentProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnFooterContentChanged(BindableObject bindable, object oldValue, object newValue)
    {
      if(!ReferenceEquals(newValue, bindable))
      {
        var panelView = (PanelView)bindable;
        var newView = (View)newValue;

        panelView.FooterContentView.Content = newView;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty BodyContentProperty = BindableProperty.Create(
      nameof(BodyContent),
      typeof(View),
      typeof(PanelView),
      propertyChanged: OnBodyContentPropertyChanged);

    /// <summary>
    /// 
    /// </summary>
    public View BodyContent
    {
      get => (View)GetValue(BodyContentProperty);
      set => SetValue(BodyContentProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnBodyContentPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
      if(!ReferenceEquals(newValue, bindable))
      {
        var panelView = (PanelView)bindable;
        var newView = (View)newValue;

        panelView.BodyContentView.Content = newView;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty TitleBorderColorProperty = BindableProperty.Create(
      nameof(TitleBorderColor),
      typeof(Color),
      typeof(PanelView));

    /// <summary>
    /// 
    /// </summary>
    public Color TitleBorderColor
    {
      get => (Color)GetValue(TitleBorderColorProperty);
      set => SetValue(TitleBorderColorProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty TitleBackgroundColorProperty = BindableProperty.Create(
      nameof(TitleBackgroundColor),
      typeof(Color),
      typeof(PanelView),
      defaultValue: Colors.Transparent);

    /// <summary>
    /// 
    /// </summary>
    public Color TitleBackgroundColor
    {
      get => (Color)GetValue(TitleBackgroundColorProperty);
      set => SetValue(TitleBackgroundColorProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty TitleTextColorProperty = BindableProperty.Create(
      nameof(TitleTextColor),
      typeof(Color),
      typeof(PanelView));

    /// <summary>
    /// 
    /// </summary>
    public Color TitleTextColor
    {
      get => (Color)GetValue(TitleTextColorProperty);
      set => SetValue(TitleTextColorProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty TitleTextProperty = BindableProperty.Create(
      nameof(TitleText),
      typeof(string),
      typeof(PanelView));

    /// <summary>
    /// 
    /// </summary>
    public string? TitleText
    {
      get => (string)GetValue(TitleTextProperty);
      set => SetValue(TitleTextProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty TitleHorizontalTextAlignmentProperty = BindableProperty.Create(
      nameof(TitleHorizontalTextAlignment),
      typeof(TextAlignment),
      typeof(PanelView));

    /// <summary>
    /// 
    /// </summary>
    public TextAlignment TitleHorizontalTextAlignment
    {
      get => (TextAlignment)GetValue(TitleHorizontalTextAlignmentProperty);
      set => SetValue(TitleHorizontalTextAlignmentProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty IsTitleVisibleProperty = BindableProperty.Create(
      nameof(IsTitleVisible),
      typeof(bool),
      typeof(PanelView),
      defaultValue: true);

    /// <summary>
    /// 
    /// </summary>
    public bool IsTitleVisible
    {
      get => (bool)GetValue(IsTitleVisibleProperty);
      set => SetValue(IsTitleVisibleProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty BodyBorderColorProperty = BindableProperty.Create(
      nameof(BodyBorderColor),
      typeof(Color),
      typeof(PanelView));

    /// <summary>
    /// 
    /// </summary>
    public Color BodyBorderColor
    {
      get => (Color)GetValue(BodyBorderColorProperty);
      set => SetValue(BodyBorderColorProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty BodyBackgroundColorProperty = BindableProperty.Create(
      nameof(BodyBackgroundColor),
      typeof(Color),
      typeof(PanelView),
      defaultValue: Colors.Transparent);

    /// <summary>
    /// 
    /// </summary>
    public Color BodyBackgroundColor
    {
      get => (Color)GetValue(BodyBackgroundColorProperty);
      set => SetValue(BodyBackgroundColorProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty StatusBackgroundColorProperty = BindableProperty.Create(
      nameof(StatusBackgroundColor),
      typeof(Color),
      typeof(PanelView),
      defaultValue: Colors.Transparent);

    /// <summary>
    /// 
    /// </summary>
    public Color StatusBackgroundColor
    {
      get => (Color)GetValue(StatusBackgroundColorProperty);
      set => SetValue(StatusBackgroundColorProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty StatusBorderColorProperty = BindableProperty.Create(
      nameof(StatusBorderColor),
      typeof(Color),
      typeof(PanelView));

    /// <summary>
    /// 
    /// </summary>
    public Color StatusBorderColor
    {
      get => (Color)GetValue(StatusBorderColorProperty);
      set => SetValue(StatusBorderColorProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty StatusTextColorProperty = BindableProperty.Create(
      nameof(StatusTextColor),
      typeof(Color),
      typeof(PanelView));

    /// <summary>
    /// 
    /// </summary>
    public Color StatusTextColor
    {
      get => (Color)GetValue(StatusTextColorProperty);
      set => SetValue(StatusTextColorProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty StatusTextProperty = BindableProperty.Create(
      nameof(StatusText),
      typeof(string),
      typeof(PanelView));

    /// <summary>
    /// 
    /// </summary>
    public string StatusText
    {
      get => (string)GetValue(StatusTextProperty);
      set => SetValue(StatusTextProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty IsStatusVisibleProperty = BindableProperty.Create(
      nameof(IsStatusVisible),
      typeof(bool),
      typeof(PanelView),
      defaultValue: true);

    /// <summary>
    /// 
    /// </summary>
    public bool IsStatusVisible
    {
      get => (bool)GetValue(IsStatusVisibleProperty);
      set => SetValue(IsStatusVisibleProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private static string GetCloseButtonImage() => Application.Current is not null && Application.Current.PlatformAppTheme is AppTheme.Dark ?
         @"ic_close_dark" :
         @"ic_close";

    /// <summary>
    /// 
    /// </summary>
    public PanelView()
    {
      InitializeComponent();
#if DEBUG
      var externalPanelFrameColor = ExternalPanelFrame.BackgroundColor;
      var panelFrameColor = PanelFrame.BackgroundColor;
      var titleFrameColor = TitleBarFrame.BackgroundColor;
      var bodyFrameColor = BodyFrame.BackgroundColor;
      var statusFrameColor = StatusBarFrame.BackgroundColor;

      Debug.WriteLine($"External: {externalPanelFrameColor}");
      Debug.WriteLine($"Panel   : {panelFrameColor}");
      Debug.WriteLine($"Title   : {titleFrameColor}");
      Debug.WriteLine($"Body    : {bodyFrameColor}");
      Debug.WriteLine($"External: {statusFrameColor}");
#endif 

      var resourceId = GetCloseButtonImage();
      CloseButtonImage = ImageSource.FromStream(() => typeof(PanelView).Assembly.GetStreamEmbeddedResource(resourceId));
      EventAggregator.Current.GetEvent<PanelViewIsVisibleChangedEvent>().Subscribe(panelView =>
      {
        if(!ReferenceEquals(this, panelView) && IsManaged && IsVisible && panelView.IsVisible)
        {
          IsVisible = false;
        }
      });
      Closed += (sender, args) => EventAggregator.Current.GetEvent<PanelViewIsVisibleChangedEvent>().Publish(this);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected virtual void OnCloseButtonClicked(object sender, EventArgs e)
    {
      IsVisible = false;
      OnPanelClosed();
    }

    /// <summary>
    /// 
    /// </summary>
    protected virtual void OnPanelClosed() => Closed?.Invoke(this, new EventArgs());

    private double x, y;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
    {
      if(Parent is View parentView)
      {
        var bounds = Bounds;
        var parentBounds = parentView.Bounds;

        switch(e.StatusType)
        {
          case GestureStatus.Started:
            break;
          case GestureStatus.Running:
            Debug.WriteLine($"{bounds.X};{x + e.TotalX + bounds.X}>=0;{x + e.TotalX + bounds.X + bounds.Width}<={parentBounds.Width};{x + e.TotalX};{parentBounds.Width - bounds.Width - bounds.X}");
            if(x + e.TotalX + bounds.X >= 0)
            {
              TranslationX = x + e.TotalX + bounds.X + bounds.Width <= parentBounds.Width ?
               x + e.TotalX :
               parentBounds.Width - bounds.Width - bounds.X;
            }
            else
            {
              TranslationX = -bounds.X;
            }
            Debug.WriteLine($"TranslationX={TranslationX}");

            Debug.WriteLine($"{bounds.Y};{y + e.TotalY + bounds.Y}>=0;{y + e.TotalY + bounds.Y + bounds.Height}<={parentBounds.Height};{y + e.TotalY};{parentBounds.Height - bounds.Height - bounds.Y}");
            if(y + e.TotalY + bounds.Y >= 0)
            {
              TranslationY = y + e.TotalY + bounds.Y + bounds.Height <= parentBounds.Height ?
                y + e.TotalY :
                parentBounds.Height - bounds.Height - bounds.Y;
            }
            else
            {
              TranslationY = -bounds.Y;
            }
            Debug.WriteLine($"TranslationY={TranslationY}");
#if DEBUG
            Debug.WriteLine($"X={bounds.X};Y={bounds.Y};R:(Left={bounds.Left};Top={bounds.Top};Right={bounds.Right};Bottonm={bounds.Bottom})");
#endif
            break;
          case GestureStatus.Completed:
            x = TranslationX;
            y = TranslationY;
            break;
        }
      }
    }
  }
}