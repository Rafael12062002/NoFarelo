using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public GameObject painelDialogo;
    public Button buttonAceitar;
    public Button buttonNegar;
    Movimentacao velocidade;
    DialogueItem item;
    public GameObject progress;
    public ProgressCollet progressCollet;

    // Start is called before the first frame update
    void Start()
    {
        velocidade = FindObjectOfType<Movimentacao>();
        item = FindAnyObjectByType<DialogueItem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ProximaFala()
    {
        painelDialogo.SetActive(true);
    }

    public void buttonPressed()
    {
        
        //Debug.Log("Pressionado");

        //if (item != null) Debug.Log("Não está nulo " + item);
        GameObject botaoClicado = EventSystem.current.currentSelectedGameObject;
        
        if (botaoClicado == buttonAceitar.gameObject)
        {
            Item Machado = Inventory.Instance.items.FirstOrDefault(item => item.name == "Machado");

            if(Machado != null && Inventory.Instance.hasItem(Machado))
            {
                Debug.Log("Tem o Machado" + Machado);
                Debug.Log("buttonAceitar Ativado " + buttonAceitar);
                painelDialogo.SetActive(false);
                progress.SetActive(true);
                velocidade.enabled = true;

                if (DialogueItem.instance != null)
                {
                    // Debug.Log("Item e itemCollecter não é null");
                    DialogueItem.instance.colletarItem();
                }
                else
                {
                    Debug.LogWarning("item ou itemCollecter nulo");
                }
            }
            else
            {
                Debug.LogWarning("Precisa de um Machado");
                painelDialogo.SetActive(false);
                velocidade.enabled = true;
            }
        }
        else if (botaoClicado == buttonNegar.gameObject)
        {
            Debug.Log("buttonNegar Ativado", buttonNegar);
            painelDialogo.SetActive(false);
            velocidade.enabled= true;
        }
        else
        {
            Debug.LogWarning("Nenhum botão clicado");
        }

        EventSystem.current.SetSelectedGameObject(null);
    }
}
