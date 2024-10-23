using System;
using Cinemachine;
using KBCore.Refs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Platformer
{
    public class PlayerController : ValidatedMonoBehaviour
    {
        [Header(" Elements ")] 
        [SerializeField,Self] CharacterController _characterController;

        [SerializeField, Child] Animator _animator;
        [SerializeField, Anywhere] CinemachineFreeLook _freeLookCamera;
        [SerializeField, Anywhere] InputReader _input;


        [Header(" Settings ")]
        [SerializeField] float _moveSpeed = 6f;

        [SerializeField] float _rotationSpeed = 15f;
        [SerializeField] float _smoothTime = 0.2f;

        private Transform _mainCam;
        private void Awake()
        {
            _mainCam = Camera.main?.transform;
            _freeLookCamera.LookAt = transform;
            _freeLookCamera.Follow = transform;
            _freeLookCamera.OnTargetObjectWarped(transform,transform.position-_freeLookCamera.transform.position-Vector3.one);
        }

        private void Update()
        {
            HandleMovementInput();
           // UpdateAnimator();
        }

        private void HandleMovementInput()
        {
            var movemementDirection = new Vector3(_input.Direction.x,0,_input.Direction.y);
            
        }

        void test(int a, int b)
        {
            
        }
        protected override void OnValidate()
        {
            base.OnValidate();
        }
        
    }
}