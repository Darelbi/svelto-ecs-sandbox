using Kore.Coroutines;
using Svelto.ECS.Schedulers;
using Svelto.WeakEvents;
using System.Collections;

namespace Game
{
    /// <summary>
    /// My game can work with any Submission scheduler that implement this interface
    /// </summary>
    public interface ISubmissionScheduler
    {
        /// <summary>
        /// Must provide a neverending task
        /// </summary>
        /// <returns>the enumerator for the task</returns>
        IEnumerator ScheduleTask();

        /// <summary>
        /// Called by svelto to inject the needed logic
        /// </summary>
        /// <param name="submitEntityViews"></param>
        void Schedule( WeakAction submitEntityViews);

        /// <summary>
        /// Used to call submission manually
        /// </summary>
        void SubmitNow();
    }

    /// <summary>
    /// This class is an adapter, it takes my <see cref="ISubmissionScheduler"/> scheduler and adapt it
    /// to work inside Svelto.ECS because this class implements <see cref="EntitySubmissionScheduler"/>
    /// </summary>
    public class GameSubmissionScheduler : EntitySubmissionScheduler
    {
        private readonly ISubmissionScheduler scheduler;

        public GameSubmissionScheduler( ISubmissionScheduler scheduler)
        {
            this.scheduler = scheduler;
            Koroutine.Run( scheduler.ScheduleTask(), Method.LateUpdate); //be sure this is the last thing executed in a frame
        }

        public override void Schedule( WeakAction callback)
        {
            scheduler.Schedule( callback);
        }
    }
}