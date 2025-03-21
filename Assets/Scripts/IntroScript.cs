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

    private string[] introStoryLines = {
        "We woke up on an island. No memory of how we got here, or why. No memories whatsoever. Not even our names.",
        "All we have are the clothes on our backs-no food, no shelter, nothing.",
        "We've spotted some strange pages scattered around the island. Maybe we brought them with us? They seem important... I can't shake the feeling that they hold the key to something.",
        "I know the goal should be to survive, but I can't stop wondering: What happened? Why are we here?",
        "Maybe these pages hold the answers. I'm going to collect them-find out what's really going on."
    };

    private void Start()
    {
        StartCoroutine(DisplayStory(introStoryLines));
    }

    private IEnumerator DisplayStory(string[] storyLines)
    {
        foreach (string line in storyLines)
        {
            storyText.text = "";
            foreach (char letter in line.ToCharArray())
            {
                storyText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }

            yield return new WaitUntil(() => Input.anyKeyDown);
        }

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene("Instructions");
    }
}
