using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Introdução : MonoBehaviour
{
    string[] textos = new string[4];
    public GameObject[] imagens;
    public GameObject texto;
    int cont = 0;
    public float fadeDuration = 1.0f;
    public float displayTime = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        textos[0] = "O mundo não é mais o mesmo, diversos desastres ambientais estão assolando o planeta devido ao descaso do ser humano, árvores caem, rios secam, o solo se degrada, o calor se torna mais intenso, a flora foi deteriorada, a fauna entra em extinção, e pouco se tem feito para mitigar esses acontecimentos. ";
        textos[1] = "Para Ramon, dono de uma madeireira localizada na floresta amazônica, era somente mais um dia viajando a trabalho para monitorar de perto a produção.\r\n\r\n";
        textos[2] = "Entretanto, durante a viagem, uma forte tempestade atinge sua embarcação de forma violenta e o pior acontece...\r\n";
        textos[3] = "Não restou nada em meio ao caos, mas de alguma forma, Ramon sobreviveu e acorda às margens de um rio, mas o que antes era um mar de madeira, agora restou apenas destroços. A floresta que antes era sua fonte de lucro, agora é o seu único meio de subsistência.";

       if(imagens.Length == 0)
        {
            Debug.Log("Nenhuma imagem com a tag correta foi encontrada");
            return;
        }
        StartCoroutine(Rotina());
    }

   public IEnumerator Rotina()
    {
       if(cont < textos.Length)
        {
            GameObject imagemAtual = imagens[cont];
            TextMeshProUGUI textoComponent = texto.GetComponent<TextMeshProUGUI>();

            CanvasGroup imagemCanvas  = imagemAtual.GetComponent<CanvasGroup>();
            CanvasGroup textoCanvas = texto.GetComponent<CanvasGroup>();

            if(imagemCanvas == null) imagemCanvas = imagemAtual.AddComponent<CanvasGroup>();

            if(textoCanvas == null) textoCanvas = texto.AddComponent<CanvasGroup>();

            textoComponent.text = textos[cont];
            texto.SetActive(true);
            imagemAtual.SetActive(true);

            yield return StartCoroutine(FadeIn(imagemCanvas, textoCanvas));
            //yield return StartCoroutine(FadeIn(textoCanvas));

            yield return new WaitForSeconds(displayTime);
            

            yield return StartCoroutine(FadeOut(imagemCanvas, textoCanvas));
            //yield return StartCoroutine(FadeOut(textoCanvas));

            imagemAtual.SetActive(false);
            cont++;

            StartCoroutine(Rotina());
        }
       else
        {
            SceneManager.LoadScene("FaseIntrodutoria");
        }
    }

    IEnumerator FadeIn(CanvasGroup g, CanvasGroup t)
    {
        float temp = 0f;
        while(temp < fadeDuration)
        {
            temp += Time.deltaTime;
            g.alpha = Mathf.Lerp(0, 1, temp / fadeDuration);
            t.alpha = Mathf.Lerp(0, 1, temp / fadeDuration);
            yield return null;
        }
        g.alpha = 1;
        t.alpha = 1;
    }

    IEnumerator FadeOut(CanvasGroup g, CanvasGroup t)
    {
        float temp = 0f;
        while (temp < fadeDuration)
        {
            temp += Time.deltaTime;
            g.alpha = Mathf.Lerp(1, 0, temp / fadeDuration);
            t.alpha = Mathf.Lerp(1, 0, temp / fadeDuration);
            yield return null;
        }
        g.alpha = 0;
        t.alpha = 0;
    }
}
