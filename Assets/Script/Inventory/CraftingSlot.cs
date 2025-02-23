using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftingSlot : MonoBehaviour, IPointerClickHandler
{
    public static CraftingRecipe craftingRecipe;
    public CraftingRecipe item;          // O item que será colocado no slot
    public Image itemIcon;     // A referência para o componente Image que vai exibir o sprite
    public Image background;   // A imagem de fundo (branca)

    public CraftingRequirementItens crafitingIntens;
    public CraftingUI craftingUI;

    private void Awake()
    {
        if (item != null)
        {
            SetItem(item.resultItem);  // Configura o item no slot assim que a cena começar
        }
    }

    public void SetItem(Item newItem)
    {
        item.resultItem = newItem;

        if (item.resultItem != null && item.resultItem.sprite != null)
        {
            itemIcon.sprite = item.resultItem.sprite;
            itemIcon.enabled = true;
            FitItemInSlot();
        }
        else
        {
            Debug.LogWarning("Item ou sprite não atribuído!");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Verifica se o item é nulo
        if (item == null)
        {
            Debug.Log("Item vazio");
            return;
        }

        // Verifica se o resultItem de CraftingRecipe é nulo
        if (item.resultItem == null)
        {
            Debug.Log("resultItem vazio");
            return;
        }

        // Verifica se o CraftingRequirementItens está atribuído corretamente
        if (crafitingIntens == null)
        {
            Debug.Log("CraftingRequirementItens vazio");
            return;
        }


        if (item.resultItem != null && item.resultItem.requiredItens.Count > 0)
        {
            craftingRecipe = item;
            Debug.Log("Apertei no Item: " + item.resultItem + " Itens necessarios para craftar " + item.resultItem.requiredItens.Count);
            crafitingIntens.ShowRequirementItens(item.resultItem.requiredItens);
            
        }
    }

    private void FitItemInSlot()
    {
        // Ajusta o tamanho da imagem do item para caber dentro do fundo
        RectTransform itemRect = itemIcon.GetComponent<RectTransform>();
        RectTransform backgroundRect = background.GetComponent<RectTransform>();

        // Faz o item se ajustar ao tamanho do fundo
        itemRect.sizeDelta = new Vector2(backgroundRect.rect.width * 0.8f, backgroundRect.rect.height * 0.8f);

        // Centraliza o item no fundo
        itemRect.anchoredPosition = Vector2.zero;
    }

    public void ClearSlot()
    {
        item = null;
        itemIcon.enabled = false;  // Esconde a imagem quando o slot estiver vazio
    }
}