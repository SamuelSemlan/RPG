using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float walkMoveStopRadius = .2f;
    [SerializeField] float attackMoveStopRadius = 5f;

    ThirdPersonCharacter thirdPersonCharacter;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentDestination, clickPoint;


    bool isInDirectMode = false;

    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        currentDestination = transform.position;
    }
    

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.G)) // TODO add to menus
        {
            isInDirectMode = !isInDirectMode; // Toggle mode
            currentDestination = transform.position; //Clear the click target
        }

        if (isInDirectMode)
        {
            ProcessDirectMovement();
        }
        else
        {
            ProcessMouseMovement();
        }
    }

    private void ProcessDirectMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Calculate camera relative direction to move:
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 movement = v * cameraForward + h * Camera.main.transform.right;
        thirdPersonCharacter.Move(movement, false, false);
    }

    private void ProcessMouseMovement()
    {
        if (Input.GetMouseButton(0))
        {
            clickPoint = cameraRaycaster.hit.point;
            switch (cameraRaycaster.currentLayerHit)
            {
                case Layer.Walkable:
                    currentDestination = ShortDestination(clickPoint, walkMoveStopRadius);
                    break;
                case Layer.Enemy:
                    currentDestination = ShortDestination(clickPoint, attackMoveStopRadius);
                    break;
                default:
                    print("Unexpected layer found");
                    return;
            }
        }
        WalkToDestination();
    }

    private void WalkToDestination()
    {
        var playerToClickpoint = currentDestination - transform.position;
        if (playerToClickpoint.magnitude >= 0)
        {
            thirdPersonCharacter.Move(currentDestination - transform.position, false, false);
        }
        else
        {
            thirdPersonCharacter.Move(Vector3.zero, false, false);
        }
    }

    Vector3 ShortDestination(Vector3 destination, float shortening)
    {
        Vector3 reductionVector = (destination - transform.position).normalized * shortening;
        return destination - reductionVector;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, currentDestination);
        Gizmos.DrawSphere(currentDestination, .1f);
        Gizmos.DrawSphere(clickPoint, .15f);
        
//        Gizmos.DrawWireSphere(transform.position, attackMoveStopRadius);
    }
}

