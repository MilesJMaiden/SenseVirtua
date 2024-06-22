using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTextureSetup : MonoBehaviour {

	public Camera sceneRenderCamera;
	//public Camera cameraB;

	public Material cameraMatA;
	//public Material cameraMatB;

	// Use this for initialization
	void Start () {
		if (sceneRenderCamera.targetTexture != null)
		{
            sceneRenderCamera.targetTexture.Release();
		}
        sceneRenderCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
		cameraMatA.mainTexture = sceneRenderCamera.targetTexture;

		//We will instead load into a new scene.

		//if (cameraB.targetTexture != null)
		//{
		//	cameraB.targetTexture.Release();
		//}
		//cameraB.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
		//cameraMatB.mainTexture = cameraB.targetTexture;
	}
	
}
