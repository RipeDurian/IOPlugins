using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IllusionPlugin;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace IOMozaicFreeIPA
{
    public class IOMozaicFree : IPlugin
    {
        public string Name
        {
            get
            {
                return "IOMozaicFreeIPA";
            }
        }

        public string Version
        {
            get
            {
                return "0.1";
            }
        }

        private string[] MonitorObjects;
        public void OnApplicationStart()
        {
            /*
                Bruteforcing through and disabling all gameobjects with "moza" in their name 
                might cause problems, so this is a more controlled approach
            */

            debugmsg("Init GameObject Paths");

            MonitorObjects = new string[]
                {
                    // Solo

                    "/PC00/PC0000/PC00_ute05_moza",             // Cross-section Vagina
                    "/PC00/PC0000/PC00_ute05_moza_ANA",         // Cross-section Anal
                    "/PC00/PC0000/PC_moza",                     // MC Penis
                    "/PC00/PC0000/PC00_mozaAnal",               // MC Anal
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
        }

        public void OnApplicationQuit()
        {
            //throw new NotImplementedException();
        }

        public void OnLevelWasLoaded(int level)
        {
            //throw new NotImplementedException();
        }

        public void OnLevelWasInitilaised()
        {
            //throw new NotImplementedException();
        }

        public void OnLevelWasInitialized(int level)
        {
            //throw new NotImplementedException();
        }

        public void OnUpdate()
        {
            // yeah overkill
            uncensor();
        }

        public void OnFixedUpdate()
        {
            //throw new NotImplementedException();
        }

        private void uncensor()
        {
            // Crashes if not executed on main thread

            var customdelegate = new Action<object>(delegate (object param)
            {
                try
                {
                    foreach (string objectPath in MonitorObjects)
                    {
                        GameObject gameObject = GameObject.Find(objectPath);
                        if (gameObject != null && gameObject.activeSelf)
                        {
                            debugmsg("Disabling " + objectPath);
                            gameObject.SetActive(false);
                        }
                    }
                }
                catch (Exception ex)
                {
                    debugmsg(ex.ToString());
                }
            });

            customdelegate.Invoke(null);
        }

        private void debugmsg(string text)
        {
            Console.WriteLine("[IOMozaicFree] " + text);
        }
    }
}
