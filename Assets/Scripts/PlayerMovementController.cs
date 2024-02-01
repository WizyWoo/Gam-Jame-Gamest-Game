using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{

    public float MovementSpeed, SprintSpeedModifier, JumpHeight, AirControlMult, CoyoteTime;
    public bool DrawGizmos;
    private float sprinting, coyoteTimer;
    [SerializeField]
    private bool sprintingcheck, grounded, crouching;
    private Vector3 movementDir, spawnOrigin;
    private Rigidbody rb;
    private LayerMask playerMask;
    private CameraController cc;
    [SerializeField]
    private float groundCheckRad;
    [SerializeField]
    private Vector3 groundCheckOffSet;
    [SerializeField]
    private float _crouchHeight = 0.8f;
    [SerializeField]
    private Transform _camPivot;
    void Start()
    {

        cc = Camera.main.GetComponent<CameraController>();
        rb = gameObject.GetComponent<Rigidbody>();
        playerMask = ~(1 << LayerMask.NameToLayer("Player"));
        spawnOrigin = transform.position;

    }

    void Update()
    {

        if(SoundManager.instance)
        {

            if(Input.GetKeyDown(KeyCode.B))
            {
                SoundManager.instance.PlaySound(1);
            }

            if (grounded && rb.velocity.magnitude > 0.1f && !SoundManager.instance.audioSources[0].isPlaying)
            {
                SoundManager.instance.PlaySound(0);
            }
            else if (!grounded || rb.velocity.magnitude < 0.1f)
            {
                SoundManager.instance.StopSound(0);
            }

        }

        if (transform.position.y < -5)
        {

            rb.velocity = Vector3.zero;
            transform.position = spawnOrigin;

        }

        if (Physics.CheckSphere(transform.position + groundCheckOffSet, groundCheckRad, playerMask, QueryTriggerInteraction.Ignore))
        {

            grounded = true;
            coyoteTimer = CoyoteTime;

        }
        else if (coyoteTimer <= 0)
        {

            grounded = false;

        }

        if (Input.GetKeyDown(KeyCode.Space) && grounded == true)
        {

            rb.velocity = new Vector3(rb.velocity.x, JumpHeight, rb.velocity.z);
            coyoteTimer = 0;

        }

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W) && crouching == false)
        {

            sprinting = SprintSpeedModifier;
            cc.Sprinting = true;
            if(SoundManager.instance)
                SoundManager.instance.SetPitch(1.5f); // Play the sound 1.5 times faster

        }
        else
        {

            sprinting = 1;
            cc.Sprinting = false;
            if(SoundManager.instance)
                SoundManager.instance.SetPitch(1f); // Play the sound 1.5 times faster

        }

        if(Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.LeftControl))
        {

            if(crouching)
            {

                GetComponent<CapsuleCollider>().height = 1.8f;
                transform.position += new Vector3(0, 0.51f, 0);
                _camPivot.localPosition = new Vector3(0, 0.8f, 0);
                crouching = false;

            }
            else
            {

                GetComponent<CapsuleCollider>().height = _crouchHeight;
                transform.position -= new Vector3(0, 0.49f, 0);
                _camPivot.localPosition = new Vector3(0, 0.35f, 0);
                crouching = true;

            }

        }

        if(crouching)
        {

            sprinting = 0.5f;

        }

        movementDir = (transform.forward * sprinting * Input.GetAxis("Vertical") * MovementSpeed) + (transform.right * sprinting * Input.GetAxis("Horizontal") * MovementSpeed);

        if (grounded)
        {

            movementDir += new Vector3(0, rb.velocity.y, 0);

        }
        else
        {

            Vector3 airControlV3 = new Vector3();

            if (Mathf.Abs(rb.velocity.x) < MovementSpeed * sprinting)
            {

                airControlV3.x = movementDir.x * AirControlMult * Time.deltaTime + rb.velocity.x;

            }
            else
            {

                airControlV3.x = rb.velocity.x;

            }

            if (Mathf.Abs(rb.velocity.z) < MovementSpeed * sprinting)
            {

                airControlV3.z = movementDir.z * AirControlMult * Time.deltaTime + rb.velocity.z;

            }
            else
            {

                airControlV3.z = rb.velocity.z;

            }

            airControlV3.y = rb.velocity.y;
            movementDir = airControlV3;

        }

        rb.velocity = movementDir;

        if (coyoteTimer > 0)
            coyoteTimer -= Time.deltaTime;

    }

    private void OnDrawGizmos()
    {

        if (DrawGizmos)
        {

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position + groundCheckOffSet, groundCheckRad);

        }

    }

    

}
