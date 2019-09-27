using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

//Enum to indicate each prefab for Level editor
public enum ObjectType
{
    GREENCUBE,
    PURPLECUBE,
    REDCUBE,
    YELLOWCUBE,
    BLUECUBE,
}

//Holds info needed to reinstantiate a prefab at same place
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

    //All my DLL function imports... there are a lot
    //Could possibly start marshalling structs in
    //But for now, for this specific assignment, I think this should suffice
    #region DLL IMPORTS
    const string DLL_NAME = "SIMPLEPLUGIN";

    //Saves the objects to the json object
    [DllImport(DLL_NAME)]
    private static extern void SaveScene();
    //Saves the json object to the file
    [DllImport(DLL_NAME)]
    private static extern void SaveFile([MarshalAs(UnmanagedType.LPStr)]string name);
    
    //Loads the json object in from the file
    [DllImport(DLL_NAME)]
    private static extern void LoadFile([MarshalAs(UnmanagedType.LPStr)]string name);
    //Loads in the objects from the json object
    [DllImport(DLL_NAME)]
    private static extern void LoadScene();

    //Add Object to the list
    [DllImport(DLL_NAME)]
    private static extern void AddObject(float[] _position, float[] _rotation, int _type);

    //Gets position components
    [DllImport(DLL_NAME)]
    private static extern float GetObjectPosX(int index);
    [DllImport(DLL_NAME)]
    private static extern float GetObjectPosY(int index);
    [DllImport(DLL_NAME)]
    private static extern float GetObjectPosZ(int index);
    
    //Gets rotation components
    [DllImport(DLL_NAME)]
    private static extern float GetObjectRotX(int index);
    [DllImport(DLL_NAME)]
    private static extern float GetObjectRotY(int index);
    [DllImport(DLL_NAME)]
    private static extern float GetObjectRotZ(int index);

    //Gets object type
    [DllImport(DLL_NAME)]
    private static extern int GetObjectType(int index);
    
    //Gets the object count
    [DllImport(DLL_NAME)]
    private static extern int GetObjectCount();

    //Clears out the list to prep for new save version
    [DllImport(DLL_NAME)]
    private static extern void ClearList();
    #endregion

    //Object prefabs for instantiating
    public List<GameObject> LevelObjects;

    //Holds the game object you're currently selecting
    private GameObject tempHold;
    //Do you have anything selected???
    private bool objSelected = false;

    //Lists for
    /// Objects In Scene altogether
    /// Objects that were added during THIS session
    /// List of object info left so we can redo
    private List<GameObject> objectsInScene = new List<GameObject>();
    private List<GameObject> objectsThisSession = new List<GameObject>();
    private List<ObjectInfo> redoList = new List<ObjectInfo>();

    //Name of the file for the scene (can be changed mid run to create/load more levels)
    //This change happens within the Inspector
    //Please do not put a filename here that doesn't exist and then attempt to load, as it will crash Unity
    public string SceneName = "";

   //Allow for the object to follow your mouse until you click to place it
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

                tempHold = null;
            }

            objSelected = false;
        }
    }

    //Saves the level on screen as a file with the a name equal to the one written in the Inspector as LevelName
    public void SaveGame()
    {   
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

    //Loads the current written string (in inspector) as a level
    //TODO: Check to make sure the file actually exists before attempting to load
    public void LoadGame()
    {
        ClearGame();

        LoadFile(SceneName);
        LoadScene();

        int objectCount = GetObjectCount();

        for (int i = 0; i < objectCount; i++)
        {
            //Converts to Vector3
            Vector3 pos = new Vector3(GetObjectPosX(i), GetObjectPosY(i), GetObjectPosZ(i));
            //Converts to Vector3
            Vector3 rot = new Vector3(GetObjectRotX(i), GetObjectRotY(i), GetObjectRotZ(i));
            //Casts it to ObjectType
            ObjectType type = (ObjectType)GetObjectType(i);

            //Adds objects to scene
            objectsInScene.Add(SpawnObjScene(LevelObjects[(int)type], pos, rot));
        }
    }

    public void ClearGame()
    {
         //Cleans up tempHold
        tempHold = null;

        //Destroys all objects in scene currently
        for (int i = 0; i < objectsInScene.Count; i++)
        {
            Destroy(objectsInScene[i]);
        }

        //Clears objects in scene
        objectsInScene.Clear();
        //Clears objects this session
        objectsThisSession.Clear();
        //clears redo list
        redoList.Clear();

        //Clears vector in DLL
        ClearList();
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

    //Undo function
    //Will be reworked into a command style call
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

    //Redo function
    //Will be reworked into a command style call
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
