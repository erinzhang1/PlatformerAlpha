using System.Collections;
using System.Collections.Generic;
using script.User_Control;
using UnityEngine;
using UnityEngine.UI;

public class springUpdate : MonoBehaviour
{
    Image sr;

    public Sprite spring0;
    public Sprite spring1;
    public Sprite spring2;
    public Sprite spring3;
    public Sprite spring4;
    public Sprite spring5;
    public Sprite spring6;
    public Sprite spring7;
    public Sprite spring8;
    public Sprite spring9;
    public Sprite spring10;
    public UserControl springLogic;
    
    void Start()
    {
        sr = this.gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        int toSet = springLogic.springLimit - springLogic.springCount;
        switch (toSet)
        {
            case 0:
                sr.sprite = spring0;
                break;
            case 1:
                sr.sprite = spring1;
                break;
            case 2:
                sr.sprite = spring2;
                break;
            case 3:
                sr.sprite = spring3;
                break;
            case 4:
                sr.sprite = spring4;
                break;
            case 5:
                sr.sprite = spring5;
                break;
            case 6:
                sr.sprite = spring6;
                break;
            case 7:
                sr.sprite = spring7;
                break;
            case 8:
                sr.sprite = spring8;
                break;
            case 9:
                sr.sprite = spring9;
                break;
            default:
                sr.sprite = spring10;
                break;
        }
    }
}
