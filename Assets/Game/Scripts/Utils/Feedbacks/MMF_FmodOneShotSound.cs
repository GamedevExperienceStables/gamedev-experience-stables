using FMODUnity;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Game.Utils.Feedbacks
{
    [ExecuteAlways]
    [AddComponentMenu("")]
    [FeedbackPath("Fmod/One Shot Sound")]
    public class MMF_FmodOneShotSound : MMF_Feedback
    {
        /// a static bool used to disable all feedbacks of this type at once
        public static bool FeedbackTypeAuthorized = true;
        
        [MMFInspectorGroup("Sound", true, 14, true)]
        /// the sound clip to play
        [Tooltip("the sound clip to play")]
        public EventReference eventReference;
        
        protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1)
        {
            if (!Active || !FeedbackTypeAuthorized)
                return;

            if (!eventReference.IsNull)
                RuntimeManager.PlayOneShot(eventReference, position);
        }
    }
}