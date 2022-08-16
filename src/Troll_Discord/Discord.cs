using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordRPC;

public class Discord
{
    public static string gameName; // What is going to be shown

    public static string ImagePath; // Image to be shown

    public static string text2; // STATUS

    public static string devToken;

    public static DiscordRpcClient rpc;


    public Discord(string[] presenceData)
    {
        gameName = presenceData[0];
        ImagePath = presenceData[1];
        text2 = presenceData[2];
        devToken = presenceData[3];
        Console.WriteLine("Everything set up");
        Start();
    }

    public void Start()
    {
        rpc = new DiscordRpcClient(devToken);
        rpc.Initialize();
        CreateVirtualThread();
        Process.GetCurrentProcess().Exited += new EventHandler(OnExit);
    }

    public void OnExit(object sender, EventArgs e)
    {
        rpc.Deinitialize();
    }

    public void CreateVirtualThread()
    {
        List<Button> btns = new List<Button>();

        if (JsonReader.ReadJsonDynamic("config/gameConfig.json")["Buttons"]["Button1"]["active"] == "1")
        {
            btns.Add(new Button()
            {
                Label = JsonReader.ReadJsonDynamic("config/gameConfig.json")["Buttons"]["Button1"]["label"],
                Url = JsonReader.ReadJsonDynamic("config/gameConfig.json")["Buttons"]["Button1"]["url"],
            });
        }
        if (JsonReader.ReadJsonDynamic("config/gameConfig.json")["Buttons"]["Button2"]["active"] == "1")
        {
            btns.Add(new Button()
            {
                Label = JsonReader.ReadJsonDynamic("config/gameConfig.json")["Buttons"]["Button2"]["label"],
                Url = JsonReader.ReadJsonDynamic("config/gameConfig.json")["Buttons"]["Button2"]["url"],
            });
        }

        Timestamps stamps = new Timestamps
        {
            Start = DateTime.Now,
        };

        Assets assets = new Assets
        {
            LargeImageKey = JsonReader.ReadJsonDynamic("config/gameConfig.json")["image"],
            LargeImageText = JsonReader.ReadJsonDynamic("config/gameConfig.json")["name"],
            SmallImageText = JsonReader.ReadJsonDynamic("config/gameConfig.json")["smallName"],
            SmallImageKey = JsonReader.ReadJsonDynamic("config/gameConfig.json")["smallText"]
        };


        RichPresence pres = new RichPresence()
        {
            Assets = assets,
            Buttons = btns.ToArray(),
            State = text2,
            Timestamps = stamps,
        };

        rpc.SetPresence(pres);

        rpc.UpdateStartTime();

        while (true)
        {

        }
    }
}