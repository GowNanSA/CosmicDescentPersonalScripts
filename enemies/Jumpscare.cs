using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Jumpscare : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //public GameObject JumpScareImage;
    public AudioSource AudioSource; 
    // Update is called once per frame

    private void Start()
    {
        //JumpScareImage.SetActive(false); // false in beginning
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //JumpScareImage.SetActive(true);
            //StartCoroutine(DisablePicture());
            AudioSource.Play();
            //JumpScareImage?.SetActive(false);


        }

    }

    IEnumerator DisablePicture() {
        yield return new WaitForSeconds(2); 
        //JumpScareImage.SetActive(false);
    }
}