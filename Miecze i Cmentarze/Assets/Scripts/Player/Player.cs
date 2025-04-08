using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
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

    private float x;
    private float y;

    /*private readonly string swing_1 = "PlayerSwing_1";
    private readonly string swing_2 = "PlayerSwing_3";
    private readonly string swing_3 = "PlayerSwing_2";
    private readonly string hurt = "PlayerHurt";
    private readonly string dodge = "PlayerDodge";*/

    public float playerSpeed = 2;
    public float currentSpeed;

    private int swingCount = 0;
    private float lastSwing;
    private float swingResetTimer = 0.75f;
    private float dodgeX;
    private float dodgeY;
    private float dodgeCooldown = 0.3f;
    private float lastDodge;

    public PlayerBar healthBar;
    public PlayerBar staminaBar;
    public PlayerBar xpBar;
    public PlayerStats playerStats;

    private float hpRegenTimer;
    private float hpRegenCooldown = 5f;

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
        gameManager.player = this;
        gameManager.floatingTextManager = FindObjectOfType<FloatingTextManager>();
        inventory.items.Clear();
        canMove = true;
        canAttack = true;
        anim = GetComponent<Animator>();
        anim.SetFloat("AttackSpeed", 1 + playerStats.bonusAttackSpeed / 100);
        if (saveManager.isLoading == false)
        {
            maxhitpoint = 60 + (playerStats.vitality * 40);
            hitpoint = maxhitpoint;
            healthBar.SetAllBars("hp");
            staminaBar.SetAllBars("stamina");
        }
        else saveManager.Load();
        saveManager.isLoading = false;
        hpRegenTimer = 0;
        staminaRegenTimer = 0;
        xpBar.SetXpBar();
        playerStats.UpdateStats();

        CameraMotor.instance.lookAt = transform;

        currentSpeed = playerSpeed;

        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        HandleMovementInput();

        /*x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");

        if (isAttacking || !canMove) 
        {
            x = 0;
            y = 0;
        }*/

        CheckPlayerAnimations();

        if (!anim.GetCurrentAnimatorStateInfo(0).IsTag("PlayerRun") || pauseMenu.gameIsPaused)
        {
            audioManager.Stop("run_grass");
            isRunning = false;
        }

        if (!pauseMenu.gameIsPaused)
        {
            if (x != 0 || y != 0 && !isAttacking && !isDodging)
            {
                anim.SetBool("Run", true);
                if (!isRunning && anim.GetCurrentAnimatorStateInfo(0).IsTag("PlayerRun") && pauseMenu.gameIsPaused)
                {
                    audioManager.Play("run_grass");
                    isRunning = true;
                }
            }
            else if ((x == 0 && y == 0) || isAttacking || isDodging)
            {
                anim.SetBool("Run", false);
                if (isRunning)
                {
                    audioManager.Stop("run_grass");
                    isRunning = false;
                }
            }

            if (Time.time - lastSwing > swingResetTimer) swingCount = 0;

            HandleCombatInput();
        }

        HpRegen();
        StaminaRegen();

        currentSpeed = CalculateSpeed();

        if (isDodging) UpdateMotor(new Vector3(dodgeX, dodgeY, 0), currentSpeed * 2);
        else UpdateMotor(new Vector3(x, y, 0), currentSpeed);

        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("PlayerDead"))
        {
            UI.instance.deathScreen.SetActive(true);
        }
    }
    private void CheckPlayerAnimations()
    {
        AnimatorStateInfo currentAnimationState = anim.GetCurrentAnimatorStateInfo(0);

        var movementAnimationTags = gameManager.enums.EnumToList<AnimationTags>(4);

        /*foreach(var tag in  movementAnimationTags)
        {
            if (currentAnimationState.tagHash == gameManager.enums.EnumStringToHash(tag.StringValue()))
            {
                isAttacking = true;
                isDodging = false;
                return;
            }
            isAttacking = false;
        }*/

        if (Array.Exists(movementAnimationTags.ToArray(), t => gameManager.enums.EnumStringToHash(t.StringValue()) == currentAnimationState.tagHash)) 
        {
            isAttacking = true;
            isDodging = false;
            return;
        }
        else isAttacking = false;

        /*if (anim.GetCurrentAnimatorStateInfo(0).IsTag(swing_1) || anim.GetCurrentAnimatorStateInfo(0).IsTag(hurt)
            || anim.GetCurrentAnimatorStateInfo(0).IsTag(swing_2) || anim.GetCurrentAnimatorStateInfo(0).IsTag(swing_3))
        {
            isAttacking = true;
        }
        else isAttacking = false;*/

        /*if (anim.GetCurrentAnimatorStateInfo(0).IsTag(dodge)) isDodging = true;
        else isDodging = false;*/

        if (currentAnimationState.tagHash == gameManager.enums.EnumStringToHash(AnimationTags.PlayerDodge.StringValue())) isDodging = true;
        else isDodging = false;
    }
    private void HandleMovementInput()
    {
        x = 0;
        y = 0;

        if (!isAttacking && canMove) 
        {
            if (inputHandler.CheckKey(KeyActions.Up)) y += 1;
            if (inputHandler.CheckKey(KeyActions.Down)) y -= 1;
            if (inputHandler.CheckKey(KeyActions.Right)) x += 1;
            if (inputHandler.CheckKey(KeyActions.Left)) x -= 1;
        }
    }

    private void HandleCombatInput()
    {
        if (inputHandler.CheckKey(KeyActions.Attack))
        {
            Swing();
        }

        if (inputHandler.CheckKey(KeyActions.Dodge))
        {
            if (Time.time - lastDodge > dodgeCooldown && ((stamina >= 20 && !playerStats.freeDodge) || playerStats.freeDodge))
            {
                Dodge(x, y);
            }
        }
    }

    protected override float CalculateSpeed()
    {
        if (y != 0)
        {
            if (x != 0) return playerSpeed - (playerSpeed * 0.125f);
            else return playerSpeed - (playerSpeed * 0.25f);
        }
        else return playerSpeed;
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
                audioManager.Play("player_slash");
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
            if (!playerStats.freeDodge) stamina -= 20;
            staminaBar.SetValue(stamina);
            audioManager.Play("player_dodge");
        }
    }

    protected override void ReceiveDamage(Damage dmg)
    {
        dmg.damageReduction = gameManager.player.playerStats.damageReduction;
        base.ReceiveDamage(dmg);
        healthBar.SetValue(hitpoint);
        healthBar.SetText(hitpoint, maxhitpoint);
        anim.SetTrigger("Hurt");
    }

    private void HpRegen()
    {
        if (hpRegenTimer >= hpRegenCooldown)
        {
            if (hitpoint < maxhitpoint)
            {
                hitpoint += Mathf.CeilToInt(playerStats.bonusHpRegen);
                if (hitpoint > maxhitpoint) hitpoint = maxhitpoint;
                healthBar.SetValue(hitpoint);
                healthBar.SetText(hitpoint, maxhitpoint);
            }
            hpRegenTimer = 0;
        }
        hpRegenTimer += Time.deltaTime;
    }

    private void StaminaRegen()
    {
        if (staminaRegenTimer >= staminaRegenCooldown)
        {
            if (stamina < maxStamina)
            {
                stamina += 2 + Mathf.CeilToInt(2 * (playerStats.bonusStaminaRegen / 100));
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
        gameManager.ShowText("Level up", 15, Color.yellow, new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z), Vector3.up * 25, 0.5f, true);
        while (gameManager.experience >= gameManager.xpTable[gameManager.playerLevel - 1])
        {
            gameManager.availablePoints++;
            gameManager.playerLevel++;
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
