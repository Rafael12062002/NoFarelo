using System.Collections.Generic;
using System.Linq;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    public static InventoryItem carriedItem;
    public GameObject panelCrafting;
    Movimentacao player;
    [SerializeField] IventorySlot[] inventorySlot;
    [SerializeField] IventorySlot[] equipamentoSlot;

    [SerializeField] Transform draggablesTransform;
    [SerializeField] InventoryItem itemPrefab;

    [SerializeField] List<Item> items = new List<Item>();

    [SerializeField] Button giveItemBtn;

    private void Awake()
    {
        Instance = this;
        //DontDestroyOnLoad(gameObject);
        giveItemBtn.onClick.AddListener(delegate { PanelCraft(); });
    }

    private void Start()
    {
        player = FindAnyObjectByType<Movimentacao>();
    }

    private void Update()
    {
        if(carriedItem ==  null)
        {
            return;
        }

        carriedItem.transform.position = Input.mousePosition;
    }

    public void SetCarriedItem(InventoryItem item)
    {
        if(carriedItem != null)
        {
            if(item.activeSlot.myTag != SlotTag.None && item.activeSlot.myTag != carriedItem.myItem.itemTag)
            {
                return;
            }

            item.activeSlot.setItem(carriedItem);
        }

        if(item.activeSlot.myTag != SlotTag.None)
        {
            Equipment(item.activeSlot.myTag, null);
        }

        carriedItem = item;
        carriedItem.canvasGroup.blocksRaycasts = false;
        item.transform.SetParent(draggablesTransform);
    }

    public void Equipment(SlotTag tag, InventoryItem item = null)
    {
        switch(tag)
        {
            case SlotTag.Head:
                if(item == null)
                {
                    Debug.Log("Removeu Item da Tag head");
                }
                else
                {
                    Debug.Log("Equipou na tag head");
                }
                break;
            case SlotTag.Chest:
                break;
            case SlotTag.Legs:
                break;
            case SlotTag.Feet:
                break;
        }
    }

    public void PanelCraft()
    {
        panelCrafting.SetActive(true);
        player.enabled = false;
    }

    Item PickRandonItem()
    {
        int random = Random.Range(0, items.Count);
        return items[random];
    }

    Item PickItem(Item pickItem)
    {
        //Debug.Log("Verificando pickItem: " + pickItem);
        //Debug.Log("Verificando inventorySlots: " + inventorySlot);
        //Debug.Log("Verificando inventorySlots.Length: " + inventorySlot?.Length);
        //Debug.Log("Verificando firstEmptySlot: " + firstEmptySlot);
        //Debug.Log("PickItem chamado com: " + pickItem);
        //Debug.Log("Itens no inventário: " + items.Length);
        foreach (var item in items)
        {
            if(item.name == pickItem.name)
            {
               return item;
            }
        }
        return null;
    }

    public void PickUpItem(Item item)
    {
        Item _item = item;

        if (_item == null)
        {
            _item = PickItem(item);
            Debug.Log("Passou aqui!!!!");
        }

        for (int i = 0; i < inventorySlot.Length; i++)
        {
            if (inventorySlot[i].myItem == null)
            {
                InventoryItem newItem = Instantiate(itemPrefab, inventorySlot[i].transform);
                newItem.Initialize(_item, inventorySlot[i]);
                return;
            }
        }
    }

    public void DropItem(InventoryItem item)
    {
        Debug.Log($"Drop item {item.name}");
        SpawnObjectNearPlayer(item);
        Destroy(item.gameObject);
    }

    public void SpawnObjectNearPlayer(InventoryItem item)
    {
        Transform player = GameObject.Find("Player").transform;

        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float randomDistance = Random.Range(0.1f, 0.5f);

        Vector2 spawPosition = (Vector2)player.position + randomDirection * randomDistance;
        GameObject dropItemPrefab = Instantiate(item.myItem.prefab, spawPosition, Quaternion.identity);
        dropItemPrefab.GetComponent<SpriteRenderer>().sprite = item.myItem.sprite;
        dropItemPrefab.GetComponent<PickUpItem>().item = item.myItem;
    }

    public bool hasItem(Item item)
    {
        return items.Contains(item);
    }

    public void RemoveItem(Item item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
        }
    }

    public void AddItem(Item item)
    {
        items.Add(item);

    }
}
