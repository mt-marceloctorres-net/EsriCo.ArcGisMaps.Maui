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
  public class SynchronizeReplicaResult
  {
    /// <summary>
    /// 
    /// </summary>
    public bool Synchronized { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public List<SyncReplicaErrorResult> ResultErrors { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public SynchronizeReplicaResult() => ResultErrors = [];
  }
}
