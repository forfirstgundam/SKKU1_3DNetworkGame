using UnityEngine;

public class PlayerMoveAbility : MonoBehaviour
{
    public float MoveSpeed = 7f;
    public float JumpPower = 2.5f;
    
    private CharacterController _characterController;

    float _gravity = -9f;
    float _yVelocity = 0f;

    
    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }
    
    private void Update()
    {
        // 목표: 키보드 [W], [A], [S], [D] 키를 누르면 캐릭터를 그 방향으로 이동시키고 싶다.
        // 순서:
        // 1. 사용자의 키보드 입력 받기
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // 2. '캐릭터가 바라보는 방향'을 기준으로 방향 설정하기
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized; // = dir.Normalize();
        
        // 2-1. 수직 속도에 중력 값을 적용한다.
        _yVelocity += _gravity * Time.deltaTime;
        dir.y = _yVelocity;
        
        // 2-2. 점프 적용
        Debug.Log(_characterController.isGrounded);
        if (Input.GetKeyDown(KeyCode.Space) && _characterController.isGrounded)
        {
            _yVelocity = JumpPower;
        }
        
        // 3. 이동 속도에 따라 그 방향으로 이동하기
        // 캐릭터의 위치 = 현재 위치 + 속도  * 시간
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);
    }
}