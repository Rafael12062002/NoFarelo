using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Debug.Log("Verificando se é possível craftar...");

        if(CanCraft(recipe))
        {
            Debug.Log("pode");
        }
        else
        {
            Debug.Log("não pode");
        }
        Debug.Log("Iniciando Craft");
        if (CanCraft(recipe))
        {
            Debug.Log("Itens suficientes para craftar.");
            // Remover os itens necessários do inventário
            foreach (var ingredient in recipe.requiredItens)
            {
                Debug.Log($"Removendo {ingredient.amount} de {ingredient.item.name}.");
                Inventory.Instance.RemoveItems(ingredient.item.id, ingredient.amount);
                Debug.Log("Passou aqui");
            }

            if(recipe !=  null && recipe.resultItem != null)
            {
                // Adicionar o item craftado ao inventário
                Inventory.Instance.PickUpItem(recipe.resultItem);
                Debug.Log($"Crafted: {recipe.resultItem.name}");
                AddCraftedItem(recipe.resultItem);
            }
            else
            {
                Debug.LogError("Recipe ou resultItem estão nulos.");
            }
        }
        else
        {
            Debug.Log("Não há itens suficientes para craftar.");
        }
    }

    void AddCraftedItem(Item craftedItem)
    {
        if(Inventory.Instance != null)
        {
            if(craftedItem != null)
            {
                //Inventory.Instance.items.Add(craftedItem);
                //Debug.Log("Item craftado adicionado: " + craftedItem.name);
            }  
            else
            {
                Debug.LogWarning("Tentando adicionar um item nulo!");
            }
        }
        else
        {
            Debug.LogError("A instância de Inventory não foi inicializada.");
        }
    }
}
