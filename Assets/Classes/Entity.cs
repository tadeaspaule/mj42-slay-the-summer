

public class Entity
{
    public bool friendly;
    public int health = 20;
    public int maxHealth = 20;
    public string name;
    public int armor = 0;

    public int TakeDamage(int amount)
    {
        if (armor > 0) {
            armor -= amount;
            if (armor < 0) {
                amount = -armor;
                armor = 0;
            }
            else amount = 0;
        }
        int dmg = health > amount ? amount : health;
        health -= amount;
        UpdateHealth();
        return dmg;
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