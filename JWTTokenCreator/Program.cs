using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace JWTTokenCreator
{
    /// <summary>
    /// Simple JWT token creator for debugging and testing purposes.
    /// Can be invoked from the commandline as follows:
    /// JWTTokenCreator.exe "username" "sharedkey"
    /// If invoked without arguments then the username and sharedkey can be entered on the commandline manually.
    /// The token that is created is copied to the clipboard.
    /// </summary>
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            bool automaticInput = false;
            string username;
            string sharedkey;

            Console.Clear();
            Console.WriteLine("JWT Token Creator:");

            if (args != null && args.Length > 0)
            {
                username = args[0];
                sharedkey = args[1];
                automaticInput = true;
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Please enter username");
                username = Console.ReadLine();
                Console.WriteLine("Please enter shared key");
                sharedkey = Console.ReadLine();
            }
            
            string jwtToken = CreateAuthenticationToken(username, sharedkey);

            Clipboard.SetText(jwtToken);
            
            if (!automaticInput)
            {
                Console.WriteLine("JWT token has been copied to the Windows clipboard");
                Console.WriteLine();
                Console.WriteLine("JWT token: {0}", jwtToken);

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
    }
}
