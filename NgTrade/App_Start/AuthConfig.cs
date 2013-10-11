using Microsoft.Web.WebPages.OAuth;

namespace NgTrade.App_Start
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            OAuthWebSecurity.RegisterTwitterClient(consumerKey: "ne8urbLHMtfQsUOgv9DTw", consumerSecret: "sRTxeNdIIdjYy5jYDlP89HEB65EEyCaFDK3Tuiz9e0");

            OAuthWebSecurity.RegisterFacebookClient(appId: "185502631637619", appSecret: "abe912fa4061330ce88103bf9f8f9210");

            OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}
