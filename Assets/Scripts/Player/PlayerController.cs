//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="PlayerController.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace Player
{
    using System.Collections;

    using Events;

    using NateTools.Attributes;

    using UnityEngine;

    using Utility;

    // TODO: Add Sprint functionality

    /// <summary>
    ///     Controls movement of the player
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        public Color DamageFlashColour = Color.red;

        public float DamageFlashInterval = 0.2f;

        public int DamageFlashTImes = 10;

        /// <summary>
        ///     Default health of the character
        /// </summary>
        public int DefaultHealth = 5;

        /// <summary>
        ///     Current Health of the character
        /// </summary>
        public int Health = 5;

        /// <summary>
        ///     Flag if the character can currently take damage
        /// </summary>
        public bool IsImmuneToDamage = false;

        public Color OriginalColour = Color.white;

        /// <summary>
        ///     The layermask of the deadly smoke
        /// </summary>
        public LayerMask SmokeMask;

        /// <summary>
        ///     Cache of the players Animator
        /// </summary>
        private Animator anim;

        /// <summary>
        ///     A cache of the players RigidBody2D
        /// </summary>
        private Rigidbody2D body;

        /// <summary>
        ///     The offset of the ground checking bounding box from the players position
        /// </summary>
        private Vector3 colliderGroundOffset;

        /// <summary>
        ///     Requested movement speed of the character horizontally
        /// </summary>
        private float currentHorizontalSpeed = 0;

        /// <summary>
        ///     The air time of the current jump
        /// </summary>
        private float currentJumpTime = 0f;

        /// <summary>
        ///     Cached horizontal axial input
        /// </summary>
        private Vector2 directionalInput;

        /// <summary>
        ///     The bounds of the ground checking bounding box
        /// </summary>
        private Bounds groundBounds;

        /// <summary>
        ///     A flag to indicate if we should execute a jump
        /// </summary>
        private bool isExecutingJump;

        /// <summary>
        ///     A Flag to indicate if we are moving downwards vertically
        /// </summary>
        private bool isFalling;

        /// <summary>
        ///     A flag to indicate if we are touching the ground layer
        /// </summary>
        private bool isGrounded;

        /// <summary>
        ///     A flag to indicate if we are actually jumping
        /// </summary>
        private bool isJumping;

        /// <summary>
        ///     A flag to indicate if we are sprinting
        /// </summary>
        private bool isSprinting;

        /// <summary>
        ///     A wrapper around the jump axis, this behaves like a key
        /// </summary>
        private AxisKey jumpAxis;

        /// <summary>
        ///     calculated velocity of a jump
        /// </summary>
        private float jumpVelocity;

        /// <summary>
        ///     A wrapper around the Sprint axis; behaves like a key
        /// </summary>
        private AxisKey sprintAxis;

        /// <summary>
        ///     A cache of the SpriteRenderer
        /// </summary>
        private SpriteRenderer sprite;

        /// <summary>
        ///     A flag to indicate if we were on the ground on the previous frame
        /// </summary>
        private bool wasGroundedLastFrame = true;

        /// <summary>
        ///     Gets or sets the Sprint resource
        /// </summary>
        public float Energy { get { return energy; } set { energy = value; } }

        /// <summary>
        ///     Gets a value indicating whether we are touching the ground layer
        /// </summary>
        public bool IsGrounded { get { return isGrounded; } }

        /// <summary>
        ///     Gets a value indicating whether we are sprinting
        /// </summary>
        public bool IsSprinting { get { return isSprinting; } }

        /// <summary>
        ///     Execute die animations and reset the character
        /// </summary>
        public void Die()
        {
            Debug.Log("player died");
            EventManager.Raise(new PlayerDied(gameObject));
        }

        /// <summary>
        ///     Reset the character to its default state
        /// </summary>
        public void ResetCharacter()
        {
            Health = DefaultHealth;
            var spawn = GameObject.FindGameObjectWithTag("Spawn");

            if (spawn != null)
            {
                transform.position = spawn.transform.position;
            }

            EventManager.Raise(new PlayerSpawned(gameObject));
        }

        public void TickDamage(GameEvent evt)
        {
            var e = evt as PlayerDamaged;

            if (e != null)

            {
                if (!IsImmuneToDamage)
                {
                    StartCoroutine(TakeDamageTick(e.Value));
                }
            }
        }

        private IEnumerator FlashSprite()
        {
            for (var i = 0; i < DamageFlashTImes; i++)
            {
                var interval = 0f;
                while (interval < DamageFlashInterval)
                {
                    sprite.color = Color.Lerp(OriginalColour, DamageFlashColour, interval / (DamageFlashInterval / 2f));
                    yield return null;

                    interval += Time.deltaTime;
                }

                interval = DamageFlashInterval;
                while (interval > 0f)
                {
                    sprite.color = Color.Lerp(OriginalColour, DamageFlashColour, interval / (DamageFlashInterval / 2f));
                    yield return null;

                    interval -= Time.deltaTime;
                }

                sprite.color = OriginalColour;
            }
        }

        /// <summary>
        ///     Check if the GroundBounds overlaps a ground layer GameObject
        /// </summary>
        private void GroundCheck()
        {
            // Move the ground collision bounds
            groundBounds.center = transform.position + colliderGroundOffset;
            var collisions = Physics2D.OverlapAreaAll(groundBounds.max, groundBounds.min, LayerMask.GetMask("Ground"));
            isGrounded = collisions.Length > 0;
        }

        /// <summary>
        ///     Coroutine to execute pre-Jump animation and then jump
        /// </summary>
        /// <returns></returns>
        private IEnumerator Jump()
        {
            if (anim)
            {
                anim.SetTrigger("Jump");
                isJumping = true;

                // Allow pre-jump anim frames to complete
                yield return new WaitForSeconds(0.100f);

                EventManager.Raise(new PlayerJumped(gameObject));
            }

            body.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);
            currentJumpTime = 0;

            Debug.Log("jump");
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<PlayerDamaged>(TickDamage);
        }

