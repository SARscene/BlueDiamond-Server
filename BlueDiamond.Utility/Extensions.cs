using System.ComponentModel;
using System.Windows.Forms;

namespace BlueDiamond.Utility
{
    public static class Extensions
    {
        /// <summary>
        /// This is used to "invoke" from a thread to a UI thread when updating the UI
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="action"></param>
        public static void InvokeIfRequired(this ISynchronizeInvoke obj, MethodInvoker action)
        {
            if (obj.InvokeRequired)
            {
                var args = new object[0];
                obj.Invoke(action, args);
            }
            else
            {
                action();
            }
        }

    }
}
