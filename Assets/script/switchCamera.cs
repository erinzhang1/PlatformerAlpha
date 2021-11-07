using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchCamera : MonoBehaviour
{
    private GameObject mainCamera;
    private GameObject setupCamera;
    private GameObject startCamera;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.Find("Main Camera");
        setupCamera = GameObject.Find("Setup Camera");
        startCamera = GameObject.Find("Start Camera");
        mainCamera.SetActive(false);
        setupCamera.SetActive(false);
        startCamera.SetActive(true);
    }

    public void startStartCamera()
    {
        setupCamera.SetActive(false);
        startCamera.SetActive(true);
        mainCamera.SetActive(false);
    }

    public void startGameCamera()
    {
        setupCamera.SetActive(false);
        startCamera.SetActive(false);
        mainCamera.SetActive(true);
    }

    public void setGameCamera()
    {
        setupCamera.SetActive(true);
        startCamera.SetActive(false);
        mainCamera.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
