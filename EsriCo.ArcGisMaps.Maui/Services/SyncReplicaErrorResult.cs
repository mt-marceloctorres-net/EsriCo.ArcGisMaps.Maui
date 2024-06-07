using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsriCo.ArcGisMaps.Maui.Services
{
  /// <summary>
  /// 
  /// </summary>
  public class SyncReplicaErrorResult
  {
    /// <summary>
    /// 
    /// </summary>
    public string? Name { get; set; }

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
