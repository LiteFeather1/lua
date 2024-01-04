#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace LTF.Utils
{
    public static class LTFHelpersEditorOnly
    {
        /// <summary>
        /// Find all scriptableobjects in the project of type <typeparamref name="T"/>
        /// </summary>
        /// <returns></returns>
        public static T[] GetScriptableObjects<T>() where T : ScriptableObject
        {
            string[] guid = AssetDatabase.FindAssets($"t:{typeof(T).Name}");
            T[] scriptables = new T[guid.Length];
            for (int i = 0; i < guid.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid[i]);
                scriptables[i] = (T)AssetDatabase.LoadAssetAtPath(path, typeof(ScriptableObject));
            }

            return scriptables;
        }
    }
}
#endif
