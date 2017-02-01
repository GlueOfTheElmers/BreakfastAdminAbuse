using Rocket.API.Collections;
using Rocket.Core.Plugins;
using SDG.Unturned;
using Steamworks;
using static Rocket.Unturned.Events.UnturnedPlayerEvents;

namespace DefCon42
{
    public class Init : RocketPlugin<ACRConfig>
    {
        public static Init Instance;
        AdminCommandReporter acr = new AdminCommandReporter();
        protected override void Load()
        {
            Instance = this;
            Rocket.Core.Logging.Logger.Log("[Breakfast Utils] Anti Admin Abuse plugin loaded!");
            OnPlayerChatted += acr.UnturnedPlayerEvents_OnPlayerChatted;
        }
        protected override void Unload()
        {
            Rocket.Core.Logging.Logger.Log("[Breakfast Utils] Anti Admin Abuse plugin unloaded!");
            OnPlayerChatted -= acr.UnturnedPlayerEvents_OnPlayerChatted;

        }
        public override TranslationList DefaultTranslations
        {
            get
            {
                return new TranslationList
                {
                    { "god_message_enabled", "{0} has enabled god mode!" },
                    { "god_message_disabled", "{0} has disabled god mode!" },
                    { "airdrop_message", "{0} has spawned in an airdrop!" },
                    { "massdrop_message", "{0} has spawned in a mass airdrop!" },
                    { "tp_message", "{0} has teleported to {1}!" },
                    { "tp_message_self", "{0} tried to teleport to himself.." },
                    { "teleport_message", "{0} has teleported to {1}!" },
                    { "teleport_message_player_to_player", "{1} has been teleported to {2} by {3}!" },
                    { "teleport_message_player_to_location", "{1} has teleported to {2} by {3}!" },
                    { "teleport_message_self", "{0} tried to teleport to himself.." },
                    { "vanish_message_enabled", "{0} has turned on vanish!" },
                    { "vanish_message_disabled", "{0} has turned off vanish!" },
                    { "heal_message", "{0} has healed himself!" },
                    { "heal_message_player", "{0} has healed {1}!" },
                    { "item_message", "{0} spawned in {1}!" },
                    { "item_message_amount", "{0} spawned in {1}x {2}!" },
                    { "vehicle_message", "{0} has spawned in {1}!" },
                    { "spy_message", "{0} has spied on {1}!" },
                    { "kick_message", "{0} has kicked {1}!" },
                    { "slay_message", "{0} has banned {1}!" },
                    { "admin_message", "{0} has admined {1}!" }
                };
            }
        }
    }
}
