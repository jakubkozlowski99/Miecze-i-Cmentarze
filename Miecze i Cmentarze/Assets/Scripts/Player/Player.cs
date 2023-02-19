using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mover
{
    public int stamina = 100;
    public int maxStamina = 100;

    public Animator anim;
    private bool isAttacking;
    public bool canMove;
    private bool isDodging;
    public bool canAttack;

    private readonly string swing_1 = "adventurer_swing_1";
    private readonly string swing_2 = "adventurer_swing_2";
    private readonly string swing_3 = "adventurer_swing_3";
    private readonly string hurt = "adventurer_hurt";
    private readonly string dodge = "adventurer_dodge";

    public float playerYSpeed = 1.5f;
    public float playerXSpeed = 2;
    private int swingCount = 0;
    private float lastSwing;
    private float swingReset = 0.75f;
    private float dodgeX;
    private float dodgeY;
    private float dodgeCooldown = 0.3f;
    private float lastDodge;

    public PlayerBar healthBar;
    public PlayerBar staminaBar;
    public PlayerBar xpBar;
    public PlayerStats playerStats;
    private float staminaRegenTimer;
    private float staminaRegenCooldown = 0.2f;

    public bool alive;

    private bool isRunning;

    protected override void Start()
    {
        base.Start();
        alive = true;
        if (FindObjectsOfType<Player>().Length > 1) 
        {
            Destroy(gameObject);
            return;
        }
        GameManager.instance.player = this;
        GameManager.instance.floatingTextManager = FindObjectOfType<FloatingTextManager>();
        Inventory.instance.items.Clear();
        canMove = true;
        canAttack = true;
        anim = GetComponent<Animator>();
        anim.SetFloat("AttackSpeed", 1 + (playerStats.condition / 10) - 0.1f);
        if (SaveManager.instance.isLoading == false)
        {
            maxhitpoint = 60 + (playerStats.vitality * 40);
            hitpoint = maxhitpoint;
            healthBar.SetAllBars("hp");
            staminaBar.SetAllBars("stamina");
        }
        else SaveManager.instance.Load();
        SaveManager.instance.isLoading = false;
        staminaRegenTimer = 0;
        xpBar.SetXpBar();
        playerStats.UpdateStats();

        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if (isAttacking || !canMove) 
        {
            x = 0;
            y = 0;
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(swing_1) || anim.GetCurrentAnimatorStateInfo(0).IsName(hurt)
            || anim.GetCurrentAnimatorStateInfo(0).IsName(swing_2) || anim.GetCurrentAnimatorStateInfo(0).IsName(swing_3)) 
        { 
            isAttacking = true; 
        }
        else isAttacking = false;

        if (anim.GetCurrentAnimatorStateInfo(0).IsName(dodge)) isDodging = true;
        else isDodging = false;

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("adventurer_run") || PauseMenu.instance.gameIsPaused)
        {
            AudioManager.instance.Stop("run_grass");
            isRunning = false;
        }

        if (!PauseMenu.instance.gameIsPaused)
        {
            if (x != 0 || y != 0 && !isAttacking && !isDodging)
            {
                anim.SetBool("Run", true);
                if (!isRunning && anim.GetCurrentAnimatorStateInfo(0).IsName("adventurer_run") && !PauseMenu.instance.gameIsPaused)
                {
                    AudioManager.instance.Play("run_grass");
                    isRunning = true;
                }
            }
            else if ((x == 0 && y == 0) || isAttacking || isDodging)
            {
                anim.SetBool("Run", false);
                if (isRunning)
                {
                    AudioManager.instance.Stop("run_grass");
                    isRunning = false;
                }
            }

            if (Time.time - lastSwing > swingReset) swingCount = 0;

            if (Input.GetKeyDown(KeyCode.Q))
            {
                Swing();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (Time.time - lastDodge > dodgeCooldown && stamina >= 20)
                {
                    Dodge(x, y);
                }
            }
        }

        StaminaRegen();

        if (isDodging) UpdateMotor(new Vector3(dodgeX, dodgeY, 0), playerYSpeed * 2, playerXSpeed * 2);
        else UpdateMotor(new Vector3(x, y, 0), playerYSpeed, playerXSpeed);

        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Dead"))
        {
            UI.instance.deathScreen.SetActive(true);
        }
    }


    private void Swing()
    {
        if (canAttack && !isAttacking && !isDodging)
        {
            if (stamina >= 10)
            {
                isAttacking = true;
                anim.SetBool("Run", false);
                if (swingCount == 0)
                {
                    anim.SetTrigger("Swing1");
                    swingCount++;
                }
                else if (swingCount == 1)
                {
                    anim.SetTrigger("Swing2");
                    swingCount++;
                }
                else if (swingCount == 2)
                {
                    anim.SetTrigger("Swing3");
                    swingCount = 0;
                }
                lastSwing = Time.time;
                stamina -= 10;
                staminaBar.SetValue(stamina);
                AudioManager.instance.Play("player_slash");
            }
        }
    }

    private void Dodge(float x, float y)
    {
        if (canMove && !isAttacking && !isDodging) 
        { 
            lastDodge = Time.time;
            dodgeX = x;
            dodgeY = y;
            anim.SetTrigger("Dodge");
            stamina -= 20;
            staminaBar.SetValue(stamina);
            AudioManager.instance.Play("player_dodge");
        }
    }

    protected override void ReceiveDamage(Damage dmg)
    {
        base.ReceiveDamage(dmg);
        healthBar.SetValue(hitpoint);
        healthBar.SetText(hitpoint, maxhitpoint);
        anim.SetTrigger("Hurt");
    }

    private void StaminaRegen()
    {
        if (staminaRegenTimer >= staminaRegenCooldown)
        {
            if (stamina < maxStamina)
            {
                stamina += 2 + Mathf.CeilToInt(playerStats.condition) / 2 - 1;
                if (stamina > maxStamina) stamina = maxStamina;
                staminaBar.SetValue(stamina);
                staminaBar.SetText(stamina, maxStamina);
            }
            staminaRegenTimer = 0;
        }
        staminaRegenTimer += Time.deltaTime;
    }
    public void LevelUp()
    {  
        GameManager.instance.ShowText("Level up", 15, Color.yellow, new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z), Vector3.up * 25, 0.5f);
        while (GameManager.instance.experience >= GameManager.instance.xpTable[GameManager.instance.playerLevel - 1])
        {
            GameManager.instance.availablePoints++;
            GameManager.instance.playerLevel++;
        }
        xpBar.SetXpBar();
    }

    protected override void Death()
    {
        alive = false;
        canAttack = false;
        canMove = false;
        anim.SetTrigger("Death");
    }
}
