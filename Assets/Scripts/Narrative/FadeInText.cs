using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FadeInText : MonoBehaviour
{
    public RawImage textBox;
    public TextMeshProUGUI text;
    bool collided = false;
    public float fadeSpeed = 1.0f;

    // Update is called once per frame
    void Update()
    {
        if (collided)
        {
            if(textBox != null)
            {
                textBox.color = new Color(textBox.color.r, textBox.color.g, textBox.color.b, textBox.color.a + (fadeSpeed * Time.deltaTime));
            }

            if (text != null)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (fadeSpeed * Time.deltaTime));
            }
        }
        else
        {
            if (textBox != null)
            {
                textBox.color = new Color(textBox.color.r, textBox.color.g, textBox.color.b, textBox.color.a - (fadeSpeed * Time.deltaTime));
            }

            if (text != null)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (fadeSpeed * Time.deltaTime));
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collided = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collided = false; 
        }
    }
}
