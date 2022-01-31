using System;

namespace CsharpExtras.Event.Wrapper
{
    /// <summary>
    /// Encapsulates access to some object, so that accessing that object is always preceded by a pre-access action
    /// </summary>
    /// <typeparam name="TObj"></typeparam>
    public interface IPreAccessWrapper<TObj>
    {
        /// <summary>
        /// Gets the result of applying some function to the object
        /// </summary>
        /// <typeparam name="TReturn">The return type of the function</typeparam>
        /// <param name="f">The function to apply</param>
        /// <returns>The result of the function</returns>
        TReturn Get<TReturn>(Func<TObj, TReturn> f);

        /// <summary>
        /// Runs the given action on the object
        /// </summary>
        /// <param name="act">The action to run on the object</param>
        void Run(Action<TObj> act);
    }
}