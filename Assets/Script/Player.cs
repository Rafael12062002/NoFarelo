using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Entity entity;
    [Header("PlayerUI")]
    public Slider healt;
    private bool isDiminutedLife = false;
    public GameObject panelMorte;
    public Button backToMenu;
    public Button novoJogo;

    private void Awake()
    {
        if(GameManager.Instance.vida == null)
        {
            GameManager.Instance.vida = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(GameManager.Instance.vida != this)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        CarregarVida();
        Canvas.ForceUpdateCanvases();
        if (entity == null)
        {
            Debug.LogError("Entity não foi atribuída ao Player! Atribua no Inspector.");
            return;
        }
        entity.currentHealt = entity.maxHealt;
        healt.maxValue = entity.maxHealt;
        healt.value = entity.maxHealt;
        StartDiminuirVida();
        healt.onValueChanged.Invoke(healt.value);
        backToMenu.onClick.AddListener(BackToMenu);
        novoJogo.onClick.AddListener(newGame);
    }

    private void Update()
    {
        healt.value = entity.currentHealt;
        Morte();
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
            entity.currentHealt -= 3;

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

    public void Morte()
    {
        if(entity.currentHealt == 0)
        {
            panelMorte.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    public void BackToMenu()
    {
        Destroy(GameManager.Instance);
        panelMorte.SetActive(false);
        SceneManager.LoadScene("Menu");
    }
    public void newGame()
    {
        Destroy(GameManager.Instance);
        panelMorte.SetActive(false);
        GameManager.Instance.ResetarProgresso();
        SceneManager.LoadScene("Menu");
    }
}
