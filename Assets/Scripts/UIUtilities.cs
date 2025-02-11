using UnityEngine;

public static class UIUtilities 
{
    public static void AdjustHeight(GameObject menuGO, float inclinationAngle = 11.0f)
    {
        Vector3 newPosition = new Vector3 {
            x = menuGO.transform.position.x,
            y = -menuGO.transform.position.z * Mathf.Sin(Mathf.Deg2Rad * inclinationAngle),
            z = menuGO.transform.position.z
        };
        menuGO.transform.position = newPosition;
    }
}