using UnityEngine;
using System.Collections;
using System.Data;

public class core_script : MonoBehaviour
{
    //movement
    [SerializeField] float speed = 3;
    [SerializeField] float timeBetweenMove = 2f;

    bool moveRight = true;
    Rigidbody2D rb;
    Vector3 direction;

    //vie
    [SerializeField] float vie = 100;

    //timing pour les attaques
    [SerializeField] float timeBetweenAttackChange = 5.0f;
    [SerializeField] float timeBetweenAttack = 0.3f;
    [SerializeField] float timeBeforeAttack = 1.5f;

    //bullets
    [SerializeField] GameObject bullet1;
    [SerializeField] GameObject bullet0;

    //burst
    [SerializeField] int nbBulletBurst = 5;
    [SerializeField] float angleBurst = 80;

    //points
    [SerializeField] int pointsOnDeath = 20;
    [SerializeField] int pointsOnHit = 2;

    gameManager gameManager;
    bool active = true;
    bool attackChange = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        gameManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<gameManager>();


        //start coroutines
        StartCoroutine(changeAttack());
        StartCoroutine(movementRoutine());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateMovement();
    }


    //prend du dégat le détrui quand sa vie tombe à 0 ou moin. Ajoute des points quand est touché et des points quand il est détruit
    public void takeDamage(float damage)
    {
        gameManager.UpdateScore(pointsOnHit);
        vie -= damage;
        if (vie <= 0)
        {
            gameManager.killCore();
            gameManager.UpdateScore(pointsOnDeath);
            Destroy(gameObject);
        }
    }

    //fait bouger dans une direction selon le bool moveRight
    void UpdateMovement()
    {
        if (moveRight)
        {
            direction = rb.transform.right;//vecteur de dplacement vers la droite
        }

        else
        {
            direction = -rb.transform.right;//vecteur de dplacement vers la gauche
        }
        rb.MovePosition(rb.position + new Vector2(direction.x, direction.y) * speed * Time.fixedDeltaTime);
    }


    //permet de changer le bool de direction du core apres un temps donné
    IEnumerator movementRoutine()
    {
        while (active)
        {
            moveRight = !moveRight;
            yield return new WaitForSeconds(timeBetweenMove);
            
        }
    }
    

    //permet de changer entre 2 type d'attaque a un interval donné
    IEnumerator changeAttack()
    {
        
        while (active)
        {
            
            yield return new WaitForSeconds(1.5f); //donne un petit délai avant que le core recommence à attaquer
            attackChange = !attackChange;

            if (!attackChange)
            {
                StartCoroutine(BurstAttack());
            }

            else
            {
                StartCoroutine(straightAttack());
            }
            //attend avant de changer
            yield return new WaitForSeconds(timeBetweenAttackChange);
        }
    }


    //cette fonction permet de faire spawner des balles devant le core.le core attaque x fois ou x est égale au temps
    //entre le changement d'attaque divisé par le temps entre les attaques. 
    IEnumerator straightAttack()
    {
        int nbAttack = 0;

        while(nbAttack < timeBetweenAttackChange/timeBetweenAttack)
        {
            //fait spawner une balle en dessou du core ( alterne entre des 2 prefab aléatoirement)
            if (Random.Range(0, 2) == 0)
                Instantiate(bullet1, transform.position, Quaternion.Euler(0,0,90));
            else
                Instantiate(bullet0, transform.position, Quaternion.Euler(0, 0, 90));

            yield return new WaitForSeconds(timeBetweenAttack);//temps entre bullets
            nbAttack++;
        }
    }



    //cette fonction permet de faire spawner des balles en burst dans un angle précis. le core attaque x fois ou x est égale au temps
    //entre le changement d'attaque divisé par le temps entre les attaques. 
    //Precondition: le nombre de bullets par burst doit être impaire et plus grand que 1
    IEnumerator BurstAttack()
    {
        int nbAttack = 0;
        float angleEntreBullet = angleBurst / nbBulletBurst; //angle entre chaque balle

        while (nbAttack < timeBetweenAttackChange / timeBetweenAttack)
        {
            for(int i = 0; i< nbBulletBurst; i++)
            {
                //fait spawner une balle a l'extremité du cone(angle) plus un angle de i * l'angle entre chaque bullets.
                //(alterne entre des 2 prefab aléatoirement)
                if (Random.Range(0, 2) == 0)
                    Instantiate(bullet1, transform.position, Quaternion.Euler(0, 0, 90 - (((nbBulletBurst - 1) / 2) * angleEntreBullet) + (i * angleEntreBullet)));
                else
                    Instantiate(bullet0, transform.position, Quaternion.Euler(0, 0, 90 - (((nbBulletBurst - 1) / 2) * angleEntreBullet) + (i * angleEntreBullet)));
            }
                
            yield return new WaitForSeconds(timeBetweenAttack);//temps entre burst
            nbAttack++;
        }
    }

}
