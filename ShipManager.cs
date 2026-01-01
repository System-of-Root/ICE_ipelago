using System;
using HarmonyLib;

namespace ICE_ipelago{
    [HarmonyPatch]
    public class ShipManager{
        [Flags]
        public enum ShipFlags{
            BlueN = 1 << 0,
            BlueH = 1 << 1,
            InvaderN = 1 << 2,
            InvaderH = 1 << 3,
            LightningN = 1 << 4,
            LightningH = 1 << 5,
            HeartyN = 1 << 6,
            HeartyH = 1 << 7,
            TwinN = 1 << 8,
            TwinH = 1 << 9,
            SpadesN = 1 << 10,
            SpadesH = 1 << 11,
            BigN = 1 << 12,
            BigH = 1 << 13,
            MothershipN = 1 << 14,
            MothershipH = 1 << 15,
            GlassN = 1 << 16,
            GlassH = 1 << 17,
            ROTNN = 1 << 18,
            ROTNH = 1 << 19,
        }
        public static ShipFlags unlockedShips;
        public static ShipFlags currentShip;

        public static bool IsUnlocked(PlayerShipId shipId, int mode = -1){
            switch (shipId)
            {
                case PlayerShipId.BlueFighter:
                    if(mode == 0)
                        return unlockedShips.HasFlag(ShipFlags.BlueN);
                    if(mode == 1)
                        return unlockedShips.HasFlag(ShipFlags.BlueH);
                    return unlockedShips.HasFlag(ShipFlags.BlueN) || unlockedShips.HasFlag(ShipFlags.BlueH);
                case PlayerShipId.TwinShooter:
                    if(mode == 0)
                        return unlockedShips.HasFlag(ShipFlags.TwinN);
                    if(mode == 1)
                        return unlockedShips.HasFlag(ShipFlags.TwinH);
                    return unlockedShips.HasFlag(ShipFlags.TwinN) || unlockedShips.HasFlag(ShipFlags.TwinH);
                case PlayerShipId.LightningBolt:
                    if(mode == 0)
                        return unlockedShips.HasFlag(ShipFlags.LightningN);
                    if(mode == 1)
                        return unlockedShips.HasFlag(ShipFlags.LightningH);
                    return unlockedShips.HasFlag(ShipFlags.LightningN) || unlockedShips.HasFlag(ShipFlags.LightningH);
                case PlayerShipId.GlassCannon:
                    if(mode == 0)
                        return unlockedShips.HasFlag(ShipFlags.GlassN);
                    if(mode == 1)
                        return unlockedShips.HasFlag(ShipFlags.GlassH);
                    return unlockedShips.HasFlag(ShipFlags.GlassN) || unlockedShips.HasFlag(ShipFlags.GlassH);
                case PlayerShipId.BigFFighter:
                    if(mode == 0)
                        return unlockedShips.HasFlag(ShipFlags.BigN);
                    if(mode == 1)
                        return unlockedShips.HasFlag(ShipFlags.BigH);
                    return unlockedShips.HasFlag(ShipFlags.BigN) || unlockedShips.HasFlag(ShipFlags.BigH);
                case PlayerShipId.InvaderDF:
                    if(mode == 0)
                        return unlockedShips.HasFlag(ShipFlags.InvaderN);
                    if(mode == 1)
                        return unlockedShips.HasFlag(ShipFlags.InvaderH);
                    return unlockedShips.HasFlag(ShipFlags.InvaderN) || unlockedShips.HasFlag(ShipFlags.InvaderH);
                case PlayerShipId.Mothership:
                    if(mode == 0)
                        return unlockedShips.HasFlag(ShipFlags.MothershipN);
                    if(mode == 1)
                        return unlockedShips.HasFlag(ShipFlags.MothershipH);
                    return unlockedShips.HasFlag(ShipFlags.MothershipN) || unlockedShips.HasFlag(ShipFlags.MothershipH);
                case PlayerShipId.HeartyDuran:
                    if(mode == 0)
                        return unlockedShips.HasFlag(ShipFlags.HeartyN);
                    if(mode == 1)
                        return unlockedShips.HasFlag(ShipFlags.HeartyH);
                    return unlockedShips.HasFlag(ShipFlags.HeartyN) || unlockedShips.HasFlag(ShipFlags.HeartyH);
                case PlayerShipId.SpadesOfACE:
                    if(mode == 0)
                        return unlockedShips.HasFlag(ShipFlags.SpadesN);
                    if(mode == 1)
                        return unlockedShips.HasFlag(ShipFlags.SpadesH);
                    return unlockedShips.HasFlag(ShipFlags.SpadesN) || unlockedShips.HasFlag(ShipFlags.SpadesH);
                case PlayerShipId.ROTN:
                    if(mode == 0)
                        return unlockedShips.HasFlag(ShipFlags.ROTNN);
                    if(mode == 1)
                        return unlockedShips.HasFlag(ShipFlags.ROTNH);
                    return unlockedShips.HasFlag(ShipFlags.ROTNN) || unlockedShips.HasFlag(ShipFlags.ROTNH);
                default:
                    return false;
            }

        }

