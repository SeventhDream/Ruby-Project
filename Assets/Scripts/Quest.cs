using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Quest
{
    public NonPlayerCharacter nonPlayerCharatcer;
    public bool isActive;
    public string title;
    public string description;
    public int questsComplete;

    public QuestGoal goal;
    
    public void Complete()
    {
        
        isActive = false;
        nonPlayerCharatcer = GameObject.FindObjectOfType<NonPlayerCharacter>();
        nonPlayerCharatcer.RobotQuestComplete();
    }
}
