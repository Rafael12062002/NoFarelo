using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    public Item item;
    bool alreadyPickup = false;


    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if(rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        rb.gravityScale = 0f;
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.freezeRotation = true;
    }

    public void OnTriggerEnter2D(Collider2D collider2D)
    {
        if(collider2D.CompareTag("Player"))
        {
            alreadyPickup = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            alreadyPickup = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && alreadyPickup)
        {
            if(item !=  null)
            {
                Inventory.Instance.PickUpItem(item);
                
                alreadyPickup = true;
                Inventory.Instance.AddItem(item);
                Debug.Log($"Tentando adicionar item: {item.name}");
                Destroy(this.gameObject);
            }
            else
            {
                Debug.LogWarning("Tentativa de pegar um item nulo.");
                return;
            }
        }
    }

    public void DropItemInpulse(Vector2 direction, float force)
    {
        if (rb != null)
        {
            Debug.Log("Impulso aplicado!");
            rb.velocity = Vector2.zero;
            //rb.gravityScale = 0f;
            rb.AddForce(direction * force, ForceMode2D.Impulse);
        }
        else
        {
            Debug.LogWarning("Rigidbody2D está nulo, impulso não aplicado!");
        }
    }
}
