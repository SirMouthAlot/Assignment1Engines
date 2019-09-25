using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnObject : MonoBehaviour
{
    public GameObject greenCube;
    public GameObject purpleCube;
    public GameObject redCube;
    public GameObject yellowCube;
    public GameObject blueCube;

    private GameObject tempHold;
    private bool objSelected = false;

    // Update is called once per frame
    void Update()
    {
       
        if (objSelected)
        {
            //Gets Mouse position
            Vector3 mousePoint = Input.mousePosition;
            //Counteracts the camera z-position
            mousePoint.z = 10.0f;
            //Converts mouse position to world position
            Vector3 p = Camera.main.ScreenToWorldPoint(mousePoint);

            //Set transform position to world point
            tempHold.transform.position = new Vector3(p.x, p.y, tempHold.transform.position.z + Input.mouseScrollDelta.y);
        }

        if (Input.GetMouseButtonDown(0))
        {
            objSelected = false;
        }
    }

    public void SpawnObjScene(GameObject obj, Vector3 _position, Vector3 _rotation)
    {
        Instantiate(obj, _position, Quaternion.Euler(_rotation.x, _rotation.y, _rotation.z));
    }

    public void SpawnObjButton(GameObject obj)
    {
        //Holds the recently spawned object
        tempHold = Instantiate(obj);
        objSelected = true;
    }
}
