using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Player : MovingObject
{
    public int wallDamage = 1;
    public int pointPerFood = 10;
    public int pointPerSoda = 20;
    public float restartLevelDelay = 1f;
    public TMP_Text foodText;


    private Animator animator;
    private int food;

    protected override void Start()
    {
        animator = GetComponent<Animator>();
        food = GameManager.Instance.playerFoodPoint;
        foodText.text = "Food: " + food;
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        foodText.text = "Food: " + food;
        if (!GameManager.Instance.playerTurn)
            return;
        
        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");
        if (horizontal < 0 && !isMoving)
            spriteRenderer.flipX = true;
        if (horizontal > 0 && !isMoving)
            spriteRenderer.flipX = false;
        if (horizontal != 0)
            vertical = 0;
        if (horizontal != 0||vertical != 0)
        {
            AttemptMove<WallObject>(horizontal, vertical);
        }
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        food--;        
        base.AttemptMove<T>(xDir, yDir);
        //RaycastHit2D Hit;
        CheckIfGameover();
        GameManager.Instance.playerTurn = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Exit" )
        {
            Invoke("ReStart",restartLevelDelay);
            enabled = false;
        }
        else if (collision.tag =="Food")
        {
            food += pointPerFood;
            collision.gameObject.SetActive(false);
        }
        else if (collision.tag == "Soda")
        {
            food += pointPerSoda;
            collision.gameObject.SetActive(false);
        }
    }

    protected override void OnCantMove<T>(T component)
    {
        WallObject hitWall = component as WallObject;
        hitWall.DamageWall(wallDamage);
        animator.SetTrigger("PlayerChop");
    }

    private void ReStart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
    private void CheckIfGameover()
    {
        if (food <= 0)
        {
            GameManager.Instance.playerFoodPoint = food;
            GameManager.Instance.GameOver();
        }
    }
    public void LoseFood (int loss)
    {
        animator.SetTrigger("PlayerHit");
        food -= loss;
        CheckIfGameover();
    }

}
