using System.Collections;
using UnityEngine;
using Cinemachine;
using System.Reflection;

public class CameraRespawnHelper : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCam;
    public Transform boat;

    public void RemoveLookAt()
    {
        virtualCam.LookAt = null;
        StartCoroutine(AssignLookAt());
    }

    public IEnumerator AssignLookAt()
    {
        yield return new WaitForSeconds(1);
        virtualCam.LookAt = boat;
    }

    public void RepositionVirtualCamera(Vector3 respawnPosition)
    {
        virtualCam.transform.position = respawnPosition;
    }

    /*public void SnapAfterRespawn(Transform boat)
    {
        if (virtualCam == null || boat == null) return;
        StartCoroutine(SnapAtEndOfFrame(boat));
    }

    private IEnumerator SnapAtEndOfFrame(Transform boat)
    {
        yield return new WaitForEndOfFrame();

        var brain = Camera.main.GetComponent<CinemachineBrain>();
        if (brain == null) yield break;

        // --- Save camera world transform (what we want to preserve) ---
        Vector3 savedCamPos = Camera.main.transform.position;
        Quaternion savedCamRot = Camera.main.transform.rotation;

        Debug.Log($"[Snap] savedCamPos={savedCamPos}, savedCamRot={savedCamRot.eulerAngles}");

        // --- FramingTransposer (body) damping + offset save/zero ---
        var transposer = virtualCam.GetCinemachineComponent<CinemachineFramingTransposer>();
        Vector3 savedTransposerDamping = Vector3.zero;
        Vector3 savedTrackedOffset = Vector3.zero;
        bool transposerHasVectorDampingProp = false;

        if (transposer != null)
        {
            // Try property "Damping" (Vector3) first (newer versions)
            var propD = transposer.GetType().GetProperty("Damping", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (propD != null && propD.PropertyType == typeof(Vector3))
            {
                savedTransposerDamping = (Vector3)propD.GetValue(transposer);
                propD.SetValue(transposer, Vector3.zero);
                transposerHasVectorDampingProp = true;
            }
            else
            {
                // Fallback to m_XDamping / m_YDamping / m_ZDamping fields
                float x = GetFloatFieldOrProp(transposer, "m_XDamping");
                float y = GetFloatFieldOrProp(transposer, "m_YDamping");
                float z = GetFloatFieldOrProp(transposer, "m_ZDamping");
                savedTransposerDamping = new Vector3(x, y, z);
                SetFloatFieldOrProp(transposer, "m_XDamping", 0f);
                SetFloatFieldOrProp(transposer, "m_YDamping", 0f);
                SetFloatFieldOrProp(transposer, "m_ZDamping", 0f);
            }

            // Save tracked offset if present
            if (!TryGetVector3FieldOrProp(transposer, "m_TrackedObjectOffset", out savedTrackedOffset))
            {
                TryGetVector3FieldOrProp(transposer, "TrackedObjectOffset", out savedTrackedOffset);
            }
        }

        // --- Composer (aim) save/zero & screen position save ---
        var composer = virtualCam.GetCinemachineComponent<CinemachineComposer>();
        float savedScreenX = 0f, savedScreenY = 0f;
        float savedHorizDamp = 0f, savedVertDamp = 0f;
        string foundHName = null, foundVName = null;

        if (composer != null)
        {
            // Save and zero common screen fields (m_ScreenX / m_ScreenY)
            savedScreenX = GetFloatFieldOrProp(composer, "m_ScreenX", "ScreenX");
            savedScreenY = GetFloatFieldOrProp(composer, "m_ScreenY", "ScreenY");

            // Horizontal/Vertical damping fields (try a few common names)
            if (TryGetFloatFieldOrProp(composer, "m_HorizontalDamping", out savedHorizDamp))
            {
                foundHName = "m_HorizontalDamping";
                SetFloatFieldOrProp(composer, foundHName, 0f);
            }
            else if (TryGetFloatFieldOrProp(composer, "m_HorizontalDampingX", out savedHorizDamp))
            {
                foundHName = "m_HorizontalDampingX";
                SetFloatFieldOrProp(composer, foundHName, 0f);
            }

            if (TryGetFloatFieldOrProp(composer, "m_VerticalDamping", out savedVertDamp))
            {
                foundVName = "m_VerticalDamping";
                SetFloatFieldOrProp(composer, foundVName, 0f);
            }
            else if (TryGetFloatFieldOrProp(composer, "m_VerticalDampingY", out savedVertDamp))
            {
                foundVName = "m_VerticalDampingY";
                SetFloatFieldOrProp(composer, foundVName, 0f);
            }

            // As a last resort try "m_RotationDamping" (older variants)
            if (foundHName == null && TryGetFloatFieldOrProp(composer, "m_RotationDamping", out float rD))
            {
                foundHName = "m_RotationDamping";
                savedHorizDamp = rD;
                SetFloatFieldOrProp(composer, "m_RotationDamping", 0f);
            }
        }

        Debug.Log($"[Snap] savedScreenX={savedScreenX} savedScreenY={savedScreenY} savedTransposerDamping={savedTransposerDamping}");

        // --- Force a hard cut to a temporary camera so the main camera adopts saved transform ---
        var tmpGO = new GameObject("__CM_SnapTemp");
        tmpGO.hideFlags = HideFlags.HideAndDontSave;
        tmpGO.transform.SetPositionAndRotation(savedCamPos, savedCamRot);
        var tmpVC = tmpGO.AddComponent<CinemachineVirtualCamera>();
        tmpVC.Priority = virtualCam.Priority + 50;
        tmpVC.Follow = null;
        tmpVC.LookAt = null;

        var oldBlend = brain.m_DefaultBlend;
        brain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.Cut, 0f);
        brain.ManualUpdate();

        // Enforce main camera transform
        Camera.main.transform.SetPositionAndRotation(savedCamPos, savedCamRot);

        // Clean up temp
        Object.Destroy(tmpGO);

        // --- Prevent Cinemachine from "catching up" ---
        virtualCam.PreviousStateIsValid = false;

        // Maintain same world-space offset relative to boat (keeps same side)
        Vector3 worldOffset = savedCamPos - boat.position;
        virtualCam.transform.SetPositionAndRotation(boat.position + worldOffset, savedCamRot);

        // Make Cinemachine accept the warp (zero delta)
        virtualCam.OnTargetObjectWarped(boat, Vector3.zero);

        // Reapply composer screen X/Y and keep damping zero while we force state
        if (composer != null)
        {
            SetFloatFieldOrProp(composer, "m_ScreenX", savedScreenX);
            SetFloatFieldOrProp(composer, "ScreenX", savedScreenX);
            SetFloatFieldOrProp(composer, "m_ScreenY", savedScreenY);
            SetFloatFieldOrProp(composer, "ScreenY", savedScreenY);
        }

        // Force Cinemachine to process the current state immediately
        brain.ManualUpdate();

        // Re-assert camera transform (very defensive)
        Camera.main.transform.SetPositionAndRotation(savedCamPos, savedCamRot);
        virtualCam.transform.SetPositionAndRotation(boat.position + worldOffset, savedCamRot);

        // --- Restore composer damping & screen values ---
        if (composer != null)
        {
            if (!string.IsNullOrEmpty(foundHName))
                SetFloatFieldOrProp(composer, foundHName, savedHorizDamp);
            if (!string.IsNullOrEmpty(foundVName))
                SetFloatFieldOrProp(composer, foundVName, savedVertDamp);

            // restore screen positions (defensive)
            SetFloatFieldOrProp(composer, "m_ScreenX", savedScreenX);
            SetFloatFieldOrProp(composer, "ScreenX", savedScreenX);
            SetFloatFieldOrProp(composer, "m_ScreenY", savedScreenY);
            SetFloatFieldOrProp(composer, "ScreenY", savedScreenY);
        }

        // --- Restore transposer damping & offset ---
        if (transposer != null)
        {
            if (transposerHasVectorDampingProp)
            {
                var prop = transposer.GetType().GetProperty("Damping", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (prop != null && prop.PropertyType == typeof(Vector3))
                    prop.SetValue(transposer, savedTransposerDamping);
            }
            else
            {
                SetFloatFieldOrProp(transposer, "m_XDamping", savedTransposerDamping.x);
                SetFloatFieldOrProp(transposer, "m_YDamping", savedTransposerDamping.y);
                SetFloatFieldOrProp(transposer, "m_ZDamping", savedTransposerDamping.z);
            }

            // restore tracked offset defensively
            SetVector3FieldOrProp(transposer, "m_TrackedObjectOffset", savedTrackedOffset);
            SetVector3FieldOrProp(transposer, "TrackedObjectOffset", savedTrackedOffset);
        }

        // restore blend
        brain.m_DefaultBlend = oldBlend;

        // final update
        brain.ManualUpdate();

        Debug.Log($"[Snap] finalCamPos={Camera.main.transform.position}, finalCamRot={Camera.main.transform.rotation.eulerAngles}");
    }

    // ---------------- reflection helpers ----------------
    static float GetFloatFieldOrProp(object o, string name1, string name2 = null)
    {
        if (o == null) return 0f;
        var t = o.GetType();
        BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        if (name1 != null)
        {
            var p = t.GetProperty(name1, flags);
            if (p != null && p.PropertyType == typeof(float))
                return (float)p.GetValue(o);
            var f = t.GetField(name1, flags);
            if (f != null && f.FieldType == typeof(float))
                return (float)f.GetValue(o);
        }
        if (name2 != null)
        {
            var p = t.GetProperty(name2, flags);
            if (p != null && p.PropertyType == typeof(float))
                return (float)p.GetValue(o);
            var f = t.GetField(name2, flags);
            if (f != null && f.FieldType == typeof(float))
                return (float)f.GetValue(o);
        }
        return 0f;
    }

    static bool TryGetFloatFieldOrProp(object o, string name, out float val)
    {
        val = 0f;
        if (o == null) return false;
        var t = o.GetType();
        BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        var p = t.GetProperty(name, flags);
        if (p != null && p.PropertyType == typeof(float)) { val = (float)p.GetValue(o); return true; }
        var f = t.GetField(name, flags);
        if (f != null && f.FieldType == typeof(float)) { val = (float)f.GetValue(o); return true; }
        return false;
    }

    static bool TryGetVector3FieldOrProp(object o, string name, out Vector3 val)
    {
        val = Vector3.zero;
        if (o == null) return false;
        var t = o.GetType();
        BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        var p = t.GetProperty(name, flags);
        if (p != null && p.PropertyType == typeof(Vector3)) { val = (Vector3)p.GetValue(o); return true; }
        var f = t.GetField(name, flags);
        if (f != null && f.FieldType == typeof(Vector3)) { val = (Vector3)f.GetValue(o); return true; }
        return false;
    }

    static void SetFloatFieldOrProp(object o, string name, float value)
    {
        if (o == null || name == null) return;
        var t = o.GetType();
        BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        var p = t.GetProperty(name, flags);
        if (p != null && p.PropertyType == typeof(float)) { p.SetValue(o, value); return; }
        var f = t.GetField(name, flags);
        if (f != null && f.FieldType == typeof(float)) { f.SetValue(o, value); return; }
    }

    static void SetVector3FieldOrProp(object o, string name, Vector3 v)
    {
        if (o == null || name == null) return;
        var t = o.GetType();
        BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        var p = t.GetProperty(name, flags);
        if (p != null && p.PropertyType == typeof(Vector3)) { p.SetValue(o, v); return; }
        var f = t.GetField(name, flags);
        if (f != null && f.FieldType == typeof(Vector3)) { f.SetValue(o, v); return; }
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


