using System;

namespace ICE_ipelago
{
    [Serializable]
    public class APSave:Save{

        public int unlockFags;
        public int[] progress = new int[20];
        public bool[] unlockedCards = new bool[53];
    }
}