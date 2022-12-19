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
    private bool canAttack;
    private string swing_1 = "adventurer_swing_1";
    private string swing_2 = "adventurer_swing_2";
    private string swing_3 = "adventurer_swing_3";
    private string strongSwing_1 = "adventurer_strongswing_1";
    private string strongSwing_2 = "adventurer_strongswing_2";
    private string strongSwing_3 = "adventurer_strongswing_3";
    private string strongSwing_4 = "adventurer_strongswing_4";
    private string hurt = "adventurer_hurt";
    private string dodge = "adventurer_dodge";
    public float playerYSpeed = 1.5f;
    public float playerXSpeed = 2;
    private int swingCount = 0;
    private float lastSwing;
    private float swingReset = 0.75f;
    private float dodgeX;
    private float dodgeY;
    private float dodgeCooldown = 0.3f;
    private float lastDodge;
    private float strongSwingCount;
    private float lastStrongSwing;
    private float strongSwingReset = 1;

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
            || anim.GetCurrentAnimatorStateInfo(0).IsName(swing_2) || anim.GetCurrentAnimatorStateInfo(0).IsName(swing_3)
            || anim.GetCurrentAnimatorStateInfo(0).IsName(strongSwing_1) || anim.GetCurrentAnimatorStateInfo(0).IsName(strongSwing_2)
            || anim.GetCurrentAnimatorStateInfo(0).IsName(strongSwing_3) || anim.GetCurrentAnimatorStateInfo(0).IsName(strongSwing_4)) 
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

        if (Input.GetKeyDown(KeyCode.Q) && !isAttacking && !isDodging)
        {
            if (stamina >= 10) 
            {
                Swing();
            }
            
        }

        if (Time.time - lastStrongSwing > strongSwingReset) strongSwingCount = 0;

        if(Input.GetKeyDown(KeyCode.R) && !isAttacking && !isDodging)
        {
            StrongSwing();
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
        if (canAttack)
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

    private void StrongSwing()
    {
        isAttacking = true;
        anim.SetBool("Run", false);
        if (strongSwingCount == 0)
        {
            anim.SetTrigger("StrongSwing1");
            strongSwingCount++;
        }
        else if (strongSwingCount == 1)
        {
            anim.SetTrigger("StrongSwing2");
            strongSwingCount++;
        }
        else if (strongSwingCount == 2)
        {
            anim.SetTrigger("StrongSwing3");
            strongSwingCount++;
        }
        else if (strongSwingCount == 3) 
        {
            anim.SetTrigger("StrongSwing4");
            strongSwingCount = 0;
        }
        lastStrongSwing = Time.time;
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
