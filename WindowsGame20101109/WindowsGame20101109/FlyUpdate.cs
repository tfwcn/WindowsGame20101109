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
    public class FlyUpdate:IMyUpdate
    {
        private MyParentModel my3DSprite;
        public MyParentModel My3DSprite
        {
            get{return this.my3DSprite;}
            set{this.my3DSprite = value;}
        }

        public FlyUpdate(MyParentModel my3DSprite)
        {
            this.my3DSprite = my3DSprite;
        }

        private int i = 1;
        public void Update(GameTime gameTime)
        {
            my3DSprite.Rotation = new Vector3(0, 0, i);
            my3DSprite.Position = new Vector3(my3DSprite.Position.X, my3DSprite.Position.Y + 0.5f, my3DSprite.Position.Z + 2);

            i++;
            if (i > 360)
            {
                i = 1;
            }
        }
    }
}
