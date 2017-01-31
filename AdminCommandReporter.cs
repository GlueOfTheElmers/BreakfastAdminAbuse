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
        UnturnedPlayer target;
        UnturnedPlayer kicktarget;
        UnturnedPlayer slaytarget;
        UnturnedPlayer admintarget;

        internal void UnturnedPlayerEvents_OnPlayerChatted(UnturnedPlayer player, ref UnityEngine.Color color, string message, EChatMode chatMode, ref bool cancel)
        {
            if (player.IsAdmin)
            {
                if (message.StartsWith("/god"))
                {
                    if (player.GodMode.Equals(false))
                    {
                        UnturnedChat.Say(Init.Instance.Translations.Instance.Translate("god_message_enabled", player.CharacterName));
                    }
                    else
                    {
                        UnturnedChat.Say(Init.Instance.Translations.Instance.Translate("god_message_disabled", player.CharacterName));
                    }
                }
                if (message.StartsWith("/airdrop"))
                {
                    UnturnedChat.Say(Init.Instance.Translations.Instance.Translate("airdrop_message", player.CharacterName));
                }
                if (message.StartsWith("/massdrop") || message.StartsWith("mdrop") || message.StartsWith("massairdrop") || message.StartsWith("airdropmass") || message.StartsWith("dropmass") || message.StartsWith("mairdrop"))
                {
                    UnturnedChat.Say(Init.Instance.Translations.Instance.Translate("massrop_message", player.CharacterName));
                }
                else if (message.StartsWith("/tp "))
                {
                    target = null;
                    target = UnturnedPlayer.FromName(message.Substring(2).Replace(" ", ""));
                    if (target != null && (player != target))
                    {
                        UnturnedChat.Say(Init.Instance.Translations.Instance.Translate("tp_message", player.CharacterName, target.CharacterName));
                    }
                    else if (player == target)
                    {
                        string[] splitstring = message.Split(' ');

                        if (splitstring.Length > 2)
                        {
                            foreach (Node n in LevelNodes.nodes)
                            {
                                if (n.type == ENodeType.LOCATION)
                                {
                                    if (((LocationNode)n).name.ToLower().Contains(splitstring[2].Replace(" ", "").Replace("\"", "")))
                                    {
                                        UnturnedChat.Say(Init.Instance.Translations.Instance.Translate("tp_message", player.CharacterName, ((LocationNode)n).name));
                                    }
                                }
                            }
                        }
                        else
                        {
                            UnturnedChat.Say(Init.Instance.Translations.Instance.Translate("tp_message_self", player.CharacterName));
                        }
                    }
                    else if (message.StartsWith("/teleport "))
                    {
                        target = null;
                        target = UnturnedPlayer.FromName(message.Substring(9).Replace(" ", ""));
                        if (target != null && (player != target))
                        {
                            UnturnedChat.Say(Init.Instance.Translations.Instance.Translate("teleport_message", player.CharacterName, target.CharacterName));
                        }
                        else if (player == target)
                        {
                            string[] splitstring = message.Split(' ');

                            if (splitstring.Length > 2)
                            {
                                foreach (Node n in LevelNodes.nodes)
                                {
                                    if (n.type == ENodeType.LOCATION)
                                    {
                                        if (((LocationNode)n).name.ToLower().Contains(splitstring[2].Replace(" ", "").Replace("\"", "")))
                                        {
                                            UnturnedChat.Say(Init.Instance.Translations.Instance.Translate("teleport_message", player.CharacterName, ((LocationNode)n).name));
                                        }
                                    }
                                }
                            }
                            else
                            {
                                UnturnedChat.Say(Init.Instance.Translations.Instance.Translate("teleport_message_self", player.CharacterName));
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
                                        UnturnedChat.Say(Init.Instance.Translations.Instance.Translate("teleport_message", player.CharacterName, ((LocationNode)n).name));
                                    }
                                }
                            }
                        }
                    }
                    else if (message.StartsWith("/vanish"))
                    {
                        if (player.VanishMode.Equals(false))
                        {
                            UnturnedChat.Say(Init.Instance.Translations.Instance.Translate("vanish_message_enabled", player.CharacterName));
                        }
                        else
                        {
                            UnturnedChat.Say(Init.Instance.Translations.Instance.Translate("vanish_message_disabled", player.CharacterName));
                        }
                    }
                    else if (message.StartsWith("/heal"))
                    {
                        UnturnedChat.Say(Init.Instance.Translations.Instance.Translate("heal_message", player.CharacterName));
                    }
                    else if (message.StartsWith("/i "))
                    {
                        string[] splitstring = message.Substring(2).Split(' ');

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
                                UnturnedChat.Say(Init.Instance.Translations.Instance.Translate("item_message", player.CharacterName, ((ItemAsset)iAssetID).itemName));
                            }
                            else if (iAssetName != null && !(iAssetName.itemName.Contains("0") || iAssetName.itemName.Contains("1")) && message.Substring(2).Length > 0)
                            {
                                UnturnedChat.Say(Init.Instance.Translations.Instance.Translate("item_message", player.CharacterName, iAssetName.itemName));
                            }
                        }
                        else
                        {
                            if (iAssetID != null)
                            {
                                UnturnedChat.Say(Init.Instance.Translations.Instance.Translate("item_message_amount", player.CharacterName, AssetCount.ToString(), ((ItemAsset)iAssetID).itemName));
                            }
                            else if (iAssetName != null && !(iAssetName.itemName.Contains("0") || iAssetName.itemName.Contains("1")) && message.Substring(2).Length > 0)
                            {
                                UnturnedChat.Say(Init.Instance.Translations.Instance.Translate("item_message_amount", player.CharacterName, AssetCount.ToString(), iAssetName.itemName));
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
                            UnturnedChat.Say(Init.Instance.Translations.Instance.Translate("item_message", player.CharacterName, vehiclename));
                        }
                    }
                    else if (message.StartsWith("/spy "))
                    {
                        string data = Library.SteamHTMLRequest(message.Substring(4).Replace(" ", ""));

                        if (data != "")
                        {
                            UnturnedChat.Say(Init.Instance.Translations.Instance.Translate("spy_message", player.CharacterName, data));
                        }
                    }
                    else if (message.StartsWith("/kick "))
                    {
                        kicktarget = null;
                        kicktarget = UnturnedPlayer.FromName(message.Substring(4).Replace(" ", ""));
                        string data = Library.SteamHTMLRequest(message.Substring(4).Replace(" ", ""));

                        if (kicktarget != null)
                        {
                            UnturnedChat.Say(Init.Instance.Translations.Instance.Translate("kick_message", player.CharacterName, kicktarget.CharacterName));
                        }
                        else if (data != "")
                        {
                            UnturnedChat.Say(Init.Instance.Translations.Instance.Translate("kick_message", player.CharacterName, data));
                        }
                    }
                    else if (message.StartsWith("/slay "))
                    {
                        slaytarget = null;
                        slaytarget = UnturnedPlayer.FromName(message.Substring(4).Replace(" ", ""));
                        string data = Library.SteamHTMLRequest(message.Substring(4).Replace(" ", ""));

                        if (slaytarget != null)
                        {
                            UnturnedChat.Say(Init.Instance.Translations.Instance.Translate("slay_message", player.CharacterName, slaytarget.CharacterName));
                        }
                        else if (data != "")
                        {
                            UnturnedChat.Say(Init.Instance.Translations.Instance.Translate("slay_message", player.CharacterName, data));
                        }
                    }
                    else if (message.StartsWith("/admin "))
                    {
                        admintarget = null;
                        admintarget = UnturnedPlayer.FromName(message.Substring(6).Replace(" ", ""));

                        if (admintarget != null)
                        {
                            UnturnedChat.Say(Init.Instance.Translations.Instance.Translate("admin_message", player.CharacterName, admintarget.CharacterName));
                            cancel = true;
                        }
                    }
                }
            }
        }
    }
}
