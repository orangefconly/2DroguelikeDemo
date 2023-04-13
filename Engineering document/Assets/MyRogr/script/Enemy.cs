using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject
{
    public int damage;
    private Animator animator;
    private Transform target;
    private bool skipmove;

    // Start is called before the first frame update
    protected override void Start()
    {
        GameManager.Instance.AddEnemyToList(this);
        target = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        base.Start();

    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        if(skipmove)
        {
            skipmove = false;
            return;
        }
        base.AttemptMove<T>(xDir, yDir);
        skipmove = true;
    }
    public void MoveEnemy()
    {
        int xDir = 0;
        int yDir = 0;
        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
            yDir = target.position.y > transform.position.y ? 1 : -1;
        else
            xDir = target.position.x > transform.position.x ? 1 : -1;
        AttemptMove<Player>(xDir, yDir);

    }

    protected override void OnCantMove<T>(T component)
    {
        Player hitPlayer = component as Player;
        animator.SetTrigger("enemyAttack");
        hitPlayer.LoseFood(damage);
    }
}
