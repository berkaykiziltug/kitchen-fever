using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu()]
public class RecipeSO : ScriptableObject
{
    [FormerlySerializedAs("kitchenObjectsSoList")] [FormerlySerializedAs("KitchenObjectsSOList")] public List<KitchenObjectSO> kitchenObjectsSOList;
    public string recipeName;
}
