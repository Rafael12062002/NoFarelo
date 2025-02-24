using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IPointerClickHandler
{
    private Image itemIcon;
    public CanvasGroup canvasGroup { get; private set; }
    public Item myItem { get;  set; }
    public IventorySlot activeSlot { get;  set; }
    private Player player;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        itemIcon = GetComponent<Image>();
        //DontDestroyOnLoad(gameObject);  // Se necess�rio, descomente essa linha
    }

    // Inicializa o item e o slot ativo
    public void Initialize(Item item, IventorySlot parent)
    {
        if (item == null)
        {
            Debug.LogError("Item � nulo!");
            return;
        }

        if (parent == null)
        {
            Debug.LogError("Slot pai � nulo!");
            return;
        }

        this.myItem = item;
        this.activeSlot = parent;

        activeSlot.myItem = this;  // Atualiza o slot ativo com o item
        itemIcon.sprite = item.sprite;  // Atualiza o �cone do item
    }

    // Lida com os cliques no item do invent�rio
    public void OnPointerClick(PointerEventData eventData)
    {
        // Se o clique for esquerdo
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Inventory.Instance.ConsumirItem(this);
            //Inventory.Instance.SetCarriedItem(this);  // Adiciona o item ao que est� sendo carregado
        }
        // Se o clique for direito
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            Inventory.Instance.DropItem(this);  // Remove o item ou realiza a a��o de descartar
        }
    }
}
