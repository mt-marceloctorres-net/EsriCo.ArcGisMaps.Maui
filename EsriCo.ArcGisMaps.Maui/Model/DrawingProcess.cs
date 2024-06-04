using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Maui;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.UI.Editing;
using Symbology = Esri.ArcGISRuntime.Symbology;
using Drawing = System.Drawing;

using Prism.Mvvm;

namespace EsriCo.ArcGisMaps.Maui.Model
{
  /// <summary>
  /// 
  /// </summary>
  internal class DrawingProcess : BindableBase
  {
    /// <summary>
    /// 
    /// </summary>
    internal MapView? MapView { get; set; }

    /// <summary>
    /// 
    /// </summary>
    internal Drawing.Color? Color { get; set; }

    /// <summary>
    /// 
    /// </summary>
    internal string? Text { get; set; }

    /// <summary>
    /// 
    /// </summary>
    private GeometryEditor? _geometryEditor;

    /// <summary>
    /// 
    /// </summary>
    internal GeometryEditor? GeometryEditor
    {
      get => _geometryEditor;
      set => SetProperty(ref _geometryEditor, value);
    }

    private bool _isDrawing;

    /// <summary>
    /// 
    /// </summary>
    internal bool IsDrawing
    {
      get => _isDrawing;
      set => SetProperty(ref _isDrawing, value);
    }

    /// <summary>
    /// 
    /// </summary>
    internal GraphicsOverlay? DrawGraphicsOverlay { get; set; }

    /// <summary>
    /// 
    /// </summary>
    internal Action<Geometry>? PointCreated { get; set; }

    /// <summary>
    /// 
    /// </summary>
    internal Action<Geometry>? MultiPointCreated { get; set; }

    /// <summary>
    /// 
    /// </summary>
    internal Action<Geometry>? PolylineCreated { get; set; }

    /// <summary>
    /// 
    /// </summary>
    internal Action<Geometry>? FreehandLineCreated { get; set; }

    /// <summary>
    /// 
    /// </summary>
    internal Action<Geometry>? PolygonCreated { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="text"></param>
    /// <returns></returns>
    internal void StartDrawGeometry(GeometryType mode, string? text = null)
    {
      try
      {
        Text = text;
        if(MapView != null )
        {
          MapView.GeometryEditor = GeometryEditor;
          GeometryEditor ??= new GeometryEditor();
          GeometryEditor.Start(mode);
        }
      }
      catch(TaskCanceledException ex)
      {
        Debug.WriteLine(ex.Message);
      }
      catch(Exception ex)
      {
        Debug.WriteLine(ex.Message);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    internal void StopDrawGeometry()
    {
      try
      {
        var geometry = GeometryEditor?.Stop();
        if(geometry != null)
        {
          Symbol? symbol = null;

          if(Text != null)
          {
            symbol = TextSymbol(Text);
          }
          else
          {
            switch(typeof(Geometry).Name)
            {
              case "Point":
                symbol = PointSymbol();
                PointCreated?.Invoke(geometry);
                break;
              case "Multipoint":
                symbol = PointSymbol();
                MultiPointCreated?.Invoke(geometry);
                break;
              case "Polyline":
                symbol = PolylineSymbol();
                PolylineCreated?.Invoke(geometry);
                break;
              case "Polygon":
                symbol = PolygonSymbol();
                PolygonCreated?.Invoke(geometry);
                break;
            }
          }
          DrawGraphicsOverlay?.Graphics.Add(new Graphic() { Geometry = geometry, Symbol = symbol });
        }
      }
      catch(TaskCanceledException ex)
      {
        Debug.WriteLine(ex.Message);
      }
      catch(Exception ex)
      {
        Debug.WriteLine(ex.Message);
      }
      finally
      {
        IsDrawing = false;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private SimpleMarkerSymbol PointSymbol()
    {
      var color = Color != null ? Color : Drawing.Color.Red;
      var fillColor = Drawing.Color.FromArgb(128, color.Value);
      return new SimpleMarkerSymbol()
      {
        Color = fillColor,
        Size = 20,
        Style = SimpleMarkerSymbolStyle.Circle
      };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private SimpleLineSymbol PolylineSymbol() => new()
    {
      Color = Color != null ? Color.Value : Drawing.Color.Red,
      Style = SimpleLineSymbolStyle.Solid,
      Width = 2
    };

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private SimpleFillSymbol PolygonSymbol()
    {
      var color = Color != null ? Color : Drawing.Color.Red;
      var fillColor = Drawing.Color.FromArgb(128, color.Value);
      return new SimpleFillSymbol()
      {
        Color = fillColor,
        Outline = new SimpleLineSymbol()
        {
          Color = Color != null ? Color.Value : Drawing.Color.Red,
          Style = SimpleLineSymbolStyle.Solid,
          Width = 2
        },
        Style = SimpleFillSymbolStyle.Solid
      };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private static TextSymbol TextSymbol(string text) => new()
    {
      Text = text,
      Color = Drawing.Color.Black,
      HorizontalAlignment = Symbology.HorizontalAlignment.Center,
      VerticalAlignment = Symbology.VerticalAlignment.Middle,
      Size = 20
    };
  }
}
