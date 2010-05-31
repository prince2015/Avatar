using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Media;
using System.Diagnostics;

namespace BreakOut
{
    public partial class Form1 : Form
    {
        private Paddle paddle1;
        private int borderWidth = 0; // ��Ļ�ڱߵĿ��
        private int lives = 3;//�������
        private int score = 0;//��ҵ÷�
        private const int brickColumns = 10; //ש�������
        private const int brickRows = 7; //ש�������
        private int brickCount; //ש����
        private int level = 1;//��Ϸ�ؿ�
        private int bossCount = 0;//BOSS
        private Label boss;
        //��������Ϸ��������
        private SoundPlayer brickSound = new SoundPlayer(Resource1.brick);
        private SoundPlayer wallSound = new SoundPlayer(Resource1.wall);
        private SoundPlayer paddleSound = new SoundPlayer(Resource1.paddle);
        private SoundPlayer startSound=new SoundPlayer (Resource1.start);
        //��ʼ��ש������
        private Brick[,] brickArray = new Brick[brickColumns,brickRows];
        private List<Ball> ballList = new List<Ball>(1);
        private List<PowerUp> powerUpList = new List<PowerUp>();
        private Image[] backgrounds = { Resource1.desertBG, Resource1.CactusBG };
        
        public Form1()
        {
            this.DoubleBuffered = true;
            InitializeComponent();
            InitializeGame();

        }

        /// <summary>
        /// ��ʼ��һ������Ϸ����ש��/��/��ǿ������Ҫ�����ʱ�����
        /// </summary>
        private void InitializeGame()
        {
            powerUpList.Clear();
            this.BackgroundImage = backgrounds[level % backgrounds.Length];
            //ÿ�ػ�������
            //���ؿ�������ͼƬ����ʱ��ӵ�һ���ٴο�ʼ
            paddle1 = new Paddle(Brushes.Black, 300, 400, 50, 10);
            InitializeBall(level); //��ʼ��һ����
            int brickHeight = 20;//ש���
            int brickPadding = 5;//ש����
            int borderPadding = 50;//ש�鵽�߽�ľ���
            InitializeBricks(brickArray, brickColumns, brickRows, brickHeight, brickPadding, borderPadding,level);
            for (int j = 0; j < brickRows; j++)
            {
                for (int i = 0; i < brickColumns; i++)
                {
                    if (brickArray[i, j].Strength != 0) brickCount++;
                }
            }
            if (level ==1)
            {
                bossCount = 1;
                Image ba = Resource1.ball;
                this.boss = new Label();
                boss.Visible = true;
                boss.AutoSize = false;
                boss.Size = new Size(40, 40);
                boss.Image = ba;
                boss.Location = new Point((this.Size.Width-boss.Size .Width)/2,this.Size .Height/8 );
                this.Controls.Add(boss);
            }
        }

        private void CollideWithBoss(Ball ball)
        {
            
        }

        /// <summary>
        /// ��List ball���裬ʹ��Ļ��ֻ����һ���򣬲��Ҹ���������ٶȣ�����X�����Y����
        /// </summary>
        private void InitializeBall(int l)
        {
            ballList.Clear();
            Random randy = new Random();
            int radius = 7;
            int dX = randy.Next(-radius, radius);
            int dY;
            switch (l)
            {
                case 1: dY = 1;
                    break;
                case 2: dY = 2;
                    break;
                case 3: dY = 3;
                    break;
                case 4: dY = 4;
                    break;
                case 5: dY = 5;
                    break;
                case 6: dY = 6;
                    break;
                default: dY = 7;
                    break;

            }
            ballList.Add(new Ball(Brushes.White, radius, this.Width / 2, this.Height / 2,dX,dY)); //��ʼ����
        }

