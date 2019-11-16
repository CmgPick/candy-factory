using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour{

    public double money;
    public double sugar;
    public double totalMoney;

    public int producedCandies;
    public int droppedCandies;
    public int soldBoxes;
    public int droppedBoxes;
    public int sleepTimes;
    public int parachutes;
    public int restarts;

    //UI
    public Text moneyTxt;
    public Text sugarTxt;

    public GameObject infoWindow;
    public Text producedCandiesTxt;
    public Text droppedCandiesTxt;
    public Text soldBoxesTxt;
    public Text droppedBoxesTxt;
    public Text infoMoneyTxt;
    public Text infoTotalMoneyTxt;
    public Text sleepTimesTxt;
    public Text totalIncreaseTxt;
    public Text salePriceTxt;
    public Text parachutesTxt;
    public Text restartTxt;

    private NumericControl numericControl;
    private MouseController mouseController;
    private GameManager gameManager;

    //buildings
    private Tolva tolva;
    private Packing packing;
    private Warehouse warehouse;
    private Truck truck;
    private Storage storage;
    private Elevator elevator;
    private FactoryWindow factoryWindow;
    private FridgeWindow fridgeWindow;
    private Research research;


    //DEBUG
    private bool fastForward = false;

    private void Start(){

        tolva = FindObjectOfType<Tolva>();
        packing = FindObjectOfType<Packing>();
        warehouse = FindObjectOfType<Warehouse>();
        truck = FindObjectOfType<Truck>();
        storage = FindObjectOfType<Storage>();
        elevator = FindObjectOfType<Elevator>();
        research = FindObjectOfType<Research>();

        factoryWindow = FindObjectOfType<FactoryWindow>();
        fridgeWindow = FindObjectOfType<FridgeWindow>();
        gameManager = FindObjectOfType<GameManager>();
        mouseController = FindObjectOfType<MouseController>();
        numericControl = FindObjectOfType<NumericControl>();

        LoadAllValues();
        UpdateMoneyText();
    }

    public void FastForward() {

        fastForward = !fastForward;

        if (fastForward)
            Time.timeScale = 4;
        else
            Time.timeScale = 1;

    }

    private void LoadAllValues() {

        restarts = int.Parse(PlayerPrefs.GetString("RESTARTS", "0"));
        producedCandies = int.Parse(PlayerPrefs.GetString("PRODUCED", "0"));
        droppedCandies = int.Parse(PlayerPrefs.GetString("DROPPED", "0"));
        soldBoxes = int.Parse(PlayerPrefs.GetString("SOLD", "0"));
        droppedBoxes = int.Parse(PlayerPrefs.GetString("BOXES", "0"));
        parachutes = int.Parse(PlayerPrefs.GetString("PARACHUTES", "0"));

        //money = double.Parse(PlayerPrefs.GetString("MONEY", "0")); // COMMENT TO DISABLE MONEY LOADING

        totalMoney = double.Parse(PlayerPrefs.GetString("TOTALMONEY", "0"));
        sugar = double.Parse(PlayerPrefs.GetString("SUGAR", "0"));

        sleepTimes = int.Parse(PlayerPrefs.GetString("SLEEP", "0"));

        research.employeesLvl = PlayerPrefs.GetInt("EMPLOYEE", 0);
        research.employeeSleepLvl = PlayerPrefs.GetInt("EMPLOYEESLEEP", 0);
        //research.candyTypeLvl = PlayerPrefs.GetInt("CANDY", 0); // DO THIS FROM GAMEMANAGER ON AWAKE

        research.sweetnessLvl = PlayerPrefs.GetInt("SWEETNESS", 0);
        research.flavorLvl = PlayerPrefs.GetInt("FLAVOR", 0);
        research.qualityLvl = PlayerPrefs.GetInt("QUALITY", 0);
        research.marketingLvl = PlayerPrefs.GetInt("MARKETING", 0);

        // buildings

        tolva.LevelUp(PlayerPrefs.GetInt("TOLVALVL", 0).ToString());
        packing.LevelUp(PlayerPrefs.GetInt("PACKINGLVL", 0).ToString());
        warehouse.LevelUp(PlayerPrefs.GetInt("WAREHOUSELVL", 0).ToString());
        truck.LevelUp(PlayerPrefs.GetInt("TRUCKLVL", 0).ToString());
        storage.LevelUp(PlayerPrefs.GetInt("STORAGELVL", 0).ToString());
        elevator.LevelUp(PlayerPrefs.GetInt("ELEVATORLVL", 0).ToString());

        for (int i = 0; i < tolva.factories.Length; i++){

            factoryWindow.targetFactory = tolva.factories[i];
            factoryWindow.LevelUp(PlayerPrefs.GetInt("FACTORY" + tolva.factories[i].index + "LVL",0).ToString());
            //factoryWindow.EmployeeLevelUp(PlayerPrefs.GetInt("FACTORY" + tolva.factories[i].index + "EMPLOYEE",0).ToString());

            fridgeWindow.targetFridge = tolva.factories[i].fridge;
            fridgeWindow.LevelUp(PlayerPrefs.GetInt("FRIDGE" + i + "LVL", 0).ToString());
        }

        ActivateAllEmployees();

    }

    // ACTIVATES ALL PURCHASED EMPLOYEES
    public void ActivateAllEmployees() {

        tolva.ActivateEmployee();

        for (int i = 0; i < tolva.factories.Length; i++) 
            tolva.factories[i].ActivateEmployee();

    }

    // Kepps A record of produced candies
    public void CandyProduced() {

        producedCandies = int.Parse(PlayerPrefs.GetString("PRODUCED","0"));
        producedCandies ++;
        PlayerPrefs.SetString("PRODUCED", producedCandies.ToString());
    }

    // Kepps A record of dropped candies
    public void CandyDropped(){

        droppedCandies = int.Parse(PlayerPrefs.GetString("DROPPED","0"));
        droppedCandies++;
        PlayerPrefs.SetString("DROPPED", droppedCandies.ToString());
    }

    // Kepps A record of sold boxes
    public void BoxesSold(int num){

        soldBoxes = int.Parse(PlayerPrefs.GetString("SOLD", "0"));
        soldBoxes += num;
        PlayerPrefs.SetString("SOLD", soldBoxes.ToString());
    }

    // Kepps A record of dropped boxes
    public void BoxesDropped(){

        droppedBoxes = int.Parse(PlayerPrefs.GetString("BOXES", "0"));
        droppedBoxes ++;
        PlayerPrefs.SetString("BOXES", droppedBoxes.ToString());
    }

    // Kepps A record of Sleeping employees 

    public void Sleeps() {

        sleepTimes = int.Parse(PlayerPrefs.GetString("SLEEP", "0"));
        sleepTimes ++;
        PlayerPrefs.SetString("SLEEP", sleepTimes.ToString());
    }

    // Kepps A record of clicked Parachutes

    public void Parachute(){

        parachutes = int.Parse(PlayerPrefs.GetString("PARACHUTES", "0"));
        parachutes ++;
        PlayerPrefs.SetString("PARACHUTES", parachutes.ToString());
    }

    // Kepps A record of RESTARTS

    public void Restart(){

        restarts = int.Parse(PlayerPrefs.GetString("RESTARTS", "0"));
        restarts ++;
        PlayerPrefs.SetString("RESTARTS", restarts.ToString());
    }

    // MONEY
    public void GetMoney(double ammount) {

        money += ammount;
        totalMoney += ammount;
        UpdateMoneyText();

    }
    //MONEY
    public void SpendMoney(double ammount) {

        if(money >= ammount){

            money -= ammount;
            UpdateMoneyText();
        }

        if (money < 0) {

            money = 0;
            UpdateMoneyText();
        }

    }

    // SUGAR
    public void GetSugar(double ammount){

        sugar += ammount;
        UpdateMoneyText();

    }
    // SUGAR
    public void SpendSugar(double ammount){

        if (sugar >= ammount){

            sugar -= ammount;
            UpdateMoneyText();
        }

        if (sugar < 0){

            sugar = 0;
            UpdateMoneyText();
        }

    }

    private void UpdateMoneyText(){

        moneyTxt.text = numericControl.StringNumber(money);
        sugarTxt.text = numericControl.StringNumber(sugar);

        PlayerPrefs.SetString("SUGAR", sugar.ToString());
        PlayerPrefs.SetString("MONEY", money.ToString());
        PlayerPrefs.SetString("TOTALMONEY", totalMoney.ToString());
    }

    public void UpdateWindowValues() {

        producedCandiesTxt.text = "Total candies: " + producedCandies;
        droppedCandiesTxt.text = "Dropped candies: " + droppedCandies;
        soldBoxesTxt.text = "Sold boxes: " + soldBoxes;
        droppedBoxesTxt.text = "Dropped boxes: " + droppedBoxes;
        infoMoneyTxt.text = "Money: " + numericControl.StringNumber(money);
        infoTotalMoneyTxt.text = "Total sales: " + numericControl.StringNumber(totalMoney);
        sleepTimesTxt.text = "Sleeping employees: " + sleepTimes;

        //goalTxt.text = "Stage goal: " + numericControl.StringNumber(gameManager.currentGoal);
        //stageTxt.text = "Stage: " + gameManager.stage;

        restartTxt.text = "Total restarts: " + restarts;
        parachutesTxt.text = "Clicked Parachutes: " + parachutes;

        //float completion = (float)(money / gameManager.currentGoal) *100;
        //goalCompletionTxt.text = "Goal " + completion.ToString("F2") + " % completed";

        totalIncreaseTxt.text = "Total price increase " + research.TotalIncrease() + " %";
        salePriceTxt.text = "Current sale price " + gameManager.SalePrice();

    }

    public void ToogleInfoWindow() {

        UpdateWindowValues();
        infoWindow.SetActive(!infoWindow.activeSelf);

    }

}
