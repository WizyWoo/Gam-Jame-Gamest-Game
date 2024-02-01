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
            DisplayText("Press E/LMB to pick up candles, and F/RMB to blow it out", 6);
            message1Displayed = true;
        }

        if (textTimer > 10 && !message2Displayed)

        {
            DisplayText("Find the <color=#0000ffff>portals</color> and enter them to find the <color=#ffff00ff>artifacts</color>", 6);

            message2Displayed = true;
        }

        if (textTimer > 20 && !message3Displayed)

        {
            DisplayText("The <color=#ffff00ff>artifacts</color> will close the <color=#0000ffff>portals</color>", 6);
            message3Displayed = true;
        }

        if (textTimer > 30 && !message4Displayed)

        {
            DisplayText("Destroy all the <color=#0000ffff>portals</color>", 6);

            message4Displayed = true;
        }

        if (textTimer > 40 && !message5Displayed)

        {
            DisplayText("Stay away from the <color=#ff0000ff>Ghosts</color>...", 6);
            message5Displayed = true;
        }

        if (textTimer > 50 && !message6Displayed)
        {
            DisplayText("And dont let the candles burn out..", 6);
            message6Displayed = true;
        }

        if (textTimer > 60 && !message7Displayed)
        {
            DisplayText("Good luck", 6);
            message7Displayed = true;
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
