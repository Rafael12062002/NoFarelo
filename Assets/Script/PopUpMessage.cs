using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpMessage : MonoBehaviour
{
    public GameObject message;
    string[] messages = new string[5];
    public GameObject[] images;
    public GameObject panelTutorial;
    int cont = 0;
    public Button proximaInstrucao;
    private void Start()
    {
        Time.timeScale = 0f;
        panelTutorial.SetActive(true);
        proximaInstrucao.onClick.AddListener(NextTutorial);

        messages[0] = "Movimente-se usando as teclas AWSD";
        messages[1] = "Voc� pode abrir seu painel de crafting clicando neste bot�o";
        messages[2] = "Cada item apresenta seus materiais para serem fabricados, ao conseguir voc� pode crafta-lo";
        messages[3] = "Tome cuidado com seu Hp, pois por conta dos problemas climaticos voc� perde vida gradualmente";
        messages[4] = "Voc� pode recuperar HP consumindo frutas que coleta das arvores clicando com o bot�o esquerdo do mouse na fruta do seu inventario, voc� tamb�m pode dropar seus itens com o bot�o direito do mouse";

        if(images.Length == 0)
        {
            Debug.Log("Nenhuma imagem com a tag correta foi encontrada");
            return;
        }

        Tutorial();
    }
    
    public void Tutorial()
    {
        TextMeshProUGUI textComponent = message.GetComponent<TextMeshProUGUI>();
        textComponent.text = messages[cont];

        foreach(GameObject img in images)
        {
            img.SetActive(false);
        }
        images[cont].SetActive(true);
        message.SetActive(true);
    }

    public void NextTutorial()
    {
        cont++;

        if(cont < messages.Length)
        {
            Tutorial();
        }
        else
        {
            panelTutorial.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
