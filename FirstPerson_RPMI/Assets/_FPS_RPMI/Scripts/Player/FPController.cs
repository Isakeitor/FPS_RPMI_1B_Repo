using UnityEngine;
using UnityEngine.InputSystem;

public class FPController : MonoBehaviour
{
    #region General Vaiables
    [Header("Movement & Look")]
    [SerializeField] GameObject camHolder; //Ref en el inspector del objeto a rotar
    [SerializeField] float speed = 5f;
    [SerializeField] float crouchSpeed = 3f;
    [SerializeField] float sprintSpeed = 8f;
    [SerializeField] float maxForce = 1f; //Fuerza maxima de aceleracion
    [SerializeField] float sensitivity = 0.1f;

    [Header("Jump & Groundcheck")]
    [SerializeField] float jumpForce = 5f;
    [SerializeField] bool isGrounded;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.3f;
    [SerializeField] LayerMask groundLayer;

    [Header("Player State Bools")]
    [SerializeField] bool isSprinting;
    [SerializeField] bool isCrouching;
    #endregion

    //Variables de autoreferencia
    Rigidbody rb;
    Animator anim;

    //Variables de input
    Vector2 moveInput;
    Vector2 lookInput;
    float lookRotation;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Lock del cursor del raton
        Cursor.lockState = CursorLockMode.Locked; //Lockea el cursor al centro de la pantalla
        Cursor.visible = false; //Apaga la visibilidad del cursor
    }

    // Update is called once per frame
    void Update()
    {
        //GroundCheck
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void LateUpdate()
    {
        CameraLook();
    }

    void CameraLook()
    {
        //Rotacion del personaje (horizontal)
        transform.Rotate(Vector3.up * lookInput.x * sensitivity);
        //Rotacion de camara vertical
        lookRotation += (-lookInput.y * sensitivity); 
        lookRotation = Mathf.Clamp(lookRotation, -90f, 90f); //Limita la rotacion de la camara para evitar que gire completamente
        camHolder.transform.localEulerAngles = new Vector3(lookRotation, 0f, 0f); //Aplica la rotacion a la camara
    }

    void Movement()
    {
        //Definit los dos vectores que permiten aceleracion
        Vector3 currenntVelocoty = rb.linearVelocity; 
        Vector3 targetVelocity = new Vector3(moveInput.x, 0f, moveInput.y) * speed;
        //A la direccion a alcanzar, le multiplica la velocidad
        targetVelocity *= isCrouching ?crouchSpeed : isSprinting ? sprintSpeed : speed; 
        targetVelocity = transform.TransformDirection(targetVelocity); //Convierte la direccion local a global

        //Calcular el cambio de velocidad (aceleracion)
        Vector3 velocityChange = (targetVelocity - currenntVelocoty);
        velocityChange = new Vector3(velocityChange.x, 0, velocityChange.z);
        velocityChange = Vector3.ClampMagnitude(velocityChange, maxForce);

        //Aplicación del movimiento (DIRECCION + ACELERACIÓN)
        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    void Jump()
    {
        if (isGrounded) rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    #region Input Methods
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed) Jump();
    }
    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isCrouching = !isCrouching;
            anim.SetBool("isCrouching", isCrouching);
        }
    }
    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed && !isCrouching) isSprinting = true;
        if (context.canceled) isSprinting = false;
    }
    #endregion
}