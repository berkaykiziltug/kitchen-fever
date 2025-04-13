using UnityEngine;
using UnityEditor;

public class BrokenPrefabFinder
{
    [MenuItem("Tools/Find Broken Prefab Instances")]
    static void FindBrokenPrefabs()
    {
        GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject go in allGameObjects)
        {
            if (PrefabUtility.IsAnyPrefabInstanceRoot(go))
            {
                GameObject prefab = PrefabUtility.GetCorrespondingObjectFromSource(go);
                if (prefab == null)
                {
                    Debug.LogWarning("Broken prefab instance found: " + go.name, go);
                }
            }
        }
        Debug.Log("Prefab check complete.");
    }
}