using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private HashSet<string> objetosColetados = new HashSet<string>();
    public Canvas canvas;
    public EventSystem eventSystem;
    public Movimentacao Player;
    public Player vida;
    public DialogueController dialogueController;
    private bool jogoCarregado = false;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            CarregarProgresso();
            SceneManager.sceneLoaded += OnSceneLoaded;

            if(canvas != null )
            {
                DontDestroyOnLoad(canvas);
            }
            if(eventSystem != null)
            {
                DontDestroyOnLoad(eventSystem);
            }
            if (Player != null)
            {
                DontDestroyOnLoad(Player);
            }
            if(vida != null)
            {
                DontDestroyOnLoad(vida);
            }
            if(dialogueController != null)
            {
                DontDestroyOnLoad(dialogueController);
            }
        }
        else if(Instance != this)
        {
            Debug.Log("Outro GameManager foi destruído.");
            Destroy(gameObject);
        }
    }

    public void StartCoroutineFromActive(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CarregarProgresso();
    }

    public void MarcarObjetosColetado(string nome)
    {
        objetosColetados.Add(nome);
        SalvarProgresso();
    }

    public bool coletado(string nome)
    {
        return objetosColetados.Contains(nome);
    }

    public void SalvarProgresso()
    {
        string dados = string.Join(",", objetosColetados);
        PlayerPrefs.SetString("objetosColetados", dados);
        PlayerPrefs.Save();
    }

    public void CarregarProgresso()
    {
        if (jogoCarregado) return;

        string dados = PlayerPrefs.GetString("objetosColetados", "");
        if(!string.IsNullOrEmpty(dados))
        {
            objetosColetados = new HashSet<string>(dados.Split(','));
            jogoCarregado = true;
        }
    }

    public void ResetarProgresso()
    {
        objetosColetados.Clear();
        PlayerPrefs.DeleteKey("objetosColetados");
    }
}
