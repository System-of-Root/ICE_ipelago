using System;
using System.Collections.Generic;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Helpers;
using MelonLoader;

namespace ICE_ipelago{
    public class Plugin:MelonMod{
        public const string ModName = "Archipelago";
        public const string Version = "0.5.0";
        public const string Author = "Root";
        public static ArchipelagoSession session;
        public static Plugin instance;
        public static DeathLinkService deathLink;
        public static bool doDeathLink = false;
        public static bool deathLinkEnabled = false;
        public static string slotName;
        public static int goal = 20;
        public static bool hard = false;
        public Dictionary<string, object> SlotData;

        public override void OnEarlyInitializeMelon(){
            LoggerInstance.Msg("Loaded!!!");
            instance = this;
        }

        public override void OnInitializeMelon(){
            new HarmonyLib.Harmony(ModName).PatchAll();
        }

        public override void OnLateInitializeMelon(){
            //Connect("localhost:38281", "test");
        }
        
        public void Connect(string server, string user, string pass = null)
        {
            LoggerInstance.Msg("Connecting");
            APSaveManager.LoadSlot(user);
            slotName = user;
            session = ArchipelagoSessionFactory.CreateSession(server);
            session.Socket.SocketClosed += reason => ConnectionMenu.Active = true;
            session.Items.ItemReceived += OnItemReceived;
            deathLink = session.CreateDeathLinkService();
            LoginResult result;

            try
            {
                // handle TryConnectAndLogin attempt here and save the returned object to `result`
                result = session.TryConnectAndLogin("ICEwall", user, ItemsHandlingFlags.AllItems, password: pass);
            }
            catch (Exception e)
            {
                result = new LoginFailure(e.GetBaseException().Message);
            }

            if (!result.Successful)
            {
                LoginFailure failure = (LoginFailure)result;
                string errorMessage = $"Failed to Connect to {server} as {user}:";
                foreach (string error in failure.Errors)
                {
                    errorMessage += $"\n    {error}";
                }
                foreach (ConnectionRefusedError error in failure.ErrorCodes)
                {
                    errorMessage += $"\n    {error}";
                }
                MelonLogger.Error(errorMessage);
                return; // Did not connect, show the user the contents of `errorMessage`
            }
    
            // Successfully connected, `ArchipelagoSession` (assume statically defined as `session` from now on) can now be
            // used to interact with the server and the returned `LoginSuccessful` contains some useful information about the
            // initial connection (e.g. a copy of the slot data as `loginSuccess.SlotData`)
            var loginSuccess = (LoginSuccessful)result;
            MelonLogger.Msg($"Successfully connected to {server}");
            ConnectionMenu.Active = false;
            SlotData = loginSuccess.SlotData;
            
            if(SlotData.TryGetValue("death_link", out object dl) && (bool)dl){
                deathLink.EnableDeathLink();
                deathLink.OnDeathLinkReceived += OnDeathLink;
                deathLinkEnabled = true;
            }
            hard = SlotData.TryGetValue("hard_only", out object hd) && (bool)hd;
            goal = SlotData.TryGetValue("goal", out object g) ? (int)g : 20;
        }

        private void OnDeathLink(DeathLink link){
            doDeathLink = true;
            MelonLogger.MsgPastel(System.ConsoleColor.Magenta, link.Cause ?? link.Source + " died");
            Singleton<PlayerStatsManager>.Instance?.AddHealth(-999999);
        }

        private void OnItemReceived(ReceivedItemsHelper helper){
            string itemName = helper.PeekItem().ItemName;
            MelonLogger.Msg($"Received {itemName}");
            if(Enum.TryParse(itemName, out CardId id)){
                CardPickPatch.unlockedCards[(int)id] = true;
                MelonLogger.Msg($"Successfully unlocked {itemName} disk");
            } else if(itemName.EndsWith("(normal)")){
                switch(itemName.Substring(0,3)){
                    case "Blu":
                        ShipManager.unlockedShips |= ShipManager.ShipFlags.BlueN;
                        break;
                    case "Inv":
                        ShipManager.unlockedShips |= ShipManager.ShipFlags.InvaderN;
                        break;
                    case "Lig":
                        ShipManager.unlockedShips |= ShipManager.ShipFlags.LightningN;
                        break;
                    case "Hea":
                        ShipManager.unlockedShips |= ShipManager.ShipFlags.HeartyN;
                        break;
                    case "Twi":
                        ShipManager.unlockedShips |= ShipManager.ShipFlags.TwinN;
                        break;
                    case "Spa":
                        ShipManager.unlockedShips |= ShipManager.ShipFlags.SpadesN;
                        break;
                    case "Big":
                        ShipManager.unlockedShips |= ShipManager.ShipFlags.BigN;
                        break;
                    case "Mot":
                        ShipManager.unlockedShips |= ShipManager.ShipFlags.MothershipN;
                        break;
                    case "Gla":
                        ShipManager.unlockedShips |= ShipManager.ShipFlags.GlassN;
                        break;
                    case "ROT":
                        ShipManager.unlockedShips |= ShipManager.ShipFlags.ROTNN;
                        break;
                }
                MelonLogger.Msg($"Successfully unlocked ship {itemName}   {ShipManager.unlockedShips}");
            } else if(itemName.EndsWith("(hard)")){
                switch(itemName.Substring(0,3)){
                    case "Blu":
                        ShipManager.unlockedShips |= ShipManager.ShipFlags.BlueH;
                        break;
                    case "Inv":
                        ShipManager.unlockedShips |= ShipManager.ShipFlags.InvaderH;
                        break;
                    case "Lig":
                        ShipManager.unlockedShips |= ShipManager.ShipFlags.LightningH;
                        break;
                    case "Hea":
                        ShipManager.unlockedShips |= ShipManager.ShipFlags.HeartyH;
                        break;
                    case "Twi":
                        ShipManager.unlockedShips |= ShipManager.ShipFlags.TwinH;
                        break;
                    case "Spa":
                        ShipManager.unlockedShips |= ShipManager.ShipFlags.SpadesH;
                        break;
                    case "Big":
                        ShipManager.unlockedShips |= ShipManager.ShipFlags.BigH;
                        break;
                    case "Mot":
                        ShipManager.unlockedShips |= ShipManager.ShipFlags.MothershipH;
                        break;
                    case "Gla":
                        ShipManager.unlockedShips |= ShipManager.ShipFlags.GlassH;
                        break;
                    case "ROT":
                        ShipManager.unlockedShips |= ShipManager.ShipFlags.ROTNH;
                        break;
                }
            } else{
                //TODO: handle filler items here.
                switch(itemName){
                    case "Damage Multiplier":
                        ShipPatch.damage++;
                        break;
                    case "Projectile Size Multiplier":
                        ShipPatch.size++;
                        break;
                    case "Projectile Speed Multiplier":
                        ShipPatch.speed++;
                        break;
                    case "Score Multiplier":
                        //TODO: implement;
                        break;
                    default:
                        MelonLogger.Msg($"Item {itemName} not found");
                        break;
                }
            }

            helper.DequeueItem();
        }
    }
}