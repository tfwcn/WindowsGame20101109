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

namespace MyWindowsGameLibrary.AI
{
    public class MyAStarPoint
    {
        private MyAStarPoint parentPoint;
        /// <summary>
        /// 父点
        /// </summary>
        public MyAStarPoint ParentPoint
        {
            get { return parentPoint; }
            set { parentPoint = value; }
        }
        private int x;
        /// <summary>
        /// X坐标
        /// </summary>
        public int X
        {
            get { return x; }
            set { x = value; }
        }
        private int y;
        /// <summary>
        /// Y坐标
        /// </summary>
        public int Y
        {
            get { return y; }
            set { y = value; }
        }
        private int g;
        /// <summary>
        /// 起点到当前点的距离
        /// </summary>
        public int G
        {
            get { return g; }
            set { g = value; f = g + h; }
        }
        private int h;
        /// <summary>
        /// 终点到当前点的估计距离
        /// </summary>
        public int H
        {
            get { return h; }
            set { h = value; f = g + h; }
        }
        private int f;
        /// <summary>
        /// 总距离(G+H)
        /// </summary>
        public int F
        {
          get { return f; }
          set { f = value; }
        }

        public MyAStarPoint()
        {
            this.x = 0;
            this.y = 0;
        }
        public MyAStarPoint(int x,int y)
        {
            this.x = x;
            this.y = y;
        }
        public MyAStarPoint(int x, int y,int g,int h, MyAStarPoint parentPoint)
        {
            this.x = x;
            this.y = y;
            this.g = g;
            this.h = h;
            this.f = g+h;
            this.parentPoint = parentPoint;
        }
    }
    public class MyAStar
    {
        private int width;

        public int Width
        {
            get { return width; }
            set { width = value; }
        }
        private int height;

        public int Height
        {
            get { return height; }
            set { height = value; }
        }
        private MyAStarPoint start;

        public MyAStarPoint Start
        {
            get { return start; }
            set { start = value;}
        }
        private MyAStarPoint end;

        public MyAStarPoint End
        {
            get { return end; }
            set { end = value; }
        }
        private List<MyAStarPoint> startList;
        private List<MyAStarPoint> wallList;

        public List<MyAStarPoint> WallList
        {
            get { return wallList; }
            set { wallList = value; }
        }

