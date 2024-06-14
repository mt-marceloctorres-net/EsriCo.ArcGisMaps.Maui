namespace EsriCo.ArcGisMaps.Maui.Model
{
  /// <summary>
  /// 
  /// </summary>
  public class LayerListPanelException(string message, Exception innerException) : ApplicationException(message, innerException)
  {
  }
}
