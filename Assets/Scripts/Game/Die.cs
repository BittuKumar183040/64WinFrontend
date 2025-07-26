using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class DropDice : MonoBehaviour
{
    private Rigidbody _rigidbody;
    [SerializeField] float _force = 200f;
    [SerializeField] float _mass = 10f;
    [SerializeField] float _torque = 210f;

    [SerializeField] private List<Transform> faceDetectors;

    [SerializeField] private float _stopThreshold = 0.5f;
    private bool isCheckingMovement = false;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        if (_rigidbody == null)
        {
            Debug.LogError("Rigidbody component is missing.");
        }
    }

    public void DropWithSharedDirection(Vector3 dicePos, Quaternion diceRot, Vector3 forceDirection)
    {
        if (_rigidbody != null)
        {
            _rigidbody.position = dicePos;
            _rigidbody.rotation = diceRot;

            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;

            _rigidbody.linearDamping = 0f;
            _rigidbody.angularDamping = 0.05f;
            _rigidbody.mass = _mass;
            _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            _rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;

            Vector3 force = forceDirection * _force; 
            Vector3 torque = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f)
            ).normalized * _torque;

            _rigidbody.AddTorque(torque, ForceMode.Impulse);
            _rigidbody.AddForce(force, ForceMode.Impulse);

            if (!isCheckingMovement)
                StartCoroutine(CheckIfStopped());
        }
    }


    public int GetResult()
    {
        if (faceDetectors == null || faceDetectors.Count == 0)
        {
            Debug.LogError("Face detectors are not assigned or empty.");
            return -1;
        }

        float maxDot = -1f;
        string bottomFaceName = "Unknown";

        foreach (Transform face in faceDetectors)
        {
            Vector3 direction = (face.position - transform.position).normalized;
            float dot = Vector3.Dot(direction, Vector3.down);

            if (dot > maxDot)
            {
                maxDot = dot;
                bottomFaceName = face.name;
            }
        }

        return int.Parse(bottomFaceName);
    }


    private IEnumerator CheckIfStopped()
    {
        isCheckingMovement = true;
        yield return new WaitForSeconds(1f);

        while (_rigidbody.linearVelocity.magnitude > _stopThreshold || _rigidbody.angularVelocity.magnitude > _stopThreshold)
        {
            yield return null;
        }

        isCheckingMovement = false;
    }
}
