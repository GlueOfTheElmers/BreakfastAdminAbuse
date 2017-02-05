using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Linq;
using System.Collections.Generic;
using System;
using UnityEngine;
using Steamworks;
using System.IO;
using Rocket.Core;
using Rocket.API;
using Rocket.API.Serialisation;

namespace DefCon42
{
    public class DirectoryHandler
    {
        public string directory = System.IO.Directory.GetCurrentDirectory() + "/..";
        public void Initialize()
        {
            if (File.Exists(directory + "/AdminAbusers"))
            {
                Rocket.Core.Logging.Logger.Log(directory + "/AdminAbusers.txt Already exists, loopholing...");
            }
            else
            {
                File.CreateText(directory + "/AdminAbusers.txt");
            }
        }
    }
    public static class ColorExtensions
    {
        public static Color ParseColor(this string color)
        {
            return (Color)typeof(Color).GetProperty(color.ToLowerInvariant()).GetValue(null, null);
        }
    }
    public class AdminCommandReporter
    {
        string vehiclename = null;
        ushort vehicleid = 0;
        ushort AssetID = 0;
        int AssetCount = 0;

        UnturnedPlayer target = null;
        UnturnedPlayer kicktarget = null;
        UnturnedPlayer slaytarget = null;
        UnturnedPlayer admintarget = null;
        UnturnedPlayer healtarget = null;
        public List<RocketPermissionsGroup> rgroups;

