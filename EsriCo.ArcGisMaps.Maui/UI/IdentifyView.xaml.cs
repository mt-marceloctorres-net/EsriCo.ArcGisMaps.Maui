using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Mapping.Popups;

using EsriCo.ArcGisMaps.Maui.Behaviors;

namespace EsriCo.ArcGisMaps.Maui.UI
{
  /// <summary>
  /// 
  /// </summary>
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class IdentifyView : ListPanelView
  {
    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty IdentifyResultsProperty = BindableProperty.Create(
      nameof(IdentifyResults),
      typeof(IdentifyResults),
      typeof(IdentifyView),
      propertyChanged: OnIdentifyResultsChanged);

    /// <summary>
    /// 
    /// </summary>
    public IdentifyResults? IdentifyResults
    {
      get => (IdentifyResults)GetValue(IdentifyResultsProperty);
      set => SetValue(IdentifyResultsProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    private PopupManager? _popupManager;

    /// <summary>
    /// 
    /// </summary>
    public PopupManager? PopupManager
    {
      get => _popupManager;
      set
      {
        _popupManager = value;
        OnPropertyChanged(nameof(PopupManager));
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty CurrentElementIndexProperty = BindableProperty.Create(
      nameof(CurrentElementIndex),
      typeof(int),
      typeof(IdentifyView),
      defaultBindingMode: BindingMode.OneWayToSource);

    /// <summary>
    /// 
    /// </summary>
    public int CurrentElementIndex
    {
      get => (int)GetValue(CurrentElementIndexProperty);
      set => SetValue(CurrentElementIndexProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public IdentifyView() => InitializeComponent();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnIdentifyResultsChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var identifyView = bindable as IdentifyView;
      var identifyResults = newValue as IdentifyResults;
      var oldIdentifyResults = oldValue as IdentifyResults;

      if(identifyView is not null && identifyResults is not null && oldIdentifyResults is not null)
      {
        identifyView.ClearSelection(oldIdentifyResults);

        identifyView.CurrentElementIndex = 0;
        identifyView.NextButton.IsEnabled = identifyResults.GeoElementResults.Count > 1;
        identifyView.PreviousButton.IsEnabled = false;

        GetPopupManager(identifyView, identifyResults);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="identifyView"></param>
    /// <param name="identifyResults"></param>
    private static void GetPopupManager(IdentifyView identifyView, IdentifyResults? identifyResults)
    {
      try
      {
        Popup? popup = null;
        var geoElementResult = identifyResults?.GeoElementResults[identifyView.CurrentElementIndex];
        if(geoElementResult?.Layer is FeatureLayer)
        {
          var featureLayer = geoElementResult.Layer as FeatureLayer;
          if(featureLayer is not null && geoElementResult.GeoElement is Feature)
          {
            var feature = geoElementResult.GeoElement as Feature;
            if(feature is not null)
            {
              featureLayer.SelectFeature(feature);
            }
          }
          identifyView.TitleText = featureLayer?.FeatureTable is not null
            ? featureLayer.FeatureTable.DisplayName
            : string.Empty;
        }

        if(geoElementResult?.Layer is IPopupSource && geoElementResult.GeoElement is not null)
        {
          var popupSource = geoElementResult.Layer as IPopupSource;
          var popupDefinition = popupSource?.PopupDefinition;
          popup = popupDefinition is not null ?
            new Popup(geoElementResult.GeoElement, popupDefinition) :
            Popup.FromGeoElement(geoElementResult.GeoElement);
        }
        else if(geoElementResult?.GeoElement is not null)
        {
          popup = Popup.FromGeoElement(geoElementResult.GeoElement);
        }
        if(popup != null)
        {
          var popManager = new PopupManager(popup);
          identifyView.PopupManager = popManager;
        }
        identifyView.StatusText = $"{identifyView.CurrentElementIndex + 1} / {identifyResults?.GeoElementResults.Count}";
      }
      catch(Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void ClearSelection(IdentifyResults? identifyResults = null)
    {
      identifyResults ??= IdentifyResults;
      if(identifyResults is not null && identifyResults.GeoElementResults.Count > 0)
      {
        var geoElementResult = identifyResults.GeoElementResults[CurrentElementIndex];
        if(geoElementResult?.Layer is FeatureLayer)
        {
          var featureLayer = geoElementResult.Layer as FeatureLayer;
          featureLayer?.ClearSelection();
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PreviousResulteClicked(object sender, EventArgs e)
    {
      ClearSelection();
      CurrentElementIndex -= 1;
      PreviousButton.IsEnabled = CurrentElementIndex > 0;
      NextButton.IsEnabled = CurrentElementIndex < IdentifyResults?.GeoElementResults.Count - 1;
      GetPopupManager(this, IdentifyResults);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void NextResultClicked(object sender, EventArgs e)
    {
      ClearSelection();

      CurrentElementIndex += 1;
      NextButton.IsEnabled = CurrentElementIndex < IdentifyResults?.GeoElementResults.Count - 1;
      PreviousButton.IsEnabled = CurrentElementIndex > 0;
      GetPopupManager(this, IdentifyResults);

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected override void OnCloseButtonClicked(object sender, EventArgs e)
    {
      base.OnCloseButtonClicked(sender, e);
      ClearSelection();
    }
  }
}