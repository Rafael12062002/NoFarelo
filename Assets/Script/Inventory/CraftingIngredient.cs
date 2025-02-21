[System.Serializable]
public class CraftingIngredient
{
    public Item item; // O item que será usado no crafting
    public int amount; // A quantidade necessária do item
    public int id;

    public CraftingIngredient(Item item, int quantity, int id)
    {
        this.item = item;
        this.amount = quantity;
        this.id = id;
    }
}
