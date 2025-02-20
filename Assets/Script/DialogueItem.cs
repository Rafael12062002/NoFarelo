using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueItem : MonoBehaviour
{
    DialogueController controller;
    public GameObject itemColleter;
    public static DialogueItem instance;
    Movimentacao velocidade;
    private DropItem dropItem;

    // Start is called before the first frame update
    void Start()
    {
        velocidade = FindAnyObjectByType<Movimentacao>();
        controller = FindAnyObjectByType<DialogueController>();
        dropItem = FindAnyObjectByType<DropItem>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            instance = this;
            collision.collider.GetComponent<Movimentacao>().enabled = false;
            itemColleter = gameObject;
            Debug.Log("Colidiu com " + itemColleter.name);
            controller.ProximaFala();
        }
    }

    public IEnumerator IniciarColeta()
    {
        Debug.Log("Metodo chamado");

        controller.progressCollet.ResetProgress();
        yield return new WaitForSeconds(0.1f);

        controller.progressCollet.StartFill();

        while(!controller.progressCollet.isFull)
        {
            velocidade.enabled = false;
            yield return null;
        }
        if (itemColleter != null && controller != null)
        {
            velocidade.enabled = true;
            Debug.Log("Coletando... " + itemColleter.name);
            controller.progress.SetActive(false);
            GetComponent<DropItem>().Drop();
            Destroy(itemColleter, 0.1f);
            itemColleter = null;
        }
    }

    public void colletarItem()
    {
        StartCoroutine(IniciarColeta());
    }
}
