using UnityEngine;
using System.Collections;

public class FlyPhysics : MonoBehaviour {

	private static float GRAVITY = 9.81f;

	
	//Taken from: http://onlinelibrary.wiley.com/doi/10.1256/wea.29.02/pdf
	//The force that works against gravity (Traegheit)
	private static float DRAG_COEFFICIENT = 0.6f;
	
	//(Wortwörtliche) Durchschnittsfläche des Torso
    public static float CROSS_SECTION_AREA = 0.2f; //m²
    private static float CROSS_SECTION_AREA_MINIMUM = 0.2f;
    private static float CROSS_SECTION_AREA_MAXIMUM = 1.0f;

	public static float MASS_OF_DIVER_IN_KG = 75;
	public static float AIR_DENSITY = 1.225f; //1.0 (higher areas) ~ 1.5 (lower, e.g. sea)

	public static float getAngleToFloor(Quaternion q) {
		Vector3 asVector = q.ToEulerAngles ();
		return (asVector.x * 180.0f / Mathf.PI);
	}

	public static void adjustCrossSectionArea(Quaternion q) {
		float angle = getAngleToFloor (q);
		if (angle <= 0.0f) {
			CROSS_SECTION_AREA = CROSS_SECTION_AREA_MAXIMUM;
		} else if(angle >= 90.0f) {
			CROSS_SECTION_AREA = CROSS_SECTION_AREA_MINIMUM;
		} else {
			CROSS_SECTION_AREA = (CROSS_SECTION_AREA_MAXIMUM - CROSS_SECTION_AREA_MINIMUM) * ((90.0f-angle) / 90.0f) + 
				CROSS_SECTION_AREA_MINIMUM;
		}
	}

	private static float msToKMH(float value) {
		return value * 3.6f;
	}

	private static float tanh(float x) {
		return (Mathf.Exp(x) - Mathf.Exp(-x)) / (Mathf.Exp(x) + Mathf.Exp(-x));
	}
	
	public static float getVelocityInKMH(float timePassed) {
		return msToKMH(getVelocity (timePassed));
	}
	
	//Velocity at timepoint (t)
	//https://en.wikipedia.org/wiki/Free_fall#Uniform_gravitational_field_with_air_resistance
	public static  float getVelocity(float timePassed) {
		float terminalVelocity = getTerminalVelocity ();
		
		return terminalVelocity * tanh ((GRAVITY * timePassed) / terminalVelocity);
	}
	
	//RETURNS VALUE IN METER PER SECOND! With default settings ~63m/s = ~230km/h
	public static  float getTerminalVelocity() {
		return Mathf.Sqrt(2 * MASS_OF_DIVER_IN_KG * GRAVITY / (AIR_DENSITY * CROSS_SECTION_AREA * DRAG_COEFFICIENT));
	}
	
	public static float getFallHeight(Quaternion q, float timePassed) {
		float currentAngle = getAngleToFloor (q);
		//With this formula, you always drop with at least 50% of the free-fall height
		float result = ((GRAVITY * timePassed) / 2) * ((currentAngle+60.0f) / 360.0f);
		return result;
	}
}

