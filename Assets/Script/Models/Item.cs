using Unity.VisualScripting;
using UnityEngine;

public enum SlotTag {None, Head, Chest, Legs, Feet}

[CreateAssetMenu(menuName = "No farelo/item")]
public class Item : ScriptableObject
{
    public Sprite sprite;
    public SlotTag itemTag;
    public GameObject prefab;
}
