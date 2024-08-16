using SimpleJSON;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace ChroMapper_SongDataChanger.Configuration
{
    public class Options
    {
        private static Options instance;
        public static readonly string settingJsonFile = Application.persistentDataPath + "/ChroMapper-SongDataChanger.json";

        public float menuUIAnchoredPosX = -50;
        public float menuUIAnchoredPosY = -75;
        public string dragEnableBinding = "<Keyboard>/shift";
        public string batachFilePath = "";
        public float batchStartTimeout = 10;
        public float batchRunTimeout = 600;
        public string batchUITitle = "Batch";
        public string batchExtension = "bat";

        public static Options Instance
        {
            get
            {
                if (instance is null)
                {
                    instance = SettingLoad();
                    instance.SettingSave();
                }
                return instance;
            }
        }

        public static Options SettingLoad()
        {
            var options = new Options();
            if (!File.Exists(settingJsonFile))
                return options;
            var members = options.GetType().GetMembers(BindingFlags.Instance | BindingFlags.Public);
            using (var jsonReader = new StreamReader(settingJsonFile))
            {
                var optionsNode = JSON.Parse(jsonReader.ReadToEnd());
                foreach (var member in members)
                {
                    try
                    {
                        if (!(member is FieldInfo field))
                            continue;
                        var optionValue = optionsNode[field.Name];
                        if (optionValue != null)
                            field.SetValue(options, Convert.ChangeType(optionValue.Value, field.FieldType));
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"Optiong {member.Name} member to load ERROR!.\n{e}");
                        options = new Options();
                    }
                }
            }
            return options;
        }

        //オプションの保存[値変更時に自動保存しないので、必要な時に呼ぶ]
        public void SettingSave()
        {
            var optionsNode = new JSONObject();
            var members = this.GetType().GetMembers(BindingFlags.Instance | BindingFlags.Public);
            foreach (var member in members)
            {
                if (!(member is FieldInfo field))
                    continue;
                optionsNode[field.Name] = field.GetValue(this).ToString();
            }
            using (var jsonWriter = new StreamWriter(settingJsonFile, false))
                jsonWriter.Write(optionsNode.ToString(2));
        }
    }
}
