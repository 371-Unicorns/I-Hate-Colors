using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages scenes and makes sure each scene is loaded.
/// </summary>
public class SceneLoader : MonoBehaviour
{

    /// <summary>
    /// Loads requested scene using Scene Manager
    /// 
    /// Author: Courtney Chu
    /// </summary>
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
