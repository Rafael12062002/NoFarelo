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
    private HashSet<string> objetosDestruidos = new HashSet<string>();
    public Canvas canvas;
    public EventSystem eventSystem;
    public Movimentacao Player;
    public Player vida;
    public DialogueController dialogueController;
    private bool jogoCarregado = false;
    //public Button botãoContinue;
    private string ultimaCenaSalva = "";
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            CarregarProgresso();

            if (canvas != null)
            {
                DontDestroyOnLoad(canvas.gameObject);
            }
            if (eventSystem != null)
            {
                DontDestroyOnLoad(eventSystem);
            }
            if (Player != null)
            {
                DontDestroyOnLoad(Player);
            }
            if (vida != null)
            {
                DontDestroyOnLoad(vida);
            }
            if (dialogueController != null)
            {
                DontDestroyOnLoad(dialogueController);
            }
        }
        else if (Instance != this)
        {
            Debug.Log("Outro GameManager foi destruído.");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if(canvas.gameObject.scene.name != "DontDestroyOnLoad")
        {
            Destroy(canvas.gameObject);
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
        if (PlayerPrefs.HasKey("UltimaCena"))
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

    private void LogCanvasNaCena()
    {
        Canvas[] allCanvases = FindObjectsOfType<Canvas>();
        Debug.Log($"Total de Canvases encontrados na cena: {allCanvases.Length}");
        foreach (Canvas c in allCanvases)
        {
            Debug.Log($"Canvas encontrado: {c.gameObject.name}");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Cena carregada: {scene.name}");

        if (scene.name == "Menu" || scene.name == "B2")
        {
            Destroy(Player?.gameObject);
            Destroy(vida?.gameObject);
            Destroy(dialogueController?.gameObject);
            Destroy(canvas?.gameObject);
        }

        if(canvas ==  null && scene.name == "FaseIntrodutoria")
        {
            canvas = FindObjectOfType<Canvas>();
            if (canvas != null) DontDestroyOnLoad(canvas);
        }

        if (eventSystem == null)
        {
            eventSystem = FindObjectOfType<EventSystem>();
            if (eventSystem != null) DontDestroyOnLoad(eventSystem);
        }

        if (dialogueController == null)
        {
            dialogueController = FindObjectOfType<DialogueController>();
            if (dialogueController != null) DontDestroyOnLoad(dialogueController);
        }

        if (Player == null)
        {
            Player = FindObjectOfType<Movimentacao>();
            if (Player != null) DontDestroyOnLoad(Player);
        }

        if (vida == null)
        {
            vida = FindObjectOfType<Player>();
            if (vida != null) DontDestroyOnLoad(vida);
        }

        //salvarUltimaCena(scene.name);
        //CarregarProgresso();
    }


    public void RemoverCanvas()
    {
        Canvas canvasExist = FindObjectOfType<Canvas>();

        if(canvasExist != null && canvasExist.gameObject.scene.name != "DontDestroyOnLoad")
        {
            Destroy(canvasExist);
        }

    }
    public void MarcarObjetosColetado(Vector2 posicao)
    {
        string chave = $"item_{posicao.x}_{posicao.y}";
        if(!objetosColetados.Contains(chave))
        {
            objetosColetados.Add(chave);
            SalvarProgresso();
        }
    }

    public bool coletado(Vector2 posicao)
    {
        string chave = $"item_{posicao.x}_{posicao.y}";
        return objetosColetados.Contains(chave);
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
        if (!string.IsNullOrEmpty(dados))
        {
            string[] itensArray = dados.Split(',');
            foreach(string itens in itensArray)
            {
                objetosColetados.Add(itens);
            }
        }
    }

    public void ResetarProgresso()
    {
        objetosColetados.Clear();
        PlayerPrefs.DeleteKey("objetosColetados");
    }

    public void marcarObjetosDestruidos(Vector2 posicao)
    {
        string chave = $"item_{posicao.x}_{posicao.y}";
        if(!objetosDestruidos.Contains(chave))
        {
            objetosDestruidos.Add(chave);
        }
    }

    public bool foiDestruido(Vector2 posicao)
    {
        string chave = $"item_{posicao.x}_{posicao.y}";
        return objetosDestruidos.Contains(chave);
    }
}