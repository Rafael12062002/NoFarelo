using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum SlotTag {None, Head, Chest, Legs, Feet}

[CreateAssetMenu(menuName = "No farelo/item")]
public class Item : ScriptableObject
{
    public int id;
    public Sprite sprite;
    public SlotTag itemTag;
    public GameObject prefab;
    public int quantity;

    [Header("Ingredientes_necessarios")]
    public List<Item> requiredItens;

    private void OnEnable()
    {
        // Gera um ID único para o item ao ser instanciado
        if (id == 0)
        {
            id = System.Guid.NewGuid().GetHashCode();
        }
    }
}