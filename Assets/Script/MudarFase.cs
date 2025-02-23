using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MudarFase : MonoBehaviour
{
    private Movimentacao player;
    public GameObject IrFase;
    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<Movimentacao>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(IrFase.CompareTag("B1"))
            {
                SceneManager.LoadScene("B1");
                Debug.Log("Entrando B1 com a tag de fase: " + IrFase.tag);
            }
        }
        if(collision.CompareTag("Player"))
        {
            if(IrFase.CompareTag("C2"))
            {
                SceneManager.LoadScene("B2");
                Debug.Log("Entrando B2 com a tag de fase: " + IrFase.tag);
            }
        }
        if(collision.CompareTag("Player"))
        {
            if(IrFase.CompareTag("C1"))
            {
                SceneManager.LoadScene("FaseIntrodutoria");
                Debug.Log("Entrando C1 com a tag de fase: " + IrFase.tag);
            }
        }
    }
}
