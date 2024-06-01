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
  public class LegendImageInfo : BindableBase
  {
    private ImageSource? _imageSource;
    private string? _name;

    /// <summary>
    /// 
    /// </summary>
    public ImageSource? ImageSource
    {
      get => _imageSource;
      set => SetProperty(ref _imageSource, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public string? Name
    {
      get => _name;
      set => SetProperty(ref _name, value);
    }
  }
}
