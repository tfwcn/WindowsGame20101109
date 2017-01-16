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
    public abstract class MyParentModel
    {
        private Game game;

        public Game Game
        {
            get { return game; }
            set { game = value; }
        }
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        private Vector3 rotation;
        /// <summary>
        /// 旋转
        /// </summary>
        public Vector3 Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }
        private Vector3 position;
        /// <summary>
        /// 位置
        /// </summary>
        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }
        private Vector3 scale;
        /// <summary>
        /// 缩放
        /// </summary>
        public Vector3 Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public MyParentModel(Game game)
        {
            this.game = game;
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);
    }
}
