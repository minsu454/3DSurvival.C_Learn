using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFire : MonoBehaviour
{
    public int damage;
    public float damageRate;

    List<IDamagable> thingList = new List<IDamagable>();

    private Coroutine coDealDamage;

    private IEnumerator CoDealDamage()
    {
        while (true)
        {
            yield return new WaitForSeconds(damageRate);

            for (int i = 0; i < thingList.Count; i++)
            {
                thingList[i].TakePhysicalDamage(damage);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamagable damagalbe))
        {
            thingList.Add(damagalbe);
            coDealDamage = StartCoroutine(CoDealDamage());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IDamagable damagalbe))
        {
            thingList.Remove(damagalbe);
            StopCoroutine(coDealDamage);
        }
    }
}
