using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
	public float mouseSensitivityX = 1.0f;
	public float mouseSensitivityY = 1.0f;

	public float walkSpeed = 10.0f;
	Vector3 moveAmount;
	Vector3 smoothMoveVelocity;

	Transform cameraT;
	float verticalLookRotation;

	Rigidbody rigidbodyR;

	public float jumpForce = 250.0f;
	public bool grounded;
	public LayerMask groundedMask;
	public LayerMask resetMask;

	bool cursorVisible;

	// Use this for initialization
	void Start()
	{
		cameraT = Camera.main.transform;
		rigidbodyR = GetComponent<Rigidbody>();
		LockMouse();
	}

	// Update is called once per frame
	void Update()
	{
		// rotation
		transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivityX);
		verticalLookRotation += Input.GetAxis("Mouse Y") * mouseSensitivityY;
		verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90, 90);
		cameraT.localEulerAngles = Vector3.left * verticalLookRotation;

		// movement
		Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
		Vector3 targetMoveAmount = moveDir * walkSpeed;
		moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);

		// jump
		if (Input.GetButtonDown("Jump"))
		{
			if (grounded)
			{
				rigidbodyR.AddForce(transform.up * jumpForce);
				
			}
			
		}

		Ray ray = new Ray(transform.position, -transform.up);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, 2 + .2f, groundedMask))
		{
			grounded = true;
		}
		else
		{
			grounded = false;
		}

		

		/* Lock/unlock mouse on click */
		if (Input.GetKeyDown("`"))
		{
			if (!cursorVisible)
			{
				UnlockMouse();
			}
			else
			{
				LockMouse();
			}
		}
	}

	void FixedUpdate()
	{
		rigidbodyR.MovePosition(rigidbodyR.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
	}

	void UnlockMouse()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		cursorVisible = true;
	}

	void LockMouse()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		cursorVisible = false;
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Level1End"))
        {
			
			SceneManager.LoadScene("Level2");
			UnlockMouse();
        }
		else if (other.CompareTag("Level2End"))
		{

			SceneManager.LoadScene("Finish");
			UnlockMouse();
		}
	}

    private void OnCollisionEnter(Collision collision)
    {
		
		if (collision.gameObject.CompareTag("Reset"))
        {
			SceneManager.LoadScene("Level2");
			
        }
    }


}
