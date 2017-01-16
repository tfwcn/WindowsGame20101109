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
    public class MyOcTree
    {
        public MyCamera myCamera;
        public float minSize;
        public int nodeModelMaxNum;
        public Game game;
        public SpriteBatch spriteBatch;
        public MyOcTree(MyCamera myCamera, float minSize, int nodeModelMaxNum, Game game)
        {
            this.myCamera = myCamera;
            this.minSize = minSize;
            this.nodeModelMaxNum = nodeModelMaxNum;
            this.game = game;
            spriteBatch = new SpriteBatch(game.GraphicsDevice);
        }
    }
    /// <summary>
    /// 八叉树类（用来绘制模型）
    /// </summary>
    public class MyOcTreeNode
    {
        public MyOcTree myOcTree;
        private Vector3 center;
        private float size;
        private List<MyParentModel> modelList;
        private BoundingBox nodeBoundingBox;
        private MyOcTreeNode nodeUFL;
        private MyOcTreeNode nodeUFR;
        private MyOcTreeNode nodeUBL;
        private MyOcTreeNode nodeUBR;
        private MyOcTreeNode nodeDFL;
        private MyOcTreeNode nodeDFR;
        private MyOcTreeNode nodeDBL;
        private MyOcTreeNode nodeDBR;
        private int modelSum;
        private bool hasChild;

        public MyOcTreeNode(Vector3 center, float size,MyOcTree ocTree)
        {
            this.center = center;
            this.size = size;
            this.hasChild = false;
            this.modelSum = 0;
            this.myOcTree = ocTree;
            modelList = new List<MyParentModel>();
            Vector3 diagonalVector = new Vector3(size / 2.0f, size / 2.0f, size / 2.0f);
            nodeBoundingBox = new BoundingBox(center - diagonalVector, center + diagonalVector);
        }
        private void CreateChildNodes()
        {
            hasChild = true;
            float sizeOver2 = size / 2.0f;
            float sizeOver4 = size / 4.0f;
            nodeUFR = new MyOcTreeNode(center + new Vector3(sizeOver4, sizeOver4, -sizeOver4), sizeOver2, myOcTree);
            nodeUFL = new MyOcTreeNode(center + new Vector3(-sizeOver4, sizeOver4, -sizeOver4), sizeOver2, myOcTree);
            nodeUBR = new MyOcTreeNode(center + new Vector3(sizeOver4, sizeOver4, sizeOver4), sizeOver2, myOcTree);
            nodeUBL = new MyOcTreeNode(center + new Vector3(-sizeOver4, sizeOver4, sizeOver4), sizeOver2, myOcTree);
            nodeDFR = new MyOcTreeNode(center + new Vector3(sizeOver4, -sizeOver4, -sizeOver4), sizeOver2, myOcTree);
            nodeDFL = new MyOcTreeNode(center + new Vector3(-sizeOver4, -sizeOver4, -sizeOver4), sizeOver2, myOcTree);
            nodeDBR = new MyOcTreeNode(center + new Vector3(sizeOver4, -sizeOver4, sizeOver4), sizeOver2, myOcTree);
            nodeDBL = new MyOcTreeNode(center + new Vector3(-sizeOver4, -sizeOver4, sizeOver4), sizeOver2, myOcTree);
        }
        private void Distribute(MyParentModel model)
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
        public void AddModel(MyParentModel model)
        {
            modelSum++;
            if (!hasChild)
            {
                modelList.Add(model);
                bool maxObjectsReached = (modelSum > myOcTree.nodeModelMaxNum);
                bool minSizeNotReached = (size > myOcTree.minSize);
                if (maxObjectsReached && minSizeNotReached)
                {
                    CreateChildNodes();
                    foreach (MyParentModel currentModel in modelList)
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
        public MyParentModel RemoveModel(int modelId)
        {
            MyParentModel model = null;
            for(int i=0;i<modelList.Count;i++)
            {
                if (modelList[i].Id == modelId)
                {
                    model = modelList[i];
                    modelList.RemoveAt(i);
                }
            }
            if (hasChild)
            {
                if (model == null)
                    model = nodeUFR.RemoveModel(modelId);
                if (model == null)
                    model = nodeUFL.RemoveModel(modelId);
                if (model == null)
                    model = nodeUBR.RemoveModel(modelId);
                if (model == null)
                    model = nodeUBL.RemoveModel(modelId);
                if (model == null)
                    model = nodeDFR.RemoveModel(modelId);
                if (model == null)
                    model = nodeDFL.RemoveModel(modelId);
                if (model == null)
                    model = nodeDBR.RemoveModel(modelId);
                if (model == null)
                    model = nodeDBL.RemoveModel(modelId);
            }
            if (model != null)
            {
                this.modelSum--;
            }
            return model;
        }
        public List<MyParentModel> RemoveModels(List<int> modelsId)
        {
            List<MyParentModel> models = new List<MyParentModel>();
            for (int i = 0; i < modelList.Count; i++)
            {
                for (int j = 0; j < modelsId.Count; j++)
                {
                    if (modelList[i].Id == modelsId[j])
                    {
                        models.Add(modelList[i]);
                        modelList.RemoveAt(i);
                        modelsId.RemoveAt(j);
                    }
                }
            }
            if (hasChild && modelSum != 0)
            {
                models.AddRange(nodeUFR.RemoveModels(modelsId));
                models.AddRange(nodeUFL.RemoveModels(modelsId));
                models.AddRange(nodeUBR.RemoveModels(modelsId));
                models.AddRange(nodeUBL.RemoveModels(modelsId));
                models.AddRange(nodeDFR.RemoveModels(modelsId));
                models.AddRange(nodeDFL.RemoveModels(modelsId));
                models.AddRange(nodeDBR.RemoveModels(modelsId));
                models.AddRange(nodeDBL.RemoveModels(modelsId));
            }
            this.modelSum-=models.Count;
            return models;
        }
        public MyParentModel GetMyModel(int modelId)
        {
            MyParentModel model = null;
            for (int i = 0; i < modelList.Count; i++)
            {
                if (modelList[i].Id == modelId)
                {
                    model = modelList[i];
                }
            }
            if (hasChild)
            {
                if (model == null)
                    model = nodeUFR.RemoveModel(modelId);
                if (model == null)
                    model = nodeUFL.RemoveModel(modelId);
                if (model == null)
                    model = nodeUBR.RemoveModel(modelId);
                if (model == null)
                    model = nodeUBL.RemoveModel(modelId);
                if (model == null)
                    model = nodeDFR.RemoveModel(modelId);
                if (model == null)
                    model = nodeDFL.RemoveModel(modelId);
                if (model == null)
                    model = nodeDBR.RemoveModel(modelId);
                if (model == null)
                    model = nodeDBL.RemoveModel(modelId);
            }
            return model;
        }
        public List<MyParentModel> GetDrawList(BoundingFrustum boundingFrustum)
        {
            List<MyParentModel> models = new List<MyParentModel>();
            if (hasChild&&modelSum!=0)
            {
                models.AddRange(nodeUFR.GetDrawList(boundingFrustum));
                models.AddRange(nodeUFL.GetDrawList(boundingFrustum));
                models.AddRange(nodeUBR.GetDrawList(boundingFrustum));
                models.AddRange(nodeUBL.GetDrawList(boundingFrustum));
                models.AddRange(nodeDFR.GetDrawList(boundingFrustum));
                models.AddRange(nodeDFL.GetDrawList(boundingFrustum));
                models.AddRange(nodeDBR.GetDrawList(boundingFrustum));
                models.AddRange(nodeDBL.GetDrawList(boundingFrustum));
            }
            else
            {
                if (boundingFrustum.Contains(this.nodeBoundingBox) != ContainmentType.Disjoint)
                {
                    models.AddRange(this.modelList);
                }
            }
            return models;
        }
    }
}
