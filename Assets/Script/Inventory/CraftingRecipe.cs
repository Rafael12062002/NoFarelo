using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Crafting Recipe", menuName = "Crafting/New Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public List<CraftingIngredient> requiredItens;
    public Item resultItem;
}
