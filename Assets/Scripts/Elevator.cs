using UnityEngine;
using UnityEngine.UI;

public class Elevator : MonoBehaviour{

    public int stored;
    public int maxStorage;

    public Transform startPos;

    public Factory [] factories;
    public Factory targetFactory;
    public int currentfactory = 0;
    public CandyPile[] storedCandies;

    public float moveSpeed = 2f;
    public float initialSpeed = 2f;
    public float loadTime = 1;
    public float initialLoadTime = 1;
    public float loadTimer;

    [Header("Switchs")]
    public bool isMoving;
    public bool isLoading;
    public bool movingDown;
    public bool unload = false;

    [Header("FillBar")]
    public float barSizeMultiplier;
    public SpriteRenderer fillBar;

    private Storage storage;

    [Header("LevelUps")]

    public int level = 1;
    public int levelCost = 1000;
    public int initialMaxStorage = 5;

    [Header("LevelUpWindow")]
    public GameObject levelUpWindow;
    public Text levelTxt;
    public Text levelCostText;
    public Text maxStorageTxt;
    public Text speedTxt;
    public Text loadSpeedTxt;

    private NumericControl numericControl;
    private GameManager gameManager;
    private Player player;

    public void Start(){

        player = FindObjectOfType<Player>();
        gameManager = FindObjectOfType<GameManager>();
        numericControl = FindObjectOfType<NumericControl>();
        storage = FindObjectOfType<Storage>();
        targetFactory = factories[0];

        UpdateCandies();
    }

    public void SetCandyType(int type) {

        for (int i = 0; i < storedCandies.Length; i++){

            storedCandies[i].SetCandyType(type);
        }
    }

    public void UpdateCandies(){

        for (int i = 0; i < storedCandies.Length; i++) {

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

        for (int i = 0; i < level; i++){

            storage += 0.1f;
        }

        maxStorageTxt.text = "Max Storage: " + storage.ToString("F2");


        levelCostText.text = "Upgrade $ " + numericControl.StringNumber(LevelUpCost());
        speedTxt.text = "Move speed " + moveSpeed.ToString("F2");
        loadSpeedTxt.text = "Load Speed " + loadTime.ToString("F3");

    }

    public void LevelUp(string target){

        // REGULAR LEVEL UP
        if (target == "LVL" && level < 100 && player.money >= LevelUpCost()){

            player.SpendMoney(LevelUpCost());
            maxStorage = (int)(initialMaxStorage + ((level + 1) * 0.1f));
            moveSpeed = (initialSpeed + ((level + 1) * 0.1f));
            loadTime = (float)(initialLoadTime - ((level +1) * 0.0075f));

            level++;

            PlayerPrefs.SetInt("ELEVATORLVL", level);
            UpdateWindowValues();
        }

        // FOR LOADING A LEVEL, DONT USE MONEY
        else if (target != "LVL"){

            int targetLvl = int.Parse(target);

            while (level < targetLvl){

                maxStorage = (int)(initialMaxStorage + ((level + 1) * 0.1f));
                moveSpeed = (initialSpeed + ((level + 1) * 0.1f));
                loadTime = (float)(initialLoadTime - ((level +1 ) * 0.0075f));

                level++;

            }
        }

    }

    public void ToogleLevelUpWindow() {

        UpdateWindowValues();
        levelUpWindow.SetActive(!levelUpWindow.activeSelf);

    }
    #endregion


    public void GetNextFactory(){

        if (currentfactory < factories.Length - 1) {

            // only look for next factory if its active and has space
            if (factories[currentfactory + 1].isActive && stored < maxStorage){

                currentfactory++;
                isMoving = true;
                movingDown = true;
            }
            else
            {

                isMoving = true;
                movingDown = false;
                currentfactory = 0;
            }
        }
            

        else {

            isMoving = true;
            movingDown = false;
            currentfactory = 0;
        }

            

        targetFactory = factories[currentfactory];
        
    }

    // Update is called once per frame
    void FixedUpdate(){

        // Fill the completion bar
        float percent = loadTimer / loadTime;

        if(!isMoving)
            fillBar.size = new Vector2(percent * barSizeMultiplier, 1);
        else
            fillBar.size = new Vector2(0,0);

        // MOVE DOWN
        if (isMoving && movingDown && !unload) 
            transform.Translate(0, -moveSpeed * Time.deltaTime, 0);

        // MOVE UP
        if (isMoving && !movingDown && !unload)
            transform.Translate(0, moveSpeed * Time.deltaTime, 0);

        // REACHED STARTPOS
        if (isMoving && !movingDown && !unload && transform.position.y >= startPos.position.y){

            isMoving = false;
            unload = true;
            //targetFactory = null;
        }

        // UNLOAD
        if(unload && stored > 0 && storage.stored < storage.maxStorage) {

            loadTimer += Time.deltaTime;
        }

        if(unload && stored <= 0) {

            //GetNextFactory();

            loadTimer = 0;

            unload = false;
            isMoving = true;
            movingDown = true;

        }

        if(unload && loadTimer >= loadTime) {

            if(storage.stored < storage.maxStorage){

                storage.Store();
                stored--;

                UpdateCandies();
            }
            

            loadTimer = 0;

            if(stored <= 0) {

                unload = false;
                isMoving = true;
                movingDown = true;
            }
        }

        // REACHED FACTORY
        if (isMoving && movingDown && transform.position.y <= targetFactory.elevatorPos.position.y) {

            isMoving = false;
            isLoading = true;
        }

        if (isLoading && targetFactory.fridge.stored > 0 && stored < maxStorage) {

            loadTimer += Time.deltaTime;
        }

        if (isLoading && targetFactory.fridge.stored <= 0 || stored >= maxStorage) {

            loadTimer = loadTime;
            //GetNextFactory();
        }

        if (isLoading && loadTimer >= loadTime) {

            isLoading = false;
            loadTimer = 0;

            if(targetFactory.fridge.stored > 0 && stored < maxStorage) {

                stored++;
                targetFactory.fridge.Unload();

                UpdateCandies();
            }

            // keep loading if theres space
            if (targetFactory.fridge.stored > 0 && stored < maxStorage){

                isLoading = true;
            }
            else
                GetNextFactory();

        }
    }
}
