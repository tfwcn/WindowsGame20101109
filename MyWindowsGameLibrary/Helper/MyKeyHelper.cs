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
    public static class MyKeyHelper
    {
        /// <summary>
        /// 获得方向键的状态,Y=UP,DOWN,X=RIGHT,LEFT
        /// </summary>
        /// <param name="key">键盘状态，Keyboard.GetState()</param>
        /// <returns>Vector2</returns>
        public static Vector2 GetCross()
        {
            KeyboardState key = Keyboard.GetState();
            Vector2 vector2 = Vector2.Zero;
            if (key.IsKeyDown(Keys.Up))
            {
                vector2.Y++;
            }
            if (key.IsKeyDown(Keys.Down))
            {
                vector2.Y--;
            }
            if (key.IsKeyDown(Keys.Right))
            {
                vector2.X++;
            }
            if (key.IsKeyDown(Keys.Left))
            {
                vector2.X--;
            }
            return vector2;
        }
        /// <summary>
        /// 获得所有方向键的状态,W=UP,X=DOWN,Y=RIGHT,Z=LEFT
        /// </summary>
        /// <param name="key">键盘状态，Keyboard.GetState()</param>
        /// <returns>Vector4</returns>
        public static Vector4 GetCrossX()
        {
            KeyboardState key = Keyboard.GetState();
            Vector4 vector4 = Vector4.Zero;
            if (key.IsKeyDown(Keys.Up))
            {
                vector4.W = 1.0f;
            }
            if (key.IsKeyDown(Keys.Down))
            {
                vector4.X = 1.0f;
            }
            if (key.IsKeyDown(Keys.Right))
            {
                vector4.Y = 1.0f;
            }
            if (key.IsKeyDown(Keys.Left))
            {
                vector4.Z = 1.0f;
            }
            return vector4;
        }
        /// <summary>
        /// 获得方向键的状态,Y=UP,DOWN,X=RIGHT,LEFT
        /// </summary>
        /// <param name="key">键盘状态，Keyboard.GetState()</param>
        /// <returns>Vector2</returns>
        public static Vector2 GetWSDACross()
        {
            KeyboardState key = Keyboard.GetState();
            Vector2 vector2 = Vector2.Zero;
            if (key.IsKeyDown(Keys.W))
            {
                vector2.Y++;
            }
            if (key.IsKeyDown(Keys.S))
            {
                vector2.Y--;
            }
            if (key.IsKeyDown(Keys.D))
            {
                vector2.X++;
            }
            if (key.IsKeyDown(Keys.A))
            {
                vector2.X--;
            }
            return vector2;
        }
        /// <summary>
        /// 获得所有方向键的状态,W=UP,X=DOWN,Y=RIGHT,Z=LEFT
        /// </summary>
        /// <param name="key">键盘状态，Keyboard.GetState()</param>
        /// <returns>Vector4</returns>
        public static Vector4 GetWSDACrossX()
        {
            KeyboardState key = Keyboard.GetState();
            Vector4 vector4 = Vector4.Zero;
            if (key.IsKeyDown(Keys.W))
            {
                vector4.W = 1.0f;
            }
            if (key.IsKeyDown(Keys.S))
            {
                vector4.X = 1.0f;
            }
            if (key.IsKeyDown(Keys.D))
            {
                vector4.Y = 1.0f;
            }
            if (key.IsKeyDown(Keys.A))
            {
                vector4.Z = 1.0f;
            }
            return vector4;
        }
    }
}