        [HarmonyPatch(typeof(GameModeSelect), nameof(GameModeSelect.OnShipSelected))]
        [HarmonyPostfix]
        public static void OnShipSelected(GameModeSelect __instance, PlayerShipData playerShip){
            __instance._difficultyToggles[0]._locked = !IsUnlocked(playerShip._id,0);
            __instance._difficultyToggles[0].Start();
            __instance._difficultyToggles[1]._locked = !IsUnlocked(playerShip._id,1);
            __instance._difficultyToggles[1].Start();
            __instance._difficultyToggles[2]._locked = true;
            __instance._difficultyToggles[2].Start();
        }

        [HarmonyPatch(typeof(GameModeSelect), nameof(GameModeSelect.OnRun))]
        [HarmonyPrefix]
        public static bool OnRun(){
            if(!IsUnlocked(Singleton<PlayerShipManager>.Instance.SelectedPlayerShip._id,
                   Singleton<GameModeManager>.Instance.SelectedDifficultyMode._id == DifficultyModeId.Normal ? 0 : 1)){
                Singleton<AudioManager>.Instance.PlaySound(SoundId.MenuErrorSound);
                return false;
            }

            switch(Singleton<PlayerShipManager>.Instance.SelectedPlayerShip._id){
                case PlayerShipId.BlueFighter:
                    currentShip = (Singleton<GameModeManager>.Instance.SelectedDifficultyMode._id == DifficultyModeId.Normal ? ShipFlags.BlueN : ShipFlags.BlueH);
                    break;
                case PlayerShipId.TwinShooter:
                    currentShip = (Singleton<GameModeManager>.Instance.SelectedDifficultyMode._id == DifficultyModeId.Normal ? ShipFlags.TwinN : ShipFlags.TwinH);
                    break;
                case PlayerShipId.LightningBolt:
                    currentShip = (Singleton<GameModeManager>.Instance.SelectedDifficultyMode._id == DifficultyModeId.Normal ? ShipFlags.LightningN : ShipFlags.LightningH);
                    break;
                case PlayerShipId.GlassCannon:
                    currentShip = (Singleton<GameModeManager>.Instance.SelectedDifficultyMode._id == DifficultyModeId.Normal ? ShipFlags.GlassN : ShipFlags.GlassH);
                    break;
                case PlayerShipId.BigFFighter:
                    currentShip = (Singleton<GameModeManager>.Instance.SelectedDifficultyMode._id == DifficultyModeId.Normal ? ShipFlags.BigN : ShipFlags.BigH);
                    break;
                case PlayerShipId.InvaderDF:
                    currentShip = (Singleton<GameModeManager>.Instance.SelectedDifficultyMode._id == DifficultyModeId.Normal ? ShipFlags.InvaderN : ShipFlags.InvaderH);
                    break;
                case PlayerShipId.Mothership:
                    currentShip = (Singleton<GameModeManager>.Instance.SelectedDifficultyMode._id == DifficultyModeId.Normal ? ShipFlags.MothershipN : ShipFlags.MothershipH);
                    break;
                case PlayerShipId.HeartyDuran:
                    currentShip = (Singleton<GameModeManager>.Instance.SelectedDifficultyMode._id == DifficultyModeId.Normal ? ShipFlags.HeartyN : ShipFlags.HeartyH);
                    break;
                case PlayerShipId.SpadesOfACE:
                    currentShip = (Singleton<GameModeManager>.Instance.SelectedDifficultyMode._id == DifficultyModeId.Normal ? ShipFlags.SpadesN : ShipFlags.SpadesH);
                    break;
                case PlayerShipId.ROTN:
                    currentShip = (Singleton<GameModeManager>.Instance.SelectedDifficultyMode._id == DifficultyModeId.Normal ? ShipFlags.ROTNN : ShipFlags.ROTNH);
                    break;
            }
            
            
            return true;
        }
        
        
        [HarmonyPatch(typeof(PlayerShipManager), nameof(PlayerShipManager.GetShipUnlocked))]
        [HarmonyPrefix]
        public static bool GetShipUnlocked(PlayerShipManager __instance, PlayerShipId shipId,ref bool __result){
            __result = IsUnlocked(shipId);
            return false;
        }
    }
}