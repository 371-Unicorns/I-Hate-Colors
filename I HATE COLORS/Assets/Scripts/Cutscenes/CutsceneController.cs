using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutsceneController : MonoBehaviour {

    private readonly float TEXT_SPEED = 0.033f;
    private bool textAnimating, skipTextAnimation;

    public string nextScene;

    public Button skipButton;

    public Text text;
    public CutsceneFrame[] cutsceneFrames;

    private int idx = 0;
    private string showingText;

    void Start()
    {
        skipButton.gameObject.SetActive(true);
        string nextText = cutsceneFrames[idx].GetNextScript();
        StartCoroutine(AnimateText(nextText));
    }


    IEnumerator AnimateText(string strComplete)
    {
        if (strComplete == null)
        {
            yield break;
        }

        textAnimating = true;
        skipTextAnimation = false;

        int i = 0;
        showingText = "";
        while (i < strComplete.Length)
        {
            if (skipTextAnimation)
            {
                textAnimating = false;
                showingText = strComplete;
                text.text = showingText;
                yield break;
            }

            showingText += strComplete[i++];
            text.text = showingText;
            yield return new WaitForSeconds(TEXT_SPEED);
        }

        textAnimating = false;
    }
    
    /// Idea for still image cutscene taken from https://answers.unity.com/questions/120333/how-would-you-make-a-simple-still-image-cutscene.html
    /// Scrolling text partly taken from https://answers.unity.com/questions/50104/how-to-make-text-that-is-writen-automatically-lett.html
    ///   and https://answers.unity.com/questions/219281/making-text-boxes-with-letters-appearing-one-at-a.html
    private void Update()
    {
        if (idx >= cutsceneFrames.Length)
        {
            skipButton.gameObject.SetActive(false);
            SceneManager.LoadScene(nextScene);
        }

        if (Input.anyKeyDown)
        {
            if (textAnimating)
            {
                skipTextAnimation = true;
            }
            else
            {
                string nextText = cutsceneFrames[idx].GetNextScript();
                text.text = "";
                if (nextText != null)
                {
                    StartCoroutine(AnimateText(nextText));
                }
                else
                {
                    idx++;
                    if (idx < cutsceneFrames.Length)
                    {
                        StartCoroutine(AnimateText(cutsceneFrames[idx].GetNextScript()));
                    }
                }
            }
        }
    }

    private void OnGUI()
    {
        if (idx < cutsceneFrames.Length)
        {
            Rect imgPos;
            if (cutsceneFrames[idx].fullscreen)
            {
                imgPos = new Rect(0, 0, Screen.width, Screen.height);
            } else
            {
                imgPos = new Rect(0, Screen.height * .1f, Screen.width, cutsceneFrames[idx].frameImage.height * (Screen.width / (float)cutsceneFrames[idx].frameImage.width));
            }
            GUI.DrawTexture(imgPos, cutsceneFrames[idx].frameImage);
            text.transform.position = new Vector3(cutsceneFrames[idx].textPosition.x, cutsceneFrames[idx].textPosition.y, 0);
        }
    }
}
