using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static PlayerInputActions;


namespace Platformer
{
	[CreateAssetMenu(fileName ="InputReader",menuName ="Platformer/Input/InputReader"),]
	public class InputReader : ScriptableObject, IPlayerActions
	{
		public event UnityAction<Vector2> Move = delegate{};
		public event UnityAction<Vector2,bool> Look = delegate { };
		public event UnityAction EnableMouseControlCamera = delegate { };
		public event UnityAction DisableMouseControlCamera = delegate { };

		PlayerInputActions _inputActions;

		public Vector3 Direction => _inputActions.Player.Move.ReadValue<Vector2>();
		
		void OnEnable()
		{
			if( _inputActions == null)
			{
				_inputActions = new PlayerInputActions();
				_inputActions.Player.SetCallbacks(this);
			}
			_inputActions.Player.Enable();
		}

		public void OnFire(InputAction.CallbackContext context)
		{
			
		}

		public void OnJump(InputAction.CallbackContext context)
		{
			
		}

		public void OnLook(InputAction.CallbackContext context)
		{
			
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
				default:
					break;
			}
		}

		public void OnMove(InputAction.CallbackContext context)
		{
			Move.Invoke(context.ReadValue<Vector2>());
		}

		public void OnRun(InputAction.CallbackContext context)
		{
			Look.Invoke(context.ReadValue<Vector2>(), IsDeviceMouse(context));
		}
		private bool IsDeviceMouse(InputAction.CallbackContext context) => context.control.device.name == "Mouse";
	}
}
