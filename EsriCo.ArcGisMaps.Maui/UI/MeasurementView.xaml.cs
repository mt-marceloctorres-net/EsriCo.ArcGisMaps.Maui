using System.ComponentModel;

using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Maui;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.UI.Editing;

using EsriCo.ArcGisMaps.Maui.Extensions;
using EsriCo.ArcGisMaps.Maui.Model;

using Drawing = System.Drawing;
using EsriGeometry = Esri.ArcGISRuntime.Geometry;

namespace EsriCo.ArcGisMaps.Maui.UI
{
  /// <summary>
  /// 
  /// </summary>
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class MeasurementBarView : PanelView
  {
    /// <summary>
    /// 
    /// </summary>
    public class UnitItem
    {
      /// <summary>
      /// 
      /// </summary>
      public string? DisplayName { get; set; }

      /// <summary>
      /// 
      /// </summary>
      public string? Key { get; set; }

      /// <summary>
      /// 
      /// </summary>
      public object? Value { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty ColorProperty = BindableProperty.Create(
      nameof(Color),
      typeof(Drawing.Color),
      typeof(MeasurementBarView),
      defaultValue: Drawing.Color.DarkCyan,
      propertyChanged: OnColorPropertyChanged);

    /// <summary>
    /// 
    /// </summary>
    public Drawing.Color Color
    {
      get => (Drawing.Color)GetValue(ColorProperty);
      set => SetValue(ColorProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var view = bindable as MeasurementBarView;
      if(newValue is not null && view is not null)
      {
        view.Color = (Drawing.Color)newValue;
        view.DrawingProcess.Color = view.Color;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private const string DrawGraphicsOverlayId = "MeasurementGraphicsOverlay";

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty PointMeasurementToolImageProperty = BindableProperty.Create(
      nameof(PointMeasurementToolImage),
      typeof(ImageSource),
      typeof(MeasurementBarView),
      propertyChanged: OnPointMeasurementToolImageChanged);

    /// <summary>
    /// 
    /// </summary>
    public ImageSource PointMeasurementToolImage
    {
      get => (ImageSource)GetValue(PointMeasurementToolImageProperty);
      set => SetValue(PointMeasurementToolImageProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnPointMeasurementToolImageChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var view = bindable as MeasurementBarView;
      if(newValue is null && view is not null)
      {
        view.PointMeasurementToolImage = ImageSource.FromStream(() =>
          typeof(MeasurementBarView).Assembly.GetStreamEmbeddedResource(@"ic_coord"));
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty LineMeasurementToolImageProperty = BindableProperty.Create(
      nameof(LineMeasurementToolImage),
      typeof(ImageSource),
      typeof(MeasurementBarView),
      propertyChanged: OnLineMeasurementToolImageChanged);

    /// <summary>
    /// 
    /// </summary>
    public ImageSource LineMeasurementToolImage
    {
      get => (ImageSource)GetValue(LineMeasurementToolImageProperty);
      set => SetValue(LineMeasurementToolImageProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnLineMeasurementToolImageChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var view = bindable as MeasurementBarView;
      if(newValue is null && view is not null)
      {
        view.LineMeasurementToolImage = ImageSource.FromStream(() =>
          typeof(MeasurementBarView).Assembly.GetStreamEmbeddedResource(@"ic_distance"));
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty AreaMeasurementToolImageProperty = BindableProperty.Create(
      nameof(AreaMeasurementToolImage),
      typeof(ImageSource),
      typeof(MeasurementBarView),
      propertyChanged: OnAreaMeasurementToolImageChanged);

    /// <summary>
    /// 
    /// </summary>
    public ImageSource AreaMeasurementToolImage
    {
      get => (ImageSource)GetValue(AreaMeasurementToolImageProperty);
      set => SetValue(AreaMeasurementToolImageProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnAreaMeasurementToolImageChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var view = bindable as MeasurementBarView;
      if(newValue is null && view is not null)
      {
        view.AreaMeasurementToolImage = ImageSource.FromStream(() =>
          typeof(MeasurementBarView).Assembly.GetStreamEmbeddedResource(@"ic_area"));
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty NoneMeasurementToolImageProperty = BindableProperty.Create(
      nameof(NoneMeasurementToolImage),
      typeof(ImageSource),
      typeof(MeasurementBarView),
      propertyChanged: OnNoneMeasurementToolImageChanged);

    /// <summary>
    /// 
    /// </summary>
    public ImageSource NoneMeasurementToolImage
    {
      get => (ImageSource)GetValue(NoneMeasurementToolImageProperty);
      set => SetValue(NoneMeasurementToolImageProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnNoneMeasurementToolImageChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var view = bindable as MeasurementBarView;
      if(newValue is null && view is not null)
      {
        view.NoneMeasurementToolImage = ImageSource.FromStream(() =>
          typeof(MeasurementBarView).Assembly.GetStreamEmbeddedResource(@"ic_cancel"));
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty MapViewProperty = BindableProperty.Create(
      nameof(MapView),
      typeof(MapView),
      typeof(MeasurementBarView),
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
      var panelView = bindable as MeasurementBarView;
      if(newValue is MapView newMapView && panelView is not null)
      {
        if(newMapView.Map != null)
        {
          panelView.CheckMap(newMapView);
        }
        else
        {
          newMapView.PropertyChanged += (s, e) =>
          {
            if(e.PropertyName == nameof(newMapView.Map))
            {
              panelView.CheckMap(newMapView);
            }
          };
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mapView"></param>
    public void CheckMap(MapView mapView)
    {
      IsVisible = IsVisible && mapView != null && mapView.Map != null;
      if(mapView != null && mapView.Map != null && DrawingProcess is not null && DrawingProcess.DrawGraphicsOverlay is not null)
      {
        mapView.GraphicsOverlays ??= [];
        var graphicsOverlay = mapView.GraphicsOverlays?.FirstOrDefault(g => g.Id == DrawGraphicsOverlayId);
        if(graphicsOverlay is null)
        {
          DrawingProcess.DrawGraphicsOverlay.Graphics.Clear();
          mapView.GraphicsOverlays?.Add(DrawingProcess.DrawGraphicsOverlay);
        }
        DrawingProcess.MapView = mapView;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty IsDrawingProperty = BindableProperty.Create(
      nameof(IsDrawing),
      typeof(bool),
      typeof(MeasurementBarView),
      defaultValue: false,
      defaultBindingMode: BindingMode.OneWayToSource);

    /// <summary>
    /// 
    /// </summary>
    public bool IsDrawing
    {
      get => (bool)GetValue(IsDrawingProperty);
      set => SetValue(IsDrawingProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    private DrawingProcess DrawingProcess { get; set; }

    private string? _text;

    /// <summary>
    /// 
    /// </summary>
    public string? ResultText
    {
      get => _text;
      set
      {
        _text = value;
        OnPropertyChanged(nameof(ResultText));
      }
    }

    private UnitItem? _selectedItem;

    /// <summary>
    /// 
    /// </summary>
    public UnitItem? SelectedUnit
    {
      get => _selectedItem;
      set
      {
        _selectedItem = value;
        OnPropertyChanged(nameof(SelectedUnit));
      }
    }

    private List<UnitItem>? _units;

    /// <summary>
    /// 
    /// </summary>
    public List<UnitItem>? Units
    {
      get => _units;
      set
      {
        _units = value;
        OnPropertyChanged(nameof(Units));
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public UnitItem? SelectedAngularUnit { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public UnitItem? SelectedLinearUnit { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public UnitItem? SelectedAreaUnit { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public List<UnitItem>? AngularUnits { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public List<UnitItem>? LinearUnits { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public List<UnitItem>? AreaUnits { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    private Geometry? Geometry { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public MeasurementBarView()
    {
      InitializeComponent();
      IsVisible = false;
      var asm = GetType().Assembly;
      PointMeasurementToolImage = ImageSource.FromStream(() => asm.GetStreamEmbeddedResource(@"ic_coord"));
      LineMeasurementToolImage = ImageSource.FromStream(() => asm.GetStreamEmbeddedResource(@"ic_distance"));
      AreaMeasurementToolImage = ImageSource.FromStream(() => asm.GetStreamEmbeddedResource(@"ic_area"));
      NoneMeasurementToolImage = ImageSource.FromStream(() => asm.GetStreamEmbeddedResource(@"ic_cancel"));

      AngularUnits =
      [
        new UnitItem() { DisplayName = AppResources.DecimalDegrees, Key = "DecimalDegrees", Value = LatitudeLongitudeFormat.DecimalDegrees},
        new UnitItem() { DisplayName = AppResources.DegreesDecimalMinutes, Key = "DegreesDecimalMinutes", Value = LatitudeLongitudeFormat.DegreesDecimalMinutes},
        new UnitItem() { DisplayName = AppResources.DegreesMinutesSeconds, Key = "DegreesMinutesSeconds", Value = LatitudeLongitudeFormat.DegreesMinutesSeconds }
      ];
      LinearUnits =
      [
        new UnitItem() { DisplayName = AppResources.Meters, Key = "Meters", Value = EsriGeometry.LinearUnits.Meters},
        new UnitItem() { DisplayName = AppResources.Kilometers, Key = "Kilometers", Value = EsriGeometry.LinearUnits.Kilometers},
        new UnitItem() { DisplayName = AppResources.Feet, Key = "Feet", Value = EsriGeometry.LinearUnits.Feet},
        new UnitItem() { DisplayName = AppResources.Yards, Key = "Yards", Value = EsriGeometry.LinearUnits.Yards},
        new UnitItem() { DisplayName = AppResources.Miles, Key = "Miles", Value = EsriGeometry.LinearUnits.Miles}
      ];
      AreaUnits =
      [
        new UnitItem() { DisplayName = AppResources.SquareMeters, Key = "SquareMeters", Value = EsriGeometry.AreaUnits.SquareMeters },
        new UnitItem() { DisplayName = AppResources.Hectares, Key = "Hectares", Value = EsriGeometry.AreaUnits.Hectares },
        new UnitItem() { DisplayName = AppResources.SquareKilometers, Key = "SquareKilometers", Value = EsriGeometry.AreaUnits.SquareKilometers },
        new UnitItem() { DisplayName = AppResources.SquareFeet, Key = "SquareFeet", Value = EsriGeometry.AreaUnits.SquareFeet },
        new UnitItem() { DisplayName = AppResources.SquareYards, Key = "SquareYards", Value = EsriGeometry.AreaUnits.SquareYards },
        new UnitItem() { DisplayName = AppResources.Acres, Key = "Acres", Value = EsriGeometry.AreaUnits.Acres },
        new UnitItem() { DisplayName = AppResources.SquareMiles, Key = "SquareMiles", Value = EsriGeometry.AreaUnits.SquareMiles }
      ];

      DrawingProcess = new DrawingProcess()
      {
        GeometryEditor = new GeometryEditor(),
        Color = Color,
        DrawGraphicsOverlay = new GraphicsOverlay() { Id = DrawGraphicsOverlayId }
      };
      DrawingProcess.PropertyChanged += DrawingProcessPropertyChanged;

      DrawingProcess.PointCreated = g =>
      {
        Geometry = g;
        PointCreated(g);
      };
      DrawingProcess.PolylineCreated = g =>
      {
        Geometry = g;
        PolylineCreated(g);
      };
      DrawingProcess.PolygonCreated = g =>
      {
        Geometry = g;
        PolygonCreated(g);
      };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="geometry"></param>
    private void PolygonCreated(Geometry geometry)
    {
      if(SelectedAreaUnit?.Value is not null)
      {
        Geometry = geometry;

        var unit = (AreaUnit)SelectedAreaUnit.Value;
        var result = GeometryEngine.AreaGeodetic(geometry, unit, GeodeticCurveType.Geodesic);
        ResultText = $"{result:0.00} {unit.Abbreviation}";
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="geometry"></param>
    private void PolylineCreated(Geometry geometry)
    {
      if(SelectedLinearUnit?.Value is not null)
      {
        Geometry = geometry;

        var unit = (LinearUnit)SelectedLinearUnit.Value;
        var result = GeometryEngine.LengthGeodetic(geometry, unit, GeodeticCurveType.Geodesic);
        ResultText = $"{result:0.00} {unit.Abbreviation}";
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="geometry"></param>
    private void PointCreated(Geometry geometry)
    {
      if(SelectedAngularUnit?.Value is not null)
      {
        Geometry = geometry;

        var result = CoordinateFormatter
          .ToLatitudeLongitude((MapPoint)geometry, (LatitudeLongitudeFormat)SelectedAngularUnit.Value, 3);
        ResultText = result;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void ClearMeasurement()
    {
      if(DrawingProcess is not null)
      {
        DrawingProcess.GeometryEditor?.Stop();
        DrawingProcess.DrawGraphicsOverlay?.Graphics.Clear();
        Geometry = null;
        ResultText = string.Empty;
        Units = null;
        SelectedUnit = null;
        IsDrawing = false;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DrawingProcessPropertyChanged(object? sender, PropertyChangedEventArgs? e)
    {
      if(e is not null && e.PropertyName == nameof(DrawingProcess.IsDrawing) && DrawingProcess.IsDrawing)
      {
        IsDrawing = DrawingProcess.IsDrawing;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PointMeasurementToolClicked(object sender, EventArgs e)
    {
      if(DrawingProcess is not null && DrawingProcess.DrawGraphicsOverlay is not null)
      {
        DrawingProcess.DrawGraphicsOverlay.Graphics.Clear();
        Geometry = null;
        ResultText = string.Empty;

        Units = AngularUnits;
        SelectedAngularUnit ??= AngularUnits?.Find(u => u.Key == nameof(LatitudeLongitudeFormat.DegreesMinutesSeconds));
        SelectedUnit = SelectedAngularUnit;
        DrawingProcess.StartDrawGeometry(GeometryType.Point);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void LineMeasurementToolClicked(object sender, EventArgs e)
    {
      if(DrawingProcess is not null && DrawingProcess.DrawGraphicsOverlay is not null)
      {
        DrawingProcess.DrawGraphicsOverlay.Graphics.Clear();
        Geometry = null;
        ResultText = string.Empty;

        Units = LinearUnits;
        SelectedLinearUnit ??= LinearUnits?.Find(u => u.Key == nameof(EsriGeometry.LinearUnits.Kilometers));
        SelectedUnit = SelectedLinearUnit;
        DrawingProcess.StartDrawGeometry(GeometryType.Polyline);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void AreaMeasurementToolClicked(object sender, EventArgs e)
    {
      if(DrawingProcess is not null && DrawingProcess.DrawGraphicsOverlay is not null)
      {
        DrawingProcess.DrawGraphicsOverlay.Graphics.Clear();
        Geometry = null;
        ResultText = string.Empty;

        Units = AreaUnits;
        SelectedAreaUnit ??= AreaUnits?.Find(u => u.Key == nameof(EsriGeometry.AreaUnits.SquareKilometers));
        SelectedUnit = SelectedAreaUnit;
        DrawingProcess.StartDrawGeometry(GeometryType.Polygon);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PickerSelectedIndexChanged(object sender, EventArgs e)
    {
      var value = SelectedUnit?.Value;
      if(SelectedUnit is null || value is null)
      {
        return;
      }
      if(value is LatitudeLongitudeFormat)
      {
        SelectedAngularUnit = SelectedUnit;
        if(Geometry != null)
        {
          PointCreated(Geometry);
        }
      }
      else if(value is LinearUnit)
      {
        SelectedLinearUnit = SelectedUnit;
        if(Geometry != null)
        {
          PolylineCreated(Geometry);
        }
      }
      else if(value is AreaUnit)
      {
        SelectedAreaUnit = SelectedUnit;
        if(Geometry != null)
        {
          PolygonCreated(Geometry);
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MeasureViewClosed(object sender, EventArgs e) => ClearMeasurement();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void NoneMeasurementToolClicked(object sender, EventArgs e) => ClearMeasurement();
  }
}