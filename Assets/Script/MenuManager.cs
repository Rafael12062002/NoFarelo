using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Button buttonStartGame;
    public Button continueGame;
    public Button buttonQuitGame;
    public float fadeDuration = 3.0f;
    public CanvasGroup canvasGroup;
    public ParticleSystem chuva;

    // Start is called before the first frame update
    void Start()
    {
        if(!PlayerPrefs.HasKey("objetosColetados"))
        {
            continueGame.interactable = false;
        }
        buttonStartGame.onClick.AddListener(() => StartCoroutine(StartGame()));
        continueGame.onClick.AddListener(() => StartCoroutine(Continue()));
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
        //GameManager.Instance.ResetarProgresso();
        SceneManager.LoadScene("Intro");
       
    }

    public IEnumerator Continue()
    {
        continueGame.interactable = false;
        yield return StartCoroutine(FadeOut());
        yield return new WaitForSeconds(1f);
        GameManager.Instance.CarregarProgresso();
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
