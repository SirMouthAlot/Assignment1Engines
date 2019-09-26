using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public enum ObjectType
{
    GREENCUBE,
    PURPLECUBE,
    REDCUBE,
    YELLOWCUBE,
    BLUECUBE,
}

//Holds info needed to reinstantiate a cube at same place
public struct ObjectInfo
{
    public ObjectInfo(Vector3 _position, Vector3 _rotation, ObjectType _type)
    {
        position = _position;
        rotation = _rotation;
        type = _type;
    }

    public Vector3 position;
    public Vector3 rotation;
    public ObjectType type;

} 

public class SpawnObject : MonoBehaviour
{
    const string DLL_NAME = "SIMPLEPLUGIN";

    [DllImport(DLL_NAME)]
    private static extern void SaveScene();
    [DllImport(DLL_NAME)]
    private static extern void SaveFile([MarshalAs(UnmanagedType.LPStr)]string name);
    
    [DllImport(DLL_NAME)]
    private static extern void LoadFile([MarshalAs(UnmanagedType.LPStr)]string name);
    [DllImport(DLL_NAME)]
    private static extern void LoadScene();


    [DllImport(DLL_NAME)]
    private static extern void AddObject(float[] _position, float[] _rotation, int _type);
    [DllImport(DLL_NAME)]
    private static extern float[] GetObjectPositionInst(int index);
    [DllImport(DLL_NAME)]
    private static extern void DeleteObjectPositionInst(float[] pos);
    [DllImport(DLL_NAME)]
    private static extern float[] GetObjectRotationInst(int index);
    [DllImport(DLL_NAME)]
    private static extern void DeleteObjectRotationInst(float[] pos);
    [DllImport(DLL_NAME)]
    private static extern int GetObjectType(int index);
    
    [DllImport(DLL_NAME)]
    private static extern void ClearList();

    public List<GameObject> LevelObjects;

    private GameObject tempHold;
    private bool objSelected = false;

    private List<GameObject> objectsInScene = new List<GameObject>();
    private List<GameObject> objectsThisSession = new List<GameObject>();
    private List<ObjectInfo> redoList = new List<ObjectInfo>();

    public string SceneName = "";

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

            //Set transform position to p, as well as allows you to scroll to move up and down on z-axis
            tempHold.transform.position = new Vector3(p.x, p.y, tempHold.transform.position.z + Input.mouseScrollDelta.y);
        }

        if (Input.GetMouseButtonDown(0))
        {
            //if there is currently an object selected
            if (objSelected)
            {
                //Now that we've decided on a final spot for the object, add the object to the list of objects in the scene
                objectsInScene.Add(tempHold);
                //Add the object to the list of objects we've added this session
                objectsThisSession.Add(tempHold);
            }

            objSelected = false;
        }
    }

    public void SaveGame()
    {   
        Debug.Log("Please Kill me");

        ClearList();

        for (int i = 0; i < objectsInScene.Count; i++)
        {
           //Gets game object
            GameObject obj = objectsInScene[i];
            float[] pos = { obj.transform.position.x, obj.transform.position.y, obj.transform.position.z };
            float[] rot = { obj.transform.rotation.eulerAngles.x, obj.transform.rotation.eulerAngles.y, obj.transform.rotation.eulerAngles.z };

            //Add object to scene list
            AddObject(pos, rot, int.Parse(obj.tag));
        }

        SaveScene();

        SaveFile(SceneName);
    }

    public void LoadGame()
    {
        Debug.Log("Don't Kill me now bitch");
        
        LoadFile(SceneName);
        LoadScene();

        
    }

    //Spawns object in the scene, at a specific position and rotation.
    public GameObject SpawnObjScene(GameObject obj, Vector3 _position, Vector3 _rotation)
    {
        //Instantiates an object at this position, vector and rotation
        return Instantiate(obj, _position, Quaternion.Euler(_rotation));
    }

    //Spawns object using a button using just the prefab object.
    //You can then use your mouse to move the object around until you click to place it
    public void SpawnObjButton(GameObject obj)
    {
        //Holds the recently spawned object
        tempHold = Instantiate(obj);
        objSelected = true;

        //Clears redo list if you spawn in an object
        //Can't redo if you've already changed the history
        redoList.Clear();
    }

    public void RemoveLastObject()
    {
        //Will only remove objects added this session, will not undo anything from before
        if (objectsThisSession.Count >= 1)
        {
            //Gets game object
            GameObject obj = objectsThisSession[objectsThisSession.Count-1];

            //Adds to redo list
            redoList.Add(new ObjectInfo(obj.transform.position, obj.transform.rotation.eulerAngles, (ObjectType)int.Parse(obj.tag)));

            //removes from session list
            objectsThisSession.RemoveAt(objectsThisSession.Count-1);
            //Removes from main list
            objectsInScene.RemoveAt(objectsInScene.Count-1);

            //Destroys instance
            Destroy(obj);
        }
        else
        {
            Debug.Log("Nothing Left to undo!");
        }
    }

    public void AddBackLastRemoved()
    {
        if (redoList.Count >= 1)
        {
            //Gets object info
            ObjectInfo info = redoList[redoList.Count-1];

            //Adds to session list
            objectsThisSession.Add(SpawnObjScene(LevelObjects[(int)info.type], info.position, info.rotation));
            //Adds to main list
            objectsInScene.Add(objectsThisSession[objectsThisSession.Count-1]);

            //removes from redo list
            redoList.RemoveAt(redoList.Count-1);
        }
        else
        {
            Debug.Log("Nothing Left to redo!");
        }
    }
}
