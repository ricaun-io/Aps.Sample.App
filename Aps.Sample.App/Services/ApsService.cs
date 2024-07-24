using Autodesk.Authentication;
using Autodesk.Authentication.Model;
using Autodesk.SDKManager;
using System.Diagnostics;
using System.Text;

namespace Aps.Sample.App.Services
{
    public class ApsService
    {
        #region Fields

        AuthenticationClient authenticationClient = null!;
        string client_id = "LtSI0DgPFsVmBLndZSsG8a2pb1unHNJu";

        string _callbackUri = "https://aps-single-page.glitch.me/";

        List<Scopes> scopes = new List<Scopes>() { Scopes.UserProfileRead };
        string codeVerifier = null;
        ThreeLeggedToken ThreeLeggedToken = null;
        #endregion

        #region Properties

        public string CallbackUri { get => _callbackUri; }

        #endregion

        #region Constructors

        public ApsService()
        {
            SDKManager sdkManager = SdkManagerBuilder
              .Create()
              .Build();

            authenticationClient = new AuthenticationClient(sdkManager);
            ThreeLeggedToken = ThreeLeggedToken.Load();
        }

        #endregion

        #region Methods

        public string Authorize()
        {
            var codeChallenge = CreateCodeChallenge();
            var codeChallengeMethod = "S256";

            return authenticationClient.Authorize(client_id, ResponseType.Code, _callbackUri, scopes,
#if !DEBUG
                prompt:"login",
#endif
                codeChallenge: codeChallenge,
                codeChallengeMethod: codeChallengeMethod);
        }

        private string CreateCodeChallenge(string codeVerifier = null)
        {
            if (codeVerifier is null)
            {
                codeVerifier = Guid.NewGuid().ToString() + Guid.NewGuid().ToString() + Guid.NewGuid().ToString();
            }
            this.codeVerifier = codeVerifier;

            var sha256 = System.Security.Cryptography.SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(codeVerifier);
            var hash = sha256.ComputeHash(bytes);
            return Base64UrlEncode(hash);
        }

        private string Base64UrlEncode(byte[] hash)
        {
            var base64 = Convert.ToBase64String(hash);
            var base64Url = base64.Replace('+', '-').Replace('/', '_').TrimEnd('=');
            return base64Url;
        }

        public async Task GetPKCEThreeLeggedTokenAsync(string code)
        {
            ThreeLeggedToken = await authenticationClient.GetThreeLeggedTokenAsync(
                client_id,
                code,
                _callbackUri,
                clientSecret: null,
                codeVerifier: codeVerifier);

            ThreeLeggedToken.Save();
        }

        public async Task RefreshToken()
        {
            if (ThreeLeggedToken is null) return;

            try
            {
                ThreeLeggedToken = await authenticationClient.RefreshTokenAsync(
                clientId: client_id,
                clientSecret: null,
                refreshToken: ThreeLeggedToken.RefreshToken);

                ThreeLeggedToken.Save();
            }
            catch
            {
                ThreeLeggedToken = null;
                throw;
            }
        }

        public async Task EnsureTokenIsValid()
        {
            if (ThreeLeggedToken is null)
            {
                throw new Exception("Not logged in.");
            }

            var token = await authenticationClient.IntrospectTokenAsync(ThreeLeggedToken.AccessToken, client_id, clientSecret: null);
            //Debug.WriteLine($"Token: {token.Active} {token.Exp}");
            if (token.Active == false)
            {
                await RefreshToken();
            }
        }

        public async Task<UserInfo> GetUserInfoAsync()
        {
            await EnsureTokenIsValid();
            return await authenticationClient.GetUserInfoAsync(ThreeLeggedToken.AccessToken);
        }

        public bool IsLoggedIn()
        {
            return ThreeLeggedToken is not null;
        }

        public async Task Logout()
        {
            if (ThreeLeggedToken is null) return;

            var token = ThreeLeggedToken.AccessToken;
            var refreshToken = ThreeLeggedToken.RefreshToken;
            ThreeLeggedToken = null;
            ThreeLeggedToken.Save();

            await authenticationClient.RevokeAsync(token, client_id, clientSecret: null, TokenTypeHint.AccessToken);
            await authenticationClient.RevokeAsync(token, client_id, clientSecret: null, TokenTypeHint.RefreshToken);
        }

        #endregion

    }
}