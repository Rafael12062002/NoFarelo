using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingRequirementItens : MonoBehaviour
{
    public GameObject requirementPrefab;
    public Transform requirementListParent;
    List<GameObject> itemsToDestroy = new List<GameObject>();

    public void ShowRequirementItens(List<Item> ingrdientes)
    {
        if (requirementPrefab != null && requirementPrefab.GetComponent<RectTransform>() != null)
        {
            // Execute seu c�digo aqui
        }
        else
        {
            Debug.LogWarning("Tentando acessar um objeto destru�do ou inv�lido.");
        }

        foreach (Transform child in requirementListParent)
        {
            // Adiciona o objeto � lista
            itemsToDestroy.Add(child.gameObject);
        }

        // Agora destrua todos os itens de uma vez
        foreach (var item in itemsToDestroy)
        {
            Destroy(item);
        }

        foreach (Item requiredItem in ingrdientes)
        {
            GameObject slot = Instantiate(requirementPrefab, requirementListParent);
            slot.transform.Find("itemIcon").GetComponent<Image>().sprite = requiredItem.sprite;
        }
    }
}
