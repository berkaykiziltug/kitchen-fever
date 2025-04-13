using UnityEngine;
using UnityEditor;
using System.IO;

public class AssetReferenceChecker
{
    [MenuItem("Tools/Find Missing References in Project Assets")]
    public static void FindMissingReferencesInAssets()
    {
        string[] allAssetGuids = AssetDatabase.FindAssets("t:Prefab t:ScriptableObject t:Material t:GameObject");
        int totalChecked = 0;
        int totalBroken = 0;

        foreach (string guid in allAssetGuids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            Object asset = AssetDatabase.LoadMainAssetAtPath(assetPath);
            if (asset == null)
                continue;

            SerializedObject so = new SerializedObject(asset);
            SerializedProperty sp = so.GetIterator();

            bool brokenFound = false;

            while (sp.NextVisible(true))
            {
                if (sp.propertyType == SerializedPropertyType.ObjectReference)
                {
                    if (sp.objectReferenceValue == null && sp.objectReferenceInstanceIDValue != 0)
                    {
                        if (!brokenFound)
                        {
                            Debug.LogWarning($"⛔ Broken reference in asset: {assetPath}", asset);
                            brokenFound = true;
                            totalBroken++;
                        }

                        Debug.LogWarning($"   → Missing reference in property: {sp.displayName}");
                    }
                }
            }

            totalChecked++;
        }

        Debug.Log($"🔍 Scan complete. Checked {totalChecked} assets. Found {totalBroken} with missing references.");
    }
}