using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    public static CraftingSystem Instance;

    private void Awake()
    {
        if(Instance == null) Instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    public void ShowCraftiingRecipe(CraftingRecipe recipe, CraftingUI craftingUI)
    {
        craftingUI.ShowIngredientes(recipe);
    }

    public bool TryCraft(CraftingRecipe recipe)
    {
        Inventory inventory = Inventory.Instance;

        foreach(Item item in recipe.requiredItens)
        {
            if(!inventory.hasItem(item))
            {
                Debug.Log("Falta itens para craftar!!");
                return false;
            }
        }

        foreach(Item item in recipe.requiredItens)
        {
            inventory.RemoveItem(item);
        }

        inventory.AddItem(recipe.resultItem);

        Debug.Log($"Crafting bem sucedido! Criou: {recipe.resultItem.name}");
        return true;
    }
}
