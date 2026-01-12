using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using HarmonyLib;
using UnityEngine;

namespace ICE_ipelago{
    [HarmonyPatch(typeof(SaveManager))]
    public class APSaveManager{
        
        public static void LoadSlot(string slot)
        {
            SaveManager.Instance._savePath = Application.persistentDataPath + $"/{slot}.APsave";

            SaveManager.Instance.Load();
        }

        public static void DeleteSlot(string slot){
            File.Delete(Application.persistentDataPath + $"/{slot}.APsave");
        }
        
        
        [HarmonyPrefix]
        [HarmonyPatch(nameof(SaveManager.Save))]
        public static bool SavePrefix(SaveManager __instance)
        {
            if (__instance._savePath.EndsWith(".aeaa")) return true;

            APSave graph = new APSave()
            {
                PlayerPoints = 0,
                Fullscreen = Singleton<SettingsManager>.Instance.FullscreenValue,
                VSync = Singleton<SettingsManager>.Instance.VSyncValue,
                FPSCapped = Singleton<SettingsManager>.Instance.FPSCappedValue,
                Music = Singleton<AudioManager>.Instance.MusicVolume,
                Sound = Singleton<AudioManager>.Instance.SoundVolume,
                CameraShake = Singleton<SettingsManager>.Instance.CameraShakeValue,
                EnemyHitFlash = Singleton<SettingsManager>.Instance.EnemyHitFlashValue,
                Vibrations = Singleton<SettingsManager>.Instance.GamepadVibrationsValue,
                AnimatedBackgrounds = Singleton<SettingsManager>.Instance.AnimatedBackgroundsValue,
                DamageNumbers = Singleton<SettingsManager>.Instance.DamageNumbersValue,
                AutoFire = Singleton<SettingsManager>.Instance.AutoFireValue,
                MouseOnly = Singleton<SettingsManager>.Instance.MouseOnlyValue,
                AimAssist = Singleton<SettingsManager>.Instance.AimAssistValue,

                SelectedDifficultyMode = 0,
                SelectedDraftMode = 0,
                FirstTimeConsole = 0,
                FirstTimeROTN = 0,
                HardUnlocked = 0,
                BigFFighter = 0,
                GlassCannon = 0,
                HeartyDuran = 0,
                InvaderDF = 0,
                LightningBolt = 0,
                Mothership = 0,
                ROTN = 0,
                SpadesOfACE = 0,
                TwinShooter = 0,
                SelectedPlayerShip = 0,
                unlockFags = (int)ShipManager.unlockedShips,
                unlockedCards = CardPickPatch.unlockedCards,
                progress = CheckManager.progress
            };
            Singleton<PlayerPointsManager>.Instance.SetPlayerPoints(0, false);
            using (FileStream serializationStream = File.Create(SaveManager.Instance._savePath))
                new BinaryFormatter().Serialize(serializationStream, graph);

            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(SaveManager.Load))]
        public static bool LoadPrefix(SaveManager __instance)
        {
            if (__instance._savePath.EndsWith(".aeaa")) return true;

            if (!File.Exists(SaveManager.Instance._savePath))
            {
                SavePrefix(__instance);
            }
            else
            {
                APSave save;
                using (FileStream serializationStream = File.Open(__instance._savePath, FileMode.Open))
                    save = (APSave)new BinaryFormatter().Deserialize((Stream)serializationStream);
                Singleton<PlayerPointsManager>.Instance.SetPlayerPoints(save.PlayerPoints, false);
                Singleton<SettingsManager>.Instance.SetFullscreenValue(save.Fullscreen);
                Singleton<SettingsManager>.Instance.SetVSyncValue(save.VSync);
                Singleton<SettingsManager>.Instance.SetFPSCappedValue(save.FPSCapped);
                Singleton<AudioManager>.Instance.SetMusicVolume(save.Music);
                Singleton<AudioManager>.Instance.SetSoundVolume(save.Sound);
                Singleton<SettingsManager>.Instance.SetCameraShakeValue(save.CameraShake);
                Singleton<SettingsManager>.Instance.SetEnemyHitFlashValue(save.EnemyHitFlash);
                Singleton<SettingsManager>.Instance.SetGamepadVibrationsValue(save.Vibrations);
                Singleton<SettingsManager>.Instance.SetAnimatedBackgroundsValue(save.AnimatedBackgrounds);
                Singleton<SettingsManager>.Instance.SetDamageNumbersValue(save.DamageNumbers);
                Singleton<SettingsManager>.Instance.SetAutoFireValue(save.AutoFire);
                Singleton<SettingsManager>.Instance.SetMouseOnlyValue(save.MouseOnly);
                Singleton<SettingsManager>.Instance.SetAimAssistValue(save.AimAssist);
                Singleton<GameModeManager>.Instance.SetSelectedDifficultyMode(
                    Singleton<GameModeManager>.Instance.SelectedDifficultyModeFromSaveVal(save.SelectedDifficultyMode),
                    false);
                Singleton<GameModeManager>.Instance.SetSelectedDraftMode(
                    Singleton<GameModeManager>.Instance.SelectedDraftModeFromSaveVal(save.SelectedDraftMode), false);
                Singleton<TextManager>.Instance.SetFirstTimeConsoleValue(save.FirstTimeConsole, false);
                Singleton<TextManager>.Instance.SetFirstTimeROTNValue(save.FirstTimeROTN, false);
                Singleton<GameModeManager>.Instance.SetHardUnlockValue(save.HardUnlocked, false);
                Singleton<PlayerShipManager>.Instance.SetBigFFighterUnlockValue(save.BigFFighter, false);
                Singleton<PlayerShipManager>.Instance.SetGlassCannonUnlockValue(save.GlassCannon, false);
                Singleton<PlayerShipManager>.Instance.SetHeartyDuranUnlockValue(save.HeartyDuran, false);
                Singleton<PlayerShipManager>.Instance.SetInvaderDFUnlockValue(save.InvaderDF, false);
                Singleton<PlayerShipManager>.Instance.SetLightningBoltUnlockValue(save.LightningBolt, false);
                Singleton<PlayerShipManager>.Instance.SetMothershipUnlockValue(save.Mothership, false);
                Singleton<PlayerShipManager>.Instance.SetROTNFighterUnlockValue(save.ROTN, false);
                Singleton<PlayerShipManager>.Instance.SetSpadesOfAceUnlockValue(save.SpadesOfACE, false);
                Singleton<PlayerShipManager>.Instance.SetTwinShooterUnlockValue(save.TwinShooter, false);
                Singleton<PlayerShipManager>.Instance.SetSelectedPlayerShip(
                    Singleton<PlayerShipManager>.Instance.SelectedPlayerShipFromSaveVal(save.SelectedPlayerShip),
                    false);

                ShipManager.unlockedShips = (ShipManager.ShipFlags)save.unlockFags;
                CardPickPatch.unlockedCards = save.unlockedCards;
                CheckManager.progress = save.progress;
            }

            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(AchievementsManager), nameof(AchievementsManager.UnlockAchievement))]
        public static bool DisableAchievements(){
            return false;
        }
    }
}