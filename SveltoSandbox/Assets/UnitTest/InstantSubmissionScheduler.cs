using Game;
using Svelto.WeakEvents;
using System.Collections;

namespace GameUnitTest
{
    /// <summary>
    /// This scheduler is intented only for Unit testing purpose as it allows to schedule entity views instantly
    /// </summary>
    public class InstantSubmissionScheduler : ISubmissionScheduler
    {
        public IEnumerator ScheduleTask()
        {
            yield break;
        }

        public void Schedule( WeakAction submitEntityViews)
        {
            OnTick = submitEntityViews;
        }

        public void SubmitNow()
        {
            OnTick.Invoke();
        }

        private WeakAction OnTick;
    }
}