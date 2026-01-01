using HarmonyLib;
using UnityEngine;

namespace ICE_ipelago
{
    [HarmonyPatch(typeof(Card), nameof(Card.OnCardSelected))]
    public class CardPickPatch{
        public static bool[] unlockedCards =  new bool[53];
        public static bool Prefix(Card __instance)
        {
            if(unlockedCards[(int)__instance._cardId]) return true;
            Singleton<AudioManager>.Instance.PlaySound(SoundId.MenuCardSound);
            __instance._cardName.text = "Locked";
            __instance._cardName.color = Color.red;
            return false;
        }
        
        [HarmonyPatch(typeof(Card), nameof(Card.SetCard))]
        public static void Postfix(Card __instance){
            if(unlockedCards[(int)__instance._cardId]) return;
            __instance._cardName.text = "Locked";
            __instance._cardName.color = Color.red;
        }
    }
}