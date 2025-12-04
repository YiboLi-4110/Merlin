using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButtonSetting : MonoBehaviour
{
    // Start is called before the first frame update
    /*
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    */

    public void PauseAndExit()
    {
        Debug.Log("Merlin exit!");
        Application.Quit();
    }
}
