using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace CookingTerminal
{
    /// <summary>
    /// A base attached property to fulfill all the basic parts to make it easier to make new attached properties
    /// </summary>
    /// <typeparam name="Parent">The Parent class to be the attached property</typeparam>
    /// <typeparam name="Property">The type of the attached property</typeparam>
    public class BaseAttachedProperty<Parent, Property>
        where Parent : BaseAttachedProperty<Parent, Property>, new()
    {
        /*
         * The result of the attached property when inherited:
         * 
         * public Property Value { get; set; }
         * 
         */

        #region Public Events

        /// <summary>
        /// Event that fires when a value in the property changes
        /// </summary>
        public event Action<DependencyObject, DependencyPropertyChangedEventArgs> ValueChanged = (sender, e) => { };

        #endregion

        #region Public Properties

        /// <summary>
        /// A singleton instance of the parent class
        /// </summary>
        public static Parent Instance { get; private set; } = new Parent();

        #endregion

        #region Attached Property Definitions

        /// <summary>
        /// The attached property
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.RegisterAttached("Value", typeof(Property),
            typeof(BaseAttachedProperty<Parent, Property>), new PropertyMetadata(new PropertyChangedCallback(OnValuePropertyChanged)));

        
        /// <summary>
        /// The fallback event when the <see cref="ValueProperty"/> is changed
        /// </summary>
        /// <param name="d">The UI element that had it's property changed</param>
        /// <param name="e">The argument for the event</param>
        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Call the parent function
                // Notifies that the value in the parent has been changed
            Instance.OnValueChanged(d, e);

            // Call the event listeners
                // Notifies all elements having this property
            Instance.ValueChanged(d, e);

        }

        /// <summary>
        /// Gets the value of the attached property
        /// </summary>
        /// <param name="d">The element to get the property from</param>
        /// <returns></returns>
        public static Property GetValue(DependencyObject d) => (Property)d.GetValue(ValueProperty);

        /// <summary>
        /// Sets the value of the attached property
        /// </summary>
        /// <param name="d">The element that has the property</param>
        /// <param name="value">The value to set to the property</param>
        public static void SetValue(DependencyObject d, Property value) => d.SetValue(ValueProperty, value);

        #endregion

        #region Event Methods

        /// <summary>
        /// The method that is called when any of its type is changed
        /// </summary>
        /// <param name="sender">The UI element that this property was changed on</param>
        /// <param name="e">The arguments for the event</param>
        public virtual void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) { }

        #endregion

    }
}
