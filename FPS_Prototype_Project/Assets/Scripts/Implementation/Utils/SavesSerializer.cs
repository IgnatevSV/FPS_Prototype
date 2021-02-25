using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace FPSProject.Impl.Utils
{
    public class SavesSerializer
    {
        private string _activeSavePath;

        public SavesSerializer(string activeSavePath)
        {
            _activeSavePath = activeSavePath;
        }

        public void SerializeSaves(List<object> savesList)
        {
            BinaryFormatter serializer = new BinaryFormatter();
            try
            {
                if (File.Exists(_activeSavePath))
                {
                    FileStream sw = new FileStream(_activeSavePath, FileMode.OpenOrCreate);
                    serializer.Serialize(sw, savesList);
                    sw.Close();
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public List<object> DeserializeSaves()
        {
            List<object> saves = new List<object>();

            if (IsValidSavesFile())
            {
                FileStream settingsStream = File.OpenRead(_activeSavePath);
                Debug.LogFormat("Load from save: {0}", _activeSavePath);

                BinaryFormatter serializer = new BinaryFormatter();
                saves = (List<object>) serializer.Deserialize(settingsStream);
                settingsStream.Close();
            }

            return saves;
        }

        private bool IsValidSavesFile()
        {
            try
            {
                if (new FileInfo(_activeSavePath).Length != 0)
                {
                    FileStream fs = File.OpenRead(_activeSavePath);
                    BinaryReader reader = new BinaryReader(fs);
                    reader.Read();
                    fs.Close();
                    return true;
                }
                return false;
            }
            catch (Exception exception)
            {
                Debug.LogErrorFormat("Read save exeption: {0}", exception.Message);
            }

            return false;
        }
        
    }
}