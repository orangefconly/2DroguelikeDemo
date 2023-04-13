using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{
    public float moveTime = 0.2f;
    public LayerMask blockLayer;
    public SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2D;
    private Rigidbody2D rbody2D;
    private float inverseMoveTime;   
    public bool isMoving;
    

    protected virtual void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        rbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        inverseMoveTime = 1f / moveTime;

    }

    protected bool Move(int _xDir, int _yDir , out RaycastHit2D _hit)
    {
        Vector2 start = transform.position;

        Vector2 end = start + new Vector2(_xDir, _yDir);

        boxCollider2D.enabled = false;

        _hit = Physics2D.Linecast(start, end, blockLayer);

        boxCollider2D.enabled = true;

        if(_hit.transform == null && !isMoving)
        {
            StartCoroutine(SmoothMovement(end));

            return true;
        }

        return false;
    }

    protected IEnumerator SmoothMovement(Vector3 end)
    {
        isMoving = true;
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPostion = Vector3.MoveTowards(rbody2D.position, end, inverseMoveTime * Time.deltaTime);

            rbody2D.MovePosition(newPostion);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }
        rbody2D.MovePosition(end);
        isMoving = false;

    }
    protected virtual void AttemptMove<T>(int xDir, int yDir)
            where T : Component
    {
        
        RaycastHit2D hit;

        
        bool canMove = Move(xDir, yDir, out hit);

        
        if (hit.transform == null)
            
            return;

        
        T hitComponent = hit.transform.GetComponent<T>();

        
        if (!canMove && hitComponent != null)

           
            OnCantMove(hitComponent);
    }


    protected abstract void OnCantMove<T>(T component)
        where T : Component;

 
}
