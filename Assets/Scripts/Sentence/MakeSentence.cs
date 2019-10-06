﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeSentence : MonoBehaviour
{
    public Text textBox;
    public Animator animator;
    public Queue<string> sentences;
    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartSentence(Sentence sentence)
    {
        animator.SetBool("IsOpen", true);

        sentences.Clear();

        foreach(string q in sentence.sentences)
        {
            sentences.Enqueue(q);
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
        string j = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(DisplaySentence(j));
    }

    IEnumerator DisplaySentence(string sentence)
    {
        textBox.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            textBox.text += letter;
            yield return null;
        }
    }

    void EndSentence()
    {
        animator.SetBool("IsOpen", false);
    }
}
