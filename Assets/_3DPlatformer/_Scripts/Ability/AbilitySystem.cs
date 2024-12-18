using System;
using UnityEngine;

namespace Platformer.AbilitySystem
{
    public class AbilitySystem : MonoBehaviour
    {
        [SerializeField] private AbilityView abilityView;
        [SerializeField] private AbilityData[] startinAbilities;
        [SerializeField] private InputReader inputReader;
        private AbilityController controller;
        private void Awake()
        {
            controller = new AbilityController.Builder().WithAbility(startinAbilities).Build(abilityView);
            inputReader.AbilityPressedEvent += OnAbilityPressed;
        }

        private void OnDisable()
        {
            inputReader.AbilityPressedEvent -= OnAbilityPressed;
        }

        public void OnAbilityPressed(int index)
        {
            controller.OnAbilityButtonPressed(index);
        }

        private void Update()
        {
            controller.Update(Time.deltaTime);
        }
    }
}