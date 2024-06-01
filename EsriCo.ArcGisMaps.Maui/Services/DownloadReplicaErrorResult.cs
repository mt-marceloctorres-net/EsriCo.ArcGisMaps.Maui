namespace EsriCo.ArcGisMaps.Maui.Services
{
  /// <summary>
  /// 
  /// </summary>
  public class DownloadReplicaErrorResult
  {
    /// <summary>
    /// 
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool? SupportOffline { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public Exception? Error { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Message { get; set; }
  }
}
