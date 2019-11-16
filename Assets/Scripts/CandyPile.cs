using UnityEngine;

public class CandyPile : MonoBehaviour{

    private int candyType = 0;
    public SpriteRenderer spriteRenderer;
    public Sprite [] candyImages;

    // 0 candies
    // 1 lollipop
    // 2 Gummies
    // 3 cup Cake
    // 4 donuts
    // 5 chocolate
    // 6 ice cream
    // 7 mellows
    // 8 gum
    // 9 eggs
    // 10 cookies
    // 11 cake

    public bool goToPacking = false;

    private Storage storage;
    private Packing packing;

    // Start is called before the first frame update
    void Awake(){

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = null;

        storage = FindObjectOfType<Storage>();
        packing = FindObjectOfType<Packing>();

    }

    public void SetCandyType(int type) {

        candyType = type;
        spriteRenderer.sprite = candyImages[type];
    }


    // Update is called once per frame
    void Update(){

        if (goToPacking) {

            transform.Translate(- storage.beltSpeed * Time.deltaTime, 0, 0);

            if(transform.position.x <= storage.unloadEndPos.position.x) {

                packing.LoadCandies();
                Destroy(gameObject);
            }

        }
        
    }
}
