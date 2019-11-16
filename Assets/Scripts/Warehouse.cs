using UnityEngine;
using UnityEngine.UI;

public class Warehouse : MonoBehaviour{

    public int stored = 0;
    public int maxStorage = 5;

    public float unloadTime = 1;
    private float unloadTimer;

    public Box boxPrefab;
    public GameObject [] boxes;

    [Header("positions")]
    public Transform boxUpPos;
    public Transform boxRightStartPos;
    public Transform boxRightEndPos;
    public Transform boxFallPos;

    [Header("LevelUps")]

    public int level = 1;
    public int levelCost = 1000;
    public int initialMaxStorage = 5;

    [Header("LevelUpWindow")]
    public GameObject levelUpWindow;
    public Text levelTxt;
    public Text levelCostText;
    public Text maxStorageTxt;

    private GameManager gameManager;
    private NumericControl numericControl;
    private Truck truck;
    public Player player;

    // Start is called before the first frame update
    void Start(){

        gameManager = FindObjectOfType<GameManager>();
        numericControl = FindObjectOfType<NumericControl>();
        truck = FindObjectOfType<Truck>();
        player = FindObjectOfType<Player>();

    }

    public void StoreBox() {

        if (stored < maxStorage)
            stored ++;

        UpdateBoxes();
    }

    #region LEVEL UP
    public double LevelUpCost() {


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

    public void UpdateWindowValues(){

        levelTxt.text = "Level: " + level.ToString();

        float storage = initialMaxStorage;

        for (int i = 0; i < level; i++){

            storage += 0.1f;
        }

        maxStorageTxt.text = "Max Storage: " + storage.ToString("F2");

        levelCostText.text = "Upgrade $ " + numericControl.StringNumber(LevelUpCost());

    }

    public void UpdateBoxes(){

        for (int i = 0; i < boxes.Length; i++){

            if (stored > i)
                boxes[i].SetActive(true);
            else
                boxes[i].SetActive(false);
        }
    }

    public void LevelUp(string target){

        // REGULAR LEVEL UP
        if (target == "LVL" && level < 100 && player.money >= LevelUpCost()){

            player.SpendMoney(LevelUpCost());
            maxStorage = (int)(initialMaxStorage + ((level + 1)  * 0.1f));
            level++;

            PlayerPrefs.SetInt("WAREHOUSELVL", level);
            UpdateWindowValues();
        }

        // FOR LOADING A LEVEL, DONT USE MONEY
        else if (target != "LVL") {

            int targetLvl = int.Parse(target);

            while (level < targetLvl) {

                maxStorage = (int)(initialMaxStorage + ((level + 1) * 0.1f));
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

        if (stored > 0 && truck.stored < truck.maxStorage)
            unloadTimer += Time.deltaTime;

        if(unloadTimer >= unloadTime) {

            unloadTimer = 0;

            if (!truck.isMoving){

                Box newBox = Instantiate(boxPrefab, boxRightStartPos.position, Quaternion.identity);
                newBox.movingRight = true;

                stored--;
                UpdateBoxes();
            }
        }
        
    }
}
