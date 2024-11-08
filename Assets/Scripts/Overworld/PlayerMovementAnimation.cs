/*using UnityEngine;

public class PlayerMovementAnimation : MonoBehaviour
{
    private Animator animator;
    private Vector3 movement;

    void Start()
    {
        // Get the Animator component
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Get input axes
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Set animation parameters
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude); // Speed determines idle or walk state

        // Move the player
        //transform.Translate(movement * speed * Time.deltaTime);
    }
}

*//*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementAnimation : MonoBehaviour
{
    public float frame_rate = .25f;
    public PlayerMovement pm;
    public Sprite[] walk_up;
    public Sprite[] run_up;
    public Sprite[] walk_down;
    public Sprite[] run_down;
    public Sprite[] walk_left;
    public Sprite[] run_left;
    public Sprite[] walk_right;
    public Sprite[] run_right;
    public Sprite[] wink;

    private CheckMovementDirection direction;
    private SpriteRenderer sr;
    private Coroutine movement_animation;
    private Sprite idle_sprite;
    // Start is called before the first frame update
    void Start()
    {
        direction = GetComponent<CheckMovementDirection>();
        sr = GetComponent<SpriteRenderer>();
        idle_sprite = walk_down[1];
        movement_animation = StartCoroutine(CoroutineUtilities.StartContinuousAnimation(sr, wink, frame_rate));
    }

    // Update is called once per frame
    void Update()
    {
        if (pm.isRunning)
        {
            if (direction.GetMovementDirection() == Vector3.up)
            {
                movement_animation = StartCoroutine(CoroutineUtilities.StartContinuousAnimation(sr, run_up, frame_rate));
                idle_sprite = walk_up[1];
            }
            else
            if (direction.GetMovementDirection() == Vector3.down)
            {
                movement_animation = StartCoroutine(CoroutineUtilities.StartContinuousAnimation(sr, run_down, frame_rate));
                idle_sprite = walk_down[1];
            }
            else
            if (direction.GetMovementDirection() == Vector3.left)
            {
                movement_animation = StartCoroutine(CoroutineUtilities.StartContinuousAnimation(sr, run_left, frame_rate));
                idle_sprite = walk_left[1];
            }
            else
            if (direction.GetMovementDirection() == Vector3.right)
            {
                movement_animation = StartCoroutine(CoroutineUtilities.StartContinuousAnimation(sr, run_right, frame_rate));
                idle_sprite = walk_right[1];
            }
            else
            {
                sr.sprite = idle_sprite;*//*
                if (idle_sprite == walk_down)*//*
            }
        }
        else
        {
            if (direction.GetMovementDirection() == Vector3.up)
            {
                movement_animation = StartCoroutine(CoroutineUtilities.StartContinuousAnimation(sr, walk_up, frame_rate));
                idle_sprite = walk_up[1];
            }
            else
            if (direction.GetMovementDirection() == Vector3.down)
            {
                movement_animation = StartCoroutine(CoroutineUtilities.StartContinuousAnimation(sr, walk_down, frame_rate));
                idle_sprite = walk_down[1];
            }
            else
            if (direction.GetMovementDirection() == Vector3.left)
            {
                movement_animation = StartCoroutine(CoroutineUtilities.StartContinuousAnimation(sr, walk_left, frame_rate));
                idle_sprite = walk_left[1];
            }
            else
            if (direction.GetMovementDirection() == Vector3.right)
            {
                movement_animation = StartCoroutine(CoroutineUtilities.StartContinuousAnimation(sr, walk_right, frame_rate));
                idle_sprite = walk_right[1];
            }
            else
            {
                sr.sprite = idle_sprite;
            }
        }
    }
}

*/