using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepulserController : MonoBehaviour
{

	

	public Camera cam;

	public ParticleSystem ps;

	

	float verticalLookRotation;

	public float launchPower;
	public float explosionR;

	bool grounded = true;

	public LayerMask groundedMask;
	Rigidbody rigidbodyR;

    private bool canMove;


    // Start is called before the first frame update
    void Start()
    {
		rigidbodyR = GetComponent<Rigidbody>();

        

	}

    // Update is called once per frame
    void Update()
    {
		Ray ray = new Ray(transform.position, cam.transform.forward);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, 6 + .2f, groundedMask))
		{
			grounded = true;
		}
		else
		{
			grounded = false;
		}

		if (Input.GetMouseButtonDown(0))
		{
			if (grounded)
			{
				rigidbodyR.AddExplosionForce(launchPower, hit.point, explosionR );

				ps.Play();


			}

		}

		
		

		
	}
}
