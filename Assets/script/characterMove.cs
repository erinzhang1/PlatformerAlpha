using System.Collections;
using System.Collections.Generic;
using script.User_Control;
using UnityEngine;

public class characterMove : MonoBehaviour
{
    Rigidbody rb;
    public Vector3 jump = new Vector3(0, 1, 0);
    public Vector3 move = new Vector3(0, 0, 1);
    public Vector3 fanLeft = new Vector3(-1, 0, 0);
    public const float jumpForce = 1.5f;
    public const float movementSpeed = 5;
    private UserControl platformControl;

    public string characterMode = "Stop";
    private Animator _animator;
    private switchCamera switchCamera;
    
    public bool characterHasKey = false;

    //public const int totalLife = 4;
    //public int remainLife;

    private SetupCameraLogic cameraLogic;
    public Vector3 setCameraPos = new Vector3(15, 2, 10);

    public int bonus = 0;

    private EnvironmentControl environmentControl;

    private goal goal;

    // audio
    private AudioSource footStep;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        platformControl = GameObject.FindObjectOfType<UserControl>();
        switchCamera = GameObject.FindObjectOfType<switchCamera>();
        cameraLogic = GameObject.FindObjectOfType<SetupCameraLogic>();
        environmentControl = GameObject.FindObjectOfType<EnvironmentControl>();
        goal = GameObject.FindObjectOfType<goal>();

        footStep = GetComponent<AudioSource>();
        footStep.enabled = false;

        _animator.enabled = false;
        //remainLife = totalLife;
    }

    void OnCollisionStay(Collision coll)
    {
        if (coll.gameObject.tag == "Spring")
        {
            characterMode = "Running";
            footStep.enabled = false;
            rb.AddForce(jump * jumpForce, ForceMode.Impulse);
            animation animation = coll.gameObject.GetComponent<animation>();
            animation.startAni();
        }
        if (characterMode == "OnWind" && coll.gameObject.tag == "Running")
        {
            characterMode = "Running";
        }
        if (characterMode != "Stop" && coll.gameObject.tag == "Running")
        {
            footStep.enabled = true;
        }
    }

    private void OnCollisionExit(Collision coll)
    {
        if (characterMode != "Stop" && coll.gameObject.tag == "Running")
        {
            footStep.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Fan")
        {
            if (characterMode == "Running")
            {
                characterMode = "OnWind";
                footStep.enabled = false;
                transform.position += move * Time.deltaTime * movementSpeed * 5;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // running = true;
        if (other.gameObject.tag == "Fan" && characterMode != "Stop")
        {
            characterMode = "OnWind";
            Vector3 fanRotation = other.gameObject.transform.rotation.eulerAngles;
            if (fanRotation == new Vector3(270f, 0f, 0f))
            {
                rb.velocity = new Vector3(0, 10, 0);
            }
            if (fanRotation == new Vector3(0f, 180f, 180f))
            {
                rb.velocity = new Vector3(0, -3, -7);
            }
            if (fanRotation == new Vector3(0f, 0f, 0f))
            {
                rb.velocity = new Vector3(0, -3, 7);
            }
            if (fanRotation == new Vector3(90f, 0f, 0f))
            {
                rb.velocity = new Vector3(0, -10, 0);
            }
        }
    }

    // start the game play
    private void startGame()
    {
        characterMode = "Running";
        footStep.enabled = true;
        _animator.enabled = true;
        switchCamera.startGameCamera();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown("e") && !goal.GameEnded)
        {
            if (characterMode == "Stop")
            {
                startGame();
                platformControl.startGame();
            } else
            {
                restart();
            }
        }
        if (transform.position.y < -30)
        {
            //remainLife -= 1;
            //if (remainLife == 0)
            //{
            //    gameOver();
            //} else
            //{
            //    restart();
            //}
            restart();
        }
        if (goal.GameEnded)
        {
            footStep.enabled = false;
        }
    }

    void FixedUpdate()
    {
        if (characterMode == "Stop")
        {
            rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        }
        if (characterMode == "Running")
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX;
            transform.position += move * Time.deltaTime * movementSpeed;
        }
    }
    
    public void UpdateKey()
    {
        characterHasKey = true;
    }

    // restart the game
    void restart()
    {
        footStep.enabled = false;
        _animator.Play("New State", 0, 0f);
        _animator.enabled = false;
        transform.position = new Vector3(0, 0, 0);
        rb.velocity = new Vector3(0, 0, 0);
        characterMode = "Stop";
        cameraLogic.moveCamera(setCameraPos);
        switchCamera.setGameCamera();
        platformControl.restart();

        environmentControl.resetPosition();
        characterHasKey = false;
        bonus = 0;
    }

    // when you lose all your life
    //void gameOver()
    //{
    //    _animator.Play("New State", 0, 0f);
    //    _animator.enabled = false;
    //    transform.position = new Vector3(0, 0, 0);
    //    rb.velocity = new Vector3(0, 0, 0);
    //    characterMode = "Stop";
    //    platformControl.destroyAll();
    //    cameraLogic.moveCamera(setCameraPos);
    //    switchCamera.setGameCamera();
    //    Time.timeScale = 0;

    //    environmentControl.resetPosition();
    //    characterHasKey = false;
    //    bonus = 0;
    //}
}
