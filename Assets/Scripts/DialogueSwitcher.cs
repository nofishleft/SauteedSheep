using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSwitcher : MonoBehaviour
{
    public int index;
    public string[] dialogue;

    public Text text;

    public Button NextBtn;
    public Button PrevBtn;

    void Start()
    {
        text.text = dialogue[index];
        PrevBtn.interactable = false;
    }

    public void Next()
    {
        if (++index == dialogue.Length)
        {
            index = dialogue.Length - 1;
        }

        if (index == dialogue.Length - 1)
        {
            //Disable next button
            NextBtn.interactable = false;
            //Enable prev button
            PrevBtn.interactable = dialogue.Length > 1;
        }
        text.text = dialogue[index];
    }
    public void Prev()
    {
        if (--index < 0)
        {
            index = 0;

        }

        if (index == 0)
        {
            //Disable prev button
            PrevBtn.interactable = false;
            //Enable next button
            NextBtn.interactable = dialogue.Length > 1;
        }

        text.text = dialogue[index];
    }

    public void Reset()
    {
        index = 0;
        text.text = dialogue[index];
        //Disable prev button
        PrevBtn.interactable = false;
        //Enable next button
        NextBtn.interactable = dialogue.Length > 1;
    }
}
