using System;
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
    public string bossToActivateName;

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
        playerNearby = IsPlayerNearby();

        if (playerNearby && !textShown)
        {
            ShowInteractionText();
        }
        if (textShown && !playerNearby)
        {
            HideInteractionText();
        }

        if (Input.GetKeyDown(KeyCode.E) && playerNearby && active)
        {
            OnActivation();
        }
    }
    protected void OnActivation()
    {
        AudioManager.instance.StopMusic("theme_" + SceneManager.GetActiveScene().buildIndex);
        AudioManager.instance.Play("teleport");
        SaveManager.instance.SaveBosses();
        SaveManager.instance.SaveSpawners();
        //SceneManager.LoadSceneAsync(sceneIndex);
        LevelLoader.instance.LoadLevel(sceneIndex);
        GameManager.instance.player.transform.position = new Vector3(playerX, playerY, 0);
    }

    protected void ShowInteractionText()
    {
        if (active) interactionTextManager.Show("[E] Wejdz", 7, Color.yellow, new Vector3(transform.position.x, transform.position.y, transform.position.z), interactionTextOffset);
        else interactionTextManager.Show("Pokonaj " + bossToActivateName + "\n\naby otworzyc portal", 7, Color.red, new Vector3(transform.position.x, transform.position.y, transform.position.z), interactionTextOffset);
        textShown = true;
    }
    protected void HideInteractionText()
    {
        textShown = false;
        interactionTextManager.Hide();
    }

    public void CheckPortal()
    {
        var unlocked = Array.Exists(SaveManager.instance.tempBosses.ToArray(), boss => boss.bossName == bossToActivate);

        if (unlocked)
        {
            anim.enabled = true;
            active = true;
            return;
        }

        anim.enabled = false;
        spriteRenderer.sprite = unActivePortal;
    }
}
