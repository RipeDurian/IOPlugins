using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BepInEx;
using BepInEx.Logging;
using Logger = BepInEx.Logger;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IOMozaicFree
{
    [BepInPlugin(GUID: "IOMozaicFree", Name: "RipeDurian.InsultOrder.Uncensored", Version: "0.1")]
    public class Uncensorerd : BaseUnityPlugin
    {
        private string[] MonitorObjects;
        public void Start()
        {
            Logger.Log(LogLevel.Debug, "[IOMozaicFree] Init Monitored GameObject Paths.");

            MonitorObjects = new string[]
                {
                    // Solo

                    "/PC00/PC0000/PC00_ute05_moza",             // Neko Cross-section Vagina
                    "/PC00/PC0000/PC00_ute05_moza_ANA",         // Neko Cross-section Anal
                    "/PC00/PC0000/PC_moza",                     // MC Penis
                    "/CH01/CH0001/CH01_moza",                   // Neko Vagina
                    "/CH01/CH0001/CH01_mozaAnal",               // Neko Anal
                    "/CH02/CH0002/CH01_moza",                   // Bunny Vagina
                    "/CH02/CH0002/CH01_mozaAnal",               // Bunny Anal

                    // Gangbang

                    "/MB0001/PC_moza",                          // Extra 1
                    "/MB0002/PC_moza",                          // Extra 2
                    "/MB0003/PC_moza",                          // Extra 3
                    "/BG01/BG0001/moza",                        // BG Extra
                    "/BG05/BG0005/moza",                        // BG Extra
                    "/SY01/SY0001/SY_moza",                     // Shota 1
                    "/SY02/SY0002/SY_moza",                     // Shota 2
                    "/SY03/SY0003/SY_moza"                      // Shota 3
                };

            SceneManager.activeSceneChanged += ChangedActiveScene;
        }

        public void Update()
        {
            // Executing this on every frame is overkill, but oh well
            decensor();
        }

        private void ChangedActiveScene(Scene current, Scene next)
        {
            //Logger.Log(LogLevel.Debug, "[IOMozaicFree] Changed Scene");
            //if(next != null)
            //    Logger.Log(LogLevel.Debug, "[IOMozaicFree] New Scene: " + next.name);
            //decensor();
        }

        private void decensor()
        {
            //Crashes if not executed on main thread.
            var customdelegate = new Action<object>(delegate (object param)
            {
                try
                {
                    foreach (string objectPath in MonitorObjects)
                    {
                        GameObject gameObject = GameObject.Find(objectPath);
                        if (gameObject != null && gameObject.activeSelf)
                        {
                            Logger.Log(LogLevel.Debug, "[IOMozaicFree] Disabling " + objectPath);
                            gameObject.SetActive(false);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(LogLevel.Error, ex.ToString());
                }
                Resources.UnloadUnusedAssets();
            });

            customdelegate.Invoke(null);
        }

    }
}
