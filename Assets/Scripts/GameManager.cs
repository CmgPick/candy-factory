using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour{

    //public int stage = 1;
    public int initialGoal = 1000000;
    public int goalMultiplier = 10;

    public int initialFactoryCost = 1000;

    private NumericControl numericControl;
    private Player player;
    private Research research;

    [Header("SUGAR window")]
    public GameObject sugarWindow;
    public Text sugarTxt;

    private void Awake(){
     
        numericControl = FindObjectOfType<NumericControl>();
        player = FindObjectOfType<Player>();
        research = FindObjectOfType<Research>();

        //stage = PlayerPrefs.GetInt("STAGE", 1);
        //PlayerPrefs.SetInt("STAGE", 1);

        research.candyTypeLvl = PlayerPrefs.GetInt("CANDY", 0); // CHANGE CANDY TYPE

        // HARD RESET FOR TESTING
        int hardRest = PlayerPrefs.GetInt("HARDRESET", 0);

        if(hardRest == 0) {

            PlayerPrefs.SetInt("HARDRESET", 1);
            ResetAllValues();

        }

    }


    public void ShowSugarWindow() {

        Time.timeScale = 0;
        sugarTxt.text = numericControl.StringNumber(SugarExchange()) + " Sugar";
        sugarWindow.SetActive(true);

    }

    public void HideSugarWindow(){

        Time.timeScale = 1;
        sugarWindow.SetActive(false);

    }

    public void ResetStage() {

        player.GetSugar(SugarExchange());

        PlayerPrefs.SetString("MONEY", "0");
        PlayerPrefs.SetString("SUGAR", player.sugar.ToString());
        PlayerPrefs.SetString("TOTALMONEY", "0");
        PlayerPrefs.SetInt("TOLVALVL", 1);
        //PlayerPrefs.SetInt("TOLVAEMPLOYEE", 0);

        PlayerPrefs.SetInt("PACKINGLVL", 0);
        PlayerPrefs.SetInt("WAREHOUSELVL", 0);
        PlayerPrefs.SetInt("TRUCKLVL", 0);
        PlayerPrefs.SetInt("STORAGELVL", 0);
        PlayerPrefs.SetInt("ELEVATORLVL", 0);

        Tolva tolva = FindObjectOfType<Tolva>();

        for (int i = 0; i < tolva.factories.Length; i++){

            PlayerPrefs.SetInt("FACTORY" + i + "ACTIVE", 0);
            PlayerPrefs.SetInt("FACTORY" + i + "LVL", 0);
            //PlayerPrefs.SetInt("FACTORY" + i + "EMPLOYEE", 0);
            PlayerPrefs.SetInt("FRIDGE" + i + "LVL", 0);
        }

        Time.timeScale = 1;
        SceneManager.LoadScene("map");
    }

    // returns the purchase cost for factories (INITIAL COST)
    public double FactoryCost(int index) {

        double price = initialFactoryCost;

        for (int i = 0; i < index; i++){

            price *= 2;
        }

        return index * price;

    }

    //return the total Sugar per sale

    public double SugarExchange() {

         return player.totalMoney / 1000f;

    }

    // returns the sale price for boxes including power ups, upgrades, etc

    public double SalePrice() {

        return  (research.SalePrice()) * ((100f + research.TotalIncrease()) / 100f);
    }

    public void ResetAllValues(){

        PlayerPrefs.SetString("PRODUCED", "0");
        PlayerPrefs.SetString("DROPPED", "0");
        PlayerPrefs.SetString("SOLD", "0");
        PlayerPrefs.SetString("BOXES", "0");
        PlayerPrefs.SetString("MONEY", "0");
        PlayerPrefs.SetString("SUGAR", "0");
        PlayerPrefs.SetString("TOTALMONEY", "0");
        PlayerPrefs.SetString("SLEEP", "0");
        PlayerPrefs.SetString("PARACHUTES", "0");

        // buildings

        PlayerPrefs.SetInt("TOLVALVL", 0);
        //PlayerPrefs.SetInt("TOLVAEMPLOYEE", 0);
        PlayerPrefs.SetInt("PACKINGLVL", 0);
        PlayerPrefs.SetInt("WAREHOUSELVL", 0);
        PlayerPrefs.SetInt("TRUCKLVL", 0);
        PlayerPrefs.SetInt("STORAGELVL", 0);
        PlayerPrefs.SetInt("ELEVATORLVL", 0);

        PlayerPrefs.SetInt("GOALSHOWN", 0);
        //PlayerPrefs.SetInt("STAGE", 1);

        PlayerPrefs.SetInt("SWEETNESS", 0);
        PlayerPrefs.SetInt("FLAVOR", 0);
        PlayerPrefs.SetInt("QUALITY", 0);
        PlayerPrefs.SetInt("MARKETING", 0);
        PlayerPrefs.SetInt("EMPLOYEE", 0);
        PlayerPrefs.SetInt("EMPLOYEESLEEP", 0);
        PlayerPrefs.SetInt("CANDY", 0);

        PlayerPrefs.SetInt("TUTORIAL", 0);

        Tolva tolva = FindObjectOfType<Tolva>();

        for (int i = 0; i < tolva.factories.Length; i++){

            PlayerPrefs.SetInt("FACTORY" + i + "ACTIVE", 0);
            PlayerPrefs.SetInt("FACTORY" + i + "LVL", 0);
            //PlayerPrefs.SetInt("FACTORY" + i + "EMPLOYEE", 0);
            PlayerPrefs.SetInt("FRIDGE" + i + "LVL", 0);
        }

        SceneManager.LoadScene("map");

    }

#if UNITY_EDITOR

    private void Update() {

        if (Input.GetKey(KeyCode.F12))
            ResetAllValues();
    }
#endif

}
