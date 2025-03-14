using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
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

    void Awake()
    {
        if(GameManager.Instance.dialogueController == null)
        {
            DontDestroyOnLoad(gameObject);
        }
        else if(GameManager.Instance.dialogueController != null)
        {
            Destroy(gameObject);
        }
    }
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

        //if (item != null) Debug.Log("N�o est� nulo " + item);
        GameObject botaoClicado = EventSystem.current.currentSelectedGameObject;
        
        if (botaoClicado == buttonAceitar.gameObject)
        {
            Debug.Log("buttonAceitar Ativado " + buttonAceitar);
            painelDialogo.SetActive(false);
            progress.SetActive(true);
            velocidade.enabled = true;

            if (DialogueItem.instance != null)
            {
                DialogueItem.instance.colletarItem();
            }
            else
            {
                Debug.LogWarning("item ou itemCollecter nulo");
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
            Debug.LogWarning("Nenhum bot�o clicado");
        }

        EventSystem.current.SetSelectedGameObject(null);
    }
}
