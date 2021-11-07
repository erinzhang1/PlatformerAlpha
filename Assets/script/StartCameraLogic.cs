using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCameraLogic : MonoBehaviour
{
    public int levelEdge = 50;
    public int speed = 15;
    public switchCamera switchLogic;

    // Update is called once per frame
    void Update()
    {
        if (getCameraPos().z <= levelEdge)
        {
            transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
        } else
        {
            switchLogic.setGameCamera();
        }
    }

    public Vector3 getCameraPos()
    {
        return transform.position;
    }
}
