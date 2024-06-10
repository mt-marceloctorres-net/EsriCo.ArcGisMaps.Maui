using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;

using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Maui;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.UI.Editing;

using EsriCo.ArcGisMaps.Maui.Extensions;
using EsriCo.ArcGisMaps.Maui.Model;

using Drawing = System.Drawing;

namespace EsriCo.ArcGisMaps.Maui.UI
{
  /// <summary>
  /// 
  /// </summary>
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class DrawToolBarView : ContentView
  {
    /// <summary>
    /// 
    /// </summary>
    private const string DrawGraphicsOverlayId = "DrawGraphicsOverlay";

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty ColorProperty = BindableProperty.Create(
      nameof(Color),
      typeof(Drawing.Color),
      typeof(DrawToolBarView),
      propertyChanged: OnColorPropertyChanged,
      defaultValue: Drawing.Color.DarkCyan);

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
      var contentView = bindable as DrawToolBarView;
      if(newValue is not null && contentView is not null)
      {
        contentView.Color = (Drawing.Color)newValue;
        contentView.DrawingProcess.Color = contentView.Color;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty OrientationProperty = BindableProperty.Create(
      nameof(Orientation),
      typeof(StackOrientation),
      typeof(DrawToolBarView),
      defaultValue: StackOrientation.Horizontal);

    /// <summary>
    /// 
    /// </summary>
    public StackOrientation Orientation
    {
      get => (StackOrientation)GetValue(OrientationProperty);
      set => SetValue(OrientationProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty IsDrawingProperty = BindableProperty.Create(
      nameof(IsDrawing),
      typeof(bool),
      typeof(DrawToolBarView),
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
    public static readonly BindableProperty MapViewProperty = BindableProperty.Create(
      nameof(MapView),
      typeof(MapView),
      typeof(DrawToolBarView),
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
      var panelView = bindable as DrawToolBarView;
      if(newValue is MapView newMapView && panelView is not null)
      {
        if(newMapView.Map is not null)
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
      if(mapView != null && mapView.Map != null)
      {
        IsVisible = true;
        mapView.GraphicsOverlays ??= [];
        var graphicsOverlay = mapView.GraphicsOverlays.FirstOrDefault(g => g.Id == DrawGraphicsOverlayId);
        if(graphicsOverlay is null && DrawingProcess.DrawGraphicsOverlay is not null)
        {
          DrawingProcess.DrawGraphicsOverlay.Graphics.Clear();
          mapView.GraphicsOverlays.Add(DrawingProcess.DrawGraphicsOverlay);
        }
        DrawingProcess.MapView = mapView;
      }
      else
      {
        IsVisible = false;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty DrawPointToolImageProperty = BindableProperty.Create(
      nameof(DrawPointToolImage),
      typeof(ImageSource),
      typeof(DrawToolBarView),
      propertyChanged: OnDrawPointToolImageChanged);

    /// <summary>
    /// 
    /// </summary>
    public ImageSource DrawPointToolImage
    {
      get => (ImageSource)GetValue(DrawPointToolImageProperty);
      set => SetValue(DrawPointToolImageProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnDrawPointToolImageChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var contentView = bindable as DrawToolBarView;
      if(newValue is null && contentView is not null)
      {
        contentView.DrawPointToolImage = ImageSource.FromStream(() =>
          typeof(DrawToolBarView).Assembly.GetStreamEmbeddedResource(@"ic_point"));
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty DrawPolylineToolImageProperty = BindableProperty.Create(
      nameof(DrawPolylineToolImage),
      typeof(ImageSource),
      typeof(DrawToolBarView),
      propertyChanged: OnDrawPolylineToolImageChanged);

    /// <summary>
    /// 
    /// </summary>
    public ImageSource DrawPolylineToolImage
    {
      get => (ImageSource)GetValue(DrawPolylineToolImageProperty);
      set => SetValue(DrawPolylineToolImageProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnDrawPolylineToolImageChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var drawToolbarView = bindable as DrawToolBarView;
      if(newValue is null && drawToolbarView is not null)
      {
        drawToolbarView.DrawPolylineToolImage = ImageSource.FromStream(() =>
          typeof(DrawToolBarView).Assembly.GetStreamEmbeddedResource(@"ic_polyline"));
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty DrawPolygonToolImageProperty = BindableProperty.Create(
      nameof(DrawPolygonToolImage),
      typeof(ImageSource),
      typeof(DrawToolBarView),
      propertyChanged: OnDrawPolygonToolImageChanged);

    /// <summary>
    /// 
    /// </summary>
    public ImageSource DrawPolygonToolImage
    {
      get => (ImageSource)GetValue(DrawPolygonToolImageProperty);
      set => SetValue(DrawPolygonToolImageProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnDrawPolygonToolImageChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var drawToolbarView = bindable as DrawToolBarView;
      if(newValue is null && drawToolbarView is not null)
      {
        drawToolbarView.DrawPolygonToolImage = ImageSource.FromStream(() =>
          typeof(DrawToolBarView).Assembly.GetStreamEmbeddedResource(@"ic_polygon"));
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty DrawRectangleToolImageProperty = BindableProperty.Create(
      nameof(DrawRectangleToolImage),
      typeof(ImageSource),
      typeof(DrawToolBarView),
      propertyChanged: OnDrawRectangleToolImageChanged);

    /// <summary>
    /// 
    /// </summary>
    public ImageSource DrawRectangleToolImage
    {
      get => (ImageSource)GetValue(DrawRectangleToolImageProperty);
      set => SetValue(DrawRectangleToolImageProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnDrawRectangleToolImageChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var drawToolbarView = bindable as DrawToolBarView;
      if(newValue is null && drawToolbarView is not null)
      {
        drawToolbarView.DrawRectangleToolImage = ImageSource.FromStream(() =>
          typeof(DrawToolBarView).Assembly.GetStreamEmbeddedResource(@"ic_rectangle"));
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty DrawEraseToolImageProperty = BindableProperty.Create(
      nameof(DrawEraseToolImage),
      typeof(ImageSource),
      typeof(DrawToolBarView),
      propertyChanged: OnDrawEraseToolImageChanged);

    /// <summary>
    /// 
    /// </summary>
    public ImageSource DrawEraseToolImage
    {
      get => (ImageSource)GetValue(DrawEraseToolImageProperty);
      set => SetValue(DrawEraseToolImageProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnDrawEraseToolImageChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var drawToolbarView = bindable as DrawToolBarView;
      if(newValue is null && drawToolbarView is not null)
      {
        drawToolbarView.DrawEraseToolImage = ImageSource.FromStream(() =>
          typeof(DrawToolBarView).Assembly.GetStreamEmbeddedResource(@"ic_erase"));
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty DrawFreehandLineToolImageProperty = BindableProperty.Create(
      nameof(DrawFreehandLineToolImage),
      typeof(ImageSource),
      typeof(DrawToolBarView),
      propertyChanged: DrawFreehandLineToolImageChanged);

    /// <summary>
    /// 
    /// </summary>
    public ImageSource DrawFreehandLineToolImage
    {
      get => (ImageSource)GetValue(DrawFreehandLineToolImageProperty);
      set => SetValue(DrawFreehandLineToolImageProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void DrawFreehandLineToolImageChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var drawToolbarView = bindable as DrawToolBarView;
      if(newValue is null && drawToolbarView is not null)
      {
        drawToolbarView.DrawFreehandLineToolImage = ImageSource.FromStream(() =>
          typeof(DrawToolBarView).Assembly.GetStreamEmbeddedResource(@"ic_erase"));
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty DrawTextToolImageProperty = BindableProperty.Create(
      nameof(DrawTextToolImage),
      typeof(ImageSource),
      typeof(DrawToolBarView),
      propertyChanged: DrawTextToolImageChanged);

    /// <summary>
    /// 
    /// </summary>
    public ImageSource DrawTextToolImage
    {
      get => (ImageSource)GetValue(DrawTextToolImageProperty);
      set => SetValue(DrawTextToolImageProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void DrawTextToolImageChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var drawToolbarView = bindable as DrawToolBarView;
      if(newValue is null && drawToolbarView is not null)
      {
        drawToolbarView.DrawTextToolImage = ImageSource.FromStream(() =>
          typeof(DrawToolBarView).Assembly.GetStreamEmbeddedResource(@"ic_text"));
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty DrawNoneToolImageProperty = BindableProperty.Create(
      nameof(DrawNoneToolImage),
      typeof(ImageSource),
      typeof(DrawToolBarView),
      propertyChanged: OnDrawNoneToolImageChanged);

    /// <summary>
    /// 
    /// </summary>
    public ImageSource DrawNoneToolImage
    {
      get => (ImageSource)GetValue(DrawNoneToolImageProperty);
      set => SetValue(DrawNoneToolImageProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnDrawNoneToolImageChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var drawToolbarView = bindable as DrawToolBarView;
      if(newValue is null && drawToolbarView is not null)
      {
        drawToolbarView.DrawNoneToolImage = ImageSource.FromStream(() =>
          typeof(DrawToolBarView).Assembly.GetStreamEmbeddedResource(@"ic_cancel"));
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public ICommand OKCommand { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public ModalPanelView Dialog { get; set; }

    /// <summary>
    /// 
    /// </summary>
    private DrawingProcess DrawingProcess { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public Geometry? Geometry { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public DrawToolBarView()
    {
      InitializeComponent();

      IsVisible = false;
      Orientation = StackOrientation.Horizontal;

      var asm = GetType().Assembly;

      DrawPointToolImage = ImageSource.FromStream(() => asm.GetStreamEmbeddedResource(@"ic_point"));
      DrawPolylineToolImage = ImageSource.FromStream(() => asm.GetStreamEmbeddedResource(@"ic_polyline"));
      DrawPolygonToolImage = ImageSource.FromStream(() => asm.GetStreamEmbeddedResource(@"ic_polygon"));
      DrawRectangleToolImage = ImageSource.FromStream(() => asm.GetStreamEmbeddedResource(@"ic_rectangle"));
      DrawEraseToolImage = ImageSource.FromStream(() => asm.GetStreamEmbeddedResource(@"ic_erase"));
      DrawFreehandLineToolImage = ImageSource.FromStream(() => asm.GetStreamEmbeddedResource(@"ic_freehandline"));
      DrawTextToolImage = ImageSource.FromStream(() => asm.GetStreamEmbeddedResource(@"ic_text"));
      DrawNoneToolImage = ImageSource.FromStream(() => asm.GetStreamEmbeddedResource(@"ic_cancel"));

      DrawingProcess = new DrawingProcess()
      {
        GeometryEditor = new GeometryEditor(),
        Color = Color,
        DrawGraphicsOverlay = new GraphicsOverlay() { Id = DrawGraphicsOverlayId }
      };
      DrawingProcess.PropertyChanged += DrawingProcessPropertyChanged;

      OKCommand = new DelegateCommand<string>(DrawText);
      Dialog = new DrawTextToolDialog() { AcceptCommand = OKCommand };
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
    private void SketchEditorGeometryChanged(object sender, GeometryChangedEventArgs e)
    {
      var newGeometry = e.NewGeometry;
      if(newGeometry is not null)
      {
        Debug.WriteLine($"NewGeometry: {newGeometry.ToJson()}");
        if(Geometry is not null && Geometry.Equals(newGeometry))
        {
          IsDrawing = false;
        }
        else
        {
          Geometry = newGeometry;
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>7
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DrawPointToolClicked(object sender, EventArgs e) => DrawingProcess.StartDrawGeometry(GeometryType.Point);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DrawPolylineToolClicked(object sender, EventArgs e) => DrawingProcess.StartDrawGeometry(GeometryType.Polyline);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DrawFreehandLineToolClicked(object sender, EventArgs e) => DrawingProcess.StartDrawGeometry(GeometryType.Polyline);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DrawTextToolClicked(object sender, EventArgs e)
    {
      if(Parent is Layout layout)
      {
        if(!layout.Children.Contains(Dialog))
        {
          layout.Children.Add(Dialog);
        }
        Dialog.IsVisible = true;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    private void DrawText(string text) => DrawingProcess.StartDrawGeometry(GeometryType.Point, text);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DrawPolygonToolClicked(object sender, EventArgs e) => DrawingProcess.StartDrawGeometry(GeometryType.Polygon);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DrawRectangleToolClicked(object sender, EventArgs e) => DrawingProcess.StartDrawGeometry(GeometryType.Envelope);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DrawEraseToolClicked(object sender, EventArgs e)
    {
      if(DrawingProcess is not null && DrawingProcess.DrawGraphicsOverlay is not null)
      {
        DrawingProcess.DrawGraphicsOverlay.Graphics.Clear();
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DrawNoneToolClicked(object sender, EventArgs e)
    {
      if(DrawingProcess is not null && DrawingProcess.GeometryEditor is not null)
      {
        DrawingProcess.GeometryEditor.Stop();
        IsDrawing = false;
      }
    }
  }
}