using UnityEngine;
using UnityEngine.UI;

public class FridgeWindow : MonoBehaviour {

    public Fridge targetFridge;
    public int indexMultiplier; // USED TO MULTIPLY THE COST

    [Header("LevelUpWindow")]
    public GameObject levelUpWindow;
    public Text levelTxt;
    public Text levelCostText;
    public Text maxStorageTxt;


    private Player player;
    private Tolva tolva;
    private NumericControl numericControl;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start(){

        player = FindObjectOfType<Player>();
        gameManager = FindObjectOfType<GameManager>();
        numericControl = FindObjectOfType<NumericControl>();
        tolva = FindObjectOfType<Tolva>();
        
    }

    public void OpenFridgeWindow(Fridge fridge, int num) {

        indexMultiplier = num;
        targetFridge = fridge;
        UpdateWindowValues();
        levelUpWindow.SetActive(true);
    }

    public void CloseFactoryWindow( ){

        indexMultiplier = 0;
        targetFridge = null;
        levelUpWindow.SetActive(false);
    }

    #region LEVEL UP
    public double LevelUpCost(){

        double newcost = targetFridge.levelCost;

        for (int i = 0; i < targetFridge.level; i++){

            newcost *= 2;

        }

        newcost = newcost * indexMultiplier;
        return newcost;

        /*
        double cost = (targetFridge.levelCost * (targetFridge.level * targetFridge.level * indexMultiplier));
        return cost;

        double cost = ((double)(targetFridge.levelCost * targetFridge.level) * (indexMultiplier * Mathf.Exp(indexMultiplier)));

        return cost;
        */
    }


    public void UpdateWindowValues() {

        levelTxt.text = "Level: " + targetFridge.level.ToString();

        float storage = targetFridge.initialMaxStorage;

        for (int i = 0; i < targetFridge.level; i++){

            storage += 0.1f;
        }

        maxStorageTxt.text = "Max Storage: " + storage.ToString("F2");

        levelCostText.text = "Upgrade $ " + numericControl.StringNumber(LevelUpCost());
    }

    // USE LVL FOR A SIMPLE LVL UP
    public void LevelUp(string target) {

        // REGULAR LEVEL UP
        if (target == "LVL" && targetFridge.level < 100 && player.money >= LevelUpCost()) {

            player.SpendMoney(LevelUpCost());
            targetFridge.maxStorage = (int)(targetFridge.initialMaxStorage + ((targetFridge.level + 1) * 0.1f));

            //targetFridge.maxStorage = (int)(targetFridge.initialMaxStorage + (((targetFridge.level + 1) / 10) * 0.1f));
            targetFridge.level ++;

            PlayerPrefs.SetInt("FRIDGE" + targetFridge.transform.root.GetComponent<Factory>().index + "LVL", targetFridge.level);
            UpdateWindowValues();
        }

        // FOR LOADING A LEVEL, DONT USE MONEY
        else if (target != "LVL"){

            int targetLvl = int.Parse(target);

            while (targetFridge.level < targetLvl) {

                //targetFridge.maxStorage = (int)(targetFridge.initialMaxStorage + (((targetFridge.level + 1) / 10) * 0.1f));
                targetFridge.maxStorage = (int)(targetFridge.initialMaxStorage + ((targetFridge.level + 1) * 0.1f));

                targetFridge.level++;
            }
        }


    }

    #endregion
}
