using System;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Packets;
using HarmonyLib;
using MelonLoader;
using UnityEngine;

namespace ICE_ipelago{
    [HarmonyPatch]
    public class CheckManager{
        public static int[] progress = new int[20];

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Game),nameof(Game.OnLevelWin))]
        public static void OnLevelWin(Game __instance){
            MelonLogger.Msg($"OnLevelWin: {Singleton<LevelManager>.Instance.CurrentLevel.Id}");
            int index = (int)Mathf.Log((int)ShipManager.currentShip,2);
            MelonLogger.Msg($"{progress[index]}");
            switch(Singleton<LevelManager>.Instance.CurrentLevel.Id){
                case LevelId.Level1_5Boss:
                    if(progress[index] < 1){
                        progress[index]++;
                        Plugin.session.Locations.CompleteLocationChecks(
                            Plugin.session.Locations.GetLocationIdFromName("ICEwall",
                                $"{Singleton<PlayerShipManager>.Instance.SelectedPlayerShip._name} Beat Brain ({(Singleton<GameModeManager>.Instance.SelectedDifficultyMode._id == DifficultyModeId.Normal ?"normal":"hard")})"));
                    }
                    break;
                case LevelId.Level2_5Boss:
                    if(progress[index] < 2){
                        progress[index]++;
                        Plugin.session.Locations.CompleteLocationChecks(
                            Plugin.session.Locations.GetLocationIdFromName("ICEwall",
                                $"{Singleton<PlayerShipManager>.Instance.SelectedPlayerShip._name} Beat Pirate ({(Singleton<GameModeManager>.Instance.SelectedDifficultyMode._id == DifficultyModeId.Normal ?"normal":"hard")})"));
                    }
                    break;
                case LevelId.Level3_5Boss:
                    if(progress[index] < 3){
                        progress[index]++;
                        Plugin.session.Locations.CompleteLocationChecks(
                            Plugin.session.Locations.GetLocationIdFromName("ICEwall",
                                $"{Singleton<PlayerShipManager>.Instance.SelectedPlayerShip._name} Beat Tiger ({(Singleton<GameModeManager>.Instance.SelectedDifficultyMode._id == DifficultyModeId.Normal ?"normal":"hard")})"));
                    }
                    break;
                case LevelId.Level4_5Boss:
                    if(progress[index] < 4){
                        progress[index]++;
                        Plugin.session.Locations.CompleteLocationChecks(
                            Plugin.session.Locations.GetLocationIdFromName("ICEwall",
                                $"{Singleton<PlayerShipManager>.Instance.SelectedPlayerShip._name} Beat Death ({(Singleton<GameModeManager>.Instance.SelectedDifficultyMode._id == DifficultyModeId.Normal ?"normal":"hard")})"));
                    }
                    break;
                case LevelId.Level5_1ROTN:
                    if(progress[index] < 5){
                        progress[index]++;
                        Plugin.session.Locations.CompleteLocationChecks(
                            Plugin.session.Locations.GetLocationIdFromName("ICEwall",
                                $"{Singleton<PlayerShipManager>.Instance.SelectedPlayerShip._name} Beat ROTN ({(Singleton<GameModeManager>.Instance.SelectedDifficultyMode._id == DifficultyModeId.Normal ?"normal":"hard")})"));
                    }

                    int goal = 0;
                    for(int i = Plugin.hard? 1:0; i < progress.Length; i += Plugin.hard? 2:1){
                        if(progress[i] >= 5){
                            goal++;
                        }
                    }
                    if(goal>=Plugin.goal){
                        StatusUpdatePacket statusUpdatePacket = new StatusUpdatePacket();
                        statusUpdatePacket.Status = ArchipelagoClientState.ClientGoal;
                        Plugin.session.Socket.SendPacket(statusUpdatePacket);
                        MelonLogger.MsgPastel(ConsoleColor.Yellow, "Complete Goal");
                    }
                    break;
            }
            SaveManager.Instance.Save();
        }
    }
}