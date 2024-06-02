using Esri.ArcGISRuntime.Maui;
using Esri.ArcGISRuntime.UI;
namespace EsriCo.ArcGisMaps.Maui.UI
{
  /// <summary>
  /// 
  /// </summary>
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class DrawingStatusView : ContentView
  {
    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty IndicatorColorProperty = BindableProperty.Create(
      nameof(IndicatorColorProperty),
      typeof(Color),
      typeof(DrawingStatusView),
      propertyChanged: OnIndicatorColorPropertyChanged);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnIndicatorColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var newColor = (Color)newValue;
      if(bindable is DrawingStatusView contentView) 
      { 
        contentView.IndicatorColor = newColor;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public Color IndicatorColor
    {
      get => (Color)GetValue(IndicatorColorProperty);
      set
      {
        SetValue(IndicatorColorProperty, value);
        OnPropertyChanged(nameof(IndicatorColor));
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty MapViewProperty = BindableProperty.Create(
      nameof(MapView),
      typeof(MapView),
      typeof(DrawingStatusView),
      propertyChanged: OnMapViewPropertyChanged);

    /// <summary>
    /// 
    /// </summary>
    public MapView MapView
    {
      get => (MapView)GetValue(MapViewProperty);
      set => SetValue(MapViewProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnMapViewPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var contentView = bindable as DrawingStatusView;
      var oldMapView = oldValue as MapView;
      var newMapView = newValue as MapView;
      if(contentView != null && oldMapView != null && newMapView != null)
      {
        contentView.AddDrawStatusChangedHandler(oldMapView, newMapView);
      }
    }

    /// <summary>
    /// 
    /// </summary> c 
    /// <param name="oldMapView"></param>
    /// <param name="newMapView"></param>
    public void AddDrawStatusChangedHandler(MapView oldMapView, MapView newMapView)
    {
      if(oldMapView != null)
      {
        oldMapView.DrawStatusChanged -= MapViewDrawStatusChanged;
      }
      if(newMapView != null)
      {
        newMapView.DrawStatusChanged += MapViewDrawStatusChanged;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MapViewDrawStatusChanged(object? sender, DrawStatusChangedEventArgs e)
    {
      IsVisible = e.Status == DrawStatus.InProgress;
    }

    /// <summary>
    /// 
    /// </summary>
    public DrawingStatusView()
    {
      InitializeComponent();
      IsVisible = false;
      IndicatorColor = (Color)ActivityIndicator.ColorProperty.DefaultValue;
    }
  }
}