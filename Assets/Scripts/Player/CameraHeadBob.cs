using UnityEngine;


public class CameraHeadBob : MonoBehaviour
{
    [SerializeField] private float walkBobSpeed = 14f;       
    [SerializeField] private float walkBobAmount = 0.05f;    

    [SerializeField] private float runBobSpeed = 20f;        
    [SerializeField] private float runBobAmount = 0.10f;     

    [SerializeField] private float crouchBobSpeed = 8f;     
    [SerializeField] private float crouchBobAmount = 0.025f; 

    [Header("Referanslar")]
    [SerializeField] private CharacterController characterController; 

    private float defaultYPos;
    private float timer;

    private void Start()
    {
        defaultYPos = transform.localPosition.y;
    }

    private void Update()
    {
        if (characterController == null || !characterController.isGrounded)
        {
            ResetCameraPosition();
            return;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical   = Input.GetAxis("Vertical");
        bool isMoving    = Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f;

        if (isMoving)
        {
            bool isCrouching = Input.GetKey(KeyCode.R);
            bool isRunning   = Input.GetKey(KeyCode.LeftShift) && !isCrouching;

            float speed  = isCrouching ? crouchBobSpeed  : isRunning ? runBobSpeed  : walkBobSpeed;
            float amount = isCrouching ? crouchBobAmount : isRunning ? runBobAmount : walkBobAmount;

            timer += Time.deltaTime * speed;

            float newY = defaultYPos + Mathf.Sin(timer) * amount;
            transform.localPosition = new Vector3(
                transform.localPosition.x,
                newY,
                transform.localPosition.z
            );
        }
        else
        {
            ResetCameraPosition();
        }
    }

    private void ResetCameraPosition()
    {
        timer = 0f;
        transform.localPosition = new Vector3(
            transform.localPosition.x,
            Mathf.Lerp(transform.localPosition.y, defaultYPos, Time.deltaTime * 10f),
            transform.localPosition.z
        );
    }
}