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
using MyWindowsGameLibrary.Interfaces;

namespace MyWindowsGameLibrary.Models
{
    public class MyCamera
    {
        /// <summary>
        /// X轴旋转值
        /// </summary>
        private float updownRotation = 0;
        /// <summary>
        /// Y轴旋转值
        /// </summary>
        private float leftrightRotation = 0;
        /// <summary>
        /// 四元数保存轴和旋转量
        /// </summary>
        private Quaternion cameraRotation = Quaternion.Identity;
        private Matrix projection;
        /// <summary>
        /// 投影矩阵
        /// </summary>
        public Matrix Projection
        {
            get { return projection; }
            set { projection = value; }
        }
        private Matrix view;
        /// <summary>
        /// 摄像机矩阵
        /// </summary>
        public Matrix View
        {
            get { return this.view; }
            set { view = value; }
        }

        private Vector3 position;
        /// <summary>
        /// 视点
        /// </summary>
        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        private Vector3 target;
        /// <summary>
        /// 焦点
        /// </summary>
        public Vector3 Target
        {
            get { return target; }
            set { target = value; }
        }
        
        private Vector3 upVector;
        /// <summary>
        /// 上方向
        /// </summary>
        public Vector3 UpVector
        {
            get { return upVector; }
            set { upVector = value; }
        }

        private BoundingSphere sphere;
        /// <summary>
        /// 摄像机包围球
        /// </summary>
        public BoundingSphere Sphere
        {
            get { return sphere; }
            set { sphere = value; }
        }

        public MyCamera(Vector3 position, Vector3 target, Vector3 upVector)
        {
            this.position = position;
            this.target = target;
            this.upVector = upVector;
            this.view = Matrix.CreateLookAt(position, target, upVector);
        }

        public MyCamera(Vector3 position, Vector3 target, Vector3 upVector, Matrix projection)
        {
            this.position = position;
            this.target = target;
            this.upVector = upVector;
            this.view = Matrix.CreateLookAt(position, target, upVector);
            this.projection = projection;
        }

        /// <summary>
        /// 第一人称视觉旋转
        /// </summary>
        /// <param name="rotation">x,y变换值</param>
        public Matrix RotationViewMatrix(Vector2 rotation)
        {
            updownRotation -= rotation.Y;
            leftrightRotation -= rotation.X;
            if (updownRotation >= MathHelper.TwoPi || updownRotation <= -MathHelper.TwoPi)
            {
                updownRotation -= updownRotation/Math.Abs(updownRotation)* MathHelper.TwoPi;
            }
            if (leftrightRotation >= MathHelper.TwoPi || leftrightRotation <= -MathHelper.TwoPi)
            {
                leftrightRotation -= leftrightRotation / Math.Abs(leftrightRotation) * MathHelper.TwoPi;
            }
            Matrix cameraRotation = Matrix.CreateRotationX(updownRotation) * Matrix.CreateRotationY(leftrightRotation);

            Vector3 cameraOriginalTarget = target;
            Vector3 cameraOriginalUpVector = upVector;

            Vector3 cameraRotatedTarget = Vector3.Transform(cameraOriginalTarget, cameraRotation);
            Vector3 cameraFinalTarget = position + cameraRotatedTarget;

            Vector3 cameraRotatedUpVector = Vector3.Transform(cameraOriginalUpVector, cameraRotation);
            view = Matrix.CreateLookAt(position, cameraFinalTarget, cameraRotatedUpVector);
            return view;
        }
        /// <summary>
        /// 向X、Y、Z轴移动镜头
        /// </summary>
        /// <param name="vectorToAdd"></param>
        public Matrix MoveViewMatrix(Vector3 move)
        {
            if (move != Vector3.Zero)
            {
                Matrix cameraRotation = Matrix.CreateRotationX(updownRotation) * Matrix.CreateRotationY(leftrightRotation);
                Vector3 rotatedVector = Vector3.Transform(new Vector3(move.X,move.Y,-move.Z), cameraRotation);
                position += rotatedVector;
                view=RotationViewMatrix(Vector2.Zero);
            }
            return view;
        }
        /// <summary>
        /// 飞行视觉（X、Y控制方向，Z控制前后）
        /// </summary>
        /// <param name="rotation">x,y变换值</param>
        public Matrix FlyViewMatrix(Vector3 move)
        {
            Quaternion additionalRotation = Quaternion.CreateFromAxisAngle(new Vector3(0, 1, 0), -move.X) * Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), -move.Y);
            cameraRotation = cameraRotation * additionalRotation;

            Vector3 rotatedVector = Vector3.Transform(move, cameraRotation);
            position += rotatedVector;

            Vector3 cameraOriginalTarget = target;
            Vector3 cameraOriginalUpVector = upVector;

            Vector3 cameraRotatedTarget = Vector3.Transform(cameraOriginalTarget, cameraRotation);
            Vector3 cameraFinalTarget = position + cameraRotatedTarget;

            Vector3 cameraRotatedUpVector = Vector3.Transform(cameraOriginalUpVector, cameraRotation);
            view = Matrix.CreateLookAt(position, cameraFinalTarget, cameraRotatedUpVector);
            return view;
        }
    }
}
