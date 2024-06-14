using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Mapping;

namespace EsriCo.ArcGisMaps.Maui.Behaviors
{
  /// <summary>
  /// 
  /// </summary>
  public class IdentifyGeoElementResult : BindableBase
  {
    private GeoElement? _geoElement;

    /// <summary>
    /// 
    /// </summary>
    public GeoElement? GeoElement
    {
      get => _geoElement;
      set => SetProperty(ref _geoElement, value);
    }

    private ILayerContent? _layer;

    /// <summary>
    /// 
    /// </summary>
    public ILayerContent? Layer
    {
      get => _layer;
      set => SetProperty(ref _layer, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public IdentifyGeoElementResult() { }
  }
}
