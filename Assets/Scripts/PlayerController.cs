using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public float speed = 5;
    public float rocketSpeed = 5;
    public Transform centerOfMass;
    public Transform rearWheel;
    public Transform frontWheel;
    public Transform firePoint;
    public AudioClip collectCoinClip;
    public Text coinCountLabel;
    public bool playerMovmentEnabled = true;

    private Rigidbody2D rb2d;
    private WheelJoint2D[] wheelJoints;
    private JointMotor2D motorFront;
    private JointMotor2D motorRear;
    private int coinCount = 0;
    private PlayerHealthScript playerHealth;
    private const float rocketConstant = 0.1f;


    float dir = 0f; //horizontal movement keyboard input
                    //float torqueDir = 0f; //input for rotation of the car
    float slope = 0; //angle in which the car is at wrt the ground
    float maxFwdSpeed = -2500; //max fwd speed which the car can move at
    float maxBwdSpeed = 2000f; //max bwd speed
    float accelerationRate = 2000; //the rate at which the car accelerates
    float decelerationRate = -300; //the rate at which car decelerates
    float brakeSpeed = 1000f; //how soon the car stops on braking
    float gravity = 9.81f; //acceleration due to gravity

    private Vector3 facingDirection
    {
        get
        {
            return (firePoint.position - transform.position)
            .normalized;
        }
    }

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.centerOfMass = centerOfMass.transform.localPosition;
        playerHealth = GetComponent<PlayerHealthScript>();
        wheelJoints = gameObject.GetComponents<WheelJoint2D>();
        motorRear = wheelJoints[0].motor;
        motorFront = wheelJoints[1].motor;

        if(AchievementManager.instance.LoadAchievementValue(AchievementManager.Achievement.AchievementType.RocketBoost) != -1)
        {
            rocketSpeed *= rocketConstant * AchievementManager.instance.LoadAchievementValue(AchievementManager.Achievement.AchievementType.RocketBoost);
        }
    }

    void FixedUpdate()
    {
        if (playerMovmentEnabled)
        {
            float torqueDir = Input.GetAxis("Horizontal");

            if (torqueDir != 0)
            {
                rb2d.AddTorque(3 * Mathf.PI * torqueDir, ForceMode2D.Force);
            }
            else
            {
                rb2d.AddTorque(0);
            }

            slope = transform.localEulerAngles.z;

            if (slope >= 180)
                slope = slope - 360;

            dir = Input.GetAxis("Vertical");

            if (dir != 0)
            {
                motorFront.motorSpeed = Mathf.Clamp(motorFront.motorSpeed - (dir * accelerationRate - gravity * Mathf.Sin((slope * Mathf.PI) / 180) * 80) * Time.deltaTime, maxFwdSpeed, maxBwdSpeed);
                motorRear.motorSpeed = Mathf.Clamp(motorRear.motorSpeed - (dir * accelerationRate - gravity * Mathf.Sin((slope * Mathf.PI) / 180) * 80) * Time.deltaTime, maxFwdSpeed, maxBwdSpeed);
            }
            if ((dir == 0 && motorFront.motorSpeed < 0) || (dir == 0 && motorFront.motorSpeed == 0 && slope < 0))
            {
                motorFront.motorSpeed = Mathf.Clamp(motorFront.motorSpeed - (decelerationRate - gravity * Mathf.Sin((slope * Mathf.PI) / 180) * 80) * Time.deltaTime, maxFwdSpeed, 0);
                motorRear.motorSpeed = Mathf.Clamp(motorRear.motorSpeed - (decelerationRate - gravity * Mathf.Sin((slope * Mathf.PI) / 180) * 80) * Time.deltaTime, maxFwdSpeed, 0);
            }
            else if ((dir == 0 && motorFront.motorSpeed > 0) || (dir == 0 && motorFront.motorSpeed == 0 && slope > 0))
            {
                motorFront.motorSpeed = Mathf.Clamp(motorFront.motorSpeed - (-decelerationRate - gravity * Mathf.Sin((slope * Mathf.PI) / 180) * 80) * Time.deltaTime, 0, maxBwdSpeed);
                motorRear.motorSpeed = Mathf.Clamp(motorRear.motorSpeed - (-decelerationRate - gravity * Mathf.Sin((slope * Mathf.PI) / 180) * 80) * Time.deltaTime, 0, maxBwdSpeed);
            }
            if (Input.GetKey(KeyCode.Space) && motorFront.motorSpeed > 0)
            {
                motorFront.motorSpeed = Mathf.Clamp(motorFront.motorSpeed - brakeSpeed * Time.deltaTime, 0, maxBwdSpeed);
                motorRear.motorSpeed = Mathf.Clamp(motorRear.motorSpeed - brakeSpeed * Time.deltaTime, 0, maxBwdSpeed);
            }
            else if (Input.GetKey(KeyCode.Space) && motorFront.motorSpeed < 0)
            {
                motorFront.motorSpeed = Mathf.Clamp(motorFront.motorSpeed + brakeSpeed * Time.deltaTime, maxFwdSpeed, 0);
                motorRear.motorSpeed = Mathf.Clamp(motorRear.motorSpeed + brakeSpeed * Time.deltaTime, maxFwdSpeed, 0);
            }

            if (dir == 0)
            {
                motorFront.motorSpeed = Mathf.Clamp(motorFront.motorSpeed + brakeSpeed * Time.deltaTime, maxFwdSpeed, 0);
                motorRear.motorSpeed = Mathf.Clamp(motorRear.motorSpeed + brakeSpeed * Time.deltaTime, maxFwdSpeed, 0);
            }

            if (Input.GetKeyDown(ObjectKeys.PLAYER_FIRE_KEY))
            {
                Fire();
            }

        }

        wheelJoints[0].motor = motorRear;
        wheelJoints[1].motor = motorFront;
    }

    private void Fire()
    {
        GameObject obj = RocketPoolingScript.current.GetPooledObject();
        obj.transform.position = firePoint.position;
        obj.transform.rotation = transform.rotation * Quaternion.Euler(0, 0, -90);
        obj.SetActive(true);
        Rigidbody2D laserClone = obj.GetComponent<Rigidbody2D>();

        laserClone.velocity = facingDirection * rocketSpeed * 1.5f;

        laserClone.AddForce(laserClone.gameObject.transform.up * rocketSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(ObjectTags.PICKUP) && collision.gameObject.activeSelf)
        {
            AudioSource.PlayClipAtPoint(collectCoinClip, transform.position);
            coinCount++;
            GameScript.gameManger.coinCount = coinCount;
            coinCountLabel.text = coinCount.ToString();
            collision.gameObject.SetActive(false);
        }

        if (isGameObjectInverted(collision))
        {
            playerHealth.TakeDamage(100);
        }

    }

    bool isGameObjectInverted(Collider2D collision)
    {
        return (collision.gameObject.CompareTag(ObjectTags.TAIL) && (gameObject.transform.eulerAngles.z >= 140 && gameObject.transform.eulerAngles.z <= 220));
    }

    public void EnableMovment(bool enabled)
    {
        this.playerMovmentEnabled = enabled;
    }

    private bool haveAchievement()
    {
        if (AchievementManager.instance.LoadAchievementValue(AchievementManager.Achievement.AchievementType.RocketBoost) != -1)
            return true;

        return false;
    }

}
