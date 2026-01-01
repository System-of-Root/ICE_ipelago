using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Packets;
using HarmonyLib;
using MelonLoader;

namespace ICE_ipelago{
    [HarmonyPatch]
    public class CheckManager{
        public static int[] progress = new int[20];

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Game),nameof(Game.OnLevelWin))]
        public static void OnLevelWin(Game __instance){
            MelonLogger.Msg($"OnLevelWin: {Singleton<LevelManager>.Instance.CurrentLevel.Id}");
            MelonLogger.Msg($"{progress[(int)ShipManager.currentShip]}");
            switch(Singleton<LevelManager>.Instance.CurrentLevel.Id){
                case LevelId.Level1_5Boss:
                    if(progress[(int)ShipManager.currentShip] < 1){
                        progress[(int)ShipManager.currentShip]++;
                        Plugin.session.Locations.CompleteLocationChecks(
                            Plugin.session.Locations.GetLocationIdFromName("ICEwall",
                                $"{Singleton<PlayerShipManager>.Instance.SelectedPlayerShip._name} Beat Brain ({(Singleton<GameModeManager>.Instance.SelectedDifficultyMode._id == DifficultyModeId.Normal ?"normal":"hard")} )"));
                    }
                    break;
                case LevelId.Level2_5Boss:
                    if(progress[(int)ShipManager.currentShip] < 2){
                        progress[(int)ShipManager.currentShip]++;
                        Plugin.session.Locations.CompleteLocationChecks(
                            Plugin.session.Locations.GetLocationIdFromName("ICEwall",
                                $"{Singleton<PlayerShipManager>.Instance.SelectedPlayerShip._name} Beat Pirate ({(Singleton<GameModeManager>.Instance.SelectedDifficultyMode._id == DifficultyModeId.Normal ?"normal":"hard")} )"));
                    }
                    break;
                case LevelId.Level3_5Boss:
                    if(progress[(int)ShipManager.currentShip] < 3){
                        progress[(int)ShipManager.currentShip]++;
                        Plugin.session.Locations.CompleteLocationChecks(
                            Plugin.session.Locations.GetLocationIdFromName("ICEwall",
                                $"{Singleton<PlayerShipManager>.Instance.SelectedPlayerShip._name} Beat Tiger ({(Singleton<GameModeManager>.Instance.SelectedDifficultyMode._id == DifficultyModeId.Normal ?"normal":"hard")} )"));
                    }
                    break;
                case LevelId.Level4_5Boss:
                    if(progress[(int)ShipManager.currentShip] < 4){
                        progress[(int)ShipManager.currentShip]++;
                        Plugin.session.Locations.CompleteLocationChecks(
                            Plugin.session.Locations.GetLocationIdFromName("ICEwall",
                                $"{Singleton<PlayerShipManager>.Instance.SelectedPlayerShip._name} Beat Death ({(Singleton<GameModeManager>.Instance.SelectedDifficultyMode._id == DifficultyModeId.Normal ?"normal":"hard")} )"));
                    }
                    break;
                case LevelId.Level5_1ROTN:
                    if(progress[(int)ShipManager.currentShip] < 5){
                        progress[(int)ShipManager.currentShip]++;
                        Plugin.session.Locations.CompleteLocationChecks(
                            Plugin.session.Locations.GetLocationIdFromName("ICEwall",
                                $"{Singleton<PlayerShipManager>.Instance.SelectedPlayerShip._name} Beat ROTN ({(Singleton<GameModeManager>.Instance.SelectedDifficultyMode._id == DifficultyModeId.Normal ?"normal":"hard")} )"));
                    }

                    bool goal = true;
                    for(int i = 0; i < progress.Length; i++){
                        if(progress[i] < 5){
                            goal = false;
                            break;
                        }
                    }
                    if(goal){
                        StatusUpdatePacket statusUpdatePacket = new StatusUpdatePacket();
                        statusUpdatePacket.Status = ArchipelagoClientState.ClientGoal;
                        Plugin.session.Socket.SendPacket(statusUpdatePacket);
                        MelonLogger.Msg("Complete Goal");
                    }
                    break;
            }
            SaveManager.Instance.Save();
        }
    }
}