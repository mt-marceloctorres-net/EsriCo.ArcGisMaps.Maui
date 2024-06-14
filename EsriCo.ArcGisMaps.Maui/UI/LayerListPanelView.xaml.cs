using System.Collections.ObjectModel;

using Esri.ArcGISRuntime;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Maui;

using EsriCo.ArcGisMaps.Maui.Model;

using Mapping = Esri.ArcGISRuntime.Mapping;

namespace EsriCo.ArcGisMaps.Maui.UI
{
  /// <summary>
  /// 
  /// </summary>
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class LayerListPanelView : ListPanelView
  {
    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty MapViewProperty = BindableProperty.Create(
      nameof(MapView),
      typeof(MapView),
      typeof(LayerListPanelView),
      propertyChanged: OnMapViewPropertyChanged);

    /// <summary>
    /// 
    /// </summary>
    public MapView MapView
    {
      get => (MapView)GetValue(MapViewProperty);
      set => SetValue(MapViewProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnMapViewPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var panelView = bindable as LayerListPanelView;
      if(panelView is not null && newValue is MapView newMapView)
      {
        if(newMapView.Map is not null)
        {
          panelView.SetMap(newMapView.Map).Await();
        }
        else
        {
          newMapView.PropertyChanged += (s, e) =>
          {
            if(e.PropertyName == nameof(newMapView.Map) && newMapView.Map is not null)
            {
              panelView.SetMap(newMapView.Map).Await();
            }
          };
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private Mapping.Map? Map { get; set; }

    /// <summary>
    /// 
    /// </summary>
    private ObservableCollection<LayerInfoList>? _layerInfoList;

    /// <summary>
    /// 
    /// </summary>
    public ObservableCollection<LayerInfoList>? LayerInfoList
    {
      get => _layerInfoList;
      set
      {
        _layerInfoList = value;
        OnPropertyChanged(nameof(LayerInfoList));
      }
    }

    private bool CollectionHandlerAdded;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>

    private async Task SetLoadedMapAsync()
    {
      if(Map is not null)
      {
        if(Map.OperationalLayers.Count > 0)
        {
          await SetLayerInfoListAsync();
        }
        else if(!CollectionHandlerAdded)
        {
          Map.OperationalLayers.CollectionChanged += async (o, e) => await SetLayerInfoListAsync();
          CollectionHandlerAdded = true;
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="map"></param>
    protected async Task SetMap(Mapping.Map map)
    {
      try
      {
        if(map != null)
        {
          Map = map;
          if(Map.LoadStatus != LoadStatus.Loaded)
          {
            Map.Loaded += async (o, e) => await SetLoadedMapAsync();
            await Map.LoadAsync();
          }
          else
          {
            await SetLoadedMapAsync();
          }
        }
      }
      catch(Exception ex)
      {
        throw new LayerListPanelException(message: ex.Message, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private async Task SetLayerInfoListFromLoadedMapAsync()
    {
      var bindableObject = this as BindableObject;

      var layerInfoList = await bindableObject.Dispatcher.DispatchAsync(GetLayerInfoFromLoadedMapAsync);
      LayerInfoList = layerInfoList != null ?
        new ObservableCollection<LayerInfoList>(layerInfoList) :
        null;
    }

    /// <summary>
    /// 
    /// </summary>
    private async Task<List<LayerInfoList>> GetLayerInfoFromLoadedMapAsync()
    {
      var listLayerInfoList = new List<LayerInfoList>();
      if(Map is not null && Map.OperationalLayers is not null)
      {
        foreach(var ol in Map.OperationalLayers)
        {
          var layerInfoList = new LayerInfoList()
          {
            GroupLayerInfo = new LayerInfo { Layer = ol }
          };
          ol.SublayerContents
                .ToList()
                .ForEach(sl => layerInfoList.SubLayerInfoList.Add(new LayerInfo()
                {
                  ParentInfo = layerInfoList.GroupLayerInfo,
                  Layer = sl as Layer
                }));
          listLayerInfoList.Add(layerInfoList);

          var legendInfoList = await layerInfoList.GroupLayerInfo.Layer.GetLegendInfosAsync();
          await layerInfoList.GroupLayerInfo.SetLegendInfosAsync(legendInfoList);

          foreach(var sli in layerInfoList.SubLayerInfoList)
          {
            var subLegendInfoList = sli is not null && sli.Layer is not null ? await sli.Layer.GetLegendInfosAsync() : null;
            if(sli is not null && subLegendInfoList is not null)
            {
              await sli.SetLegendInfosAsync(subLegendInfoList);
            }
          }
        }
      }
      return listLayerInfoList;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private async Task SetLayerInfoListAsync()
    {
      if(Map != null)
      {
        if(Map.LoadStatus != LoadStatus.Loaded)
        {
          Map.Loaded += async (o, e) => await SetLayerInfoListFromLoadedMapAsync();
          await Map.LoadAsync();
        }
        else
        {
          await SetLayerInfoListFromLoadedMapAsync();
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public LayerListPanelView() => InitializeComponent();
  }
}