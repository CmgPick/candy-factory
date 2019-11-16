using UnityEngine;

public class Dough : MonoBehaviour{

    public Factory targetFactory;

    public bool movingLeft = true;
    public float speed = 1f;

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void FixedUpdate(){

        if (movingLeft)
            transform.Translate(-speed * Time.deltaTime, 0, 0);

        if(transform.position.x <= -4.56) 
            movingLeft = false;

        if(!movingLeft)
            transform.Translate(0, -speed * Time.deltaTime, 0);

        if(transform.position.y <= targetFactory.doughHeight.position.y) {

            targetFactory.SpwanFabricDough();
            Destroy(gameObject);
        }

    }
}
