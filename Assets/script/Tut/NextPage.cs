using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class NextPage : MonoBehaviour
{
    public GameObject springTutUI;
    public GameObject fanTutUI;
    public GameObject rotateUI;
    public GameObject deleteUI;
    public GameObject startUI;
    public GameObject cameraUI;

    private TutoralManagement tutoralManagement;

    private void Start()
    {
        tutoralManagement = GameObject.FindObjectOfType<TutoralManagement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button0) && tutoralManagement.nowActive != "")
        {
            switch(tutoralManagement.nowActive)
            {
                case "springTut":
                    springClickNext();
                    break;
                case "start":
                    startClickClose();
                    break;
                case "delete":
                    deleteClickNext();
                    break;
                case "fanTut":
                    fanClickNext();
                    break;
                case "rotate":
                    rotateClickNext();
                    break;
            }
        }
    }

    public void springClickNext()
    {
        tutoralManagement.nowActive = "start";
        springTutUI.SetActive(false);
        startUI.SetActive(true);
    }
    
    public void fanClickNext()
    {
        tutoralManagement.nowActive = "rotate";
        fanTutUI.SetActive(false);
        cameraUI.SetActive(true);
    }
    
    public void rotateClickNext()
    {
        tutoralManagement.nowActive = "";
        rotateUI.SetActive(false);
        tutoralManagement.stage2();
    }
    
    public void deleteClickNext()
    {
        tutoralManagement.nowActive = "";
        deleteUI.SetActive(false);
        tutoralManagement.stage1();
    }
    
    public void startClickClose()
    {
        tutoralManagement.nowActive = "delete";
        startUI.SetActive(false);
        deleteUI.SetActive(true);
    }

    public void cameraClickNext()
    {
        tutoralManagement.nowActive = "camera";
        cameraUI.SetActive(false);
        rotateUI.SetActive(true);
    }
}
