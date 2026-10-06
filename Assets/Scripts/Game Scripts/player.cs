using System.Collections;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Threading;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;




public class player : MonoBehaviour
{

    Vector2 moveInput;
    [SerializeField] float speed = 3;
    [SerializeField] GameObject bullet;
    [SerializeField] float rateOfFire = 10;
    gameManager gameManager;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;

    float vie = 3;

    //input fire
    float fireInput;
    float fireDelay;
    //joueur touché
    float tempsEntreClignotant = 0.25f;
    bool estInvincible = false;
    // bullets par vie
    [SerializeField] float bulletVie2 = 0.1f;
    float angleBulletVie3 = 15f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); 
        gameManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<gameManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateMovement();
        UpdateFire(); 
    }

    //permet de déplacer le personnage 
    void UpdateMovement()
    {
        //vecteur de déplacement du personnage qui le fait bouger sur l'axe des x et y
        Vector3 direction = rb.transform.right * moveInput.x + rb.transform.up * moveInput.y;

        if (direction.magnitude > 0)
            rb.MovePosition(transform.position + direction.normalized * speed * Time.fixedDeltaTime);
    }


    //permet de tirer 3, 2 ou une balle selon les points de vie et respecte le fire rate
    void UpdateFire()
    {

        switch (vie)
        {
            case 3:
                //fait spawner trois balles dans un cone
                if (fireInput > 0 && fireDelay <= 0)
                {
                    Instantiate(bullet, transform.position, Quaternion.identity);
                    Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, angleBulletVie3));
                    Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, -angleBulletVie3));
                    fireDelay = 1 / rateOfFire;
                }
                else
                    fireDelay -= Time.deltaTime;
                break;
            case 2:
                //fait spawner 2 balles face au joueur
                if (fireInput > 0 && fireDelay <= 0)
                {
                    Instantiate(bullet, new Vector2(transform.position.x + bulletVie2, transform.position.y), Quaternion.identity);
                    Instantiate(bullet, new Vector2(transform.position.x - bulletVie2, transform.position.y), Quaternion.identity);
                    fireDelay = 1 / rateOfFire;
                }
                else
                    fireDelay -= Time.deltaTime;
                break;
            case 1:
                //fait spawner 1 balle face au joueur
                if (fireInput > 0 && fireDelay <= 0)
                {
                    Instantiate(bullet, transform.position, Quaternion.identity);
                    fireDelay = 1 / rateOfFire;
                }
                else
                    fireDelay -= Time.deltaTime;
                break;
        }


    }
    public void InputMove(CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void Inputfire(CallbackContext context)
    {
        fireInput = context.ReadValue<float>();
    }


    //fonction qui permet d'enlever de la vie au joueur. quand le joueur n'a plus de vie on déclanche un game over et détruit l'objet
    public void takeDamage(float damage)
    {   
        if (!estInvincible)
        {
            estInvincible = true;
            StartCoroutine(Clignotant());
            vie -= damage;
            if (vie <= 0)
            {
                gameManager.gameOver(false);
                Destroy(gameObject);
                
            }
        }
    }
    

    //fait clignoter le joueur en rouge et il devient invincible pour une courte durée
    IEnumerator Clignotant()
    {
        int compteur = 0;
        while(compteur < 2)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(tempsEntreClignotant);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(tempsEntreClignotant);
            compteur++;
        }
        estInvincible = false;
    }
   
}


