/*
The CharacterController component is used to control the movement of the player.
*/

using UnityEngine;

namespace ScriptBoy.DiggableTerrains2D_Demos
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterController2D : MonoBehaviour
    {
        [SerializeField] Transform[] m_FlipTransforms;

        Rigidbody2D m_Rigidbody2D;
        Vector3 m_Velocity;

        bool m_IsFacingRight = true;
        bool m_IsGrounded;

        private Animator _animator;
        public float moveSpeed = 5f;
        public float jumpForce = 10f;
        private int maxJumps = 1;
        private int jumpCount;

       /// <summary>
        /// Determining which way the player is facing.
        /// </summary>
        public bool isFacingRight => m_IsFacingRight;

        /// <summary>
        /// Whether or not the player is grounded.
        /// </summary>
        public bool isGrounded => m_IsGrounded;


        int m_Grounds = 0;

        void Start()
        {
            //Get the Rigidbody2D attached to the GameObject
            _animator = GetComponent<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public void Update()
        {
            
            float horizontalInput = Input.GetAxis("Horizontal"); 
            m_Rigidbody2D.linearVelocity =
                new Vector2(horizontalInput * moveSpeed, m_Rigidbody2D.linearVelocity.y); 

            
            if (isGrounded)
            {
                jumpCount = 0;
            }

            
            if (Input.GetButtonDown("Jump") && jumpCount < maxJumps)
            {
                Jump();
            }

            if (m_Rigidbody2D.linearVelocity.x > 0 && !m_IsFacingRight)
            {
                Flip();
            }
            else if (m_Rigidbody2D.linearVelocity.x < 0 && m_IsFacingRight)
            {
                Flip();
            }
        }

        void Jump()
        {
            _animator.SetTrigger("Jump");
            jumpCount++;
            m_Rigidbody2D.linearVelocity = new Vector2(m_Rigidbody2D.linearVelocity.x, jumpForce); // Y ekseninde zıpla
        }

        // Switch the way the player is facing.
        void Flip()
        {
            m_IsFacingRight = !m_IsFacingRight;
            foreach (var t in m_FlipTransforms)
            {
                Vector3 theScale = t.localScale;
                theScale.x *= -1;
                t.localScale = theScale;
            }
        }

        // Detect collision enter with ground.
        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.tag == "Ground")
            {
                m_Grounds++;
            }

            m_IsGrounded = m_Grounds > 0;
        }

        // Detect collision exit with ground.
        void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.collider.tag == "Ground")
            {
                m_IsGrounded = false;
                m_Grounds--;
            }

            m_IsGrounded = m_Grounds > 0;
        }
    }
}