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
    public class MyTerrain : DrawableGameComponent
    {
        private Texture2D heightMap;//高度图
        private Texture2D grassTexture;//贴图
        private MyCamera camera;//摄像机
        private BasicEffect basicEffect;
        private GraphicsDevice device;
        private float[,] heightData;
        /// <summary>
        /// 高度数据
        /// </summary>
        public float[,] HeightData
        {
            get { return heightData; }
        }
        private Vector3 scale;
        private VertexBuffer vertexBuffer;
        private IndexBuffer indexBuffer;
        /// <summary>
        /// 地图缩放
        /// </summary>
        public Vector3 Scale
        {
            get { return scale; }
            set { scale = value; }
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

        public MyTerrain(Game game, Texture2D heightMap, Texture2D grassTexture,MyCamera camera)
            : base(game)
        {
            this.heightMap = heightMap;
            this.grassTexture = grassTexture;
            this.camera = camera;
            this.scale = Vector3.One;
            this.position = Vector3.Zero;
            device = Game.GraphicsDevice;

            basicEffect = new BasicEffect(device); 
        }
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
            LoadHeightdata();//读取高度
            VertexPositionNormalTexture[] vertices = CreateTerrainVertices();
            int[] indices = CreateTerrainIndices();
            vertices = GenerateNormalsForTriangleStrip(vertices, indices);
            vertexBuffer = new VertexBuffer(device, VertexPositionNormalTexture.VertexDeclaration, vertices.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData<VertexPositionNormalTexture>(vertices);//写入顶点缓冲
            indexBuffer = new IndexBuffer(device, typeof(int), indices.Length, BufferUsage.WriteOnly);
            indexBuffer.SetData<int>(indices); //写入索引缓冲
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

            base.Update(gameTime);
        }

        /// <summary>
        /// 绘图方法
        /// </summary>
        /// <param name="gameTime">时间</param>
        public override void Draw(GameTime gameTime)
        {
            int width = heightData.GetLength(0);
            int height = heightData.GetLength(1);

            basicEffect.World = Matrix.Identity*Matrix.CreateScale(scale) * Matrix.CreateTranslation(position);
            basicEffect.View = camera.View;
            basicEffect.Projection = camera.Projection;
            basicEffect.Texture = grassTexture;
            basicEffect.TextureEnabled = true;

            basicEffect.EnableDefaultLighting();
            Vector3 lightDirection = new Vector3(0, 0, 0);
            lightDirection.Normalize();
            basicEffect.DirectionalLight0.Direction = lightDirection;
            basicEffect.DirectionalLight0.Enabled = true;
            basicEffect.DirectionalLight1.Enabled = false;
            basicEffect.DirectionalLight2.Enabled = false;
            basicEffect.AmbientLightColor = new Vector3(0.5f, 0.5f, 0.5f);
            basicEffect.SpecularColor = new Vector3(1.0f, 1.0f, 1.0f);

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.SetVertexBuffer(vertexBuffer);
                device.Indices = indexBuffer; 

                device.DrawIndexedPrimitives(PrimitiveType.TriangleStrip, 0, 0, width * height, 0, width * 2 * (height - 1) - 2);
            }

            base.Draw(gameTime);
        }
        /// <summary>
        /// 读取高度图
        /// </summary>
        private void LoadHeightdata()
        {
            int width = heightMap.Width;
            int height = heightMap.Height;
            Color[] heightMapColors = new Color[width * height];
            heightMap.GetData<Color>(heightMapColors);
            heightData = new float[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    heightData[x, y] = heightMapColors[x + y * width].R/255.0f;
                }
            }
        }
        /// <summary>
        /// 创建顶点
        /// </summary>
        /// <returns>VertexPositionNormalTexture[]</returns>
        private VertexPositionNormalTexture[] CreateTerrainVertices()
        {
            int width = heightData.GetLength(0);
            int height = heightData.GetLength(1);

            VertexPositionNormalTexture[] terrainVertices = new VertexPositionNormalTexture[width * height];

            int i = 0;
            for (int z = 0; z < height; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    Vector3 position = new Vector3(x, heightData[x, z], -z);
                    Vector3 normal = new Vector3(0, 1, 0);
                    Vector2 texCoord = new Vector2((float)x / grassTexture.Width, (float)z / grassTexture.Height);
                    terrainVertices[i++] = new VertexPositionNormalTexture(position, normal, texCoord);
                }
            }

            return terrainVertices;
        }
        /// <summary>
        /// 创建索引
        /// </summary>
        /// <returns>int[]</returns>
        private int[] CreateTerrainIndices()
        {
            int width = heightData.GetLength(0);
            int height = heightData.GetLength(1);

            int[] terrainIndices = new int[(width) * 2 * (height - 1)];

            int i = 0;
            int z = 0;
            while (z < height - 1)
            {
                for (int x = 0; x < width; x++)
                {
                    terrainIndices[i++] = x + z * width;
                    terrainIndices[i++] = x + (z + 1) * width;
                }
                z++;

                if (z < height - 1)
                {
                    for (int x = width - 1; x >= 0; x--)
                    {
                        terrainIndices[i++] = x + (z + 1) * width;
                        terrainIndices[i++] = x + z * width;
                    }
                }
                z++;
            }
            return terrainIndices;
        } 
        /// <summary>
        /// 创建顶点法线
        /// </summary>
        /// <param name="vertices">顶点集合</param>
        /// <param name="indices">索引集合</param>
        /// <returns>VertexPositionNormalTexture[]</returns>
        private VertexPositionNormalTexture[] GenerateNormalsForTriangleStrip(VertexPositionNormalTexture[] vertices, int[] indices)
        {
            for (int i = 0; i < vertices.Length; i++)
                vertices[i].Normal = new Vector3(0, 0, 0);

            bool swappedWinding = false;
            for (int i = 2; i < indices.Length; i++)
            {
                Vector3 firstVec = vertices[indices[i - 1]].Position - vertices[indices[i]].Position;
                Vector3 secondVec = vertices[indices[i - 2]].Position - vertices[indices[i]].Position;
                Vector3 normal = Vector3.Cross(firstVec, secondVec);
                normal.Normalize();

                if (swappedWinding)
                    normal *= -1;

                if (!float.IsNaN(normal.X))
                {
                    vertices[indices[i]].Normal += normal;
                    vertices[indices[i - 1]].Normal += normal;
                    vertices[indices[i - 2]].Normal += normal;
                }

                swappedWinding = !swappedWinding;
            }

            for (int i = 0; i < vertices.Length; i++)
                vertices[i].Normal.Normalize();

            return vertices;
        }
    }
}
