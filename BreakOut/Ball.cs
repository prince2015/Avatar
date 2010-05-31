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
        /// �����brush
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
        /// ������ǻ���򷵻�true��������ש�黥��Ӱ��
        /// ���������������������ɲ��һ�û�������壬����Ϊtrue
        /// </summary>
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        /// <summary>
        /// ����һ���µĻ����
        /// </summary>
        /// <param name="ball">�����brush</param>
        /// <param name="rad">��뾶</param>
        /// <param name="centreX">�������ĳ�ʼ��X����</param>
        /// <param name="centreY">�������ĳ�ʼ��Y����</param>
        /// <param name="Xspeed">��ʼ�����x������ٶ�. ������Ϊ��</param>
        /// <param name="Yspeed">��ʼ�����Y������ٶ�. ������Ϊ��</param>
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
        ///����һ������˵���Ƿ�
        /// </summary>
        /// <param name="ball">�����brush</param>
        /// <param name="rad">��뾶</param>
        /// <param name="centreX">�������ĳ�ʼ��X����</param>
        /// <param name="centreY">�������ĳ�ʼ��Y����</param>
        /// <param name="Xspeed">��ʼ�����x������ٶ�. ������Ϊ��</param>
        /// <param name="Yspeed">��ʼ�����Y������ٶ�. ������Ϊ��</param>
        /// <param name="activeBall">���Ͱ���ײ��</param>
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
        /// ��ʼ��һ����Y�����ٶ�Ϊ��뾶��X���ٶ�Ϊ0��������ʾ������߲�������
        /// </summary>
        /// <param name="ball">�����brush</param>
        /// <param name="rad">��뾶</param>
        /// <param name="centreX">�������ĳ�ʼ��X����</param>
        /// <param name="centreY">�������ĳ�ʼ��Y����</param>
        /// <param name="activeBall">���Ͱ���ײ��</param>
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
        /// ��X,Y���괦������
        /// </summary>
        /// <param name="paper">�����Graphics</param>
        public void Draw(Graphics paper)
        {
            paper.FillEllipse(ballBrush, x - radius, y - radius, 2 * radius, 2 * radius);
        }

        /// <summary>
        /// �ƶ�����
        /// </summary>
        public void Move()
        {
            x += dX;
            y += dY;
        }

        /// <summary>
        /// �����Ļҳ��Ƚ�����ײģ��
        /// ����������˰�Ľǣ����򱻷��䷴��
        /// ����������˰���ϲ���Y�����ٶȷ�ת
        /// ���������һ�������ƶ���������������Ǹ������һ���ٶ�
        /// ͬ��Ҫȷ������ٶȲ��ܳ�����뾶
        /// </summary>
        /// <param name="paddle">��Ҫ�жϵİ�</param>
        public void Collide(Paddle paddle)
        {
            ReverseY(); //������Σ��ȷ�תY�����ٶ���˵

            //����������κ�һ�ǣ���תX�ٶ�
            if ((paddle.Top > this.Top +dY  && paddle.Top < this.Bottom + dY)
                && (paddle.Left > this.Left + dX && paddle.Left < this.Right + dX && dX > 0)
                || (paddle.Right > this.Left + dX && paddle.Right < this.Right + dX && dX < 0))
                ReverseX();

            int ratio = 4; //   ���ٶ�/���ٶ�

            dX += (paddle.X - paddle.OldX)/ratio;
            dY += (paddle.Y - paddle.OldY)/ratio;
            //���ٶȶ����ٶȲ�����Ӱ��

            if (dX >  radius) dX = radius;
            if (dX < -radius) dX = -radius;
            if (dY > radius) dY = radius;
            if (dY < -radius) dY = -radius;
            //��ֹ����ٶȹ��죨�����뾶������������뾶����ײ����޷���⵽��ײ���������㷨����
            
            this.Active = true;
            //��֮ǰ���û�м����ô���ںͰ�����֮�󼤻�������ɵ���
        }

        /// <summary>
        /// ��д��ToString(),������ٶȱ��string��
        /// </summary>
        /// <returns>���ص�����  "Ball { x, y, X-velocity, Y-velocity }"</returns>
        public override string ToString()
        {
            string s = "Ball {X = " + x + ", Y = " + y + ", dX = " + dX + ", dY = " + dY + "}";
            return s;
        }

        /// <summary>
        /// ��תX�ٶ�
        /// </summary>
        public void ReverseX() { dX = -dX; }
        /// <summary>
        /// ��תY�ٶ�
        /// </summary>
        public void ReverseY() { dY = -dY; }

    }
}
