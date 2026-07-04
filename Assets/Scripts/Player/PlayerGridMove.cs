using System.Collections;
using UnityEngine;

public class PlayerGridMove : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("How fast the player transitions between tiles.")]
    public float walkSpeed = 4f;

    [Header("Grid Detection Layers")]
    [Tooltip("The Unity Layer containing your solid world geometry tilemaps (walls, fences).")]
    public LayerMask solidObjectsLayer;
    [Tooltip("The Unity Layer containing interactable world elements (NPCs, signs).")]
    public LayerMask interactableLayer;
    [Tooltip("The Unity Layer containing map boundary transitions (doors, cave entry tiles).")]
    public LayerMask triggerLayer;

    // State Tracking Flags
    private bool isMoving;                          // Flips to true during active tile-to-tile transitions
    private Vector2 input;                          // Stores raw directional keyboard/D-pad values
    private Vector2 facingDirection = Vector2.down; // Keeps track of orientation (Defaults to South/Down)

    // Component References
    private Animator animator;

    private void Awake()
    {
        // Cache the animator component attached to this game object at startup
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Rule: Only process inputs if the player is stationary on a tile center
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            // Enforce classic cardinal limit (No diagonal movements)
            if (input.x != 0) input.y = 0;

            if (input != Vector2.zero)
            {
                // --- CARDINAL TURN CHECK ---
                if (input != facingDirection)
                {
                    UpdateFacingDirection(); // Flip the sprite orientation properties instantly

                    // GEN I ACCURACY FILTER: Only trigger the turn-in-place latency delay 
                    // if the character was completely stationary (meaning the Walk animation state was inactive).
                    // If the Animator is still flagged as moving, we bypass the delay entirely!
                    if (animator != null && !animator.GetBool("isMoving"))
                    {
                        StartCoroutine(TriggerTurnDelay());
                        return; // Halt this frame so a tap only rotates the character
                    }
                }

                // Calculate targeted tile coordinates
                Vector3 targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                if (IsWalkable(targetPos))
                {
                    StartCoroutine(MoveToGridCell(targetPos));
                }
                else
                {
                    TriggerWallBump();
                }
            }
        }
    }


    // Replicates the hardware latency window of the Game Boy D-Pad checking loops
    private IEnumerator TriggerTurnDelay()
    {
        isMoving = true; // Lock the player state machine briefly so they can't step forward yet

        // 0.1 seconds perfectly mimics the 1-2 frames of turn lag in Pokemon Red
        yield return new WaitForSeconds(0.1f);

        isMoving = false; // Open the player state machine back up for inputs
    }



    private void UpdateFacingDirection()
    {
        // Convert the input vector into a strict absolute cardinal enum vector
        if (input.x > 0) facingDirection = Vector2.right;
        else if (input.x < 0) facingDirection = Vector2.left;
        else if (input.y > 0) facingDirection = Vector2.up;
        else if (input.y < 0) facingDirection = Vector2.down;

        // Push values into the flat 2D Simple Directional Blend Trees
        if (animator != null)
        {
            animator.SetFloat("Horizontal", facingDirection.x);
            animator.SetFloat("Vertical", facingDirection.y);
        }
    }

    private bool IsWalkable(Vector3 targetPos)
    {
        // Calculate the exact midway cell between his start and end points
        Vector3 midwayPos = transform.position + (targetPos - transform.position) * 0.5f;

        // Check both the midway cell AND the final landing cell for walls
        if (Physics2D.OverlapCircle(midwayPos, 0.1f, solidObjectsLayer | interactableLayer) != null ||
            Physics2D.OverlapCircle(targetPos, 0.1f, solidObjectsLayer | interactableLayer) != null)
        {
            return false; // Path is blocked by an 8x8 element
        }
        return true; // Path is completely clear
    }


    private IEnumerator MoveToGridCell(Vector3 targetPos)
    {
        isMoving = true;

        if (animator != null)
            animator.SetBool("isMoving", true);

        // Smooth Movement Loop: Keep moving closer until the remaining distance closes entirely
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, walkSpeed * Time.deltaTime);
            yield return null;
        }

        // Hard-Snap Guard: Force position coordinates onto precise integers
        transform.position = targetPos;

        // Post-Step Checklist: The player has landed precisely on a new tile, check if it's a doorway
        CheckForAreaTransitions();

        // --- NEW CONTINUOUS WALK LOGIC ---
        // Before we turn off the movement animation, check if the player is STILL holding down a key
        // We fetch the input raw again right at the moment of landing
        float nextInputX = Input.GetAxisRaw("Horizontal");
        float nextInputY = Input.GetAxisRaw("Vertical");
        if (nextInputX != 0) nextInputY = 0; // Enforce cardinal limit

        // If they released the keys, or if the next tile ahead is blocked, STOP the animation
        Vector3 nextTargetPos = transform.position + new Vector3(nextInputX, nextInputY, 0);
        if (new Vector2(nextInputX, nextInputY) == Vector2.zero || !IsWalkable(nextTargetPos))
        {
            isMoving = false;

            if (animator != null)
                animator.SetBool("isMoving", false); // Only snap back to Idle template if they actually stop walking
        }
        else
        {
            // If they are still holding the button and the path is clear, we KEEP isMoving true.
            // This prevents the animator from resetting to frame 0, making the walk continuous!
            isMoving = false;
        }
    }


    private void CheckForAreaTransitions()
    {
        // Drop a localized search checking if the current tile contains an interactive map boundary
        Collider2D triggerCheck = Physics2D.OverlapCircle(transform.position, 0.1f, triggerLayer);
        if (triggerCheck != null)
        {
            // TEMPORARILY COMMENTED OUT: Awaiting Area transition script confirmation
            /*
            if (triggerCheck.TryGetComponent<AreaMove>(out var areaTransition))
            {
                Debug.Log($"Stepped onto a door trigger! Running area transition logic.");
            }
            */

            Debug.Log("Stepped onto a trigger tile!");
        }
    }


    private void TriggerWallBump()
    {
        // Prints message to the Unity Console. Perfect hook point for your audio management systems.
        Debug.Log("BUMP! Playing classic thud sound effect.");
    }
}
