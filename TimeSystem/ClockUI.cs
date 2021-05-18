using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/*
Purpose: This class starts and tracks the time of the current day. It then calls the dayReset function to reset the day and 
prepare the game for the next day. The dayReset function includes resetting important mechanics and displaying information 
regarding the player's actions during the day, such as the tasks completed, bills paid, etc.
Created: 10/2/2020 by Keshav Balaji 
Updated: 10/25/2020 by Keshav Balaji
Updated: 11/2/2020 by Benjamin Wagley
Updated: 11/19/2020 by Jonah Bui
Updated: 12/20/2020 by Jonah Bui (Added functions to indicate when a day ended, day began, and when in-game time has passed.)
Updated: 12/23/2020 by Jonah Bui (Added method to pause time while in things such as menus)
Updated: 12/24/2020 bu Jonah Bui (Added static version of displayTime that can be used by any class.)
Updated: 12/29/2020 by Keshav Balaji (Deleted DayReset script and moved its functions to this script)
*/

public class ClockUI : MonoBehaviour
{
    //This variable stores the textbox where the time is displayed
    public TextMeshProUGUI textDisplay;
    public TextMeshProUGUI mainTextDisplay; //added text variable for main UI 
    public TextMeshProUGUI dayTextSmallPhone;
    public TextMeshProUGUI dayTextMainScreen;
    public energy_system energySystem;
    public Test testSystem;
    public Interactor testSystemInteractor;     // Used to start a test if the player has not completed one by the end of the day
    public Player player;
    public GameObject playerSpawnPoint;
    public GameObject dayResetCanvas;

    public Canvas interactionCanvas;
    public TextMeshProUGUI interactionText;
    public Button button1;
    public Button button2;

    public Camera alternateCamera;
    public GameObject femaleCustomizer;
    public GameObject maleCustomizer;

    public bool chooseSex;
    public bool inEditor = false;

    [Header("Check this box to use the below field")]
    public bool desiredTimeUse;

    [Header("Format in 24-hour clock (ex: 15:00 = 3:00 PM)")]
    public string desiredTime;

    //This variable stores the current day number in the game
    [HideInInspector] public static int dayNo = 1;

    //This variable stores the current time of the day
    [HideInInspector] public float currentTime;

    //This variable stores the factor that determines how many ingame minutes equal one real-world minute
    [HideInInspector] public const float timeFactor = 16f;

    //This variable stores the starting time of each day of the game
    private float startTime = 32400f;

    //This variable stores the ending time of each day of the game
    private float endTime = 61201f;

    //This boolean variable controls the clock and checks if the day has begun
    [HideInInspector] public bool dayBegin = true;

    //This boolean variable controls the clock and checks if the day has ended
    private bool dayEnd = true;

    // Checks if the day has ended and the player is in the end of day menu and has not pressed space yet.
    public bool IsResetting { get; private set; } = false;

    public bool DayEnd { get => dayEnd; set => dayEnd = value; }

    private money_tracking_system moneyTracker;
    private float moneyEarned = 0f;
    private float moneySpent = 0f; 

    public bool sexChosen {get; private set;} = false;

    //
    private taskDatabase delObject;
    private SaveSystem saveObject;
    //

    public bool pauseTime { get; set; } = false;    // Used to check wheter the game time is paused or not
    private float tempTime;                         // Used to track when a single unit of time has passed.
    private float gameTimeScale;                    // Used to store the original time scale of the game. Need to set to 0 when pausing the game.
    private int messageCount = 0;
    private void Awake()
    {
        # if UNITY_STANDALONE && !UNITY_EDITOR
        inEditor = false;
        chooseSex = true;
        desiredTimeUse = false;
        # endif

        //
        delObject = FindObjectOfType<taskDatabase>();
        saveObject = FindObjectOfType<SaveSystem>();
        //

        moneyTracker = FindObjectOfType<money_tracking_system>();

        if(dayResetCanvas.activeSelf)
            dayResetCanvas.SetActive(false);

        currentTime = startTime;
        textDisplay.text = displayTime(currentTime);
        mainTextDisplay.text = displayTime(currentTime);
        
        if(inEditor)
        {
            if(chooseSex)
                ChooseSexEvent();
            else
                StartClockNewGame();
        }
    }

    //Start is called before the first frame update
    void Start()
    {
        if (GameEvents.Events)
        {
            GameEvents.Events.onGamePause += PauseTime;
            GameEvents.Events.onGamePlay += PlayTime;
        }
        if(SaveSystem.Instance)
        {
            if(chooseSex)
                SaveSystem.Instance.onNewGame += ChooseSexEvent;
            else
            {
                SaveSystem.Instance.onNewGame += StartClockNewGame;
                sexChosen = true;
            }
        }

        Time.timeScale = 1f;   // Needed. <- When loading a scene, time scale is screwed up. Need to restore it.
        gameTimeScale = Time.timeScale;
    }

