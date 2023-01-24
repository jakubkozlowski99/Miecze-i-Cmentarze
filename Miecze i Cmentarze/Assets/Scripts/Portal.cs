using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : Collidable
{
    public InteractionTextManager interactionTextManager;

    public Sprite unActivePortal;

    public bool active;

    public string bossToActivate;

    public int sceneIndex;

    private bool playerNearby;
    private bool textShown;
    public float interactionTextOffset;

    public float playerX;
    public float playerY;

    public Animator anim;
    public SpriteRenderer spriteRenderer;

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        CheckPortal();
    }

    protected override void Update()
    {
        if (active)
        {
            playerNearby = IsPlayerNearby();

            if (playerNearby && !textShown && active)
            {
                ShowInteractionText();
            }
            if (textShown && !playerNearby)
            {
                HideInteractionText();
            }

            if (Input.GetKeyDown(KeyCode.E) && playerNearby)
            {
                OnActivation();
            }
        }

        
    }
    protected void OnActivation()
    {
        AudioManager.instance.StopMusic("theme_" + SceneManager.GetActiveScene().buildIndex);
        AudioManager.instance.Play("teleport");
        SaveManager.instance.SaveSpawners();
        //SceneManager.LoadSceneAsync(sceneIndex);
        LevelLoader.instance.LoadLevel(sceneIndex);
        GameManager.instance.player.transform.position = new Vector3(playerX, playerY, 0);
    }

    protected void ShowInteractionText()
    {
        interactionTextManager.Show("[E] Wejdz", 7, Color.yellow, new Vector3(transform.position.x, transform.position.y, transform.position.z), interactionTextOffset);
        textShown = true;
    }
    protected void HideInteractionText()
    {
        textShown = false;
        interactionTextManager.Hide();
    }

    public void CheckPortal()
    {
        foreach(BossData boss in SaveManager.instance.tempBosses)
        {
            if (boss.bossName == bossToActivate)
            {
                anim.enabled = true;
                active = true;
                return;
            }
        }
        anim.enabled = false;
        spriteRenderer.sprite = unActivePortal;
    }
}
