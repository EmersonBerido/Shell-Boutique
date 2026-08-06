using UnityEngine;

[CreateAssetMenu(fileName = "DyeObject", menuName = "ScriptableObjects/InspectableObject", order = 1)]
public class DyeObject : ScriptableObject
{
    public Sprite sprite;
    public string dyeName;
    public Color color;
}
