using UnityEngine;
using UnityEngine.EventSystems;

public class IventorySlot : MonoBehaviour, IPointerClickHandler
{
    public InventoryItem myItem { get; set; }

    //public SlotTag myTag;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (Inventory.carriedItem == null)
            {
                return;
            }
            setItem(Inventory.carriedItem);
        }
    }

    public void setItem(InventoryItem item)
    {
        if (item == null)
        {
            Debug.LogError("Item passado para o slot é nulo!");
            return;
        }

        this.myItem = item;

        Inventory.Instance.items.Add(item.myItem);
        Debug.Log($"Item {item.myItem.name} adicionado ao inventário.");

        Inventory.carriedItem = null;

        if (item.activeSlot != null)
        {
            item.activeSlot.myItem = null;
        }

        myItem = item;
        myItem.activeSlot = this;
        myItem.transform.SetParent(transform);
        myItem.canvasGroup.blocksRaycasts = true;
        //ClearSlot();
    }

    public void ClearSlot()
    {
        myItem = null;

    }
}
