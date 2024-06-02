using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsriCo.ArcGisMaps.Maui.Services
{
  /// <summary>
  /// 
  /// </summary>
  /// <param name="message"></param>
  public class ReplicaMangerException(string message) : ApplicationException(message)
  {
  }
}
