using System;

namespace CsharpExtras.Event.Wrapper
{
    /// <summary>
    /// The purpose of this class is to enforce every access to some object to raise some desired event.
    /// For example, suppose we have a tree datastructure and we want to maintain a running count of the number of nodes.
    /// Operations like add, remove will need to update this count. By putting the raw tree (without count) behind an event wrapper,
    /// and using an int as the event type, we force any accesses to the raw tree to provide some report on the count - either a delta or absolute value.
    /// This makes it much more unlikely for someone to accidentally update the tree and forget to update count.
    /// </summary>
    /// <typeparam name="TObj">The object that we are storing privately i.e. encapsulating</typeparam>
    /// <typeparam name="TEvent">The event type that must be returned after any access to the object</typeparam>
    public interface IEventObjWrapper<TObj, TEvent>
    {
        /// <summary>
        /// This method runs the specified function on the encapsulated object.
        /// </summary>
        /// <param name="action">The action to run. 
        /// Every action we perform on the object must return an event object. A client can pass in whatever
        /// function they want here, so they can do anything they want with the object, but they are forced
        /// in each case to think about what event object to return in each case.</param>
        void Run(Func<TObj, TEvent> action);
    }
}