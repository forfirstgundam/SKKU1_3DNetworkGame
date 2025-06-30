using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private PlayerAttack _attack;

    private void Start()
    {
        _attack = GetComponentInParent<PlayerAttack>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // 자기 자신과 부딛혔다면 아무고또 안한다.
        if (other.transform == _attack.transform)
        {
            return;
        }

        // IDamagable 인터페이스를 구현하고 있는지 확인
        IDamageable damagedObject = other.GetComponent<IDamageable>();
        if (damagedObject != null)
        {
            _attack.Hit(damagedObject);
        }
    }
}