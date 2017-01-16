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

namespace MyWindowsGameLibrary.Models
{
    public abstract class MyParentSprite
    {
        private Game game;

        public Game Game
        {
            get { return game; }
            set { game = value; }
        }
        private Point size;
        /// <summary>
        /// 大小
        /// </summary>
        public Point Size
        {
            get { return size; }
            set { size = value; }
        }
        private Vector2 position;
        /// <summary>
        /// 位置
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
