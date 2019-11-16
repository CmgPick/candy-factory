using UnityEngine;
using UnityEngine.UI;

public class Storage : MonoBehaviour{

    public int stored;
    public int maxStorage = 5;
    public CandyPile [] storedCandies;

    public bool unload = false;
    public int unloaded = 0;

    public float unloadTime = 1;
    public float beltSpeed = 1;
    private float unloadTimer;

    [Header("Positions")]
    public Transform unloadStartPos;
    public Transform unloadEndPos;

    
    public CandyPile candyPile;

    [Header("LevelUps")]

    public int level = 1;
    public int levelCost = 1000;
    public int initialMaxStorage = 5;

    [Header("LevelUpWindow")]
    public GameObject levelUpWindow;
    public Text levelTxt;
    public Text levelCostText;
    public Text maxStorageTxt;

    private Player player;
    private Packing packing;
    private NumericControl numericControl;
    private GameManager gameManager;
    private Research research;

    // Start is called before the first frame update
    void Start(){

        player = FindObjectOfType<Player>();
        numericControl = FindObjectOfType<NumericControl>();
        packing = FindObjectOfType<Packing>();
        gameManager = FindObjectOfType<GameManager>();
        research = FindObjectOfType<Research>();

        UpdateCandies();

    }

    public void SetCandyType(int type) {

        for (int i = 0; i < storedCandies.Length; i++){

            storedCandies[i].SetCandyType(type);

        }

    }

    public void UpdateCandies(){

        for (int i = 0; i < storedCandies.Length; i++){

            if (stored > i)
                storedCandies[i].gameObject.SetActive(true);
            else
                storedCandies[i].gameObject.SetActive(false);
        }
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

    public void UpdateWindowValues(){

        levelTxt.text = "Level: " + level.ToString();

        float storage = initialMaxStorage;

        for (int i = 0; i < level; i++) {

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

            PlayerPrefs.SetInt("STORAGELVL", level);
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


    public void Unload() {

        if (!unload)
            return;

        if (unload && !packing.isLoaded && packing.canLoad) {

            CandyPile newCandyPile = Instantiate(candyPile, unloadStartPos.position, Quaternion.identity);
            newCandyPile.goToPacking = true;
            newCandyPile.SetCandyType(research.candyTypeLvl);
            stored--;
            UpdateCandies();

            /*
            unloaded ++;

            if (unloaded == 5){

                packing.canLoad = false;
                unload = false;
                unloaded = 0;
            }*/

            packing.canLoad = false;
            unload = false;
        }

            
    }

    public void Store() {

        if (stored < maxStorage)
            stored++;

        UpdateCandies();
 
    }

    // Update is called once per frame
    void FixedUpdate(){

        if (stored >= 1 && !packing.isLoaded && packing.canLoad)
            unload = true;


            if (unload)
            unloadTimer += Time.deltaTime;

        if(unloadTimer >= unloadTime) {

            unloadTimer = 0;
            Unload();
        }
    }
}
