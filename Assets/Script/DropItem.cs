using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField] private List<Item> itemsToDrop; // Lista de itens a serem droppados
    [SerializeField] private int itemDropRate = 50;
    [SerializeField] private int itemMinDrop = 1;
    [SerializeField] private int itemMaxDrop = 3; // Agora o máximo é 3, mas pode ser ajustado conforme necessário

    private void Start()
    {
        //Debug.Log($"DropItem inicializado no objeto: {gameObject.name}");
    }

    public void Drop()
    {
        int rand = Random.Range(1, 101);

        // Verifica a chance de drop
        if (rand <= itemDropRate)
        {
            int amount = Random.Range(itemMinDrop, itemMaxDrop + 1); // Escolhe aleatoriamente quantos itens vão ser droppados

            for (int i = 0; i < amount; i++)
            {
                // Escolhe aleatoriamente um item da lista de itens a serem droppados
                if (itemsToDrop.Count > 0)
                {
                    int randomItemIndex = Random.Range(0, itemsToDrop.Count);
                    Item selectedItem = itemsToDrop[randomItemIndex];

                    if (selectedItem.prefab != null)
                    {
                        GameObject droppedItemObject = Instantiate(selectedItem.prefab, transform.position, Quaternion.identity);
                        PickUpItem pickUpItem = droppedItemObject.GetComponent<PickUpItem>();

                        if (pickUpItem != null)
                        {
                            pickUpItem.item = selectedItem;
                            float randomForce = Random.Range(1.5f, 3f);
                            Vector2 direction = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(0.5f, 1f));
                            
                        }
                        Debug.Log($"Instanciado: {selectedItem.prefab.name} em {transform.position}");
                    }
                    else
                    {
                        Debug.Log("Prefab do item está vazio");
                    }
                }
            }
        }
    }

   
}
