using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipScene : MonoBehaviour
{
    public SceneType sceneType;
    public CalligraphyGame game;
    public void OnClickedSkipBtn()
    {
        SceneTransitionManager.singleton.GoToSceneAsync(sceneType.ToString());
    }
    public void OnClickedSkipDrawingBtn()
    {
        game.SkipCurrentDrawTask();
    }
}
