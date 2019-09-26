using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class SimplePluginTest : MonoBehaviour
{
    const string DLL_NAME = "SIMPLEPLUGIN";

    [DllImport(DLL_NAME)]
    private static extern void SaveScene();
    [DllImport(DLL_NAME)]
    private static extern void SaveFile(string name);
    
    [DllImport(DLL_NAME)]
    private static extern void LoadFile(string name);
    [DllImport(DLL_NAME)]
    private static extern void LoadScene();


    [DllImport(DLL_NAME)]
    private static extern void AddObject(ObjectInfo obj);
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
    
    public void SimpleFunctionCall()
    {
        
    }

    void Update()
    {
      
    }
}
