using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

namespace Platformer
{
    public class PlatformMover : MonoBehaviour
    {
        [SerializeField] Vector3 moveTo = Vector3.zero;
        [SerializeField] private Ease ease = Ease.Linear;
        [SerializeField] private float _durationTime;
        
        private void Start()
        {
            var startPosition = transform.position;
            transform.DOMove(startPosition+moveTo, _durationTime)
                .SetEase(ease)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
}
