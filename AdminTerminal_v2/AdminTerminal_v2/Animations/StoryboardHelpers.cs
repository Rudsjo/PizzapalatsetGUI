using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media.Animation;

namespace AdminTerminal_v2
{
    public static class StoryboardHelpers
    {
        /// <summary>
        /// Adds a fade in animation to the storyboard
        /// </summary>
        /// <param name="storyboard">The storyboard to add the animation to</param>
        /// <param name="seconds">The duration of the animation</param>
        public static void AddFadeIn(this Storyboard storyboard, float seconds)
        {
            // Creates the fade animation
            var animation = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(seconds)),
                From = 0,
                To = 1
            };

            // Set the target property name
            Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity"));

            // Add this to the storyboard
            storyboard.Children.Add(animation);
        }

        /// <summary>
        /// Adds a fade out animation to the storyboard
        /// </summary>
        /// <param name="storyboard">The storyboard that the animation is added to</param>
        /// <param name="seconds">The duration of the animation</param>
        public static void AddFadeOut(this Storyboard storyboard, float seconds)
        {
            // Creates the fade animation
            var animation = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(seconds)),
                From = 1,
                To = 0
            };

            // Set the target property name
            Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity"));

            // Add this to the storyboard
            storyboard.Children.Add(animation);
        }
    }
}
