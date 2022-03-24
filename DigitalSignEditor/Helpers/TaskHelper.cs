using System.Threading.Tasks;

namespace DigitalSignEditor.Helpers {

    public static class TaskHelper {

        public static void Forget(this Task task) {
            if (!task.IsCompleted || task.IsFaulted) {
                _ = ForgetAwaited(task);
            }

            async static Task ForgetAwaited(Task task) {
                try {
                    await task.ConfigureAwait(false);
                } catch {
                }
            }
        }
    }
}