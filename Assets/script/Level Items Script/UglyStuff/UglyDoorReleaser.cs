using System;
using script.UI;
using UnityEngine;

namespace script.Level_Items_Script.UglyStuff
{
    public class UglyDoorReleaser : MonoBehaviour
    {
        public Animation TheAnimation;
        public GameObject TargetGameObject;
        public StonePosIndicator SPI;

        private void OnTriggerEnter(Collider other)
        {
            if(!GameStateChecker.isTheCharaMoving) return;
            if (other.gameObject == TargetGameObject)
            {
                if(!TheAnimation.isPlaying) TheAnimation.Play();
                if (SPI != null)
                {
                    SPI.Check();
                }
                // Destroy(this);
            }
        }

        private void Update()
        {
            TargetGameObject.GetComponent<Collider>().isTrigger = !GameStateChecker.isTheCharaMoving;
        }
    }
}