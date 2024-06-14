using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Esri.ArcGISRuntime.Mapping;

using Prism.Mvvm;

namespace EsriCo.ArcGisMaps.Maui.Model
{
  /// <summary>
  /// 
  /// </summary>
  public class LayerInfo : BindableBase
  {
    private Layer? _layer;
    private LayerInfo? _parentInfoLayer;
    private List<LegendImageInfo>? _legendInfos;

    /// <summary>
    /// 
    /// </summary>
    public Layer? Layer
    {
      get => _layer;
      set => SetProperty(ref _layer, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public LayerInfo? ParentInfo
    {
      get => _parentInfoLayer;
      set => SetProperty(ref _parentInfoLayer, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public List<LegendImageInfo>? LegendImageInfos
    {
      get => _legendInfos;
      set => SetProperty(ref _legendInfos, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="legendInfos"></param>
    public async Task SetLegendInfosAsync(IEnumerable<LegendInfo> legendInfos)
    {
      if(LegendImageInfos == null)
      {
        LegendImageInfos = [];
      }
      else
      {
        LegendImageInfos.Clear();
      }

      foreach(var li in legendInfos.ToList())
      {
        var imageData = li.Symbol != null ? await li.Symbol.CreateSwatchAsync() : null;
        var stream = imageData != null ? await imageData.GetEncodedBufferAsync() : null;
        if(stream != null)
        {
          LegendImageInfos.Add(new LegendImageInfo()
          {
            Name = li.Name,
            ImageSource = ImageSource.FromStream(() => stream)
          });
        }
      }
    }
  }
}
