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
using MyWindowsGameLibrary;
using MyWindowsGameLibrary.Interfaces;
using MyWindowsGameLibrary.Models;
using MyWindowsGameLibrary.Helper;
using MyWindowsGameLibrary.UI;

namespace WindowsGame20101109
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        MySpriteManager mySpriteManager;
        MyModelManager my3DSpriteManager;
        MyCamera myCamera;
        MySkysphere mySky;
        MyMenu myMenu;
        MyTerrain myTerrain;
        int btn2D; 
        int btn3D;
        int btnFoundRoad;
        private FoundRoadTest foundRoad;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //设置全屏
            //GraphicsAdapter adapter = graphics.GraphicsDevice.Adapter;
            //graphics.PreferredBackBufferWidth = adapter.CurrentDisplayMode.Width;
            //graphics.PreferredBackBufferHeight = adapter.CurrentDisplayMode.Height;
            //graphics.ToggleFullScreen();
            //初始化二维管理器
            mySpriteManager = new MySpriteManager(this);
            //初始化摄像机
            Matrix projection = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4.0f/*视野范围*/, GraphicsDevice.Viewport.AspectRatio/*按屏幕比例转换*/, 1.0f/*最近距离*/, 10000.0f/*最远距离*/);
            myCamera = new MyCamera(new Vector3(0,0,1000), Vector3.Forward, Vector3.Up, projection);
            my3DSpriteManager = new MyModelManager(this,myCamera); 
            //初始化天空球
            mySky=new MySkysphere(this);
            mySky.View = myCamera.View;
            //创建菜单
            myMenu = new MyMenu(this);
            //创建地形
            myTerrain = new MyTerrain(this, Content.Load<Texture2D>("heightmap"), Content.Load<Texture2D>("GrassTx"), myCamera);
            myTerrain.Scale = new Vector3(10, 150, 10);
            myTerrain.Position = new Vector3(-600, -200, 1000);
            //创建寻路算法测试
            foundRoad = new FoundRoadTest(this,graphics);

            Components.Add(foundRoad);
            Components.Add(myTerrain);
            Components.Add(mySky);
            Components.Add(mySpriteManager);
            Components.Add(my3DSpriteManager);
            Components.Add(myMenu);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //显示初始化
            mySky.Visible = false;
            mySky.Enabled = false;
            mySpriteManager.Visible = false;
            mySpriteManager.Enabled = false;
            my3DSpriteManager.Visible = false;
            my3DSpriteManager.Enabled = false;
            myTerrain.Visible = false;
            myTerrain.Enabled = false;
            foundRoad.Visible = false;
            foundRoad.Enabled = false;
            //添加二维精灵
            mySpriteManager.MySpriteList.Add(new MySprite(Content.Load<Texture2D>("title"), new Vector2(0, 0)));
            MyParentSprite threerings = new MySprite(Content.Load<Texture2D>("threerings"), 60, 60, new Vector2(0, 100), new Point(8, 6), 20);
            mySpriteManager.MySpriteList.Add(threerings);
            SpriteUpdate spriteUpdate = new SpriteUpdate(threerings, graphics);
            mySpriteManager.MyUpdateList.Add(spriteUpdate);
            //添加三维模型和模型动作
            int flyId= my3DSpriteManager.AddMyModel(Content.Load<Model>("fly"), Vector3.Zero);
            my3DSpriteManager.UpdateMyModelIdList.Add(flyId);
            MyModel fly = (MyModel)my3DSpriteManager.GetMyModel(flyId);
            FlyUpdate fupdate = new FlyUpdate(fly);
            my3DSpriteManager.MyUpdateList.Add(fupdate);
            my3DSpriteManager.AddMyModel(Content.Load<Model>("fly"), new Vector3(200, 0, 0));
            my3DSpriteManager.AddMyModel(Content.Load<Model>("fly"), new Vector3(-200, 0, 0));
            my3DSpriteManager.AddMyModel(Content.Load<Model>("fly"), new Vector3(0, 200, 0));
            //my3DSpriteManager.AddMyModel(Content.Load<Model>("fly"), new Vector3(0, -200, 0));
            //my3DSpriteManager.AddMyModel(Content.Load<Model>("fly"), new Vector3(0, 0, 200));
            my3DSpriteManager.AddMyModel(Content.Load<Model>("fly"), new Vector3(0, 0, -200));
            //初始化鼠标助手
            MyMouseHelper.Reset(graphics);
            MyMouseHelper.RotationSpeed = 0.005f;
            //添加菜单按钮
            btn2D = myMenu.AddButton(new MyButton(new Vector2(this.graphics.PreferredBackBufferWidth / 2 - 100, this.graphics.PreferredBackBufferHeight / 2 - 25 - 100),
                Content.Load<Texture2D>("2DUp"), Content.Load<Texture2D>("2DHover"), Content.Load<Texture2D>("2DHover")));
            btn3D = myMenu.AddButton(new MyButton(new Vector2(this.graphics.PreferredBackBufferWidth / 2 - 100, this.graphics.PreferredBackBufferHeight / 2 - 25),
                Content.Load<Texture2D>("3DUp"), Content.Load<Texture2D>("3DHover"), Content.Load<Texture2D>("3DHover")));
            btnFoundRoad = myMenu.AddButton(new MyButton(new Vector2(this.graphics.PreferredBackBufferWidth / 2 - 100, this.graphics.PreferredBackBufferHeight / 2 - 25 + 100),
                Content.Load<Texture2D>("findRoadUp"), Content.Load<Texture2D>("findRoadHover"), Content.Load<Texture2D>("findRoadHover")));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                //this.Exit();
            // TODO: Add your update logic here
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            if (myMenu.Visible)
            {
                if (myMenu.Index == btn2D)
                {
                    MyMouseHelper.Reset(graphics);
                    mySpriteManager.Visible = true;
                    mySpriteManager.Enabled = true;

                    myMenu.Visible = false;
                    myMenu.Enabled = false;
                } 
                if (myMenu.Index == btn3D)
                {
                    MyMouseHelper.Reset(graphics);
                    mySky.Visible = true;
                    mySky.Enabled = true;
                    my3DSpriteManager.Visible = true;
                    my3DSpriteManager.Enabled = true;
                    myTerrain.Visible = true;
                    myTerrain.Enabled = true;

                    myMenu.Visible = false;
                    myMenu.Enabled = false;
                    this.IsMouseVisible = false;
                } 
                if (myMenu.Index == btnFoundRoad)
                {
                    MyMouseHelper.Reset(graphics);
                    foundRoad.Visible = true;
                    foundRoad.Enabled = true;

                    myMenu.Visible = false;
                    myMenu.Enabled = false;
                }
            }
            else
            {
                if (ks.IsKeyDown(Keys.Back))
                {
                    mySky.Visible = false;
                    mySky.Enabled = false;
                    mySpriteManager.Visible = false;
                    mySpriteManager.Enabled = false;
                    my3DSpriteManager.Visible = false;
                    my3DSpriteManager.Enabled = false;
                    myTerrain.Visible = false;
                    myTerrain.Enabled = false;
                    foundRoad.Visible = false;
                    foundRoad.Enabled = false;

                    myMenu.Visible = true;
                    myMenu.Enabled = true;
                    this.IsMouseVisible = true;
                }
            }
            if (this.my3DSpriteManager.Visible)
            {
                Vector3 v3 = MyMouseHelper.GetMouseChange(graphics);

                if (v3 != Vector3.Zero)
                {
                    myCamera.RotationViewMatrix(new Vector2(v3.X, v3.Y));

                    //my3DSpriteManager.View = myCamera.FlyViewMatrix(v3);
                }
                Vector2 key = MyKeyHelper.GetWSDACross();
                myCamera.MoveViewMatrix(new Vector3(key.X, 0, key.Y));
                //my3DSpriteManager.View = myCamera.View;
                mySky.View = myCamera.View;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }
    }
}
