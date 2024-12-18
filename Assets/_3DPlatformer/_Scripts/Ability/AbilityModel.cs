namespace Platformer.AbilitySystem
{
    public class AbilityModel
    {
        public readonly ObservableList<Ability> abilities = new();

        public void Add(Ability a)
        {
            abilities.Add(a);
        }
    }

    public class Ability
    {
        public readonly AbilityData data;

        public Ability(AbilityData data)
        {
            this.data = data;
        }

        public AbilityCommand CreateCommand()
        {
            return new AbilityCommand(data);
        }
    }
}