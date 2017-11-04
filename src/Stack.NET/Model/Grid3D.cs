﻿using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Stack.NET.Utility;

namespace Stack.NET.Model
{
    public sealed class Grid3D
    {
        public Grid3D()
        {
            Cubes = new List<Cube>();
        }

        public Color Surface { get; set; }
        public List<Cube> Cubes { get; set; }

        public double Segment { get; set; }
        public double Length { get; set; }
        public double HalfLength => Length / 2.0D;

        public void Place(int x, int y, int z, Color color)
        {
            var point = new Index3D(x, y, z);
            var index = Cubes.FindIndex(p => p.Position.Equals(point));

            if (index == -1)
            {
                var cube = new Cube(new Index3D(x, y, z), color);
                Cubes.Add(cube);
            }
            else
            {
                Cubes[index].Surface = color;
            }
        }

        public void Destroy(int x, int y, int z)
        {
            var point = new Index3D(x, y, z);
            var index = Cubes.FindIndex(p => p.Position.Equals(point));

            if (index != -1)
                Cubes.RemoveAt(index);
        }

        public void Range(out Index3D min, out Index3D max)
        {
            var vmax = new Index3D(int.MinValue, int.MinValue, int.MinValue);
            var vmin = new Index3D(int.MaxValue, int.MaxValue, int.MaxValue);

            foreach (var index in Cubes)
            {
                vmin = Index3D.Min(vmin, index.Position);
                vmax = Index3D.Max(vmax, index.Position);
            }

            min = vmin;
            max = vmax;
        }

        public Index3D Maximum()
        {
            var vmax = new Index3D(int.MinValue, int.MinValue, int.MinValue);
            return Cubes.Aggregate(vmax, (current, index) => Index3D.Max(current, index.Position));
        }

        public Index3D Minimum()
        {
            var vmin = new Index3D(int.MaxValue, int.MaxValue, int.MaxValue);
            return Cubes.Aggregate(vmin, (current, index) => Index3D.Min(current, index.Position));
        }

        public Point3D Center()
        {
            Range(out var min, out var max);

            var center = new Point3D(
                MathHelper.Center(min.X, max.X + 1),
                MathHelper.Center(min.Y, max.Y + 1),
                MathHelper.Center(min.Z, max.Z + 1));

            return new Point3D
            {
                X = center.X * Segment,
                Y = center.Y * Segment,
                Z = center.Z * Segment
            };
        }

        public Point3D Position(Index3D position)
        {
            return new Point3D
            {
                X = position.X * Segment + (Segment - Length) / 2.0D,
                Y = position.Y * Segment,
                Z = position.Z * Segment + (Segment - Length) / 2.0D
            };
        }
    }
}