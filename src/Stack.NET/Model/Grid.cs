using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;
using Stack.NET.Utility;

namespace Stack.NET.Model
{
    /// <summary>Represents a loosely structured 3-dimensional grid.</summary>
    public sealed class Grid
    {
        private readonly Dictionary<Index3D, Cube> _cubes;
        private readonly WorldSystem _system;

        public Grid()
        {
            _cubes = new Dictionary<Index3D, Cube>();
            _system = new WorldSystem
            {
                Padding = new Padding(GridConstants.Spacing)
            };
        }

        public IReadOnlyCollection<KeyValuePair<Index3D, Cube>> Values => _cubes;
        public IReadOnlyCollection<Cube> Cubes => _cubes.Values;
        public IReadOnlyCollection<Index3D> Positions => _cubes.Keys;

        public double Segment { get; set; }

        public double Length
        {
            get => _system.Length;
            set => _system.Length = value;
        }

        /// <summary>Places a cube at the specified position.</summary>
        /// <param name="x">The x-component of the index.</param>
        /// <param name="y">The y-component of the index.</param>
        /// <param name="z">The z-component of the index.</param>
        /// <param name="cube">The cube color.</param>
        public void Place(int x, int y, int z, Cube cube)
        {
            Place(new Index3D(x, y, z), cube);
        }

        /// <summary>Places a cube at the specified position.</summary>
        /// <param name="position">The position.</param>
        /// <param name="cube">The cube.</param>
        public void Place(Index3D position, Cube cube)
        {
            if (_cubes.ContainsKey(position))
                _cubes[position] = cube;
            else
                _cubes.Add(position, cube);
        }

        /// <summary>Destroys a cube at the specified position.</summary>
        /// <param name="x">The x-component of the index.</param>
        /// <param name="y">The y-component of the index.</param>
        /// <param name="z">The z-component of the index.</param>
        public void Destroy(int x, int y, int z)
        {
            var point = new Index3D(x, y, z);
            _cubes.Remove(point);
        }

        /// <summary>Destroys a cube at the specified position.</summary>
        /// <param name="position">The position.</param>
        public void Destroy(Index3D position)
        {
            _cubes.Remove(position);
        }

        public void Range(out Index3D min, out Index3D max)
        {
            var vmax = new Index3D(int.MinValue, int.MinValue, int.MinValue);
            var vmin = new Index3D(int.MaxValue, int.MaxValue, int.MaxValue);

            foreach (var index in Positions)
            {
                vmin = Index3D.Min(vmin, index);
                vmax = Index3D.Max(vmax, index);
            }

            min = vmin;
            max = vmax;
        }

        public Index3D Maximum()
        {
            var vmax = new Index3D(int.MinValue, int.MinValue, int.MinValue);
            return Positions.Aggregate(vmax, Index3D.Max);
        }

        public Index3D Minimum()
        {
            var vmin = new Index3D(int.MaxValue, int.MaxValue, int.MaxValue);
            return Positions.Aggregate(vmin, Index3D.Min);
        }

        public Point3D Center()
        {
            Range(out var min, out var max);
            return _system.Center(min, max);
        }

        public Point3D Position(Index3D position)
        {
            return _system.Get(position);
        }
    }
}