        /// <summary>
        /// ��ʼ��һ��2ά���飬����ש����������
        /// </summary>
        /// <param name="brickArray">��Ҫ������������</param>
        /// <param name="columns">ש������</param>
        /// <param name="rows">ש������</param>
        /// <param name="brickHeight">ÿ��ש��ĸ߶� (����)</param>
        /// <param name="brickPadding">ש���϶ (����)</param>
        /// <param name="borderPadding">���ÿ��ש��ı� (����)</param>
        private void InitializeBricks(Brick[,] brickArray, int columns, int rows, int brickHeight, int brickPadding, int borderPadding,int level)
        {
            int brickWidth = ((this.Width - borderPadding * 2) / brickColumns) - brickPadding;//���ש����
            int columnWidth = ((this.Width - borderPadding * 2) / brickColumns);
            int rowHeight = brickHeight + brickPadding;

            Random randy = new Random();
                for (int j = 0; j < brickRows; j++)
                {
                    for (int i = 0; i < brickColumns; i++)
                    {
                        int x = borderPadding + i * columnWidth;
                        int y = (j * rowHeight) + borderPadding;
                        brickArray[i, j] = new Brick(x, y, brickWidth, brickHeight, randy,level ); //����һ�����ש��
                    }
                }            
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            LoopThroughBalls(); //���ÿ�����״̬�������ƶ�
            LoopThroughPowerUps(); //���Ƿ�Ե����������
            if (ballList.Count == 0) LostAllBalls();
            paddle1.StorePosition();
            if (brickCount == 0&&bossCount ==0) NextLevel();
            this.Invalidate();  //�ػ�������Ļ
        }
        /// <summary>
        /// �������߽磬�壬ש�����ײ��������֮������ײ�����ʵ�ֱȽ����ѣ�
        /// ���û��ס�򣬴��ڴ����Ƴ������
        /// </summary>
        private void LoopThroughBalls()
        {
            for (int n = ballList.Count - 1; n >= 0; n--) //��ÿ�������ѭ��
            {
                if (ballList[n].Active == true) //��������û�����壬��ô����ש��
                {
                    DetectWallCollision(ballList[n]);
                    CheckBricks(ballList[n], brickArray); //������Ƿ��ש������ײ
                    CollideWithBoss();
                }
                DetectPaddleCollision(ballList[n], paddle1);
                ballList[n].Move();
                if (ballList[n].Top > this.Height) ballList.RemoveAt(n);//�Ƴ�������Ļ����
            }
        }

        /// <summary>
        /// ��齫Ҫ�����߽�����б�Ҫ��ʱ��ת��ķ���
        /// </summary>
        /// <param name="ball">�ж��Ƿ���ײ���������</param>
        private void DetectWallCollision(Ball ball)
        {
            bool hit = false;
            if ((ball.Right + ball.XSpeed > this.Width - borderWidth) //�ұ߽�
                || (ball.Left + ball.XSpeed < borderWidth)) //��߽�
            {
                hit = true;
                ball.ReverseX();
            }

            if (ball.Top + ball.YSpeed < borderWidth)//�ϱ߽�
            {
                hit = true;
                ball.ReverseY();
            }
            if (hit) { wallSound.Play(); }
        }

        /// <summary>
        /// �����������ש�����ײ�Ƿ���������������ж����ש������û���������
        /// </summary>
        /// <param name="ball">Ҫ������</param>
        /// <param name="bricks">Ҫ�����Ƿ��е��ߵ�ש��</param>
        private void CheckBricks(Ball ball, Brick[,] bricks)
        {
            bool ultimatelyReverseX = false; //����������ȷ����ֻ����һ��ʱ���ڱ���תһ��
            bool ultimatelyReverseY = false; //���������ֱ���ת����ֱ�Ӵ���ש������
                                        
            foreach (Brick brick in bricks)
            {
                bool hit, reverseX, reverseY;
                if (brick.Strength > 0) //ש��û������ʱ����Ƿ�����ײ
                {
                    //ִ����ײ���
                    DetectBrickCollision(ball, brick, out hit, out reverseX, out reverseY);
                    if (hit)
                    {
                        brick.Hit(); //��ש���һ��Ѫ
                        if (brick.Strength == 0)//ש��Ѫ��Ϊ0��ʱ��Ӧ����ʧ
                        {
                            CheckBrickForPowerUps(brick); //���ש�����Ƿ����������
                            brickCount--; //��ש������1
                        }
                        score += level * brick.CalculateScore();
                        brickSound.Play();
                        if (reverseX) ultimatelyReverseX = true;
                        if (reverseY) ultimatelyReverseY = true;
                    }
                }
            }
            if (ultimatelyReverseY) ball.ReverseY();
            if (ultimatelyReverseX) ball.ReverseX();
        }

