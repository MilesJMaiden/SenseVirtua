using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GlazeRepair : MonoBehaviour
{
    public GameObject otherPiece;
    public GameObject leftPiece;
    public GameObject wholePiece;
    public bool isLeftHandPiece;
    public MeshRenderer wholeLeftMeshrenderer;
    public AudioSource glazeAudio;

    private XRGrabInteractable grabInteractable;
    public bool isCombined = false;


    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.onSelectExited.AddListener(OnRelease);

    }

    void OnDestroy()
    {
        grabInteractable.onSelectExited.RemoveListener(OnRelease);
    }

    void OnCollisionEnter(Collision collision)
    {
        // when trigger entered check if that is the other piece

        Debug.Log("entering");
        if (collision.gameObject == otherPiece.gameObject && !isCombined)
        {
            isCombined = true;
        }
        else
            Debug.Log("not combined!!");
    }
    private void Update()
    {
        if(isCombined)
        {
            CombinePieces();
        }
    }

    public void OnRelease(XRBaseInteractor interactor)
    {
        // if it is, combine the two when release any of the 2
        if (isCombined)
        {
            CombinePieces();
        }

    }

    public void CombinePieces()
    {
              
        // Disable the left
        leftPiece.SetActive(false);

        // Enable the right-left mesh
        wholeLeftMeshrenderer.enabled = true;

        // Play the audio clip
        glazeAudio.Play();

        isCombined = false;
        

        
    }


   
}
