using System;
using UnityEngine;

namespace Platformer.CutScenes
{
    public class CutsceneTrigger: MonoBehaviour
    {
        [SerializeField] private CutsceneData CutsceneData;

        [Tooltip(
            "Play the cutscene on the Start, otherwise the cutscene will be triggered by colider. Make sure no overlap")]
        [SerializeField] private bool playOnStart;
        [SerializeField] private bool needToPressButton;
        [SerializeField] private bool onlyOnce;
        private Vector3 position;
        private Quaternion rotation;

        private void Awake()
        {
            position = transform.position;
            rotation = transform.rotation;
        }

        private void Start()
        {
            if (playOnStart)
            {
                //TODO: CALL CutsceneManager to play cutscene data;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!needToPressButton)
            {
                if (onlyOnce)
                {
                    Destroy(this);
                }
            }
            else
            {
                //Interact
            }
        }

        private void OnTriggerExit(Collider other)
        {
            //CutsceneManager.Instance.NotAbleToInteract();
            transform.position = position;
            transform.rotation = rotation;
        }
    }
}