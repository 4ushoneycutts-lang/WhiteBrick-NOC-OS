using System.Collections.Generic;
using Avalonia;

namespace WhiteBrick.NOC.Utilities
{
    // Centralized registry for layout rectangles used by calibration and (future) widgets.
    public static class LayoutRegistry
    {
        private static readonly Dictionary<string, Rect> _rects = new();

        public static void Set(string key, Rect rect)
        {
            lock (_rects)
            {
                _rects[key] = rect;
            }
        }

        public static Rect Get(string key)
        {
            lock (_rects)
            {
                return _rects.TryGetValue(key, out var v) ? v : new Rect(0, 0, 0, 0);
            }
        }

        public static IReadOnlyDictionary<string, Rect> All
        {
            get
            {
                lock (_rects)
                {
                    return new Dictionary<string, Rect>(_rects);
                }
            }
        }
    }
}
