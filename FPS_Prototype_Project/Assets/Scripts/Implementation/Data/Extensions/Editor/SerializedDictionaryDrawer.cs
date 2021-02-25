using UnityEditor;

namespace FPSProject.Impl.Configs.Extensions
{
    [CustomPropertyDrawer(typeof(WeaponsDictionary))]
    [CustomPropertyDrawer(typeof(BulletDictionary))]
    [CustomPropertyDrawer(typeof(ScoreDataDictionary))]
    [CustomPropertyDrawer(typeof(DestroyableObjectsDictionary))]
    [CustomPropertyDrawer(typeof(PoolableObjectsDictionary))]
    public class SerializedDictionaryDrawer : SerializableDictionaryPropertyDrawer {}
}