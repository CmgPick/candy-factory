using UnityEngine;
using UnityEngine.UI;

public class FactoryWindow : MonoBehaviour{

    public Factory targetFactory;
    public int indexMultiplier;

    [Header("LevelUpWindow")]
    public GameObject levelUpWindow;
    public Text levelTxt;
    public Text levelCostText;

    public Text speedTxt;

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

    public void OpenFactoryWindow(Factory factory, int num) {

        indexMultiplier = num;
        targetFactory = factory;
        UpdateWindowValues();
        levelUpWindow.SetActive(true);
    }

    public void CloseFactoryWindow( ){

        targetFactory = null;
        levelUpWindow.SetActive(false);
    }

    #region LEVEL UP
    public double LevelUpCost(){

        double newcost = targetFactory.levelCost;

        for (int i = 0; i < targetFactory.level; i++){

            newcost *= 2;

        }

        newcost = newcost * indexMultiplier;
        return newcost;

        /*
        double cost = (targetFactory.levelCost * (targetFactory.level * targetFactory.level * indexMultiplier));
        return cost;*/
    }

    public void UpdateWindowValues() {

        levelTxt.text = "Level: " + targetFactory.level.ToString();
        speedTxt.text = "Fabric time: " + targetFactory.fabricTime .ToString("F2");
        levelCostText.text = "Upgrade $ " + numericControl.StringNumber(LevelUpCost());
    }

    // USE LVL FOR A SIMPLE LVL UP
    public void LevelUp(string target){

        // REGULAR LEVEL UP
        if (target == "LVL" && targetFactory.level < 100 && player.money >= LevelUpCost()){

            player.SpendMoney(LevelUpCost());
            targetFactory.fabricTime = (targetFactory.fabricTime - 0.015f);
            targetFactory.level++;

            PlayerPrefs.SetInt("FACTORY" + targetFactory.index + "LVL", targetFactory.level);
            UpdateWindowValues();
        }

        // FOR LOADING A LEVEL, DONT USE MONEY
        else if (target != "LVL"){

            int targetLvl = int.Parse(target);

            while (targetFactory.level < targetLvl){

                targetFactory.fabricTime = (targetFactory.fabricTime - 0.015f);
                targetFactory.level++;
            }
        }

        //Factory PURCHASED

        if (PlayerPrefs.GetInt("FACTORY" + targetFactory.index + "ACTIVE", 0) == 1)
            targetFactory.AlreadyPurchased();
    }

    #endregion
}
