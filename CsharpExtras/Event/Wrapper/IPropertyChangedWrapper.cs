using System;

namespace CsharpExtras.Event.Wrapper
{
    /// <summary>
    /// The purpose of this type is to enforce event notification whenever some property on an object changes.
    /// The type can be thought of as a wrapper type, wrapping some object.
    /// Access is restricted to the wrapped object so that for any operations performed on it, a propety changed event is raised
    /// Note: this class doesn't strictly enforce that an event is raised every time the object's state is mutated.
    /// Rather, it helps an architect easily ensure such an event is always raised, provided interaction with the object always goes through the wrapper.
    /// It is up to architects using this class to ensure that they don't create any "side-doors" allowing to mutate the state of the wrapped object
    /// For example, it would be bad pratice to return a reference to the wrapped object as part of the Get method because then that reference could be mutated, without any events raised.
    /// </summary>
    /// <typeparam name="TObj">The type of the object being wrapped</typeparam>
    /// <typeparam name="TEvent">The event type that should be raised whenever someone interacts with the object</typeparam>
    public interface IPropertyChangedWrapper<TObj, TEvent>
    {
        /// <summary>
        /// This event is raised whenever someone interacet with the given object through this class
        /// </summary>
        event Action<TEvent>? OnPropertyChanged;

        /// <summary>
        /// Runs the given action on the object and raises an event capturing how the object's property changed before/after running the action        
        /// </summary>
        /// <param name="act">The action to run</param>
        void Run(Action<TObj> act);

        /// <summary>
        /// Runs the given function on the object and raises an event capturing how the object's property changed before/after running the function
        /// </summary>
        /// <typeparam name="TReturn">The return type of the function</typeparam>
        /// <param name="f">The function to run</param>
        /// <returns>The result of applying the function to the object</returns>
        TReturn Get<TReturn>(Func<TObj, TReturn> f);
    }
}