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
using MyWindowsGameLibrary.Helper;
using MyWindowsGameLibrary.AI;

namespace WindowsGame20101109
{
    public class FoundRoadTest : DrawableGameComponent
    {
        private int width;
        private int height;
        private int wallNum;
        private VertexPositionColor[] lineData;
        private VertexPositionColor[] wallData;
        private VertexPositionColor[] roadData;
        private int[,] data;
        private BasicEffect effect;
        private MyAStarPoint start;
        private MyAStarPoint end;
        private GraphicsDeviceManager graphics;
        private MyAStar aStar;

        public FoundRoadTest(Game game,GraphicsDeviceManager graphics)
            : base(game)
        {
            this.width = graphics.PreferredBackBufferWidth;
            this.height = graphics.PreferredBackBufferHeight;
            this.effect = new BasicEffect(game.GraphicsDevice);
            this.effect.VertexColorEnabled = true;
            this.graphics = graphics;
            int w, h;
            if (width % 10 != 0)
            {
                w = width / 10;
            }
            else
            {
                w = width / 10 - 1;
            }
            if (height % 10 != 0)
            {
                h = height / 10;
            }
            else
            {
                h = height / 10 - 1;
            }
            this.start = new MyAStarPoint(0, 0);
            this.end = new MyAStarPoint(w, h);
            this.aStar = new MyAStar(width / 10, height / 10, start, end);
            lineData = new VertexPositionColor[w * h * 2];
            data = new int[width / 10, height / 10];
        }
        private void InitData()
        {
            wallData = null;
            roadData = null;
            wallNum = 0;
            for (int x = 0; x < data.GetLength(0); x++)
            {
                for (int y = 0; y < data.GetLength(1); y++)
                {
                    data[x, y] = 0;
                }
            }
        }
        /// <summary>
        /// 创建网格
        /// </summary>
        private void CreatPoints()
        {
            float cw = this.width / 2;
            float ch = this.height / 2;
            int i = 0;
            for (int x = 10; x < width; x += 10)
            {
                lineData[i++] = new VertexPositionColor(new Vector3(-1f + x / cw, 1f - 0 / ch, 0), Color.White);
                lineData[i++] = new VertexPositionColor(new Vector3(-1f + x / cw, 1f - height / ch, 0), Color.White);
            }
            for (int y = 10; y < height; y += 10)
            {
                lineData[i++] = new VertexPositionColor(new Vector3(-1f + 0 / cw, 1f - y / ch, 0), Color.White);
                lineData[i++] = new VertexPositionColor(new Vector3(-1f + width / cw, 1f - y / ch, 0), Color.White);
            }
        }
        /// <summary>
        /// 创建墙列表
        /// </summary>
        private void CreatWall()
        {
            float cw = this.width / 2;
            float ch = this.height / 2;
            wallData=new VertexPositionColor[wallNum*6];
            aStar.WallList.Clear();
            int i = 0;
            for (int x = 0; x < data.GetLength(0); x++)
            {
                for (int y = 0; y < data.GetLength(1); y++)
                {
                    if (data[x, y] == 1)
                    {
                        wallData[i++] = new VertexPositionColor(new Vector3(-1f + (x*10) / cw, 1f - (y*10) / ch, 0), Color.Blue);
                        wallData[i++] = new VertexPositionColor(new Vector3(-1f + (x * 10+10) / cw, 1f - (y * 10) / ch, 0), Color.Blue);
                        wallData[i++] = new VertexPositionColor(new Vector3(-1f + (x * 10) / cw, 1f - (y * 10 + 10) / ch, 0), Color.Blue);
                        wallData[i++] = new VertexPositionColor(new Vector3(-1f + (x * 10) / cw, 1f - (y * 10 + 10) / ch, 0), Color.Blue);
                        wallData[i++] = new VertexPositionColor(new Vector3(-1f + (x * 10+10) / cw, 1f - (y * 10) / ch, 0), Color.Blue);
                        wallData[i++] = new VertexPositionColor(new Vector3(-1f + (x * 10+10) / cw, 1f - (y * 10 + 10) / ch, 0), Color.Blue);
                        aStar.WallList.Add(new MyAStarPoint(x, y));
                    }
                }
            }
        }
        /// <summary>
        /// 创建路
        /// </summary>
        private void CreatRoad()
        {
            float cw = this.width / 2;
            float ch = this.height / 2;
            List<MyAStarPoint> road= aStar.FindRoad();
            roadData = new VertexPositionColor[road.Count * 6];
            int i = 0;
            foreach (MyAStarPoint p in road)
            {
                roadData[i++] = new VertexPositionColor(new Vector3(-1f + (p.X * 10) / cw, 1f - (p.Y * 10) / ch, 0), Color.Red);
                roadData[i++] = new VertexPositionColor(new Vector3(-1f + (p.X * 10 + 10) / cw, 1f - (p.Y * 10) / ch, 0), Color.Red);
                roadData[i++] = new VertexPositionColor(new Vector3(-1f + (p.X * 10) / cw, 1f - (p.Y * 10 + 10) / ch, 0), Color.Red);
                roadData[i++] = new VertexPositionColor(new Vector3(-1f + (p.X * 10) / cw, 1f - (p.Y * 10 + 10) / ch, 0), Color.Red);
                roadData[i++] = new VertexPositionColor(new Vector3(-1f + (p.X * 10 + 10) / cw, 1f - (p.Y * 10) / ch, 0), Color.Red);
                roadData[i++] = new VertexPositionColor(new Vector3(-1f + (p.X * 10 + 10) / cw, 1f - (p.Y * 10 + 10) / ch, 0), Color.Red);
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public override void Initialize()
        {
            InitData();
            CreatPoints();
            base.Initialize();
        }

        /// <summary>
        /// 加载
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
        }

        /// <summary>
        /// 卸载
        /// </summary>
        protected override void UnloadContent()
        {
            base.UnloadContent();
        }
        /// <summary>
        /// 逻辑方法
        /// </summary>
        /// <param name="gameTime">时间</param>
        public override void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();
            if (mouseState.X >= 0 && mouseState.X < width && mouseState.Y >= 0 && mouseState.Y < height)
            {
                if (mouseState.LeftButton == ButtonState.Pressed)//添加墙
                {
                    roadData = null;
                    if (data[mouseState.X / 10, mouseState.Y / 10] == 0)
                    {
                        data[mouseState.X / 10, mouseState.Y / 10] = 1;
                        wallNum++;
                    } 
                    CreatWall();
                }
                else if (mouseState.RightButton == ButtonState.Pressed)//取消墙
                {
                    roadData = null;
                    if (data[mouseState.X / 10, mouseState.Y / 10] == 1)
                    {
                        data[mouseState.X / 10, mouseState.Y / 10] = 0;
                        wallNum--;
                    }
                    CreatWall();
                }
                if (keyState.IsKeyDown(Keys.D1))//按1更改起点
                {
                    roadData = null;
                    start.X = mouseState.X / 10;
                    start.Y = mouseState.Y / 10;
                    data[mouseState.X / 10, mouseState.Y / 10] = 0;
                    aStar.Start.X = start.X;
                    aStar.Start.Y = start.Y;
                }
                else if (keyState.IsKeyDown(Keys.D2))//按2更改终点
                {
                    roadData = null;
                    end.X = mouseState.X / 10;
                    end.Y = mouseState.Y / 10;
                    data[mouseState.X / 10, mouseState.Y / 10] = 0;
                    aStar.End.X = end.X;
                    aStar.End.Y = end.Y;
                }
                else if (keyState.IsKeyDown(Keys.Space))//按空格生成路径
                {
                    CreatWall();
                    CreatRoad();
                }
                else if (keyState.IsKeyDown(Keys.C))//按c清除路径
                {
                    InitData();
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
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                this.Game.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, lineData, 0, lineData.Length / 2, VertexPositionColor.VertexDeclaration);

                if (roadData != null && roadData.Length != 0)
                {
                    this.Game.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, roadData, 0, roadData.Length / 3, VertexPositionColor.VertexDeclaration);
                }
                if (wallData != null && wallData.Length != 0)
                {
                    this.Game.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, wallData, 0, wallData.Length / 3, VertexPositionColor.VertexDeclaration);
                }
                MyGeometryHelper.FillRect(this.Game, graphics, new Vector2(start.X*10,start.Y*10), new Vector2(10, 10), Color.Green);
                MyGeometryHelper.FillRect(this.Game, graphics, new Vector2(end.X*10, end.Y*10), new Vector2(10, 10), Color.Orange);
            }
            base.Draw(gameTime);
        }
    }
}
