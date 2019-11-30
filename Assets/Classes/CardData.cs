using System.Collections.Generic;

public class CardData
{
    public string name;
    public int cost;
    public int damageTarget;
    public int damageAll;
    public int damageSelf;
    public int armorSelf;
    public int cardDraw;

    public string GetCardText()
    {
        string txt = "";
        if (damageSelf > 0) txt += $"Take {damageSelf} damage.";
        if (damageTarget > 0) txt += $"Deal {damageTarget} damage.";
        if (damageAll > 0) txt += $"Deal {damageAll} damage to ALL enemies.";
        if (armorSelf > 0) txt += $"Gain {armorSelf} armor.";
        if (cardDraw > 0) txt += $"Draw {cardDraw} cards.";
        return txt;
    }

    public void UseCard(Entity player, Character target, List<Entity> allEnemies)
    {
        player.TakeDamage(damageSelf);
        target.e.TakeDamage(damageTarget);
        foreach (Entity e in allEnemies) e.TakeDamage(damageAll);
        player.armor += armorSelf;
    }

    public bool IsSelfCard()
    {
        return damageTarget == 0 && damageAll == 0;
    }
}