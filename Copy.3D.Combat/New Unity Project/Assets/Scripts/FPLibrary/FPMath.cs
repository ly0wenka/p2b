﻿/* Copyright (C) <2009-2011> <Thorben Linneweber, Jitter Physics>
* 
*  This software is provided 'as-is', without any express or implied
*  warranty.  In no event will the authors be held liable for any damages
*  arising from the use of this software.
*
*  Permission is granted to anyone to use this software for any purpose,
*  including commercial applications, and to alter it and redistribute it
*  freely, subject to the following restrictions:
*
*  1. The origin of this software must not be misrepresented; you must not
*      claim that you wrote the original software. If you use this software
*      in a product, an acknowledgment in the product documentation would be
*      appreciated but is not required.
*  2. Altered source versions must be plainly marked as such, and must not be
*      misrepresented as being the original software.
*  3. This notice may not be removed or altered from any source distribution. 
*/

namespace FPLibrary {

    /// <summary>
    /// Contains common math operations.
    /// </summary>
    public sealed class FPMath {

        /// <summary>
        /// PI constant.
        /// </summary>
        public static Fix64 Pi = Fix64.Pi;

        /**
        *  @brief PI over 2 constant.
        **/
        public static Fix64 PiOver2 = Fix64.PiOver2;

        /// <summary>
        /// A small value often used to decide if numeric 
        /// results are zero.
        /// </summary>
		public static Fix64 Epsilon = Fix64.Epsilon;

        /**
        *  @brief Degree to radians constant.
        **/
        public static Fix64 Deg2Rad = Fix64.Deg2Rad;

        /**
        *  @brief Radians to degree constant.
        **/
        public static Fix64 Rad2Deg = Fix64.Rad2Deg;

        /// <summary>
        /// Gets the square root.
        /// </summary>
        /// <param name="number">The number to get the square root from.</param>
        /// <returns></returns>
        #region public static FP Sqrt(FP number)
        public static Fix64 Sqrt(Fix64 number) {
            return Fix64.Sqrt(number);
        }
        #endregion

        /// <summary>
        /// Gets the maximum number of two values.
        /// </summary>
        /// <param name="val1">The first value.</param>
        /// <param name="val2">The second value.</param>
        /// <returns>Returns the largest value.</returns>
        #region public static FP Max(FP val1, FP val2)
        public static Fix64 Max(Fix64 val1, Fix64 val2) {
            return (val1 > val2) ? val1 : val2;
        }
        #endregion

        /// <summary>
        /// Gets the minimum number of two values.
        /// </summary>
        /// <param name="val1">The first value.</param>
        /// <param name="val2">The second value.</param>
        /// <returns>Returns the smallest value.</returns>
        #region public static FP Min(FP val1, FP val2)
        public static Fix64 Min(Fix64 val1, Fix64 val2) {
            return (val1 < val2) ? val1 : val2;
        }
        #endregion

        /// <summary>
        /// Gets the maximum number of three values.
        /// </summary>
        /// <param name="val1">The first value.</param>
        /// <param name="val2">The second value.</param>
        /// <param name="val3">The third value.</param>
        /// <returns>Returns the largest value.</returns>
        #region public static FP Max(FP val1, FP val2,FP val3)
        public static Fix64 Max(Fix64 val1, Fix64 val2, Fix64 val3) {
            Fix64 max12 = (val1 > val2) ? val1 : val2;
            return (max12 > val3) ? max12 : val3;
        }
        #endregion

        /// <summary>
        /// Returns a number which is within [min,max]
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The clamped value.</returns>
        #region public static FP Clamp(FP value, FP min, FP max)
        public static Fix64 Clamp(Fix64 value, Fix64 min, Fix64 max) {
            value = (value > max) ? max : value;
            value = (value < min) ? min : value;
            return value;
        }
        #endregion

        /// <summary>
        /// Changes every sign of the matrix entry to '+'
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="result">The absolute matrix.</param>
        #region public static void Absolute(ref JMatrix matrix,out JMatrix result)
        public static void Absolute(ref FPMatrix matrix, out FPMatrix result) {
            result.M11 = Fix64.Abs(matrix.M11);
            result.M12 = Fix64.Abs(matrix.M12);
            result.M13 = Fix64.Abs(matrix.M13);
            result.M21 = Fix64.Abs(matrix.M21);
            result.M22 = Fix64.Abs(matrix.M22);
            result.M23 = Fix64.Abs(matrix.M23);
            result.M31 = Fix64.Abs(matrix.M31);
            result.M32 = Fix64.Abs(matrix.M32);
            result.M33 = Fix64.Abs(matrix.M33);
        }
        #endregion

