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
    }
}
