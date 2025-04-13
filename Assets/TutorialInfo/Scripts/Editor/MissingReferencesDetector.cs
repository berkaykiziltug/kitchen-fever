using UnityEngine;
using UnityEditor;

public class MissingReferencesDetector
{
    [MenuItem("Tools/Find Missing References (All Components)")]
    public static void FindMissingReferences()
    {
        GameObject[] allGameObjects = Object.FindObjectsOfType<GameObject>();

        foreach (GameObject go in allGameObjects)
        {
            Component[] components = go.GetComponents<Component>();

            foreach (Component component in components)
            {
                if (component == null)
                {
                    Debug.LogError($"Missing component in GameObject: {go.name}", go);
                    continue;
                }

                SerializedObject so = new SerializedObject(component);
                SerializedProperty sp = so.GetIterator();

                while (sp.NextVisible(true))
                {
                    if (sp.propertyType == SerializedPropertyType.ObjectReference)
                    {
                        if (sp.objectReferenceValue == null && sp.objectReferenceInstanceIDValue != 0)
                        {
                            Debug.LogWarning($"Missing reference in GameObject: {go.name} | Component: {component.GetType().Name} | Property: {sp.displayName}", go);
                        }
                    }
                }
            }
        }

        Debug.Log("Missing reference scan complete.");
    }
}