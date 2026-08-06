using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class MaterialAmount
{
    public MaterialObject material;
    public int amount;
}

[CreateAssetMenu(fileName = "ShellObject", menuName = "ScriptableObjects/ShellObject", order = 1)]
public class ShellObject : ScriptableObject
{
    public List<MaterialAmount> recipe;
    public GameObject prefab;
}
