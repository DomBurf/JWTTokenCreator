using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace JWTTokenCreator
{
    /// <summary>
    /// Simple JWT token creator for testing and debugging JWT tokens. Can encode and decode JWT tokens.
    /// Can be invoked from the commandline as follows:
    /// 
    /// To encode a token:
    /// JWTTokenCreator.exe 1 "username" "sharedkey"
    /// If invoked without arguments then the username and sharedkey can be entered on the commandline manually.
    /// The token that is created is copied to the clipboard.
    /// 
    /// To decode a token:
    /// JWTTokenCreator.exe 2 "jsontoken" "sharedkey"
    /// If invoked without arguments then the JSON token and sharedkey can be entered on the commandline manually.
    /// The token that is decoded is copied to the clipboard.
    /// </summary>
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            string username;
            string sharedkey;
            string jsonWebToken;
            int option;

            Console.Clear();
            Console.WriteLine("JWT Token Creator:");

            if (args != null && args.Length > 0)
            {
                option = Convert.ToInt32(args[0]);
                if (option == 1)
                {
                    username = args[1];
                    sharedkey = args[2];

                    string jwtToken = CreateAuthenticationToken(username, sharedkey);
                    Clipboard.SetText(jwtToken);
                }
                else if (option == 2)
                {
                    jsonWebToken = args[1];
                    sharedkey = args[2];
                    string jwtToken = DecodeAuthenticationToken(sharedkey, jsonWebToken);
                    Clipboard.SetText(jwtToken);
                }
            }
            else
            {
                Console.WriteLine("Please select one of the following options:");
                Console.WriteLine();
                Console.WriteLine("1) Encode a JWT token");
                Console.WriteLine("2) Decode a JWT token");

                var info = Console.ReadKey();
                string answer = info.KeyChar + Console.ReadLine();
                Int32.TryParse(answer, out option);

                if (option == 1)
                {
                    Console.WriteLine();
                    Console.WriteLine("Please enter username");
                    username = Console.ReadLine();
                    Console.WriteLine("Please enter shared key");
                    sharedkey = Console.ReadLine();

                    string jwtToken = CreateAuthenticationToken(username, sharedkey);
                    Clipboard.SetText(jwtToken);

                    Console.WriteLine("JWT token has been copied to the Windows clipboard");
                    Console.WriteLine();
                    Console.WriteLine("JWT token: {0}", jwtToken);
                }
                else if (option == 2)
                {
                    Console.WriteLine();
                    Console.WriteLine("Please enter jsontoken");
                    jsonWebToken = Console.ReadLine();
                    Console.WriteLine("Please enter shared key");
                    sharedkey = Console.ReadLine();

                    string jwtToken = DecodeAuthenticationToken(sharedkey, jsonWebToken);
                    Clipboard.SetText(jwtToken);
                }
                Console.WriteLine();
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }

        private static string CreateAuthenticationToken(string username, string sharedkey)
        {
            TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
            int timestamp = (int)t.TotalSeconds;

            var payload = new Dictionary<string, object>
            { 
                { "iat", timestamp },
                { "jti", Guid.NewGuid().ToString() },
                { "name", username }, 
                { "sharedkey", sharedkey }
            };

            string token = JWT.JsonWebToken.Encode(payload, sharedkey, JWT.JwtHashAlgorithm.HS256);
            return token;
        }

        private static string DecodeAuthenticationToken(string sharedkey, string jsonWebToken)
        {
            string decodedJwt = JWT.JsonWebToken.Decode(jsonWebToken, sharedkey);
            //extract the constituent components of the token
            dynamic root = JObject.Parse(decodedJwt);
            //string name = root.name;
            //int jwtcreated = (int)root.iat;
            //Guid jwtId = (Guid)root.jti;
            return root.ToString();
        }
    }
}
