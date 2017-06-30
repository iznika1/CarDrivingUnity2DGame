using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineScript : MonoBehaviour {

    private AudioSource carSound;

    private const float lowPtich = 0.5f;
    private const float highPitch = 5f;

    //change the reductionFactor to 0.1f if you are using the rigidbody velocity as parameter to determine the pitch
    private const float reductionFactor = .001f;

    //Rigidbody2D carRigidbody;
    private float userInput;
    //wheeljoint2d reference
    WheelJoint2D wj;

    void Awake()
    {
        //get the Audio Source component attached to the car
        carSound = GetComponent<AudioSource>();
        //get the wheelJoint2D component attached to the car
        wj = GetComponent<WheelJoint2D>();
        //carRigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        //get the userInput
        userInput = Input.GetAxis("Vertical");
        //get the absolute value of jointSpeed
        float forwardSpeed = Mathf.Abs(wj.jointSpeed);
        //float forwardSpeed = transform.InverseTransformDirection(carRigidbody.velocity).x;
        //calculate the pitch factor which will be added to the audio source
        float pitchFactor = Mathf.Abs(forwardSpeed * reductionFactor * userInput);
        //clamp the calculated pitch factor between lowPitch and highPitch
        carSound.pitch = Mathf.Clamp(pitchFactor, lowPtich, highPitch);
    }
}
