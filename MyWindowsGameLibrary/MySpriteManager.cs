using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MyWindowsGameLibrary.Interfaces;
using MyWindowsGameLibrary.Models;

namespace MyWindowsGameLibrary
{
    public class MySpriteManager : DrawableGameComponent
    {
        SpriteBatch spriteBatch;

        private List<MyParentSprite> mySpriteList;
        /// <summary>
        /// 精灵列表
        /// </summary>
        public List<MyParentSprite> MySpriteList
        {
            get { return mySpriteList; }
            set { mySpriteList = value; }
        }
        private List<IMyUpdate> myUpdateList;
        /// <summary>
        /// 逻辑列表
        /// </summary>
        public List<IMyUpdate> MyUpdateList
        {
            get { return myUpdateList; }
            set { myUpdateList = value; }
        }

        public MySpriteManager(Game game)
            : base(game)
        {
            this.mySpriteList = new List<MyParentSprite>();
            this.myUpdateList = new List<IMyUpdate>();
        }

        #region 基本事件

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// 加载
        /// </summary>
        protected override void LoadContent()
        {
            // 创建一个新的SpriteBatch,可以用来抽取的纹理。
            spriteBatch = new SpriteBatch(base.Game.GraphicsDevice);
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
                //控制逻辑
                foreach (IMyUpdate myUpdate in myUpdateList)
                {
                    myUpdate.Update(gameTime);
                }
                //精灵逻辑
                foreach (MySprite mySprite in mySpriteList)
                {
                    mySprite.Update(gameTime);
                }
            
            base.Update(gameTime);
        }

        /// <summary>
        /// 绘图方法
        /// </summary>
        /// <param name="gameTime">时间</param>
        public override void Draw(GameTime gameTime)
        {
                //GraphicsDevice.Clear(Color.CornflowerBlue);
                //base.Game.GraphicsDevice.;
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);

                //精灵绘制
                foreach (MySprite mySprite in mySpriteList)
                {
                    mySprite.Draw(spriteBatch);
                }

                spriteBatch.End();
            base.Draw(gameTime);
        }
        #endregion
    }
}