     // Update is called once per frame and increments the time per frame
    void Update()
    {
        if (!dayEnd)
            ResumeClock();
    }

    private void OnDestroy()
    {
        if (GameEvents.Events)
        {
            GameEvents.Events.onGamePause -= PauseTime;
            GameEvents.Events.onGamePlay -= PlayTime; 
        }
        if (SaveSystem.Instance)
        {
            if (chooseSex)
                SaveSystem.Instance.onNewGame -= ChooseSexEvent;
            else
                SaveSystem.Instance.onNewGame -= StartClockNewGame;
        }
    }
    /// <summary>
    /// Pauses the game.
    /// </summary>
    private void PauseTime()
    {
        pauseTime = true;
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Plays the game. Unpauses it.
    /// </summary>
    private void PlayTime()
    {
        pauseTime = false;
        Time.timeScale = gameTimeScale;
    }

    /// <summary>
    /// Starts the clock on a new game.
    /// </summary>
    private void StartClockNewGame()
    {
        currentTime = (desiredTimeUse && !String.IsNullOrEmpty(desiredTime)) ? DesiredTime(desiredTime) : startTime;
        tempTime = currentTime;
        dayBegin = false;
        dayEnd = false;
        mainTextDisplay.color = textDisplay.color = Color.white;
        dayTextSmallPhone.text = dayTextMainScreen.text = "Day " + dayNo;
        delObject.createpackages(saveObject.IsNewGame);
        // Tells the game that a day has begun
        if (GameEvents.Events)
            GameEvents.Events.DayStart();
    }

    /// <summary>
    /// Starts the clock on a new day.
    /// </summary>
    private void StartClock()
    {
        currentTime = startTime;
        tempTime = currentTime;
        dayBegin = false;
        dayEnd = false;
        mainTextDisplay.color = textDisplay.color = Color.white;
        delObject.newDay();
        // Tells the game that a day has begun
        if (GameEvents.Events)
            GameEvents.Events.DayStart();
    }

    private void ChooseSexEvent()
    {
        StartCoroutine(ChooseSex());
    }

    private IEnumerator ChooseSex()
    {
        player.Interacting = true;
        interactionCanvas.enabled = true;

        interactionText.text = "Please choose the sex of the player (Male/Female).\nRight click the mouse to enable or disable the mouse cursor.";

        button1.GetComponent<Transform>().GetChild(0).GetComponent<TextMeshProUGUI>().text = "Female";
        button1.onClick.AddListener(SetFemale);

        button2.GetComponent<Transform>().GetChild(0).GetComponent<TextMeshProUGUI>().text = "Male";
        button2.onClick.AddListener(SetMale);

        yield return new WaitUntil(() => sexChosen);
        player.Interacting = false;
        StartClockNewGame();
    }

    public void SetMale()
    {
        player.SetSex(Sex.Male);

        femaleCustomizer.SetActive(false);
        if(!maleCustomizer.activeSelf)
            maleCustomizer.SetActive(true);
        
        player.transform.position = playerSpawnPoint.transform.position;
        player.transform.rotation = playerSpawnPoint.transform.rotation;
        interactionCanvas.enabled = false;
        sexChosen = true;
    }

    public void SetFemale()
    {
        player.SetSex(Sex.Female);

        maleCustomizer.SetActive(false);
        if(!femaleCustomizer.activeSelf)
            femaleCustomizer.SetActive(true);

        player.transform.position = playerSpawnPoint.transform.position;
        player.transform.rotation = playerSpawnPoint.transform.rotation;
        interactionCanvas.enabled = false;
        sexChosen = true;
    }

    /// <summary>
    /// This function continues the clock time.
    /// </summary>
    private void ResumeClock()
    {
        if (!pauseTime)
        {
            //This if statement checks if the time is between the start and end times for the day and if it isn't, then the next if statement is checked
            if (!(currentTime >= startTime && currentTime <= endTime))
            {
                //This statement checks if the current time of day has reached the ending time of the day
                if (currentTime >= endTime)
                {
                    if(!dayEnd)
                        resetClock();
                    else
                        return;
                }
            }

            displayTime();
            timeWarning();

            if (currentTime >= 39600f && messageCount == 0)
            {
                player.EnableMovement = false;
                PopUpMessage();
                messageCount++;
                player.EnableMovement = true;
            }
            else if (currentTime >= 54000f && messageCount == 1)
            { 
                player.EnableMovement = false;
                PopUpMessage();
                messageCount++;
                player.EnableMovement = true;
            }

            //Increments the current time per frame based on the time factor
            currentTime += Time.deltaTime * timeFactor;

            // Tells the game when time has passed.
            if (GameEvents.Events && currentTime > tempTime + 1f)
            {
                /* If the current time is greater than the previous recorded time + 1 unit of time, then
                 * we know specifically that a single unit of time has passed. As a result, tell the game
                 * that a single unit of time has passed. Update is always called every frame, but we don't
                 * want to call TimeUpdate every Update. Instead we'd call it every time a unit of time has 
                 * passed.*/
                tempTime = currentTime;
                GameEvents.Events.TimeUpdate(currentTime);
            }
        }
    }
    
    //This function displays the time on the screen.
    private void displayTime()
    {
        int minutes = (int) (currentTime / 60) % 60;
        int hours;

        if(currentTime >= 43200f && currentTime <= 46799f)
            hours = (int) ((currentTime / 3600) % 12) + 12;
        else
            hours = (int) (currentTime / 3600) % 12;

        string timeString = string.Format("{0:00}:{1:00}", hours, minutes);

        if (currentTime >= 32400f && currentTime <= 43199f)
        {
            textDisplay.text = timeString + " AM";
            mainTextDisplay.text = timeString + " AM";
        }
        else
        {
            textDisplay.text = timeString + " PM";
            mainTextDisplay.text = timeString + " PM";
        }

           // Added this to prevent the ClockUI breaking if the taskAssign fails.
        try
        {
               //task assign time
               //notice that this isn't an if-else structure, I'm trying to avoid entering the assignment check since it costs a lot of time
            if (taskDatabase.ezDelivery.head.isCompleted == false || taskDatabase.ezDelivery.head.next.isCompleted == false)
            {
                taskAssign(currentTime, taskDatabase.ezDelivery.head);
            }
            if (taskDatabase.meDelivery.head.isCompleted == false || taskDatabase.meDelivery.head.next.isCompleted == false)
            {
                taskAssign(currentTime, taskDatabase.meDelivery.head);
            }
            if (taskDatabase.hdDelivery.head.isCompleted == false)
            {
                taskAssign(currentTime, taskDatabase.hdDelivery.head);
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning("[Research] taskAssign throwing problems.\n" + e);
        }
    }
    
    public static string displayTime(float time)
    {
        int minutes = (int)(time / 60) % 60;
        int hours;

        if(time >= 43200f && time <= 46799f)
            hours = (int) ((time / 3600) % 12) + 12;
        else
            hours = (int) (time / 3600) % 12;

        string timeString = string.Format("{0:00}:{1:00}", hours, minutes);
        return (time >= 32400f && time <= 43199f) ? timeString += " AM" : timeString += " PM";
    }

       //task assignment function
    private void taskAssign(float currTime, taskDatabase.taskNode currHead)
    {        
        //Debug.Log("Assign: " + currHead.assignTime);
           //brute force through list
        for (int i = 0; i < 2; i++)
        {
            if (currTime >= currHead.assignTime && currHead.isActive == false && currHead.isCompleted == false)
            {
                //Debug.Log("currTime: " + currentTime);
                //Debug.Log("assignTime: " + currHead.assignTime);
                
                //Debug.Log("ASSIGNED");
                
                reActivate(currHead.pickUp.head);
                currHead.isActive = true;

                if(currHead.difficulty == 0 && i == 0)
                    Notification.Instance.PushNotification("Task 1 Assigned!");
                else if (currHead.difficulty == 0 && i == 1)
                    Notification.Instance.PushNotification("Task 2 Assigned!");
                else if (currHead.difficulty == 1 && i == 0)
                    Notification.Instance.PushNotification("Task 3 Assigned!");
                else if (currHead.difficulty == 1 && i == 1)
                    Notification.Instance.PushNotification("Task 4 Assigned!");
                else if (currHead.difficulty == 2 && i == 0)
                    Notification.Instance.PushNotification("Task 5 Assigned!");

                //Debug.Log("Did turn active?: " + currHead.isActive);
                return;
            }
               //if(currHead.expireTime)
            currHead = currHead.next;
        }
        
    }
    
    private void reActivate(taskDatabase.pickNode currHead)
    {
        for(int i = 0;i < 3;i++)
        {
            if(currHead.theBox.activeSelf == false)
            {
                //Debug.Log("ACTIVATED");
                currHead.theBox.SetActive(true);
            }
            if (currHead.next.theBox.activeSelf == true)
            {
                //Debug.Log("RETURNED");
                return;
            }
            currHead = currHead.next;
        }
    }

    //This function changes the color of the clock text as the current day nears its ending
    private void timeWarning()
    {
        //Changes the color of the text to orange 80 minutes before the end of the day (3:40 PM)
        if (currentTime >= 56400f && currentTime <= 59400f)
        {
            textDisplay.color = new Color32(255, 165, 0, 255);
            mainTextDisplay.color = new Color32(255, 165, 0, 255);
        }

        //Changes the color of the text to red 30 minutes before the end of the day (4:30 PM)
        if (currentTime >= 59400f && currentTime <= endTime)
        {
            textDisplay.color = new Color32(255, 0, 0, 255);
            mainTextDisplay.color = new Color32(255, 0, 0, 255);
        }
    }

    //This function resets the clock and prepares the game for the next day. It also calls the dayReset() function that displays the actions of the player during
    //the current day and moves the game to the next day.
    private void resetClock()
    {
        dayBegin = true;
        dayEnd = true;

        StartCoroutine(DayReset());
    }
    
    //This function calls the dayResetUI() function and resets important mechanincs of the game such as hunger, thirst, energy.
    //It also resets the player's test eligiblity and tracks and makes necessary changes to bills and tasks of the player.
    private IEnumerator DayReset()
    {
        IsResetting = true;

            //task management reset

        player.SetMovementType(MovementType.Idle);
        player.Interacting = true;

        // Needed so test can use the interface
        Notification.Instance.ForceClear();

        // Ensure mandatory test is taken
        if(!testSystem.dailyTestTaken)
        {
            // Pull up the test
            testSystemInteractor.OnInteractorStart(player, testSystem, PostEvent.Hide);

            yield return new WaitUntil(() => testSystemInteractor.finishedInteracting);
        }
        player.Interacting = true;

        dayResetCanvas.SetActive(true);
        DayResetUI();
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        dayResetCanvas.SetActive(false);
        messageCount = 0;
        IsResetting = false;

        dayNo++;
        dayTextSmallPhone.text = dayTextMainScreen.text = "Day " + dayNo;

        //Set the energy for the player
        energySystem.foodAmount = 50f;
        energySystem.liquidAmount = 50f;

        StartClock();
        
        player.Interacting = false;
        player.transform.position = playerSpawnPoint.transform.position;
    }

    //This function will display important information at the end of the current play day. For example, if the player just finished Day 1, this function will display
    //information such as money earned, bills not paid, tasks not completed, and so on.
    private void DayResetUI()
    {
        TextMeshProUGUI titleText = dayResetCanvas.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI bodyText = dayResetCanvas.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();

        titleText.text = "Day " + dayNo + " has ended!";

        if(dayNo == 1)
        {
            moneyEarned += moneyTracker.amountEarned;
            moneySpent += moneyTracker.amountSpent;

            bodyText.text = "Money Earned on Day " + dayNo + ": " + moneyEarned.ToPrice(); 
            bodyText.text += "\nMoney Spent on Day " + dayNo + ": " + moneySpent.ToPrice();
            bodyText.text += "\nTotal Money Earned: " + moneyTracker.amountEarned.ToPrice();
            bodyText.text += "\nTotal Money Spent: " + moneyTracker.amountSpent.ToPrice();
        }
        else
        {
            bodyText.text = "Money Earned on Day " + dayNo + ": " + (moneyTracker.amountEarned - moneyEarned).ToPrice(); 
            bodyText.text += "\nMoney Spent on Day " + dayNo + ": " + (moneyTracker.amountSpent - moneySpent).ToPrice();
            bodyText.text += "\nTotal Money Earned: " + moneyTracker.amountEarned.ToPrice();
            bodyText.text += "\nTotal Money Spent: " + moneyTracker.amountSpent.ToPrice();

            moneyEarned += moneyTracker.amountEarned - moneyEarned;
            moneySpent += moneyTracker.amountSpent - moneySpent;
        }
    }

    public float DesiredTime(string time)
    {
        string[] timeDivided = time.Split(':');
        float hour = float.Parse(timeDivided[0]);
        float minutes = float.Parse(timeDivided[1]);

        return (hour >= 1 && hour <=5) ? ((hour+12) * 3600f) + (minutes * 60f) : (hour * 3600f) + (minutes * 60f);
    }
    
    private void PopUpMessage()
    {
        Notification.Instance.PushNotificationWithButton("Earn more. Spend less.\n(Right click the mouse to enable or disable the mouse cursor.)", "I understand");
    }
}