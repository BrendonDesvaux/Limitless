using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class fogLaunch : MonoBehaviour
{
    public float timePassed = 0.0f;
    private ParticleSystem ps;

    private void Start()
    {

        ps = GetComponent<ParticleSystem>();

    }
    void Update()
    {
        var main = ps.main;
        Time.timeScale = 1f;
        timePassed += Time.deltaTime;
        
        if (timePassed < 30f)
        {
            main.startSize = 0f;
        }
        if (timePassed > 30f)
        {
            main.startSize = 200f;
        }
        if( timePassed > 60F)
        {
            timePassed = 0f;
        }
    }
}
