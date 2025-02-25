using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public Button botãoContinue;
    private string ultimaCenaSalva = "";
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            CarregarProgresso();

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

    public bool temJogoSalvo()
    {
        return objetosColetados.Count > 0 || PlayerPrefs.HasKey("UltimaCena");
    }

    public void salvarUltimaCena(string cena)
    {
        ultimaCenaSalva = cena;
        PlayerPrefs.SetString("UltimaCena", cena);
        PlayerPrefs.Save();
    }

    public void ContinuarJogo()
    {
        if(PlayerPrefs.HasKey("UltimaCena"))
        {
            string cena = PlayerPrefs.GetString("UltimaCena");
            SceneManager.LoadScene(cena);
        }
        else
        {
            novoJogo();
        }
    }

    public void novoJogo()
    {
        ResetarProgresso();
        SceneManager.LoadScene("Intro");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(Player ==  null)
        {
            Player = FindObjectOfType<Movimentacao>();
        }
        if(vida == null)
        {
            vida = FindObjectOfType<Player>();
        }
        if(canvas == null && scene.name == "FaseIntrodutoria")
        {
            canvas = FindObjectOfType<Canvas>();
        }
        if(eventSystem == null && scene.name == "FaseIntrodutoria")
        {
            eventSystem = FindObjectOfType<EventSystem>();
        }
        if(dialogueController == null)
        {
            dialogueController = FindObjectOfType<DialogueController>();
        }

        if(Player != null)
        {
            DontDestroyOnLoad(Player);
        }
        if(vida != null)
        {
            DontDestroyOnLoad(vida);
        }
        if(canvas != null)
        {
            DontDestroyOnLoad(canvas);
        }
        if(eventSystem != null)
        {
            DontDestroyOnLoad(eventSystem);
        }
        if(dialogueController != null)
        {
            DontDestroyOnLoad(dialogueController);
        }
        if(botãoContinue != null)
        {
            DontDestroyOnLoad(botãoContinue);
        }

        salvarUltimaCena(scene.name);
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
