using System;
using System.IO;
using System.IO.Compression;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security.OAuth;

namespace ElvenCurse.Server.Infrastructure
{
    public class CustomCookieAuthenticationProvider: OAuthBearerAuthenticationProvider
    {
        public override Task RequestToken(OAuthRequestTokenContext context)
        {
            var cookie = context.Request.Cookies["ElvenCurseAuthcookie"];
            if (string.IsNullOrEmpty(cookie))
            {
                return base.RequestToken(context);
            }

            context.OwinContext.Request.User = GetPrincipaluserFromCookie(cookie);
            return base.RequestToken(context);
        }

        private ClaimsPrincipal GetPrincipaluserFromCookie(string cookievalue)
        {
            cookievalue = cookievalue.Replace('-', '+').Replace('_', '/');

            var padding = 3 - ((cookievalue.Length + 3) % 4);
            if (padding != 0)
                cookievalue = cookievalue + new string('=', padding);

            var bytes = Convert.FromBase64String(cookievalue);

            bytes = System.Web.Security.MachineKey.Unprotect(bytes,
                "Microsoft.Owin.Security.Cookies.CookieAuthenticationMiddleware",
                    "ApplicationCookie", "v1");

            if (bytes == null)
            {
                return null;
            }

            using (var memory = new MemoryStream(bytes))
            {
                using (var compression = new GZipStream(memory,
                                                    CompressionMode.Decompress))
                {
                    using (var reader = new BinaryReader(compression))
                    {
                        reader.ReadInt32();
                        string authenticationType = reader.ReadString();
                        reader.ReadString();
                        reader.ReadString();

                        int count = reader.ReadInt32();

                        var claims = new Claim[count];
                        for (int index = 0; index != count; ++index)
                        {
                            string type = reader.ReadString();
                            type = type == "\0" ? ClaimTypes.Name : type;

                            string value = reader.ReadString();

                            string valueType = reader.ReadString();
                            valueType = valueType == "\0" ?
                                           "http://www.w3.org/2001/XMLSchema#string" :
                                             valueType;

                            string issuer = reader.ReadString();
                            issuer = issuer == "\0" ? "LOCAL AUTHORITY" : issuer;

                            string originalIssuer = reader.ReadString();
                            originalIssuer = originalIssuer == "\0" ?
                                                         issuer : originalIssuer;

                            claims[index] = new Claim(type, value,
                                                   valueType, issuer, originalIssuer);
                        }

                        var identity = new ClaimsIdentity(claims, authenticationType,
                                                      ClaimTypes.Name, ClaimTypes.Role);
                        //return identity;
                        var principal = new ClaimsPrincipal(identity);

                        //return principal.Identity.IsAuthenticated;
                        return principal;
                    }
                }
            }
        }
    }
}
