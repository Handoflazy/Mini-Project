using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Match3
{
	public class InputReader : MonoBehaviour
	{
		private PlayerInput _playerInput;
		private InputAction _selectAction;
		private InputAction _fireAction;

		public event Action Fire;
		public event Action Release;

		public Vector2 Selected => _selectAction.ReadValue<Vector2>();

		private Vector2 clickPosition;
		// Start is called before the first frame update
		void Start()
		{
			_playerInput = GetComponent<PlayerInput>();
			//_playerInput.SwitchCurrentActionMap("UI");
			_selectAction = _playerInput.actions["Select"];
			_fireAction = _playerInput.actions["Fire"];
			_fireAction.started += OnClickDown;
			_fireAction.canceled += OnRelease;
	  
		}
		public Vector2Int GetFireDirection()
		{
			Vector2 direction = (Selected - clickPosition).normalized;
			float angle = Vector2.SignedAngle(Vector2.right, direction);

			if (angle > -45 && angle <= 45) return Vector2Int.right;
			if (angle > 45 && angle <= 135) return Vector2Int.up;
			if (angle > 135 || angle <= -135) return Vector2Int.left;
			return Vector2Int.down;

		}
		private void OnDisable()
		{
			_fireAction.started -= OnClickDown;
			_fireAction.canceled -= OnRelease;
		}
		private void OnClickDown(InputAction.CallbackContext context)
		{
			Fire?.Invoke();
			clickPosition = Selected;
		}

		private void OnRelease(InputAction.CallbackContext context)
		{
			Release?.Invoke();
	      
		}
	}
	
}
