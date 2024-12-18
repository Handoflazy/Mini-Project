using System.Collections.Generic;
using Platformer._3DPlatformer._Scripts.Utilities.Utilities;
using UnityEngine.EventSystems;
using Utilities.ImprovedTimers;

namespace Platformer.AbilitySystem
{
    public class AbilityController
    {
        private readonly AbilityModel model;
        private readonly AbilityView view;
        private readonly Queue<AbilityCommand> abilityQueue = new();
        private readonly CountdownTimer timer = new CountdownTimer(0);

        AbilityController(AbilityView view, AbilityModel model)
        {
            this.view = view;
            this.model = model;
            ConnectModel();
            ConnectView();
        }

        public void Update(float deltaTime)
        {
            view.UpdateRadical(timer.Progress);

            if (!timer.IsRunning && abilityQueue.TryDequeue(out AbilityCommand cmd))
            {
                cmd.Execute();
                timer.Reset(cmd.duration);
                timer.Start();
            }
        }

        private void ConnectView()
        {
            view.UpdateButtonSprite(model.abilities);
        }

        private void ConnectModel()
        {
            model.abilities.AnyValueChanged += UpdateButtons;
        }

        public void OnAbilityButtonPressed(int index)
        {
            if (timer.Progress < .25f || !timer.IsRunning)
            {
                if (model.abilities[index] != null)
                {
                    abilityQueue.Enqueue(model.abilities[index].CreateCommand());
                }
            }
            EventSystem.current?.SetSelectedGameObject(null);
        }

        private void UpdateButtons(IList<Ability> list) => view.UpdateButtonSprite(list);

        public class Builder
        {
            private readonly AbilityModel model = new AbilityModel();

            public Builder WithAbility(AbilityData[] datas)
            {
                foreach (var data in datas)
                {
                    model.Add(new Ability(data));
                }

                return this;
            }

            public AbilityController Build(AbilityView view)
            {
                Preconditions.CheckNotNull(view);
                return new AbilityController(view, model);
            }
        }
    }
}