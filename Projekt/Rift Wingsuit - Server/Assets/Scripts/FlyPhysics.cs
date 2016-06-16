using UnityEngine;
using System.Collections;

public class FlyPhysics : MonoBehaviour
{

    private static float GRAVITY = 9.81f;


    //Taken from: http://onlinelibrary.wiley.com/doi/10.1256/wea.29.02/pdf
    //The force that works against gravity (Traegheit)
    private static float DRAG_COEFFICIENT = 0.6f;

    public static Vector3 RATIO_APPLICATION_VECTOR = new Vector3(0.0f, 0.2f, 0.375f);

    //(Wortwörtliche) Durchschnittsfläche des Torso
    private static float CROSS_SECTION_AREA_MINIMUM = 0.25f;
    private static float CROSS_SECTION_AREA_MAXIMUM = 1.0f;
    private static float CROSS_SECTION_AREA = CROSS_SECTION_AREA_MINIMUM; //m²

    public static float BEST_GLIDE_ANGLE_RATIO = 0.75f;
    public static float BEST_GLIDE_ANGLE = 22.0f;

    public static float MASS_OF_DIVER_IN_KG = 75;
    private static float AIR_DENSITY = 1.225f; //1.0 (higher areas) ~ 1.5 (lower, e.g. sea)

    public static float getAngleToFloor(Quaternion q)
    {
        float angle = q.ToEulerAngles().x * 180.0f / Mathf.PI;
        if (angle < 0.0f)
        {
            angle = 0.0f;
        }
        return angle;
    }

    public static float getRotation(Quaternion q)
    {
        float rotation = q.ToEulerAngles().z * 180.0f / Mathf.PI;
        return rotation;
    }

    private static Vector3 calculatePitchVector(float pitch)
    {
        float velocityMS = FlyPhysics.getVelocity(Time.time);

        float angleDiff = 90.0f - pitch;
        //The ratio consists of 3 parts:
        //- The "left-rotation" (90-ANGLE) of the axis system -> easier calculation
        //- The ratio between said angle and the optimal gliding angle (also converted)
        //- The fall-to-fly-ratio (i.e. who many meters do we 'fly' in comparison to 1m of height drop))
        float glidingRatio = angleDiff / (90.0f - BEST_GLIDE_ANGLE) * BEST_GLIDE_ANGLE_RATIO;

        //In case the angle is not optimal, consider it as "breaking"
        //This is done because the point beyond the optimal angle must not 
        //lead to an acceleration (higher angles = faster)
        //nor better gliding (because we passed beyond said point)
        if (pitch < BEST_GLIDE_ANGLE)
        {
            //Losing Speed = Ratio between the current angle and the best one
            //The total speed may be up to halved (if angle == 0)
            float losingSpeed = ((BEST_GLIDE_ANGLE - pitch) / BEST_GLIDE_ANGLE) * 0.2f * velocityMS;
            //printf("Losing speed due to angle < BEST_GLIDING => BREAKING: %.3f\n", losingSpeed);
            velocityMS -= losingSpeed;

            //Seen as the body offers more resistence beyond the best gliding angle
            //the gliding-ratio needs to be adjusted (i.e. clamped))
            glidingRatio -= (glidingRatio - BEST_GLIDE_ANGLE_RATIO) * 1.1f;
        }

        Vector3 output = new Vector3(0.0f, -(velocityMS * (1.0f - glidingRatio)), velocityMS * glidingRatio);
        return new Vector3(0.0f, output.y * RATIO_APPLICATION_VECTOR.y, output.z * RATIO_APPLICATION_VECTOR.z);
    }

    private static Vector3 calculateRotationVector(float rotation)
    {
        return new Vector3(rotation / 90.0f * 1.8f, 0.0f, 0.0f);
    }


    public static Vector3 calculateFallVector(float pitch, float rotation)
    {
        Vector3 pitchVector = calculatePitchVector(pitch);
        Vector3 rotationVector = calculateRotationVector(rotation);
        return new Vector3(rotationVector.x, pitchVector.y, pitchVector.z);
    }

    public static void adjustCrossSectionArea(float angle)
    {
        if (angle <= 0.0f)
        {
            CROSS_SECTION_AREA = CROSS_SECTION_AREA_MAXIMUM;
        }
        else if (angle >= 90.0f)
        {
            CROSS_SECTION_AREA = CROSS_SECTION_AREA_MINIMUM;
        }
        else {
            CROSS_SECTION_AREA = (CROSS_SECTION_AREA_MAXIMUM - CROSS_SECTION_AREA_MINIMUM) * ((90.0f - angle) / 90.0f) +
                CROSS_SECTION_AREA_MINIMUM;
        }
    }

    private static float msToKMH(float value)
    {
        return value * 3.6f;
    }

    private static float tanh(float x)
    {
        return (Mathf.Exp(x) - Mathf.Exp(-x)) / (Mathf.Exp(x) + Mathf.Exp(-x));
    }

    public static float getVelocityInKMH(float timePassed)
    {
        return msToKMH(getVelocity(timePassed));
    }

    //Velocity at timepoint (t)
    //https://en.wikipedia.org/wiki/Free_fall#Uniform_gravitational_field_with_air_resistance
    public static float getVelocity(float timePassed)
    {
        float terminalVelocity = getTerminalVelocity();

        return terminalVelocity * tanh((GRAVITY * timePassed) / terminalVelocity);
    }

    //RETURNS VALUE IN METER PER SECOND! With default settings ~63m/s = ~230km/h
    public static float getTerminalVelocity()
    {
        return Mathf.Sqrt(2 * MASS_OF_DIVER_IN_KG * GRAVITY / (AIR_DENSITY * CROSS_SECTION_AREA * DRAG_COEFFICIENT));
    }

    public static float getFallHeight(Quaternion q, float timePassed)
    {
        float currentAngle = getAngleToFloor(q);
        //With this formula, you always drop with at least 50% of the free-fall height
        float result = ((GRAVITY * timePassed) / 2);
        return result;
    }
}

