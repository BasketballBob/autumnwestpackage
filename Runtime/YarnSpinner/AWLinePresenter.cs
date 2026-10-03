using UnityEngine;
using Yarn.Unity;

namespace AWP
{
    public class AWLinePresenter : DialoguePresenterBase
    {
        

        public override YarnTask OnDialogueStartedAsync()
        {
            return YarnTask.CompletedTask;
        }

        public override YarnTask OnDialogueCompleteAsync()
        {
            return YarnTask.CompletedTask;
        }

        public override YarnTask RunLineAsync(LocalizedLine line, LineCancellationToken token)
        {
            throw new System.NotImplementedException();
        }
    }
}
