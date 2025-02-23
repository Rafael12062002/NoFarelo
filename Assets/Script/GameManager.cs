using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Canvas canvas;
    public EventSystem eventSystem;
    public Movimentacao Player;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("GameManager persiste entre as cenas.");
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
}
