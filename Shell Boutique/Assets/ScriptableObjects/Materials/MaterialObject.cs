using UnityEngine;

[CreateAssetMenu(fileName = "MaterialObject", menuName = "ScriptableObjects/MaterialObject", order = 1)]
public class MaterialObject : ScriptableObject
{
    public Sprite sprite;
    public string material;
}
