using System;
using System.Collections.Generic;

namespace ConsoleGame.World
{
    public static class Continents
    {
        private static readonly Dictionary<string, string[]> _areas = new()
        {
            {"A", new []{"B", "C", "D"}},
            {"B", new []{"A", "D"}},
            {"C", new []{"A"}},
            {"D", new []{"B"}},
            {"E", new []{"B"}},
        };

        public static Dictionary<string, string[]> GetOptions => _areas;
    }
}