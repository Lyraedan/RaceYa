using UnityEngine;

[System.Serializable]
public class AxleData
{
    public WheelObj[] wheels;
    public bool steer;
    public bool drive;
    public bool brakes;
    public bool rearAxle;
    public bool antiRollBar;

    private CarController home;
    private Rigidbody rb;
    private float antiRoll;
    private int axleId;
    private int leftWheelIndex;
    private int rightWheelIndex;
    private float driftFactor;
    private float handBrakeFrictionMultiplier = 2f;
    private float negativeTurnAngle;
    private float positiveTurnAngle;
    internal void Setup(Rigidbody rigidbody, float antiRollForce, int axle, CarController carController)
    {
        home = carController;
        axleId = axle;
        rb = rigidbody;
        antiRoll = antiRollForce;

        for (int i = 0; i < wheels.Length; ++i)
        {
            if (wheels[i].leftWheel) leftWheelIndex = i;
            else rightWheelIndex = i;
        }

        positiveTurnAngle = Mathf.Rad2Deg * Mathf.Atan(home.wheelBase / (home.turnRadius + (home.rearTrack / 2)));
        negativeTurnAngle = Mathf.Rad2Deg * Mathf.Atan(home.wheelBase / (home.turnRadius - (home.rearTrack / 2)));

    }

    internal void UpdateWheels()
    {
        for (int i = 0; i < wheels.Length; ++i)
        {
            //Update position
            wheels[i].wheelCollider.GetWorldPose(out Vector3 pos, out Quaternion rotation);
            wheels[i].mesh.position = pos;
            wheels[i].mesh.rotation = rotation;
        }
    }

    internal void UpdateForces(float torque, float brakeTorque, float steering)
    {
        for (int i = 0; i < wheels.Length; ++i)
        {
            //if (steer) wheels[i].wheelCollider.steerAngle = CalculateAckermannSteering(steeringInput, i);     //This needs fine tuning before use
            if (steer) wheels[i].wheelCollider.steerAngle = steering;
            if (drive) wheels[i].wheelCollider.motorTorque = torque;
            if (brakes) wheels[i].wheelCollider.brakeTorque = brakeTorque;
        }
    }

    private float CalculateAckermannSteering(float input, int wheelIndex)
    {
        var angleLeft = 0f;
        var angleRight = 0f;

        if (input > 0f)
        {
            angleLeft = positiveTurnAngle * input;
            angleRight = negativeTurnAngle * input;
        }
        else if (input < 0f)
        {
            angleLeft = negativeTurnAngle * input;
            angleRight = positiveTurnAngle * input;
        }

        if (wheelIndex == leftWheelIndex) return angleLeft;

        return angleRight;
    }

    internal void UpdateGrip(bool handbrake, float steering)
    {
        for (int i = 0; i < wheels.Length; ++i)
        {
            //Update position
            wheels[i].wheelCollider.GetWorldPose(out Vector3 pos, out Quaternion rotation);
            wheels[i].mesh.position = pos;
            wheels[i].mesh.rotation = rotation;

            var leftWheel = CheckRollbar(leftWheelIndex);
            var rightWheel = CheckRollbar(rightWheelIndex);
            var checkWheel = i == leftWheelIndex ? leftWheel : rightWheel;

            //Play sound at wheel position if skidding
            if (checkWheel.isSkidding) home.StartWheelSkid(axleId, i);
            else home.EndWheelSkid(axleId, i);

            AdjustTraction(i, handbrake);

            if (rearAxle) AdjustDrift(checkWheel, steering);

            if (!antiRollBar) continue;

            float antiRollForce = (leftWheel.distance - rightWheel.distance) * antiRoll;

            if (leftWheel.isGrounded)
                rb.AddForceAtPosition(wheels[leftWheelIndex].wheelCollider.transform.up * -antiRollForce, wheels[leftWheelIndex].wheelCollider.transform.position);

            if (rightWheel.isGrounded)
                rb.AddForceAtPosition(wheels[rightWheelIndex].wheelCollider.transform.up * -antiRollForce, wheels[rightWheelIndex].wheelCollider.transform.position);
        }
    }

    private void AdjustTraction(int i, bool handbrake)
    {
        if (handbrake)
        {
            AdjustHandbrakeDraft(i);
            return;
        }

        var forwardFriction = wheels[i].wheelCollider.forwardFriction;
        var sidewaysFriction = wheels[i].wheelCollider.sidewaysFriction;

        var force = ((home.speedKph * handBrakeFrictionMultiplier) / 300) + 1;
        forwardFriction.extremumValue = force;
        forwardFriction.asymptoteValue = force;
        sidewaysFriction.extremumValue = force;
        sidewaysFriction.asymptoteValue = force;

        wheels[i].wheelCollider.forwardFriction = forwardFriction;
        wheels[i].wheelCollider.sidewaysFriction = sidewaysFriction;
    }

    void AdjustHandbrakeDraft(int i)
    {
        var sidewaysFriction = wheels[i].wheelCollider.sidewaysFriction;
        var forwardFriction = wheels[i].wheelCollider.forwardFriction;

        var velocity = 0f;
        var drift = Mathf.SmoothDamp(forwardFriction.asymptoteValue, driftFactor * handBrakeFrictionMultiplier, ref velocity, 0.7f * Time.deltaTime);

        sidewaysFriction.extremumValue = drift;
        sidewaysFriction.asymptoteValue = drift;
        forwardFriction.extremumValue = drift;
        forwardFriction.asymptoteValue = drift;

        if (!rearAxle)
        {
            sidewaysFriction.extremumValue = 1.1f;
            sidewaysFriction.asymptoteValue = 1.1f;
            forwardFriction.extremumValue = 1.1f;
            forwardFriction.asymptoteValue = 1.1f;

            wheels[i].wheelCollider.sidewaysFriction = sidewaysFriction;
            wheels[i].wheelCollider.forwardFriction = forwardFriction;
        }

        rb.AddForce(home.transform.forward * (home.speedKph / 400f) * -8000f);
    }

    void AdjustDrift(RollbarData checkWheel, float steering)
    {
        if (checkWheel.sidewaysSlip < 0) driftFactor = (1 - steering) * Mathf.Abs(checkWheel.sidewaysSlip);
        else if (checkWheel.sidewaysSlip > 0) driftFactor = (1 + steering) * Mathf.Abs(checkWheel.sidewaysSlip);
    }

    internal bool CheckGrounded()
    {
        for (int w = 0; w < wheels.Length; ++w)
        {
            wheels[w].wheelCollider.GetGroundHit(out WheelHit hit);
            if (hit.normal != Vector3.zero) return false;
        }
        return true;
    }

    RollbarData CheckRollbar(int w)
    {
        var isGrounded = wheels[w].wheelCollider.GetGroundHit(out WheelHit hit);
        if (!isGrounded) return new RollbarData();

        var skidMin = 1.75f;    //0.5f
        return new RollbarData()
        {
            isGrounded = true,
            isSkidding = Mathf.Abs(hit.forwardSlip) > skidMin || Mathf.Abs(hit.sidewaysSlip) > skidMin,
            distance = (-wheels[w].wheelCollider.transform.InverseTransformPoint(hit.point).y - wheels[w].wheelCollider.radius) / wheels[w].wheelCollider.suspensionDistance,
            sidewaysSlip = hit.sidewaysSlip
        };
    }
}
