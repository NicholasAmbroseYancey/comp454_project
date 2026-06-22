using UnityEngine;
using UnityEngine.InputSystem; 
using TMPro; 

public class PlayOnKeyPress : MonoBehaviour
{
    private Animator anim;
    private bool isWalking = true; 
    private bool isWaitingForStick = false; 

    public float interactionDistance = 3f; 
    public float circleRadius = 5f;
    public float walkSpeed = 2f;
    private Transform playerTransform;

    private Vector3 startPosition;
    private float currentAngle;

    public TextMeshProUGUI dialogueText; 
    public string npcMessage = "Hello traveler!"; 
    
    [Header("Stick Quest Settings")]
    public string thankYouMessage = "Thank you for finding my stick!"; 
    public Transform stickTransform; 
    public float stickDeliveryDistance = 10f; 

    void Start()
    {
        anim = GetComponent<Animator>();
        
        startPosition = transform.position;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerTransform = player.transform;
        }

        if (dialogueText != null)
        {
            dialogueText.text = "";
        }
    }

    void Update()
    {
        // Controls physical movement in the scene
        if (isWalking)
        {
            currentAngle += Time.deltaTime * walkSpeed;

            float x = startPosition.x + Mathf.Cos(currentAngle) * circleRadius;
            float z = startPosition.z + Mathf.Sin(currentAngle) * circleRadius;
            
            float terrainHeight = Terrain.activeTerrain.SampleHeight(new Vector3(x, 0, z));

            Vector3 nextPosition = new Vector3(x, terrainHeight, z);
            Vector3 moveDirection = nextPosition - transform.position;

            if (moveDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveDirection);
            }

            transform.position = nextPosition;
        }

        float currentDistance = Vector3.Distance(transform.position, playerTransform.position);

        if (currentDistance <= interactionDistance)
        {
            if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            {
                if (isWalking && !isWaitingForStick)
                {
                    // 1. Fire the Talk trigger
                    if (anim != null) anim.SetTrigger("Talk");
                    
                    isWalking = false;
                    isWaitingForStick = true; 

                    transform.LookAt(new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z));
                    
                    if (dialogueText != null) 
                    {
                        dialogueText.text = npcMessage;
                    }
                }
            }
        }

        if (isWaitingForStick && stickTransform != null)
        {
            float distanceToStick = Vector3.Distance(transform.position, stickTransform.position);

            if (distanceToStick <= stickDeliveryDistance)
            {
                CompleteQuest();
            }
        }
    }

    private void CompleteQuest()
    {
        isWaitingForStick = false; 
        
        if (dialogueText != null) 
        {
            dialogueText.text = thankYouMessage;
        }

        // 2. Fire the Jump trigger
        if (anim != null)
        {
            anim.SetTrigger("Jump");
        }

        // Hide the stick object
        if (stickTransform != null)
        {
            stickTransform.gameObject.SetActive(false); 
        }

        Invoke("ResumeWalkingMath", 1.2f); 
    }

    private void ResumeWalkingMath()
    {
        // We don't need anim.SetTrigger("Walk") here anymore! 
        // Unity's "Has Exit Time" handles the animation transition for us.
        
        isWalking = true; // Unfreezes the NPC's movement script

        if (dialogueText != null) 
        {
            dialogueText.text = "";
        }
    }
}