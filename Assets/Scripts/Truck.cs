using UnityEngine.UI;
using UnityEngine;

public class Truck : MonoBehaviour{

    public int stored = 0;
    public int maxStorage = 5;

    public int speed = 1;

    public bool isMoving = false;
    public bool moveRight = true;

    public Vector3 startPos;
    public Vector3 endPos;

    public GameObject[] boxes;
    public GameObject sellBtn;
    private Player player;

    [Header("LevelUps")]

    public int level = 1;
    public int levelCost = 1000;
    public int initialMaxStorage = 5;

    [Header("LevelUpWindow")]
    public GameObject levelUpWindow;
    public Text levelTxt;
    public Text levelCostText;
    public Text maxStorageTxt;

    private NumericControl numericControl;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start(){

        gameManager = FindObjectOfType<GameManager>();
        numericControl = FindObjectOfType<NumericControl>();
        player = FindObjectOfType<Player>();

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

    public void UpdateWindowValues() {

        levelTxt.text = "Level: " + level.ToString();

        float storage = initialMaxStorage;

        for (int i = 0; i < level; i++){

            storage += 0.1f;
        }

        maxStorageTxt.text = "Max Storage: " + storage.ToString("F2");

        levelCostText.text = "Upgrade $ " + numericControl.StringNumber(LevelUpCost());

    }

    public void LevelUp(string target){

        // REGULAR LEVEL UP
        if (target == "LVL" && level < 100 && player.money >= LevelUpCost()){

            player.SpendMoney(LevelUpCost());
            maxStorage = (int)(initialMaxStorage + ((level + 1) * 0.1f));
            level++;

            PlayerPrefs.SetInt("TRUCKLVL", level);
            UpdateWindowValues();
        }

        // FOR LOADING A LEVEL, DONT USE MONEY
        else if (target != "LVL"){

            int targetLvl = int.Parse(target);

            while (level < targetLvl){

                maxStorage = (int)(initialMaxStorage + ((level + 1) * 0.1f));
                level++;

            }
        }

    }

    public void ToogleLevelUpWindow(){

        UpdateWindowValues();
        levelUpWindow.SetActive(!levelUpWindow.activeSelf);

    }
    #endregion


    public void UpdateBoxes(){

        for (int i = 0; i < boxes.Length; i++){

            if (stored > i)
                boxes[i].SetActive(true);
            else
                boxes[i].SetActive(false);


        }
        
    }

    public void LoadBox() {

        if (stored < maxStorage) {

            stored++;
            UpdateBoxes();
        }
            

    }

    public void Delivery() {

        if(stored > 0 && !isMoving) {

            print("Moving");
            isMoving = true;
        }


    }

    public void SaleGoods() {

        //FIX THIS FOR REAL VALUES
        double totalSale = stored * gameManager.SalePrice();

        player.GetMoney(totalSale);

        player.BoxesSold (stored);

        stored = 0;
        UpdateBoxes();

        // checks if the money goal is completed
        //gameManager.CheckGoalCompletion();

    }

    // Update is called once per frame
    void FixedUpdate(){

        if (isMoving || stored <= 0)
            sellBtn.SetActive(false);
        else
            sellBtn.SetActive(true);

        if (isMoving && moveRight)
            transform.Translate(speed * Time.deltaTime, 0, 0);

        if(isMoving && moveRight && transform.position.x >= endPos.x) {

            SaleGoods();
            moveRight = false;
        }

        if (isMoving && !moveRight)
            transform.Translate( -speed * Time.deltaTime, 0, 0);

        if (isMoving && !moveRight && transform.position.x <= startPos.x){

            isMoving = false;
            moveRight = true;
        }

    }
}