        /// <summary>
        /// �����������ש���Ƿ�����ײ�������ж���ķ����Ƿ���Ҫ��ת
        /// </summary>
        /// <param name="ball">��Ҫ������</param>
        /// <param name="brick">��Ҫ����ש��</param>
        /// <param name="hit">true��������ײ</param>
        /// <param name="reverseX">��ֵΪtrueʱ��X�����ٶ���Ҫ����ת</param>
        /// <param name="reverseY">��ֵΪtrueʱ��Y�����ٶ���Ҫ����ת</param>
        private void DetectBrickCollision(Ball ball, Brick brick, out bool hit, out bool reverseX, out bool reverseY)
        {
            reverseX = false; //ÿ��ש��ֻ�ܷ�����һ��
            reverseY = false;
            hit = false;
            //����ٶȲ��ܳ�����ֱ������������޷���������������ֱ�Ӵ�Խש��

                //�����ƶ�ʱ����ײ�ж�
                if ((ball.YSpeed < 0) //�ж����Ƿ��������ƶ�
                && (ball.Top + ball.YSpeed < brick.Bottom) //�����ƶ�ʱ������ϱ߽�����ٶ����С��ש���±߽������±߽�����ٶȴ���ש���±߽磬��Y��������ײ
                && (ball.Bottom + ball.YSpeed > brick.Bottom)
                && (ball.Right + ball.XSpeed >= brick.Left) //��
                && (ball.Left + ball.XSpeed <= brick.Right)) //��
                {
                    hit = true;
                    reverseY = true;
                }

                //�����ƶ�ʱ����ײ�ж�
                if ((ball.YSpeed > 0) //�ж����Ƿ��������ƶ�
                && (ball.Bottom + ball.YSpeed > brick.Top) //ͬ�ϣ�������ײ�ж�
                && (ball.Top + ball.YSpeed < brick.Top) 
                && (ball.Right + ball.XSpeed >= brick.Left)
                && (ball.Left + ball.XSpeed <= brick.Right))
                {
                    hit = true;
                    reverseY = true;
                }

                //�����ƶ�ʱ����ײ�ж�
                if ((ball.XSpeed > 0) //�ж����Ƿ��������ƶ�
               && (ball.Left + ball.XSpeed < brick.Left) //ͬ�ϣ�������ײ�ж�
               && (ball.Right + ball.XSpeed > brick.Left) 
               && (ball.Bottom + ball.YSpeed >= brick.Top)
               && (ball.Top + ball.YSpeed <= brick.Bottom)) 
                {
                    hit = true;
                    reverseX = true;
                }

                //�����ƶ�ʱ����ײ�ж�
                if ((ball.XSpeed < 0) //�ж����Ƿ��������ƶ�
                    && (ball.Right + ball.XSpeed > brick.Right) //ͬ�ϣ�������ײ�ж�
                    && (ball.Left + ball.XSpeed < brick.Right) 
                    && (ball.Bottom + ball.YSpeed >= brick.Top)
                    && (ball.Top + ball.YSpeed <= brick.Bottom))
                {
                    hit = true;
                    reverseX = true;
                }
        }

