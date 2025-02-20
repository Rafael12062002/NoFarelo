using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCrafting : MonoBehaviour
{
    public GameObject panelCrafting;
    public Button fechar;
    Movimentacao player;
    
    public void Start()
    {
        player = FindAnyObjectByType<Movimentacao>();
        fechar.onClick.AddListener(delegate { FecharPanel(); });
    }

    public void FecharPanel()
    {
        panelCrafting.SetActive(false);
        player.isWalking = false;
        player.enabled = true;
    }
}
