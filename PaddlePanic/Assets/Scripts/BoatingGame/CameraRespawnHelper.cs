using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraRespawnHelper : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCam;

    // Call this after you've moved/rotated the boat
    public void SnapAfterRespawn(Transform boat)
    {
        if (virtualCam == null || boat == null) return;
        StartCoroutine(SnapAtEndOfFrame(boat));
    }
    private IEnumerator SnapAtEndOfFrame(Transform boat)
    {
        yield return new WaitForEndOfFrame();

        var brain = Camera.main.GetComponent<CinemachineBrain>();
        if (brain == null) yield break;

        // Save camera world position & rotation
        Vector3 savedCamPos = Camera.main.transform.position;
        Quaternion savedCamRot = Camera.main.transform.rotation;

        // Save Framing Transposer offsets and angles
        var transposer = virtualCam.GetCinemachineComponent<CinemachineFramingTransposer>();
        Vector3 savedOffset = Vector3.zero;
        if (transposer != null)
        {
            savedOffset = transposer.m_TrackedObjectOffset;
            // Save the rotation of the virtual camera relative to the target
            savedCamRot = virtualCam.transform.rotation;
        }

        // Temporary camera cut (forces Cinemachine to accept current transform)
        var tmpGO = new GameObject("__CM_SnapTemp");
        tmpGO.transform.SetPositionAndRotation(savedCamPos, savedCamRot);
        var tmpVC = tmpGO.AddComponent<CinemachineVirtualCamera>();
        tmpVC.Priority = (virtualCam != null) ? virtualCam.Priority + 50 : 1000;
        tmpVC.Follow = null;
        tmpVC.LookAt = null;

        var oldBlend = brain.m_DefaultBlend;
        brain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.Cut, 0f);
        brain.ManualUpdate();

        Camera.main.transform.SetPositionAndRotation(savedCamPos, savedCamRot);
        Object.Destroy(tmpGO);

        // Reset the real VC
        virtualCam.PreviousStateIsValid = false;
        virtualCam.OnTargetObjectWarped(boat, Vector3.zero);

        // Restore Transposer offset
        if (transposer != null)
            transposer.m_TrackedObjectOffset = savedOffset;

        // Force VC rotation to the saved one
        virtualCam.transform.rotation = savedCamRot;

        brain.ManualUpdate();
        brain.m_DefaultBlend = oldBlend;
    }

    /*private IEnumerator SnapAtEndOfFrame(Transform boat)
    {
        yield return new WaitForEndOfFrame();

        var brain = Camera.main.GetComponent<CinemachineBrain>();
        if (brain == null) yield break;

        // Save camera world transform
        Vector3 savedCamPos = Camera.main.transform.position;
        Quaternion savedCamRot = Camera.main.transform.rotation;

        // Save Framing Transposer offsets
        var transposer = virtualCam.GetCinemachineComponent<CinemachineFramingTransposer>();
        Vector3 savedOffset = Vector3.zero;
        if (transposer != null)
            savedOffset = transposer.m_TrackedObjectOffset;

        // Temporary camera cut (same as before)
        var tmpGO = new GameObject("__CM_SnapTemp");
        tmpGO.transform.SetPositionAndRotation(savedCamPos, savedCamRot);
        var tmpVC = tmpGO.AddComponent<CinemachineVirtualCamera>();
        tmpVC.Priority = (virtualCam != null) ? virtualCam.Priority + 50 : 1000;
        tmpVC.Follow = null;
        tmpVC.LookAt = null;

        var oldBlend = brain.m_DefaultBlend;
        brain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.Cut, 0f);
        brain.ManualUpdate();

        Camera.main.transform.SetPositionAndRotation(savedCamPos, savedCamRot);
        Object.Destroy(tmpGO);

        // Reset the real VC
        virtualCam.PreviousStateIsValid = false;
        virtualCam.OnTargetObjectWarped(boat, Vector3.zero);

        // **Restore Transposer offset**
        if (transposer != null)
            transposer.m_TrackedObjectOffset = savedOffset;

        brain.ManualUpdate();
        brain.m_DefaultBlend = oldBlend;
    } */
}


