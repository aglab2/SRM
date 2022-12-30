using System;
using System.Collections.Generic;
using System.IO;
using TwitchLib.Api;
using TwitchLib.Api.Helix.Models.Channels.ModifyChannelInformation;

namespace TitleUpdaterConsole
{
    class Program
    {
        private static TwitchAPI api;

        static string Clean(string data)
        {
            return data.Replace("\\n", "");
        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
                return;

            api = new TwitchAPI();
            api.Settings.ClientId = Environment.GetEnvironmentVariable("TWITCH_CLIENT_ID");
            api.Settings.AccessToken = Environment.GetEnvironmentVariable("TWITCH_ACCESS_TOKEN");

            var filePath = args[0];
            var lines = File.ReadAllLines(filePath);
            Dictionary<string, string> info = new Dictionary<string, string>();
            foreach (var line in lines)
            {
                try
                {
                    string[] parts = line.Split(new char[] { '=' });
                    info[parts[0].Trim()] = parts[1].Trim();
                }
                catch (Exception)
                { }
            }
            if (info.ContainsKey("noupdate"))
            {
                return;
            }

            var title = $"{Environment.GetEnvironmentVariable("TWITCH_MARATHON_NAME")} - ";
            if (info.TryGetValue("hackname", out string hackname))
            {
                title += Clean(hackname);
            }
            title += " ";
            if (info.TryGetValue("category", out string category))
            {
                title += Clean(category);
            }
            title += " by ";
            if (info.TryGetValue("runner1", out string runner1))
            {
                title += Clean(runner1);
            }
            if (info.TryGetValue("runner2", out string runner2))
            {
                title += ", " + Clean(runner2);
            }
            if (info.TryGetValue("runner3", out string runner3))
            {
                title += ", " + Clean(runner3);
            }
            if (info.TryGetValue("runner4", out string runner4))
            {
                title += ", " + Clean(runner4);
            }

            ModifyChannelInformationRequest req = new ModifyChannelInformationRequest
            {
                Title = title,
            };
            var task = api.Helix.Channels.ModifyChannelInformationAsync(Environment.GetEnvironmentVariable("TWITCH_CHANNEL_ID"), req);
            task.Wait();
        }
    }
}
