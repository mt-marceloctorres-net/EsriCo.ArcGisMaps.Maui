using System.Threading.Tasks;

namespace EsriCo.ArcGisMaps.Maui.Services
{
  /// <summary>
  /// 
  /// </summary>
  public interface IConfigurationInfo
  {
    /// <summary>
    /// 
    /// </summary>
    string? FileName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    string Folder { get; set; }

    /// <summary>
    /// 
    /// </summary>
    bool IsLoaded { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    bool FileExist();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task LoadAsync();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task SaveAsync();
  }
}