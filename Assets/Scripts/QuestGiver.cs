using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class QuestGiver : MonoBehaviour
{
    public Quest quest;

    private EnemyController enemyController;

    public GameObject questWindow;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI titleTextInfo;
    public TextMeshProUGUI descriptionTextInfo;
    public RubyController rubyController;

    public void OpenQuestWindow()
    {
        questWindow.SetActive(true);
        titleText.text = quest.title;
        titleTextInfo.text = quest.title;
        descriptionText.text = quest.description;
        descriptionTextInfo.text = quest.description;

    }

    public void AcceptQuest()
    {
        questWindow.SetActive(false);
        quest.isActive = true;
        // give to player.
        rubyController = FindObjectOfType<RubyController>();
        rubyController.quest = quest;
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
    }
}
