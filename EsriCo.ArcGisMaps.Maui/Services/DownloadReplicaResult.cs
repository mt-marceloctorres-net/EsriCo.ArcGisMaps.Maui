using Map = Esri.ArcGISRuntime.Mapping.Map;

namespace EsriCo.ArcGisMaps.Maui.Services
{
  /// <summary>
  /// 
  /// </summary>
  public class DownloadReplicaResult
  {
    /// <summary>
    /// 
    /// </summary>
    public Map? Map { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? MapTitle { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public List<DownloadReplicaErrorResult>? ResultErrors { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DownloadReplicaResult() => ResultErrors = [];
  }
}
