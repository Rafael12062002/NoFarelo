using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MudarFase : MonoBehaviour
{
    private Movimentacao player;
    Player vida;
    public GameObject IrFase;
 
    void Start()
    {
        IrFase.SetActive(false);
        IrFase.SetActive(true);
        player = FindAnyObjectByType<Movimentacao>();
        vida = FindAnyObjectByType<Player>();
        StartCoroutine(FecharJogo());
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || vida == null || IrFase == null) return;

        string nomeCena = "";

        switch(IrFase.tag)
        {
            case "B1":
                nomeCena = "B1";
                break;
            case "C1":
                nomeCena = "FaseIntrodutoria";
                break;
            case "A1":
                nomeCena = "A1";
                break;
            case "B2":
                nomeCena = "B2";
                break;
            default:
                Debug.LogWarning("Tag de fase não reconhecida: " + IrFase.tag);
                return;
        }

        vida.SalvarVida();
        StartCoroutine(TrocarCena(nomeCena));
    }

    private IEnumerator TrocarCena(string cena)
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(cena);
    }

    public IEnumerator FecharJogo()
    {
        if(IrFase.name == "B2")
        {
            yield return new WaitForSeconds(5);
            Application.Quit();
        }
    }
}
