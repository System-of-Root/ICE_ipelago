using HarmonyLib;
using UnityEngine;

namespace ICE_ipelago{
    [HarmonyPatch(typeof(MainMenu),nameof(MainMenu.Start))]
    public class ConnectionMenu : MonoBehaviour{
        public static ConnectionMenu instance;
        public static bool Active = true;
        public string slot;
        public string server;
        public string port = "";
        public string password;

        public static void Postfix(MainMenu __instance){
            instance = __instance.GetComponent<ConnectionMenu>();
            if(instance == null) 
                instance = __instance.gameObject.AddComponent<ConnectionMenu>();
        }
        
        void OnGUI()
        {
            if(!Active) return;
            
            GUI.Box(new Rect(10,10,200,220), "Archipelago Settings");
            GUI.SetNextControlName("slotname");
            GUI.Label(new Rect(25, 30, 150, 30), "Slot");
            slot = GUI.TextField(new Rect(25, 30, 150, 30), slot);
            GUI.SetNextControlName("server");
            GUI.Label(new Rect(25, 30*2, 150, 30), "Server");
            server = GUI.TextField(new Rect(25, 30*2, 150, 30), server);
            GUI.SetNextControlName("port");
            GUI.Label(new Rect(25, 30*3, 150, 30), "Port");
            port = GUI.TextField(new Rect(25, 30*3, 150, 30), port); 
            GUI.SetNextControlName("password");
            GUI.Label(new Rect(25, 30*4, 150, 30), "Password");
            password = GUI.TextField(new Rect(25, 30*4, 150, 30), password);
            string connecton = server;
            
            if(port != "") connecton += ":" + port;
            if (GUI.Button(new Rect(25, 30*5+5, 150, 25), "Continue"))
            {
                Plugin.instance.Connect(connecton,slot,password == ""? null : password);
            }
            if (GUI.Button(new Rect(25, 30*6+5, 150, 25), "New"))
            {
                APSaveManager.DeleteSlot(slot);
                Plugin.instance.Connect(connecton,slot,password == ""? null : password);
            }
        }
    }
}