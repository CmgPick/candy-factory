using UnityEngine;

public class Fridge : MonoBehaviour{

    public int candyType;

    public int stored = 0;
    public int maxStorage = 5;
    public int initialMaxStorage = 5;

    public GameObject[] content;

    [Header("LevelUps")]

    public int level = 1;
    public int levelCost = 500;


    public void SetCandyType() {

        for (int i = 0; i < content.Length; i++){

            content[i].GetComponent<CandyPile>().SetCandyType(candyType);
            content[i].SetActive(false);
        }

        UpdateContent();

    }

    public void Store() {

        if (stored < maxStorage)
            stored++;

        UpdateContent();
    }

    public void Unload() {

        if (stored > 0)
            stored--;

        UpdateContent();
    }

    void UpdateContent(){

        for (int i = 0; i < maxStorage; i++){

            if (i < stored)
                content[i].SetActive(true);
            else
                content[i].SetActive(false);

        }
        
    }
}
