using System.Runtime.CompilerServices;

using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Maui;

using EsriCo.ArcGisMaps.Maui.Extensions;

using Prism.Behaviors;

namespace EsriCo.ArcGisMaps.Maui.Behaviors
{
  /// <summary>
  /// 
  /// </summary>
  public class SetViewpointBehavior : BehaviorBase<MapView> {
    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty VisibleAreaProperty = BindableProperty.Create(
      nameof(VisibleArea),
      typeof(Polygon),
      typeof(SetViewpointBehavior),
      defaultBindingMode: BindingMode.OneWayToSource);

    /// <summary>
    /// 
    /// </summary>
    public Polygon? VisibleArea {
      get => (Polygon)GetValue(VisibleAreaProperty);
      set => SetValue(VisibleAreaProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty MapScaleProperty = BindableProperty.Create(
      nameof(MapScale),
      typeof(double),
      typeof(SetViewpointBehavior),
      defaultBindingMode: BindingMode.OneWayToSource);

    /// <summary>
    /// 
    /// </summary>
    public double MapScale {
      get => (double)GetValue(MapScaleProperty);
      set => SetValue(MapScaleProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty ViewpointProperty = BindableProperty.Create(
      nameof(Viewpoint),
      typeof(Viewpoint),
      typeof(SetViewpointBehavior));

    /// <summary>
    /// 
    /// </summary>
    public Viewpoint Viewpoint {
      get => (Viewpoint)GetValue(ViewpointProperty);
      set => SetValue(ViewpointProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="propertyName"></param>
    protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
      base.OnPropertyChanged(propertyName);
      if(propertyName == nameof(Viewpoint)) {
        SetViewpoint(Viewpoint).Await();
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="viewpoint"></param>
    private async Task SetViewpoint(Viewpoint viewpoint) {
      if(viewpoint != null) {
        var currentViewpoint = AssociatedObject.GetCurrentViewpoint(ViewpointType.BoundingGeometry);
        var equals = currentViewpoint?.AreEquals(viewpoint);
        if(equals.HasValue && !equals.Value) {
          _ = await AssociatedObject.SetViewpointAsync(viewpoint);
          VisibleArea = AssociatedObject.VisibleArea;
          MapScale = AssociatedObject.MapScale;
        }
      }
    }
  }
}