        /// <summary>
        /// Returns the sine of value.
        /// </summary>
        public static Fix64 Sin(Fix64 value) {
            return Fix64.Sin(value);
        }

        /// <summary>
        /// Returns the cosine of value.
        /// </summary>
        public static Fix64 Cos(Fix64 value) {
            return Fix64.Cos(value);
        }

        /// <summary>
        /// Returns the tan of value.
        /// </summary>
        public static Fix64 Tan(Fix64 value) {
            return Fix64.Tan(value);
        }

        /// <summary>
        /// Returns the arc sine of value.
        /// </summary>
        public static Fix64 Asin(Fix64 value) {
            return Fix64.Asin(value);
        }

        /// <summary>
        /// Returns the arc cosine of value.
        /// </summary>
        public static Fix64 Acos(Fix64 value) {
            return Fix64.Acos(value);
        }

        /// <summary>
        /// Returns the arc tan of value.
        /// </summary>
        public static Fix64 Atan(Fix64 value) {
            return Fix64.Atan(value);
        }

        /// <summary>
        /// Returns the arc tan of coordinates x-y.
        /// </summary>
        public static Fix64 Atan2(Fix64 y, Fix64 x) {
            return Fix64.Atan2(y, x);
        }

        /// <summary>
        /// Returns the largest integer less than or equal to the specified number.
        /// </summary>
        public static Fix64 Floor(Fix64 value) {
            return Fix64.Floor(value);
        }

        /// <summary>
        /// Returns the smallest integral value that is greater than or equal to the specified number.
        /// </summary>
        public static Fix64 Ceiling(Fix64 value) {
            return value;
        }

        /// <summary>
        /// Rounds a value to the nearest integral value.
        /// If the value is halfway between an even and an uneven value, returns the even value.
        /// </summary>
        public static Fix64 Round(Fix64 value) {
            return Fix64.Round(value);
        }

        /// <summary>
        /// Returns a number indicating the sign of a Fix64 number.
        /// Returns 1 if the value is positive, 0 if is 0, and -1 if it is negative.
        /// </summary>
        public static int Sign(Fix64 value) {
            return Fix64.Sign(value);
        }

        /// <summary>
        /// Returns the absolute value of a Fix64 number.
        /// Note: Abs(Fix64.MinValue) == Fix64.MaxValue.
        /// </summary>
        public static Fix64 Abs(Fix64 value) {
            return Fix64.Abs(value);                
        }

        public static Fix64 Barycentric(Fix64 value1, Fix64 value2, Fix64 value3, Fix64 amount1, Fix64 amount2) {
            return value1 + (value2 - value1) * amount1 + (value3 - value1) * amount2;
        }

        public static Fix64 CatmullRom(Fix64 value1, Fix64 value2, Fix64 value3, Fix64 value4, Fix64 amount) {
            // Using formula from http://www.mvps.org/directx/articles/catmull/
            // Internally using FPs not to lose precission
            Fix64 amountSquared = amount * amount;
            Fix64 amountCubed = amountSquared * amount;
            return (Fix64)(0.5 * (2.0 * value2 +
                                 (value3 - value1) * amount +
                                 (2.0 * value1 - 5.0 * value2 + 4.0 * value3 - value4) * amountSquared +
                                 (3.0 * value2 - value1 - 3.0 * value3 + value4) * amountCubed));
        }

        public static Fix64 Distance(Fix64 value1, Fix64 value2) {
            return Fix64.Abs(value1 - value2);
        }

        public static Fix64 Hermite(Fix64 value1, Fix64 tangent1, Fix64 value2, Fix64 tangent2, Fix64 amount) {
            // All transformed to FP not to lose precission
            // Otherwise, for high numbers of param:amount the result is NaN instead of Infinity
            Fix64 v1 = value1, v2 = value2, t1 = tangent1, t2 = tangent2, s = amount, result;
            Fix64 sCubed = s * s * s;
            Fix64 sSquared = s * s;

            if (amount == 0f)
                result = value1;
            else if (amount == 1f)
                result = value2;
            else
                result = (2 * v1 - 2 * v2 + t2 + t1) * sCubed +
                         (3 * v2 - 3 * v1 - 2 * t1 - t2) * sSquared +
                         t1 * s +
                         v1;
            return (Fix64)result;
        }

        public static Fix64 Lerp(Fix64 value1, Fix64 value2, Fix64 amount) {
            return value1 + (value2 - value1) * amount;
        }

        public static Fix64 SmoothStep(Fix64 value1, Fix64 value2, Fix64 amount) {
            // It is expected that 0 < amount < 1
            // If amount < 0, return value1
            // If amount > 1, return value2
            Fix64 result = Clamp(amount, 0f, 1f);
            result = Hermite(value1, 0f, value2, 0f, result);
            return result;
        }

    }
}
