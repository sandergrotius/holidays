using System;

namespace Holidays
{
    public static class ArgumentChecker
    {
        public static void IsFalse(bool validIfFalse, string message)
        {
            if (validIfFalse)
            {
                throw new ArgumentException(message);
            }
        }

        public static T NotNull<T>(T argument, string name)
        {
            if (argument == null)
            {
                throw new ArgumentNullException($"Argument '{name}' must not be null.");
            }

            return argument;
        }

        public static int NotNegativeOrZero(int argument, string name)
        {
            if (argument <= 0)
            {
                throw new ArgumentException($"Argument {name} should not be negative or zero but has value {argument}.");
            }

            return argument;
        }
    }
}
