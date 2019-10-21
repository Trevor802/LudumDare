using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text textBox;
    public Animator animator;
    public List<string> sentences;
    public string sceneToLoadAfterDialogue;
    private int sentenceIndex = 0;

    public void StartSentence()
    {
        animator.SetBool("IsOpen", true);
        textBox.gameObject.SetActive(true);
        NextSentence();
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            NextSentence();
        }
    }

    public void NextSentence()
    {
        if(sentenceIndex >= sentences.Count)
        {
            EndSentence();
            return;
        }
        StopAllCoroutines();
        textBox.text = "";
        // Uncomment the code block to display all sentences in one page
        /*
        for (int i = 0; i < sentenceIndex; i++)
        {
            textBox.text += sentences[i];
        }
        */
        string j = sentences[sentenceIndex];
        sentenceIndex++;
        StartCoroutine(DisplaySentence(j));
    }

    IEnumerator DisplaySentence(string sentence)
    {
        string rt = "";
        bool caching = false;
        bool metHead = false;
        foreach(char letter in sentence.ToCharArray())
        {
            if (letter == '<')
            {
                caching = true;
            }
            else if (letter == '>')
            {
                if (metHead)
                {
                    caching = false;
                    metHead = false;
                    textBox.text += rt;
                    rt = "";
                }
                else
                {
                    metHead = true;
                }
            }
            if (!caching)
            {
                textBox.text += letter;
                yield return null;
            }
            else
            {
                rt += letter;
            }
        }
    }

    void EndSentence()
    {
        animator.SetBool("IsOpen", false);
        SceneManager.LoadScene(sceneToLoadAfterDialogue);
    }
}
