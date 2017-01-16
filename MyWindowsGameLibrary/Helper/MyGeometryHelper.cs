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
    public static class MyGeometryHelper
    {
        /// <summary>
        /// 获得全局包围球
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static BoundingSphere GetBoundingSphere(Model model)
        {
            BoundingSphere boundingSphere = new BoundingSphere();
            Matrix[] modelTransforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(modelTransforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                BoundingSphere origMeshSphere = mesh.BoundingSphere;
                origMeshSphere.Transform(modelTransforms[mesh.ParentBone.Index]);
                boundingSphere = BoundingSphere.CreateMerged(boundingSphere, origMeshSphere);
            } 
            return boundingSphere;
        }
        /// <summary>
        /// 绘制字符串
        /// </summary>
        /// <param name="spriteBatch">spriteBatch</param>
        /// <param name="spriteFont">字体</param>
        /// <param name="text">字符串</param>
        /// <param name="position">坐标</param>
        public static void DrawString(SpriteBatch spriteBatch,SpriteFont spriteFont,String text, Vector2 position)
        {
            DrawString(spriteBatch, spriteFont, text, position, Color.Black);
        }
        /// <summary>
        /// 绘制字符串
        /// </summary>
        /// <param name="spriteBatch">spriteBatch</param>
        /// <param name="spriteFont">字体</param>
        /// <param name="text">字符串</param>
        /// <param name="position">坐标</param>
        /// <param name="color">颜色</param>
        public static void DrawString(SpriteBatch spriteBatch, SpriteFont spriteFont, String text, Vector2 position,Color color)
        {
            if (text == null) return;
            spriteBatch.Begin();
            spriteBatch.DrawString(spriteFont, text, position, color);
            spriteBatch.End();
        }
        private static BasicEffect effect;
        public static void DrawLine(Game game,GraphicsDeviceManager graphics, Vector2 p1, Vector2 p2, Color color)
        {
            if (effect == null)
            {
                effect = new BasicEffect(game.GraphicsDevice);
                effect.VertexColorEnabled = true;
            }
            float cw = graphics.PreferredBackBufferWidth / 2;
            float ch = graphics.PreferredBackBufferHeight / 2;
            VertexPositionColor[] v = new VertexPositionColor[2];
            v[0] = new VertexPositionColor(new Vector3(-1f + p1.X / cw, 1f - p1.Y / ch, 0), color);
            v[1] = new VertexPositionColor(new Vector3(-1f + p2.X / cw, 1f - p2.Y / ch, 0), color);
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                game.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                    PrimitiveType.LineList, v, 0, 1);
            }
        }

        public static void DrawRect(Game game, GraphicsDeviceManager graphics, Vector2 position, Vector2 size, Color color)
        {
            if (effect == null)
            {
                effect = new BasicEffect(game.GraphicsDevice);
                effect.VertexColorEnabled = true;
            }
            float cw = graphics.PreferredBackBufferWidth / 2;
            float ch = graphics.PreferredBackBufferHeight / 2;
            VertexPositionColor[] v = new VertexPositionColor[5];
            v[0] = new VertexPositionColor(new Vector3(-1f + (position.X) / cw, 1f - (position.Y) / ch, 0), color);
            v[1] = new VertexPositionColor(new Vector3(-1f + (position.X + size.X) / cw, 1f - (position.Y) / ch, 0), color);
            v[2] = new VertexPositionColor(new Vector3(-1f + (position.X + size.X) / cw, 1f - (position.Y + size.Y) / ch, 0), color);
            v[3] = new VertexPositionColor(new Vector3(-1f + (position.X) / cw, 1f - (position.Y + size.Y) / ch, 0), color);
            v[4] = new VertexPositionColor(new Vector3(-1f + (position.X) / cw, 1f - (position.Y) / ch, 0), color);
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                game.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                    PrimitiveType.LineStrip, v, 0, 4, VertexPositionColor.VertexDeclaration);
            }
        }

        public static void FillRect(Game game, GraphicsDeviceManager graphics, Vector2 position, Vector2 size, Color color)
        {
            if (effect == null)
            {
                effect = new BasicEffect(game.GraphicsDevice);
                effect.VertexColorEnabled = true;
            }
            float cw = graphics.PreferredBackBufferWidth / 2;
            float ch = graphics.PreferredBackBufferHeight / 2;
            VertexPositionColor[] v = new VertexPositionColor[4];
            v[0] = new VertexPositionColor(new Vector3(-1f + (position.X) / cw, 1f - (position.Y) / ch, 0), color);
            v[1] = new VertexPositionColor(new Vector3(-1f + (position.X + size.X) / cw, 1f - (position.Y) / ch, 0), color);
            v[2] = new VertexPositionColor(new Vector3(-1f + (position.X) / cw, 1f - (position.Y + size.Y) / ch, 0), color);
            v[3] = new VertexPositionColor(new Vector3(-1f + (position.X + size.X) / cw, 1f - (position.Y + size.Y) / ch, 0), color);
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                game.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                    PrimitiveType.TriangleStrip, v, 0, 2, VertexPositionColor.VertexDeclaration);
            }
        }
    }
}
