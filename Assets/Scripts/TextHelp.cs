using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;

public class TextHelp : MonoBehaviour
{
    public Text textObject; // Assign this in the Unity Editor
    public float fadeDuration; // Duration of the fade in/out

    public float textTimer;
    private bool message1Displayed = false;
    private bool message2Displayed = false;
    private bool message3Displayed = false;
    private bool message4Displayed = false;
    private bool message5Displayed = false;
    private bool message6Displayed = false;
    private bool message7Displayed = false;

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        textTimer += Time.deltaTime;

        if (textTimer > 0 && !message1Displayed)

        {
            DisplayText("If you need help, check the controlls/How to play in menu", 4);
            message1Displayed = true;
        }

        if (textTimer > 8 && !message2Displayed)

        {
            DisplayText("0 / 4 Portals Closed", 4);

            message2Displayed = true;
        }

        if (textTimer > 16 && !message3Displayed)

        {
            DisplayText("Good Luck", 4);
            message3Displayed = true;
        }


    }




    public void DisplayText(string text, float delay)
    {
        textObject.text = text;
        StartCoroutine(FadeTextInAndOut(fadeDuration, delay));
    }

    IEnumerator FadeTextInAndOut(float duration, float delay)
    {
        // Fade in
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            textObject.color = new Color(textObject.color.r, textObject.color.g, textObject.color.b, Mathf.Lerp(0, 1, t / duration));
            yield return null;
        }

        textObject.color = new Color(textObject.color.r, textObject.color.g, textObject.color.b, 1);

        // Wait for delay
        yield return new WaitForSeconds(delay);

        // Fade out
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            textObject.color = new Color(textObject.color.r, textObject.color.g, textObject.color.b, Mathf.Lerp(1, 0, t / duration));
            yield return null;
        }

        textObject.color = new Color(textObject.color.r, textObject.color.g, textObject.color.b, 0);
    }
}
