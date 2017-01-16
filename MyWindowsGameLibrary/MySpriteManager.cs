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
        /// �����б�
        /// </summary>
        public List<MyParentSprite> MySpriteList
        {
            get { return mySpriteList; }
            set { mySpriteList = value; }
        }
        private List<IMyUpdate> myUpdateList;
        /// <summary>
        /// �߼��б�
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

        #region �����¼�

        /// <summary>
        /// ��ʼ��
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// ����
        /// </summary>
        protected override void LoadContent()
        {
            // ����һ���µ�SpriteBatch,����������ȡ������
            spriteBatch = new SpriteBatch(base.Game.GraphicsDevice);
        }

        /// <summary>
        /// ж��
        /// </summary>
        protected override void UnloadContent()
        {
            
        }

        /// <summary>
        /// �߼�����
        /// </summary>
        /// <param name="gameTime">ʱ��</param>
        public override void Update(GameTime gameTime)
        {
                //�����߼�
                foreach (IMyUpdate myUpdate in myUpdateList)
                {
                    myUpdate.Update(gameTime);
                }
                //�����߼�
                foreach (MySprite mySprite in mySpriteList)
                {
                    mySprite.Update(gameTime);
                }
            
            base.Update(gameTime);
        }

        /// <summary>
        /// ��ͼ����
        /// </summary>
        /// <param name="gameTime">ʱ��</param>
        public override void Draw(GameTime gameTime)
        {
                //GraphicsDevice.Clear(Color.CornflowerBlue);
                //base.Game.GraphicsDevice.;
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);

                //�������
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
