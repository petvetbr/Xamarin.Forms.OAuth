﻿using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Xamarin.Forms.OAuth.Providers
{
    public sealed class FoursquareOAuthProvider : OAuthProvider
    {
        // Does not provide token refreshing
        private const string _defaultVersion = "20140806";

        internal FoursquareOAuthProvider(string clientId, string clientSecret, string redirectUrl, string version, params string[] scopes)
            : base(new OAuthProviderDefinition(
                "Foursquare",
                "https://foursquare.com/oauth2/authenticate",
                "https://foursquare.com/oauth2/access_token",
                "https://api.foursquare.com/v2/users/self",
                clientId,
                clientSecret,
                redirectUrl,
                scopes
                )
            {
                IncludeRedirectUrlInTokenRequest = true,
                TokenRequestUrlParameter = "oauth_token",
                ResourceQueryParameters = new[] { new KeyValuePair<string, string>("v", version) }
            })
        { }

        internal FoursquareOAuthProvider(string clientId, string clientSecret, string redirectUrl, params string[] scopes)
            : this(clientId, clientSecret, redirectUrl, _defaultVersion, scopes)
        { }

        internal override AccountData ReadAccountData(string json)
        {
            var jObject = JObject.Parse(json);

            var response = jObject["response"] as JObject;
            var user = response["user"] as JObject;

            return new AccountData(user.GetStringValue("id"),
                $"{user.GetStringValue("firstName")} {user.GetStringValue("lastName")}");
        }
    }
}
