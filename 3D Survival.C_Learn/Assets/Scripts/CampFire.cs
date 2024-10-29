using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFire : MonoBehaviour
{
    public int damage;
    public float damageRate;

    List<IDamagalbe> thingList = new List<IDamagalbe>();

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(DealDamage), 0, damageRate);
    }

    private void DealDamage()
    {
        for (int i = 0; i < thingList.Count; i++)
        {
            thingList[i].TakePhysicalDamage(damage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamagalbe damagalbe))
        {
            thingList.Add(damagalbe);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IDamagalbe damagalbe))
        {
            thingList.Remove(damagalbe);
        }
    }
}
