using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControleJogador : MonoBehaviour
{

    private Rigidbody2D rig;
    public float speed;
    public float jumpForce;
    private bool pulando = false;
    private Animator animator;
    public Transform cameraPerso;
    public float minimoCameraX;
    public float minimoCameraY;
    public float maximoCameraX;
    public float maximoCameraY;
    public Transform fundo;
    public GameObject fireball;
    public GameObject sons;
    public Text coins;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float camx = rig.transform.position.x + 3;
        if (camx < minimoCameraX)
        {
            camx = minimoCameraX;
        }
        if(camx > maximoCameraX)
        {
            camx = maximoCameraX;
        }

        float camy = rig.transform.position.y + 3;
        if (camy < minimoCameraY)
        {
            camy = minimoCameraY;
        }
        if (camy > maximoCameraY)
        {
            camy = maximoCameraY;
        }
        cameraPerso.position = new Vector3(camx, camy, -10);

        float fundox = (((camx - minimoCameraX)) / 1.5F) + 22;
        fundo.position = new Vector3(fundox, 1, 2F);


        float mov = Input.GetAxisRaw("Horizontal");
        if(mov == 1)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        } else if(mov == -1)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        if(pulando == false)
        {
            rig.velocity = new Vector2(mov * speed, rig.velocity.y);
            animator.SetFloat("Velocidade", Mathf.Abs(mov));
        }
        if (Input.GetKeyDown(KeyCode.Space) && pulando == false)
        {
            rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            pulando = true;
            animator.SetBool("Pulando", true);
            sons.GetComponents<AudioSource>()[3].Play();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            animator.SetBool("Abaixado", true);
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            animator.SetBool("Abaixado", false);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            float fx;
            float movFire;
            bool flipFire;
            if (GetComponent<SpriteRenderer>().flipX)
            {
                movFire = -3F;
                fx = rig.transform.position.x - 2;
                flipFire = false;
            }
            else
            {
                movFire = 3F;
                fx = rig.transform.position.x + 2;
                flipFire = true;
            }
            float fy = rig.transform.position.y + 0.5F;
            float fz = rig.transform.position.z;
            GameObject novo = Instantiate(fireball, new Vector3(fx, fy, fz), Quaternion.identity);
            novo.GetComponent<ControleFireball>().mov = movFire;
            novo.GetComponent<SpriteRenderer>().flipX = flipFire;
            sons.GetComponents<AudioSource>()[4].Play();
        }
}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Coin")
        {
            Destroy(collision.gameObject);
            int numero;
            int.TryParse(coins.text, out numero);
            numero++;
            coins.text = "" + numero;
            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        pulando = false;
        animator.SetBool("Pulando", false);
    }
}
