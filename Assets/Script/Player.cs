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

    void Start()
    {
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
            Debug.Log("Diminuindo");

            if(entity.currentHealt < 0)
            {
                entity.currentHealt = 0;
            }
        }

        isDiminutedLife = false;
    }

    public void AddVida(int quantidade)
    {
        entity.currentHealt += quantidade;
        if (entity.currentHealt > 100) entity.currentHealt = 100;
        //Debug.Log($"Vida do personagem: {entity.currentHealt}");
    }
}
