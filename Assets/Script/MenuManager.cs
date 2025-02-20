using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Button buttonStartGame;
    public Button buttonQuitGame;
    public float fadeDuration = 3.0f;
    public CanvasGroup canvasGroup;
    public ParticleSystem chuva;

    // Start is called before the first frame update
    void Start()
    {
        buttonStartGame.onClick.AddListener(() => StartCoroutine(StartGame()));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator StartGame()
    {
        buttonStartGame.interactable = false;
        Debug.Log("Iniciando Fade Out...");
        yield return StartCoroutine(FadeOut());
        chuva.Stop();
        Debug.Log("Fade completo. Carregando a próxima cena...");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Intro");
       
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator FadeOut()
    {
        float t = 0f;
        while(t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1, 0, t / fadeDuration);
            Debug.Log($"Alpha: {canvasGroup.alpha}");
            yield return null;
        }
        canvasGroup.alpha = 0;
    }
}
