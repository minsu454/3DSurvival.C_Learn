using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    public void TakePhysicalDamage(int damage);
}

public interface IHealalbe
{
    public void TakePhysicalHeal(int damage);
}

public class PlayerCondition : MonoBehaviour, IDamagable, IHealalbe
{
    public UICondition uiCondition;

    Condition health { get { return uiCondition.health; } }
    Condition hunger { get { return uiCondition.hunger; } }
    Condition stamina { get { return uiCondition.stamina; } }
    Condition mana { get { return uiCondition.mana; } }

    public float noHungerHealthDecay;

    public event Action onTakeDamage;
    public event Action onTakeHeal;

    // Update is called once per frame
    void Update()
    {
        hunger.Subtract(hunger.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);
        mana.Add(mana.passiveValue * Time.deltaTime);
        
        if (hunger.curValue == 0f)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
        }

        if (health.curValue == 0f)
        {
            Die();
        }
    }

    public void Heal(float amout)
    {
        health.Add(amout);
    }

    public void Eat(float amout)
    {
        hunger.Add(amout);
    }

    private void Die()
    {
        Debug.Log("죽었다..");
    }

    public void TakePhysicalDamage(int damage)
    {
        health.Subtract(damage);
        onTakeDamage?.Invoke();
    }

    public bool UseStamina(float amount)
    {
        if (stamina.curValue - amount < 0f)
            return false;

        stamina.Subtract(amount);
        return true;
    }

    public bool UseSkill(float manaAmount, float healAmount)
    {
        if (stamina.curValue - manaAmount < 0f)
            return false;

        mana.Subtract(manaAmount);
        Heal(healAmount);
        return true;
    }

    public void TakePhysicalHeal(int heal)
    {
        health.Add(heal);
        onTakeHeal?.Invoke();
    }
}
