using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Transform recipeContainer; // Onde as receitas ser�o exibidas
    [SerializeField] private GameObject recipeButtonPrefab; // Prefab do bot�o de receita
    [SerializeField] private Transform ingredientsContainer; // Onde os ingredientes ser�o exibidos
    [SerializeField] private GameObject ingredientButtonPrefab; // Prefab de bot�o para os ingredientes (se necess�rio)
    [SerializeField] private Button craftButton; // Bot�o de craft

    public void ShowCraftingRecipes(List<CraftingRecipe> recipes)
    {
        // Limpar qualquer receita anterior da UI
        foreach (Transform child in recipeContainer)
        {
            Destroy(child.gameObject);
        }

        // Criar um bot�o para cada receita
        foreach (var recipe in recipes)
        {
            
            GameObject button = Instantiate(recipeButtonPrefab, recipeContainer);
            button.GetComponentInChildren<Text>().text = recipe.resultItem.name;

            var currentRecipe = recipe;
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                ShowIngredientes(currentRecipe); // Exibe os ingredientes da receita
            });
        }
        Debug.Log("Mostrando todos os itens" + recipes);
    }

    public void ShowIngredientes(CraftingRecipe recipe)
    {
        // Limpar a UI de ingredientes
        foreach (Transform child in ingredientsContainer)
        {
            Destroy(child.gameObject);
        }

        // Adicionar os ingredientes na UI
        foreach (var item in recipe.requiredItens)
        {
            GameObject ingredientUI = Instantiate(ingredientButtonPrefab, ingredientsContainer);
            // Se voc� quiser mostrar um �cone ou outra coisa, aqui seria onde voc� faz isso
            //ingredientUI.GetComponentInChildren<Text>().text = $"{item.id} x{item.amount}"; // Exibe o nome e a quantidade do item
        }

        // Exibir o bot�o de craft
        craftButton.gameObject.SetActive(true); // Ativa o bot�o de craft
        craftButton.onClick.RemoveAllListeners(); // Remove ouvintes antigos

        // Verifica se � poss�vel craftar e adiciona a fun��o de craft
        craftButton.onClick.AddListener(() => CraftItem(recipe));

        // Desabilita o bot�o se n�o for poss�vel craftar
        craftButton.interactable = CanCraft(recipe);
    }
    private bool CanCraft(CraftingRecipe recipe)
    {
        foreach (var ingredient in recipe.requiredItens)
        {
            if (ingredient.item == null)
            {
                Debug.LogError($"Ingrediente {ingredient} est� com item nulo!");
                return false;
            }
            int count = Inventory.Instance.GetItemCount(ingredient.item); // Verifique a quantidade do item no invent�rio
            if (count < ingredient.amount)
            {
                return false; // Se faltar algum item, n�o pode craftar
            }
        }
        return true;
    }

    public void CraftItem(CraftingRecipe recipe)
    {
        if(CraftingSlot.craftingRecipe != null)
        {
            CraftingSystem.Instance.Craft(CraftingSlot.craftingRecipe);
        }
        else
        {
            Debug.Log("Nenhuma receita selecionada");
        }
    }

}
