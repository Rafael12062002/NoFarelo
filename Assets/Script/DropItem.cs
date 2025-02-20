using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField] private Item item;
    [SerializeField] private int itemDropRate = 50;
    [SerializeField] private int itemMinDrop = 1;
    [SerializeField] private int itemMaxDrop = 5;

    private void Start()
    {
        Debug.Log($"DropItem inicializado no objeto: {gameObject.name}");
    }

    public void Drop()
    {
        int rand = Random.Range(1, 101);
       
        if(rand <= itemDropRate)
        {
            int amount = Random.Range(itemMinDrop, itemMaxDrop + 1);
          
            for (int i = 0; i < amount; i++)
            {
               if(item.prefab != null)
                {
                    GameObject droppedItemObject = Instantiate(item.prefab, transform.position, Quaternion.identity);
                    //droppedItemObject.SetActive(true);
                    PickUpItem pickUpItem = droppedItemObject.GetComponent<PickUpItem>();

                    if (pickUpItem != null)
                    {
                        pickUpItem.item = this.item;
                        float randomForce = Random.Range(1.5f, 3f);
                        Vector2 direction = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(0.5f, 1f));
                        StartCoroutine(ApplyForceDelayed(pickUpItem, direction, randomForce));
                    }
                    Debug.Log($"Instanciado: {item.prefab.name}");
                }
                else
                {
                    Debug.Log("Vazio no objeto");
                }
            }
        }
    }

    private IEnumerator ApplyForceDelayed(PickUpItem pickUpItem, Vector2 direction, float force)
    {
        yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
        pickUpItem.DropItemInpulse(direction, force);
    }
}
