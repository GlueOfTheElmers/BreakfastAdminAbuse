using System;
using System.IO;
using System.Net;
using Rocket.Unturned.Player;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Events;
using SDG.Unturned;
using UnityEngine;
using static Rocket.Unturned.Events.UnturnedPlayerEvents;
namespace DefCon42
{
    public class Library
    {
        public static string HTMLRequest(string url)
        {
            ServicePointManager
           .ServerCertificateValidationCallback +=
           (sender, cert, chain, sslPolicyErrors) => true;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;
            request.Accept = "text/html";

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
        public static string SteamHTMLRequest(string input)
        {
            string url = "http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key=" + Init.Instance.Configuration.Instance.steamapikey + "&steamids=" + input;
            string html = Library.HTMLRequest(url);
            string data = Library.getBetween(html, "\"personaname\":", ",");
            data = data.Replace("\"", "");
            return data;
        }
        public static void UnturnedHTMLRequest(UnturnedPlayer player, string url, string desc)
        {
            player.Player.channel.send("askBrowserRequest", player.CSteamID, SDG.Unturned.ESteamPacket.UPDATE_RELIABLE_BUFFER, desc, url);
        }

        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }
    }
}
