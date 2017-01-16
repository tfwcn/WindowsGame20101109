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
    public class MySkysphere : DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        GraphicsDevice device;
        Model SkySphere;
        Effect SkySphereEffect;
        private Matrix view;

        public Matrix View
        {
            get { return view; }
            set { view = value; }
        }
        private Matrix projection;

        public Matrix Projection
        {
            get { return projection; }
            set { projection = value; }
        }

        public MySkysphere(Game game)
            : base(game)
        {
            view = Matrix.CreateLookAt(Vector3.Zero/*摄像机位置*/, Vector3.Forward/*面向点*/, Vector3.Up/*上方向*/);
            projection = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4.0f/*视野范围*/, base.Game.GraphicsDevice.Viewport.AspectRatio/*按屏幕比例转换*/, 1.0f/*最近距离*/, 10000.0f/*最远距离*/);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Initialize()
        {
            device = base.Game.GraphicsDevice;
            base.Initialize();
        }

        /// <summary>
        /// 加载
        /// </summary>
        protected override void LoadContent()
        {
            // 创建一个新的SpriteBatch,可以用来抽取的纹理。
            spriteBatch = new SpriteBatch(base.Game.GraphicsDevice);
            // Load the effect, the texture it uses, and 
            // the model used for drawing it
            SkySphereEffect = base.Game.Content.Load<Effect>("SkySphere");

            //TextureCube SkyboxTexture =
            //    base.Game.Content.Load<TextureCube>("uffizi_cross");
            TextureCube SkyboxTexture =
                base.Game.Content.Load<TextureCube>("skyboxtexture");
            SkySphere = base.Game.Content.Load<Model>("SphereHighPoly");

            // Set the parameters of the effect
            SkySphereEffect.Parameters["ViewMatrix"].SetValue(view);
            SkySphereEffect.Parameters["ProjectionMatrix"].SetValue(projection);
            SkySphereEffect.Parameters["SkyboxTexture"].SetValue(SkyboxTexture);
            // Set the Skysphere Effect to each part of the Skysphere model
            foreach (ModelMesh mesh in SkySphere.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = SkySphereEffect;
                }
            }
        }

        /// <summary>
        /// 卸载
        /// </summary>
        protected override void UnloadContent()
        {

        }
        /// <summary>
        /// 逻辑方法
        /// </summary>
        /// <param name="gameTime">时间</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// 绘图方法
        /// </summary>
        /// <param name="gameTime">时间</param>
        public override void Draw(GameTime gameTime)
        {
            SkySphereEffect.Parameters["ViewMatrix"].SetValue(view);
            SkySphereEffect.Parameters["ProjectionMatrix"].SetValue(projection);
            //关闭深度缓冲
            DepthStencilState dss = new DepthStencilState();
            dss.DepthBufferWriteEnable = false;
            device.DepthStencilState = dss;  
            dss.Dispose();
            //绘制天空
            foreach (ModelMesh mesh in SkySphere.Meshes)
            {
                mesh.Draw();
            }
            //打开深度缓冲
            dss = new DepthStencilState();
            dss.DepthBufferWriteEnable = true;
            device.DepthStencilState=dss;
            dss.Dispose();
            base.Draw(gameTime);
        }
    }
}
