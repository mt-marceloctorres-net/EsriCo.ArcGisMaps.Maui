using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsriCo.ArcGisMaps.Maui.Model
{
  /// <summary>
  /// 
  /// </summary>
  public class LayerInfos : List<LayerInfo>
  {
    /// <summary>
    /// 
    /// </summary>
    public LayerInfo? GroupLayerInfo { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public List<LayerInfo> SubLayerInfos => this;
  }
}