        /// <summary>
        /// ����ײ����ã����ש�����Ƿ����������
        /// ����У����߽��ᱻ������Ϸ�е�list powerup
        /// </summary>
        /// <param name="brick">��Ҫ����ש��</param>
        private void CheckBrickForPowerUps(Brick brick)
        {
            if (brick.powerBall) //�������������Ч
            {
                ;
                int radius = 7; //�����ɵ���İ뾶�������
                ballList.Add(new Ball(Brushes.White , radius, brick.Centre.X, brick.Centre.Y, false));
                //��������ש����������ɵ�
            }

            //�������Ҫ������������Ч
            string powerUpLabel = null;
            Brush powerUpBrush = null;

            if (brick.powerShrink)
            {
                powerUpLabel = "����";
                powerUpBrush = Brushes.LightBlue;
            }
            if (brick.powerStretch)
            {
                powerUpLabel = "�쳤";
                powerUpBrush = Brushes.LightGreen;
            }
            if (brick.powerMulti)
            {
                powerUpLabel = "������";
                powerUpBrush = Brushes.Pink;
            }

            if (powerUpLabel != null)
            {
                PowerUp powerup = new PowerUp(powerUpLabel, powerUpBrush, brick.Centre.X, brick.Centre.Y);
                powerUpList.Add(powerup);
            } //ֻ��ש������������ߵ�����²�����������
        }

        /// <summary>
        /// �����Ͱ��Ƿ�����ײ����������͵���Collision()����
        /// </summary>
        /// <param name="ball">Ҫ������</param>
        /// <param name="paddle">Ҫ���İ�</param>
        private void DetectPaddleCollision(Ball ball, Paddle paddle)
        {
            if ((ball.Right + ball.XSpeed > paddle.Left) &&
                (ball.Left + ball.XSpeed < paddle.Right) &&
                (ball.Bottom + ball.YSpeed > paddle.Top) &&
                (ball.Top + ball.YSpeed < paddle.Bottom) &&
                ball.YSpeed >= 0) //ͬ�ϱ��жϣ�������ײ���
            {
                ball.Collide(paddle);
                paddleSound.Play();
            }
        }

        /// <summary>
        /// ��ͼ����������������ש�飬�壬������ߣ���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawBackground(e.Graphics);
            foreach (Brick brick in brickArray) if (brick.Strength > 0) brick.Draw(e.Graphics);
            paddle1.Draw(e.Graphics);
            foreach (PowerUp powerup in powerUpList) powerup.Draw(e.Graphics);
            foreach(Ball ball in ballList) ball.Draw(e.Graphics);
        }
        //������ƶ���ĺ���
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            paddle1.X = e.X - paddle1.Width / 2;
            //�����X����ʱ�̱��������һ��
            
