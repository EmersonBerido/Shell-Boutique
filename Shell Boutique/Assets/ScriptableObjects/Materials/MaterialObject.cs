using UnityEngine;

[CreateAssetMenu(fileName = "MaterialObject", menuName = "ScriptableObjects/InspectableObject", order = 1)]
public class MaterialObject : ScriptableObject
{
    public Sprite sprite;
    public string material;
}
