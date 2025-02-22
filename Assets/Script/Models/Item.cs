using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "No farelo/item")]
public class Item : ScriptableObject
{
    public int id;
    public string name;
    public Sprite sprite;
    //public SlotTag itemTag;
    public GameObject prefab;
    public int quantity = 0;
    public InventoryItem inventoryItem;

    [Header("Ingredientes Necessários")]
    public List<Item> requiredItens;

    // Remove o construtor e ajusta para uso com ScriptableObject
    public static Item CreateItem(int id, string name, int quantity, Sprite sprite, GameObject prefab)
    {
        Item item = ScriptableObject.CreateInstance<Item>();
        item.id = id;
        item.name = name;
        item.quantity = quantity;
        item.sprite = sprite;
        item.prefab = prefab;
        return item;
    }

    private void OnEnable()
    {
        if (id == 0)
        {
            id = System.Guid.NewGuid().GetHashCode();
        }
    }

    public override string ToString()
    {
        return $"{name} (ID: {id}, Quantidade: {quantity})";
    }
}
