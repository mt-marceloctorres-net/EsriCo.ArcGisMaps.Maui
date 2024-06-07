using Esri.ArcGISRuntime;
using Esri.ArcGISRuntime.Portal;
using Esri.ArcGISRuntime.Security;

using EsriCo.ArcGisMaps.Maui.Extensions;

using Map = Esri.ArcGISRuntime.Mapping.Map;

namespace EsriCo.ArcGisMaps.Maui.Services
{
  /// <summary>
  /// 
  /// </summary>
  public class PortalConnection : BindableBase, IPortalConnection
  {

    private bool? _active;

    /// <summary>
    /// 
    /// </summary>
    public bool? Active
    {
      get => _active;
      set => SetProperty(ref _active, value);
    }

    private string? _user;

    /// <summary>
    /// 
    /// </summary>
    public string? User
    {
      get => _user;
      set => SetProperty(ref _user, value);
    }

    private string? _password;

    /// <summary>
    /// 
    /// </summary>
    public string? Password
    {
      get => _password;
      set => SetProperty(ref _password, value);
    }

    private string? _domain;

    /// <summary>
    /// 
    /// </summary>
    public string? Domain
    {
      get => _domain;
      set => SetProperty(ref _domain, value);
    }

    private string? _userName;

    /// <summary>
    /// 
    /// </summary>
    public string? UserName
    {
      get => _userName;
      private set => SetProperty(ref _userName, value);
    }

    private string? _organizationName;

    /// <summary>
    /// 
    /// </summary>
    public string? OrganizationName
    {
      get => _organizationName;
      private set => SetProperty(ref _organizationName, value);
    }

    private string? _organizationSubDomain;

    /// <summary>
    /// 
    /// </summary>
    public string? OrganizationSubDomain
    {
      get => _organizationSubDomain;
      private set => SetProperty(ref _organizationSubDomain, value);
    }

    private ImageSource? _imageSource;

    /// <summary>
    /// 
    /// </summary>
    public ImageSource? UserImage
    {
      get => _imageSource;
      set => SetProperty(ref _imageSource, value);
    }

    private string? _imageUserString;

    /// <summary>
    /// 
    /// </summary>
    public string? UserImageString
    {
      get => _imageUserString;
      private set => SetProperty(ref _imageUserString, value);
    }

    private string? _baseUrl;

    /// <summary>
    /// 
    /// </summary>
    public string? BaseUrl
    {
      get => _baseUrl;
      set
      {
        if(_baseUrl != value)
        {
          _baseUrl = value;
          if(!string.IsNullOrEmpty(_baseUrl))
          {
            ServerRegisterUrl = ResetSharingUrl(_baseUrl);
            ServerRegister();
          }
        }
      }
    }

    private double? _tokenValidDays;

    /// <summary>
    /// 
    /// </summary>
    public double? TokenValidDays
    {
      get => _tokenValidDays;
      set => SetProperty(ref _tokenValidDays, value);
    }

    private DateTimeOffset? _tokenExpirationDateTime;

