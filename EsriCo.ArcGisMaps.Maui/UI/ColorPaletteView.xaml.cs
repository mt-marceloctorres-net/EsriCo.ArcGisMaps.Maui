using System.Collections.ObjectModel;

using EsriCo.ArcGisMaps.Maui.Model;

using Drawing = System.Drawing;

namespace EsriCo.ArcGisMaps.Maui.UI
{
  /// <summary>
  /// 
  /// </summary>
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class ColorPaletteView : ContentView
  {
    /// <summary>
    /// 
    /// </summary>
    public ObservableCollection<ColorInfo>? ColorList { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public ColorPaletteView()
    {
      var t = typeof(Color);
      var color = Activator.CreateInstance(t);
      if(color is not null)
      {
        var colors = t.GetFields()
          .Where(f => f.IsStatic && f.IsInitOnly)
          .Select(f => new ColorInfo { Name = f.Name, Color = f.GetValue(color) is not null ? (Drawing.Color)f.GetValue(color) : Drawing.Color.White });
        ColorList = new ObservableCollection<ColorInfo>(colors);
      }
      InitializeComponent();
    }
  }
}