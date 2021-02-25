using System;
using UnityEngine;

namespace FPSProject.Impl.Configs.Extensions
{
    [Serializable]
    public class PoolableObjectsDictionary : SerializableDictionary<MonoBehaviour, int> {}
}