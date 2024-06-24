using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{


    public void Start()
    {
        GameObject player = GameObject.Find("Player");
        player.transform.position = transform.position;
    }
}

