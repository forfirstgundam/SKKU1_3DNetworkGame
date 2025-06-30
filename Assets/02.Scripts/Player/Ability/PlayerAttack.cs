using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : PlayerAbility
{    
    private Animator _animator;

    private float _attackTimer = 0f;

    private bool _isAttacking = false;
    public bool IsAttacking => _isAttacking;

    public Collider WeaponCollider;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        DeactivateCollider();
    }

    // - 위치 / 회전처럼 상시로 확인이 필요한 데이터 동기화 : IPunObservable(OnPhotonSerializeView)
    // - 트리거/공격/피격처럼 간헐적으로 특정한 이벤트가 발생했을 때의 변화된 데이터 동기화 : RPC
    //  RPC : Remoste Procedure Call
    //      - 물리적으로 떨어져 있는 다른 디바이스의 함수를 호출하는 기능
    //      - RPC 함수를 호출하면 네트워크를 통해 다른 사용자의 스크립트에서 해당 함수가 호출됨
    
    private void Update()
    {
        if (_photonView.IsMine == false)
        {
            return;
        }

        _attackTimer += Time.deltaTime;

        
        if (Input.GetMouseButton(0) && _owner.Stat.Stamina >= _owner.Stat.StaminaAttackCost && _attackTimer >= (1f / _owner.Stat.AttackSpeed))
        {
            _owner.Stat.Stamina -= _owner.Stat.StaminaAttackCost;
            
            _attackTimer = 0f;

            // 1. 일반 메서드 호출 방식
            //PlayAttackAnimation(Random.Range(1, 4));

            // 2. RPC 메서드 호출 방식
            _photonView.RPC(nameof(PlayAttackAnimation), RpcTarget.All, Random.Range(1, 4));
        }
    }

    public void ActivateCollider()
    {
        WeaponCollider.enabled = true;
        Debug.Log("collider on");
    }

    public void DeactivateCollider()
    {
        WeaponCollider.enabled = false;
        Debug.Log("collider off");
    }


    [PunRPC]
    private void PlayAttackAnimation(int randomNumber)
    {
        _animator.SetTrigger($"Attack{randomNumber}");
    }

    public void Hit(IDamageable damagedObject)
    {
        DeactivateCollider();

        damagedObject.Damaged(_owner.Stat.Damage);
    }
}