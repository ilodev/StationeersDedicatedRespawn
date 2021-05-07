using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts;
using UnityEngine;

namespace DedicatedRespawn
{
    public class Config
    {
        public List<ConfigItem> Items = new List<ConfigItem>();
        public String DefaultStartCondition = null;
        public String DefaultRespawnCondition = null;

        public Config()
        {

        }

        public Config(string DefaultStartCondition = null, string DefaultRespawnCondition = null)
        {
            this.DefaultStartCondition = DefaultStartCondition;
            this.DefaultRespawnCondition = DefaultRespawnCondition;
        }

        public String findSettingsFile()
        {
            string settingsFile = Application.streamingAssetsPath + "/../../DedicatedRespawn.xml";
            return settingsFile;
        }
        public void Save()
        {
            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(this.GetType());
            TextWriter writer = new StreamWriter(findSettingsFile());
            x.Serialize(writer, this);
            writer.Close();
        }

        public Config Load()
        {
            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(this.GetType());
            try
            {
                TextReader reader = new StreamReader(findSettingsFile());
                Config filedata;
                filedata = (Config)x.Deserialize(reader);
                reader.Close();
                return filedata;
            } catch(Exception e)
            {
                UnityEngine.Debug.Log(string.Format("Exception loading custom respawn config: {0}", e.ToString()));
            }
            return null;
        }

    }

    public class ConfigItem
    {
        public ulong steamId;
        public string StartCondition;
        public string RespawnCondition;

        public ConfigItem()
        {
            this.steamId = 0;
            this.StartCondition = null;
            this.RespawnCondition = null;
        }

        public ConfigItem(ulong steamId, String StartCondition, String RespawnCondition)
        {
            this.steamId = steamId;
            this.StartCondition = StartCondition;
            this.RespawnCondition = RespawnCondition;
        }
    }


}
