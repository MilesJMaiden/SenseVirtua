using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButtonManager : MonoBehaviour
{
    #region Custom Methods
    /// <summary>
    /// Method to load Arm Swinging Locomotion scene
    /// </summary>
    public void StartArmSwingingScene()
    {
        SceneManager.LoadScene("Jake Thesis Scene  - Arm Swing");
        Debug.Log("Load Arm Swing");
    }

    /// <summary>
    /// Method to load Teleportation Locomotion scene
    /// </summary>
    public void StartTeleportScene()
    {
        SceneManager.LoadScene("Jake Thesis Scene  - Teleport");
        Debug.Log("Load Teleport");
    }

    /// <summary>
    /// Method to load Starting scene
    /// </summary>
    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Jake Start Menu");
        Debug.Log("Load Start menu");
    }
    #endregion
}
