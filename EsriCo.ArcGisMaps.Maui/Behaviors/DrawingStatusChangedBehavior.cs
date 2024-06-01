using System.Windows.Input;

using Esri.ArcGISRuntime.Maui;
using Esri.ArcGISRuntime.UI;

using Prism.Behaviors;

namespace EsriCo.ArcGisMaps.Maui.Behaviors
{
  /// <summary>
  /// 
  /// </summary>
  public class DrawingStatusChangedBehavior : BehaviorBase<GeoView>
  {
    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty CommandProperty = BindableProperty.Create(
      nameof(Command),
      typeof(ICommand),
      typeof(DrawingStatusChangedBehavior));

    /// <summary>
    /// 
    /// </summary>
    public ICommand Command
    {
      get => (ICommand)GetValue(CommandProperty);
      set => SetValue(CommandProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    protected override void OnAttachedTo(GeoView bindable)
    {
      base.OnAttachedTo(bindable);
      bindable.DrawStatusChanged += DrawStatusChangedEventHandler;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    protected override void OnDetachingFrom(GeoView bindable)
    {
      base.OnDetachingFrom(bindable);
      bindable.DrawStatusChanged -= DrawStatusChangedEventHandler;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DrawStatusChangedEventHandler(object? sender, DrawStatusChangedEventArgs e)
    {
      if(Command != null)
      {
        var inProgress = e.Status == DrawStatus.InProgress;
        if(Command.CanExecute(inProgress))
        {
          Command.Execute(inProgress);
        }
      }
    }
  }
}
