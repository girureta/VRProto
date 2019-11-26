using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRProto
{
    public class GrabbableBehaviour : MonoBehaviour
    {
        public Transform leftHandSnapPoint;
        public Transform rightHandSnapPoint;

        public GameObject grabCollider;

        protected GrabberBehaviour currentHoverinGrabber;

        public enum GrabbableState
        {
            free,
            hovering,
            grabbed
        }

        protected GrabbableState state = GrabbableState.free;

        public GrabbableState GetState()
        {
            return state;
        }

        public bool CanGrab()
        {
            return state == GrabbableState.hovering;
        }

        public void Grab()
        {
            if (state == GrabbableState.hovering)
            {
                state = GrabbableState.grabbed;
                CheckCollider();
            }
        }

        protected void CheckCollider()
        {
            switch (state)
            {
                case GrabbableState.free:
                     grabCollider.SetActive(true);
                    break;
                case GrabbableState.hovering:
                    break;
                case GrabbableState.grabbed:
                    grabCollider.SetActive(false);
                    break;
                default:
                    break;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (state == GrabbableState.free)
            {
                var grabber = other.gameObject.GetComponentInParent<GrabberBehaviour>();
                if (grabber != null)
                {
                    state = GrabbableState.hovering;
                    currentHoverinGrabber = grabber;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (currentHoverinGrabber != null && state == GrabbableState.hovering)
            {
                GrabberBehaviour otherGrabber = other.gameObject.GetComponentInParent<GrabberBehaviour>();
                if (otherGrabber != null && currentHoverinGrabber == otherGrabber)
                {
                    currentHoverinGrabber = null;
                    state = GrabbableState.free;
                }
            }
        }
    }
}
