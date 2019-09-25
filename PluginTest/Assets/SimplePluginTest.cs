using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class SimplePluginTest : MonoBehaviour
{
    const string DLL_NAME = "SIMPLEPLUGIN";

    [DllImport(DLL_NAME)]
    private static extern int SimpleFunction();

    public void SimpleFunctionCall()
    {
        Debug.Log(SimpleFunction());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SimpleFunctionCall();
        }
    }
}
