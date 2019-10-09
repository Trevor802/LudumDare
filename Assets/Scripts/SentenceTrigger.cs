using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentenceTrigger : MonoBehaviour
{
    public Sentence sentence;
    public void TriggerSentence()
    {
        FindObjectOfType<MakeSentence>().StartSentence(sentence);
    }
}
