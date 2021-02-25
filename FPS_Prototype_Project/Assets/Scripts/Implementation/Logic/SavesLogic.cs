using System.Collections.Generic;
using System.IO;
using FPSProject.Impl.Utils;
using UnityEngine;
using Zenject;

namespace FPSProject.Impl.Logic
{
    public class SavesLogic : ISaves
    {
        private const string SAVES_LOCAL_DIR = "Saves";
        
        private readonly string _savesFullPath = Path.Combine(Application.persistentDataPath, SAVES_LOCAL_DIR);
        
        private List<object> _savesList;

        public SavesLogic()
        {
            if (!File.Exists(_savesFullPath))
            {
                CreateSave();
            }
            
            InitSavesList();
        }
        
        public T GetSavesData<T>() where T : ISavesPart, new()
        {
            foreach (var saves in _savesList)
            {
                if (saves is T part)
                {
                    return part;
                }
            }

            var defaultSaves = new T();
            _savesList.Add(defaultSaves);
            return defaultSaves;
        }

        public void Save()
        {
            SavesSerializer serializer = new SavesSerializer(_savesFullPath);
            serializer.SerializeSaves(_savesList);
        }
        
        private void InitSavesList()
        {
            SavesSerializer serializer = new SavesSerializer(_savesFullPath);
            _savesList = serializer.DeserializeSaves();
        }

        private void CreateSave()
        {
            File.Create(_savesFullPath).Dispose();
        }
    }
}