        public MyAStar(int width, int height, MyAStarPoint start, MyAStarPoint end)
        {
            startList = new List<MyAStarPoint>();
            startList.Add(start);
            this.wallList = new List<MyAStarPoint>();
            this.start = start;
            this.end = end;
            this.width = width;
            this.height = height;
        }
        /// <summary>
        /// 逆序输出路径列表
        /// </summary>
        /// <returns>List"AStarPoint"</returns>
        public List<MyAStarPoint> FindRoad()
        {
            List<MyAStarPoint> roadList=null;
            MyAStarPoint nowPoint = null;
            startList.Clear();
            startList.Add(start);
            do
            {
                int minF=-1;
                for (int i = startList.Count - 1; i >= 0; i--)
                {
                    MyAStarPoint p = startList[i];
                    if (minF == -1 || p.F < minF)
                    {
                        minF = p.F;
                        nowPoint = p;
                    }
                }
                AddStartList(nowPoint);
            } while (!(startList.Count==0 || (nowPoint.ParentPoint!=null&&nowPoint.H == 0)));

            roadList = new List<MyAStarPoint>();
            if (nowPoint != null && nowPoint.H == 0)
            {
                roadList.Add(nowPoint);
                while (nowPoint.ParentPoint != null)
                {
                    nowPoint = nowPoint.ParentPoint;
                    roadList.Add(nowPoint);
                }
            }
            return roadList;
        }
        private void AddStartList(MyAStarPoint point)
        {
            int x,y;
            x=point.X - 1;
            y=point.Y - 1;
            //上左
            if (x >= 0 && y >= 0)
            {
                if (IsClose(x, y) == null)//检查是否在关闭列表
                {
                    if (IsClose(x, y + 1) == null && IsClose(x + 1, y) == null)//检查是否有障碍物
                    {
                        MyAStarPoint s = IsStart(x, y);
                        if (s == null)//检查是否已存在于开始列表
                        {
                            int g = point.G + 14;
                            int h = (Math.Abs(x - end.X) + Math.Abs(y - end.Y)) * 10;
                            startList.Add(new MyAStarPoint(x, y, g, h, point));

                        }
                        else
                        {
                            if (point.G + 14 < s.G)//更新最短路径
                            {
                                s.ParentPoint = point;
                                s.G = point.G + 14;
                            }
                        }
                    }
                }
            }
            x = point.X;
            y = point.Y - 1;
            //上中
            if (y >= 0)
            {
                if (IsClose(x, y) == null)//检查是否在关闭列表
                {
                    MyAStarPoint s = IsStart(x, y);
                    if (s == null)//检查是否已存在于开始列表
                    {
                        int g = point.G + 10;
                        int h = (Math.Abs(x - end.X) + Math.Abs(y - end.Y)) * 10;
                        startList.Add(new MyAStarPoint(x, y, g, h, point));
                    }
                    else
                    {
                        if (point.G + 10 < s.G)//更新最短路径
                        {
                            s.ParentPoint = point;
                            s.G = point.G + 10;
                        }
                    }
                }
            }
            x = point.X+1;
            y = point.Y - 1;
            //上右
            if (x < width && y >= 0)
            {
                if (IsClose(x, y) == null)//检查是否在关闭列表
                {
                    if (IsClose(x, y + 1) == null && IsClose(x - 1, y) == null)//检查是否有障碍物
                    {
                        MyAStarPoint s = IsStart(x, y);
                        if (s == null)//检查是否已存在于开始列表
                        {
                            int g = point.G + 14;
                            int h = (Math.Abs(x - end.X) + Math.Abs(y - end.Y)) * 10;
                            startList.Add(new MyAStarPoint(x, y, g, h, point));

                        }
                        else
                        {
                            if (point.G + 14 < s.G)//更新最短路径
                            {
                                s.ParentPoint = point;
                                s.G = point.G + 14;
                            }
                        }
                    }
                }
            }
            x = point.X-1;
            y = point.Y;
            //中左
            if (x >= 0)
            {
                if (IsClose(x, y) == null)//检查是否在关闭列表
                {
                    MyAStarPoint s = IsStart(x, y);
                    if (s == null)//检查是否已存在于开始列表
                    {
                            int g = point.G + 10;
                            int h = (Math.Abs(x - end.X) + Math.Abs(y - end.Y)) * 10;
                            startList.Add(new MyAStarPoint(x, y, g, h, point));
                    }
                    else
                    {
                        if (point.G + 10 < s.G)//更新最短路径
                        {
                            s.ParentPoint = point;
                            s.G = point.G + 10;
                        }
                    }
                }
            }
            x = point.X+1;
            y = point.Y;
            //中右
            if (x < width)
            {
                if (IsClose(x, y) == null)//检查是否在关闭列表
                {
                    MyAStarPoint s = IsStart(x, y);
                    if (s == null)//检查是否已存在于开始列表
                    {
                            int g = point.G + 10;
                            int h = (Math.Abs(x - end.X) + Math.Abs(y - end.Y)) * 10;
                            startList.Add(new MyAStarPoint(x, y, g, h, point));
                    }
                    else
                    {
                        if (point.G + 10 < s.G)//更新最短路径
                        {
                            s.ParentPoint = point;
                            s.G = point.G + 10;
                        }
                    }
                }
            }
            x = point.X-1;
            y = point.Y + 1;
            //下左
            if (x >= 0 && y < height)
            {
                if (IsClose(x, y) == null)//检查是否在关闭列表
                {
                    if (IsClose(x, y - 1) == null && IsClose(x + 1, y) == null)//检查是否有障碍物
                    {
                        MyAStarPoint s = IsStart(x, y);
                        if (s == null)//检查是否已存在于开始列表
                        {

                            int g = point.G + 14;
                            int h = (Math.Abs(x - end.X) + Math.Abs(y - end.Y)) * 10;
                            startList.Add(new MyAStarPoint(x, y, g, h, point));

                        }
                        else
                        {
                            if (point.G + 14 < s.G)//更新最短路径
                            {
                                s.ParentPoint = point;
                                s.G = point.G + 14;
                            }
                        }
                    }
                }
            }
            x = point.X;
            y = point.Y + 1;
            //下中
            if (y < height)
            {
                if (IsClose(x, y) == null)//检查是否在关闭列表
                {
                    MyAStarPoint s = IsStart(x, y);
                    if (s == null)//检查是否已存在于开始列表
                    {
                            int g = point.G + 10;
                            int h = (Math.Abs(x - end.X) + Math.Abs(y - end.Y)) * 10;
                            startList.Add(new MyAStarPoint(x, y, g, h, point));
                    }
                    else
                    {
                        if (point.G + 10 < s.G)//更新最短路径
                        {
                            s.ParentPoint = point;

                            s.G = point.G + 10;
                        }
                    }
                }
            }
            x = point.X+1;
            y = point.Y + 1;
            //下右
            if (x < width && y < height)
            {
                if (IsClose(x, y) == null)//检查是否在关闭列表
                {
                    if (IsClose(x, y - 1) == null && IsClose(x - 1, y) == null)//检查是否有障碍物
                    {
                        MyAStarPoint s = IsStart(x, y);
                        if (s == null)//检查是否已存在于开始列表
                        {
                            int g = point.G + 14;
                            int h = (Math.Abs(x - end.X) + Math.Abs(y - end.Y)) * 10;
                            startList.Add(new MyAStarPoint(x, y, g, h, point));

                        }
                        else
                        {
                            if (point.G + 14 < s.G)//更新最短路径
                            {
                                s.ParentPoint = point;

                                s.G = point.G + 14;
                            }
                        }
                    }
                }
            } 
            if (startList.Remove(point))
                wallList.Add(point);
        }
        /// <summary>
        /// 检查点是否在开始列表内
        /// </summary>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <returns>AStarPoint</returns>
        private MyAStarPoint IsStart(int x, int y)
        {
            foreach (MyAStarPoint p in startList)
            {
                if (p.X == x && p.Y == y)
                {
                    return p;
                }
            }
            return null;
        }
        /// <summary>
        /// 检查点是否在关闭列表内
        /// </summary>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <returns>AStarPoint</returns>
        private MyAStarPoint IsClose(int x, int y)
        {
            foreach (MyAStarPoint p in wallList)
            {
                if (p.X == x && p.Y == y)
                {
                    return p;
                }
            }
            return null;
        }
    }
}
