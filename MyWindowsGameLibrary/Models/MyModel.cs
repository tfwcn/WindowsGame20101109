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
using MyWindowsGameLibrary.Helper;

namespace MyWindowsGameLibrary.Models
{
    public class MyModel:MyParentModel
    {
        private Model model;
        /// <summary>
        /// 模型
        /// </summary>
        public Model Model
        {
            get { return model; }
            set { model = value; }
        }
        private Effect effect;
        /// <summary>
        /// 渲染效果
        /// </summary>
        public Effect Effect
        {
            get { return effect; }
            set { 
                effect = value;
                foreach (ModelMesh mesh in model.Meshes)
                {
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        part.Effect = effect;
                    }
                }
            }
        }
        private MyCamera camera;
        /// <summary>
        /// 摄像机
        /// </summary>
        public MyCamera Camera
        {
            get { return camera; }
            set { camera = value; }
        }
        private Matrix changeMatrix;
        /// <summary>
        /// 变换矩阵
        /// </summary>
        public Matrix ChangeMatrix
        {
            get { return changeMatrix; }
            set { changeMatrix = value; }
        }
        private BoundingSphere boundingSphere;
        /// <summary>
        /// 包围球
        /// </summary>
        public BoundingSphere BoundingSphere
        {
            get { return boundingSphere; }
            set { boundingSphere = value; }
        }

        public MyModel(Game game, int id,Model model,Vector3 position,MyCamera camera):base(game)
        {
            this.Id = id;
            this.model = model;
            this.Position = position;
            this.changeMatrix = Matrix.Identity;
            this.Rotation = Vector3.Zero;
            this.Scale = Vector3.One;
            this.camera = camera;
            this.boundingSphere = MyGeometryHelper.GetBoundingSphere(model);
        }

        public override void Update(GameTime gameTime)
        {
            this.changeMatrix = Matrix.CreateRotationX(MathHelper.ToRadians(Rotation.X)/*转换成弧度*/)/*旋转角度*/
                * Matrix.CreateRotationY(MathHelper.ToRadians(Rotation.Y))/*旋转角度*/
                * Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation.Z))/*旋转角度*/
                * Matrix.CreateScale(Scale)/*缩放*/
                * Matrix.CreateTranslation(Position.X, Position.Y, Position.Z)/*移动坐标*/;
        }

        /// <summary>
        /// 绘图方法
        /// </summary>
        /// <param name="gameTime">时间</param>
        public override void Draw(GameTime gameTime)
        {
            //宣告變數儲存模型的骨骼資訊    
            Matrix[] temprobot = new Matrix[model.Bones.Count];
            //然後再將模型的骨骼資訊複製到剛剛宣告的矩陣變數裡。
            model.CopyAbsoluteBoneTransformsTo(temprobot);

            //將模型的每個部位畫出來    
            foreach (ModelMesh mesh in model.Meshes)
            {
                if (effect == null)
                {
                    // 這邊使用XNA提供的基本效果來繪製模型
                    foreach (BasicEffect basicEffect in mesh.Effects)
                    {
                        // 使用XNA提供的打光方法，模型才不會一片漆黑或没光泽    
                        basicEffect.EnableDefaultLighting();
                        //basicEffect.PreferPerPixelLighting = true;//逐像素渲染
                        // 將模型的位置傳給效果來繪製模型
                        basicEffect.World = temprobot[mesh.ParentBone.Index]/*获得每个多边形的相对位置*/*changeMatrix;
                        basicEffect.View = camera.View;//觀看矩陣
                        basicEffect.Projection = camera.Projection;//投影矩陣 
                        //basicEffect.CurrentTechnique = effect.Techniques[0];//着色器
                    }
                }
                // 實際把模型畫出來 
                mesh.Draw();
            } 
        }
    }
}
