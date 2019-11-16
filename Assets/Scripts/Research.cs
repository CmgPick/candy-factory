using UnityEngine.UI;
using UnityEngine;

public class Research : MonoBehaviour{

    public int candyTypeLvl = 0;
    public int employeesLvl = 0;
    public int employeeSleepLvl = 0;
    public int sweetnessLvl = 0;
    public int flavorLvl = 0;
    public int qualityLvl = 0;
    public int marketingLvl = 0;

    public float baseSellPrice = 500f;

    public int employeesPriceBase = 100;
    public int employeesSleepPriceBase = 100;
    public int candyTypePriceBase = 100;
    public int sweetnessPriceBase = 5000;
    public int flavorPriceBase = 5000;
    public int qualityPriceBase = 5000;
    public int marketingPriceBase = 5000;

    public GameObject ResearchWindow;

    public Text employeesLvlTxt;
    public Text employeSleepTxt;
    public Text candyTypeTxt;
    public Text sweetnessLvlTxt;
    public Text flavorLvlTxt;
    public Text qualityLvlTxt;
    public Text marketingLvlTxt;

    public Text employeeLvlPriceTxt;
    public Text employeeSleepPriceTxt;
    public Text candyTypePriceTxt;
    public Text sweetnessPriceTxt;
    public Text flavorPriceTxt;
    public Text qualityPriceTxt;
    public Text marketingPriceTxt;

    public Text totalIncreaseTxt;

    private NumericControl numericControl;
    private Player player;
    private Tolva tolva;

    // Start is called before the first frame update
    void Start() {

        numericControl = FindObjectOfType<NumericControl>();
        player = FindObjectOfType<Player>();
        tolva = FindObjectOfType<Tolva>();
    }

    public void UpdateWindowValues() {

        employeesLvlTxt.text = "Unlocked employees: " + employeesLvl;
        employeSleepTxt.text = "Employee sleep chance: " + "(" + SleepChance().ToString("F2") + "%" + ")";
        candyTypeTxt.text = "Candies type " + (candyTypeLvl + 1) + " (Sell price " + numericControl.StringNumber(SalePrice()) + ")";

        sweetnessLvlTxt.text = "Sweetness Lvl " + sweetnessLvl  + " (Sell price + 5%)";
        flavorLvlTxt.text = "Flavor Lvl " + flavorLvl + " (Sell price + 5%)";
        qualityLvlTxt.text = "Quality Lvl " + qualityLvl + " (Sell price + 10%)";
        marketingLvlTxt.text = "Marketing Lvl " + marketingLvl + " (Sell price + 10%)";

        //PRICES
        employeeLvlPriceTxt.text = numericControl.StringNumber(EmployeeCost());
        employeeSleepPriceTxt.text = numericControl.StringNumber(EmployeeSleepCost());
        candyTypePriceTxt.text = numericControl.StringNumber(CandyTypeCost());

        sweetnessPriceTxt.text = numericControl.StringNumber(SweetnessCost());
        flavorPriceTxt.text = numericControl.StringNumber(FlavorCost());
        qualityPriceTxt.text = numericControl.StringNumber(QualityCost());
        marketingPriceTxt.text = numericControl.StringNumber(MarketingCost());

        totalIncreaseTxt.text = "Total sell price increase: " + TotalIncrease() + " %";

    }

    // 1 to 4 to select level up target
    public void LevelUp(int target) {

        if (target == 1 && employeesLvl < 13 && player.sugar >= EmployeeCost()){

            player.SpendSugar(EmployeeCost());
            employeesLvl ++;

            PlayerPrefs.SetInt("EMPLOYEE", employeesLvl);
        }

        if (target == 2 && employeeSleepLvl < 100 && player.sugar >= EmployeeSleepCost()){

            player.SpendSugar(EmployeeSleepCost());
            employeeSleepLvl ++;

            PlayerPrefs.SetInt("EMPLOYEESLEEP", employeeSleepLvl);
        }

        if (target == 3 && candyTypeLvl < 12 && player.sugar >= CandyTypeCost()){

            player.SpendSugar(CandyTypeCost());
            candyTypeLvl ++;

            PlayerPrefs.SetInt("CANDY", candyTypeLvl);

            tolva.UpdateCandyGraphics();
        }

        if (target == 4 && sweetnessLvl < 100 && player.sugar >= SweetnessCost()){

                player.SpendSugar(SweetnessCost());
                sweetnessLvl++;

                PlayerPrefs.SetInt("SWEETNESS", sweetnessLvl);
        }

        if (target == 5 && flavorLvl < 100 && player.sugar >= FlavorCost()){

                player.SpendSugar(FlavorCost());
                flavorLvl ++;

                PlayerPrefs.SetInt("FLAVOR", flavorLvl);
        }

        if (target == 6 && qualityLvl < 100 && player.sugar >= QualityCost()){

            player.SpendSugar(QualityCost());
            qualityLvl++;

            PlayerPrefs.SetInt("QUALITY", qualityLvl);
        }

        if (target == 7 && marketingLvl < 100 && player.sugar >= MarketingCost()) {

            player.SpendSugar(MarketingCost());
            marketingLvl++;

            PlayerPrefs.SetInt("MARKETING", marketingLvl);
        }

        player.ActivateAllEmployees();
        UpdateWindowValues();
    }

    public int TotalIncrease(){

        return (sweetnessLvl * 5) + (flavorLvl * 5) + (qualityLvl * 10) + (marketingLvl * 10);

    }

    // COST
    public double EmployeeCost() {

        return employeesPriceBase * (employeesLvl +1) * (employeesLvl + 1) * (employeesLvl + 1);
    }

    public double EmployeeSleepCost() {

        return employeesSleepPriceBase * (employeeSleepLvl + 1) * (employeeSleepLvl + 1) * (employeeSleepLvl + 1);
    }

    public double CandyTypeCost(){

        return candyTypePriceBase * Mathf.Exp((candyTypeLvl + 1) * 2f);
    }

    public double SweetnessCost() {

        return sweetnessPriceBase * Mathf.Exp((sweetnessLvl / 1.25f) + 1);
    }

    public double FlavorCost(){

        return flavorPriceBase * Mathf.Exp((flavorLvl / 1.25f) + 1);
    }

    public double QualityCost(){

        return qualityPriceBase * Mathf.Exp((qualityLvl / 1.25f) + 1);
    }

    public double MarketingCost() {

        return marketingPriceBase * Mathf.Exp((marketingLvl / 1.25f) + 1);
    }


    public float SleepChance() {

        return 10f - (employeeSleepLvl * 0.1f);
    }

    public double SalePrice(){

        return baseSellPrice * (candyTypeLvl + 1);
    }

    public void ToogleLevelUpWindow() {

        UpdateWindowValues();
        ResearchWindow.SetActive(!ResearchWindow.activeSelf);

    }
}
