using UnityEngine;
using UnityEngine.UI;

public class Packing : MonoBehaviour{

    public float packingTime = 1;
    private float packingTimer;
    public int stored;
    public int maxStorage = 5;

    public bool canLoad = true;
    public bool isLoaded = false;
    public Box boxPrefab;

    [Header("FillBar")]
    public float barSizeMultiplier;
    public SpriteRenderer fillBar;

    [Header("LevelUps")]

    public int level = 1;
    public int levelCost = 500;
    public float initialSpeed = 2f;

    [Header("LevelUpWindow")]
    public GameObject levelUpWindow;
    public Text levelTxt;
    public Text levelCostText;
    public Text speedTxt;

    private NumericControl numericControl;

    [Header("Positions")]
    public Transform startPos;
    public Transform endPos;

    private Warehouse warehouse;
    private GameManager gameManager;
    private Player player;

    // Start is called before the first frame update
    void Start(){

        numericControl = FindObjectOfType<NumericControl>();
        warehouse = FindObjectOfType<Warehouse>();
        player = FindObjectOfType<Player>();
        gameManager = FindObjectOfType<GameManager>();
    }

    public void LoadCandies(){

        canLoad = false;
        isLoaded = true;

        /*

        if (stored < maxStorage)
        stored ++;

        if (stored == maxStorage){

            canLoad = false;
            isLoaded = true;
        }

        */

    }


    #region LEVEL UP
    public double LevelUpCost(){

        double newcost = levelCost;

        for (int i = 0; i < level; i++){

            newcost *= 2;

        }

        return newcost;

        /*
        double cost = (levelCost * (level * level));
        return cost;
        */
    }

    public void UpdateWindowValues() {

        levelTxt.text = "Level: " + level.ToString();
        speedTxt.text = "packing speed: " + packingTime.ToString("F2");
        levelCostText.text = "Upgrade $ " + numericControl.StringNumber(LevelUpCost());

    }

    public void LevelUp(string target){

        // REGULAR LEVEL UP
        if (target == "LVL" && level < 100 && player.money >= LevelUpCost()){

            player.SpendMoney(LevelUpCost());
            packingTime = (packingTime - 0.015f);
            level++;

            PlayerPrefs.SetInt("PACKINGLVL", level);
            UpdateWindowValues();
        }

        // FOR LOADING A LEVEL, DONT USE MONEY
        else if (target != "LVL"){

            int targetLvl = int.Parse(target);

            while (level < targetLvl) {

                packingTime = (packingTime - 0.015f);
                level++;

            }
        }
    }

    public void ToogleLevelUpWindow() {

        UpdateWindowValues();
        levelUpWindow.SetActive(!levelUpWindow.activeSelf);

    }
    #endregion

    // Update is called once per frame
    void FixedUpdate(){

        if (isLoaded && warehouse.stored < warehouse.maxStorage)
        packingTimer += Time.deltaTime;

        // Fill the completion bar
        float percent = packingTimer / packingTime;
        fillBar.size = new Vector2(percent * barSizeMultiplier, 1);

        if (packingTimer >= packingTime) {

            
            isLoaded = false;
            packingTimer = 0;

            Box newBox = Instantiate(boxPrefab, startPos.position, Quaternion.identity);
            newBox.movingUp = true;

            canLoad = true;
            stored = 0;
        }
        
    }
}
