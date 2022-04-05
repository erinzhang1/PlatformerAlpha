using System;
using System.Collections.Generic;
using script.UI;
using script.User_Control;
using UnityEngine;
using Random = UnityEngine.Random;

namespace script.Level_Items_Script
{
    public class BaseLevelItemScript : MonoBehaviour
    {
        public bool MeTempRemoved = false;
        public Material normal;
        public Material outline;

        public List<Renderer> ToBeSwitchedRenderers;

        public UserControl control;
        
        public bool Uncontrollable = false;

        protected Vector3 TempLocalScale;

        /// <summary>
        /// This is for bound detection of the platforms for deployment.
        /// This collider can (and probably should) be disabled
        /// This collider should contain all part of the platform
        /// </summary>
        public Collider OuterFrame;

        protected Vector3 targetLerpToPosition;


        private Vector3 tempPos = Vector3.one;

        private bool JustMoved = false;

        private float LerpMultiplier = 0.2f;

        protected Vector3 TargetEuler = Vector3.zero;

        protected Vector3 RealEuler = Vector3.zero;

        public GameObject DestoryParticleEffect;

        public AudioSource deleteSound;

        public AutoResetCounter ShakyAutoRemoveProgress = new AutoResetCounter(2f);

        protected virtual void Start()
        {
            TempLocalScale = this.transform.localScale;
            Debug.Assert(OuterFrame != null, $"THE OUTER FRAME IS NULL FOR {this.gameObject.name}");
            tempPos = this.transform.position;
            targetLerpToPosition = this.transform.position;
            TargetEuler = this.transform.eulerAngles;
            deleteSound = GameObject.Find("DeletedSFX").GetComponent<AudioSource>();
            ShakyAutoRemoveProgress.MaxmizeTemp();
        }
        public virtual void SetControl(UserControl uc)
        {
            control = uc;
        }
        
        public virtual void HighlightMe()
        {
            if(Uncontrollable) return;
            foreach (Renderer rdr in ToBeSwitchedRenderers)
            {
                Material[] temp = new Material[2];
                temp[0] = normal;
                temp[1] = outline;
                rdr.materials = temp;
            }
        }
        
        public virtual void DisHighlightMe()
        {
            if(Uncontrollable) return;
            foreach (Renderer rdr in ToBeSwitchedRenderers)
            {
                Material[] temp = new Material[1];
                temp[0] = normal;
                rdr.materials = temp;
            }
        }

        public virtual void RemoveMe(UserControl uc)
        {
            if(Uncontrollable) return;
            if (DestoryParticleEffect != null)
            {
                GameObject go = Instantiate(DestoryParticleEffect, this.transform.position, this.transform.rotation);
                go.transform.parent = null;
            }
            if (deleteSound)
            {
                deleteSound.enabled = true;
                deleteSound.Play();
            }
            uc.LevelItemList.Remove(this);
            Destroy(this.gameObject);
        }

        protected virtual void Update()
        {
            this.transform.position = Vector3.Lerp(this.transform.position, targetLerpToPosition, LerpMultiplier);
            if (control && control.characterMove.characterMode == CharaStates.Stop)
            {
                if (MeTempRemoved)
                {
                    MeTempRemoved = false;
                    foreach (Renderer rdr in this.gameObject.GetComponentsInChildren<Renderer>())
                    {
                        rdr.enabled = true;
                    }
                    foreach (Collider cldr in this.gameObject.GetComponentsInChildren<Collider>())
                    {
                        cldr.enabled = true;
                    }
                    this.transform.localScale = TempLocalScale;
                }
            }

            if (control && control.nowSelected != this)
            {
                this.DisHighlightMe();
            }

            CheckPositionAvailable();
            ShakyAutoRemoveProgress.Temp = Mathf.Clamp(ShakyAutoRemoveProgress.Temp + (0.5f * Time.deltaTime), 0f, ShakyAutoRemoveProgress.Max);

            ClampEuler();
        }

        protected virtual void CheckPositionAvailable()
        {
            if(Uncontrollable) return;
            if (JustMoved)
            {
                JustMoved = false;
            }
            else if (control)
            {
                foreach (var other in control.LevelItemList)
                {
                    if (this.OuterFrame.bounds.Intersects(other.OuterFrame.bounds) && (other.gameObject != this.gameObject))
                    {
                        Vector3 Offset = new Vector3(0, 
                            (1-ShakyAutoRemoveProgress.Ratio()) * 0.25f * Random.Range(-0.5f, 0.5f), 
                            (1-ShakyAutoRemoveProgress.Ratio()) * 0.25f * Random.Range(-0.5f, 0.5f)
                        );
                        this.transform.position = this.transform.position + Offset;
                        if(ShakyAutoRemoveProgress.IsZeroReached(Time.deltaTime * 2)) RemoveMe(control);
                        // targetLerpToPosition = this.tempPos;
                        JustMoved = true;
                        return;
                    }
                }

                this.tempPos = targetLerpToPosition;
            }
        }

        public virtual void RemoveMeInGame(UserControl uc)
        {
            if(Uncontrollable) return;
            MeTempRemoved = true;
            foreach (Renderer rdr in this.gameObject.GetComponentsInChildren<Renderer>())
            {
                rdr.enabled = false;
            }
            foreach (Collider cldr in this.gameObject.GetComponentsInChildren<Collider>())
            {
                cldr.enabled = false;
            }

            this.transform.localScale = Vector3.zero;
        }

        public virtual void SetMyPos(Vector3 pos)
        {
            if(Uncontrollable) return;
            if (JustMoved)
            {
                JustMoved = false;
            }
            else
            {
                targetLerpToPosition = pos;
                JustMoved = true;
            }
        }

        public virtual void RotateOnce()
        {
            if(Uncontrollable) return;
            TargetEuler += (new Vector3(-90f, 0f, 0f));
            if (TargetEuler.x < -360)
            {
                TargetEuler += Vector3.right * 360;
            }
        }

        public virtual void RotateTo(Vector3 Euler)
        {
            if(Uncontrollable) return;
            TargetEuler = Euler;
        }
        
        
        public virtual void ClampEuler()
        {
            if(Uncontrollable) return;
            Vector3 Regulated = TargetEuler;
            RealEuler = Vector3.Lerp(RealEuler, Regulated,  0.2f);
            this.transform.eulerAngles = RealEuler;
        }
    }
}