            if (timer1.Enabled == false) this.Invalidate();
            //ֻ�е���ʱ���ڼ���״̬�²�redraw
        }


        /// <summary>
        /// �ƶ�������ߣ����ÿ�����ߺͰ�Ĺ�ϵ
        /// ����������������ApplyPowerUp()����
        /// �������Ե����ߵ�����Ļ������Ϸ���Ƴ��������
        /// </summary>
        private void LoopThroughPowerUps()
        {
            for (int n = powerUpList.Count - 1; n >= 0; n--)
            {
                bool remove = false;
                if (powerUpList[n].Top > this.Height) remove = true;
                //������Ļ���Ƴ�
                
                Rectangle powerRect = powerUpList[n].Rectangle;
                Rectangle paddleRect = paddle1.Rectangle;
                if (powerRect.IntersectsWith(paddleRect)) //����͵��ߵ�λ�ù�ϵ
                {
                    ApplyPowerUp(powerUpList[n].Type);
                    remove = true;
                }
                if (remove) powerUpList.RemoveAt(n); //��ѭ����β�Ƴ����ߣ������м��Ƴ�
                else powerUpList[n].Move();
            }
        }

        /// <summary>
        /// ���������˵���ʱ����ã�����Ϸ������������Ч��.
        /// </summary>
        /// <param name="type">��������</param>
        private void ApplyPowerUp(string type)
        {
            if (type == "����")//�Ѱ����̣����ǲ�����̫��
            {
                if (paddle1.Width > 40) paddle1.Width = paddle1.Width - 20;
            }
            if (type == "�쳤") //�쳤
            {
                paddle1.Width = paddle1.Width + 20;
            }
            if (type == "������") //�������
            {
                Ball ball = ballList[0];
                double angle = Math.Atan2(-ball.YSpeed, ball.XSpeed);
                double speed = Math.Sqrt(ball.XSpeed * ball.XSpeed + ball.YSpeed * ball.YSpeed);
                double angle1 = angle + 2*Math.PI / 3; //��ʼ�Ƕȼ�120��
                double angle2 = angle - 2*Math.PI / 3; //��ʼ�Ƕȼ�120��
                //���ٶȴӼ�����ת����ֱ������ϵ
                float dX1 = (float)(Math.Cos(angle1) * speed);
                float dY1 = (float)(-Math.Sin(angle1) * speed);
                float dX2 = (float)(Math.Cos(angle2) * speed);
                float dY2 = (float)(-Math.Sin(angle2) * speed);
                //�����һ�����λ�������������ӵ�в�ͬ���ٶ�
                //����Ϸ�л���һ������������Ч���ῴ�����ܻ������۹�����
                ballList.Add(new Ball(ball.Brush, ball.Radius, ball.X, ball.Y, dX1, dY1));
                ballList.Add(new Ball(ball.Brush, ball.Radius, ball.X, ball.Y, dX2, dY2));
            }
        }

        /// <summary>
        /// ��Ļ�ϻ����߽�
        /// </summary>
        /// <param name="paper">һ��Graphics object����Ҫ���仭������</param>
        private void DrawBackground(Graphics paper)
        {
            paper.FillRectangle(Brushes.Black,0,0,borderWidth,this.Height); //��߽�
            paper.FillRectangle(Brushes.Black,0,0,this.Width, borderWidth); //�ϱ߽�
            paper.FillRectangle(Brushes.Black,this.Width-borderWidth,0,borderWidth,this.Height); //�ұ߽�
            Font statsFont = new Font("΢���ź�", 8);
            paper.DrawString(
                "Level: " + level + 
                " Score: " + score,
                statsFont, Brushes.White, borderWidth, 1);
            //�����Ͻǻ��������͹ؿ�
            paper.DrawString("Lives: " + lives, statsFont, Brushes.White, this.Width - 45 - borderWidth, 1);
            //�����Ͻǻ�������
        }

        /// <summary>
        /// �����Ļ��һ����û�е�ʱ�����
        /// </summary>
        private void LostAllBalls()
        {
            lives -= 1;             //������һ
            timer1.Enabled = false; //��ͣ��Ϸ
            InitializeBall(level);       //����һ������
            powerUpList.Clear();    //�������
            if (lives < 0) GameOver();//����С��0����Ϸ����...������ҽ��͵�̫������...��
        }

        /// <summary>
        /// ֪ͨ��ҷ���֮�����¿�ʼ��Ϸ
        /// </summary>
        private void GameOver()
        {
            MessageBox.Show("��...���а��㣬��ô��͹��ˣ��� " + score + " �֣�̫���˰�");
            lives = 3;
            score = 0;
            InitializeGame();
        }

        /// <summary>
        /// ÿͨ��һ�أ�������Ϸ����һ������
        /// </summary>
        private void NextLevel()
        {
            timer1.Enabled = false;
            lives++; 
            level++; 
            MessageBox.Show("ţB�ˣ���һ��"+level +"����Ŭ���ɣ�������һ������");
            InitializeGame();
        }
        //�������ҿ����ƶ����ո��ǿ�ʼ��Ϸ
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString().Equals("Left"))
            {
                paddle1.X -= 20;
            }
            if (e.KeyCode.ToString().Equals("Right"))
            {
                paddle1.X += 20;
            }
            if (e.KeyCode.ToString().Equals("Space"))//���ո�ʼ
            {
                timer1.Enabled = true;
                
            }
            if (e.KeyCode.ToString().Equals("A"))//��A��ͣ���߻ָ�
            {
                if (timer1.Enabled == true)
                    timer1.Enabled = false;
                else
                    timer1.Enabled = false;
            }
            if (timer1.Enabled == false) this.Invalidate();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            startSound.Play();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
       
    }
}