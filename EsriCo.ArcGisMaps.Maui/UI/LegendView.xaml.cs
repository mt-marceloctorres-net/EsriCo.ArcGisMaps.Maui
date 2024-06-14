using EsriCo.ArcGisMaps.Maui.Model;

namespace EsriCo.ArcGisMaps.Maui.UI
{
  /// <summary>
  /// 
  /// </summary>
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class LegendView : LayerListPanelView
  {
    /// <summary>
    /// 
    /// </summary>
    public double ItemRenderHeight { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public LegendView()
    {
      ItemRenderHeight = 25;
      InitializeComponent();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnInnerListViewItemAppearing(object sender, ItemVisibilityEventArgs e)
    {
      if(sender is ListView listView)
      {
        var list = (List<LegendImageInfo>)listView.ItemsSource;
        var height = list.Count * ItemRenderHeight;
        listView.HeightRequest = height;
      }
    }
  }
}