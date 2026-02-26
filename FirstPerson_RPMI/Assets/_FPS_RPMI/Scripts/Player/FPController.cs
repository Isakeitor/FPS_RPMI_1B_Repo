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

    [Header("Player State Bools")]
    [SerializeField] bool isSprinting;
    [SerializeField] bool isCrouching;
    #endregion

    //Variables de autoreferencia
    Rigidbody rb;

    //Variables de input
    Vector2 moveInput;
    Vector2 lookInput;
    float lookRotation;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
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

    }
    public void OnCrouch(InputAction.CallbackContext context)
    {

    }
    public void OnSprint(InputAction.CallbackContext context)
    {

    }
    #endregion


}
