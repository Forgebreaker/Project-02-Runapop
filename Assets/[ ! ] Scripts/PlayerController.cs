using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    [Header("---- Move ----")]
    [SerializeField] public float MoveSpeed = 7.5f;
    [SerializeField] private bool KeyboardController;
    private Vector3 TempPosition;
    [SerializeField] private float X_MinPosition;
    [SerializeField] private float X_MaxPosition;

    [Header("---- Jump ----")]
    [SerializeField] private float JumpForce = 10f;
    [SerializeField] private GameObject Object_IsGroundedCheckpoint;
    [SerializeField] private float Radius_IsGroundedCheckpoint;
    [SerializeField] private LayerMask WhatIsGround;
    private bool IsGrounded;
    private Rigidbody2D Player_RB;

    [Header("---- Animation ----")]
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private string Bool_PlayerMove = "Move";
    private string Float_PlayerInAir = "Y_Velocity";
    private string Bool_PlayerGrounded = "IsGrounded";

    [Header("---- Button Gathering ----")]
    private GameObject LeftMovement_Button_GO;
    private GameObject RightMovement_Button_GO;
    private GameObject Jump_Button_GO;
    private bool isMovingLeft = false;
    private bool isMovingRight = false;

    [Header("---- Sound Effects ----")]

    private AudioSource _audioSource;
    [SerializeField] private AudioClip JumpSound;
    [SerializeField] private AudioClip CoinGatheringSound;
    [SerializeField] private AudioClip DeathSound;

    [Header("---- Other Stuffs ----")]
    [SerializeField] private int PlayerMaxHealth;
    [SerializeField] private AudioClip HitSound;
    private int PlayerCurrentHealth;
    private float HitCoolDown = 1f;
    private float CurrentHitCooldown = 0f;
    private bool IsAlive = true;
    private GameObject Health_GO;
    private TMP_Text Health_text;
    private GameObject CoinIncreasedUI_GO;
    private TMP_Text CoinIncreasedUI;
    private int CoinGatheredNumber;
    private bool Able2GetCoin;
    private float GetCoinTime = 0.05f;
    private float CurrentGetCoinTime;
    [SerializeField] private GameObject CoinGatheredEffect;
    [SerializeField] private GameObject DeathEffect;


    void Start()
    {
        PlayerCurrentHealth = PlayerMaxHealth;
        CurrentGetCoinTime = GetCoinTime;
        CoinGatheredNumber = 0;
        Player_RB = this.gameObject.GetComponent<Rigidbody2D>();
        _audioSource = this.gameObject.GetComponent<AudioSource>();
        _animator = this.gameObject.GetComponentInChildren<Animator>();
        _spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        ResourcesGathering();
        IsAlive = true;

    }

    void Update()
    {
        if (IsAlive)
        {
            MoveFunction();
            AnimationControlFunction();
            KeyboardControllerFunction();
            IsGrounded = Physics2D.OverlapCircle(Object_IsGroundedCheckpoint.transform.position,
                                                 Radius_IsGroundedCheckpoint, WhatIsGround);
        }

        if (CurrentGetCoinTime <= 0)
        {
            Able2GetCoin = true;
        }
        else if (CurrentGetCoinTime > 0)
        {
            Able2GetCoin = false;
            CurrentGetCoinTime -= Time.deltaTime;
        }

        if (CurrentHitCooldown > 0) 
        {
            CurrentHitCooldown -= Time.deltaTime;
        }

        if (PlayerCurrentHealth <= 0 && IsAlive)
        {
            StartCoroutine(WaitToRealDeath());
            Instantiate(DeathEffect, this.gameObject.transform.position, Quaternion.identity);
            IsAlive = false;
        }

        CoinIncreasedUI.text = $"{CoinGatheredNumber}";
        Health_text.text = $"x{PlayerCurrentHealth}";
    }

    private void MoveFunction() 
    {
        if (isMovingLeft)
        {
            this.gameObject.transform.Translate(new Vector3(-1, 0, 0) * MoveSpeed * Time.deltaTime, Space.World);
            this.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        if (isMovingRight)
        {
            this.gameObject.transform.Translate(new Vector3(1, 0, 0) * MoveSpeed * Time.deltaTime, Space.World);
            this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        TempPosition = this.gameObject.transform.position;

        if (TempPosition.x < X_MinPosition)
        {
            TempPosition.x = X_MinPosition;
        }

        if (TempPosition.x > X_MaxPosition)
        {
            TempPosition.x = X_MaxPosition;
        }

        this.gameObject.transform.position = TempPosition;
    }

    private void JumpFunction()
    {
        if (IsGrounded == true)
        {
            _audioSource.PlayOneShot(JumpSound, Random.Range(0.5f, 1f));
            Player_RB.linearVelocity = new Vector2(Player_RB.linearVelocity.x, JumpForce);
        }
    }

    private void AnimationControlFunction() 
    {
        _animator.SetBool(Bool_PlayerMove, isMovingLeft == true || isMovingRight == true);
        _animator.SetBool(Bool_PlayerGrounded, IsGrounded);
        _animator.SetFloat(Float_PlayerInAir, Player_RB.linearVelocityY);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && PlayerCurrentHealth >= 1 && CurrentHitCooldown <= 0 && IsAlive)
        {
            PlayerCurrentHealth--;
            CurrentHitCooldown = HitCoolDown; // Ensure this is the correct variable name
            StartCoroutine(LostHealthEffect());
        }

        if (collision.CompareTag("Coin") && Able2GetCoin == true)
        {
            Destroy(collision.gameObject);
            CoinGatheredNumber += 1;
            CurrentGetCoinTime = GetCoinTime;
            GameDataManager.AddCoins(1);
            _audioSource.PlayOneShot(CoinGatheringSound);
            Instantiate(CoinGatheredEffect, collision.transform.position, Quaternion.identity);
        }
    }

    IEnumerator LostHealthEffect()
    {
        SpriteRenderer Avatar = _animator.gameObject.GetComponent<SpriteRenderer>();
        Avatar.color = new Color(Avatar.color.r, Avatar.color.g, Avatar.color.b, 0.5f);
        _audioSource.PlayOneShot(HitSound);
        yield return new WaitForSeconds(CurrentHitCooldown);
        Avatar.color = new Color(Avatar.color.r, Avatar.color.g, Avatar.color.b, 1f);
    }


    IEnumerator WaitToRealDeath() 
    { 
        this.gameObject.transform.localScale = Vector3.zero;
        _audioSource.PlayOneShot(DeathSound);
        GameDataManager.NewestCoinNumbGathered(CoinGatheredNumber);
        yield return new WaitForSeconds(2f);
        Scene currentScene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene("Result");
    }

    /*************************************************** Calculate & Resource Data ****************************************************************/

    private void OnDrawGizmos()
    {
        if (Object_IsGroundedCheckpoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(Object_IsGroundedCheckpoint.transform.position, Radius_IsGroundedCheckpoint);
        }
    }

    void ResourcesGathering()
    {
        LeftMovement_Button_GO = GameObject.FindGameObjectWithTag("LeftMoveButton");

        if (LeftMovement_Button_GO != null)
        {
            EventTrigger trigger = LeftMovement_Button_GO.AddComponent<EventTrigger>();
            AddEventTrigger(trigger, EventTriggerType.PointerDown, (eventData) => isMovingLeft = true);
            AddEventTrigger(trigger, EventTriggerType.PointerUp, (eventData) => isMovingLeft = false);
        }

        RightMovement_Button_GO = GameObject.FindGameObjectWithTag("RightMoveButton");

        if (RightMovement_Button_GO != null)
        {
            EventTrigger trigger = RightMovement_Button_GO.AddComponent<EventTrigger>();
            AddEventTrigger(trigger, EventTriggerType.PointerDown, (eventData) => isMovingRight = true);
            AddEventTrigger(trigger, EventTriggerType.PointerUp, (eventData) => isMovingRight = false);
        }

        Jump_Button_GO = GameObject.FindGameObjectWithTag("JumpButton");

        if (Jump_Button_GO != null)
        {
            EventTrigger trigger = Jump_Button_GO.AddComponent<EventTrigger>();
            AddEventTrigger(trigger, EventTriggerType.PointerDown, (eventData) => JumpFunction());
        }

        CoinIncreasedUI_GO = GameObject.FindGameObjectWithTag("Coin Increased UI");
        if (CoinIncreasedUI_GO != null) 
        {
            CoinIncreasedUI = CoinIncreasedUI_GO.GetComponent<TMP_Text>();
        }

        Health_GO = GameObject.FindGameObjectWithTag("Player Health");
        if (Health_GO != null)
        {
            Health_text = Health_GO.GetComponent<TMP_Text>();
        }
    }

    void AddEventTrigger(EventTrigger trigger, EventTriggerType eventType, UnityEngine.Events.UnityAction<BaseEventData> action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = eventType };
        entry.callback.AddListener(action);
        trigger.triggers.Add(entry);
    }

    private void KeyboardControllerFunction() 
    {
        if (KeyboardController == true)
        {
            float HorizontalInput = Input.GetAxisRaw("Horizontal");
            this.gameObject.transform.Translate(new Vector3(HorizontalInput, 0, 0) * MoveSpeed * Time.deltaTime, Space.World);

            if (HorizontalInput < -0.1f) 
            {
                this.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
            }

            if (HorizontalInput > 0.1f)
            {
                this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            if (Input.GetKeyDown(KeyCode.Space)) 
            {
                JumpFunction();
            }

            isMovingLeft = isMovingRight = HorizontalInput != 0;
        }
    }

}
