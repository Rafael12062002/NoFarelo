using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IPointerClickHandler
{
    Image itemIcon;
    public CanvasGroup canvasGroup {  get; private set; }

    public Item myItem { get; set; }

    public IventorySlot activeSlot { get; set; }
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        itemIcon = GetComponent<Image>();
        //DontDestroyOnLoad(gameObject);
    }

    public void Initialize(Item item, IventorySlot parent)
    {
        activeSlot = parent;
        activeSlot.myItem = this;
        myItem = item;
        itemIcon.sprite = item.sprite;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            Inventory.Instance.SetCarriedItem(this);
        }
        else if(eventData.button == PointerEventData.InputButton.Right)
        {
            Inventory.Instance.DropItem(this);
        }
    }
}
