using System.Collections.ObjectModel;

namespace EsriCo.ArcGisMaps.Maui.UI
{
  /// <summary>
  /// 
  /// </summary>
  public class ColorInfo
  {
    /// <summary>
    /// 
    /// </summary>
    public string? Name { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public Color? Color { get; set; }
  }

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
      if(color != null)
      {
        var colors = t.GetFields()
          .Where(f => f.IsStatic && f.IsInitOnly)
          .Select(f => new ColorInfo { Name = f.Name, Color = f.GetValue(color) as Color });
        ColorList = new ObservableCollection<ColorInfo>(colors);
      }
      InitializeComponent();
    }
  }
}