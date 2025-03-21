using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    [Header("Story Setup")]
    public TMP_Text storyText;
    public float typingSpeed = 0.05f;
    private string fullText;
    private bool isTyping = false;
    private int currentLineIndex = 0;

    private string[] gameOverStoryLines = {
        "The world fell silent. Cities turned to ash. The Blight spread across the world before we even had a chance to mourn it. We couldn't outrun it. We couldn't hide.",
        "It was alive-deadly and unstoppable. And worse-its touch stole our memories. Each day we wake up, we remember less. Family, friends, the past-everything fades.",
        "As the blight consumed our city, leaving it in ruins, we knew we had to escape. We sailed to the one place untouched by its grasp-a hidden island, far from the death and destruction of our world.",
        "But we couldn't even remember how we got here. All we had were torn pages from our old diaries-manic, scribbled notes from our past selves, warning us.",
        "This island was supposed to be our final refuge. The Blight never reached it... or so we thought..."
    };

    private void Start()
    {
        StartCoroutine(DisplayNextLine());
    }

    private IEnumerator DisplayNextLine()
    {
        isTyping = true;
        fullText = gameOverStoryLines[currentLineIndex];
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

                if (currentLineIndex < gameOverStoryLines.Length)
                {
                    StartCoroutine(DisplayNextLine());
                }
                else
                {
                    SceneManager.LoadScene("GameOver");
                }
            }
        }
    }
}
