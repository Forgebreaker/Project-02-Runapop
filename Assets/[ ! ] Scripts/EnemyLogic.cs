using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    [SerializeField] private bool EagleMonster;
    [SerializeField] private bool FrogMonster;
    [SerializeField] private bool FoxMonster;
    private float MoveSpeed;
    private Animator _animator;
    private Rigidbody2D _enemyRB;

    private void Start()
    {
        MonsterType();
    }

    private void Update()
    {
        MoveForward();
    }
    private void MoveForward()
    {
        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        if (Player != null)
        {
            MoveSpeed = Player.GetComponent<PlayerController>().MoveSpeed * 0.8f;
        }
        else
        {
            MoveSpeed = 5f;
        }

        this.gameObject.transform.Translate(Vector3.right * MoveSpeed * Time.deltaTime, Space.Self);

        if (this.gameObject.transform.position.x <= -18f || this.gameObject.transform.position.x >= 18f) 
        { 
            Destroy(this.gameObject);
        }
    }

    private void MonsterType() 
    {
        _enemyRB = this.gameObject.GetComponent<Rigidbody2D>();
        _animator = this.gameObject.GetComponentInChildren<Animator>();
        _enemyRB.constraints = RigidbodyConstraints2D.FreezeRotation;


        if (_animator != null)
        {
            _animator.SetBool("Eagle Monster", EagleMonster);
            _animator.SetBool("Frog Monster", FrogMonster);
            _animator.SetBool("Fox Monster", FoxMonster);
        }

        if (EagleMonster) 
        {
            _enemyRB.gravityScale = 0;
        }
    }
}
