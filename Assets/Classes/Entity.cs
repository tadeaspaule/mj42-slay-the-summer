

public class Entity
{
    public bool friendly;
    public int health;
    public int maxHealth;
    public string name;
    public int armor;

    public void DealDamage(int amount)
    {
        if (armor > 0) {
            armor -= amount;
            if (armor < 0) {
                amount = -armor;
                armor = 0;
            }
            else amount = 0;
        }
        health -= amount;
        UpdateHealth();
    }

    public void Heal(int amount)
    {
        health += amount;
        UpdateHealth();
    }

    void UpdateHealth()
    {
        if (health < 0) health = 0;
        else if (health > maxHealth) health = maxHealth;
    }
}