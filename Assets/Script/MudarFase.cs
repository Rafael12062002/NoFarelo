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
    private string nomeCena = "";

    void Start()
    {
        Debug.Log($"vida: {vida}, IrFase: {IrFase}");
        IrFase.SetActive(false);
        IrFase.SetActive(true);
        player = GameObject.FindWithTag("Player").GetComponent<Movimentacao>();
        vida = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    private void Update()
    {
        Collider2D colisor = GetComponent<Collider2D>();
        if(colisor == null)
        {
            Debug.LogError("Colisor sumiu");
        }
        else if(!colisor.enabled)
        {
            Debug.LogError("Colisor desativado");
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Colisão detectada com: {collision.gameObject.name}");

        if (!collision.CompareTag("Player") || vida == null || IrFase == null)
        {
            Debug.LogError("Colisão falhou! Player não detectado ou referências nulas.");
            return;
        }

        Debug.Log("Player detectado! Verificando a tag de mudança de fase...");

        switch (IrFase.tag)
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


        Debug.Log($"Mudando para a cena: {nomeCena}");
        vida.SalvarVida();
        StartCoroutine(TrocarCena(nomeCena));
        StartCoroutine(FecharJogo());
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