    /// <summary>
    /// 
    /// </summary>
    public DateTimeOffset? TokenExpirationDateTime
    {
      get => _tokenExpirationDateTime;
      private set => SetProperty(ref _tokenExpirationDateTime, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public bool? SignedIn { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public string? WebMapId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? ServerRegisterUrl { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public TokenCredential? Credential { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public ArcGISPortal? Portal { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public PortalUser? PortalUser { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public PortalInfo? PortalInfo { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public TokenAuthenticationType TokenAuthenticationType { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public AuthenticationType AuthenticationType { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public Func<CredentialRequestInfo, Task<Credential>>? ChallengeHandlerAsync { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public PortalConnection()
    {
      TokenAuthenticationType = TokenAuthenticationType.ArcGISToken;
      TokenValidDays = 14;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="baseUrl"></param>
    /// <returns></returns>
    private static string ResetSharingUrl(string baseUrl)
    {
      var uriBuilder = new UriBuilder(baseUrl);
      if(!baseUrl.Contains("/sharing/rest"))
      {
        uriBuilder.Path = uriBuilder.Path.EndsWith('/') ? string.Concat(uriBuilder.Path, "sharing/rest") : string.Concat(uriBuilder.Path, "/sharing/rest");
      }
      return uriBuilder.Uri.AbsoluteUri;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private string? SubDomainUrl()
    {
      if(BaseUrl != null)
      {
        var uriBuilder = new UriBuilder(BaseUrl);
        if(PortalInfo != null)
        {
          uriBuilder.Host = !string.IsNullOrEmpty(PortalInfo.OrganizationSubdomain) ?
            string.Concat(PortalInfo.OrganizationSubdomain, ".", PortalInfo.CustomBaseDomain) :
            uriBuilder.Host;
        }
        return uriBuilder.Uri.AbsoluteUri;
      }
      return null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    public async Task<Credential> CreateCredentialAsync(CredentialRequestInfo info)
    {
      if(info != null && info.ServiceUri != null && User != null && Password != null && Domain != null)
      {
        var credential = info.AuthenticationType == AuthenticationType.NetworkCredential
          ? new ArcGISNetworkCredential(info.ServiceUri, new System.Net.NetworkCredential(User, Password, Domain))
          : await AddCredentialAsync();
        if(credential != null)
        {
          return credential;
        }
      }
      throw new PortalConnectionException("Credential is null");
    }

    /// <summary>
    /// 
    /// </summary>
    public async Task SignInAsync()
    {
      try
      {
        AuthenticationManager.Current.ChallengeHandler = ChallengeHandlerAsync != null
          ? new ChallengeHandler(ChallengeHandlerAsync)
          : new ChallengeHandler(CreateCredentialAsync);

        if(BaseUrl != null)
        {
          var loginType = await ArcGISPortal.GetLoginTypeForUriAsync(new Uri(BaseUrl));
          if(loginType is not PortalLoginType.UsernamePassword and not PortalLoginType.Unknown)
          {
            var challengeRequest = new CredentialRequestInfo
            {
              GenerateTokenOptions = new GenerateTokenOptions
              {
                TokenAuthenticationType = TokenAuthenticationType
              },
              ServiceUri = new Uri(BaseUrl)
            };
            _ = await AuthenticationManager.Current.GetCredentialAsync(challengeRequest, false);
          }

          Portal = await ArcGISPortal.CreateAsync(new Uri(BaseUrl));
          PortalInfo = Portal.PortalInfo;
          PortalUser = Portal.User;

          if(PortalUser != null)
          {
            UserImage = PortalUser.ThumbnailUri != null ?
              ImageSource.FromUri(PortalUser.ThumbnailUri) :
              ImageSource.FromStream(() => GetType().Assembly.GetStreamEmbeddedResource("ic_user"));
            GetUserImageString();
            UserName = PortalUser.FullName;
          }
          if(PortalInfo != null)
          {
            OrganizationName = PortalInfo.OrganizationName;
            OrganizationSubDomain = SubDomainUrl();
          }

          var licenseInfo = await Portal.GetLicenseInfoAsync();
          _ = ArcGISRuntimeEnvironment.SetLicense(licenseInfo);
          SignedIn = true;
        }
        else
        {
          throw new PortalConnectionException("BaseUrl is null");
        }
      }
      catch(Exception ex)
      {
        Console.WriteLine(ex);
        throw;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public void SignOut()
    {
      if(Credential != null)
      {
        AuthenticationManager.Current.RemoveCredential(Credential);
        Credential = null;
      }
      Name = null;
      OrganizationName = null;
      OrganizationSubDomain = null;
      UserName = string.Empty;
      UserImage = null;
      UserImageString = string.Empty;
      SignedIn = false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<Map?> GetMapAsync()
    {
      if(Portal != null && WebMapId != null)
      {
        var item = await PortalItem.CreateAsync(Portal, WebMapId);
        return new Map(item);
      }
      return null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="webMapId"></param>
    /// <returns></returns>
    public async Task<Map?> GetMapAsync(string? webMapId)
    {
      if(Portal != null && webMapId != null)
      {
        var item = await PortalItem.CreateAsync(Portal, webMapId);
        return new Map(item);
      }
      return null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool IsCredentialNull() => Credential == null;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<string> GetLicenseInfoJsonAsync()
    {
      if(Portal != null)
      {
        var licenseInfo = await Portal.GetLicenseInfoAsync();
        return licenseInfo.ToJson();
      }
      return string.Empty;
    }

    /// <summary>
    /// 
    /// </summary>
    private void ServerRegister()
    {
      if(ServerRegisterUrl != null)
      {
        var serverInfo = new ServerInfo(new Uri(ServerRegisterUrl))
        {
          TokenAuthenticationType = TokenAuthenticationType,
        };
        AuthenticationManager.Current.RegisterServer(serverInfo);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void GetUserImageString()
    {
      var token = CancellationToken.None;
      if(PortalUser != null)
      {
        var task = PortalUser.GetThumbnailDataAsync(token);
        var s = task.Result ?? GetType().Assembly.GetStreamEmbeddedResource("ic_user");
        UserImageString = s != null ? s.ToBase64String() : string.Empty;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private async Task<Credential?> AddCredentialAsync()
    {
      if(ServerRegisterUrl != null)
      {
        TokenExpirationDateTime = null;
        var uri = new Uri(ServerRegisterUrl);
        var now = DateTime.Now;

        Credential = await AuthenticationManager.Current.GenerateCredentialAsync(
          uri,
          User,
          Password,
          new GenerateTokenOptions
          {
            TokenAuthenticationType = TokenAuthenticationType,
            TokenExpirationInterval = TokenValidDays != null ? TimeSpan.FromMinutes(TokenValidDays.Value * 24 * 60) : TimeSpan.MinValue,
          });
        if(Credential.ExpirationDate != null)
        {
          TokenExpirationDateTime = Credential.ExpirationDate.Value;
        }

        var timeSpan = TokenExpirationDateTime != null ? TokenExpirationDateTime - now : TimeSpan.Zero;
        TokenValidDays = timeSpan.HasValue ? timeSpan.Value.TotalDays : 0;
        return Credential;

      }
      return null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="groupTitle"></param>
    /// <returns></returns>
    public PortalGroup? GetGroup(string? groupTitle)
    {
      if(PortalUser != null && groupTitle != null)
      {
        var groups = PortalUser.Groups;
        return groups.FirstOrDefault(g => g.Title == groupTitle);
      }
      return null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="group"></param>
    /// <returns></returns>
    public async Task<List<PortalItem>> GetWebMapItemsByGroupAsync(PortalGroup? group)
    {
      if(Portal != null && group != null)
      {
        var portalQueryParams = new PortalQueryParameters($"type:\"web map\" group:{group.GroupId}")
        {
          SortField = "Title",
          SortOrder = PortalQuerySortOrder.Ascending,
        };
        var results = await Portal.FindItemsAsync(portalQueryParams);
        return results.Results.ToList();
      }
      return [];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="group"></param>
    /// <param name="itemTitle"></param>
    /// <returns></returns>
    public async Task<PortalItem?> GetWebMapItemByGroupAndTitleAsync(PortalGroup? group, string? itemTitle)
    {
      if(group != null && itemTitle != null)
      {
        var webMapItems = await GetWebMapItemsByGroupAsync(group);
        return webMapItems?.Find(i => i.Title == itemTitle);
      }
      return null;
    }
  }
}
