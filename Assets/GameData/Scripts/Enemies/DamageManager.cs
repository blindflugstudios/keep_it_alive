namespace KeepItAlive.Enemies
{
    public class DamageManager
    {
        public float ApplyDamageReturnRemainingHealth(float health)
        {
            health = Hit(health);

            return health;
        }

        public float Hit(float health)
        {
            return health - 5;
        }
    }
}