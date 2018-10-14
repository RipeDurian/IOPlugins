using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using IllusionPlugin;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IOTextureReplacer
{
    public class IOTextureReplacer : IPlugin
    {
        public string imagesPath { get; set; }
        public string imagesDumpPath { get; set; }
        private Dictionary<string, string> images;

        public string Name
        {
            get
            {
                return "IOTextureReplacerIPA";
            }
        }

        public string Version
        {
            get
            {
                return "0.1";
            }
        }

        public void OnApplicationQuit()
        {
            throw new NotImplementedException();
        }

        public void OnApplicationStart()
        {
            SceneManager.sceneLoaded += OnLevelFinishedLoading;

            imagesPath = @"Plugins/images";
            imagesDumpPath = @"Plugins/dump/images";
            getImages();
            replaceImages();
        }

        public void OnFixedUpdate()
        {
            //throw new NotImplementedException();
        }

        public void OnLevelWasInitialized(int level)
        {
            //throw new NotImplementedException();
        }

        public void OnLevelWasLoaded(int level)
        {
            //throw new NotImplementedException();
        }

        public void OnUpdate()
        {
            if (UnityEngine.Event.current.type == EventType.KeyUp)//Event.current.isKey && Event.current.type)
            {
                if (UnityEngine.Event.current.keyCode == KeyCode.F11 && UnityEngine.Event.current.alt)
                {
                    debugmsg("Dumping Images..." + Environment.NewLine);
                    try
                    {
                        DumpImagesFromScene();
                    }
                    catch (Exception ex)
                    {
                        debugmsg("[Error] " + ex.ToString());
                    }
                }
            }
        }

        public void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            debugmsg("New Scene: " + scene.name);
            getImages();
            replaceImages();
        }

        private void getImages()
        {
            try
            {
                debugmsg("Fetching Images...");
                images = new Dictionary<string, string>();

                foreach (string file in Directory.GetFiles(imagesPath))
                {
                    if (Path.GetExtension(file).Equals(".png", StringComparison.OrdinalIgnoreCase))
                    {
                        images.Add(Path.GetFileNameWithoutExtension(file), file);
                    }
                }
            }
            catch (Exception ex)
            {
                debugmsg("[Error] " + ex.ToString());
            }
        }

        private void replaceImages()
        {
            UnityEngine.Object[] textures = Resources.FindObjectsOfTypeAll(typeof(Texture2D));

            foreach (UnityEngine.Object t in textures)
            {
                try
                {
                    Texture2D tex = (Texture2D)t;
                    if (images.ContainsKey(t.name))
                    {
                        debugmsg("Replacing Image: " + tex.name);
                        byte[] fileData = File.ReadAllBytes(images[t.name]);
                        tex.LoadImage(fileData);

                    }
                }
                catch (Exception ex)
                {

                    debugmsg("[Error] " + ex.ToString());
                }
            }

            Resources.UnloadUnusedAssets();
        }

        private void DumpImagesFromScene()
        {
            string dumppath = Path.Combine(imagesDumpPath, SceneManager.GetActiveScene().name);

            if (!Directory.Exists(dumppath))
            {
                Directory.CreateDirectory(dumppath);
            }

            UnityEngine.Object[] textures = Resources.FindObjectsOfTypeAll(typeof(Texture2D));

            foreach (UnityEngine.Object t in textures)
            {
                try
                {
                    Texture2D tex = (Texture2D)t;
                    Texture2D tex_readable = getReadableTexture2D(tex);
                    byte[] bytes = tex_readable.EncodeToPNG();
                    File.WriteAllBytes(Path.Combine(dumppath, t.name + ".png"), bytes);
                    debugmsg("Dumping " + t.name);
                }
                catch (Exception ex)
                {
                    debugmsg("[Error]" + ex.ToString());
                }
            }

            debugmsg("Dumppath: " + dumppath);
        }

        public static Texture2D getReadableTexture2D(Texture2D texture)
        {
            RenderTexture tmp = RenderTexture.GetTemporary(
                                texture.width,
                                texture.height,
                                0,
                                RenderTextureFormat.Default,
                                RenderTextureReadWrite.Linear);

            Graphics.Blit(texture, tmp);
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = tmp;
            Texture2D myTexture2D = new Texture2D(texture.width, texture.height);
            myTexture2D.ReadPixels(new Rect(0, 0, tmp.width, tmp.height), 0, 0);
            myTexture2D.Apply();
            RenderTexture.active = previous;

            RenderTexture.ReleaseTemporary(tmp);

            return myTexture2D;
        }

        private void debugmsg(string text)
        {
            Console.WriteLine("[IOTextureReplacer] " + text);
        }

    }
}
