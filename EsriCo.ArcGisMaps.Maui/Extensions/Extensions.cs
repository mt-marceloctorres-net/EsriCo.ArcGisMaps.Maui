using System.Diagnostics;
using System.Reflection;

using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;

namespace EsriCo.ArcGisMaps.Maui.Extensions
{
  /// <summary>
  /// 
  /// </summary>
  public static class Extensions
  {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="axis"></param>
    /// <returns></returns>
    public static string ToDms(this double value, string axis)
    {
      var quadrant = string.Empty;
      var sign = Math.Sign(value);
      value = Math.Abs(value);

      if(axis != null && axis != string.Empty)
      {
#pragma warning disable IDE0045 // Convertir a expresión condicional
        if(axis.Equals("X", StringComparison.CurrentCultureIgnoreCase))
        {
          quadrant = sign < 0 ? "W" : "E";
        }
        else
        {
          quadrant = sign < 0 ? "S" : "N";
        }
#pragma warning restore IDE0045 // Convertir a expresión condicional
      }

      var degree = Math.Floor(value);
      var minutes = Math.Floor((value - degree) * 60);
      var seconds = (((value - degree) * 60) - minutes) * 60;

      return string.Format("{0:0}°{1:00}'{2:00.000}\" {3}", degree, minutes, seconds, quadrant);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mapPoint"></param>
    /// <returns></returns>
    public static string ToDms(this MapPoint mapPoint) => $"Y:{mapPoint.Y.ToDms("Y")}, X:{mapPoint.X.ToDms("X")}";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="vp"></param>
    /// <param name="otro"></param>
    /// <returns></returns>
    public static bool AreEquals(this Viewpoint vp, Viewpoint otro)
    {

      var json = vp.ToJson();
      var jsonOtro = otro != null ? otro.ToJson() : string.Empty;
      Debug.WriteLine($"Viewpoint 1:{json}");
      Debug.WriteLine($"Viewpoint 2:{jsonOtro}");

      return json == jsonOtro;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="vp"></param>
    /// <param name="bounds"></param>
    /// <param name="center"></param>
    /// <returns></returns>
    public static bool AreEquals(this Viewpoint vp, Viewpoint bounds, Viewpoint center) => vp.TargetGeometry.IsEqual(bounds.TargetGeometry) ||
        (vp.TargetGeometry.IsEqual(center.TargetGeometry) &&
          vp.TargetScale.Equals(center.TargetScale));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="imageString"></param>
    /// <returns></returns>
    public static ImageSource ToImageSource(this string imageString)
    {
      var bytes = Convert.FromBase64String(imageString);
      return ImageSource.FromStream(() => new MemoryStream(bytes));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string ToBase64String(this Stream s)
    {
      byte[]? bytes = null;
      using(var ms = new MemoryStream())
      {
        s.CopyTo(ms);
        bytes = ms.ToArray();
      }
      return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="assembly"></param>
    /// <param name="resource"></param>
    /// <returns></returns>
    public static Stream? GetStreamEmbeddedResource(this Assembly assembly, string resource)
    {
      var fullName = Array.Find(assembly.GetManifestResourceNames(), s => s.Contains(resource));
      return !string.IsNullOrEmpty(fullName) ? assembly.GetManifestResourceStream(fullName) : null;
    }
  }
}
