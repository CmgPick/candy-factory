using UnityEngine;

public class Box : MonoBehaviour{

    public float speed = 1;

    public bool movingUp = false;
    public bool movingRight = false;
    public bool willFall = false;

    private Warehouse warehouse;
    private Truck truck;

    // Start is called before the first frame update
    void Start() {

        warehouse = FindObjectOfType<Warehouse>();
        truck = FindObjectOfType<Truck>();
    }

    // Update is called once per frame
    void FixedUpdate(){

        if(movingUp)
        transform.Translate(0, speed * Time.deltaTime, 0);

        // reached up pos
        if(movingUp && transform.position.y >= warehouse.boxUpPos.position.y){

            warehouse.StoreBox();
            movingUp = false;
            Destroy(gameObject);
        }

        if(movingRight)
            transform.Translate(speed * Time.deltaTime, 0, 0);

        if(willFall)
            transform.Translate(0 , -speed * 1.5f * Time.deltaTime, 0);

        if(willFall && transform.position.y <= warehouse.boxFallPos.position.y) {

            warehouse.player.BoxesDropped();
            Destroy(gameObject);
        }


        if ( movingRight && transform.position.x >= warehouse.boxRightEndPos.position.x && !willFall){

            if (!truck.isMoving){

                movingRight = false;
                truck.LoadBox();

                Destroy(gameObject);
            }
            else
                willFall = true;

        }

    }
}
