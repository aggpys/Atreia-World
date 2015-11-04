using System;
using System.Windows.Forms;

namespace Rift.Utils
{
    /// <summary>
    /// Represents the <see cref="System.Windows.Forms.Form"/> extension methods.
    /// </summary>
    public static class FormExtensions
    {
        /// <summary>
        /// Invokes the specified action if required.
        /// </summary>
        /// <typeparam name="T"><see cref="System.Windows.Forms.Form"/></typeparam>
        /// <param name="form">The <see cref="System.Windows.Forms.Form"/> to apply an action specified.</param>
        /// <param name="action">An action to invoke.</param>
        public static void InvokeAction<T>(this T form, Action<T> action) where T : Form
        {
            if (form.InvokeRequired)
                form.Invoke(new Action(() => action(form)));
            else
                action(form);
        }

        /// <summary>
        /// Invokes the specified action if required.
        /// </summary>
        /// <typeparam name="T"><see cref="System.Windows.Forms.Form"/></typeparam>
        /// <param name="form">The <see cref="System.Windows.Forms.Form"/> to apply an action specified.</param>
        /// <param name="action">An action to invoke.</param>
        public static void InvokeAction<T>(this T form, Action action) where T : Form
        {
            if (form.InvokeRequired)
                form.Invoke(action);
            else
                action();
        }
    }
}