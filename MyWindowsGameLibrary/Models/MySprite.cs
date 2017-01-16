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
    public class MySprite : MyParentSprite
    {
        private int lastTime;//记录上一帧时间
        #region 属性
        private Texture2D image;
        /// <summary>
        /// 图片
        /// </summary>
        public Texture2D Image
        {
            get { return image; }
            set { image = value; }
        }
        private Point offset;
        /// <summary>
        /// 偏移量
        /// </summary>
        public Point Offset
        {
            get { return offset; }
            set { offset = value; }
        }
        private Point rowAndCol;
        /// <summary>
        /// 行列数
        /// </summary>
        public Point RowAndCol
        {
            get { return rowAndCol; }
            set { rowAndCol = value; }
        }
        private int[] order;
        /// <summary>
        /// 动画顺序数组
        /// </summary>
        public int[] Order
        {
            get { return order; }
            set { order = value; }
        }
        private int time;
        /// <summary>
        /// 动画间隔时间
        /// </summary>
        public int Time
        {
            get { return time; }
            set { time = value>0?value:-1; }
        }
        private bool isBegin;
        /// <summary>
        /// 是否开始播放
        /// </summary>
        public bool IsBegin
        {
            get { return isBegin; }
            set { isBegin = value; }
        }
        private bool isRepeat;
        /// <summary>
        /// 是否循环播放
        /// </summary>
        public bool IsRepeat
        {
            get { return isRepeat; }
            set { isRepeat = value; }
        }
        private Color color;
        /// <summary>
        /// 渲染颜色
        /// </summary>
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }
        private float rotation;
        /// <summary>
        /// 旋转
        /// </summary>
        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }
        private Vector2 origin;
        /// <summary>
        /// 原点
        /// </summary>
        public Vector2 Origin
        {
            get { return origin; }
            set { origin = value; }
        }
        private Vector2 scale;
        /// <summary>
        /// 缩放
        /// </summary>
        public Vector2 Scale
        {
            get { return scale; }
            set { scale = value; }
        }
        private SpriteEffects effects;
        /// <summary>
        /// 效果
        /// </summary>
        public SpriteEffects Effects
        {
            get { return effects; }
            set { effects = value; }
        }
        private float layerDepth;
        /// <summary>
        /// 层
        /// </summary>
        public float LayerDepth
        {
            get { return layerDepth; }
            set { layerDepth = value; }
        }
        #endregion

        public MySprite(Texture2D image,Vector2 position)
        {
            this.image = image;
            this.Size = new Point(image.Width,image.Height);
            this.Position = position;

            this.offset = Point.Zero;
            this.rowAndCol = new Point(1, 1);
            this.order = new int[]{0};
            this.time = 0;
            this.color = Color.White;
            this.rotation = 0;
            this.scale = Vector2.One;
            this.effects = SpriteEffects.None;
            this.layerDepth = 1;
        }

        public MySprite(Texture2D image, int width, int height, Vector2 position, Point rowAndCol, int time)
            : this(image, position)
        {
            this.Size = new Point(width,height);
            this.rowAndCol = rowAndCol;
            this.order = new int[] { 0 };
            this.time = time;
        }

        public MySprite(Texture2D image, int width, int height, Vector2 position, Point rowAndCol, int time, int[] order)
            : this(image, width, height, position, rowAndCol,time)
        {
            this.order = order;
        }
        int nowOrder = 0;
        int defaultOrder = 0;
        public override void Update(GameTime gameTime)
        {
            lastTime += gameTime.ElapsedGameTime.Milliseconds;
            if (time <= lastTime)
            {
                //判断是否超出范围
                if (order[nowOrder]>0 && order[nowOrder] <= rowAndCol.X * rowAndCol.Y)
                {
                    offset.X = order[nowOrder] % rowAndCol.Y;
                    offset.Y = order[nowOrder] % rowAndCol.X;
                }
                else
                {
                    offset.X = (defaultOrder) % rowAndCol.Y;
                    offset.Y = (defaultOrder)/rowAndCol.Y;
                }
                nowOrder++;
                if (nowOrder >= order.Length)
                {
                    nowOrder = 0;
                }
                defaultOrder++;
                if (defaultOrder >= rowAndCol.X * rowAndCol.Y)
                {
                    defaultOrder = 0;
                }

                lastTime = 0;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, Position, new Rectangle(offset.X*Size.X, offset.Y*Size.Y, Size.X, Size.Y), color, rotation, origin, scale, effects, layerDepth);
        }
    }
}
