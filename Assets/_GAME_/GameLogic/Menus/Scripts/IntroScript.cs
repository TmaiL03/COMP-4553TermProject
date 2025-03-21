using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class IntroScript : MonoBehaviour
{
    [Header("Story Setup")]
    public TMP_Text storyText;
    public float typingSpeed = 0.05f;
    private string fullText;
    private bool isTyping = false;
    private int currentLineIndex = 0;

    private string[] introStoryLines = {
        "We woke up on an island. No memory of how we got here, or why. No memories whatsoever. Not even our names.",
        "All we have are the clothes on our backs-no food, no shelter, nothing.",
        "We've spotted some strange pages scattered around the island. Maybe we brought them with us? They seem important... I can't shake the feeling that they hold the key to something.",
        "I know the goal should be to survive, but I can't stop wondering: What happened? Why are we here?",
        "Maybe these pages hold the answers. I'm going to collect them-find out what's really going on."
    };

    private void Start()
    {
        StartCoroutine(DisplayNextLine());
    }

    private IEnumerator DisplayNextLine()
    {
        isTyping = true;
        fullText = introStoryLines[currentLineIndex];
        storyText.text = "";
            
        foreach (char letter in fullText.ToCharArray())
        {
            if (!isTyping)
            {
                storyText.text = fullText;
                break;
            }

            storyText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.anyKeyDown)
        {
            if (isTyping)
            {
                isTyping = false;
                storyText.text = fullText;
            }
            else
            {
                currentLineIndex++;

                if (currentLineIndex < introStoryLines.Length)
                {
                    StartCoroutine(DisplayNextLine());
                }
                else
                {
                    SceneManager.LoadScene("Instructions");
                }
            }
        }
    }
}
