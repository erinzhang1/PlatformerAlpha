﻿using System;
using System.Collections.Generic;
using script.Level_Items_Script;
using script.Level_Layout_Script;
using UnityEngine;
using Random = UnityEngine.Random;

namespace script.User_Control
{
    public class UserControl : MonoBehaviour
    {
        public GameObject character;
        public GameObject springPlatform;
        public GameObject fanPlatform;

        public BaseLevelItemScript nowSelected;
        private int nowSelectedIndex;

        // a list contain all platform we make
        public List<BaseLevelItemScript> LevelItemList;

        public characterMove characterMove;
        private SetupCameraLogic cameraLogic;

        public int springLimit = 2;
        public int springCount = 0;

        public int fanLimit = 3;
        public int fanCount = 0;
        
        private Vector3 DraggingOffset = Vector3.zero;
        private bool isDragging = false;
        private bool isDraggingRotating = false;

        private Vector3 MouseWorldPosOnXZero = Vector3.zero;

        public goal goal;

        public DualPurposeCursor DPCursor;

        void Start()
        {
            LevelItemList = new List<BaseLevelItemScript>();
            characterMove = GameObject.FindObjectOfType<characterMove>();
            cameraLogic = GameObject.FindObjectOfType<SetupCameraLogic>();
            goal = GameObject.FindObjectOfType<goal>();
            DPCursor = GameObject.FindObjectOfType<DualPurposeCursor>();
            Time.timeScale = 1;
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
                if (baseLevelItemScript != null && !isDragging && !hitt.collider.isTrigger)
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

                isDraggingRotating = DPCursor.RotatePressed;
                if (isDraggingRotating)
                {
                    Vector3 RelativeDir = (MouseWorldPosOnXZero - nowSelected.transform.position).normalized;
                    Vector3 targetDir = Vector3.right;
                    
                    // Yeah Ugly but I am too lazy to be graceful :P
                    if (RelativeDir.y > 0.9f)
                    {
                        targetDir *= 0;
                        nowSelected.RotateTo(targetDir);
                    }
                    else if (RelativeDir.z > 0.9f)
                    {
                        targetDir *= 90;
                        nowSelected.RotateTo(targetDir);
                    }
                    else if (RelativeDir.z < -0.9f)
                    {
                        targetDir *= -90;
                        nowSelected.RotateTo(targetDir);
                    }
                    else if (RelativeDir.y < -0.9f)
                    {
                        targetDir *= -180;
                        nowSelected.RotateTo(targetDir);
                    }


                }
            }

            // delete logic
            if (DPCursor.DeletePressed && (nowSelected != null) )
            {
                if (characterMove.characterMode == CharaStates.Stop)
                {
                    nowSelected.RemoveMe(this);
                }
                else
                {
                    // nowSelected.RemoveMeInGame(this);
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
        
        
        
    }
}