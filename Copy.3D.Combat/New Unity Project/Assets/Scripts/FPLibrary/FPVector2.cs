﻿#region License

/*
MIT License
Copyright © 2006 The Mono.Xna Team

All rights reserved.

Authors
 * Alan McGovern

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

#endregion License

using System;

namespace FPLibrary {

    [Serializable]
    public struct FPVector2 : IEquatable<FPVector2>
    {
#region Private Fields

        private static FPVector2 zeroVector = new FPVector2(0, 0);
        private static FPVector2 oneVector = new FPVector2(1, 1);

        private static FPVector2 rightVector = new FPVector2(1, 0);
        private static FPVector2 leftVector = new FPVector2(-1, 0);

        private static FPVector2 upVector = new FPVector2(0, 1);
        private static FPVector2 downVector = new FPVector2(0, -1);

        #endregion Private Fields

        #region Public Fields

        public Fix64 x;
        public Fix64 y;

        #endregion Public Fields

#region Properties

        public static FPVector2 zero
        {
            get { return zeroVector; }
        }

        public static FPVector2 one
        {
            get { return oneVector; }
        }

        public static FPVector2 right
        {
            get { return rightVector; }
        }

        public static FPVector2 left {
            get { return leftVector; }
        }

        public static FPVector2 up
        {
            get { return upVector; }
        }

        public static FPVector2 down {
            get { return downVector; }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Constructor foe standard 2D vector.
        /// </summary>
        /// <param name="x">
        /// A <see cref="System.Single"/>
        /// </param>
        /// <param name="y">
        /// A <see cref="System.Single"/>
        /// </param>
        public FPVector2(Fix64 x, Fix64 y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Constructor for "square" vector.
        /// </summary>
        /// <param name="value">
        /// A <see cref="System.Single"/>
        /// </param>
        public FPVector2(Fix64 value)
        {
            x = value;
            y = value;
        }

        public void Set(Fix64 x, Fix64 y) {
            this.x = x;
            this.y = y;
        }

        #endregion Constructors

        #region Public Methods

        public static void Reflect(ref FPVector2 vector, ref FPVector2 normal, out FPVector2 result)
        {
            Fix64 dot = Dot(vector, normal);
            result.x = vector.x - ((2f*dot)*normal.x);
            result.y = vector.y - ((2f*dot)*normal.y);
        }

        public static FPVector2 Reflect(FPVector2 vector, FPVector2 normal)
        {
            FPVector2 result;
            Reflect(ref vector, ref normal, out result);
            return result;
        }

        public static FPVector2 Add(FPVector2 value1, FPVector2 value2)
        {
            value1.x += value2.x;
            value1.y += value2.y;
            return value1;
        }

        public static void Add(ref FPVector2 value1, ref FPVector2 value2, out FPVector2 result)
        {
            result.x = value1.x + value2.x;
            result.y = value1.y + value2.y;
        }

        public static FPVector2 Barycentric(FPVector2 value1, FPVector2 value2, FPVector2 value3, Fix64 amount1, Fix64 amount2)
        {
            return new FPVector2(
                FPMath.Barycentric(value1.x, value2.x, value3.x, amount1, amount2),
                FPMath.Barycentric(value1.y, value2.y, value3.y, amount1, amount2));
        }

        public static void Barycentric(ref FPVector2 value1, ref FPVector2 value2, ref FPVector2 value3, Fix64 amount1,
                                       Fix64 amount2, out FPVector2 result)
        {
            result = new FPVector2(
                FPMath.Barycentric(value1.x, value2.x, value3.x, amount1, amount2),
                FPMath.Barycentric(value1.y, value2.y, value3.y, amount1, amount2));
        }

        public static FPVector2 CatmullRom(FPVector2 value1, FPVector2 value2, FPVector2 value3, FPVector2 value4, Fix64 amount)
        {
            return new FPVector2(
                FPMath.CatmullRom(value1.x, value2.x, value3.x, value4.x, amount),
                FPMath.CatmullRom(value1.y, value2.y, value3.y, value4.y, amount));
        }

        public static void CatmullRom(ref FPVector2 value1, ref FPVector2 value2, ref FPVector2 value3, ref FPVector2 value4,
                                      Fix64 amount, out FPVector2 result)
        {
            result = new FPVector2(
                FPMath.CatmullRom(value1.x, value2.x, value3.x, value4.x, amount),
                FPMath.CatmullRom(value1.y, value2.y, value3.y, value4.y, amount));
        }

        public static FPVector2 Clamp(FPVector2 value1, FPVector2 min, FPVector2 max)
        {
            return new FPVector2(
                FPMath.Clamp(value1.x, min.x, max.x),
                FPMath.Clamp(value1.y, min.y, max.y));
        }

        public static void Clamp(ref FPVector2 value1, ref FPVector2 min, ref FPVector2 max, out FPVector2 result)
        {
            result = new FPVector2(
                FPMath.Clamp(value1.x, min.x, max.x),
                FPMath.Clamp(value1.y, min.y, max.y));
        }

        /// <summary>
        /// Returns FP precison distanve between two vectors
        /// </summary>
        /// <param name="value1">
        /// A <see cref="FPVector2"/>
        /// </param>
        /// <param name="value2">
        /// A <see cref="FPVector2"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.Single"/>
        /// </returns>
        public static Fix64 Distance(FPVector2 value1, FPVector2 value2)
        {
            Fix64 result;
            DistanceSquared(ref value1, ref value2, out result);
            return (Fix64) Fix64.Sqrt(result);
        }


        public static void Distance(ref FPVector2 value1, ref FPVector2 value2, out Fix64 result)
        {
            DistanceSquared(ref value1, ref value2, out result);
            result = (Fix64) Fix64.Sqrt(result);
        }

        public static Fix64 DistanceSquared(FPVector2 value1, FPVector2 value2)
        {
            Fix64 result;
            DistanceSquared(ref value1, ref value2, out result);
            return result;
        }

        public static void DistanceSquared(ref FPVector2 value1, ref FPVector2 value2, out Fix64 result)
        {
            result = (value1.x - value2.x)*(value1.x - value2.x) + (value1.y - value2.y)*(value1.y - value2.y);
        }

        /// <summary>
        /// Devide first vector with the secund vector
        /// </summary>
        /// <param name="value1">
        /// A <see cref="FPVector2"/>
        /// </param>
        /// <param name="value2">
        /// A <see cref="FPVector2"/>
        /// </param>
        /// <returns>
        /// A <see cref="FPVector2"/>
        /// </returns>
        public static FPVector2 Divide(FPVector2 value1, FPVector2 value2)
        {
            value1.x /= value2.x;
            value1.y /= value2.y;
            return value1;
        }

        public static void Divide(ref FPVector2 value1, ref FPVector2 value2, out FPVector2 result)
        {
            result.x = value1.x/value2.x;
            result.y = value1.y/value2.y;
        }

        public static FPVector2 Divide(FPVector2 value1, Fix64 divider)
        {
            Fix64 factor = 1/divider;
            value1.x *= factor;
            value1.y *= factor;
            return value1;
        }

        public static void Divide(ref FPVector2 value1, Fix64 divider, out FPVector2 result)
        {
            Fix64 factor = 1/divider;
            result.x = value1.x*factor;
            result.y = value1.y*factor;
        }

        public static Fix64 Dot(FPVector2 value1, FPVector2 value2)
        {
            return value1.x*value2.x + value1.y*value2.y;
        }

        public static void Dot(ref FPVector2 value1, ref FPVector2 value2, out Fix64 result)
        {
            result = value1.x*value2.x + value1.y*value2.y;
        }

        public override bool Equals(object obj)
        {
            return (obj is FPVector2) ? this == ((FPVector2) obj) : false;
        }

        public bool Equals(FPVector2 other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            return (int) (x + y);
        }

        public static FPVector2 Hermite(FPVector2 value1, FPVector2 tangent1, FPVector2 value2, FPVector2 tangent2, Fix64 amount)
        {
            FPVector2 result = new FPVector2();
            Hermite(ref value1, ref tangent1, ref value2, ref tangent2, amount, out result);
            return result;
        }

        public static void Hermite(ref FPVector2 value1, ref FPVector2 tangent1, ref FPVector2 value2, ref FPVector2 tangent2,
                                   Fix64 amount, out FPVector2 result)
        {
            result.x = FPMath.Hermite(value1.x, tangent1.x, value2.x, tangent2.x, amount);
            result.y = FPMath.Hermite(value1.y, tangent1.y, value2.y, tangent2.y, amount);
        }

        public Fix64 magnitude {
            get {
                Fix64 result;
                DistanceSquared(ref this, ref zeroVector, out result);
                return Fix64.Sqrt(result);
            }
        }

        public static FPVector2 ClampMagnitude(FPVector2 vector, Fix64 maxLength) {
            return Normalize(vector) * maxLength;
        }

        public Fix64 LengthSquared()
        {
            Fix64 result;
            DistanceSquared(ref this, ref zeroVector, out result);
            return result;
        }

        public static FPVector2 Lerp(FPVector2 value1, FPVector2 value2, Fix64 amount) {
            amount = FPMath.Clamp(amount, 0, 1);

            return new FPVector2(
                FPMath.Lerp(value1.x, value2.x, amount),
                FPMath.Lerp(value1.y, value2.y, amount));
        }

        public static FPVector2 LerpUnclamped(FPVector2 value1, FPVector2 value2, Fix64 amount)
        {
            return new FPVector2(
                FPMath.Lerp(value1.x, value2.x, amount),
                FPMath.Lerp(value1.y, value2.y, amount));
        }

        public static void LerpUnclamped(ref FPVector2 value1, ref FPVector2 value2, Fix64 amount, out FPVector2 result)
        {
            result = new FPVector2(
                FPMath.Lerp(value1.x, value2.x, amount),
                FPMath.Lerp(value1.y, value2.y, amount));
        }

        public static FPVector2 Max(FPVector2 value1, FPVector2 value2)
        {
            return new FPVector2(
                FPMath.Max(value1.x, value2.x),
                FPMath.Max(value1.y, value2.y));
        }

        public static void Max(ref FPVector2 value1, ref FPVector2 value2, out FPVector2 result)
        {
            result.x = FPMath.Max(value1.x, value2.x);
            result.y = FPMath.Max(value1.y, value2.y);
        }

        public static FPVector2 Min(FPVector2 value1, FPVector2 value2)
        {
            return new FPVector2(
                FPMath.Min(value1.x, value2.x),
                FPMath.Min(value1.y, value2.y));
        }

        public static void Min(ref FPVector2 value1, ref FPVector2 value2, out FPVector2 result)
        {
            result.x = FPMath.Min(value1.x, value2.x);
            result.y = FPMath.Min(value1.y, value2.y);
        }

        public void Scale(FPVector2 other) {
            this.x = x * other.x;
            this.y = y * other.y;
        }

        public static FPVector2 Scale(FPVector2 value1, FPVector2 value2) {
            FPVector2 result;
            result.x = value1.x * value2.x;
            result.y = value1.y * value2.y;

            return result;
        }

        public static FPVector2 Multiply(FPVector2 value1, FPVector2 value2)
        {
            value1.x *= value2.x;
            value1.y *= value2.y;
            return value1;
        }

        public static FPVector2 Multiply(FPVector2 value1, Fix64 scaleFactor)
        {
            value1.x *= scaleFactor;
            value1.y *= scaleFactor;
            return value1;
        }

        public static void Multiply(ref FPVector2 value1, Fix64 scaleFactor, out FPVector2 result)
        {
            result.x = value1.x*scaleFactor;
            result.y = value1.y*scaleFactor;
        }

        public static void Multiply(ref FPVector2 value1, ref FPVector2 value2, out FPVector2 result)
        {
            result.x = value1.x*value2.x;
            result.y = value1.y*value2.y;
        }

        public static FPVector2 Negate(FPVector2 value)
        {
            value.x = -value.x;
            value.y = -value.y;
            return value;
        }

        public static void Negate(ref FPVector2 value, out FPVector2 result)
        {
            result.x = -value.x;
            result.y = -value.y;
        }

        public void Normalize()
        {
            Normalize(ref this, out this);
        }

        public static FPVector2 Normalize(FPVector2 value)
        {
            Normalize(ref value, out value);
            return value;
        }

        public FPVector2 normalized {
            get {
                FPVector2 result;
                FPVector2.Normalize(ref this, out result);

                return result;
            }
        }

        public static void Normalize(ref FPVector2 value, out FPVector2 result)
        {
            Fix64 factor;
            DistanceSquared(ref value, ref zeroVector, out factor);
            factor = 1f/(Fix64) Fix64.Sqrt(factor);
            result.x = value.x*factor;
            result.y = value.y*factor;
        }

        public static FPVector2 SmoothStep(FPVector2 value1, FPVector2 value2, Fix64 amount)
        {
            return new FPVector2(
                FPMath.SmoothStep(value1.x, value2.x, amount),
                FPMath.SmoothStep(value1.y, value2.y, amount));
        }

        public static void SmoothStep(ref FPVector2 value1, ref FPVector2 value2, Fix64 amount, out FPVector2 result)
        {
            result = new FPVector2(
                FPMath.SmoothStep(value1.x, value2.x, amount),
                FPMath.SmoothStep(value1.y, value2.y, amount));
        }

        public static FPVector2 Subtract(FPVector2 value1, FPVector2 value2)
        {
            value1.x -= value2.x;
            value1.y -= value2.y;
            return value1;
        }

        public static void Subtract(ref FPVector2 value1, ref FPVector2 value2, out FPVector2 result)
        {
            result.x = value1.x - value2.x;
            result.y = value1.y - value2.y;
        }

        public static Fix64 Angle(FPVector2 a, FPVector2 b) {
            return Fix64.Acos(a.normalized * b.normalized) * Fix64.Rad2Deg;
        }

        public FPVector ToFPVector() {
            return new FPVector(this.x, this.y, 0);
        }

        public override string ToString() {
            return string.Format("({0:f1}, {1:f1})", x.AsFloat(), y.AsFloat());
        }

        #endregion Public Methods

#region Operators

        public static FPVector2 operator -(FPVector2 value)
        {
            value.x = -value.x;
            value.y = -value.y;
            return value;
        }


        public static bool operator ==(FPVector2 value1, FPVector2 value2)
        {
            return value1.x == value2.x && value1.y == value2.y;
        }


        public static bool operator !=(FPVector2 value1, FPVector2 value2)
        {
            return value1.x != value2.x || value1.y != value2.y;
        }


        public static FPVector2 operator +(FPVector2 value1, FPVector2 value2)
        {
            value1.x += value2.x;
            value1.y += value2.y;
            return value1;
        }


        public static FPVector2 operator -(FPVector2 value1, FPVector2 value2)
        {
            value1.x -= value2.x;
            value1.y -= value2.y;
            return value1;
        }


        public static Fix64 operator *(FPVector2 value1, FPVector2 value2)
        {
            return FPVector2.Dot(value1, value2);
        }


        public static FPVector2 operator *(FPVector2 value, Fix64 scaleFactor)
        {
            value.x *= scaleFactor;
            value.y *= scaleFactor;
            return value;
        }


        public static FPVector2 operator *(Fix64 scaleFactor, FPVector2 value)
        {
            value.x *= scaleFactor;
            value.y *= scaleFactor;
            return value;
        }


        public static FPVector2 operator /(FPVector2 value1, FPVector2 value2)
        {
            value1.x /= value2.x;
            value1.y /= value2.y;
            return value1;
        }


        public static FPVector2 operator /(FPVector2 value1, Fix64 divider)
        {
            Fix64 factor = 1/divider;
            value1.x *= factor;
            value1.y *= factor;
            return value1;
        }

        #endregion Operators
    }
}