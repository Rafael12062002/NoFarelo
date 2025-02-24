using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MudarFase : MonoBehaviour
{
    private Movimentacao player;
    public GameObject IrFase;
    Player vida;
    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<Movimentacao>();
        vida = FindAnyObjectByType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            
            if(IrFase.CompareTag("B1") && vida != null)
            {
                vida.SalvarVida();
                SceneManager.LoadScene("B1");
                Debug.Log("Entrando B1 com a tag de fase: " + IrFase.tag);
            }
        }
        if(collision.CompareTag("Player"))
        {
            if(IrFase.CompareTag("C1") && vida != null)
            {
                vida.SalvarVida();
                SceneManager.LoadScene("FaseIntrodutoria");
                Debug.Log("Entrando C1 com a tag de fase: " + IrFase.tag);
            }
        }
        if(collision.CompareTag("Player"))
        {
            if(IrFase.CompareTag("A1")  && vida != null)
            {
                vida.SalvarVida();
                SceneManager.LoadScene("A1");
                Debug.Log("Entrando A1 com a tag de fase: " + IrFase.tag);
            }
        }
    }
}
