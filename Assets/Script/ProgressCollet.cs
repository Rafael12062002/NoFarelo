using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ProgressCollet : MonoBehaviour
{
    public float minValue = 0f;
    public float maxValue = 100f;
    public float currentValue = 0f;
    public Image Fill_Image;
    public float fillTime = 3f;
    public bool isFull = false;
    private bool isFilling = false; // Adicionado para controlar quando a barra pode encher

    private void Start()
    {
        Fill_Image.fillAmount = 0f;
        StartFill(); // Começa a encher automaticamente ao iniciar
    }

    private IEnumerator FillCoroutine(float time)
    {
        isFilling = true; // A barra pode encher
        Debug.Log("Iniciando a coroutine de preenchimento...");

        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            currentValue = Mathf.Lerp(minValue, maxValue, elapsedTime / time);
            Fill_Image.fillAmount = currentValue / maxValue;
            yield return null;
        }

        currentValue = maxValue;
        Fill_Image.fillAmount = 1f;
        isFull = true;
        isFilling = false; // Impede que a barra recomece sozinha

        Debug.Log("Esta cheio: " + isFull);
    }

    public void ResetProgress()
    {
        Debug.Log("Resetando progresso...");

        StopAllCoroutines(); // Para qualquer coroutine ativa
        currentValue = 0f;
        Fill_Image.fillAmount = 0f;
        isFull = false;
        isFilling = false; // Bloqueia o preenchimento automático
    }

    public void StartFill()
    {
        if (!isFilling) // Só inicia se não estiver preenchendo
        {
            StartCoroutine(FillCoroutine(fillTime));
        }
    }
}
