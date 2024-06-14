using Esri.ArcGISRuntime.Maui;
using Esri.ArcGISRuntime.UI;

using Drawing = System.Drawing;

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
      typeof(Drawing.Color),
      typeof(DrawingStatusView),
      defaultValue: Drawing.Color.Black,
      propertyChanged: OnIndicatorColorPropertyChanged);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnIndicatorColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var newColor = (Drawing.Color)newValue;
      if(bindable is DrawingStatusView contentView)
      {
        contentView.IndicatorColor = newColor;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public Drawing.Color IndicatorColor
    {
      get => (Drawing.Color)GetValue(IndicatorColorProperty);
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
      if(bindable is DrawingStatusView contentView && oldValue is MapView oldMapView && newValue is MapView newMapView)
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
    private void MapViewDrawStatusChanged(object? sender, DrawStatusChangedEventArgs e) => IsVisible = e.Status == DrawStatus.InProgress;

    /// <summary>
    /// 
    /// </summary>
    public DrawingStatusView()
    {
      InitializeComponent();
      IsVisible = false;
      IndicatorColor = (Drawing.Color)ActivityIndicator.ColorProperty.DefaultValue;
    }
  }
}