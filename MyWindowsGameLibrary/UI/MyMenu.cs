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

namespace MyWindowsGameLibrary.UI
{
    public class MyMenu : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private int btnId;
        private int index;
        /// <summary>
        /// 选择的按钮
        /// </summary>
        public int Index
        {
            get { return index; }
            set { index = value; }
        }
        private List<MyButton> buttonList;
        /// <summary>
        /// 按钮列表
        /// </summary>
        public List<MyButton> ButtonList
        {
            get { return buttonList; }
            set { buttonList = value; }
        }
        public MyMenu(Game game):base(game)
        {
            this.index = -1;
            this.btnId = 1;
            this.buttonList = new List<MyButton>();
        }
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
            this.index = -1;
            foreach (MyButton b in buttonList)
            {
                b.Update(gameTime);
                if (b.MouseDown)
                {
                    this.index = b.Id;
                }
            }
            base.Update(gameTime);
        }
        /// <summary>
        /// 绘图方法
        /// </summary>
        /// <param name="gameTime">时间</param>
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);

            foreach (MyButton b in buttonList)
            {
                b.Draw(spriteBatch);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        public MyButton GeyMyButton(int id)
        {
            foreach (MyButton b in buttonList)
            {
                if (b.Id == id)
                {
                    return b;
                }
            }
            return null;
        }
        public int AddButton(MyButton button)
        {
            button.Id = btnId;
            buttonList.Add(button);
            btnId++;
            return button.Id;
        }
        public bool RemoveMyButton(int id)
        {
            for (int i = 0; i<buttonList.Count; i++)
            {
                if (buttonList[i].Id == id)
                {
                    buttonList.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }
    }
}
