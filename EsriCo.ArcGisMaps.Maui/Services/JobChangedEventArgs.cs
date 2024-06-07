using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Esri.ArcGISRuntime.Tasks;

namespace EsriCo.ArcGisMaps.Maui.Services
{
  /// <summary>
  /// 
  /// </summary>
  public class JobChangedEventArgs : EventArgs
  {
    /// <summary>
    /// 
    /// </summary>
    public JobStatus Status { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public IReadOnlyList<JobMessage>? Messages { get; set; }
  }
}
