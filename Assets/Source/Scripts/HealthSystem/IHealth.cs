namespace Source.Scripts.HealthSystem
{
    public interface IHealth
    {
        public float CurrentValue { get; }
        public float DefaultValue { get; }
    }
}