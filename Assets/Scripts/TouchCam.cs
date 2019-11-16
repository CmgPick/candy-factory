
using UnityEngine;

// 2d camera panning script


public class TouchCam : MonoBehaviour{

    public Vector3 minPos;
    public Vector3 maxPos;

    private Transform cam;
    private Vector3 touchStart;

    private Vector3 startPos;
    public float upBtnPosition;
    public GameObject upBtn;

    private MouseController mouseController;



    // Start is called before the first frame update
    void Start(){

        cam = Camera.main.transform;
        startPos = cam.position;
        mouseController = FindObjectOfType<MouseController>();
        
    }

    public void GoUp() {

        cam.position = startPos;
    }

    // Update is called once per frame
    void Update(){

        if (transform.position.y < upBtnPosition)
            upBtn.SetActive(true);
        else
            upBtn.SetActive(false);

        if (Input.GetMouseButtonDown(0) && !mouseController.windowOpen) {

            touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }


        if (Input.GetMouseButton(0) && !mouseController.windowOpen) {

            Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cam.position += direction;

    
            if (cam.position.x < minPos.x)
                cam.position = new Vector3(minPos.x, cam.position.y, cam.position.z);

            if (cam.position.x > maxPos.x)
                cam.position = new Vector3(maxPos.x, cam.position.y, cam.position.z);

            if (cam.position.y < minPos.y)
                cam.position = new Vector3(cam.position.x, minPos.y, cam.position.z);

            if (cam.position.y > maxPos.y)
                cam.position = new Vector3(cam.position.x, maxPos.y, cam.position.z);

        }


    }
}
