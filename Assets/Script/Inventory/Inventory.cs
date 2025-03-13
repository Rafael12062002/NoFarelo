using System.Collections.Generic;
using System.Linq;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UI;

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
    private Player vida;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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
        vida = FindAnyObjectByType<Player>();
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
            Debug.LogError("Item é nulo!");
            return count;
        }

        foreach (var inventoryItem in items)
        {
            if (inventoryItem == item)
            {
                count++;
            }
        }
        Debug.Log($"Contando itens: {item.name} - Total no inventário: {count}");
        return count;
    }

    // Método para remover uma quantidade específica de um item
    public void RemoveItems(int itemId, int quantityToRemove)
    {
        int removedCount = 0;

        // Percorrer a lista de trás para frente para evitar problemas ao remover elementos
        for (int i = items.Count - 1; i >= 0 && quantityToRemove > 0; i--)
        {
            if (items[i].id == itemId)
            {
                RemoveFromInventoryUI(items[i]); // Remove da UI
                items.RemoveAt(i); // Remove o objeto da lista
                removedCount++;
                quantityToRemove--;

                // Se já removemos a quantidade necessária, podemos parar
                if (quantityToRemove == 0)
                    break;
            }
        }

        Debug.Log($"Itens removidos: {removedCount}. Ainda faltam remover: {quantityToRemove}");
    }

    // Método para remover da interface visual do inventário
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
        // Supondo que você tenha uma lista de slots da UI, você pode atualizar os slots com os dados mais recentes do inventário
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
                // Limpa o slot se não houver item
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
        Item existItem = items.FirstOrDefault(i => i.name == pickItem.name);
        foreach (var item in items)
        {
            Debug.Log("Comparando: " + item.name + " com " + pickItem.name);
            if (item.name == pickItem.name && existItem != null)
            {
                existItem.quantity = item.quantity;
                //PickUpItem(pickItem);
                //AddItem(item);
                return item;
            }
        }
        return null;
    }

    public bool PickUpItem(Item item)
    {
        if (item == null)
        {
            Debug.LogError("Item está nulo!");
            return false;
        }

        Debug.Log("Chamando PickUpItem para: " + item.name);

        bool espacoSlot = false;

        for(int i = 0; i < inventorySlot.Length; i++)
        {
            if (inventorySlot[i].myItem == null)
            {
                espacoSlot = true;
                break;
            }
        }

        if(!espacoSlot)
        {
            Debug.Log("Inventario cheio");
            return false;
        }

        if(espacoSlot)
        {
            items.Add(item);
            Debug.Log($"Item {item.name} adicionado ao inventário.");
        }
        else
        {
            return false;
        }
        // Adicionar ao primeiro slot vazio do inventário
        for (int i = 0; i < inventorySlot.Length; i++)
        {
            if (inventorySlot[i].myItem == null)
            {
                InventoryItem newItem = Instantiate(itemPrefab, inventorySlot[i].transform);
                if (newItem != null)
                {
                    newItem.Initialize(item, inventorySlot[i]); // Inicializa o slot com o item
                }
                return true; // Sai do método após adicionar o item ao inventário
            }
        }
        return true;
    }


    private void AddItemToInventory(Item item)
    {
        // Agora adiciona o item à lista diretamente
        if (!items.Contains(item))
        {
            items.Add(item);
            Debug.Log($"Item {item.name} adicionado à lista do inventário.");
        }
        else
        {
            Debug.Log($"Item {item.name} já existe na lista do inventário.");
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

    public void ConsumirItem(InventoryItem inventoryItem)
    {
        Debug.Log("ConsumirItem foi chamada!");
        if (inventoryItem == null)
        {
            Debug.LogError("InventoryItem é nulo!");
            return;
        }

        if (inventoryItem.myItem == null)
        {
            Debug.LogError("O item dentro do InventoryItem é nulo!");
            return;
        }

        Debug.Log($"Tentando consumir o item: {inventoryItem.myItem.name}");

        if (inventoryItem.myItem.name == "Jambo" || inventoryItem.myItem.name == "Ajuru" || inventoryItem.myItem.name == "Cupu")
        {
            // Remove o item do inventário
            if (Inventory.Instance.items.Contains(inventoryItem.myItem))
            {
                Inventory.Instance.items.Remove(inventoryItem.myItem);
            }
            else
            {
                Debug.LogWarning("Item não encontrado na lista de inventário.");
            }
                vida.AddVida(20);

            // Destroi o objeto visualmente
            Destroy(inventoryItem.gameObject);

            Debug.Log("Consumiu o item Jambo e ganhou vida!");
            vida.StartDiminuirVida();
        }
        else
        {
            Debug.LogWarning("O item clicado não é um Jambo.");
        }
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

    public bool HasItemInventory(string name)
    {
        return items.Exists(item => item.name == name);
    }

    public Item GetItemByName(string name)
    {
        foreach(Item item in items)
        {
            if(item.name == name)
            {
                return item;
            }
        }
        return null;
    }

    public void SaveInventory()
    {
        List<string> itemNames = new List<string>();

        foreach (Item item in items)
        {
            itemNames.Add($"{item.name}:{item.quantity}");
        }

        PlayerPrefs.SetString("PlayerInventory", string.Join(",", itemNames));
        PlayerPrefs.Save();
    }

    public void LoadInventory()
    {
        string saveInventory = PlayerPrefs.GetString("PlayerInventory", "");

        if(!string.IsNullOrEmpty(saveInventory))
        {
            string[] itemNames = saveInventory.Split(',');

            foreach(string itemData in itemNames)
            {
                string[] itemParts = itemData.Split(":");
                string itemName = itemParts[0];
                int quantity = int.Parse(itemParts[1]);
                Item item = Inventory.Instance.GetItemByName(itemName);

                if(item != null)
                {
                    item.quantity = quantity;
                    Inventory.Instance.PickItem(item);
                }
            }
        }
    }
}