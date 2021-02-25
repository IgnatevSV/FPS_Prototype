using System.Linq;
using UnityEngine;

namespace FPSProject.Impl.Configs.Extensions
{
    public static class ConfigExtensions
    {
        public static T GetConfigDataById<T>(this IdToDataDictionary<T> dictionary, int id) where T : IConfigData
        {
            if (dictionary != null && dictionary.Any() && dictionary.ContainsKey(id))
            {
                return dictionary[id];
            }

            Debug.LogWarning($"ConfigData with id '{id}' not found!");
            
            return default;
        }
    }
}