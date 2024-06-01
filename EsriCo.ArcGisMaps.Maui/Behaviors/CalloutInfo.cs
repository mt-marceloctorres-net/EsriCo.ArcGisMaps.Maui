using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.UI;

namespace EsriCo.ArcGisMaps.Maui.Behaviors
{
  /// <summary>
  /// 
  /// </summary>
  public class CalloutInfo
  {
    /// <summary>
    /// 
    /// </summary>
    public GeoElement? GeoElement { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public MapPoint? Point { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public CalloutDefinition? CalloutDefinition { get; set; }
  }
}
