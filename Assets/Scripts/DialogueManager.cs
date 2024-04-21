using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public Animator animator;
    public Animator textDisplayAnim;

    NonPlayerCharacter nonPlayerCharacter;

    void Start()
    {
        nonPlayerCharacter = FindObjectOfType<NonPlayerCharacter>();
    }

    public void Continue()
    {
        nonPlayerCharacter.StartConversation();
    }


    public void Skip()
    {
        nonPlayerCharacter.SpeedUp();
    }

}