using System;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Models;
using System.Timers;
using System.Collections.Generic;
using TwitchLib.Communication.Events;

namespace MillBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Bot bot = new Bot();
            Console.ReadLine();
        }
    }

    class Bot
    {
        TwitchClient client;
        Timer reconnectTimer;
        Timer timer;
   
        IDictionary<string, int> answers;
        ISet<string> users;

        bool checkForVotes;

        public Bot()
        {
            ConnectionCredentials credentials = new ConnectionCredentials(Environment.GetEnvironmentVariable("TWITCH_MILLIONAIRE_NAME"), Environment.GetEnvironmentVariable("TWITCH_MILLIONAIRE_OAUTH"));

            checkForVotes = false;

            client = new TwitchClient();
            client.Initialize(credentials, Environment.GetEnvironmentVariable("TWITCH_MILLIONAIRE_CHANNEL"));

            client.OnLog += Client_OnLog;
            client.OnJoinedChannel += Client_OnJoinedChannel;
            client.OnMessageReceived += Client_OnMessageReceived;
            client.OnConnected += Client_OnConnected;

            client.Connect();

            reconnectTimer = new Timer(300000);
            reconnectTimer.Elapsed += OnReconnectEvent;
            reconnectTimer.Enabled = true;
            reconnectTimer.AutoReset = true;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            // client.SendMessage(client.JoinedChannels[0], "/emoteonly");
            client.SendMessage(client.JoinedChannels[0], $"Results = a : {answers["a"]}, b : {answers["b"]}, c : {answers["c"]}, d : {answers["d"]}");
            checkForVotes = false;
        }

        private void OnReconnectEvent(Object source, ElapsedEventArgs e)
        {
            client.Disconnect();
            client.Connect();
        }

        private void Client_OnLog(object sender, OnLogArgs e)
        {
            Console.WriteLine($"{e.DateTime.ToString()}: {e.BotUsername} - {e.Data}");
        }

        private void Client_OnConnected(object sender, OnConnectedArgs e)
        {
            Console.WriteLine($"Connected to {e.AutoJoinChannel}");
        }

        private void Client_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            // client.SendMessage(client.JoinedChannels[0], "I am here 4Head");
        }

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            if (checkForVotes)
            {
                if (e.ChatMessage.Message.StartsWith("!vote"))
                {
                    if (users.Contains(e.ChatMessage.Username))
                    {
                        client.SendMessage(e.ChatMessage.Channel, $"@{e.ChatMessage.Username}: you have voted already");
                        return;
                    }
                    {
                        string answer = e.ChatMessage.Message.Substring(5).Trim().ToLower();
                        if (!answers.Keys.Contains(answer))
                        {
                            client.SendMessage(e.ChatMessage.Channel, $"@{e.ChatMessage.Username}: wrong vote: put a,b,c or d");
                            return;
                        }
                        answers[answer]++;
                    }
                    users.Add(e.ChatMessage.Username);
                }
            }
            else
            {
                if ((e.ChatMessage.IsModerator || e.ChatMessage.IsBroadcaster) && e.ChatMessage.Message.StartsWith("!millstart"))
                {
                    answers = new Dictionary<string, int>
                    {
                        ["a"] = 0,
                        ["b"] = 0,
                        ["c"] = 0,
                        ["d"] = 0
                    };
                    users = new HashSet<string>();

                    timer = new Timer(30000);
                    timer.Elapsed += OnTimedEvent;
                    timer.Enabled = true;
                    timer.AutoReset = false;
                    checkForVotes = true;

                    client.SendMessage(e.ChatMessage.Channel, "Vote for answer, '!vote a' '!vote b' '!vote c' or '!vote d'");
                    // client.SendMessage(e.ChatMessage.Channel, "/emoteonlyoff");
                }
            }
        }
    }

}
