using System;
using Microsoft.Xna.Framework;

namespace SpacePhysics.Debugging
{
    internal class DebugItem
    {
        public string Label { get; private set; }
        public Func<string> ValueGetter { get; private set; }
        public Vector2 position;

        public DebugItem(string label, Func<string> valueGetter)
        {
            Label = label;
            ValueGetter = valueGetter;
        }

        public string GetValue() => ValueGetter();
    }
}
