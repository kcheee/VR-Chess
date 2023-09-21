using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public void onclickStart()
    {
        Debug.Log("scene Change");
        SceneManager.LoadScene(1);
        
    }
}
