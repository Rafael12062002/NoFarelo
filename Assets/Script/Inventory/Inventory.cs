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
    [SerializeField] public InventoryItem itemPrefab;

    [SerializeField] public List<Item> items = new List<Item>();

    [SerializeField] Button giveItemBtn;
    IventorySlot limparSlot;
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
        if (carriedItem == null)
        {
            return;
        }

        carriedItem.transform.position = Input.mousePosition;
    }

    public int GetItemCount(Item item)
    {
        int count = 0;

        if (item == null)
        {
            Debug.LogError("Item � nulo!");
            return count;
        }

        foreach (var inventoryItem in items)
        {
            if (inventoryItem == item)
            {
                count++;
            }
        }
        Debug.Log($"Contando itens: {item.name} - Total no invent�rio: {count}");
        return count;
    }

    // M�todo para remover uma quantidade espec�fica de um item
    public void RemoveItems(string itemName, int quantityToRemove)
    {
        int removedCount = 0;
        for (int i = items.Count - 1; i >= 0; i--) // Percorrer de tr�s para frente evita problemas de indexa��o ao remover
        {
            if (items[i].name == itemName)
            {
                items.RemoveAt(i);
                removedCount++;

                if (removedCount >= quantityToRemove)
                    break; // Sai do loop ap�s remover a quantidade necess�ria
            }
        }
        //limparSlot.ClearSlot();
        Debug.Log("Itens removidos: " + removedCount);
    }

    // M�todo para adicionar um item ao invent�rio
    public void AddItem(Item item)
    {
        Debug.Log("AddItem Chamado");
        foreach(var inInventory in items)
        {
            if(inInventory.name == item.name)
            {
                Debug.Log("Comparando: " + item.name + " com " + inInventory.name);
                inInventory.quantity++;
                items.Add(item);
                UpdateInventoryUI();
                Debug.Log("Id do novo item: " + item.id);
                Debug.Log("quantidade atual do item: " + inInventory.quantity);
                return;
            }
            Debug.Log("Passou aqui");
        } 
    }

    public void UpdateInventoryUI()
    {
       
    }

    public void SetCarriedItem(InventoryItem item)
    {
        if (carriedItem != null)
        {
            if (item.activeSlot.myTag != SlotTag.None && item.activeSlot.myTag != carriedItem.myItem.itemTag)
            {
                return;
            }

            item.activeSlot.setItem(carriedItem);
        }
        carriedItem = item;
        carriedItem.canvasGroup.blocksRaycasts = false;
        item.transform.SetParent(draggablesTransform);
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
        Debug.Log("Verificando pickItem: " + pickItem);
        //Debug.Log("Verificando inventorySlots: " + inventorySlot);
        Debug.Log("Verificando inventorySlots.Length: " + inventorySlot?.Length);
        //Debug.Log("Verificando firstEmptySlot: " + firstEmptySlot);
        //Debug.Log("PickItem chamado com: " + pickItem);
        Debug.Log("Itens no invent�rio: " + items.Count);
        foreach (var item in items)
        {
            Debug.Log("Comparando: " + item.name + " com " + pickItem.name);
            if (item.name == pickItem.name)
            {
                AddItem(item);
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

        // Verifica se o item n�o est� na lista e o adiciona
        if (!items.Contains(_item))
        {
            items.Add(_item);
            Debug.Log($"Item {item.name} adicionado � lista do invent�rio.");
        }

        // Verifica os slots do invent�rio
        for (int i = 0; i < inventorySlot.Length; i++)
        {
            if (inventorySlot[i].myItem == null)
            {
                InventoryItem newItem = Instantiate(itemPrefab, inventorySlot[i].transform);
                newItem.Initialize(_item, inventorySlot[i]);
                AddItem(_item);
                return;
            }
        }
    }

    private void AddItemToInventory(Item item)
    {
        // Agora adiciona o item � lista diretamente
        if (!items.Contains(item))
        {
            items.Add(item);
            Debug.Log($"Item {item.name} adicionado � lista do invent�rio.");
        }
        else
        {
            Debug.Log($"Item {item.name} j� existe na lista do invent�rio.");
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
        foreach (Item invItem in items)
        {
            if (invItem.name == item.name)
            {
                return true;
            }
        }
        return false;
    }
}