using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class Tutorial : MonoBehaviour{

    public GameObject tutorialWindow;
    public Text message;

    public int tutorialStep = 0;
    public List<string> messages;

    public Factory [] factories;
    private Player player;
    private Storage storage;
    private Warehouse warehouse;
    private Parachutes parachutes;
    private GameManager gameManager;
    private Research research;

    [Header("Info Btn")]
    public Animation infoBtnAnim;
    public static bool infoBtnClicked = false;

    [Header("animations")]
    public Animation sugarBtnAnim;

    public Animation employeeBtnAnim;



    [Header ("Parachute control")]
    public bool parachuteLaunched = false;
    public bool waitingForParachute = true;

    [Header("bottleneck")]
    public bool bottleNeckStarted = false;
    public bool showBottleNeckMsg = false;

    [Header("things to enable and disable")]
    public Button infoBtn;
    public Button getSugarBtn;
    public GameObject goUpBtn;
    public TouchCam touchCamScript;
    public GameObject dandy;
    public BoxCollider parachuteCollider;

    private void Start(){

        tutorialStep = PlayerPrefs.GetInt("TUTORIAL",0);

        player = FindObjectOfType<Player>();
        storage = FindObjectOfType<Storage>();
        warehouse = FindObjectOfType<Warehouse>();
        parachutes = FindObjectOfType<Parachutes>();
        gameManager = FindObjectOfType<GameManager>();
        research = FindObjectOfType<Research>();

    }

    IEnumerator LaunchParachute() {

        print("parachute coroutine");

        parachuteLaunched = true;

        yield return new WaitForSeconds(15);

        print("parachute launched");
        parachuteCollider.enabled = false;

        parachutes.isMoving = true;

        yield return new WaitForSeconds(2);

        waitingForParachute = false;



    }

    IEnumerator BottleNeck() {

        print("bottleneck coroutine started");

        bottleNeckStarted = true;
        yield return new WaitForSeconds(30);

        ShowWindow();

    }

    public void InfoButtonFirstPress() {

        infoBtnClicked = true;
    }

    private void Update(){

        if (tutorialStep >= 10 && !parachutes.isOn)
            parachutes.isOn = true;

        if(tutorialStep == 10)
            parachuteCollider.enabled = true;

        // activate info button after STEP 12
        if (tutorialStep >= 12)
            infoBtn.interactable = true;
        else
            infoBtn.interactable = false;

        // activate UP Btn NAD CAM MOVING SCRIPT after purchasing second factory
        if (tutorialStep >= 13 && goUpBtn.activeSelf == false){

            goUpBtn.SetActive(true);
            touchCamScript.enabled = true;
        }

        else if (tutorialStep < 13) {

            goUpBtn.SetActive(false);
            touchCamScript.enabled = false;
        }

        // activate get sugar btn when enough money

        if (tutorialStep < 15)
            getSugarBtn.interactable = false;

        else if (tutorialStep == 15 && gameManager.SugarExchange() >= research.EmployeeCost()){

            getSugarBtn.interactable = true;
            sugarBtnAnim.Play("SugarBtnTilt");
        }
        else if( tutorialStep >15)
            getSugarBtn.interactable = true;


        // ACTIVATE RESEARCH FACILITY and DANDY

        if (tutorialStep < 17 && dandy.activeSelf == true){

            dandy.SetActive(false);
            research.GetComponent<BoxCollider>().enabled = false;

        }
        else if (tutorialStep >= 17 && dandy.activeSelf == false){

            dandy.SetActive(true);
            research.GetComponent<BoxCollider>().enabled = true;

        }

        if (research.employeesLvl == 0)
            employeeBtnAnim.Play("employeeBtnTilt");
        else
            employeeBtnAnim.Stop();

        if (tutorialStep < 9)
            parachutes.isOn = false;
            


        // SHOW FIRST MESSAGE ON START
        if (tutorialStep == 0)
            ShowWindow();

        // SHOW SECOND MESSAGE RIGHT AFTER FIRST ONE
        if (tutorialStep == 1) 
            ShowWindow();

        // SHOW THIRD MESSAGE AFTER PURCHASING FIRST FACTORY
        if (tutorialStep == 2 && factories[0].isActive) 
            ShowWindow(); 
        
        // SHOW 4th MESSAGE AFTER FEEDING FIRST FACTORY
        if (tutorialStep == 3 && factories[0].isFilled) 
            ShowWindow();
        
        // SHOW 5th MESSAGE AFTER STORING IN FRIDGE
        if (tutorialStep == 4 && factories[0].fridge.stored == 1)
            ShowWindow();

        // SHOW 6th MESSAGE AFTER STORING ONE IN STORAGE
        if (tutorialStep == 5 && storage.stored == 1)
            ShowWindow();

        // SKIP this
        // SHOW 7th MESSAGE AFTER STORING 5 IN STORAGE
        //if (tutorialStep == 6 && storage.stored == 5)
        //    ShowWindow();

        if (tutorialStep == 6)
            tutorialStep ++;

        // SHOW 8th MESSAGE AFTER STORING ONE IN WAREHOUSE
        if (tutorialStep == 7 && warehouse.stored == 1)
            ShowWindow();

        // SHOW 9th MESSAGE AFTER SELLING THE FIRST BOX
        if (tutorialStep == 8 && player.soldBoxes >= 1)
            ShowWindow();

        // WAIT 30 seconds and relase a parachute
        if (tutorialStep == 9 && !parachuteLaunched) 
            StartCoroutine("LaunchParachute");      

        // THE PARACHUTE IS LAUNCHED; SHOW 10TH MESSAGE FOR CLICKING IT
        if (tutorialStep == 9 && !waitingForParachute)
            ShowWindow();

        // SHOW 11 TH MESSAGE AFTER CLICKING PARACHUTE
        if (tutorialStep == 10 && player.parachutes >0)
            ShowWindow();

        // SHOW 12 th MESSAGE PURCHASING SECOND FACTORY NAD START TILTING THE INFO BTN
        if (tutorialStep == 11 && factories[1].isActive){

            if(!infoBtnClicked)
            infoBtnAnim.Play("InfoBtnTilt");

            ShowWindow();
        }
            

        // SHOW 13TH message right after
        if (tutorialStep == 12 && infoBtnClicked) {

            infoBtnAnim.Stop();
            ShowWindow();
        }
            

        // BOTTLE NECK MSG SHOWS FROM COROUTINE
        if (tutorialStep == 13 && !bottleNeckStarted)
            StartCoroutine("BottleNeck");

        // SHOW 13TH message right after
        if (tutorialStep == 14)
            ShowWindow();

        // MESSAGE 14 TO ACTIVATE GET SUGAR
        if (tutorialStep == 15 && gameManager.SugarExchange() >= research.EmployeeCost())
            ShowWindow();

        // SHOW 15TH message right after (factory reset)
        if (tutorialStep == 16 && player.sugar > 0)
            ShowWindow();

        // ACTIVATE RESEARCH FACILITY (SHOW RIGHT AFTER)
        if (tutorialStep == 17)
            ShowWindow();

        // FISRT EMPLOYEE PURCHASED
        if (tutorialStep == 18 && player.sleepTimes >= 1)
            ShowWindow();

        // FINAL TUTORIAL WINDOW ??
        if (tutorialStep == 19)
            ShowWindow();

    }

    public void ShowWindow(){

        tutorialWindow.SetActive(true);
        message.text = messages[tutorialStep];
        Time.timeScale = 0;
    }

    public void HideWindow(){

        tutorialWindow.SetActive(false);
        Time.timeScale = 1;
        tutorialStep ++;

        PlayerPrefs.SetInt("TUTORIAL", tutorialStep);
    }

    // ADD  THE MESSAGES FOR THE TUTORIAL
    private void Add(string msg) {

        messages.Add(msg);
    }

    void Awake(){

        // TS 0 Msg 1  WELCOME MESSAGE
        Add("Hi! im Dandy. Welcome to candy Factory!");

        // TS 1 Msg 2  RIGHT AFTER WELCOME, PURCHASE FIRST LINE
        Add("Lets buy the first factory Line. TAP and HOLD to unlock it.");

        // TS 2 Msg 3  FEED FIRST LINE
        Add("Great, now tap the button to dispatch the raw materials.");

        // TS 3 Msg 4  PRODUCE FIRST CANDY
        Add("Now, Tap to dispatch those candies.");

        // TS 4 Msg 5  CANDIES IN FRIDGE
        Add("Lets wait for the elevator to pick them up.");

        //  TS 5 Msg 6  CANDIES IN STORAGE
        Add("Time to put it in a box.");

        //  TS 6 Msg 7  - 5 CANDIES IN STORAGE
        Add("Time to put it in a box.");

        // TS 7 Msg 8   BOX IN WAREHOUSE 
        Add("Last step, sell and profit! TAP the sell button in the truck.");

        // TS 8 Msg 9   BOX SOLD
        Add("Great! that was easy. Now lets get money to unlock the second production Line.");

        // TS 9 Msg 10  PARACHUTE BOX FALLS
        Add("Look! a box is falling! Quick, TAP it to get a reward!");

        //  TS 9 Msg 11  PARACHUTE CLICKED
        Add("Awesome! we got a box of candies, that helps!");

        // TS 10 Msg 12  SECOND FACTORY PURCHASED ( activate info btn)
        Add("Yeah! we are growing. I will unlock the info button in the top left. why dont you take a look?");

        // TS 11 Msg 13  INSTANTLY AFTER SECOND PURCHASE (activate cam script)
        Add("Great, now i will unlock the rest of the factory for you. You can SLIDE to move the camera around.");

        // TS 12 msg 14 UPGRADE ELEVATOR!

        Add("You should try making the elevator faster. TAP and HOLD it to see the upgrade window.");

        //TS 13 msg 15 GENERAL UPGRADES
        Add("All the active components in the factory can be upgraded too. Detect slow spots and give them a boost!");

        // TS 13 Msg 15  ACTIVATE GET SUGAR BTN
        Add("Time to get a special currency, SUGAR! tap the Get Suggar button in the upper right corner to see more.");

        // TS 14 Msg 16  ACTIVATE RESEARCH FACILITY after GETTING SUGAR
        Add("Our factories are blocked again, but dont worry! Meet me in the research facility.");

        // TS 15 Msg 17  ACTIVATE RESEARCH FACILITY after GETTING SUGAR
        Add("Here you can Buy a lot of permanent upgrades, TAP and HOLD on me in the research facility and get your first worker!");

        // TS 16 Msg 18  ACTIVATE RESEARCH FACILITY after GETTING SUGAR
        Add("Our workers tend to fall asleep! TAP them to awake. You can also reduce sleep chance in the research facility.");

        // TS 17 Msg 19 FINAL TUTORIAL WINDOW ??
        Add("Thats all manager, Go there and have fun!");

        /*

        // TS 0 Msg 1  WELCOME MESSAGE
        Add("Welcome to CANDY FACTORY, manager! My name is Dandy and im here to help you turn this " +
            "little fabric into the world´s best Candy Factory! to do so, we need to first produce " +
            "all the candies we can and sell them to get lots of money, we will be growing bigger " +
            "faster and better on each run. Lets get started!");

        // TS 1 Msg 2  RIGHT AFTER WELCOME, PURCHASE FIRST LINE
        Add("The first thing we need to do is to stablish a production line. This will take the raw " +
            "materials and produce sweet, delicious candies! TAP and HOLD on the first padlock to " +
            "unlock it." );

        // TS 2 Msg 3  FEED FIRST LINE
        Add("Excelent! the first line is active and waiting for materials to transform into candies. " +
            "You will notice a dispatch button went active in the raw materials section, TAP it to " +
            "feed this production line.");

        // TS 3 Msg 4  PRODUCE FIRST CANDY
        Add("Great! the production line will now take the raw materials and transform them into candies! " +
            "when that happens, a dispatch button will pop up, TAP it to send the production to the next " +
            "stage.");

        // TS 4 Msg 5  CANDIES IN FRIDGE
        Add("Good Job! the candies are now stored in the fridge, and will wait there until the elevator " +
            "picks them up to the next stage in the factory.");

        //  TS 5 Msg 6  CANDIES IN STORAGE
        Add("Good! our candy is now safe in the storage area, now we need to pack them in a box in the next " +
            "stage, the packing machine.");

        //  TS 6 Msg 7  - 5 CANDIES IN STORAGE
        Add("Great! now we have enough candies to pack them in a box. and send them up to the warehouse, " +
            "keep up the good job manager.");

        // TS 7 Msg 8   BOX IN WAREHOUSE 
        Add("Our first box is now stored in the warehouse, this building will store our production and " +
            "fill our truck to be sold. A sell button will pop up whenever the truck has some boxes, now " +
            " TAP the sell button to earn the profit");

        // TS 8 Msg 9   BOX SOLD
        Add("Incredible! we took raw materials and turn them into the most delicious candies in the world, " +
            "and we also got some money in the process. The faster we can produce the more we can sell, so " +
            "lets pack more boxes and get enough money to unlock the next production line. Twice the production, " +
            "double the money!");

        // TS 9 Msg 10  PARACHUTE BOX FALLS
        Add("Look! a box is falling! Quick, TAP it to get a reward!");

        //  TS 9 Msg 11  PARACHUTE CLICKED
        Add("Awesome! We got a bit of help to unlock our next production line faster. Boxes like this may fall " +
            "randomly, be aware and tap them to collect awesome rewards.");

        // TS 10 Msg 12  SECOND FACTORY PURCHASED ( activate info btn)
        Add("Wow, our factory is already growing. With two production lines we can produce twice as fast " +
            "Keep up the good job. TIP: in the upper left corner theres an info window, there you can see " +
            "a lot of usefull window about our factory, including the total money We have earned.");

        // TS 11 Msg 13  INSTANTLY AFTER SECOND PURCHASE (activate cam script)
        Add("We have a total of 12 production lines, you can tap and swipe to see the rest of the factory. " +
            "Once you are down you can tap on the lower left button to return fast to the top of the factory. " +
            "Lets keep making some money to unlock a new cool feature.");

        // TS 12 msg 14 UPGRADE ELEVATOR!

        Add("Manager, i noticed we have a bottle neck in our factory, the candies in our second production " +
            "line are not beign picked as fast as they should. Why dont you give a boost to the elevator " +
            "sppeed to fix this? TAP and HOLD the elevator and give it a couple upgrades.");

        //TS 13 msg 15 GENERAL UPGRADES
        Add("Part of managing a Candy Factory like this is to spot and fix bottle necks like this by upgrading " +
            "the problematic sections, for example, if you TAP and hold on a FRIDGE you will notice it can hold " +
            "up to 5 candies at a time, if you send more than that it will be lost. You can see how many candies " +
            "you have dropped in the INFO window.");

        // TS 13 Msg 15  ACTIVATE GET SUGAR BTN
        Add("Wow, we have earned a lot of money now, you can check in the info window the total ammount of money " +
            "we have earned. We should rise the production already. First, go to the GET SUGAR button in the upper " +
            "right of the screen and lets exchange the total money we earned so far for a new powerfull currency: " +
            "SUGAR!");

        // TS 14 Msg 16  ACTIVATE RESEARCH FACILITY after GETTING SUGAR
        Add("Awesome! you will notice a few things, our factories are blocked again, but dont worry! we got enough " +
            "SUGAR to buy upgrades and research technologies that will help us grow faster. I will go to the research " +
            "facility and help you from there. ");

        // TS 15 Msg 17  ACTIVATE RESEARCH FACILITY after GETTING SUGAR
        Add("The research facility is a very important building for our factory, here you can buy upgrades like " +
            "employees for all your factories, that way you dont have to tap each time you need to dispatch raw " +
            "materials or candies. Now, TAP and HOLD on me in the research facility and get your first worker!");

        // TS 16 Msg 18  ACTIVATE RESEARCH FACILITY after GETTING SUGAR
        Add("Well done! workers are a big help to boost production, we cant run a huge Candy Factory just " +
            "by ourselves anyway right? Theres a small drawback, our workers tend to fall asleep! but dont worry, " +
            "you can tap them to get them working again. Also, you can reduce their sleep chance in the research " +
            "facility, you just need some SUGAR for it.");

        // TS 17 Msg 19 FINAL TUTORIAL WINDOW ??
        Add("By the way, you dont lose the upgrades purchased in the research Facility after getting candies, so " +
            "dot worry for it. Well, i think you already know the basics to turn this fabric into the world´s " +
            "biggest Candy Factory, manager! From now its up to you. Go there and make me proud!");

    */
    }

}
