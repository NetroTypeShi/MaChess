using UnityEngine;

public class Piece
{
    private float health;
    private float maxHealth;

    public Piece(float initialHealth)
    {
        health = initialHealth;
        maxHealth = initialHealth;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        health = Mathf.Max(0, health); // No permitir valores negativos
    }

    public void Heal(float amount)
    {
        health += amount;
        health = Mathf.Min(maxHealth, health); // No permitir superar la vida máxima
    }

    public float GetHealthFactor()
    {
        return health / maxHealth; // Proporción de vida restante
    }

    public bool IsDestroyed()
    {
        return health <= 0;
    }
}

