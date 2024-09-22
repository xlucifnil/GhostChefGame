using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    GameObject player;
    int heartSegments = 2;
    public Image fullHeart, halfHeart, emptyHeart;
    public Vector2 heartOffset;
    public int heartSpacing;
    public List<Image> hearts = new List<Image>();
    public TMPro.TextMeshProUGUI SnackText;
    public Image energyBar;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        DisplayHealth();
        DisplayEnergy();
    }

    public void DisplayHealth()
    {
        if (player != null)
        {
            for (int i = 0; i < hearts.Count; i++)
            {
                Destroy(hearts[i]);
            }
            hearts.Clear();
            int currentHealth = player.GetComponent<PlayerStats>().health;
            int maxHealth = player.GetComponent<PlayerStats>().maxHealth;

            for (int i = 0; i < currentHealth / heartSegments; i++)
            {
                Image aHeart = Instantiate(fullHeart, gameObject.transform);
                aHeart.transform.position = new Vector2(aHeart.transform.position.x + heartOffset.x + (heartSpacing * hearts.Count) - Screen.width/2, aHeart.transform.position.y + heartOffset.y + Screen.height/2);
                hearts.Add(aHeart);
            }

            if (currentHealth % heartSegments == 1)
            {
                Image aHeart = Instantiate(halfHeart, gameObject.transform);
                aHeart.transform.position = new Vector2(aHeart.transform.position.x + heartOffset.x + (heartSpacing * hearts.Count) - Screen.width/2, aHeart.transform.position.y + heartOffset.y + Screen.height/2);
                hearts.Add(aHeart);
            }

            for (int i = hearts.Count; i < maxHealth / heartSegments; i++)
            {
                Image aHeart = Instantiate(emptyHeart, gameObject.transform);
                aHeart.transform.position = new Vector2(aHeart.transform.position.x + heartOffset.x + (heartSpacing * hearts.Count) - Screen.width / 2, aHeart.transform.position.y + heartOffset.y + Screen.height / 2);
                hearts.Add(aHeart);
            }
        }
    }

    public void DisplayEnergy()
    {
        if (player != null)
        {
            energyBar.fillAmount = player.GetComponent<PlayerStats>().currentEnergy / player.GetComponent<PlayerStats>().maxEnergy;
        }
    }
}
