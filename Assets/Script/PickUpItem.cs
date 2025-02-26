using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    public Item item;
    bool alreadyPickup = false;
    private Rigidbody2D rb;
    public AudioSource somColetado;
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

    private void Start()
    {
        somColetado = GetComponent<AudioSource>();
        string nameObject = gameObject.name;

        if(GameManager.Instance != null && GameManager.Instance.coletado(transform.position))
        {
            Debug.Log($"Item {nameObject} já foi coletado. Removendo da cena.");
            Destroy(gameObject);
        }
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
            if (item != null)
            {
                Debug.Log($"Tentando adicionar item: {item.name}");

                bool coletado = Inventory.Instance.PickUpItem(item);

                if (coletado)
                {
                    somColetado.Play();
                    alreadyPickup = false; // Evita execução múltipla no mesmo frame
                    Vector2 posicao = gameObject.transform.position;
                    GameManager.Instance.MarcarObjetosColetado(posicao);
                    Destroy(gameObject, 0.1f);
                }
            }
            else
            {
                Debug.LogWarning("Tentativa de pegar um item nulo.");
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
