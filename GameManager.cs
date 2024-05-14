using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; // Controlador de vida en los corazones
using TMPro; // libreria para temas de dialogo

public class GameManager : MonoBehaviour
{
    public Image[] playerHearts;
    public Sprite[] heartStatus;
    int currentHearts; // número maximo de corazones
    int hp;

    static int minHearts = 3;
    static int maxHearts = 14;

    public GameObject dialogBox;
    public TextMeshProUGUI dialogText;

    public GameObject npcDialogBox;
    public TextMeshProUGUI npcDialogText;
    public TextMeshProUGUI npcName;
    public Image npcImage;


    void Start()
    {
        currentHearts = Mathf.Clamp(currentHearts, minHearts, maxHearts);
        hp = Mathf.Clamp(hp, 1, currentHearts * 4);
        UpdateCurrentHearts();
    }

    public bool CanHeal()
    {
        return hp < currentHearts * 4; // esto es para la vida actual del los corazones
    }

    public void IncreaseMaxHP()
    {
        currentHearts++;
        currentHearts = Mathf.Clamp(currentHearts, minHearts, maxHearts);
        hp = currentHearts * 4; // aumenta el corazón grande por 4
        UpdateCurrentHearts();
    }

    public void UpdateCurrentHP(int x)
    {
        hp += x;
        hp = Mathf.Clamp(hp, 0, currentHearts * 4); 
        UpdateCurrentHearts();
    }

    private void UpdateCurrentHearts()
    {
        int aux = hp;

        for (int i = 0; i < maxHearts; i++)
        {
            if (i < currentHearts)
            {
                playerHearts[i].enabled = true; // activar la imagen del corazón
                playerHearts[i].sprite = GetHeartStatus(aux);
                aux -= 4;
            }
            else
            {
                playerHearts[i].enabled = false; // Se desactiva la imagen
            }
        }
    }

    private Sprite GetHeartStatus(int x)
    {
        switch (x)
        {
            case >= 4: return heartStatus[4];
            case 3: return heartStatus[3];
            case 2: return heartStatus[2];
            case 1: return heartStatus[1];
            default: return heartStatus[0];
        }
    }

    public void ShowText(string text)
    {
        dialogBox.SetActive(true);
        dialogText.text = text;
        Time.timeScale = 0; // deteneer el juego cuando hay interacción
    }

    public void HideText()
    {
        dialogBox.SetActive(false);
        dialogText.text = "";
        Time.timeScale = 1; // el juego se reanuda una vez se deja la interacción
    }

    public void NPCShowText(string text, string name, Sprite image)
    {
        npcDialogBox.SetActive(true);
        npcDialogText.text = text;
        npcName.text = name;
        npcImage.sprite = image;
        Time.timeScale = 0; // deteneer el juego cuando hay interacción
    }

    public void NPCHideText()
    {
        npcDialogBox.SetActive(false);
        npcDialogText.text = "";
        npcName.text = "";
        npcImage.sprite = null;
        Time.timeScale = 1; // el juego se reanuda una vez se deja la interacción
    }
}
