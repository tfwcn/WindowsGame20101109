using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MyWindowsGameLibrary.Helper
{
    /// <summary>
    /// 产生随机数
    /// </summary>
    public static class MyRandomHelper
    {
        public static Random random = new Random();
        /// <summary>
        /// 返回随机颜色
        /// </summary>
        /// <returns>Color</returns>
        public static Color GetRandomColor()
        {
            return new Color((byte)random.Next(255), (byte)random.Next(255), (byte)random.Next(255));
        }
        /// <summary>
        /// 返回非负的整数二维随机向量
        /// </summary>
        /// <returns>Vector2</returns>
        public static Vector2 GetRandomIntVector2()
        {
            return new Vector2(GetRandomInt(), GetRandomInt());
        }
        /// <summary>
        /// 返回0到max之间的整数二维随机向量
        /// </summary>
        /// <param name="max">最大值</param>
        /// <returns>Vector2</returns>
        public static Vector2 GetRandomIntVector2(Vector2 max)
        {
            return new Vector2(GetRandomInt((int)max.X), GetRandomInt((int)max.Y));
        }
        /// <summary>
        /// 返回min到max之间的整数二维随机向量
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns>Vector2</returns>
        public static Vector2 GetRandomIntVector2(Vector2 min, Vector2 max)
        {
            return new Vector2(GetRandomInt((int)min.X, (int)max.X), GetRandomInt((int)min.Y, (int)max.Y));
        }

        /// <summary>
        /// 返回非负的整数三维随机向量
        /// </summary>
        /// <returns>Vector3</returns>
        public static Vector3 GetRandomIntVector3()
        {
            return new Vector3(GetRandomInt(), GetRandomInt(), GetRandomInt());
        }
        /// <summary>
        /// 返回0到max之间的整数三维随机向量
        /// </summary>
        /// <param name="max">最大值</param>
        /// <returns>Vector3</returns>
        public static Vector3 GetRandomIntVector3(Vector3 max)
        {
            return new Vector3(GetRandomInt((int)max.X), GetRandomInt((int)max.Y), GetRandomInt((int)max.Z));
        }
        /// <summary>
        /// 返回min到max之间的整数三维随机向量
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns>Vector3</returns>
        public static Vector3 GetRandomIntVector3(Vector3 min, Vector3 max)
        {
            return new Vector3(GetRandomInt((int)min.X, (int)max.X), GetRandomInt((int)min.Y, (int)max.Y), GetRandomInt((int)min.Z, (int)max.Z));
        }
        /// <summary>
        /// 返回0.0到1.0之间的浮点数二维随机向量
        /// </summary>
        /// <returns>Vector2</returns>
        public static Vector2 GetRandomFloatVector2()
        {
            return new Vector2(GetRandomFloat(), GetRandomFloat());
        }
        /// <summary>
        /// 返回0.0到max之间的浮点数二维随机向量
        /// </summary>
        /// <param name="max">最大值</param>
        /// <returns>Vector2</returns>
        public static Vector2 GetRandomFloatVector2(Vector2 max)
        {
            return new Vector2(GetRandomFloat(max.X), GetRandomFloat(max.Y));
        }
        /// <summary>
        /// 返回min到max之间的浮点数二维随机向量
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns>Vector2</returns>
        public static Vector2 GetRandomFloatVector2(Vector2 min, Vector2 max)
        {
            return new Vector2(GetRandomFloat(min.X, max.X), GetRandomFloat(min.Y, max.Y));
        }
        /// <summary>
        /// 返回0.0到1.0之间的浮点数三维随机向量
        /// </summary>
        /// <returns>Vector3</returns>
        public static Vector3 GetRandomFloatVector3()
        {
            return new Vector3(GetRandomFloat(), GetRandomFloat(), GetRandomFloat());
        }
        /// <summary>
        /// 返回0.0到max之间的浮点数三维随机向量
        /// </summary>
        /// <param name="max">最大值</param>
        /// <returns>Vector3</returns>
        public static Vector3 GetRandomFloatVector3(Vector3 max)
        {
            return new Vector3(GetRandomFloat(max.X), GetRandomFloat(max.Y), GetRandomFloat(max.Z));
        }
        /// <summary>
        /// 返回min到max之间的浮点数三维随机向量
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns>Vector3</returns>
        public static Vector3 GetRandomFloatVector3(Vector3 min, Vector3 max)
        {
            return new Vector3(GetRandomFloat(min.X, max.X), GetRandomFloat(min.Y, max.Y), GetRandomFloat(min.Z, max.Z));
        }
        /// <summary>
        /// 返回非负随机整数
        /// </summary>
        /// <returns>byte</returns>
        public static byte GetRandomByte()
        {
            return (byte)GetRandomInt();
        }
        /// <summary>
        /// 返回0到max之间的随机整数
        /// </summary>
        /// <param name="max">最大值</param>
        /// <returns>int</returns>
        public static byte GetRandomByte(byte max)
        {
            return (byte)GetRandomInt(max);
        }
        /// <summary>
        /// 返回min到max之间的随机整数
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns>int</returns>
        public static byte GetRandomByte(byte min, byte max)
        {
            return (byte)GetRandomInt(min, max);
        }
        /// <summary>
        /// 返回非负随机整数
        /// </summary>
        /// <returns>int</returns>
        public static int GetRandomInt()
        {
            return random.Next();
        }
        /// <summary>
        /// 返回0到max之间的随机整数
        /// </summary>
        /// <param name="max">最大值</param>
        /// <returns>int</returns>
        public static int GetRandomInt(int max)
        {
            return random.Next(0, max);
        }
        /// <summary>
        /// 返回min到max之间的随机整数
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns>int</returns>
        public static int GetRandomInt(int min, int max)
        {
            return random.Next(min, max);
        }
        /// <summary>
        /// 返回0.0到1.0之间的随机单精度浮点数
        /// </summary>
        /// <returns>float</returns>
        public static float GetRandomFloat()
        {
            return (float)random.NextDouble();
        }
        /// <summary>
        /// 返回0到max之间的随机单精度浮点数
        /// </summary>
        /// <param name="max">最大值</param>
        /// <returns>float</returns>
        public static float GetRandomFloat(float max)
        {
            return (float)random.NextDouble() * max;
        }
        /// <summary>
        /// 返回min到max之间的随机单精度浮点数
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns>float</returns>
        public static float GetRandomFloat(float min, float max)
        {
            return (float)random.NextDouble() * (max - min) + min;
        }
        /// <summary>
        /// 返回0.0到1.0之间的随机双精度浮点数
        /// </summary>
        /// <returns>double</returns>
        public static double GetRandomDouble()
        {
            return random.NextDouble();
        }
        /// <summary>
        /// 返回0到max之间的随机双精度浮点数
        /// </summary>
        /// <param name="max">最大值</param>
        /// <returns>double</returns>
        public static double GetRandomDouble(double max)
        {
            return random.NextDouble() * max;
        }
        /// <summary>
        /// 返回min到max之间的随机双精度浮点数
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns>double</returns>
        public static double GetRandomDouble(double min, double max)
        {
            return random.NextDouble() * (max - min) + min;
        }
        /// <summary>
        /// 用随机数填充制定字节数组的元素
        /// </summary>
        /// <param name="buffer">字节数组</param>
        public static void GetRandomBytes(byte[] buffer)
        {
            random.NextBytes(buffer);
        }
    }
}
