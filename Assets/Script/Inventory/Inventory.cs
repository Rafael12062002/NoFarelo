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
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
            giveItemBtn.onClick.AddListener(delegate { PanelCraft(); });
        }
        else
        {
            Destroy(gameObject);
        }
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
        int removedCount = 0;

        // Percorrer a lista de tr�s para frente para evitar problemas ao remover elementos
        for (int i = items.Count - 1; i >= 0 && quantityToRemove > 0; i--)
        {
            if (items[i].id == itemId)
            {
                RemoveFromInventoryUI(items[i]); // Remove da UI
                items.RemoveAt(i); // Remove o objeto da lista
                removedCount++;
                quantityToRemove--;

                // Se j� removemos a quantidade necess�ria, podemos parar
                if (quantityToRemove == 0)
                    break;
            }
        }

        Debug.Log($"Itens removidos: {removedCount}. Ainda faltam remover: {quantityToRemove}");
    }

    // M�todo para remover da interface visual do invent�rio
    private void RemoveFromInventoryUI(Item item)
    {
        foreach (var slot in inventorySlot)
        {
            if (slot.myItem != null && slot.myItem.myItem == item)
            {
                Destroy(slot.myItem.gameObject); // Remove o objeto visual
                slot.ClearSlot(); // Limpa o slot
                break;
            }
        }
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
        
        foreach (var item in items)
        {
            Debug.Log("Comparando: " + item.name + " com " + pickItem.name);
            if (item.name == pickItem.name)
            {
                item.quantity = 1;
                //PickUpItem(pickItem);
                //AddItem(item);
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

        items.Add(item);
        Debug.Log($"Item {item.name} adicionado ao invent�rio.");

        // Adicionar ao primeiro slot vazio do invent�rio
        for (int i = 0; i < inventorySlot.Length; i++)
        {
            if (inventorySlot[i].myItem == null)
            {
                InventoryItem newItem = Instantiate(itemPrefab, inventorySlot[i].transform);
                if (newItem != null)
                {
                    newItem.Initialize(item, inventorySlot[i]); // Inicializa o slot com o item
                }
                return; // Sai do m�todo ap�s adicionar o item ao invent�rio
            }
        }

        // Caso o invent�rio esteja cheio, voc� pode adicionar um tratamento aqui
        Debug.Log("Invent�rio cheio. N�o foi poss�vel adicionar o item.");
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
        if(items.Contains(item.myItem))
        {
            items.Remove(item.myItem);
        }
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