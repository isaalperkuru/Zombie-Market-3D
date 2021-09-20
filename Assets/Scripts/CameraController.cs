using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform target;

    private GameManager gameManager;
    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("CameraPoint").transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null && !gameManager.GetLevelFinish)
        {
            transform.position = target.position;
            transform.rotation = target.rotation;
        }
        if(gameManager.GetLevelFinish)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }
}
