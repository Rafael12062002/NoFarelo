using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUI : MonoBehaviour
{
    public Transform craftingGrid;
    public Transform ingredientsGrid;
    public GameObject slotPrefab;

    public void ShowCraftableItems(List<CraftingRecipe> recipes)
    {
        foreach(Transform child in craftingGrid) Destroy(child.gameObject);

        foreach(var recipe in recipes)
        {
            GameObject slot = Instantiate(slotPrefab, craftingGrid);
            slot.GetComponent<CraftingSlot>().SetItem(recipe.resultItem);
            slot.GetComponent<Button>().onClick.AddListener(() => ShowIngredientes(recipe));
        }
    }

    public void ShowIngredientes(CraftingRecipe recipe)
    {
        foreach(Transform child in ingredientsGrid) Destroy(child.gameObject);

        foreach(Item item in recipe.requiredItens)
        {
            GameObject slot = Instantiate(slotPrefab, ingredientsGrid);
            slot.GetComponent<CraftingSlot>().SetItem(item);
        }
    }
}
