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

    protected override void Start()
    {
        base.Start();
        canMove = true;
        canAttack = true;
        anim = GetComponent<Animator>();
        anim.SetFloat("AttackSpeed", 1 + (playerStats.condition / 10) - 0.1f);
        maxhitpoint = 60 + (playerStats.vitality * 40);
        hitpoint = maxhitpoint;
        healthBar.setMaxValue(maxhitpoint);
        healthBar.setValue(hitpoint);
        healthBar.setText(hitpoint, maxhitpoint);
        staminaBar.setMaxValue(maxStamina);
        staminaBar.setValue(stamina);
        staminaBar.setText(stamina, maxStamina);
        staminaRegenTimer = 0;
        xpBar.setXpBar();

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

        if (x != 0 || y != 0 && !isAttacking && !isDodging)
        {
            anim.SetBool("Run", true);
        }
        else if ((x == 0 && y == 0) || isAttacking || isDodging)
        {
            anim.SetBool("Run", false);
        }

        if (Time.time - lastSwing > swingReset) swingCount = 0;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Swing();
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (Time.time - lastDodge > dodgeCooldown && stamina >= 20)
            {
                Dodge(x, y);
            }
        }

        StaminaRegen();

        if (isDodging) UpdateMotor(new Vector3(dodgeX, dodgeY, 0), playerYSpeed * 2, playerXSpeed * 2);
        else UpdateMotor(new Vector3(x, y, 0), playerYSpeed, playerXSpeed);
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
                staminaBar.setValue(stamina);
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
            staminaBar.setValue(stamina);
        }
    }

    protected override void ReceiveDamage(Damage dmg)
    {
        base.ReceiveDamage(dmg);
        healthBar.setValue(hitpoint);
        healthBar.setText(hitpoint, maxhitpoint);
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
                staminaBar.setValue(stamina);
                staminaBar.setText(stamina, maxStamina);
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
        xpBar.setXpBar();
    }
}
