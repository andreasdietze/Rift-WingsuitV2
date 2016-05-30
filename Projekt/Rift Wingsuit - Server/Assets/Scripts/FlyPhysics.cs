using UnityEngine;
using System.Collections;

public class FlyPhysics : MonoBehaviour {

	private static float GRAVITY = 9.81f;

	
	//Taken from: http://onlinelibrary.wiley.com/doi/10.1256/wea.29.02/pdf
	//The force that works against gravity (Traegheit)
	private static float DRAG_COEFFICIENT = 0.6f;
	
	//(Wortwörtliche) Durchschnittsfläche des Torso
	public static float CROSS_SECTION_AREA = 0.4f; //m²
	public static float MASS_OF_DIVER_IN_KG = 75;
	public static float AIR_DENSITY = 1.225f; //1.0 (higher areas) ~ 1.5 (lower, e.g. sea)

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
	
	public static float getFallHeight(float timePassed) {
		return (GRAVITY * timePassed) / 2;
	}
}

