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
    public Sprite sprite;
    public Color currColor; // this will not be the requested color

    public override bool Equals(object obj)
    {
        if (obj == null || obj.GetType() != typeof(ShellObject))
            return false;

        ShellObject shellObj = (ShellObject)obj;

        if (ReferenceEquals(recipe, shellObj.recipe))
            return true;

        if (recipe == null || shellObj.recipe == null || recipe.Count != shellObj.recipe.Count)
            return false;

        for (int i = 0; i < recipe.Count; i++)
        {
            if (recipe[i].material != shellObj.recipe[i].material ||
                recipe[i].amount != shellObj.recipe[i].amount)
                return false;
        }

        return true;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;

            if (recipe != null)
            {
                foreach (MaterialAmount ingredient in recipe)
                {
                    hash = hash * 31 + (ingredient == null || ingredient.material == null
                        ? 0
                        : ingredient.material.GetHashCode());
                    hash = hash * 31 + (ingredient == null ? 0 : ingredient.amount);
                }
            }

            return hash;
        }
    }
}
