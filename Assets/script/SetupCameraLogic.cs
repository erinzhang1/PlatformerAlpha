using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupCameraLogic : MonoBehaviour
{
    public Vector2 MouseStartPos;
    public Vector3 CamStartPos;
    public float Ratio = 0.01f;
    public bool isDragging;

    void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        //
        // if ((Input.GetKey(KeyCode.RightArrow) || Screen.width - mousePos.x < mouseEdgeBuffer) && getCameraPos().z <= rightEdge)
        // {
        //     transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
        // }
        // if ((Input.GetKey(KeyCode.LeftArrow) || mousePos.x < mouseEdgeBuffer) && getCameraPos().z >= leftEdge)
        // {
        //     transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
        // }
        // if ((Input.GetKey(KeyCode.DownArrow) || mousePos.y < mouseEdgeBuffer) && getCameraPos().y >= bottomEdge)
        // {
        //     transform.Translate(new Vector3(0, -speed * Time.deltaTime, 0));
        // }
        // if ((Input.GetKey(KeyCode.UpArrow) || Screen.height - mousePos.y < mouseEdgeBuffer) && getCameraPos().y <= topEdge)
        // {
        //     transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
        // }

        if (Input.GetMouseButton(2))
        {
            
            if (isDragging == false)
            {
                isDragging = true;
                MouseStartPos = mousePos;
                CamStartPos = this.transform.position;
            }

            Vector3 diff = mousePos - MouseStartPos;

            this.transform.position = CamStartPos - new Vector3(0, diff.y * Ratio, diff.x * Ratio);
        }                                                                                        
        else                                                                                     
        {                                                                                        
            isDragging = false;                                                                  
        }                                                                                        
        
    }
    public void moveCamera(Vector3 position)
    {
        transform.position = position;
    }

    public Vector3 getCameraPos()
    {
        return transform.position;
    }
}
