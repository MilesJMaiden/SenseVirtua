using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TransitionPoint : MonoBehaviour
{
    public SceneType sceneType;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            //FadeEffectCanvas.instance.StartEffect(CompleteFadeOut);


            SceneTransitionManager.singleton.GoToSceneAsync(sceneType.ToString());

        }


    }
    void CompleteFadeOut()
    {
        SceneManager.LoadScene(sceneType.ToString());
    }

}
public enum SceneType
{
    Beginning,
    Middle,
    End
}
