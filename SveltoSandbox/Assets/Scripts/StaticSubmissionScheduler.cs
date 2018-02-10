using Kore.Utils;
using Svelto.WeakEvents;
using System.Collections;

namespace Game
{
    /// <summary>
    /// Scheduler implementing my <see cref="ISubmissionScheduler"/> interface, we make it a singleton through interface, same
    /// as <see cref="UnitySumbmissionEntityViewScheduler"/> but more clean thanks to Scene Scoped Singletons.
    /// To get a reference to it we just need to call <see cref="StaticSubmissionScheduler.Instance"/> and a GameObject with
    /// this object will be generated
    /// </summary>
    public class StaticSubmissionScheduler :    SceneScopedSingletonI< StaticSubmissionScheduler, ISubmissionScheduler>
                                            ,   ISubmissionScheduler
    {
        public IEnumerator ScheduleTask()
        {
            while (true)
            {
                yield return null;

                if (OnTick != null && OnTick.IsValid)
                    OnTick.Invoke();

                if (OnTick != null && !OnTick.IsValid)
                    yield break;
            }
        }

        public void Schedule( WeakAction submitEntityViews)
        {
            OnTick = submitEntityViews;
        }


        public override void Init()
        {
            // Instance created
        }

        public override void OnDestroyCalled()
        {
            // Scene unloaded
        }

        private WeakAction OnTick;
    }
}