using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackController : MonoBehaviour
{
    [SerializeField] Weapon currentWeapon;

    private Transform mainCamera;

    private Animator anim;

    private bool isAttacking = false;
    private void Awake()
    {
        mainCamera = GameObject.FindGameObjectWithTag("CameraPoint").transform;
        anim = mainCamera.transform.GetChild(0).GetComponent<Animator>();
        if(currentWeapon != null)
            SpawnWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
    }

    private void Attack()
    {
        if (Mouse.current.leftButton.isPressed && !isAttacking)
        {
            StartCoroutine(AttackTrigger());
        }
    }

    private IEnumerator AttackTrigger()
    {
        isAttacking = true;
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(currentWeapon.GetAttackRate);
        isAttacking = false;
    }

    private void SpawnWeapon()
    {
        if (currentWeapon == null)
            return;
        currentWeapon.SpawnNewWeapon(mainCamera.transform.GetChild(0).GetChild(0), anim);
    }

    public void EquipWeapon(Weapon weaponType)
    {
        if(currentWeapon != null)
        {
            currentWeapon.Drop();
        }
        currentWeapon = weaponType;
        SpawnWeapon();
    }

    public int GetDamage()
    {
        if(currentWeapon != null)
        {
            return currentWeapon.GetDamage;
        }
        return 0;
    }
}
