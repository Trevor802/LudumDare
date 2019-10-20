using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MakeSentence : MonoBehaviour
{
    public Text textBox;
    public Animator animator;
    public List<string> sentences;
    [SerializeField]
    public AudioClip clip;
    // Start is called before the first frame update
    void Start()
    {
        sentences = new List<string>();
        AudioManager.instance.PlayMusic(clip);

    }

    public void StartSentence(Sentence sentence)
    {
        animator.SetBool("IsOpen", true);

        sentences.Clear();

        foreach(string q in sentence.sentences)
        {
            sentences.Add(q);
        }

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
        if(sentences.Count == 0)
        {
            EndSentence();
            return;
        }
        string j = sentences[0];
        sentences.RemoveAt(0);
        StopAllCoroutines();
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
        SceneManager.LoadScene("2DLevel");
    }
}
