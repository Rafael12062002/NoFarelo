using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftingSlot : MonoBehaviour, IPointerClickHandler
{
    public Item item;          // O item que será colocado no slot
    public Image itemIcon;     // A referência para o componente Image que vai exibir o sprite
    public Image background;   // A imagem de fundo (branca)

    public CraftingRequirementItens crafitingIntens;
    public CraftingUI craftingUI;

    private void Awake()
    {
        if (item != null)
        {
            SetItem(item);  // Configura o item no slot assim que a cena começar
        }
    }

    public void SetItem(Item newItem)
    {
        item = newItem;

        if (item != null && item.sprite != null)
        {
            itemIcon.sprite = item.sprite;
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
        if (item == null)
        {
            Debug.Log("Item vazio");
            return;
        }

        if (crafitingIntens == null)
        {
            Debug.Log("craftingItens vazio");
            return;
        }

        if(item.requiredItens ==  null)
        {
            Debug.Log("requiredItens vazio");
            return;
        }

        if(item != null && item.requiredItens.Count > 0)
        {
            crafitingIntens.ShowRequirementItens(item.requiredItens);
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