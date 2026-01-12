using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using HarmonyLib;

namespace ICE_ipelago{
    [HarmonyPatch]
    public class ShipPatch{
        public static int damage = 0;
        public static int size = 0;
        public static int speed = 0;
        
        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlayerShip),nameof(PlayerShip.KillPlayer))]
        public static void OnDeath(){
            if(Plugin.deathLinkEnabled && !Plugin.doDeathLink)
                Plugin.deathLink.SendDeathLink(new DeathLink(Plugin.slotName));
            Plugin.doDeathLink = false;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PlayerStatsManager), nameof(PlayerStatsManager.SetRemainingStats))]
        public static void SetRemainingStats(){
            Singleton<PlayerStatsManager>.Instance._extraAttack += damage;
            Singleton<PlayerStatsManager>.Instance._extraProjectileSize += size;
            Singleton<PlayerStatsManager>.Instance._extraProjectileSpeed += speed*5;
        }
    }
}