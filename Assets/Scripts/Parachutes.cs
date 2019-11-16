using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Parachutes : MonoBehaviour{

    public GameObject rewardWindow;
    public Text rewardText;
    public Transform parachute;
    public Vector3 startPos;

    public bool isOn = false;
    public bool isMoving = false;
    public float speed = 1f;
    public bool goLeft = false;

    public bool parachuteStarted = false;

    private NumericControl numericControl;
    private MouseController mouseController;
    private Warehouse warehouse;
    private Player player;
    private Elevator elevator;
    private Tolva tolva;

    public double moneyReward;
    public int boxesReward;
    public double sugarReward;

    public int currentReward;

    // Start is called before the first frame update
    void Start(){

        startPos = parachute.position;

        numericControl = FindObjectOfType<NumericControl>();
        mouseController = FindObjectOfType<MouseController>();
        warehouse = FindObjectOfType<Warehouse>();
        player = FindObjectOfType<Player>();
        elevator = FindObjectOfType<Elevator>();
        tolva = FindObjectOfType<Tolva>();
    }

    public IEnumerator ParachuteWaiting() {

        parachuteStarted = true;

        int random = Random.Range(30, 61);
        print("launching parachute in " + random);

        yield return new WaitForSeconds(random);

        LaunchParachute();
    }

    public void LaunchParachute() {

        // MAX MUST BE REWARDS + 1
        currentReward = Random.Range(0, 3);

        if (currentReward == 0)
            BoxesAmmount();

        else if (currentReward == 1)
            SugarAmmount();

        else if (currentReward == 2)
            MoneyAmmount();

        rewardText.text = RewardWindowTxt(currentReward);

        isMoving = true;

    }

    // SET the number of boxes that will be received
    private void BoxesAmmount() {

        boxesReward = Random.Range(0, 4);
    }

    // SET the number of sugar received
    private void SugarAmmount(){


        if(player.sugar <= 1) {

            sugarReward = 1;
        }
        else{


            float percent = Random.Range(0.05f, 0.10f);
            sugarReward = player.sugar * percent;

        }

    }

    //SET the ammount of money to be received
    private void MoneyAmmount() {

        if(player.money <= 1000) {

            moneyReward = 1000;

        }

        else {

            float percent = Random.Range(0.1f, 0.25f);
            moneyReward = player.money * percent;

        }
    }

    public string RewardWindowTxt(int reward) {

        if (reward == 0)
        {
            if (boxesReward == 1)
                return "We received 1 candies Box! You can receive it or see an Ad and double the ammount";
            else
                return "Great we received " + boxesReward + " full Boxes! You can receive them or see an Ad and double the ammount";
        }

        else if (reward == 1)
        {

            return "Awesome we received " + numericControl.StringNumber(sugarReward) + " Sugar! You can receive it or see an Ad and double the ammount";
        }

        else if (reward == 2)
        {

            return "Yeah we received " + numericControl.StringNumber(moneyReward) + " Money! You can receive it or see an Ad and double the ammount";

        }

        else return "Error";

    }

    public void CloseParachuteWindow() {

        rewardWindow.SetActive(false);
        Time.timeScale = 1;

    }

    public void ReturnHome(){

        isOn = true;
        isMoving = false;
        parachute.position = startPos;

        parachuteStarted = false;

    }
    
    public void ParachuteClicked() {

        print("Parachute clicked");
        rewardWindow.SetActive(true);

        Time.timeScale = 0;

    }

    public void SeeAd() {
        print("see add");
    }

    // give the player a random reward 
    public void CloseRewardWindow(bool ad) {

        Time.timeScale = 1;

        //0 boxes // 1 sugar // 2 money

        switch (currentReward) {

            //give the player a random number of boxes (1 to 3)?
            case 0:

                for (int i = 0; i < boxesReward; i++){

                    warehouse.StoreBox();
                }
                
                break;

            case 1:
                // Get SUGAR 5 - 10 % of current sugar?

                player.GetSugar(sugarReward);

                break;

            case 2:

                player.GetMoney(moneyReward); // 10 - 25% current money?

                break;

        }

        player.Parachute();
        CloseParachuteWindow();

        ReturnHome();
    }


    // Update is called once per frame
    void FixedUpdate(){

        if (isOn && !parachuteStarted)
            StartCoroutine("ParachuteWaiting");

        if(isMoving && !mouseController.windowOpen) {

            // go down
            parachute.Translate(0, -speed * Time.deltaTime, 0);

            //move right
            if (!goLeft)
                parachute.Translate(speed * Time.deltaTime, 0 , 0);

            //move left
            if (goLeft)
                parachute.Translate( - speed * Time.deltaTime, 0, 0);

            //reached Leftpos
            if (goLeft && parachute.position.x < -3)
                goLeft = false;

            //reached rightpos
            if (!goLeft && parachute.position.x > 2)
                goLeft = true;

            //reached bottom
            if (parachute.position.y < -40){

                ReturnHome();
            }
        }
        
    }
}
