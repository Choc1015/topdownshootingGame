using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] int playerSpeed = 1;
    [SerializeField] GameObject shield;
    [SerializeField] GameObject panel;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        shield.SetActive(false);
        StartCoroutine(InputShield());
        StartCoroutine(InputShooter());
    }

    private void FixedUpdate()
    {
        playerMove();
        
    }

    private void Update()
    {

    }

    private IEnumerator InputShooter()
    {
        while (true)
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                GameObject bullet = ObjectPoolingManager.instance.GetBullet((int)BulletType.PLAYER);
                bullet.GetComponent<BulletMove>().SetPosition(transform.position);
                yield return new WaitForSeconds(0.2f);

            }
            else yield return new WaitUntil(() => Input.GetKey(KeyCode.LeftControl));
        }
    }

    IEnumerator InputShield()
    {
        while (true){
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                shield.SetActive(true);
                GameManager.Instance.isShield = true;
                yield return new WaitForSeconds(2f);
                shield.SetActive(false);
                GameManager.Instance.isShield = false;
                yield return new WaitForSeconds(1f);
            }
            else
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.LeftShift));
        }
      
    }

    private void playerMove()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector2 playerMovement = new Vector2(horizontalInput, verticalInput) * playerSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + playerMovement);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BulletToPlayer") )
        {
            if (GameManager.Instance.isShield == true && collision.gameObject.name == "FollowBullet(Clone)")
                return;
            Debug.Log("Á×À½");
            panel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            panel.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
