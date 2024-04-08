using System.Collections;
using System.ComponentModel;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Variables

    public float speed;
    public float jumpForce;
    public Rigidbody2D rb2d;
    public bool isGrounded;
    private bool hasJumped;

    // Acceleration and deceleration variables
    [SerializeField] float acceleration = 5f;
    [SerializeField] float deceleration = 5f;

    // Camera follow variables
    public Transform cameraFollowTarget;
    public TrailRenderer trailRenderer;

    // Respawn system variables
    private Vector3 respawnPosition;
    public float cast;
    public LayerMask groundLayer;
    #endregion 

    void Start()
    {
        respawnPosition = transform.position;
    }

    #region Update Methods
    void Update()
    {
        if (isGrounded && !hasJumped && Input.GetKeyDown(KeyCode.Space) && Input.GetKey(KeyCode.Space))
        {
            PlayerJump();
            hasJumped = true;
        }

        // Color swapping logic
        HandleColorSwap();

        // Camera follow
        HandleCameraFollow();

        // Player respawn
        HandleRespawnInput();
    }
    #endregion

    #region Fixed Update
    void FixedUpdate()
    {
        isGrounded = CheckGround();
        HandleMovementInput();
    }
    #endregion

    #region Input Handling Methods

    void HandleMovementInput()
    {
        float horizontalInput = SimpleInput.GetAxis("Horizontal");

        // Apply acceleration and deceleration
        float targetVelocity = horizontalInput * speed;
        float currentVelocity = rb2d.velocity.x;
        float newVelocity = Mathf.MoveTowards(currentVelocity, targetVelocity, acceleration * Time.deltaTime);

        rb2d.velocity = new Vector2(newVelocity, rb2d.velocity.y);

        // Apply deceleration when not moving
        if (Mathf.Approximately(horizontalInput, 0f))
        {
            rb2d.velocity = new Vector2(Mathf.MoveTowards(rb2d.velocity.x, 0f, deceleration * Time.deltaTime), rb2d.velocity.y);
        }
    }

    void HandleColorSwap()
    {
        if (Input.GetKeyDown(GameManager.Instance.playerController.playerColorSwap.swapColorKey))
        {
            SwapColors();
        }
    }

    void HandleCameraFollow()
    {
        if (cameraFollowTarget != null)
        {
            Vector3 targetPosition = new Vector3(cameraFollowTarget.position.x, cameraFollowTarget.position.y, Camera.main.transform.position.z);
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, targetPosition, Time.deltaTime * 5f);
        }
    }

    void HandleRespawnInput()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RespawnPlayer();
            Debug.Log("Player Respawned!");
        }
    }
    #endregion

    #region Player Actions
    private bool CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, cast, groundLayer);
        return hit.collider != null;
    }

    public void PlayerJump()
    {
        if (!isGrounded) return;

        rb2d.AddForce(jumpForce * Vector2.up, ForceMode2D.Force);
        //rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce, ForceMode2D.Force);
    }

    public void SwapColors()
    {
        GameManager.Instance.playerController.playerColorSwap.isWhite = !GameManager.Instance.playerController.playerColorSwap.isWhite;
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = GameManager.Instance.playerController.playerColorSwap.isWhite ? GameManager.Instance.playerController.playerColorSwap.whiteColor : GameManager.Instance.playerController.playerColorSwap.blackColor;
        trailRenderer.colorGradient = GameManager.Instance.playerController.playerColorSwap.isWhite ? GameManager.Instance.playerController.playerColorSwap.trailColorBlack : GameManager.Instance.playerController.playerColorSwap.trailColorWhite;
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position - new Vector3(0, cast, 0));
    }

    public void RespawnPlayer()
    {
        /*Debug.LogWarning("Ary Bhadve thik se implement kar, SceneManager ka use karke");*/
        transform.position = respawnPosition;
        rb2d.velocity = Vector2.zero;
    }
}
