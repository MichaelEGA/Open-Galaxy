using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class MainMenu: MonoBehaviour
{
    public GameObject mainMenu;

    public CanvasGroup background_black_cg;
    public CanvasGroup background_image_cg;
    public CanvasGroup disclaimer_cg;
    public CanvasGroup title_cg;
    public CanvasGroup menu_cg;
    public CanvasGroup loadingScreen_cg;

    public EventSystem eventSystem;
    public Dictionary<string, System.Delegate> functions;
    public GameObject[] buttons;
    public GameObject MenuBar;
    public GameObject MenuContent;
    public Text MenuTitle;
    public Text MenuTime;
    public Text menuMessage;
    public List<GameObject> SubMenus;
    public string firstMenu;
    public string activeMenu;
    private float pressTime;
    public AudioSource musicAudioSoure;
    public AudioSource buttonAudioSource;
    public AudioClip buttonHighlight;

    public Texture2D background;

    public List<string> campaigns;
    public List<string> campaignDescriptions;
    public List<string> mainMissionCampaigns;
    public List<string> mainMissionNames;
    public List<string> customMissionCampaigns;
    public List<string> customMissionNames;
    public List<Texture2D> backgroundPictures;

    public bool missionRunning;

    void Start()
    {
        //This adds the event system if it can't already find one in the game
        eventSystem = GameObject.FindFirstObjectByType<EventSystem>();
        InputSystemUIInputModule inputSystem = GameObject.FindFirstObjectByType<InputSystemUIInputModule>();

        if (eventSystem == null)
        {
            gameObject.AddComponent<EventSystem>();
        }

        if (inputSystem == null)
        {
            gameObject.AddComponent<InputSystemUIInputModule>();
        }

        //This loads the menu data. You may wish to call this function from another place
        MainMenuFunctions.LoadMenu();
    }

    void Update()
    {
        //This outputs the day and time on the menu
        if (MenuTime != null)
        {
            MenuTime.text = System.DateTime.Now.DayOfWeek.ToString() + " " + System.DateTime.Now.ToString("hh:mm");
        }

        GoBack();
    }

    //This allows you to "go back up the menu" by pressing the back button on the controller or by pressing escape
    private void GoBack()
    {
        Keyboard keyboard = Keyboard.current;
        Gamepad gamepad = Gamepad.current;

        if (keyboard != null)
        {
            if (keyboard.escapeKey.isPressed == true & pressTime < Time.time)
            {
                MainMenuFunctions.ActivateParentMenu();
                pressTime = Time.time + 0.25f;
            }
        }

        if (gamepad != null)
        {
            if (gamepad.bButton.isPressed == true & pressTime < Time.time)
            {
                MainMenuFunctions.ActivateParentMenu();
                pressTime = Time.time + 0.25f;
            }

            if (gamepad.circleButton.isPressed == true & pressTime < Time.time)
            {
                MainMenuFunctions.ActivateParentMenu();
                pressTime = Time.time + 0.25f;
            }
        }
    }
    
}
