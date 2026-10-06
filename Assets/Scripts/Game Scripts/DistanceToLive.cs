using UnityEngine;

public class DistanceToLive : MonoBehaviour
{
    [SerializeField] float distance = 9;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //détruit le gameObject apres une certaine distance du 0,0,0
        if(Vector3.Distance(transform.position, new Vector3(0, 0, 0)) > distance)
        {
            Destroy(gameObject);
        }
    }
}
