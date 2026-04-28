using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public string SceneToLoad;

    public void Start()
    {
        Debug.developerConsoleVisible = false;
    }
    
    public void LoadTheScene()
    {
        SceneManager.LoadScene(SceneToLoad);
    }
}