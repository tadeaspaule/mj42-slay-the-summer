public class Enemy
{
    public string name;
    public int health;
    public int shielding;
    public int attacking;
    public float shieldChance;

    public void SetupCharacter(Character character)
    {
        Entity e = new Entity();
        e.name = name;
        e.health = health;
        e.maxHealth = health;
        character.e = e;
        character.nextAttack = attacking;
        character.nextArmor = shielding;
    }

    public Enemy GetClone()
    {
        Enemy e = new Enemy();
        e.name = name;
        e.health = health;
        e.attacking = attacking;
        e.shielding = shielding;
        e.shieldChance = shieldChance;
        return e;
    }

    public static Enemy GetBoss()
    {
        Enemy e = new Enemy();
        e.name = "boss";
        e.health = 100;
        e.attacking = 6;
        e.shielding = 6;
        e.shieldChance = 0.5f;
        return e;
    }
}