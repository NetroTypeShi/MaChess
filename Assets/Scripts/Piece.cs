using Unity.VisualScripting;
using UnityEngine;

public class Piece
{
    private float maxHealth = 10f;
    private float health;
    private float baseDamage = 2f;

    public Piece(float initialHealth)
    {
        // si la vida inicial es menor que 0, se pone a 0,
        // si es más que el máximo, se pone al máximo
        // si está entre 0 y el máximo, se deja tal cual
        health = Mathf.Clamp(initialHealth, 0, maxHealth);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }

    public void Heal(float amount)
    {
        health += amount;
    }

    public float GetHealthFactor()
    {
        // Si el valor de salud es menor que 0, se va a 0,
        // si el valor es más que 1, se va a 1
        // si está entre 0 y uno lo devuleve tal cual
        return Mathf.Clamp01(health / maxHealth);
    }

    public bool IsDestroyed()
    {
        return health <= 0;
    }

    public float GetDamage()
    {
        return baseDamage;
    }

    public void IncreaseDamage(float amount)
    {
        baseDamage += amount;
    }
}

