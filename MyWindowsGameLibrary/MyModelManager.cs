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
using MyWindowsGameLibrary.Helper;

namespace MyWindowsGameLibrary
{
    public class MyModelManager : DrawableGameComponent
    {
        private int newId = 0;
        private List<int> removeModelIdList;

        private MyCamera camera;
        /// <summary>
        /// 摄像机
        /// </summary>
        public MyCamera Camera
        {
            get { return camera; }
            set { camera = value; }
        }
        private List<MyParentModel> myModelList;
        /// <summary>
        /// 模型列表
        /// </summary>
        public List<MyParentModel> MyModelList
        {
            get { return myModelList; }
            set { myModelList = value; }
        }
        private List<int> updateMyModelIdList;
        /// <summary>
        /// 需要更新的模型ID列表
        /// </summary>
        public List<int> UpdateMyModelIdList
        {
            get { return updateMyModelIdList; }
            set { updateMyModelIdList = value; }
        }
        private List<IMyUpdate> myUpdateList;
        /// <summary>
        /// 逻辑列表
        /// </summary>
        public List<IMyUpdate> MyUpdateList
        {
            get { return myUpdateList; }
            set { myUpdateList = value; }
        }
        private MyOcTreeNode myOcTree;
        /// <summary>
        /// 八叉树
        /// </summary>
        public MyOcTreeNode MyOcTree
        {
            get { return myOcTree; }
            set { myOcTree = value; }
        }

        public MyModelManager(Game game, MyCamera camera)
            : base(game)
        {
            this.myModelList = new List<MyParentModel>();
            this.myUpdateList = new List<IMyUpdate>();
            this.updateMyModelIdList = new List<int>();
            this.removeModelIdList = new List<int>();
            this.camera = camera;
            this.myOcTree = new MyOcTreeNode(Vector3.Zero, 10000, new MyOcTree(camera, 50, 1, game));
        }

        #region 基本事件
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
            //自定义逻辑
            foreach (IMyUpdate myUpdate in myUpdateList)
            {
                myUpdate.Update(gameTime);
            }

            //移除精灵
            myOcTree.RemoveModels(removeModelIdList);
            removeModelIdList.Clear();

            //更新八叉树
            foreach (int id in updateMyModelIdList)
            {
                MyParentModel m = myOcTree.RemoveModel(id);
                myOcTree.AddModel(m);
            }

            //获得需要绘制的精灵列表
            BoundingFrustum frustum = new BoundingFrustum(camera.View * camera.Projection);
            myModelList.Clear();
            myModelList = myOcTree.GetDrawList(frustum);

            //精灵逻辑
            foreach (MyParentModel myModel in myModelList)
            {
                myModel.Update(gameTime);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// 绘图方法
        /// </summary>
        /// <param name="gameTime">时间</param>
        public override void Draw(GameTime gameTime)
        {
            //精灵绘制
            foreach (MyParentModel my3DSprite in myModelList)
            {
                my3DSprite.Draw(gameTime);
            }

            base.Draw(gameTime);
        }
        #endregion

        #region 操作model
        public int AddMyModel(Model model, Vector3 position)
        {
            MyParentModel myModel = new MyModel(this.Game, newId, model, position, camera);
            myOcTree.AddModel(myModel);
            newId++;
            return myModel.Id;
        }

        public MyParentModel GetMyModel(int id)
        {
            return myOcTree.GetMyModel(id);
        }

        public void RemoveMyModel(int id)
        {
            removeModelIdList.Add(id);
        }
        #endregion
    }
}
