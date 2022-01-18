using System;

namespace CsharpExtras.Event.Notify
{
    /// <summary>
    /// Encapsulates a value and an update operation, notifying listeners of before/after update events.
    /// The purpose of doing so is to ensure that all updates will result in corresponding event notifications.
    /// Note: the before/after events do not report on the current value, but any client can just get this via the value property
    /// Note: this class has not been veriried as thread safe
    /// </summary>
    /// <typeparam name="TUpdate">The argument type of the update function.
    /// Note: a tuple type can be used if the underlying function needs multipler arguments.</typeparam>
    /// <typeparam name="TVal">The type of the value stored within this class</typeparam>
    public interface IUpdateNotifier<TVal, TUpdate>
    {
        /// <summary>
        /// The value stored in the class
        /// </summary>
        TVal Value { get; }

        /// <summary>
        /// This event is triggered before an update occurs
        /// </summary>
        event Action<TUpdate>? AfterUpdate;

        /// <summary>
        /// This event is triggered after an update occurs
        /// </summary>
        event Action<TUpdate>? BeforeUpdate;

        /// <summary>
        /// Calling this function will call the underlying update function & trigger the before/after events
        /// </summary>
        /// <param name="update">Value to pass to the underlying update function</param>
        void Update(TUpdate update);
    }
}