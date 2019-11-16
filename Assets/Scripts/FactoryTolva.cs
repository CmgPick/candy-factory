using UnityEngine;
using UnityEngine.UI;

public class FactoryTolva : MonoBehaviour
{


    public float fillTime = 2f;
    private float fillTimer;

    public bool readyToDispatch = false;
    public bool isSleeping = false;

    [Header("FillBar")]
    public float barSizeMultiplier;
    public SpriteRenderer fillBar;

    [Header("Fabrics")]
    public Factory targetFactory;

    public Transform outputPos;
    public Dough doughPrefab;

    [Header("LevelUps")]

    public int level = 1;
    public int levelCost = 500;
    public float initialSpeed = 2f;

    public bool hasEmployee = false;

    public GameObject employee;
    public GameObject employeeSleeping;


    public GameObject dispatchBtn;

    [Header("LevelUpWindow")]
    public GameObject levelUpWindow;
    public Text levelTxt;
    public Text levelCostText;
    public Text speedTxt;


    private GameManager gameManager;
    private NumericControl numericControl;
    private Player player;
    private Storage storage;
    private Elevator elevator;
    private Research research;

    // SET FACTORY INDEX ON AWAKE SO THE PLAYER SCRIPT CAN PROPERLY LOAD VALUES
    private void Awake()
    {


    }

    // Start is called before the first frame update
    void Start()
    {

        research = FindObjectOfType<Research>();
        storage = FindObjectOfType<Storage>();
        player = FindObjectOfType<Player>();
        numericControl = FindObjectOfType<NumericControl>();
        gameManager = FindObjectOfType<GameManager>();
        elevator = FindObjectOfType<Elevator>();


    }

    // checks if the factory previous to the clicked one is already purchased

    public bool FactoryIsActive(int index)
    {

        if (index < 0)
            return true;

        return targetFactory.isActive ? true : false;
    }

    // UPDATES ALL CANDIES TO MATCH THE CORRESPONDING IMAGE BY RESEARCH LVL
    public void UpdateCandyGraphics()
    {

        storage.SetCandyType(research.candyTypeLvl);
        elevator.SetCandyType(research.candyTypeLvl);

        //SetUpFabrics();
    }

    #region LEVEL UP
    public double LevelUpCost()
    {

        double newcost = levelCost;

        for (int i = 0; i < level; i++)
        {

            newcost *= 2;

        }

        return newcost;

        /*
        double cost = (levelCost * (level * level));
        return cost;
        */
    }


    public void UpdateWindowValues()
    {

        levelTxt.text = "Level: " + level.ToString();
        speedTxt.text = "Fabric time: " + fillTime.ToString("F2");
        levelCostText.text = "Upgrade $ " + numericControl.StringNumber(LevelUpCost());

    }

    // USE LVL FOR A SIMPLE LVL UP
    public void LevelUp(string target)
    {

        // REGULAR LEVEL UP
        if (target == "LVL" && level < 100 && player.money >= LevelUpCost())
        {

            player.SpendMoney(LevelUpCost());
            fillTime = (fillTime - 0.015f);
            level++;

            PlayerPrefs.SetInt("TOLVALVL", level);
            UpdateWindowValues();
        }

        // FOR LOADING A LEVEL, DONT USE MONEY
        else if (target != "LVL")
        {

            int targetLvl = int.Parse(target);

            while (level < targetLvl)
            {

                fillTime = (fillTime - 0.015f);
                level++;
            }
        }
    }

    // ACTIVATE THE EMPLOYEE IS  ITS RESEARCHED
    public void ActivateEmployee()
    {

        if (research.employeesLvl > 0)
        {

            hasEmployee = true;
            employee.SetActive(true);
        }
    }

    public void ToogleLevelUpWindow()
    {

        UpdateWindowValues();
        levelUpWindow.SetActive(!levelUpWindow.activeSelf);

    }
    #endregion


    public void SetUpFabrics()
    {

        //factories = FindObjectsOfType<Factory>();

        for (int i = 0; i < factories.Length; i++)
        {

            factories[i].candyType = research.candyTypeLvl;

            factories[i].SetFridge();
            factories[i].SetSample();

            //initial purchase cost
            factories[i].purchaseCost = gameManager.FactoryCost(i);
            factories[i].purchaseCostTxt.text = "Unlock: $" + numericControl.StringNumber(factories[i].purchaseCost);
        }


    }

    public void DeliverToFactory()
    {

        //print("tolva deliver");

        if (targetFactory != null)
            return;

        for (int i = 0; i < factories.Length; i++)
        {

            if (!factories[i].isQueued && factories[i].isActive && !factories[i].isSleeping)
            {

                targetFactory = factories[i];
                targetFactory.isQueued = true;

                Dough newDough = Instantiate(doughPrefab, outputPos.position, Quaternion.identity);
                newDough.targetFactory = targetFactory;

                targetFactory = null;
                fillTimer = 0;

                readyToDispatch = false;
                SleepChance();

                return;
            }


        }
    }

    // puts the employee to SLEEP by percent chance

    public void SleepChance()
    {

        float chance = Random.Range(0f, 100f);

        if (chance <= research.SleepChance())
        {

            employeeSleeping.SetActive(true);
            isSleeping = true;
            player.Sleeps();
        }


    }

    public void AwakeEmployee()
    {

        isSleeping = false;
        employeeSleeping.SetActive(false);

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        // enable button only if theres a factory that can receive and has no employee
        if (readyToDispatch && !hasEmployee)
        {

            for (int i = 0; i < factories.Length; i++)
            {

                if (!factories[i].isQueued && factories[i].isActive)
                {

                    dispatchBtn.SetActive(true);
                    return;
                }
            }

        }
        else
            dispatchBtn.SetActive(false);

        // Dispatch automatically if has an employee that is not sleeping
        if (readyToDispatch && hasEmployee && !isSleeping)
        {

            DeliverToFactory();

        }


        if (fillTimer < fillTime)
            fillTimer += Time.deltaTime;

        // Fill the completion bar
        float percent = fillTimer / fillTime;
        fillBar.size = new Vector2(percent * barSizeMultiplier, 1);

        if (fillTimer >= fillTime && targetFactory == null)
        {

            //DeliverToFactory();
            readyToDispatch = true;
        }


    }

}
