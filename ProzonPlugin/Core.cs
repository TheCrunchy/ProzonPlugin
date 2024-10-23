using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NLog;
using Torch;
using Torch.API;
using Torch.API.Managers;
using Torch.API.Plugins;
using Torch.API.Session;
using Torch.Managers;
using Torch.Managers.PatchManager;
using Torch.Session;

namespace ProzonPlugin
{
    public class Core : TorchPluginBase
    {
        public static Config config;
        public int ticks;
        public TorchSessionState TorchState;
        public static Logger Log = LogManager.GetLogger("ProzonFileMover");
        public override void Init(ITorchBase torch)
        {
            base.Init(torch);

            SetupConfig();
            string sourcePrefabFolder = config.FolderPath;
   
            var baseModPath = StoragePath + $"//Content//244850//{config.ModId}//Data//Prefabs";
            Core.Log.Error($"{sourcePrefabFolder} - {baseModPath}");
            foreach (var file in Directory.GetFiles(baseModPath, "*.sbc", SearchOption.AllDirectories))
            {
                File.Delete(file);
            }

            // Assumes .sbc prefab files

            try
            {
                
                foreach (var file in Directory.GetFiles(sourcePrefabFolder, "*.sbc", SearchOption.AllDirectories))  // Assumes .sbc prefab files
                {
                    string fileName = Path.GetFileName(file);
                    string destinationFile = Path.Combine(baseModPath, fileName);

                    // Copy the file, overwrite if exists
                    File.Copy(file, destinationFile, true);
                    Log.Info($"Copied {fileName} to {baseModPath}");
                }


            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

        }

        private void SetupConfig()
        {
            FileUtils utils = new FileUtils();
            var path = StoragePath + "/ProzonFileMover.xml";
            if (File.Exists(path))
            {
                config = utils.ReadFromXmlFile<Config>(path);
                utils.WriteToXmlFile<Config>(path, config, false);
            }
            else
            {
                config = new Config();
                utils.WriteToXmlFile<Config>(path, config, false);
            }

        }
    }
}

