using Kore.Coroutines;
using Kore.Utils;
using Svelto.WeakEvents;
using System.Collections;

namespace Game
{
    /// <summary>
    /// Interface for a submission scheduler
    /// </summary>
    public interface ISubmissionScheduler
    {
        /// <summary>
        /// Must provide a neverending task
        /// </summary>
        /// <returns>the enumerator for the task</returns>
        IEnumerator ScheduleTask();

        void Schedule( WeakAction submitEntityViews);
    }

    /// <summary>
    /// Scheduler implementing the interface, we make it a singleton through interface, same
    /// as <see cref="UnitySumbmissionEntityViewScheduler"/>
    /// </summary>
    public class EntitySubmissionScheduler : SceneScopedSingletonI< EntitySubmissionScheduler, ISubmissionScheduler>, ISubmissionScheduler
    {
        public IEnumerator ScheduleTask()
        {
            while (true)
            {
                yield return null;

                if (OnTick!= null && OnTick.IsValid)
                    OnTick.Invoke();

                if(OnTick!=null && !OnTick.IsValid)
                    yield break;
            }
        }

        public void Schedule( WeakAction submitEntityViews)
        {
            OnTick = submitEntityViews;
        }

        private WeakAction OnTick;
    }

    /// <summary>
    /// Concrete class implementing the svelto <see cref="Svelto.ECS.Schedulers.EntitySubmissionScheduler"/> running the task
    /// </summary>
    public class GameSubmissionScheduler : Svelto.ECS.Schedulers.EntitySubmissionScheduler
    {
        public GameSubmissionScheduler()
        {
            Koroutine.Run( EntitySubmissionScheduler.Instance.ScheduleTask(), Method.LateUpdate);
        }

        public override void Schedule( WeakAction callback)
        {
            EntitySubmissionScheduler.Instance.Schedule( callback);
        }
    }
}