using System;
using System.Collections.Generic;
using script.Level_Items_Script;
using script.Level_Layout_Script;
using script.UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace script.User_Control
{
    [RequireComponent(typeof(LineRenderer))]
    public class UserControl : MonoBehaviour
    {
        public GameObject character;
        public GameObject springPlatform;
        public GameObject fanPlatform;
        public GameObject directionBoard;

        public BaseLevelItemScript nowSelected;
        // private int nowSelectedIndex;

        // a list contain all platform we make
        public List<BaseLevelItemScript> LevelItemList;

        public characterMove characterMove;
        private SetupCameraLogic cameraLogic;

        public int springLimit = 2;
        public int springCount = 0;

        public int fanLimit = 3;
        public int fanCount = 0;

        public int directionBoardLimit = 1;
        public int directionBoardCount = 0;

        private Vector3 DraggingOffset = Vector3.zero;
        private bool isDragging = false;
        private bool isDraggingRotating = false;
        public AutoResetCounter DraggingRotationMinTimeRequiredForHolding = new AutoResetCounter(0.1f);

        private Vector3 MouseWorldPosOnXZero = Vector3.zero;

        public goal goal;

        public DualPurposeCursor DPCursor;

        public LineRenderer LR;

        void Start()
        {
            LevelItemList = new List<BaseLevelItemScript>();
            characterMove = GameObject.FindObjectOfType<characterMove>();
            cameraLogic = GameObject.FindObjectOfType<SetupCameraLogic>();
            goal = GameObject.FindObjectOfType<goal>();
            DPCursor = GameObject.FindObjectOfType<DualPurposeCursor>();
            LR = (LR == null)? this.gameObject.GetComponent<LineRenderer>() : LR;
            Time.timeScale = 1;
            DraggingRotationMinTimeRequiredForHolding.MaxmizeTemp();
        }

        void Update()
        {
            if(goal.GameEnded) return;
            Ray ray = Camera.main.ScreenPointToRay(DPCursor.transform.position);
            isDragging = !(nowSelected == null) && isDragging;
            isDraggingRotating = !(nowSelected == null) && isDraggingRotating;
            nowSelected = (isDragging || isDraggingRotating) ? nowSelected : null;
            foreach (RaycastHit hitt in Physics.RaycastAll(ray, 1500))
            {
                BaseLevelItemScript baseLevelItemScript = hitt.collider.gameObject.GetComponent<BaseLevelItemScript>();
                if (
                    baseLevelItemScript != null 
                    && !baseLevelItemScript.Uncontrollable 
                    && !isDragging && !isDraggingRotating 
                    && !hitt.collider.isTrigger
                    )
                {
                    // DisHighLight the previous selection
                    if(nowSelected) nowSelected.DisHighlightMe();
                    
                    nowSelected = baseLevelItemScript;
                    nowSelected.HighlightMe();
                }
                
                BaseXZero XZero = hitt.collider.gameObject.GetComponent<BaseXZero>();
                if (XZero != null)
                {
                    MouseWorldPosOnXZero = hitt.point;
                }
            }
            
            // we can only add platform in stop mode
            if (characterMove.characterMode == CharaStates.Stop && (nowSelected != null))
            {
                if (DPCursor.SelectPressed)
                {
                    if (isDragging == false)
                    {
                        isDragging = true;
                        DraggingOffset = nowSelected.transform.position - MouseWorldPosOnXZero;
                    }

                    DraggingOffset = Vector3.zero;
                    nowSelected.SetMyPos(MouseWorldPosOnXZero + DraggingOffset);
                }
                else if (isDragging == true)
                {
                    isDragging = false;
                    nowSelected = null;
                }

                if (nowSelected != null)
                {

                    if ( (!isDraggingRotating) && DPCursor.RotatePressed)
                    {
                        nowSelected.RotateOnce();
                        DraggingRotationMinTimeRequiredForHolding.MaxmizeTemp();
                    }
                    else if(isDraggingRotating)
                    {
                        LR.positionCount = 2;
                        LR.SetPosition(0, nowSelected.transform.position);
                        LR.SetPosition(1, MouseWorldPosOnXZero);
                        Vector3 RelativeDir = (MouseWorldPosOnXZero - nowSelected.transform.position).normalized;
                        Vector3 targetDir = Vector3.right;
                        if (DraggingRotationMinTimeRequiredForHolding.IsZeroReached(Time.deltaTime, false))
                        {
                    
                            // // Yeah Ugly but I am too lazy to be graceful :P
                            // if (RelativeDir.y > 0.7f)
                            // {
                            //     targetDir *= 0;
                            //     nowSelected.RotateTo(targetDir);
                            // }
                            // else if (RelativeDir.z > 0.7f)
                            // {
                            //     targetDir *= 90;
                            //     nowSelected.RotateTo(targetDir);
                            // }
                            // else if (RelativeDir.z < -0.7f)
                            // {
                            //     targetDir *= -90;
                            //     nowSelected.RotateTo(targetDir);
                            // }
                            // else if (RelativeDir.y < -0.7f)
                            // {
                            //     targetDir *= -180;
                            //     nowSelected.RotateTo(targetDir);
                            // }
                            nowSelected.transform.LookAt(MouseWorldPosOnXZero);
                            nowSelected.RotateTo(nowSelected.transform.eulerAngles + (Vector3.right * 90));
                        }
                        
                    }
                }
                isDraggingRotating = DPCursor.RotatePressed;
            }
            
            if (!isDraggingRotating && LR.positionCount != 0)
            {
                LR.SetPosition(1, Vector3.Lerp(LR.GetPosition(1), LR.GetPosition(0), 0.1f));
                if((LR.GetPosition(1) - LR.GetPosition(0)).sqrMagnitude < 0.03f) LR.positionCount = 0;
            }
            
            // delete logic
            if (DPCursor.DeletePressed && (nowSelected != null) )
            {
                if (characterMove.characterMode == CharaStates.Stop)
                {
                    nowSelected.RemoveMe(this);
                }
            }

        }
        

        public void startGame()
        {
            restart();
        }

        public void restart()
        {
            resetPotentialSpring();
            if (LevelItemList.Count != 0)
            {
                foreach (var VARIABLE in LevelItemList)
                {
                    VARIABLE.DisHighlightMe();
                }
            }
        }

        private void resetPotentialSpring()
        {
            foreach (BaseLevelItemScript gameObject in LevelItemList)
            {
                if (gameObject.CompareTag("SpringPlatform"))
                {
                    Animator springAnimator = gameObject.GetComponent<Animator>();
                    springAnimator.Play("New State", 0, 0f);
                    springAnimator.Update(0);
                    springAnimator.enabled = false;
                }
            }
        }

        public void SpawnFan()
        {
            if (fanCount != fanLimit && characterMove.characterMode == CharaStates.Stop)
            {
                fanCount += 1;
                GameObject newnew = Instantiate(fanPlatform, MouseWorldPosOnXZero + (Vector3.up * 0), Quaternion.identity);
                nowSelected = newnew.GetComponent<BaseLevelItemScript>();
                nowSelected.SetControl(this);
                isDragging = true;
                LevelItemList.Add(nowSelected);
            }
        }

        public void SpawnSpringPlatform()
        {
            if (springCount != springLimit && characterMove.characterMode == CharaStates.Stop)
            {
                springCount += 1;
                GameObject newnew = Instantiate(springPlatform, MouseWorldPosOnXZero + (Vector3.up * 0), Quaternion.identity);
                nowSelected = newnew.GetComponent<BaseLevelItemScript>();
                nowSelected.SetControl(this);
                isDragging = true;
                LevelItemList.Add(nowSelected);
            }
        }
        
        public void SpawnDirectionBoard()
        {
            if (directionBoardCount != directionBoardLimit && characterMove.characterMode == CharaStates.Stop)
            {
                directionBoardCount += 1;
                GameObject newnew = Instantiate(directionBoard, MouseWorldPosOnXZero + (Vector3.up * 0), Quaternion.identity);
                nowSelected = newnew.GetComponent<BaseLevelItemScript>();
                nowSelected.SetControl(this);
                isDragging = true;
                LevelItemList.Add(nowSelected);
            }
        }
        
    }
}