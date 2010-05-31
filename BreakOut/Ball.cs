using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace BreakOut
{
    class Ball
        {
        private bool active;
        private float x, y, dX, dY, radius;
        private Brush ballBrush;
        
        /// <summary>
        /// 画球的brush
        /// </summary>
        public Brush Brush
        {
            get { return ballBrush; }
            set { ballBrush = value; }
        }

        public float X { get { return x; } }
        public float Y { get { return y; } }
        public float Radius { get { return radius; } }
        public float Left { get { return x - radius; } }
        public float Right { get { return x + radius; } }
        public float Top { get { return y - radius; } }
        public float Bottom { get { return y + radius; } }
        public float XSpeed { get { return dX; } }
        public float YSpeed { get { return dY; } }

        /// <summary>
        /// 如果球是活动的则返回true，可以与砖块互相影响
        /// 除非这个球是增益道具生成并且还没碰到过板，否则为true
        /// </summary>
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        /// <summary>
        /// 创造一个新的活动的球
        /// </summary>
        /// <param name="ball">画球的brush</param>
        /// <param name="rad">球半径</param>
        /// <param name="centreX">在球中心初始化X坐标</param>
        /// <param name="centreY">在球中心初始化Y坐标</param>
        /// <param name="Xspeed">初始化球的x方向加速度. 正方向为右</param>
        /// <param name="Yspeed">初始化球的Y方向加速度. 正方向为下</param>
        public Ball(Brush ball, float rad, float centreX, float centreY, float Xspeed, float Yspeed)
        {
            radius = rad;
            x = centreX;
            y = centreY;
            dX = Xspeed;
            dY = Yspeed;
            ballBrush = ball;
            this.Active = true;
        }

        /// <summary>
        ///创造一个新球，说明是否活动
        /// </summary>
        /// <param name="ball">画球的brush</param>
        /// <param name="rad">球半径</param>
        /// <param name="centreX">在球中心初始化X坐标</param>
        /// <param name="centreY">在球中心初始化Y坐标</param>
        /// <param name="Xspeed">初始化球的x方向加速度. 正方向为右</param>
        /// <param name="Yspeed">初始化球的Y方向加速度. 正方向为下</param>
        /// <param name="activeBall">球会和板碰撞吗</param>
        public Ball(Brush ball, float rad, float centreX, float centreY, float Xspeed, float Yspeed, bool activeBall)
        {
            radius = rad;
            x = centreX;
            y = centreY;
            dX = Xspeed;
            dY = Yspeed;
            ballBrush = ball;
            active = activeBall;
        }

        /// <summary>
        /// 初始化一个球，Y方向速度为其半径，X方速度为0，用来表示增益道具产生的球
        /// </summary>
        /// <param name="ball">画球的brush</param>
        /// <param name="rad">球半径</param>
        /// <param name="centreX">在球中心初始化X坐标</param>
        /// <param name="centreY">在球中心初始化Y坐标</param>
        /// <param name="activeBall">球会和板碰撞吗</param>
        public Ball(Brush ball, float rad, float centreX, float centreY, bool activeBall)
        {
            radius = rad;
            x = centreX;
            y = centreY;
            dX = 0; 
            dY = radius; 
            ballBrush = ball;
            active = activeBall;
        }
         
        
        /// <summary>
        /// 在X,Y坐标处画出球
        /// </summary>
        /// <param name="paper">画球的Graphics</param>
        public void Draw(Graphics paper)
        {
            paper.FillEllipse(ballBrush, x - radius, y - radius, 2 * radius, 2 * radius);
        }

        /// <summary>
        /// 移动函数
        /// </summary>
        public void Move()
        {
            x += dX;
            y += dY;
        }

        /// <summary>
        /// 板和球的灰常先进的碰撞模型
        /// 如果球碰到了板的角，则球被反射反弹
        /// 如果球碰到了板的上部，Y方向速度反转
        /// 板如果在向一个方向移动，则它会给球在那个方向的一点速度
        /// 同样要确保球的速度不能超过其半径
        /// </summary>
        /// <param name="paddle">需要判断的板</param>
        public void Collide(Paddle paddle)
        {
            ReverseY(); //无论如何，先反转Y方向速度再说

            //如果碰到了任何一角，反转X速度
            if ((paddle.Top > this.Top +dY  && paddle.Top < this.Bottom + dY)
                && (paddle.Left > this.Left + dX && paddle.Left < this.Right + dX && dX > 0)
                || (paddle.Right > this.Left + dX && paddle.Right < this.Right + dX && dX < 0))
                ReverseX();

            int ratio = 4; //   板速度/球速度

            dX += (paddle.X - paddle.OldX)/ratio;
            dY += (paddle.Y - paddle.OldY)/ratio;
            //板速度对球速度产生的影响

            if (dX >  radius) dX = radius;
            if (dX < -radius) dX = -radius;
            if (dY > radius) dY = radius;
            if (dY < -radius) dY = -radius;
            //防止球的速度过快（超过半径），如果超过半径，碰撞检测无法检测到碰撞，可能是算法问题
            
            this.Active = true;
            //球之前如果没有激活，那么现在和板碰了之后激活（对于生成的球）
        }

        /// <summary>
        /// 重写的ToString(),把球的速度变成string型
        /// </summary>
        /// <returns>返回的形如  "Ball { x, y, X-velocity, Y-velocity }"</returns>
        public override string ToString()
        {
            string s = "Ball {X = " + x + ", Y = " + y + ", dX = " + dX + ", dY = " + dY + "}";
            return s;
        }

        /// <summary>
        /// 反转X速度
        /// </summary>
        public void ReverseX() { dX = -dX; }
        /// <summary>
        /// 反转Y速度
        /// </summary>
        public void ReverseY() { dY = -dY; }

    }
}
