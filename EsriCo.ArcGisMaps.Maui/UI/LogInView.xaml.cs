﻿using System.Windows.Input;

namespace EsriCo.ArcGisMaps.Maui.UI
{
  /// <summary>
  /// 
  /// </summary>
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class LogInView : ModalPanelView
  {
    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty LogInCommandProperty = BindableProperty.Create(
      nameof(LogInCommand),
      typeof(ICommand),
      typeof(LogInView));

    /// <summary>
    /// 
    /// </summary>
    public ICommand LogInCommand
    {
      get => (ICommand)GetValue(LogInCommandProperty);
      set => SetValue(LogInCommandProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty CancelCommandProperty = BindableProperty.Create(
      nameof(CancelCommand),
      typeof(ICommand),
      typeof(LogInView));

    /// <summary>
    /// 
    /// </summary>
    public ICommand CancelCommand
    {
      get => (ICommand)GetValue(CancelCommandProperty);
      set => SetValue(CancelCommandProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty UsernameProperty = BindableProperty.Create(
      nameof(Username),
      typeof(string),
      typeof(LogInView),
      defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// 
    /// </summary>
    public string Username
    {
      get => (string)GetValue(UsernameProperty);
      set => SetValue(UsernameProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty PasswordProperty = BindableProperty.Create(
      nameof(Password),
      typeof(string),
      typeof(LogInView),
      defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// 
    /// </summary>
    public string Password
    {
      get => (string)GetValue(PasswordProperty);
      set => SetValue(PasswordProperty, value);
    }

    private bool _isPasswordHidden;

    /// <summary>
    /// 
    /// </summary>
    public bool IsPasswordHidden
    {
      get => _isPasswordHidden;
      set
      {
        _isPasswordHidden = value;
        OnPropertyChanged(nameof(IsPasswordHidden));
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public LogInView()
    {
      try
      {
        InitializeComponent();
        IsPasswordHidden = true;
      }
      catch(Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
    }
  }
}