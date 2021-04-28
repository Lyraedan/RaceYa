using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    public Transform centerOfMass;
    [SerializeField] public AxleData[] axles;
    [Range(0f, 1f)] public float steerHelper = 0.5f;
    public float torque = 200f;
    public float reverseTorque = 250f;
    public float brakeTorque = 500f;
    public float minSteerAngle = 10f;
    public float maxSteerAngle = 30f;
    public float antiRollForce = 400f;
    public float downForceValue = 25f;
    public float turningDownForceValue = 100f;
    public float maxSpeed;
    public float maxReverseSpeed = -20f;
    public Transform skidTrail;
    public Text speed;
    public Text steerSetAngle;
    public float wheelBase;
    public float turnRadius;
    public float rearTrack;

    public NetworkedUser user;

    internal float speedKph;
    internal float speedMph;
    private float maxSteer;
    private float oldRotation;

    private Rigidbody rb;

    private Transform[,] skidTrails;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.transform.localPosition;
        SetupVehicle();
    }

    void SetupVehicle()
    {
        skidTrails = new Transform[2, 2];
        for (int i = 0; i < axles.Length; ++i)
        {
            axles[i].Setup(rb, antiRollForce, i, this);
        }
    }

    void FixedUpdate()
    {
        if (!user.started || user.finished) return;

        //Down force
        var setTorque = Input.GetKey(KeyCode.W) ? torque : 0f;

        //var setBrakeTorque = Input.GetKey(KeyCode.S) ? brakeTorque : 0;
        var steerAngle = maxSteerAngle;
        var handbrake = Input.GetKey(KeyCode.Space);

        if (speedKph > 50)
            steerAngle = Mathf.Lerp(minSteerAngle, maxSteerAngle, 1f - ((speedKph - 50f) / 50f));

        var steering = 0f;
        var setSteer = 0f;
        if (Input.GetKey(KeyCode.A)) { steering = steerAngle * -1f; setSteer = -1; }
        if (Input.GetKey(KeyCode.D)) { steering = steerAngle * 1f; setSteer = 1; }

        var setBrakeTorque = 0f;
        if (Input.GetKey(KeyCode.S))
        {
            if (speedKph > 1f) setBrakeTorque = brakeTorque;
            else
            {
                setTorque = reverseTorque * -1f;
                steering *= -1f;
            }
        }

        for (int i = 0; i < axles.Length; ++i)
            axles[i].UpdateWheels();

        for (int i = 0; i < axles.Length; ++i)
            axles[i].UpdateForces(setTorque, setBrakeTorque, steering);

        CheckSteeringAngle();

        for (int i = 0; i < axles.Length; ++i)
            axles[i].UpdateGrip(handbrake, setSteer);

        rb.AddForce(rb.velocity.magnitude * downForceValue * -Vector3.up);

        speedKph = rb.velocity.magnitude * 3.6f;
        speedMph = rb.velocity.magnitude * 2.237f;
        var dot = Vector3.Dot(transform.forward, rb.velocity);
        if (dot < 0)
        {
            speedKph *= -1f;
            speedMph *= -1f;
        }

        if(speed != null)
            speed.text = $"{Mathf.RoundToInt(speedKph)}";

        if (Mathf.Abs(setTorque) <= 0f) rb.velocity *= 0.998f;
        if (speedKph > maxSpeed && dot > 0) rb.velocity *= 0.99f;
        if (speedKph < maxReverseSpeed && dot < 0) rb.velocity *= 0.99f;
    }

    void CheckSteeringAngle()
    {
        for (int a = 0; a < axles.Length; ++a)
        {
            if (!axles[a].CheckGrounded()) return;
        }

        if (Mathf.Abs(oldRotation - transform.eulerAngles.y) > 10f)
        {
            oldRotation = transform.eulerAngles.y;
            return;
        }

        var turnAdjust = (transform.eulerAngles.y - oldRotation) * steerHelper;
        var velRotation = Quaternion.AngleAxis(turnAdjust, Vector3.up);
        rb.velocity = velRotation * rb.velocity;

        oldRotation = transform.eulerAngles.y;
    }

    internal void StartWheelSkid(int axleId, int i)
    {
        //Get from pool
        if (skidTrails[axleId, i] == null)
            skidTrails[axleId, i] = Instantiate(skidTrail);

        skidTrails[axleId, i].parent = axles[axleId].wheels[i].wheelCollider.transform;
        skidTrails[axleId, i].localRotation = Quaternion.Euler(90f, 0, 0);
        skidTrails[axleId, i].localPosition = axles[axleId].wheels[i].wheelCollider.radius * -Vector3.up;
    }

    internal void EndWheelSkid(int axleId, int i)
    {
        if (skidTrails[axleId, i] == null) return;
        Transform holder = skidTrails[axleId, i];
        skidTrails[axleId, i] = null;

        holder.parent = null;
        holder.rotation = Quaternion.Euler(90f, 0, 0);

        Destroy(holder.gameObject, 30f);        //todo: Return to pool
    }
}
