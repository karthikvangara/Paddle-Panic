using UnityEngine;
using Cinemachine;
public class CameraRespawnHelper : MonoBehaviour 
{ 
    public CinemachineVirtualCamera virtualCam; 
    public void Respawn(Transform respawnPoint, GameObject boat) 
    { 
        Debug.Log("Camera Handling when respawned"); // Move the boat instantly
        boat.transform.position = respawnPoint.position;
        boat.transform.rotation = respawnPoint.rotation; // Force Cinemachine to snap to new position (no smoothing)
        var brain = Camera.main.GetComponent<CinemachineBrain>();
        if (brain != null) 
        {
            // Temporarily disable blending
            var oldBlend = brain.m_DefaultBlend;
            brain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.Cut, 0f); 
            // Force camera to align immediately
            virtualCam.ForceCameraPosition(boat.transform.position, boat.transform.rotation);
            // Restore original blend
            brain.m_DefaultBlend = oldBlend; 
        } 
    } 
}