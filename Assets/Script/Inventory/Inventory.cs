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
    public void RemoveItems(int itemId, int quantityToRemove)
    {
        Item item = items.FirstOrDefault(i => i.id == itemId);
        int removedCount = 0;
        for (int i = items.Count - 1; i >= 0; i--) // Percorrer de tr�s para frente evita problemas de indexa��o ao remover
        {
            item.quantity -= quantityToRemove;

            if (item.quantity == 0)
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
        Debug.Log("AddItem Chamado para " + item.name + " (ID: " + item.id + "), Quantidade original recebida: " + item.quantity);

        Item existItem = items.FirstOrDefault(i => i.id == item.id);

        if (existItem != null) // Se o item j� existe, apenas soma a quantidade
        {
            Debug.Log("Item j� existe no invent�rio. ID: " + existItem.id);
            existItem.quantity += item.quantity; // Apenas soma a quantidade
        }
        else
        {
            Debug.Log("Item novo adicionado ao invent�rio.");
            items.Add(item);
        }

        Debug.Log("Quantidade atual do item: " + (existItem != null ? existItem.quantity : item.quantity));
        UpdateInventoryUI();
    }

    void UpdateInventoryUI()
    {
        // Supondo que voc� tenha uma lista de slots da UI, voc� pode atualizar os slots com os dados mais recentes do invent�rio
        // Por exemplo, percorrendo os itens e atualizando suas imagens e quantidades
       
        for (int i = 0; i < inventorySlot.Length; i++)
        {
            if (i < items.Count)
            {
                // Atualiza o slot da UI com o item atual
                //inventorySlot[i].setItem(items[i]);
            }
            else
            {
                // Limpa o slot se n�o houver item
                inventorySlot[i].ClearSlot();
            }
        }
    }

    public void SetCarriedItem(InventoryItem item)
    {
        if (carriedItem != null)
        {
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
                item.quantity = 1;
                AddItem(item);
                return item;
            }
        }
        return null;
    }

    public void PickUpItem(Item item)
    {
        if (item == null)
        {
            Debug.LogError("Item est� nulo!");
            return;
        }

        Debug.Log("Chamando PickUpItem para: " + item.name);

        // Verifica se o item j� existe no invent�rio
        Item _item = items.FirstOrDefault(i => i.id == item.id);

        if (_item != null)
        {
            _item.quantity += item.quantity;
            Debug.Log($"Item {item.name} j� existe. Nova quantidade: {_item.quantity}");
        }
        else
        {
            _item = Item.CreateItem(item.id, item.quantity, item.sprite, item.prefab);
            items.Add(_item);
            Debug.Log($"Item {item.name} adicionado ao invent�rio.");
        }

        // Adicionar aos slots de invent�rio
        for (int i = 0; i < inventorySlot.Length; i++)
        {
            if (inventorySlot[i].myItem == null)
            {
                InventoryItem newItem = Instantiate(itemPrefab, inventorySlot[i].transform);
                if (newItem != null)
                {
                    newItem.Initialize(_item, inventorySlot[i]);
                }
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