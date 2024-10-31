using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static PlayerInputActions;


namespace Platformer
{
	[CreateAssetMenu(fileName ="InputReader",menuName ="Platformer/Input/InputReader"),]
	public class InputReader : ScriptableObject, IPlayerActions
	{
		[SerializeField, Range(1, 20f)] private float MouseSensitivity = 1f;
		public event UnityAction<Vector2> Move = delegate{};
		public event UnityAction<Vector2,bool> Look = delegate { };
		public event UnityAction EnableMouseControlCamera = delegate { };
		public event UnityAction DisableMouseControlCamera = delegate { };
		public event UnityAction<bool> Jump = delegate { };
		public event UnityAction<bool> Dash = delegate { };

		PlayerInputActions _inputActions;

		public Vector3 Direction => _inputActions.Player.Move.ReadValue<Vector2>();
		public Vector3 LookDirection => _inputActions.Player.Look.ReadValue<Vector2>();
		
		void OnEnable()
		{
			if( _inputActions == null)
			{
				_inputActions = new PlayerInputActions();
				_inputActions.Player.SetCallbacks(this);
			}
		}

		public void EnablePlayerActions() => _inputActions.Enable();

		public void OnFire(InputAction.CallbackContext context)
		{
			
		}

		public void OnJump(InputAction.CallbackContext context)
		{
			switch (context.phase)
			{
				case InputActionPhase.Started:
					Jump?.Invoke(true);
					break;
				case InputActionPhase.Canceled:
					Jump?.Invoke(false);
					break;
			}
		}

		public void OnLook(InputAction.CallbackContext context)
		{
			Look?.Invoke(context.ReadValue<Vector2>()*MouseSensitivity, IsDeviceMouse(context));
		}

		public void OnMouseControlCamera(InputAction.CallbackContext context)
		{
			switch (context.phase)
			{
				case InputActionPhase.Started:
					EnableMouseControlCamera.Invoke();
					break;
				case InputActionPhase.Canceled:
					DisableMouseControlCamera.Invoke();
					break;
			}
		}

		public void OnMove(InputAction.CallbackContext context)
		{
			Move.Invoke(context.ReadValue<Vector2>());
		}

		public void OnRun(InputAction.CallbackContext context)
		{
		
				switch (context.phase)
				{
					case InputActionPhase.Started:
						Dash?.Invoke(true);
						break;
					case InputActionPhase.Canceled:
						Dash?.Invoke(false);
						break;
				}
			
		}
		private bool IsDeviceMouse(InputAction.CallbackContext context) => context.control.device.name == "Mouse";
	}
}
