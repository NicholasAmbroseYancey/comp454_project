using UnityEngine;
using UnityEngine.InputSystem; 
using TMPro; 

public class PlayOnKeyPress : MonoBehaviour
{
    private Animator anim;
    private bool isWalking = true; 

    public float interactionDistance = 3f; 
    public float circleRadius = 5f;
    public float walkSpeed = 2f;
    private Transform playerTransform;

    private Vector3 startPosition;
    private float currentAngle;

    public TextMeshProUGUI dialogueText; 
    public string npcMessage = "Hello traveler!"; 

    void Start()
    {
        anim = GetComponent<Animator>();
        
        startPosition = transform.position;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        playerTransform = player.transform;

        if (dialogueText != null)
        {
            dialogueText.text = "";
        }
    }

    void Update()
    {
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
                if (isWalking)
                {
                    anim.Play("Talk");
                    isWalking = false;

                    transform.LookAt(new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z));
                    
                    if (dialogueText != null) 
                    {
                        dialogueText.text = npcMessage;
                    }
                }
                else
                {
                    anim.Play("Walking");
                    isWalking = true;

                    if (dialogueText != null) 
                    {
                        dialogueText.text = "";
                    }
                }
            }
        }
    }
}