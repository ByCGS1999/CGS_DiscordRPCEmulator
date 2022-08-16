using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using DiscordRPC;
using Newtonsoft.Json;
using System.Net;
using System.IO.Compression;
using System.IO;
using System.Diagnostics;

/// <summary>
/// THIS CLASS MAKES ALL THE WORK.
/// IT CONTROLS YOUR DISCORD RICH PRESENCE AND OVERWRITES IT TO MAKE EVERYONE THINK YOU'RE PLAYING A GAME OR OTHER GAME THAT YOU AREN'T REALLY.
/// </summary>


public class MainClass
{
    public static string SoftwareVersion = "1.0.0";

    public static string UpdateURL = "https://raw.githubusercontent.com/ByCGS1999/CGS_DiscordRPCEmulator/main/VersionControl";

    public static string RedirUrl = "https://raw.githubusercontent.com/ByCGS1999/CGS_DiscordRPCEmulator/main/DownloadURL";

    private static bool gotResponse = false;


    public static void Main()
    {
        Console.Title = "CGS Discord Rich Presence Emulator";
        Console.WriteLine("Discord rich presence emulator loading...");

        CreateMainThread();

        /* DISABLED SINCE IM NOT GOING TO PUSH NEW UPDATES
        Console.WriteLine("Searching for updates...");
        WebClient cli = new WebClient();

        SoftwareVersion = JsonReader.ReadJsonConfig("config/application.json", "version");

        if(cli.DownloadString(UpdateURL) == SoftwareVersion)
        {
            Console.WriteLine("Software is up to date!");
            Console.WriteLine("Creating main thread");

            CreateMainThread();
        }
        else
        {
            Console.WriteLine("A new version has been found!");
            Console.WriteLine("Would you like to download it?\nY. Yes               N. No");

            while (!gotResponse)
            {
                ConsoleKeyInfo info = Console.ReadKey();

                switch (info.Key)
                {
                    case ConsoleKey.Y:
                        DownloadNewVersion(cli);
                        gotResponse = true;
                        break;

                    case ConsoleKey.N:
                        Console.WriteLine("\nSoftware is outdated!");
                        Console.WriteLine("Creating main thread");
                        CreateMainThread();
                        gotResponse = true;
                        break;
                    default:
                        Console.WriteLine(info.Key.ToString() + " Is not a valid option. Press Y or N");
                        gotResponse = false;
                        break;
                }
            }
        }
        */
    }

    public static void DownloadNewVersion(WebClient cli)
    {
        Directory.CreateDirectory("tmp");

        string downloadURI = cli.DownloadString(RedirUrl);

        Console.WriteLine("Download To: " + downloadURI);

        cli.DownloadFile(downloadURI, "update.zip");

        File.Move("update.zip", "tmp/update.zip",true);

        OnDownloadFinished();
    }


    public static void OnDownloadFinished()
    {
        try
        {
            ZipFile.ExtractToDirectory("tmp/update.zip", "tmp/update/",true);

            //MOVE THEM OVERWRITTING
            string[] files = Directory.GetFiles("tmp/update/");

            foreach (string fname in files)
            {
                Console.WriteLine(fname);
                FileInfo mFile = new FileInfo(fname);
                // to remove name collisions
                if (new FileInfo(Directory.GetCurrentDirectory() + "/" + mFile.Name).Exists == false)
                {
                    mFile.MoveTo(Directory.GetCurrentDirectory() + "/" + mFile.Name);
                }
                else
                {
                    mFile.MoveTo(Directory.GetCurrentDirectory() + "/" + mFile.Name,true);
                }

                Console.WriteLine("Application successfully updated! Killing process in 5 seconds, Open it manually");
                Thread.Sleep(5000);
                Console.WriteLine("Killing process.");
                Process.GetCurrentProcess().Kill();
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine("An error has occurred while updating to the new version. Error Details: " + ex.Message.ToString());
        }
    }


    public static void CreateMainThread()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Green;


        Console.WriteLine("Initializing.");
        Console.WriteLine("Ready to run, Reading config.json");

        List<string> args = new List<string>();


        args.Add(JsonReader.ReadJsonConfig("config/gameConfig.json", "name"));
        args.Add(JsonReader.ReadJsonConfig("config/gameConfig.json", "image"));
        args.Add(JsonReader.ReadJsonConfig("config/gameConfig.json", "status"));
        args.Add(JsonReader.ReadJsonConfig("config/gameConfig.json", "applicationId"));

        new Discord(args.ToArray());
    }

}

