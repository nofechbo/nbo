using System;

namespace MyFactory
{
    // Complex object for testing
    class ComplexObject
    {
        private readonly string description;
        private readonly int size;

        public ComplexObject(string description, int size)
        {
            this.description = description;
            this.size = size;
        }

        public override string ToString()
        {
            return $"ComplexObject{{description = \"{description}\", size = {size}}}";
        }
    }
}
