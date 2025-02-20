using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Crafting Recipe", menuName = "Crafting/New Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public List<Item> requiredItens;
    public Item resultItem;
}
