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

namespace MyWindowsGameLibrary.Helper
{
    public class MyOcTree0
    {

    }
    /// <summary>
    /// 八叉树类（用来绘制模型）
    /// </summary>
    public class MyOcTreeNode0
    {
        private const int maxObjectsInNode = 2;
        private const float minSize = 5f;
        private Vector3 center;
        private float size;
        private List<MyModel> modelList;
        private BoundingBox nodeBoundingBox;
        private MyOcTreeNode nodeUFL;
        private MyOcTreeNode nodeUFR;
        private MyOcTreeNode nodeUBL;
        private MyOcTreeNode nodeUBR;
        private MyOcTreeNode nodeDFL;
        private MyOcTreeNode nodeDFR;
        private MyOcTreeNode nodeDBL;
        private MyOcTreeNode nodeDBR;
        private List<MyOcTreeNode> childList;
        private static int modelsDrawn;

        private Game game;
        private SpriteBatch spriteBatch;
        public MyCamera myCamera;

        public static int ModelsDrawn
        {
            get { return MyOcTreeNode.modelsDrawn; }
            set { MyOcTreeNode.modelsDrawn = value; }
        }
        private static int modelsStoredInQuadTree;
        private static float maxSize = 0;
        private static BoundingFrustum cameraFrustum;

        public MyOcTreeNode(Vector3 center, float size,Game game)
        {
            this.game = game;
            spriteBatch = new SpriteBatch(game.GraphicsDevice);
            this.center = center;
            this.size = size;
            if (MyOcTreeNode.maxSize == 0)
                maxSize = size;
            modelList = new List<MyModel>();
            childList = new List<MyOcTreeNode>(8);
            Vector3 diagonalVector = new Vector3(size / 2.0f, size / 2.0f, size / 2.0f);
            nodeBoundingBox = new BoundingBox(center - diagonalVector, center + diagonalVector);
        }
        private void CreateChildNodes()
        {
            float sizeOver2 = size / 2.0f;
            float sizeOver4 = size / 4.0f;
            nodeUFR = new MyOcTreeNode(center + new Vector3(sizeOver4, sizeOver4, -sizeOver4), sizeOver2,game);
            nodeUFL = new MyOcTreeNode(center + new Vector3(-sizeOver4, sizeOver4, -sizeOver4), sizeOver2, game);
            nodeUBR = new MyOcTreeNode(center + new Vector3(sizeOver4, sizeOver4, sizeOver4), sizeOver2, game);
            nodeUBL = new MyOcTreeNode(center + new Vector3(-sizeOver4, sizeOver4, sizeOver4), sizeOver2, game);
            nodeDFR = new MyOcTreeNode(center + new Vector3(sizeOver4, -sizeOver4, -sizeOver4), sizeOver2, game);
            nodeDFL = new MyOcTreeNode(center + new Vector3(-sizeOver4, -sizeOver4, -sizeOver4), sizeOver2, game);
            nodeDBR = new MyOcTreeNode(center + new Vector3(sizeOver4, -sizeOver4, sizeOver4), sizeOver2, game);
            nodeDBL = new MyOcTreeNode(center + new Vector3(-sizeOver4, -sizeOver4, sizeOver4), sizeOver2, game);
            childList.Add(nodeUFR);
            childList.Add(nodeUFL);
            childList.Add(nodeUBR);
            childList.Add(nodeUBL);
            childList.Add(nodeDFR);
            childList.Add(nodeDFL);
            childList.Add(nodeDBR);
            childList.Add(nodeDBL);
        }
        private void AddModel(MyModel model)
        {
            if (childList.Count == 0)
            {
                modelList.Add(model);
                bool maxObjectsReached = (modelList.Count > maxObjectsInNode);
                bool minSizeNotReached = (size > minSize);
                if (maxObjectsReached && minSizeNotReached)
                {
                    CreateChildNodes();
                    foreach (MyModel currentModel in modelList)
                    {
                        Distribute(currentModel);
                    }
                    modelList.Clear();
                }
            }
            else
            {
                Distribute(model);
            }
        }
        private void Distribute(MyModel model)
        {
            Vector3 position = model.Position;
            if (position.Y > center.Y) //Up 
                if (position.Z < center.Z) //Forward
                    if (position.X < center.X) //Left 
                        nodeUFL.AddModel(model);
                    else //Right
                        nodeUFR.AddModel(model);
                else //Back
                    if (position.X < center.X) //Left
                        nodeUBL.AddModel(model);
                    else //Right 
                        nodeUBR.AddModel(model);
            else //Down
                if (position.Z < center.Z) //Forward
                    if (position.X < center.X) //Left 
                        nodeDFL.AddModel(model);
                    else //Right
                        nodeDFR.AddModel(model);
                else //Back
                    if (position.X < center.X) //Left
                        nodeDBL.AddModel(model);
                    else //Right 
                        nodeDBR.AddModel(model);
        }
        /// <summary>
        /// 添加模型
        /// </summary>
        /// <param name="newModel">模型</param>
        /// <returns>模型id</returns>
        public int Add(MyModel newModel)
        {
            newModel.Id = modelsStoredInQuadTree++;
            AddModel(newModel);
            return newModel.Id;
        }
        /// <summary>
        /// 绘制模型列表
        /// </summary>
        /// <param name="cameraFrustum"></param>
        public void Draw()
        {
            if (this.size == MyOcTreeNode.maxSize)
            {
                MyOcTreeNode.cameraFrustum = new BoundingFrustum(myCamera.View * myCamera.Projection);
            }
            ContainmentType cameraNodeContainment = cameraFrustum.Contains(nodeBoundingBox);
            if (cameraNodeContainment != ContainmentType.Disjoint)
            {
                foreach (MyModel model in modelList)
                {
                    model.Draw(spriteBatch);
                    modelsDrawn++;
                }
                foreach (MyOcTreeNode childNode in childList)
                    childNode.Draw();
            }
        }
        /// <summary>
        /// 更新模型世界矩阵
        /// </summary>
        /// <param name="modelID">模型id</param>
        /// <param name="position">坐标</param>
        /// <param name="scale">缩放比例</param>
        /// <param name="rotation">旋转</param>
        public void UpdateModelWorldMatrix(int modelID, Vector3 position, Vector3 scale, Vector3 rotation)
        {
            MyModel deletedModel = RemoveModel(modelID);
            if (deletedModel == null)
                return;
            deletedModel.Position = position;
            deletedModel.Scale = scale;
            deletedModel.Rotation = rotation;
            AddModel(deletedModel);
        }
        private MyModel RemoveModel(int modelID)
        {
            MyModel model = null;
            for (int index = 0; index < modelList.Count; index++)
            {
                if (modelList[index].Id == modelID)
                {
                    model = modelList[index];
                    modelList.Remove(model);
                }
            }
            int child = 0;
            while ((model == null) && (child < childList.Count))
            {
                model = childList[child++].RemoveModel(modelID);
            }
            return model;
        }
    }
}
