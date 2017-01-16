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
    public static class MyMouseHelper
    {
        /// <summary>
        /// 上一帧鼠标状态
        /// </summary>
        private static MouseState oldMouse;
        /// <summary>
        /// 上一帧中键滚动数
        /// </summary>
        private static int oldScrollWheelValue;

        private static float rotationSpeed = 1;
        /// <summary>
        /// 鼠标灵敏度
        /// </summary>
        public static float RotationSpeed
        {
            get { return rotationSpeed; }
            set { rotationSpeed = value; }
        }

        /// <summary>
        /// 初始化鼠标
        /// </summary>
        public static void Reset(GraphicsDeviceManager graphics)
        {
            Mouse.SetPosition(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);
            oldMouse = Mouse.GetState();
        }

        /// <summary>
        /// 获得鼠标变化值
        /// </summary>
        /// <returns>Vector3</returns>
        public static Vector3 GetMouseChange(GraphicsDeviceManager graphics)
        {
            MouseState mouse = Mouse.GetState();
            Vector3 vector3 = Vector3.Zero;
            if (mouse != oldMouse)
            {
                float xDifference = mouse.X - oldMouse.X;
                float yDifference = mouse.Y - oldMouse.Y;
                vector3.X = xDifference * rotationSpeed;
                vector3.Y = yDifference * rotationSpeed;
                Mouse.SetPosition(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);
                //oldMouse = mouse;
                vector3.Z = mouse.ScrollWheelValue - oldScrollWheelValue;
                oldScrollWheelValue = mouse.ScrollWheelValue;
            }
            return vector3;
        }
    }
}
