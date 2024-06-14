using System.Windows.Input;

using EsriCo.ArcGisMaps.Maui.Extensions;
using EsriCo.ArcGisMaps.Maui.Services;

namespace EsriCo.ArcGisMaps.Maui.UI
{
  /// <summary>
  /// 
  /// </summary>
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class PortalConnectionsView : ContentView
  {
    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty PortalConnectionsProperty = BindableProperty.Create(
      nameof(PortalConnections),
      typeof(List<PortalConnection>),
      typeof(PortalConnectionsView),
      defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// 
    /// </summary>
    public List<PortalConnection> PortalConnections
    {
      get => (List<PortalConnection>)GetValue(PortalConnectionsProperty);
      set => SetValue(PortalConnectionsProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty AddAccountCommandProperty = BindableProperty.Create(
      nameof(AddAccountCommand),
      typeof(ICommand),
      typeof(PortalConnectionsView));

    /// <summary>
    /// 
    /// </summary>
    public ICommand AddAccountCommand
    {
      get => (ICommand)GetValue(AddAccountCommandProperty);
      set => SetValue(AddAccountCommandProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public ImageSource LoginImage { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public ImageSource ActiveImage { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public ICommand CloseCommand { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public PortalConnectionsView()
    {
      InitializeComponent();
      var asm = GetType().Assembly;

      LoginImage = ImageSource.FromStream(() => asm.GetStreamEmbeddedResource(@"ic_key"));
      ActiveImage = ImageSource.FromStream(() => asm.GetStreamEmbeddedResource(@"ic_checked"));
      CloseCommand = new DelegateCommand(() => IsVisible = false);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CloseButtonClicked(object sender, System.EventArgs e) => IsVisible = false;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void AddPortalButtonClicked(object sender, EventArgs e)
    {
      if(AddAccountCommand != null && AddAccountCommand.CanExecute(null))
      {
        AddAccountCommand.Execute(null);
      }
    }
  }
}