using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Linq;
using System.Collections.Generic;
using System;
using UnityEngine;
using Steamworks;

namespace DefCon42
{
    public class AdminCommandReporter
    {
        public static Color MessageColor;
        string vehiclename = null;
        ushort vehicleid = 0;
        ushort AssetID = 0;
        int AssetCount = 0;

        internal void UnturnedPlayerEvents_OnPlayerChatted(UnturnedPlayer player, ref UnityEngine.Color color, string message, EChatMode chatMode, ref bool cancel)
        {
            if (player.IsAdmin)
            {
                if (message.StartsWith("/god"))
                {
                    if(player.GodMode.Equals(false))
                    {
                        UnturnedChat.Say(player.CharacterName + " enabled godmode!");
                    }
                    else
                    {
                        UnturnedChat.Say(player.CharacterName + " disabled godmode!");
                    }
                }
                if (message.StartsWith("/airdrop"))
                {
                    UnturnedChat.Say(player.DisplayName + " spawned in an airdrop!");
                }
                if (message.StartsWith("/massdrop") || message.StartsWith("mdrop") || message.StartsWith("massairdrop") || message.StartsWith("airdropmass") || message.StartsWith("dropmass") || message.StartsWith("mairdrop"))
                {
                    UnturnedChat.Say(player.DisplayName + " spawned in a mass airdrop!");
                }
                else if (message.StartsWith("/tp "))
                {
                    UnturnedPlayer target = UnturnedPlayer.FromName(message.Substring(3).Replace(" ", ""));
                    if (target != null)
                    {
                        UnturnedChat.Say(player.CharacterName + " tp'd to " + target.CharacterName + "!");
                    }
                    else
                    {
                        foreach(Node n in LevelNodes.nodes)
                        {
                            if(n.type == ENodeType.LOCATION)
                            {
                                if(((LocationNode)n).name.ToLower().Contains(message.Substring(4).Replace(" ", "").Replace("\"", "")))
                                {
                                    UnturnedChat.Say(player.CharacterName + " tp'd to " + ((LocationNode)n).name + "!");
                                }
                            }
                        }
                    }
                }
                else if (message.StartsWith("/teleport "))
                {
                    UnturnedPlayer target = UnturnedPlayer.FromName(message.Substring(9).Replace(" ", ""));
                    if (target != null && (player != target))
                    {
                        UnturnedChat.Say(player.CharacterName + " tp'd to " + target.CharacterName + "!");
                    }
                    else if (player == target)
                    {
                        string[] splitstring = message.Substring(2).Split(' ');

                        if (splitstring.Length > 2)
                        {
                            foreach (Node n in LevelNodes.nodes)
                            {
                                if (n.type == ENodeType.LOCATION)
                                {
                                    if (((LocationNode)n).name.ToLower().Contains(splitstring[2].Replace(" ", "").Replace("\"", "")))
                                    {
                                        UnturnedChat.Say(player.CharacterName + " tp'd to " + ((LocationNode)n).name + "!");
                                    }
                                }
                            }
                        }
                        else
                        {
                            UnturnedChat.Say(player.CharacterName + "tried to TP to himself..");
                        }
                    }
                    else
                    {
                        foreach (Node n in LevelNodes.nodes)
                        {
                            if (n.type == ENodeType.LOCATION)
                            {
                                if (((LocationNode)n).name.ToLower().Contains(message.Substring(9).Replace(" ", "").Replace("\"", "")))
                                {
                                    UnturnedChat.Say(player.CharacterName + " tp'd to " + ((LocationNode)n).name + "!");
                                }
                            }
                        }
                    }
                }
                else if (message.StartsWith("/vanish"))
                {
                    if (player.VanishMode.Equals(false))
                    {
                        UnturnedChat.Say(player.CharacterName + " enabled vanish!");
                    }
                    else
                        UnturnedChat.Say(player.CharacterName + " disabled vanish!");
                }
                else if (message.StartsWith("/heal"))
                {
                    UnturnedChat.Say(player.CharacterName + " used /heal!");
                }
                else if (message.StartsWith("/i "))
                {
                    string[] splitstring = message.Substring(2).Split(' ');

                    ushort.TryParse(splitstring[1], out AssetID);
                    if(splitstring.Length > 2)
                    {
                        int.TryParse(splitstring[2].Replace("\"", ""), out AssetCount);
                    }

                    Asset iAssetID = Assets.find(EAssetType.ITEM, AssetID);
                    ItemAsset iAssetName = (Assets.find(EAssetType.ITEM).Cast<ItemAsset>().Where(i => i.itemName != null).OrderBy(i => i.itemName.Length).Where(i => i.itemName.ToLower().Contains(splitstring[1].Replace("\"", "").ToLower())).FirstOrDefault());

                    if (AssetCount == 0)
                    {
                        if (iAssetID != null)
                        {
                            UnturnedChat.Say(player.DisplayName + " spawned in " + ((ItemAsset)iAssetID).itemName + "!");
                        }
                        else if (iAssetName != null && !(iAssetName.itemName.Contains("0") || iAssetName.itemName.Contains("1")) && message.Substring(2).Length > 0)
                        {
                            UnturnedChat.Say(player.DisplayName + " spawned in " + iAssetName.itemName + "!");
                        }
                    }
                    else
                    {
                        if (iAssetID != null)
                        {
                            UnturnedChat.Say(player.DisplayName + " spawned in " + AssetCount.ToString() + "x " + ((ItemAsset)iAssetID).itemName + "!");
                        }
                        else if (iAssetName != null && !(iAssetName.itemName.Contains("0") || iAssetName.itemName.Contains("1")) && message.Substring(2).Length > 0)
                        {
                            UnturnedChat.Say(player.DisplayName + " spawned in " + AssetCount.ToString() + "x " + iAssetName.itemName + "!");
                        }
                        else
                        {
                            UnturnedChat.Say(player.DisplayName + " tried to spawn something in!");
                        }
                    }

                    iAssetID = null;
                    iAssetName = null;
                    AssetCount = 0; 
                }
                else if (message.StartsWith("/v "))
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
                        UnturnedChat.Say(player.CharacterName + " used /v to spawn in a " + vehiclename + "!");
                        cancel = true;
                    }
                    else
                    {
                        UnturnedChat.Say(player.CharacterName + " used /v to try and spawn in a vehicle!");
                    }
                }
                else if (message.StartsWith("/spy "))
                {
                    string data = Library.SteamHTMLRequest(message.Substring(4).Replace(" ", ""));

                    if (data != "")
                    {
                        UnturnedChat.Say(player.CharacterName + " used /spy on " + data + "!");
                    }
                    else
                    {
                        UnturnedChat.Say(player.CharacterName + " tried to use /spy on somebody!");
                    }
                }
                else if (message.StartsWith("/kick "))
                {
                    UnturnedPlayer kicktarget = UnturnedPlayer.FromName(message.Substring(4).Replace(" ", ""));
                    string data = Library.SteamHTMLRequest(message.Substring(4).Replace(" ", ""));

                    if (kicktarget != null)
                    {
                        UnturnedChat.Say(player.CharacterName + " kicked " + kicktarget.CharacterName + "!");
                    }
                    else if (data != "")
                    {
                        UnturnedChat.Say(player.CharacterName + " kicked " + data + "!");
                    }
                }
                else if (message.StartsWith("/slay "))
                {
                    UnturnedPlayer slaytarget = UnturnedPlayer.FromName(message.Substring(4).Replace(" ", ""));
                    string data = Library.SteamHTMLRequest(message.Substring(4).Replace(" ", ""));

                    if (slaytarget != null)
                    {
                        UnturnedChat.Say(player.CharacterName + " banned " + slaytarget + "!");
                    }
                    else if (data != "")
                    {
                        UnturnedChat.Say(player.CharacterName + " banned " + data + "!");
                    }
                }
                else if (message.StartsWith("/admin "))
                {
                    UnturnedPlayer admintarget = UnturnedPlayer.FromName(message.Substring(6).Replace(" ", ""));

                    if (admintarget != null)
                    {
                        UnturnedChat.Say(player.CharacterName + " tried to admin " + admintarget + "!");
                        cancel = true;
                    }
                }
            }
        } 
    } 
}
