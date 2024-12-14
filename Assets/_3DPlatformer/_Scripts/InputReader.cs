using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


namespace Platformer
{
	[CreateAssetMenu(fileName ="InputReader",menuName ="Platformer/Input/InputReader"),]
	public class InputReader : DescriptionBaseSO, GameInput.IPlayerActions, GameInput.IDialoguesActions, GameInput.IMenusActions
	{
		//TODO: SPECIFIC GAME STATE CAN'T RECEIVED INPUT;
		
		//Game Play
		public event UnityAction JumpEvent = delegate { };
		public event UnityAction JumpCanceledEvent = delegate { };
		public event UnityAction AttackEvent = delegate { };
		public event UnityAction AttackCanceledEvent = delegate { };
		public event UnityAction InteractEvent = delegate { }; // Used to talk, pickup objects, interact with tools like the cooking cauldron
		public event UnityAction InventoryActionButtonEvent = delegate { };
		public event UnityAction SaveActionButtonEvent = delegate { };
		public event UnityAction ResetActionButtonEvent = delegate { };
		
		
		public event UnityAction<Vector2> MoveEvent = delegate{};
		public event UnityAction<Vector2,bool> CameraMoveEvent = delegate { };
		public event UnityAction EnableMouseControlCamera = delegate { };
		public event UnityAction DisableMouseControlCamera = delegate { };
		public event UnityAction StartedRunning = delegate { };
		public event UnityAction StoppedRunning = delegate { };
		// Shared between menus and dialogues
		public event UnityAction MoveSelectionEvent = delegate { };

		// Dialogues
		public event UnityAction AdvanceDialogueEvent = delegate { };

		// Menus
		public event UnityAction MenuMouseMoveEvent = delegate { };
		public event UnityAction MenuClickButtonEvent = delegate { };
		public event UnityAction MenuUnpauseEvent = delegate { };
		public event UnityAction MenuPauseEvent = delegate { };
		public event UnityAction MenuCloseEvent = delegate { };
		public event UnityAction OpenInventoryEvent = delegate { }; // Used to bring up the inventory
		public event UnityAction CloseInventoryEvent = delegate { }; // Used to bring up the inventory
		public event UnityAction<float> TabSwitched = delegate { };

		GameInput input;

		public Vector3 Direction => input.Player.Move.ReadValue<Vector2>();
		public Vector3 LookDirection => input.Player.RotateCamera.ReadValue<Vector2>();
		
		void OnEnable()
		{
			if (input != null) return;
			input = new GameInput();
				
			input.Player.SetCallbacks(this);
			input.Menus.SetCallbacks(this);
			input.Dialogues.SetCallbacks(this);
		}

		private void OnDisable()
		{
			DisableAllInput();
		}
		public void OnAttack(InputAction.CallbackContext context)
		{
			switch (context.phase)
			{
				case InputActionPhase.Started:
					AttackEvent?.Invoke();
					break;
				case InputActionPhase.Canceled:
					AttackCanceledEvent?.Invoke();
					break;
				
			}
		}
		public void EnableGameplayInput()
		{
			input.Player.Enable();
			input.Menus.Disable();
			input.Dialogues.Disable();
		}
		public void EnableMenusInput()
		{
			input.Player.Disable();
			input.Menus.Enable();
			input.Dialogues.Disable();
		}

		public void EnableDialogueInput()
		{
			input.Dialogues.Enable();
			input.Player.Disable();
			input.Menus.Disable();
		}
		private void DisableAllInput()
		{
			input.Player.Disable();
			input.Menus.Disable();
			input.Dialogues.Disable();
		}

		public void EnablePlayerActions() => input.Enable();
		public void DisablePlayerActions() => input.Disable();

		public void OnJump(InputAction.CallbackContext context)
		{
			switch (context.phase)
			{
				case InputActionPhase.Started:
					JumpEvent?.Invoke();
					break;
				case InputActionPhase.Canceled:
					JumpCanceledEvent?.Invoke();
					break;
			}
		}

		

