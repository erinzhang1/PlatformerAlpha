using System.Collections;
using System.Collections.Generic;
using script.User_Control;
using UnityEngine;
using UnityEngine.UI;

public class fanUpdate : MonoBehaviour
{
    Image sr;

    public Sprite fan0;
    public Sprite fan1;
    public Sprite fan2;
    public Sprite fan3;
    public Sprite fan4;
    public Sprite fan5;
    public Sprite fan6;
    public Sprite fan7;
    public Sprite fan8;
    public Sprite fan9;
    public Sprite fan10;
    public UserControl fanLogic;

    void Start()
    {
        sr = this.gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        int toSet = fanLogic.fanLimit - fanLogic.fanCount;
        switch(toSet)
        {
            case 0:
                sr.sprite = fan0;
                break;
            case 1:
                sr.sprite = fan1;
                break;
            case 2:
                sr.sprite = fan2;
                break;
            case 3:
                sr.sprite = fan3;
                break;
            case 4:
                sr.sprite = fan4;
                break;
            case 5:
                sr.sprite = fan5;
                break;
            case 6:
                sr.sprite = fan6;
                break;
            case 7:
                sr.sprite = fan7;
                break;
            case 8:
                sr.sprite = fan8;
                break;
            case 9:
                sr.sprite = fan9;
                break;
            default:
                sr.sprite = fan10;
                break;
        }
    }
}
