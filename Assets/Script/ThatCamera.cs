using UnityEngine;

public class ThatCamera : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 10f;       // 이동 속도

    [Header("Mouse Look Settings")]
    [SerializeField] private float mouseSensitivity = 2f; // 마우스 감도

    private float rotationX = 0f;
    private float rotationY = 0f;

    void Start()
    {
        // 마우스 커서를 화면 중앙에 고정하고 숨깁니다.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 시작할 때의 카메라 회전 값을 초기화합니다.
        Vector3 currentRotation = transform.localRotation.eulerAngles;
        rotationX = currentRotation.y;
        rotationY = currentRotation.x;
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
    }

    // 마우스 움직임에 따른 카메라 회전 처리
    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        rotationX += mouseX;
        rotationY -= mouseY; // 마우스 위로 올리면 카메라가 위를 보도록 (-) 처리

        // 위아래 회전 각도를 -90도에서 90도 사이로 제한 (화면이 뒤집히는 것 방지)
        rotationY = Mathf.Clamp(rotationY, -90f, 90f);

        transform.localRotation = Quaternion.Euler(rotationY, rotationX, 0f);
    }

    // 키보드 입력에 따른 카메라 이동 처리
    private void HandleMovement()
    {
        Vector3 moveDirection = Vector3.zero;

        // 1. WASD 입력 (바라보는 방향 기준 이동)
        float horizontal = Input.GetAxisRaw("Horizontal"); // A, D (-1, 1)
        float vertical = Input.GetAxisRaw("Vertical");     // W, S (-1, 1)

        moveDirection += transform.forward * vertical;
        moveDirection += transform.right * horizontal;

        // 2. Space / Left Ctrl 입력 (글로벌 Y축 기준 상승/하강)
        if (Input.GetKey(KeyCode.Space))
        {
            moveDirection += Vector3.up;
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            moveDirection += Vector3.down;
        }

        // 3. 이동 계산 및 반영 (프레임 독립적 적용)
        transform.position += moveDirection.normalized * moveSpeed * Time.deltaTime;
    }
}