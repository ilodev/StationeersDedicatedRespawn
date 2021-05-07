using System;
using Assets.Scripts;
using Assets.Scripts.Objects.Entities;
using Assets.Scripts.Objects.Items;
using BepInEx;
using HarmonyLib;
using UnityEngine.Networking;

/**
  Custom start/respawn conditions based on SteamID of the player

  Sample XML file: "stationeers dedicated server/dedicatedrespawn.xml" example

<?xml version="1.0" encoding="utf-8"?>
<Config xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <Items>
    <ConfigItem>
      <steamId>12345678</steamId>
      <StartCondition>Vulcan_Green</StartCondition>
      <RespawnCondition>Minimal</RespawnCondition>
    </ConfigItem>
    <ConfigItem>
      <steamId>87654321</steamId>
      <StartCondition>Vulcan</StartCondition>
      <RespawnCondition>Stationeer</RespawnCondition>
    </ConfigItem>
  </Items>
  <DefaultStartCondition>Normal</DefaultStartCondition>
  <DefaultRespawnCondition>Normal</DefaultRespawnCondition>
</Config>   


Note, missing entries in the config file wont be used, e.g. if you don't specify a default respawn condition
the plugin will not override the current value from the settings file.

ConfigItems are not necessary, define one for each SteamID you want to control the spawn/respawn conditions only,
the rest of the players joining will use Default* values.

*/
namespace DedicatedRespawn 
{
    [BepInPlugin("org.ilo.plugins.DedicatedRespawnPlugin", "Stationeers Dedicated respawn", "1.0.0.0")]
    public class DedicatedRespawnPlugin : BaseUnityPlugin
    {

        public Config PluginConfig;
        public static DedicatedRespawnPlugin Instance;

        // Awake is called once when both the game and the plug-in are loaded
        void Awake()
        {
            UnityEngine.Debug.Log("Dedicated Respawn Plugin is awake");
            Instance = this;

            PluginConfig = new Config();
            Config saved = PluginConfig.Load();
            if (saved == null)
            {
                // Need to create a custom config file.
                PluginConfig.Save();
            } else
            {
                PluginConfig = saved;
            }

            // Setup a hook to Log values when the server is started
            GameManager.OnGameStartedOnce += new GameManager.Event(OnGameStartedOnce);

            // Perform all the harmony injections
            try
            {
                var harmony = new Harmony("net.ilo.plugins.DedicatedRespawnPlugin");
                harmony.PatchAll();
                UnityEngine.Debug.Log("Patch succeeded");
            }
            catch (Exception e)
            {
                UnityEngine.Debug.Log(string.Format("Patch Failed: {0}", e.ToString()));                
            }

        }

        
        private void OnGameStartedOnce()
        {
            // Set start and spawn conditions to Minimal
            UnityEngine.Debug.Log("World About to start");
            WorldManager.SetStartCondition(PluginConfig.DefaultStartCondition, true);
            WorldManager.SetRespawnCondition(PluginConfig.DefaultRespawnCondition, true);
        }
    }

    // public static Human MoveBrainToNewHuman(
    //  Brain playerBrain,
    //  string SteamName,
    //  NetworkConnection conn,
    //  ulong steamId,
    //  CharacterIdentity identityArray)
    [HarmonyPatch(typeof(Human), "MoveBrainToNewHuman", new Type[] {
        typeof(Brain),
        typeof(String),
        typeof(NetworkConnection),
        typeof(ulong),
        typeof(CharacterIdentity) })
    ]
    class ConditionsMoveBrainToNewHuman
    {
        static void Prefix(
            Brain playerBrain,
            string SteamName,
            NetworkConnection conn,
            ulong steamId,
            CharacterIdentity identityArray
        )
        {
            // Set default values, since they could have been overriden earlier for the last joined player
            WorldManager.SetRespawnCondition(DedicatedRespawnPlugin.Instance.PluginConfig.DefaultRespawnCondition, false);
            WorldManager.SetStartCondition(DedicatedRespawnPlugin.Instance.PluginConfig.DefaultStartCondition, false);

            foreach (ConfigItem item in DedicatedRespawnPlugin.Instance.PluginConfig.Items)
            {
                if (item.steamId == steamId )
                {
                    WorldManager.SetRespawnCondition(item.RespawnCondition, false);
                    WorldManager.SetStartCondition(item.StartCondition, false);
                }
            }
            UnityEngine.Debug.Log(
                "Moving Brain for " + SteamName + "(" + steamId + ") IsRespawning: " + playerBrain.IsRespawning + " with: " + 
                (playerBrain.IsRespawning ? WorldManager.CurrentRespawnCondition.Key : WorldManager.CurrentStartCondition.Key)
            );
        }
    }
}
