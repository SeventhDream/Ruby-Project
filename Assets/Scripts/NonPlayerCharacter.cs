using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

// This script controls the behaviour of non-player character GameObjects.
// Author: Reuel Terezakis

public class NonPlayerCharacter : MonoBehaviour
{
    public TextAsset textFile;

    Rigidbody2D rigidbody2D;

    // Dialogue box.
    Coroutine lastRoutine = null;
    public int questProgress = 0;
    public GameObject dialogueBox;
    public Image dialogPortrait;
    public TextMeshProUGUI dialogueText;
    [TextArea(3, 10)]
    public string[] sentences;
    public float typingSpeed;
    private RubyController rubyController;
    public GameObject continueButton;
    public Sprite NPCSprite;
    public Sprite PCSprite;
    public int index;
    public int endLine;
    private DialogueManager dialogueManager;
    private TextManager textManager;

    // Quest
    QuestGiver questGiver;
    private bool winCondition;
    public GameObject ammunitionPrefab;

    // Stores the Canvas Gameobject (for the dialog box).
    public GameObject dialogBox;

    void Awake()
    {
        winCondition = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        dialogueManager = FindObjectOfType<DialogueManager>();
        textManager = FindObjectOfType<TextManager>();
        endLine = 17;
        index = 0;
        if (textFile != null)
        {
            // Grab the text within the file and split it into separate lines.
            sentences = (textFile.text.Split('\n'));
        }

        rubyController = GameObject.FindObjectOfType<RubyController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (index > 0)
        {
            if (dialogueText.text == sentences[index - 1])
            {
                continueButton.SetActive(true);
            }
        }

        Vector2 position = rigidbody2D.position;

        // Stores the collider intersected by the raycast.
        RaycastHit2D hit = Physics2D.Raycast(rigidbody2D.position + Vector2.left * 0.2f, Vector2.up, -2.0f, LayerMask.GetMask("Character"));

        // Check if raycast made a collision.
        if (hit.collider != null)
        {
            RubyController character = hit.collider.GetComponent<RubyController>(); // Access behaviour script on collided GameObject
            if (character != null)
            {
                DisplayDialog(); // Call function to display dialog.
            }

            if (rubyController.isTalking == true)
            {
                HideDialog();
            }
        }
        else
        {
            HideDialog();
        }
    }

    // Called by RubyController when PC interacts with NPC (frog)
    public void DisplayDialog()
    {

        dialogBox.SetActive(true); // Enable dialog box.
    }

    public void HideDialog()
    {
        dialogBox.SetActive(false); // disable dialog box.
    }

    IEnumerator Type()
    {
        foreach (char letter in sentences[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    // Called by RubyController when PC interacts with NPC (frog)
    public void StartConversation()
    {
        StopCoroutine(Type());
        dialogueText.text = "";
        dialogueManager.animator.SetBool("IsOpen", true);
        if (rubyController.isTalking == true && lastRoutine != null)
        {
            StopCoroutine(lastRoutine);
            dialogueText.text = "";
            dialogueText.text = sentences[index - 1];
            continueButton.SetActive(true);
        }
        if (index <= endLine)
        {
            if (sentences[index].Contains("JAMBI"))
            {
                dialogPortrait.sprite = NPCSprite;
            }
            else if (sentences[index].Contains("RUBY"))
            {
                dialogPortrait.sprite = PCSprite;
            }
            rubyController.isTalking = true;
            dialogueText.text = "";
            lastRoutine = StartCoroutine(Type());
            index++;
        }
        else
        {
            if (winCondition == true)
            {
                SceneManager.LoadScene("Victory", LoadSceneMode.Single);
            }
            if (questProgress == 0)
            {
                GameObject ammunitionObject = Instantiate(ammunitionPrefab, rigidbody2D.position + Vector2.left * 2.0f, Quaternion.identity);
                GameObject ammunitionObject1 = Instantiate(ammunitionPrefab, rigidbody2D.position + Vector2.left * 4.0f, Quaternion.identity);
                questProgress++;
                    index = 18;
                    endLine = 21;
                questGiver = FindObjectOfType<QuestGiver>();
                questGiver.OpenQuestWindow();
            }
            dialogueText.text = "";
            dialogueManager.animator.SetBool("IsOpen", false);
            rubyController.isTalking = false;
        }
    }

    public void RobotQuestComplete()
    {
        index = 22;
        endLine = 24;
        winCondition = true;
    }

    public void SpeedUp()
    {
        StopCoroutine(lastRoutine);
        dialogueText.text = "";
        dialogueText.text = sentences[index - 1];
        continueButton.SetActive(true);
    }
}
