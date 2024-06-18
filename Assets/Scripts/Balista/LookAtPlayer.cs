using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.UIElements.ToolbarMenu;

public class LookAtPlayer : MonoBehaviour
{
    [SerializeField] private GameObject ballistaPrefab;
    [SerializeField] private PlayerController player;    
    [SerializeField] private Transform pBow;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float cdArrow;
    [SerializeField] private float shotForce;
    
    private float count;
    
    void Update()
    {
        count += Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        
        LookAtP();
        Shoot();        
    }

    private void LookAtP()
    {        
        Quaternion newRotation = Quaternion.LookRotation(player.transform.position - transform.position);
        Quaternion currentRotation = transform.rotation;
        Quaternion finalRotation = Quaternion.Lerp(currentRotation, newRotation, Time.deltaTime * rotationSpeed);
        transform.rotation = finalRotation;
    }

    private void Shoot()
    {
        if (count > cdArrow  && (player.currentHealth > 0))
        {
            GameObject gameObject = Instantiate(ballistaPrefab, pBow.transform.position, pBow.transform.rotation);            

            Destroy(gameObject, 2.5f);
            count = 0;
        }
    }

}
