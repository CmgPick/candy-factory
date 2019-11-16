using UnityEngine;
using UnityEngine.UI;

public class Factory : MonoBehaviour{

    public double purchaseCost;
    public bool isActive = true;
    public int index;
    public int candyType;
    public GameObject notActiveVeil;
    public CandyPile sample;
    public Text purchaseCostTxt;

    public float fabricTime = 2f;
    private float fabricTimer;
    public bool readyToDispatch = false;
    public bool isSleeping = false;

    public bool isFilled = false;
    public bool isQueued = false;

    public bool checkFridgeSpace = true;
    public bool fridgeHasSpace = false;

    [Header("GameObjects")]
    public GameObject inputPrefab;
    public GameObject outputPrefab;

    public GameObject inputObject;
    public GameObject outputObject;


    [Header("FillBar")]
    public float barSizeMultiplier;
    public SpriteRenderer fillBar;

    [Header("positions")]
    public Transform doughHeight;
    public Transform doughSpawnPoint;
    public Transform doughIntakePoint;
    public Transform outputSpawnPoint;
    public Transform outputEndPoint;
    public Transform outputFallPos;
    public Transform elevatorPos;

    [Header("values")]
    public float beltSpeed = 1f;

    [Header("LevelUps")]

    public int level = 1;
    public int levelCost = 500;
    public float initialSpeed = 2f;
    public bool hasEmployee = false;

    public GameObject employee;
    public GameObject employeeSleeping;

    public GameObject dispatchBtn;


    public Fridge fridge;
    private Player player;
    private Research research;

    private void Awake(){

        research = FindObjectOfType<Research>();
        player = FindObjectOfType<Player>();
    }

    void Start(){

        dispatchBtn.SetActive(false);
        //transform.Find("machine").GetComponent<BoxCollider>().enabled = false;
    }

    public void SetFridge() {

        fridge.candyType = candyType;
        fridge.SetCandyType();

    }

    public void SetSample() {

        sample.SetCandyType(candyType);
        sample.GetComponent<SpriteRenderer>().enabled = false;

    }

    public void SpwanFabricDough() {

        inputObject = Instantiate(inputPrefab, doughSpawnPoint.position, Quaternion.identity);
    }

    public void SpawnOutput() {

        sample.GetComponent<SpriteRenderer>().enabled = false;

        readyToDispatch = false;
        isFilled = false;
        fabricTimer = 0;

        outputObject = Instantiate(outputPrefab, outputSpawnPoint.position, Quaternion.identity);
        outputObject.GetComponent<CandyPile>().SetCandyType(candyType);

        player.CandyProduced();

        if(hasEmployee && isActive)
        SleepChance();
    }

    public void SleepChance(){

        float chance = Random.Range(0f, 100f);

        if (chance <= research.SleepChance()){

            employeeSleeping.SetActive(true);
            isSleeping = true;
            player.Sleeps();
        }
    }

    //ACTIVATES PURCHASED EMPLOYEES
    public void ActivateEmployee() {

        if (research.employeesLvl > index + 1 && isActive){

            hasEmployee = true;
            employee.SetActive(true);

        }
    }

    // AWAKES SLEEPIMG EMPLOYEE
    public void AwakeEmployee(){

        isSleeping = false;
        employeeSleeping.SetActive(false);

    }

    public void AlreadyPurchased() {

        isActive = true;
        //transform.Find("machine").GetComponent<BoxCollider>().enabled = true;
        notActiveVeil.SetActive(false);

    }

    public void PurchaseFactory() {

        // FIX save the purchased state

        if(player.money >= purchaseCost) {

            isActive = true;
            notActiveVeil.SetActive(false);
            player.SpendMoney(purchaseCost);
            //transform.Find("machine").GetComponent<BoxCollider>().enabled = true;

            PlayerPrefs.SetInt("FACTORY" + index + "ACTIVE", 1);
        }

    }

    // Update is called once per frame
    void FixedUpdate(){

        if (research.employeesLvl > index + 1 && isActive)
            ActivateEmployee();
        

            if (fabricTimer < fabricTime && isFilled && !isSleeping)
            fabricTimer += Time.deltaTime;

        // enable button only if theres no employee
        if (readyToDispatch && !hasEmployee){

            dispatchBtn.SetActive(true);
            return; 
        }
        else
            dispatchBtn.SetActive(false);

        // Fill the completion bar
        float percent = fabricTimer / fabricTime;
        fillBar.size = new Vector2(percent * barSizeMultiplier, 1);

        // Dispatch automatically if has an employee that is not sleeping
        if (readyToDispatch && hasEmployee && !isSleeping){

            SpawnOutput();

        }

        if (fabricTimer >= fabricTime ){

            //isFilled = false;
            //SpawnOutput();
            //fabricTimer = 0;

            if (hasEmployee && !isSleeping)
                readyToDispatch = true;
            else if(!hasEmployee)
                readyToDispatch = true;

        }

        // INPUT DOUGH

        if (inputObject != null)
            inputObject.transform.Translate(beltSpeed * Time.deltaTime, 0, 0);

        if (inputObject != null && inputObject.transform.position.x >= doughIntakePoint.position.x) {

            sample.GetComponent<SpriteRenderer>().enabled = true;

            isFilled = true;
            isQueued = false;
            Destroy(inputObject);
            inputObject = null;
        }

        //OUTPUT

        if (outputObject != null)
            outputObject.transform.Translate(beltSpeed * Time.deltaTime, 0, 0);

        if (outputObject != null && outputObject.transform.position.x >= outputEndPoint.position.x && checkFridgeSpace){

            fridgeHasSpace = fridge.stored < fridge.maxStorage ? true : false;
            checkFridgeSpace = false;
        }
        

        if (outputObject != null && outputObject.transform.position.x >= outputEndPoint.position.x && fridgeHasSpace && !checkFridgeSpace){

            
            Destroy(outputObject);
            outputObject = null;

            fridge.Store();
            checkFridgeSpace = true;
        }

        // fall to ground if no space
        if (outputObject != null && outputObject.transform.position.x >= outputEndPoint.position.x && !fridgeHasSpace && !checkFridgeSpace) {

            outputObject.transform.Translate(0, -beltSpeed * 1.5f * Time.deltaTime, 0);
        }

        if (outputObject != null && outputObject.transform.position.x >= outputEndPoint.position.x &&
            outputObject.transform.position.y <= outputFallPos.position.y){


            checkFridgeSpace = true;

            Destroy(outputObject);
            outputObject = null;

            player.CandyDropped();
        }
    }


}
