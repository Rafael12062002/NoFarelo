using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class CraftingSystem : MonoBehaviour
{
    public static CraftingSystem Instance;
    [SerializeField] private CraftingUI craftingUI;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    public void ShowCraftingRecipe(CraftingRecipe recipe)
    {
        craftingUI.ShowIngredientes(recipe);
    }

    public bool CanCraft(CraftingRecipe recipe)
    {
        foreach (var ingredient in recipe.requiredItens)
        {
            if (ingredient.item == null)
            {
                Debug.LogError($"Ingrediente {ingredient} está com item nulo!");
                return false;
            }

            int count = Inventory.Instance.GetItemCount(ingredient.item);
            if (count < ingredient.amount)
            {
                return false; // Se faltar algum item, não pode craftar
            }
        }
        return true;
    }

    // Método para realizar o craft
    public void Craft(CraftingRecipe recipe)
    {
        if (CanCraft(recipe))
        {
            Debug.Log("Itens suficientes para craftar.");
            // Remover os itens necessários do inventário
            foreach (var ingredient in recipe.requiredItens)
            {
                Inventory.Instance.RemoveItems("Galho", 4);
            }

            // Adicionar o item craftado ao inventário
            Inventory.Instance.AddItem(recipe.resultItem);
            AddCraftedItem(recipe.resultItem);

            Debug.Log($"Crafted: {recipe.resultItem.name}");
        }
        else
        {
            Debug.Log("Não há itens suficientes para craftar.");
        }
    }

    void AddCraftedItem(Item craftedItem)
    {
        Inventory.Instance.items.Add(craftedItem);
        Debug.Log("Item craftado adicionado: " + craftedItem.name);
    }
}
