using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRProto
{
    public class GrabberBehaviour : MonoBehaviour
    {
        public bool isLeftHanded;

        /// <summary>
        /// The collider which this object uses to grab other objects
        /// </summary>
        public GameObject interactionCollider;
        public GameObject grabCollider;

        /// <summary>
        /// Where objects are attached to
        /// </summary>
        public Transform snapPoint;
        public Rigidbody grabberRiggidBody;

        public Action<GrabberBehaviour> OnNewGrabber;

        public enum GrabberState
        {
            //The grabber is free and acting like an object that we can grab
            free,
            //The grabber is being hovered
            hovering,
            //The grabber is being worn
            worn,
            hoveringOtherObject,
            grabbingOtherObject
        }

        protected GrabberState state = GrabberState.free;

        public GrabberBehaviour currentlyHoveredGrabber;
        protected GrabbableBehaviour currentlyHoveredGrabbable;
        

        private void Awake()
        {
            CheckColliders();
        }

        public void Wear( Transform grabberPivot)
        {
            if (state == GrabberState.free)
            {
                grabberRiggidBody = gameObject.AddComponent<Rigidbody>();
                grabberRiggidBody.isKinematic = true;
                grabberRiggidBody.useGravity = false;
                state = GrabberState.worn;
                transform.SetParent(grabberPivot);
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                CheckColliders();
            }
        }

        public void UnWear()
        {
            if (state == GrabberState.worn)
            {
                Destroy(grabberRiggidBody);
                transform.SetParent(null);
                state = GrabberState.free;
                CheckColliders();
            }
        }

        public GrabberState GetState()
        {
            return state;
        }

        protected void TryGrabGrabber()
        {
            if (state == GrabberState.worn)
            {
                if (currentlyHoveredGrabber != null)
                {
                    OnNewGrabber(currentlyHoveredGrabber);
                }
            }
        }

        protected void TryGrabOtherObject()
        {
            if (state == GrabberState.hoveringOtherObject)
            {
                if (currentlyHoveredGrabbable != null && currentlyHoveredGrabbable.CanGrab() && state != GrabberState.grabbingOtherObject)
                {
                    state = GrabberState.grabbingOtherObject;
                    Transform handSnapPoint = isLeftHanded ? currentlyHoveredGrabbable.leftHandSnapPoint : currentlyHoveredGrabbable.rightHandSnapPoint;
                    currentlyHoveredGrabbable.transform.SetParent(snapPoint);

                    currentlyHoveredGrabbable.transform.rotation = snapPoint.rotation * Quaternion.Inverse(handSnapPoint.localRotation);
                    currentlyHoveredGrabbable.transform.position = snapPoint.position - currentlyHoveredGrabbable.transform.rotation * handSnapPoint.localPosition;
                }
            }
        }

        public void TryGrab()
        {
            switch (state)
            {

                case GrabberState.worn:
                    TryGrabGrabber();
                    break;
                case GrabberState.hoveringOtherObject:
                    TryGrabOtherObject();
                    break;
                default:
                    break;
            }
        }

        public void Release()
        {
            if (state == GrabberState.grabbingOtherObject)
            {
                currentlyHoveredGrabbable.transform.SetParent(null);
                state = GrabberState.worn;
            }            
        }

        protected void CheckColliders()
        {
            switch (state)
            {
                case GrabberState.free:
                    interactionCollider.SetActive(false);
                    grabCollider.SetActive(true);
                    break;
                case GrabberState.hovering:
                    break;
                case GrabberState.worn:
                    interactionCollider.SetActive(true);
                    grabCollider.SetActive(false);
                    break;
                default:
                    break;
            }
        }


        protected void CheckIsAnotherGrabber(GameObject other)
        {
            GrabberBehaviour otherGrabber = other.gameObject.GetComponentInParent<GrabberBehaviour>();
            if (otherGrabber != null && OnNewGrabber != null)
            {
                currentlyHoveredGrabber = otherGrabber;
            }
        }

        protected void CheckIsGrabbable(GameObject other)
        {
            GrabbableBehaviour grabbable = other.gameObject.GetComponentInParent<GrabbableBehaviour>();
            if (grabbable != null)
            {
                currentlyHoveredGrabbable = grabbable;
                state = GrabberState.hoveringOtherObject;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (state == GrabberState.worn)
            {
                CheckIsAnotherGrabber(other.gameObject);
                CheckIsGrabbable(other.gameObject);
            }

        }

        private void OnTriggerExit(Collider other)
        {
            if (currentlyHoveredGrabber != null)
            {
                GrabberBehaviour otherGrabber = other.gameObject.GetComponentInParent<GrabberBehaviour>();
                if (otherGrabber != null && currentlyHoveredGrabber == otherGrabber)
                {
                    currentlyHoveredGrabber = null;
                }
            }
        }

    }
}
