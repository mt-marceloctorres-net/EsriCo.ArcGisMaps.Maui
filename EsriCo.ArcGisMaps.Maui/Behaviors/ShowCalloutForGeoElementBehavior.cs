using System.Runtime.CompilerServices;

using Esri.ArcGISRuntime.Maui;

using Prism.Behaviors;

namespace EsriCo.ArcGisMaps.Maui.Behaviors
{
  /// <summary>
  /// 
  /// </summary>
  public class ShowCalloutForGeoElementBehavior : BehaviorBase<MapView>
  {
    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty CalloutInfoProperty = BindableProperty
      .Create(nameof(CalloutInfo), typeof(CalloutInfo), typeof(ShowCalloutForGeoElementBehavior));

    /// <summary>
    /// 
    /// </summary>
    public CalloutInfo CalloutInfo
    {
      get => (CalloutInfo)GetValue(CalloutInfoProperty);
      set => SetValue(CalloutInfoProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="propertyName"></param>
    protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
      base.OnPropertyChanged(propertyName);
      if(propertyName == nameof(CalloutInfo))
      {
        ShowCallout();
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void ShowCallout()
    {
      if(CalloutInfo != null && CalloutInfo.Point != null && CalloutInfo.GeoElement != null && CalloutInfo.CalloutDefinition != null)
      {
        var punto = AssociatedObject.LocationToScreen(CalloutInfo.Point);
        AssociatedObject.ShowCalloutForGeoElement(CalloutInfo.GeoElement, punto, CalloutInfo.CalloutDefinition);
      }
      else
      {
        AssociatedObject.DismissCallout();
      }
    }
  }
}
