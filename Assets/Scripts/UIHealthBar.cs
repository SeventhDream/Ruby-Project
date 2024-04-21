using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

// This script controls the behaviour of the UI Health Bar.
// Author: Reuel Terezakis

public class UIHealthBar : MonoBehaviour
{

    // Control panel
    public GameObject infoPanel;
    public TextMeshProUGUI questName;
    public TextMeshProUGUI questInfo;

    // Ammo Score
    public TextMeshProUGUI AmmoCount;
    public GameObject Ammo;
    public GameObject AmmoCounter;
    public bool canShoot = true;

    // Makes the script a static member (shared by all objects of that type).
    public static UIHealthBar instance { get; private set; } // We don’t want people to be able to change it from outside the script.

    public Image mask;
    float originalSize;

    private RubyController rubyController;

    // Store in the static 'instance' 'this', which is a special C# keyword that means “the object that currently runs that function”.
    void Awake()
    {
        instance = this; // Script stores itself in the static member called “instance” (Singleton, because only one object of that type can exist).
    }

    // Start is called before the first frame update
    void Start()
    {
        rubyController = FindObjectOfType<RubyController>();
        // Gets the size on screen with rect.width.
        originalSize = mask.rectTransform.rect.width;
    }

    // Update is called once per frame.
    void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            infoPanel.SetActive(true);
        }
        else
        {
            infoPanel.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
        }

        if (rubyController.health <= 0)
        {
            SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        }
    }

    public void SetValue(float value)
    {
        //Sets the size and anchor from code with SetSizeWithCurrentAnchors. 
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * value);
    }

    // Displays the current ammo count in the UI.
    public void UpdateAmmo(int ammo)
    {
        AmmoCount.text = ammo.ToString() + "/10";
    }
}
