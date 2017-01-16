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
using MyWindowsGameLibrary.Models;

namespace WindowsGame20101109
{
    public class SpriteUpdate:IMyUpdate
    {
        private MyParentSprite mySprite;

        public MyParentSprite MySprite
        {
            get { return mySprite; }
            set { mySprite = value; }
        }
        private GraphicsDeviceManager graphics;

        public SpriteUpdate(MyParentSprite mySprite, GraphicsDeviceManager graphics)
        {
            this.mySprite = mySprite;
            this.graphics = graphics;
        }

        int xv = 1,yv=1;
        public void Update(GameTime gameTime)
        {
            if (mySprite.Position.X < 0)
            {
                xv = 1;
            } 
            else if (mySprite.Position.X +mySprite.Size.X>graphics.PreferredBackBufferWidth)
            {
                xv = -1;
            }
            if (mySprite.Position.Y < 0)
            {
                yv = 1;
            }
            else if (mySprite.Position.Y + mySprite.Size.Y > graphics.PreferredBackBufferHeight)
            {
                yv = -1;
            }
            mySprite.Position += new Vector2(xv, yv);
        }
    }
}
