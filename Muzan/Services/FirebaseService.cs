namespace Muzan.Services
{
    using Google.Apis.Auth.OAuth2;
    using Google.Apis.Util.Store;
    using System;
    using System.Net;
    using System.Threading.Tasks;

    public class FirebaseAuthService
    {
        private static readonly string[] Scopes = { "https://www.googleapis.com/auth/cloud-platform" };
        private static readonly string ApplicationName = "ukiyoeeveryday";

        public async Task<string> GetAccessTokenAsync()
        {

            var credential = GoogleCredential.FromFile("Secrets/serviceAccountKey.json")
                .CreateScoped(Scopes);

            var accessToken = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();
            return accessToken;
        }
    }

}
