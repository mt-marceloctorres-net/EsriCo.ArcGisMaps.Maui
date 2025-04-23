using System.Runtime.CompilerServices;

namespace EsriCo.ArcGisMaps.Maui.UI
{
  /// <summary>
  /// 
  /// </summary>
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class ModalPanelView : PanelView
  {
    /// <summary>
    /// 
    /// </summary>
    private readonly string ColorKey = "DarkGrayTransparent";

    /// <summary>
    /// 
    /// </summary>
    private Frame? ModalFrame { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public ModalPanelView()
    {
      InitializeComponent();
      CreateModalFrame();
    }

    /// <summary>
    /// 
    /// </summary>
    private void CreateModalFrame()
    {
      var resource = Resources.MergedDictionaries
        .Where(r => r.ContainsKey(ColorKey))
        .Select(r => r[ColorKey]).FirstOrDefault();
      var backColor = resource != null ? (Color)resource : Colors.Gray;
      ModalFrame = new Frame()
      {
        BackgroundColor = backColor,
        Padding = 0,
        Margin = 0,
        HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, true),
        VerticalOptions = new LayoutOptions(LayoutAlignment.Fill, true),
        ZIndex = -1
      };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="propertyName"></param>
    protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
      base.OnPropertyChanged(propertyName);
      if(propertyName == nameof(IsVisible))
      {
        if(IsVisible)
        {
          InsertModalFrame();
        }
        else
        {
          RemoveModalFrame();
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void InsertModalFrame()
    {
      if(Parent is Layout layout && ModalFrame is not null && !layout.Contains(ModalFrame))
      {
        ZIndex = 1000;
        layout.Remove(this);
        layout.Add(ModalFrame);
        layout.Add(this);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void RemoveModalFrame()
    {
      if(Parent is Layout layout && layout.Children.Contains(ModalFrame))
      {
        _ = layout.Children.Remove(ModalFrame);
      }
    }
  }
}