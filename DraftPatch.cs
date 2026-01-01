using System.Collections.Generic;
using HarmonyLib;

namespace ICE_ipelago
{
  [HarmonyPatch(typeof(Draft), nameof(Draft.SetDraftCards))]
  public class DraftPatch
  {
    public static bool Prefix(Draft __instance)
    {
      List<CardId> draftableCards = Singleton<CardManager>.Instance.GetDraftableCards();
      List<CardId> cardIdList1 = new List<CardId>();
      List<CardId> cardIdList2 = new List<CardId>();
      for (int index = 0; index < draftableCards.Count; ++index)
      {
        if (Singleton<CardManager>.Instance.GetCard(draftableCards[index]).Super)
          cardIdList2.Add(draftableCards[index]);
        else
          cardIdList1.Add(draftableCards[index]);
      }

      Card[] cardArray = __instance._draftFour ? __instance._draftCardsFour : __instance._draftCardsThree;
      bool flag1 = false;
      bool flag2 = false;
      if (!Singleton<CardManager>.Instance.AllRareDraft)
      {
        flag1 = UnityEngine.Random.Range(0, 100) < 2;
        if (flag1)
          flag2 = UnityEngine.Random.Range(0, 100) < 20;
      }

      if (!Singleton<CardManager>.Instance.MultiDraft && !flag2)
      {
        flag2 = UnityEngine.Random.Range(0, 100) < 1;
        if (flag2 && !Singleton<CardManager>.Instance.AllRareDraft && !flag1)
          flag1 = UnityEngine.Random.Range(0, 100) < 20;
      }

      if (flag1 & flag2)
      {
        Singleton<CardManager>.Instance.SetAllRareDraft();
        __instance._draftCount = cardArray.Length;
        Singleton<CardManager>.Instance.SetMultiDraft();
        Singleton<AudioManager>.Instance.PlaySound(SoundId.LuckySuperSound);
        __instance._sideTextThree.text = "SUPER JACKPOT DRAFT EVENT!!! CHOOSE ANY NUMBER";
        __instance._sideTextFour.text = "SUPER JACKPOT DRAFT EVENT!!! CHOOSE ANY NUMBER";
        __instance._superEventAnimator.SetTrigger("SuperDuper");
      }
      else if (flag1)
      {
        __instance._sideTextThree.text = "SUPER RARE DRAFT EVENT!!!";
        __instance._sideTextFour.text = "SUPER RARE DRAFT EVENT!!!";
        __instance._superEventAnimator.SetTrigger("Super");
        Singleton<AudioManager>.Instance.PlaySound(SoundId.LuckySuperSound);
        Singleton<CardManager>.Instance.SetAllRareDraft();
      }
      else if (flag2)
      {
        __instance._sideTextThree.text = "SUPER MULTI DRAFT EVENT!!! CHOOSE ANY NUMBER";
        __instance._sideTextFour.text = "SUPER MULTI DRAFT EVENT!!! CHOOSE ANY NUMBER";
        __instance._superEventAnimator.SetTrigger("Super");
        Singleton<AudioManager>.Instance.PlaySound(SoundId.LuckySuperSound);
        __instance._draftCount = cardArray.Length;
        Singleton<CardManager>.Instance.SetMultiDraft();
      }

      for (int index1 = 0; index1 < cardArray.Length; ++index1)
      {
        int num = UnityEngine.Random.Range(0, 100);
        if (flag1)
          num = 0;
        if (Singleton<CardManager>.Instance.PlayerDeck.Contains(CardId.Singularity))
        {
          if (num < Singleton<CardManager>.Instance.SuperChance)
            cardArray[index1].SetCard(Singleton<CardManager>.Instance.GetCard(CardId.SingularityExplosion));
          else
            cardArray[index1].SetCard(Singleton<CardManager>.Instance.GetCard(CardId.SingularityGrowth));
        }
        else if (num < Singleton<CardManager>.Instance.SuperChance && cardIdList2.Count > 0 || cardIdList1.Count < 1)
        {
          int index2 = UnityEngine.Random.Range(0, cardIdList2.Count);
          cardArray[index1].SetCard(Singleton<CardManager>.Instance.GetCard(cardIdList2[index2]));
          cardIdList2.RemoveAt(index2);
        }
        else
        {
          int index3 = UnityEngine.Random.Range(0, cardIdList1.Count);
          cardArray[index1].SetCard(Singleton<CardManager>.Instance.GetCard(cardIdList1[index3]));
          cardIdList1.RemoveAt(index3);
        }
      }
      
      return false;
    }
  }
}