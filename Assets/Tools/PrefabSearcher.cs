using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class PrefabSearcher : EditorWindow
{
    private string searchName = ""; // 搜索的子物体名称
    private List<string> foundPrefabs = new List<string>();

    [MenuItem("Tools/Prefab Searcher")]
    public static void ShowWindow()
    {
        GetWindow<PrefabSearcher>("Prefab Searcher");
    }

    private void OnGUI()
    {
        GUILayout.Label("Prefab Searcher", EditorStyles.boldLabel);
        searchName = EditorGUILayout.TextField("Search Child Name:", searchName);

        if (GUILayout.Button("Search"))
        {
            SearchPrefabs();
        }

        if (foundPrefabs.Count > 0)
        {
            GUILayout.Label("Found Prefabs:");
            foreach (string prefabPath in foundPrefabs)
            {
                if (GUILayout.Button(prefabPath))
                {
                    Selection.activeObject = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                }
            }
        }
    }

    private void SearchPrefabs()
    {
        foundPrefabs.Clear();
        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets" });

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (prefab != null && HasChildWithName(prefab.transform, searchName))
            {
                foundPrefabs.Add(path);
            }
        }
    }

    private bool HasChildWithName(Transform parent, string name)
    {
        // 检查物体本身的名字
        if (parent.name.Contains(name))
        {
            return true;
        }

        // 递归检查子物体
        foreach (Transform child in parent)
        {
            if (HasChildWithName(child, name)) // 递归查找子物体
            {
                return true;
            }
        }
        return false;
    }

}
