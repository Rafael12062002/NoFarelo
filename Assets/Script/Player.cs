using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Entity entity;
    [Header("PlayerUI")]
    public Slider healt;
    private bool isDiminutedLife = false;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        CarregarVida();
        if (entity == null)
        {
            Debug.LogError("Entity não foi atribuída ao Player! Atribua no Inspector.");
            return;
        }
        entity.currentHealt = entity.maxHealt;
        healt.maxValue = entity.maxHealt;
        healt.value = entity.maxHealt;
        StartDiminuirVida();
    }

    private void Update()
    {
        healt.value = entity.currentHealt;
    }

    public void StartDiminuirVida()
    {
        if(!isDiminutedLife)
        {
            StartCoroutine(DiminuirVida());
        }
    }

    private IEnumerator DiminuirVida()
    {
        isDiminutedLife = true;

        while(entity.currentHealt > 0)
        {
            yield return new WaitForSeconds(10f);
            entity.currentHealt -= 5;

            if(entity.currentHealt < 0)
            {
                entity.currentHealt = 0;
            }
        }

        isDiminutedLife = false;
    }

    public void AddVida(int quantidade)
    {
        Debug.Log("AddVida foi chamada!");

        if (entity == null)
        {
            Debug.LogError("Entity está nulo no AddVida!");
            return;
        }

        Debug.Log($"Antes: {entity.currentHealt}");
        entity.currentHealt += quantidade;
        if (entity.currentHealt > 100) entity.currentHealt = 100;
        Debug.Log($"Depois: {entity.currentHealt}");
    }

    public void SalvarVida()
    {
        PlayerPrefs.SetInt("VidaPlayer", entity.currentHealt);
        PlayerPrefs.Save();
    }

    public void CarregarVida()
    {
        if(PlayerPrefs.HasKey("VidaPlayer"))
        {
            entity.currentHealt = PlayerPrefs.GetInt("VidaPlayer");
        }
    }
}
