using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Normal Movement Variables")]
    public float jumpForce = 250.0f;
    public float walkSpeed = 10.0f;
    Vector3 moveAmount;
    Vector3 smoothMoveVelocity;
    Vector3 moveDir;
    float verticalLookRotation;

    [Header("Repulser Variables")]
    public float launchPower;
    public float explosionR;
    public ParticleSystem ps;
    RaycastHit hitLaunch;
    private bool canLaunch;

    [Header("General Use Variables")]
    public bool grounded;
    public float mouseSensitivityX { get; set; }
    public float mouseSensitivityY { get; set; }
    public LayerMask groundedMask;
    public LayerMask resetMask;
    Rigidbody rigidbodyR;
    Transform cameraT;

    [Header("Menu Variables")]
    public Canvas menu;
    public Canvas crosshair;
    public Slider sens;
    public Slider music;
    public AudioSource backgroundMusic;
    bool cursorVisible;
    private int canLook = 1;
    private bool canMove = true;

    [Header("Tether Variables")]
    public LineRenderer line;
    public Transform startPoint, player;
    public Vector3 endPoint, beginning;
    public float maxDistance;
    SpringJoint joint;


    // Use this for initialization
    void Start()
	{
        line.enabled = false;
        mouseSensitivityX = 1.0f;
		mouseSensitivityY = mouseSensitivityX;
		cameraT = Camera.main.transform;
		rigidbodyR = GetComponent<Rigidbody>();
		menu.gameObject.SetActive(false);
		LockMouse();
	}

	// Update is called once per frame
	void Update()
	{
        mouseSensitivityX = sens.value;
        mouseSensitivityY = sens.value;

        backgroundMusic.volume = music.value;

		// rotation
		transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivityX * canLook);
		verticalLookRotation += Input.GetAxis("Mouse Y") * mouseSensitivityY * canLook;
		verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90, 90);
		cameraT.localEulerAngles = Vector3.left * verticalLookRotation;

		// movement
		moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
		Vector3 targetMoveAmount = moveDir * walkSpeed;
		moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);

        //repulser logic
        Ray rayLaunch = new Ray(transform.position, cameraT.transform.forward);
        
        if (Physics.Raycast(rayLaunch, out hitLaunch, 6 + .2f, groundedMask))
        {
            canLaunch = true;
        }
        else
        {
            canLaunch = false;
        }

        // jump
        if (Input.GetButtonDown("Jump"))
		{
			if (grounded && canMove)
			{
				rigidbodyR.AddForce(transform.up * jumpForce);
				
			}
			
		}

        //repulser
        if (Input.GetMouseButtonDown(0))
        {
            if (canMove && canLaunch)
            {
                Repulser();
            }

        }

        //Tether Controller
        if (Input.GetMouseButtonDown(1))
        {
            line.enabled = true;
            startTether();

        }
        if (Input.GetMouseButtonUp(1))
        {
            stopTether();
            line.enabled = false;
        }

        beginning = startPoint.position;


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
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (!cursorVisible)
			{
				UnlockMouse();
				menu.gameObject.SetActive(true);
                crosshair.gameObject.SetActive(false);
                canLook = 0;
                canMove = false;
                
			}
			else
			{
				LockMouse();
				menu.gameObject.SetActive(false);
                crosshair.gameObject.SetActive(true);
                canLook = 1;
                canMove = true;
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
			
        }
		else if (other.CompareTag("Level2End"))
		{

			SceneManager.LoadScene("Level3");
			
		}
		else if (other.CompareTag("Level3End"))
		{

			SceneManager.LoadScene("Finish");
			UnlockMouse();
		}
	}

    private void OnCollisionEnter(Collision collision)
    {
		
		if (collision.gameObject.CompareTag("Reset"))
        {
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
			
        }
    }

    private void Repulser()
    {

        rigidbodyR.AddExplosionForce(launchPower, hitLaunch.point, explosionR);

        ps.Play();


        

    }

    private void LateUpdate()
    {
        DrawTether();
    }

    void startTether()
    {

        RaycastHit hit;
        if (Physics.Raycast(cameraT.position, cameraT.forward, out hit, maxDistance, groundedMask))
        {

            endPoint = hit.point;

            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = endPoint;

            float distanceFromPoint = Vector3.Distance(player.position, endPoint);

            joint.maxDistance = distanceFromPoint * .8f;
            joint.maxDistance = distanceFromPoint * .25f;


            joint.spring = 1.5f;
            joint.damper = 4f;
            joint.massScale = 10f;

            line.positionCount = 2;



        }
    }
    void stopTether()
    {
        line.positionCount = 0;

        Destroy(joint);
    }

    void DrawTether()
    {
        line.SetPosition(0, beginning);
        line.SetPosition(1, endPoint);
    }

}


