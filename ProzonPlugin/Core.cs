using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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
        public override void Init(ITorchBase torch)
        {
            base.Init(torch);

            SetupConfig();

            string destinationModId = config.ModId.ToString();

            // Assuming the mods are located in the Space Engineers mods directory
            // Define the full paths to the prefab folders of both mods
            string sourcePrefabFolder = config.FolderPath;

            var baseModPath = StoragePath + $"//Content//244850//{config.ModId}//Data//Prefabs";
            // Ensure the destination folder exists, create it if necessary
   
            // Copy all files from the source prefab folder to the destination prefab folder
            try
            {
                foreach (var file in Directory.GetFiles(sourcePrefabFolder, "*.sbc"))  // Assumes .sbc prefab files
                {
                    string fileName = Path.GetFileName(file);
                    string destinationFile = Path.Combine(baseModPath, fileName);

                    // Copy the file, overwrite if exists
                    File.Copy(file, destinationFile, true);
                    Console.WriteLine($"Copied {fileName} to {baseModPath}");
                }

                Console.WriteLine("All prefab files copied successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

        }

        public override void Update()
        {
            //here you can do stuff in the games update, this will run once every 256 ticks so, lower is more frequent, higher is less frequent
            ticks++;
            if (ticks % 256 == 0 && TorchState == TorchSessionState.Loaded)
            {

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

