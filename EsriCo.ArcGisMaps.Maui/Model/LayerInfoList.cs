namespace EsriCo.ArcGisMaps.Maui.Model
{
  /// <summary>
  /// 
  /// </summary>
  public class LayerInfoList : List<LayerInfo>
  {
    /// <summary>
    /// 
    /// </summary>
    public LayerInfo? GroupLayerInfo { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public List<LayerInfo> SubLayerInfoList => this;
  }
}
