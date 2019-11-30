

public class Entity
{
    public bool friendly;
    public int health = 10;
    public int maxHealth = 10;
    public string name = "enemy";
    public int armor = 0;

    public void TakeDamage(int amount)
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