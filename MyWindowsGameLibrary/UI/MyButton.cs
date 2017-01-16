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
using MyWindowsGameLibrary.Models;

namespace MyWindowsGameLibrary.UI
{
    public class MyButton:MySprite
    {
        private int id;
        /// <summary>
        /// ID
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        private Texture2D nomalImage;
        /// <summary>
        /// 默认图片
        /// </summary>
        public Texture2D NomalImage
        {
            get { return nomalImage; }
            set { nomalImage = value; }
        }
        private Texture2D hoverImage;
        /// <summary>
        /// 选择时显示的图片
        /// </summary>
        public Texture2D HoverImage
        {
            get { return hoverImage; }
            set { hoverImage = value; }
        }
        private Texture2D actionImage;
        /// <summary>
        /// 按下时显示的图片
        /// </summary>
        public Texture2D ActionImage
        {
            get { return actionImage; }
            set { actionImage = value; }
        }
        private bool mouseHover;
        /// <summary>
        /// 鼠标悬停事件
        /// </summary>
        public bool MouseHover
        {
            get { return mouseHover; }
            set { mouseHover = value; }
        }
        private bool mouseDown;
        /// <summary>
        /// 鼠标按下事件
        /// </summary>
        public bool MouseDown
        {
            get { return mouseDown; }
            set { mouseDown = value; }
        }
        private bool mouseOut;
        /// <summary>
        /// 鼠标离开事件
        /// </summary>
        public bool MouseOut
        {
            get { return mouseOut; }
            set { mouseOut = value; }
        }
        private bool enabled;
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public MyButton(Vector2 position, Texture2D image)
            : base(image, position)
        {
            this.nomalImage = image;
            this.enabled = true;
        }

        public MyButton(Vector2 position, Texture2D image, Texture2D hoverImage, Texture2D actionImage)
            : this(position, image)
        {
            this.hoverImage = hoverImage;
            this.actionImage = actionImage;
        }

        public MyButton(Vector2 position, int width, int height, Texture2D image, Texture2D hoverImage, Texture2D actionImage)
            : this(position, image,  hoverImage,  actionImage)
        {
            this.Size = new Point(width, height);
        }

        public override void Update(GameTime gameTime)
        {
            mouseHover = false;
            mouseDown = false;
            mouseOut = false;
            MouseState mouse = Mouse.GetState();
            if (mouse.X >= this.Position.X && mouse.X <= (this.Position.X + this.Size.X) && mouse.Y >= this.Position.Y && mouse.Y <= (this.Position.Y + this.Size.Y))
            {
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    mouseDown = true;
                    if (actionImage != null)
                    {
                        this.Image = actionImage;
                    }
                    else
                    {
                        this.Color = new Color(200,200,200);
                    }
                }
                else
                {
                    mouseHover = true;
                    if (actionImage != null)
                    {
                        this.Image = hoverImage;
                    }
                    else
                    {
                        this.Color = Color.White;
                    }
                }
            }
            else
            {
                mouseOut = true;
                this.Image = nomalImage;
                if (actionImage != null && actionImage != null)
                {
                    this.Color = Color.White;
                }
                else
                {
                    this.Color = new Color(200, 200, 200);
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Image, this.Position, new Rectangle(this.Offset.X * this.Size.X, this.Offset.Y * this.Size.Y, this.Size.X, this.Size.Y), this.Color, this.Rotation, this.Origin, this.Scale, this.Effects, this.LayerDepth);
        }
    }
}