#if UNITY_EDITOR
        /// <summary>
        ///     Visualization of character properties
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(groundBounds.center, groundBounds.size);
            Gizmos.color = new Color(1f, 0.92f, 0.016f, 0.5f);
            Gizmos.DrawCube(groundBounds.center, groundBounds.size);
        }

#endif

        private void OnEnable()
        {
            EventManager.AddListener<PlayerDamaged>(TickDamage);
        }

        /// <summary>
        ///     Sends state data to the animator
        /// </summary>
        private void SetAnimationStates()
        {
            anim.SetBool("Falling", (body.velocity.y < 0f) && !isGrounded);
            anim.SetBool("MovingUp", (body.velocity.y > 0f) && !isGrounded);
            anim.SetFloat("RunSpeed", Mathf.Abs(currentHorizontalSpeed));
            anim.SetBool("Running", (currentHorizontalSpeed != 0f) && isGrounded && !isJumping);
        }

        /// <summary>
        ///     Calculates and sets the initial jump fields of the character
        /// </summary>
        private void SetJumpParameters()
        {
            /* 
             * =================================
             * g = 2.MaxHeight / TimeToApex^2
             * 
             * v[jump]^2 = V[initial]^2 + 2.g * MaxHeight
             * v[initial] = 0;
             * v[jump]^2 = 2.g * MaxHeight
             * v[jump] = sqrt(2.g * MaxHeight)
             *==================================
             */

            // calculate and set gravity scale for the character
            var g = (2f * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2f);
            body.gravityScale = (-g) / Physics2D.gravity.y;

            // set jump velocity
            jumpVelocity = Mathf.Sqrt(2f * Mathf.Abs(g) * maxJumpHeight);
        }

        /// <summary>
        ///     Initialization of the character
        /// </summary>
        private void Start()
        {
            jumpAxis = new AxisKey("Jump");
            sprintAxis = new AxisKey("Sprint");

            body = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            sprite = GetComponent<SpriteRenderer>();

            if (sprite)
            {
                OriginalColour = sprite.color;
            }

            var boxCollider2D = GetComponent<BoxCollider2D>();
            colliderGroundOffset = new Vector3(boxCollider2D.bounds.center.x, GetComponent<CircleCollider2D>().bounds.min.y) - transform.position;
            groundBounds = new Bounds(colliderGroundOffset + transform.position, new Vector3(boxCollider2D.bounds.size.x / 2, -0.1f));

            SetJumpParameters();

            EventManager.Raise(new HealthChanged(gameObject, Health));
        }

        /// <summary>
        ///     Take one tick of damage and become immune for 1 second
        /// </summary>
        /// <param name="amount">Amount of damage to take</param>
        /// <returns>
        ///     IEnumerator for a coroutine
        /// </returns>
        private IEnumerator TakeDamageTick(int amount)
        {
            if (Health < 1)
            {
                yield break;
            }

            StartCoroutine(FlashSprite());

            Health -= amount;
            EventManager.Raise(new HealthChanged(gameObject, Health));

            if (Health < 1)
            {
                Die();
                yield break;
            }

            IsImmuneToDamage = true;
            yield return new WaitForSeconds(1f);

            IsImmuneToDamage = false;
        }

        /// <summary>
        ///     Frame update, States are reset, Input is processed and executed
        /// </summary>
        private void Update()
        {
            // Update key axes wrappers and axial input
            jumpAxis.Update();
            sprintAxis.Update();

            var horizontalAxis = Input.GetAxisRaw("Horizontal");
            directionalInput = new Vector2(horizontalAxis, Input.GetAxisRaw("Vertical"));

            // Set Relevant Flags
            GroundCheck();

            // process states
            if (isGrounded)
            {
                if (!wasGroundedLastFrame)
                {
                    anim.SetTrigger("Land");
                    isJumping = false;
                }

                // set jump flag for movement routine
                if (jumpAxis.AxisDown)
                {
                    // One off physics forces are generally ok in Update, but should investigate further
                    isExecutingJump = true;
                }

                // do we have horizontal input?
                if ((directionalInput.x != 0) && ((Mathf.Sign(directionalInput.x) == Mathf.Sign(currentHorizontalSpeed)) || (currentHorizontalSpeed == 0f)))
                {
                    // accelerate in the direction pressed
                    currentHorizontalSpeed += directionalInput.x * ((1 / timeToMaxSpeed) * maxSpeed) * Time.deltaTime;

                    // make sure our initial speed is taken into account
                    currentHorizontalSpeed = Mathf.Max(Mathf.Abs(currentHorizontalSpeed), initialSpeed) * Mathf.Sign(currentHorizontalSpeed);
                }
                else
                {
                    // -Deceleration-
                    // cache the direction of movement
                    var direction = Mathf.Sign(currentHorizontalSpeed);

                    // reduce the absolute speed
                    var speed = Mathf.Abs(currentHorizontalSpeed);
                    speed -= ((1 / timeToZeroSpeed) * maxSpeed) * Time.deltaTime;

                    // clamp speed to 2ero to stop character from changing direction
                    speed = Mathf.Max(speed, 0);

                    // Set speed to computed
                    currentHorizontalSpeed = speed * direction;
                }
            }
            else
            {
                // do we have horizontal input in the opposite direction?
                if ((directionalInput.x != 0) && ((Mathf.Sign(directionalInput.x) != Mathf.Sign(currentHorizontalSpeed)) && (currentHorizontalSpeed != 0f)))
                {
                    // -Deceleration-
                    // cache the direction of movement
                    var direction = Mathf.Sign(currentHorizontalSpeed);

                    // reduce the absolute speed
                    var speed = Mathf.Abs(currentHorizontalSpeed);
                    speed -= airSpeedFactor * ((1 / timeToZeroSpeed) * maxSpeed) * Time.deltaTime;

                    // clamp speed to 2ero to stop character from changing direction
                    speed = Mathf.Max(speed, 0);

                    // Set speed to computed
                    currentHorizontalSpeed = speed * direction;
                }

                // Are we before the apex
                if (body.velocity.y > 0)
                {
                    currentJumpTime += Time.deltaTime;
                }
            }

            // -Process movement-
            // Clamp speed to MaxSpeed
            currentHorizontalSpeed = Mathf.Min(maxSpeed, Mathf.Abs(currentHorizontalSpeed)) * Mathf.Sign(currentHorizontalSpeed);

            // Set movement velocity
            body.velocity = new Vector2(currentHorizontalSpeed, body.velocity.y);

            // Process jump if flag is set
            if (isExecutingJump)
            {
                StartCoroutine(Jump());
                isExecutingJump = false;
            }

            SetAnimationStates();

            wasGroundedLastFrame = isGrounded;

            if (currentHorizontalSpeed != 0)
            {
                sprite.flipX = currentHorizontalSpeed < 0;
            }
        }

        #region Resources

        /// <summary>
        ///     Maximum Sprint energy
        /// </summary>
        [Header("Resource")]
        [SerializeField]
        [Color(0, 255, 0)]
        private float maxEnergy = 100f;

        /// <summary>
        ///     Sprint energy
        /// </summary>
        [SerializeField]
        [Color(0, 255, 0)]
        private float energy = 100f;

        #endregion

        #region Speed

        /// <summary>
        ///     The minimum speed a character can move while directional input is active
        /// </summary>
        [SerializeField]
        [Header("Speed")]
        [Color(255, 28, 0)]
        private float initialSpeed = 1f;

        /// <summary>
        ///     The maximum horizontal movement speed
        /// </summary>
        [SerializeField]
        [Color(255, 28, 0)]
        private float maxSpeed = 2f;

        /// <summary>
        ///     The maximum horizontal movement speed
        /// </summary>
        [SerializeField]
        [Color(255, 28, 0)]
        private float airSpeedFactor = 0.2f;

        /// <summary>
        ///     The time it should take to reach maximum horizontal speed
        /// </summary>
        [SerializeField]
        [Color(255, 28, 0)]
        private float timeToMaxSpeed = 2f;

        /// <summary>
        ///     The time it should take to reach 0 horizontal speed while no input is active
        /// </summary>
        [SerializeField]
        [Color(255, 28, 0)]
        private float timeToZeroSpeed = 1f;

        #endregion

        #region Jump

        /// <summary>
        ///     The time it should take to reach maximum jump height
        /// </summary>
        [Header("Jump")]
        [SerializeField]
        [Color(0, 28, 255)]
        private float timeToJumpApex = 1.5f;

        /// <summary>
        ///     The maximum height in unity units for jump apex
        /// </summary>
        [Range(1f, 10f)]
        [SerializeField]
        private float maxJumpHeight = 1.5f;

        #endregion
    }
}