		public void OnInteract(InputAction.CallbackContext context)
		{
			if ((context.phase == InputActionPhase.Performed)) //TODO: STATE IS GAMEPLAY
					InteractEvent.Invoke();
		}

		public void OnPause(InputAction.CallbackContext context)
		{
			if (context.phase == InputActionPhase.Performed)
				MenuPauseEvent.Invoke();
		}

		public void OnOpenInventory(InputAction.CallbackContext context)
		{
			if(context.phase==InputActionPhase.Performed)
				OpenInventoryEvent.Invoke();
		}

		public void OnRotateCamera(InputAction.CallbackContext context)
		{
			CameraMoveEvent?.Invoke(context.ReadValue<Vector2>(), IsDeviceMouse(context));
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
			MoveEvent.Invoke(context.ReadValue<Vector2>());
		}

		public void OnRun(InputAction.CallbackContext context)
		{
		
				switch (context.phase)
				{
					case InputActionPhase.Started:
						StartedRunning.Invoke();
						break;
					case InputActionPhase.Canceled:
						StoppedRunning.Invoke();
						break;
				}
			
		}
		private static bool IsDeviceMouse(InputAction.CallbackContext context) => context.control.device.name == "Mouse";
		public void OnMoveSelection(InputAction.CallbackContext context)
		{
			if (context.phase == InputActionPhase.Performed)
				MoveSelectionEvent.Invoke();
		}

		public void OnNavigate(InputAction.CallbackContext context)
		{
			
		}

		public void OnSubmit(InputAction.CallbackContext context)
		{
			
		}

		public void OnConfirm(InputAction.CallbackContext context)
		{
			if (context.phase == InputActionPhase.Performed)
				MenuClickButtonEvent.Invoke();
		}

		public void OnCancel(InputAction.CallbackContext context)
		{
			if (context.phase == InputActionPhase.Performed)
			{
				MenuCloseEvent.Invoke();
			}
		}

		public void OnMouseMove(InputAction.CallbackContext context)
		{
			if (context.phase == InputActionPhase.Performed)
				MenuMouseMoveEvent.Invoke();
		}

		public void OnUnpause(InputAction.CallbackContext context)
		{
			if (context.phase == InputActionPhase.Performed)
				MenuUnpauseEvent.Invoke();
		}

		public void OnChangeTab(InputAction.CallbackContext context)
		{
			if (context.phase == InputActionPhase.Performed)
				TabSwitched.Invoke(context.ReadValue<float>());
		}

		public void OnInventoryActionButton(InputAction.CallbackContext context)
		{
			if (context.phase == InputActionPhase.Performed)
				InventoryActionButtonEvent.Invoke();
		}

		public void OnSaveActionButton(InputAction.CallbackContext context)
		{
			if (context.phase == InputActionPhase.Performed)
				SaveActionButtonEvent.Invoke();
		}

		public void OnResetActionButton(InputAction.CallbackContext context)
		{
			if (context.phase == InputActionPhase.Performed)
				ResetActionButtonEvent.Invoke();
		}

		public void OnClick(InputAction.CallbackContext context)
		{
			
		}

		public void OnPoint(InputAction.CallbackContext context)
		{
			
		}

		public void OnRightClick(InputAction.CallbackContext context)
		{
			
		}

		public void OnCloseInventory(InputAction.CallbackContext context)
		{
			CloseInventoryEvent.Invoke();
		}

		public void OnAdvanceDialogue(InputAction.CallbackContext context)
		{
			if (context.phase == InputActionPhase.Performed)
			{
				
				AdvanceDialogueEvent.Invoke();
			}
		}

		public void OnTestExit(InputAction.CallbackContext context)
		{
			if (context.phase == InputActionPhase.Performed)
			{
				
				Debug.Log("Exit");
				EnableGameplayInput();
			}
		}
	}
}
