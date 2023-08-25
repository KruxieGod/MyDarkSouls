interface IHealthBar
{
    internal void SetMaxHealth(int maxHealth);
    internal void SetCurrentHealth(int currentHealth);
    internal void SetName(string name);
    internal void Initialize();
    internal void SetOff();
}