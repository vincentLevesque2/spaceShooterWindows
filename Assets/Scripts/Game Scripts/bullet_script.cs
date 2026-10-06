using TMPro;
using UnityEngine;

public class bullet_script : MonoBehaviour
{
    [SerializeField] float speed = 3;
    [SerializeField] float damage = 1;
    

    // Update is called once per frame
    void Update()
    {
       
        transform.Translate(transform.up * speed * Time.deltaTime);//va vers l'avant
    }


    //entre en collision et détruit l'objet. appelle un takeDamage si il peut
    private void OnTriggerEnter2D(Collider2D other)
    {
        //scripts de tout les objet qui peuvent prendre du dégat
        player player = other.gameObject.GetComponent<player>();
        core_script core = other.gameObject.GetComponent<core_script>();
        Boss_script boss = other.gameObject.GetComponent<Boss_script>();

        //appelle takeDamage si un des script n'est pas null
        if (core != null)
        {
            core.takeDamage(damage);
            
        }

        if (player != null)
        {
            player.takeDamage(damage);
        }

        if (boss != null)
        {
            boss.takeDamage(damage);
        }
        Destroy(gameObject);//detruit le gameObject
    }
}

