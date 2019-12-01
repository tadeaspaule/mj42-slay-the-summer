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

    Enemy GetClone()
    {
        Enemy e = new Enemy();
        e.name = name;
        e.health = health;
        e.attacking = attacking;
        e.shieldChance = shieldChance;
        e.shielding = shielding;
        return e;
    }
}