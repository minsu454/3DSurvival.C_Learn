using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipTool : Equip
{
    public float attackRate;
    private bool attacking;
    public float attackDistance;
    public float useStamina;

    [Header("Resource Gathering")]
    public bool doesGatherResources;

    [Header("Combat")]
    public bool doesDealDamage;
    public int damage;

    private Animator animator;
    private Camera camera;

    private void Start()
    {
        animator = GetComponent<Animator>();
        camera = Camera.main;
    }

    public override void OnAttackInput()
    {
        base.OnAttackInput();

        if (attacking)
        {
            return;
        }

        if (CharacterManager.Instance.Player.condition.UseStamina(useStamina))
        {
            attacking = true;
            animator.SetTrigger("Attack");
            Invoke(nameof(OnCanAttack), attackRate);
        }
    }

    private void OnCanAttack()
    {
        attacking = false;
    }

    public void OnHit()
    {
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, attackDistance))
        {
            if (doesGatherResources && hit.collider.TryGetComponent(out Resource resource))
            {
                resource.Gether(hit.point, hit.normal);
            }
            else if (doesDealDamage && hit.collider.TryGetComponent(out IDamagalbe entity))
            {
                entity.TakePhysicalDamage(damage);
            }
        }
    }
}
