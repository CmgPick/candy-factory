using UnityEngine;

public class MouseController : MonoBehaviour{

    bool clicked = false;
    float clickTimer;

    public bool windowOpen = false;

    private Tolva tolva;
    private Truck truck;
    private Packing packing;
    private Warehouse warehouse;
    private Storage storage;
    private Elevator elevator;
    private FactoryWindow factoryWindow;
    private FridgeWindow fridgeWindow;
    private Research research;
    private Parachutes parachutes;

    private void Start() {

        elevator = FindObjectOfType<Elevator>();
        tolva = FindObjectOfType<Tolva>();
        truck = FindObjectOfType<Truck>();
        packing = FindObjectOfType<Packing>();
        warehouse = FindObjectOfType<Warehouse>();
        storage = FindObjectOfType<Storage>();
        factoryWindow = FindObjectOfType<FactoryWindow>();
        fridgeWindow = FindObjectOfType<FridgeWindow>();
        research = FindObjectOfType<Research>();
        parachutes = FindObjectOfType<Parachutes>();
    }

    public void CloseWindows(){

        windowOpen = false;
    }

    public void OpenWindows(){

        windowOpen = true;
    }

    // Update is called once per frame
    void Update() {

        if (Input.GetButtonDown("Fire1") && !clicked){

            CastQuickRay();
            clicked = true;
        }

        if (Input.GetButtonUp("Fire1") && clicked){
            
            clicked = false;
            clickTimer = 0;
        }
    }

    private void FixedUpdate(){

        if (clicked)
            clickTimer += Time.deltaTime;

        if(clickTimer >= 0.5f) {

            clicked = false;
            clickTimer = 0;

            CastRay();
        }

    }

    // CAST INSTANTLY
    void CastQuickRay() {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100)){

            // TRUCK SELL
            if (hit.transform.tag == "Sell" && !windowOpen){

                truck.Delivery();
            }

            //FACTORY DELIVERY
            if (hit.transform.tag == "FactoryDeliver" && !windowOpen) {

                Factory factory = hit.transform.root.GetComponent<Factory>();
                factory.SpawnOutput();

                print("factory deliver");
            }

            //FACTORY EMPLOYEE
            if (hit.transform.tag == "FactoryEmployee" && !windowOpen) {

                Factory factory = hit.transform.root.GetComponent<Factory>();
                factory.AwakeEmployee();

                print("factory awaken");

            }

            //TOLVA DISPATCH
            if (hit.transform.tag == "TolvaDeliver" && !windowOpen){

                print("deliver");
                tolva.DeliverToFactory();

            }

            //TOLVA EMPLOYEE
            if (hit.transform.tag == "TolvaEmployee" && !windowOpen) {

                print("tolva employee");
                tolva.AwakeEmployee();

            }

            //PARACHUTE
            if (hit.transform.tag == "Parachute" && !windowOpen){

                print("parachute");
                parachutes.ParachuteClicked();
            }


        }
    }

    // NEEDS TO HOLD FOR 0.5 SEC
    void CastRay() { 

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

         if (Physics.Raycast(ray, out hit, 100)){

            // RESEARCH

            if (hit.transform.tag == "Research" && !windowOpen){

                windowOpen = true;

                research.ToogleLevelUpWindow();
                print("research");
            }

            // TRUCK

            if (hit.transform.tag == "Truck" && !windowOpen){

                    windowOpen = true;

                    truck.ToogleLevelUpWindow();
                    print("Storage");
            }


                //PURCHASE
                if (hit.transform.tag == "PurchaseFactory" && !windowOpen){

                    if(tolva.FactoryIsActive(hit.transform.root.GetComponent<Factory>().index -1))
                    hit.transform.root.GetComponent<Factory>().PurchaseFactory();

                    print("Purhase");
                }

                //UPGRADE FACTORY
                if (hit.transform.tag == "UpgradeFactory" && !windowOpen){

                    Factory factory = hit.transform.root.GetComponent<Factory>();

                    if (factory.isActive) {

                        windowOpen = true;
                        factoryWindow.OpenFactoryWindow(factory, factory.index + 1);

                        print("factoryWindow");

                    }

                }


                //UPGRADE FRIDGE
                if (hit.transform.tag == "Fridge" && !windowOpen){

                    Factory factory = hit.transform.root.GetComponent<Factory>();

                    if (factory.isActive) {

                        windowOpen = true;
                        fridgeWindow.OpenFridgeWindow(factory.fridge, factory.index + 1);

                    }

                    print("fridge window");
                }

                //UPGRADE TOLVA
                if (hit.transform.tag == "Tolva" && !windowOpen){

                    windowOpen = true;

                    tolva.ToogleLevelUpWindow();
                    print("tolva");
                }

                //UPGRADE PACKING
                if (hit.transform.tag == "Packing" && !windowOpen){

                    windowOpen = true;

                    packing.ToogleLevelUpWindow();
                    print("packing");
                }

                //UPGRADE WAREHOUSE
                if (hit.transform.tag == "Warehouse" && !windowOpen){

                    windowOpen = true;

                    warehouse.ToogleLevelUpWindow();
                    print("warehouse");
                }

                //UPGRADE STORAGE
                if (hit.transform.tag == "Storage" && !windowOpen){

                    windowOpen = true;

                    storage.ToogleLevelUpWindow();
                    print("Storage");
                }

                //UPGRADE ELEVATOR
                if (hit.transform.tag == "Elevator" && !windowOpen){

                    windowOpen = true;

                    elevator.ToogleLevelUpWindow();
                    print("Elevator");
                }

            }


    }
}
