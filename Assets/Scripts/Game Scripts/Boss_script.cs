using UnityEngine;
using System.Collections;

public class Boss_script : MonoBehaviour
{
    // Bullets
    [SerializeField] GameObject bullet1;
    [SerializeField] GameObject bullet0;

    // Attaque #1
    [SerializeField] float timeOfAttack = 5f;
    [SerializeField] float timeBetweenAttack = 0.3f;
    [SerializeField] float cooldown = 5f;
    [SerializeField] float angleBurst = 80;
    [SerializeField] int nbBulletBurst = 20;

    // Attaque #2
    [SerializeField] float cooldown2 = 10f;
    [SerializeField] float timeBetweenAttack2 = 0.1f;
    [SerializeField] float timeOfAttack2 = 5f;
    [SerializeField] float angleEntreBullet2 = 5;

    //Firewalls
    [SerializeField] float tempsFirewallAvantAttaque = 1.5f;
    [SerializeField] GameObject firewall;
    Vector3 posFireWallGauche = new Vector3(-4, -4.4f, 0);
    Vector3 posFireWallMilieu = new Vector3(0, -4.4f, 0);
    Vector3 posFireWallDroite = new Vector3(4, -4.4f, 0);

    // Cores
    [SerializeField] GameObject cores;
    Vector3 posCoreDroite = new Vector3(-2,2,0);
    Vector3 posCoreGauche = new Vector3(6,2,0);

    // Vie
    [SerializeField] float maxVie = 500;
    float vie;


    gameManager gameManager;
    Rigidbody2D rb;
    bool active = true;
    bool attackChange = true;
    public bool apparaitreBoss = true;
    [SerializeField] int nbAttaqueAvantCore = 4;
    [SerializeField] float tempsAvantPremiereAttaque = 1.5f;


    //ui
    [SerializeField] int scoreOnHit = 2;
    [SerializeField] int scoreOnDeath = 100;

    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vie = maxVie;
        rb = GetComponent<Rigidbody2D>();
        gameManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<gameManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(apparaitreBoss)
        {
            StartCoroutine(changeAttack());
            apparaitreBoss=false;
        }
    }
    // Permet au boss de prendre des degats et lorqu'il est mort faire apparaitre l'ecran de victoire
    public void takeDamage(float damage)
    {
        gameManager.UpdateScore(scoreOnHit);
        vie -= damage;
        gameManager.UpdateHealthBar(vie / maxVie);
        if (vie <= 0)
        {
            gameManager.UpdateScore(scoreOnDeath);
            gameManager.gameOver(true);
            Destroy(gameObject);
        }
    }
    // change les attaque dependament si la coroutine est acitve et si le compteur est moins que nbAttaqueAvantCore, puis fait appelle a la fonction pour apparaitre les cores
    IEnumerator changeAttack()
    {
        int compteur = 0;
        yield return new WaitForSeconds(tempsAvantPremiereAttaque);
        while (active && compteur < nbAttaqueAvantCore)
        {
            attackChange = !attackChange;

            if (!attackChange)
            {
                ApparitionFirewall();
                yield return new WaitForSeconds(tempsFirewallAvantAttaque);
                StartCoroutine(BoomAttack());
                yield return new WaitForSeconds(cooldown);
            }
            else
            {
                StartCoroutine(SprayAndPrayAttack());
                yield return new WaitForSeconds(cooldown2);
            }
            compteur++;
        }
        ApparitionCore();
    }
    // Retire le sprite renderer et le box collider 2d pour le rendre boss "desactive"
    private void ApparitionCore()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        Instantiate(cores, posCoreDroite, Quaternion.identity);
        Instantiate(cores, posCoreGauche, Quaternion.identity);
    }
    // random 0, 1, 2 pour faire apparaitre la safe zone (firewall)
    private void ApparitionFirewall()
    {
        float choix = Random.Range(0, 3);
        switch (choix)
        {
            case 0:
                Instantiate(firewall, posFireWallGauche, Quaternion.identity);
                break;
            case 1:
                Instantiate(firewall, posFireWallMilieu, Quaternion.identity);
                break;
            case 2:
                Instantiate(firewall, posFireWallDroite, Quaternion.identity);
                break;
        }
    }

    //cette fonction permet de faire spawner des balles en burst dans un angle précis. le core attaque x fois ou x est égale au temps
    //entre le changement d'attaque divisé par le temps entre les attaques. 
    //Precondition: le nombre de bullets par burst doit être impaire et plus grand que 1
    IEnumerator BoomAttack()
    {
        int nbAttack = 0;
        float angleEntreBullet = angleBurst / nbBulletBurst;

        while (nbAttack < timeOfAttack / timeBetweenAttack)
        {
            for (int i = 0; i < nbBulletBurst; i++)
            {
                if (Random.Range(0, 2) == 0)
                    Instantiate(bullet1, transform.position, Quaternion.Euler(0, 0, 90 - ((nbBulletBurst - 1) / 2 * angleEntreBullet) + (i * angleEntreBullet)));
                else
                    Instantiate(bullet0, transform.position, Quaternion.Euler(0, 0, 90 - ((nbBulletBurst - 1) / 2 * angleEntreBullet) + (i * angleEntreBullet)));
            }

            yield return new WaitForSeconds(timeBetweenAttack);
            nbAttack++;
        }
    }

    IEnumerator SprayAndPrayAttack()
    {
        int nbAttack = 0;
        float derniereAngle = 0;
        float angleEntreBullet = angleEntreBullet2;

        while (nbAttack < timeOfAttack2 / timeBetweenAttack2)
        {
            // recoit 0 ou 1 puis fait apparaitre l'la bullet aproprier en gardant en memoire l'angle de la derniere bullet +  angleEntreBullet
            if (Random.Range(0, 2) == 0)
                Instantiate(bullet1, transform.position, Quaternion.Euler(0, 0, derniereAngle + angleEntreBullet));
            else
                Instantiate(bullet0, transform.position, Quaternion.Euler(0, 0, derniereAngle + angleEntreBullet));
            derniereAngle += angleEntreBullet;
            yield return new WaitForSeconds(timeBetweenAttack2);
            nbAttack++;
        }
    }
}