        public void Initialize()
        {
            foreach (string s in Init.Instance.Configuration.Instance.groups)
            {
                rgroups.Add(R.Permissions.GetGroup(s));
            }
        }
        public void Message(string Translation, string arg)
        {
            string[] args = arg.Split(',');
            UnturnedPlayer p = UnturnedPlayer.FromName(args[0]);
            if (!(R.Permissions.HasPermission(p, "abuse.ignore") && Init.Instance.Configuration.Instance.UseIgnorePermission))
            {
                Color mcolor = ColorExtensions.ParseColor(Init.Instance.Configuration.Instance.messagecolor);
                UnturnedChat.Say(Init.Instance.Translations.Instance.Translate(Translation, args), mcolor);
                if (Init.Instance.Configuration.Instance.LogAbuse)
                {
                    DirectoryHandler d = new DirectoryHandler();
                    using (StreamWriter w = File.AppendText(d.directory + "/AdminAbusers.txt"))
                    {
                        w.WriteLine(Init.Instance.Translations.Instance.Translate(Translation, args) + w.NewLine);
                        w.Close();
                    }
                }
            }
        }
        internal void UnturnedPlayerEvents_OnPlayerChatted(UnturnedPlayer player, ref UnityEngine.Color color, string message, EChatMode chatMode, ref bool cancel)
        {
            if (player.IsAdmin || R.Permissions.GetGroups(player, false).Any(x => rgroups.Any(y => y == x)))
            {
                if (Init.Instance.Configuration.Instance.SayGod && message.StartsWith("/god"))
                {
                    if (player.GodMode.Equals(false))
                    {
                        Message("god_message_enabled", player.CharacterName);
                    }
                    else
                    {
                        Message("god_message_disabled", player.CharacterName);
                    }
                }
                if (Init.Instance.Configuration.Instance.SayAirdrop && message.StartsWith("/airdrop"))
                {
                    Message("airdrop_message", player.CharacterName);
                }
                if (message.StartsWith("/massdrop") || message.StartsWith("mdrop") || message.StartsWith("massairdrop") || message.StartsWith("airdropmass") || message.StartsWith("dropmass") || message.StartsWith("mairdrop"))
                {
                    Message("massdrop_message", player.CharacterName);
                }
                else if (Init.Instance.Configuration.Instance.SayTP && message.StartsWith("/tp "))
                {
                    string[] splitstring = message.Split(' ');
                    target = UnturnedPlayer.FromName(splitstring[1]);
                    if (splitstring.Length > 1)
                    {
                        if (target != null)
                        {
                            if (player.CSteamID != target.CSteamID)
                            {
                                Message("tp_message", player.CharacterName + "," + target.CharacterName);
                            }
                            else if (player.CSteamID == target.CSteamID)
                            {
                                Message("tp_message_self", player.CharacterName);
                            }
                        }
                        else
                        {
                            foreach (Node n in LevelNodes.nodes)
                            {
                                if (n.type == ENodeType.LOCATION)
                                {
                                    if (((LocationNode)n).name.ToLower().Contains(splitstring[1].Replace(" ", "").Replace("\"", "")))
                                    {
                                        Message("tp_message", (player.CharacterName + "," + ((LocationNode)n).name));
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                else if (Init.Instance.Configuration.Instance.SayTeleport && message.StartsWith("/teleport"))
                {
                    string[] splitstring = message.Split(' ');
                    target = UnturnedPlayer.FromName(splitstring[1]);
                    if (splitstring[1].Contains('/'))
                    {
                        splitstring[3] = splitstring[2];
                        splitstring[2] = splitstring[1].Split('/')[1];
                        splitstring[1] = splitstring[1].Split('/')[0];
                    }
                    if (target != null)
                    {
                        if (player.CSteamID != target.CSteamID)
                        {
                            Message("teleport_message", player.CharacterName + "," + target.CharacterName);
                        }
                        else if (player.CSteamID == target.CSteamID)
                        {
                            Message("tp_message_self", player.CharacterName);
                        }
                    }
                    else if (splitstring.Length > 2)
                    {
                        target = UnturnedPlayer.FromName(splitstring[1]);
                        UnturnedPlayer _target = UnturnedPlayer.FromName(splitstring[2]);
                        if (target != null && _target != null)
                        {
                            Message("teleport_player_to_player", target.CharacterName + "," + _target.CharacterName + "," + player.CharacterName);
                        }
                        else if (target != null)
                        {
                            foreach (Node n in LevelNodes.nodes)
                            {
                                if (n.type == ENodeType.LOCATION)
                                {
                                    if (((LocationNode)n).name.ToLower().Contains(splitstring[2].Replace(" ", "").Replace("\"", "")))
                                    {
                                        if (player == target)
                                        {
                                            Message("teleport_message", player.CharacterName + "," + ((LocationNode)n).name);
                                            break;
                                        }
                                        else
                                        {
                                            Message("teleport_message_player_to_location", target.CharacterName + "," + ((LocationNode)n).name + "," + player.CharacterName);
                                            break;
                                        }      
                                    }
                                }
                            }
                        }
                    }
                    else if (splitstring.Length > 1)
                    {
                        foreach (Node n in LevelNodes.nodes)
                        {
                            if (n.type == ENodeType.LOCATION)
                            {
                                if (((LocationNode)n).name.ToLower().Contains(splitstring[1].Replace(" ", "").Replace("\"", "")))
                                {
                                    Message("teleport_message", player.CharacterName + "," + ((LocationNode)n).name);
                                }
                            }
                        }
                    }
                    target = null;
                }
                else if (Init.Instance.Configuration.Instance.SayVanish && message.StartsWith("/vanish"))
                {
                    if (player.VanishMode.Equals(false))
                    {
                        Message("vanish_message_enabled", player.CharacterName);
                    }
                    else
                    {
                        Message("vanish_message_disabled", player.CharacterName);
                    }
                }
                else if (Init.Instance.Configuration.Instance.SayI && message.StartsWith("/i "))
                {
                    string[] splitstring = message.Split(' ');
                    ushort.TryParse(splitstring[1], out AssetID);
                    if (splitstring.Length > 2)
                    {
                        int.TryParse(splitstring[2].Replace("\"", ""), out AssetCount);
                    }

                    Asset iAssetID = Assets.find(EAssetType.ITEM, AssetID);
                    ItemAsset iAssetName = (Assets.find(EAssetType.ITEM).Cast<ItemAsset>().Where(i => i.itemName != null).OrderBy(i => i.itemName.Length).Where(i => i.itemName.ToLower().Contains(splitstring[1].Replace("\"", "").ToLower())).FirstOrDefault());

                    if (AssetCount == 0)
                    {
                        if (iAssetID != null)
                        {
                            Message("item_message", player.CharacterName + "," + ((ItemAsset)iAssetID).itemName);
                        }
                        else if (iAssetName != null && !(iAssetName.itemName.Contains("0") || iAssetName.itemName.Contains("1")) && message.Substring(2).Length > 0)
                        {
                            Message("item_message", player.CharacterName + "," + iAssetName.itemName);
                        }
                    }
                    else
                    {
                        if (iAssetID != null)
                        {
                            Message("item_message_amount", player.CharacterName + "," + AssetCount.ToString() + "," + ((ItemAsset)iAssetID).itemName);
                        }
                        else if (iAssetName != null && !(iAssetName.itemName.Contains("0") || iAssetName.itemName.Contains("1")) && message.Substring(2).Length > 0)
                        {
                            Message("item_message_amount", player.CharacterName + "," + AssetCount.ToString() + "," + iAssetName.itemName);
                        }
                    }

                    iAssetID = null;
                    iAssetName = null;
                    AssetCount = 0;
                }
                else if (Init.Instance.Configuration.Instance.SayV && message.StartsWith("/v "))
                {
                    Asset[] vAssets = Assets.find(EAssetType.VEHICLE);
                    foreach (VehicleAsset asset in vAssets)
                    {
                        ushort.TryParse(message.Substring(2).Replace(" ", "").ToLower(), out vehicleid);
                        if (asset.vehicleName.ToLower().Contains(message.Substring(2).Replace(" ", "").ToLower()))
                        {
                            vehiclename = asset.vehicleName;
                            break;
                        }
                        else if (asset.id.Equals(vehicleid))
                        {
                            vehiclename = asset.vehicleName;
                            break;
                        }
                        else { vehiclename = null; }
                    }
                    if (vehiclename != null)
                    {
                        Message("vehicle_message", player.CharacterName + "," + vehiclename);
                    }
                }
                else if (Init.Instance.Configuration.Instance.SaySpy && message.StartsWith("/spy "))
                {
                    string data = Library.SteamHTMLRequest(message.Substring(4).Replace(" ", ""));

                    if (data != "")
                    {
                        Message("spy_message", player.CharacterName + "," + data);
                    }
                }
                else if (Init.Instance.Configuration.Instance.SayKick && message.StartsWith("/kick "))
                {
                    kicktarget = UnturnedPlayer.FromName(message.Substring(4).Replace(" ", ""));
                    string data = Library.SteamHTMLRequest(message.Substring(4).Replace(" ", ""));

                    if (kicktarget != null)
                    {
                        Message("kick_message", player.CharacterName + "," + kicktarget.CharacterName);
                    }
                    else if (data != "")
                    {
                        Message("kick_message", player.CharacterName + "," + data);
                    }
                    kicktarget = null;
                }
                else if (Init.Instance.Configuration.Instance.SaySlay && message.StartsWith("/slay "))
                {
                    slaytarget = UnturnedPlayer.FromName(message.Substring(4).Replace(" ", ""));
                    string data = Library.SteamHTMLRequest(message.Substring(4).Replace(" ", ""));

                    if (slaytarget != null)
                    {
                        Message("slay_message", player.CharacterName + "," + slaytarget.CharacterName);
                    }
                    else if (data != "")
                    {
                        Message("slay_message", player.CharacterName + "," + data);
                    }
                    slaytarget = null;
                }
                else if (Init.Instance.Configuration.Instance.SayHeal && message.StartsWith("/heal"))
                {
                    healtarget = UnturnedPlayer.FromName(message.Substring(5).Replace(" ", ""));

                    if (healtarget != null)
                    {
                        Message("heal_message_player", player.CharacterName + "," + healtarget.CharacterName);
                    }
                    else
                    {
                        Message("heal_message", player.CharacterName);
                    }
                    
                    healtarget = null;
                }
                else if (Init.Instance.Configuration.Instance.SayAdmin && message.StartsWith("/admin "))
                {
                    admintarget = UnturnedPlayer.FromName(message.Substring(6).Replace(" ", ""));

                    if (admintarget != null)
                    {
                        Message("admin_message", player.CharacterName + "," + admintarget.CharacterName);
                    }
                    admintarget = null;
                }
            }        
        }
    }
}
