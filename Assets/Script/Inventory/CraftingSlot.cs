using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSlot : MonoBehaviour
{
    public Item item;          // O item que ser� colocado no slot
    public Image itemIcon;     // A refer�ncia para o componente Image que vai exibir o sprite
    public Image background;   // A imagem de fundo (branca)

    private void Awake()
    {
        if (item != null)
        {
            SetItem(item);  // Configura o item no slot assim que a cena come�ar
        }
    }

    public void SetItem(Item newItem)
    {
        item = newItem;

        if (item != null && item.sprite != null)
        {
            // Aqui voc� define o sprite da imagem do slot
            itemIcon.sprite = item.sprite;
            itemIcon.enabled = true;  // Torna a imagem vis�vel

            // Ajusta a posi��o e o tamanho do item dentro do fundo branco
            FitItemInSlot();
        }
        else
        {
            Debug.LogWarning("Item ou sprite n�o atribu�do!");
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