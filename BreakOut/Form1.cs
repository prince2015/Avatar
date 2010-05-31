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
        private int borderWidth = 0; // 屏幕黑边的宽度
        private int lives = 3;//玩家生命
        private int score = 0;//玩家得分
        private const int brickColumns = 10; //砖块的列数
        private const int brickRows = 7; //砖块的行数
        private int brickCount; //砖块数
        private int level = 1;//游戏关卡
        private int bossCount = 0;//BOSS
        private Label boss;
        //以下是游戏声音部分
        private SoundPlayer brickSound = new SoundPlayer(Resource1.brick);
        private SoundPlayer wallSound = new SoundPlayer(Resource1.wall);
        private SoundPlayer paddleSound = new SoundPlayer(Resource1.paddle);
        private SoundPlayer startSound=new SoundPlayer (Resource1.start);
        //初始化砖块数组
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
        /// 初始化一个新游戏，当砖块/球/增强道具需要重设的时候调用
        /// </summary>
        private void InitializeGame()
        {
            powerUpList.Clear();
            this.BackgroundImage = backgrounds[level % backgrounds.Length];
            //每关换个背景
            //当关卡数大于图片数的时候从第一张再次开始
            paddle1 = new Paddle(Brushes.Black, 300, 400, 50, 10);
            InitializeBall(level); //初始化一个球
            int brickHeight = 20;//砖块高
            int brickPadding = 5;//砖块间距
            int borderPadding = 50;//砖块到边界的距离
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
        /// 把List ball重设，使屏幕上只存在一个球，并且赋予随机的速度（包括X方向和Y方向）
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
            ballList.Add(new Ball(Brushes.White, radius, this.Width / 2, this.Height / 2,dX,dY)); //初始化球
        }

        /// <summary>
        /// 初始化一个2维数组，并用砖块数组填满
        /// </summary>
        /// <param name="brickArray">需要被填满的数组</param>
        /// <param name="columns">砖块列数</param>
        /// <param name="rows">砖块行数</param>
        /// <param name="brickHeight">每个砖块的高度 (像素)</param>
        /// <param name="brickPadding">砖块间隙 (像素)</param>
        /// <param name="borderPadding">填充每组砖块的边 (像素)</param>
        private void InitializeBricks(Brick[,] brickArray, int columns, int rows, int brickHeight, int brickPadding, int borderPadding,int level)
        {
            int brickWidth = ((this.Width - borderPadding * 2) / brickColumns) - brickPadding;//算出砖块宽度
            int columnWidth = ((this.Width - borderPadding * 2) / brickColumns);
            int rowHeight = brickHeight + brickPadding;

            Random randy = new Random();
                for (int j = 0; j < brickRows; j++)
                {
                    for (int i = 0; i < brickColumns; i++)
                    {
                        int x = borderPadding + i * columnWidth;
                        int y = (j * rowHeight) + borderPadding;
                        brickArray[i, j] = new Brick(x, y, brickWidth, brickHeight, randy,level ); //加入一个随机砖块
                    }
                }            
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            LoopThroughBalls(); //检查每个球的状态并将其移动
            LoopThroughPowerUps(); //板是否吃到了增益道具
            if (ballList.Count == 0) LostAllBalls();
            paddle1.StorePosition();
            if (brickCount == 0&&bossCount ==0) NextLevel();
            this.Invalidate();  //重画整个屏幕
        }
        /// <summary>
        /// 检查球与边界，板，砖块的碰撞（球与球之间无碰撞，这个实现比较困难）
        /// 如果没接住球，从内存里移除这个球
        /// </summary>
        private void LoopThroughBalls()
        {
            for (int n = ballList.Count - 1; n >= 0; n--) //对每个球进行循环
            {
                if (ballList[n].Active == true) //如果这个球还没碰到板，那么忽视砖块
                {
                    DetectWallCollision(ballList[n]);
                    CheckBricks(ballList[n], brickArray); //检查球是否和砖块有碰撞
                    CollideWithBoss();
                }
                DetectPaddleCollision(ballList[n], paddle1);
                ballList[n].Move();
                if (ballList[n].Top > this.Height) ballList.RemoveAt(n);//移除掉出屏幕的球
            }
        }

        /// <summary>
        /// 检查将要碰到边界的球，有必要的时候反转球的方向
        /// </summary>
        /// <param name="ball">判断是否碰撞的球的名字</param>
        private void DetectWallCollision(Ball ball)
        {
            bool hit = false;
            if ((ball.Right + ball.XSpeed > this.Width - borderWidth) //右边界
                || (ball.Left + ball.XSpeed < borderWidth)) //左边界
            {
                hit = true;
                ball.ReverseX();
            }

            if (ball.Top + ball.YSpeed < borderWidth)//上边界
            {
                hit = true;
                ball.ReverseY();
            }
            if (hit) { wallSound.Play(); }
        }

        /// <summary>
        /// 检查给定的球和砖块的碰撞是否发生，如果发生，判断这个砖块里有没有增益道具
        /// </summary>
        /// <param name="ball">要检测的球</param>
        /// <param name="bricks">要检测的是否含有道具的砖块</param>
        private void CheckBricks(Ball ball, Brick[,] bricks)
        {
            bool ultimatelyReverseX = false; //这两个变量确保球只能在一个时钟内被反转一次
            bool ultimatelyReverseY = false; //，否则会出现被反转两次直接穿过砖块的情况
                                        
            foreach (Brick brick in bricks)
            {
                bool hit, reverseX, reverseY;
                if (brick.Strength > 0) //砖块没被碰到时检测是否发生碰撞
                {
                    //执行碰撞检测
                    DetectBrickCollision(ball, brick, out hit, out reverseX, out reverseY);
                    if (hit)
                    {
                        brick.Hit(); //给砖块减一格血
                        if (brick.Strength == 0)//砖块血量为0的时候应该消失
                        {
                            CheckBrickForPowerUps(brick); //检测砖块里是否有增益道具
                            brickCount--; //将砖块数减1
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
        /// 检查给定的球和砖块是否发生碰撞，并且判断球的方向是否需要反转
        /// </summary>
        /// <param name="ball">需要检测的球</param>
        /// <param name="brick">需要检测的砖块</param>
        /// <param name="hit">true代表发生碰撞</param>
        /// <param name="reverseX">其值为true时，X方向速度需要被反转</param>
        /// <param name="reverseY">其值为true时，Y方向速度需要被反转</param>
        private void DetectBrickCollision(Ball ball, Brick brick, out bool hit, out bool reverseX, out bool reverseY)
        {
            reverseX = false; //每个砖块只能反弹球一次
            reverseY = false;
            hit = false;
            //球的速度不能超过其直径，否则可能无法正常工作，可能直接穿越砖块

                //向上移动时的碰撞判断
                if ((ball.YSpeed < 0) //判断球是否在向上移动
                && (ball.Top + ball.YSpeed < brick.Bottom) //向上移动时，球的上边界加上速度如果小于砖块下边界而球的下边界加上速度大于砖块下边界，即Y方向发生碰撞
                && (ball.Bottom + ball.YSpeed > brick.Bottom)
                && (ball.Right + ball.XSpeed >= brick.Left) //右
                && (ball.Left + ball.XSpeed <= brick.Right)) //左
                {
                    hit = true;
                    reverseY = true;
                }

                //向下移动时的碰撞判断
                if ((ball.YSpeed > 0) //判断球是否在向下移动
                && (ball.Bottom + ball.YSpeed > brick.Top) //同上，都是碰撞判断
                && (ball.Top + ball.YSpeed < brick.Top) 
                && (ball.Right + ball.XSpeed >= brick.Left)
                && (ball.Left + ball.XSpeed <= brick.Right))
                {
                    hit = true;
                    reverseY = true;
                }

                //向右移动时的碰撞判断
                if ((ball.XSpeed > 0) //判断球是否在向右移动
               && (ball.Left + ball.XSpeed < brick.Left) //同上，都是碰撞判断
               && (ball.Right + ball.XSpeed > brick.Left) 
               && (ball.Bottom + ball.YSpeed >= brick.Top)
               && (ball.Top + ball.YSpeed <= brick.Bottom)) 
                {
                    hit = true;
                    reverseX = true;
                }

                //向左移动时的碰撞判断
                if ((ball.XSpeed < 0) //判断球是否在向左移动
                    && (ball.Right + ball.XSpeed > brick.Right) //同上，都是碰撞判断
                    && (ball.Left + ball.XSpeed < brick.Right) 
                    && (ball.Bottom + ball.YSpeed >= brick.Top)
                    && (ball.Top + ball.YSpeed <= brick.Bottom))
                {
                    hit = true;
                    reverseX = true;
                }
        }

        /// <summary>
        /// 在碰撞后调用，检查砖块里是否有增益道具
        /// 如果有，道具将会被加入游戏中的list powerup
        /// </summary>
        /// <param name="brick">需要检测的砖块</param>
        private void CheckBrickForPowerUps(Brick brick)
        {
            if (brick.powerBall) //额外的球立即生效
            {
                ;
                int radius = 7; //新生成的球的半径是随机的
                ballList.Add(new Ball(Brushes.White , radius, brick.Centre.X, brick.Centre.Y, false));
                //新球是在砖块的中心生成的
            }

            //增益道具要被板碰到才有效
            string powerUpLabel = null;
            Brush powerUpBrush = null;

            if (brick.powerShrink)
            {
                powerUpLabel = "缩短";
                powerUpBrush = Brushes.LightBlue;
            }
            if (brick.powerStretch)
            {
                powerUpLabel = "伸长";
                powerUpBrush = Brushes.LightGreen;
            }
            if (brick.powerMulti)
            {
                powerUpLabel = "多重球";
                powerUpBrush = Brushes.Pink;
            }

            if (powerUpLabel != null)
            {
                PowerUp powerup = new PowerUp(powerUpLabel, powerUpBrush, brick.Centre.X, brick.Centre.Y);
                powerUpList.Add(powerup);
            } //只在砖块里有增益道具的情况下才添加增益道具
        }

        /// <summary>
        /// 检查球和板是否发生碰撞，如果发生就调用Collision()函数
        /// </summary>
        /// <param name="ball">要检测的球</param>
        /// <param name="paddle">要检测的板</param>
        private void DetectPaddleCollision(Ball ball, Paddle paddle)
        {
            if ((ball.Right + ball.XSpeed > paddle.Left) &&
                (ball.Left + ball.XSpeed < paddle.Right) &&
                (ball.Bottom + ball.YSpeed > paddle.Top) &&
                (ball.Top + ball.YSpeed < paddle.Bottom) &&
                ball.YSpeed >= 0) //同上边判断，都是碰撞检测
            {
                ball.Collide(paddle);
                paddleSound.Play();
            }
        }

        /// <summary>
        /// 画图函数，画出背景，砖块，板，增益道具，球
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
        //用鼠标移动板的函数
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            paddle1.X = e.X - paddle1.Width / 2;
            //将板的X坐标时刻保持与鼠标一致
            
            if (timer1.Enabled == false) this.Invalidate();
            //只有当计时器在激活状态下才redraw
        }


        /// <summary>
        /// 移动增益道具，检查每个道具和板的关系
        /// 如果发生交错，则调用ApplyPowerUp()函数
        /// 如果被板吃掉或者掉出屏幕，从游戏中移除这个道具
        /// </summary>
        private void LoopThroughPowerUps()
        {
            for (int n = powerUpList.Count - 1; n >= 0; n--)
            {
                bool remove = false;
                if (powerUpList[n].Top > this.Height) remove = true;
                //掉出屏幕，移除
                
                Rectangle powerRect = powerUpList[n].Rectangle;
                Rectangle paddleRect = paddle1.Rectangle;
                if (powerRect.IntersectsWith(paddleRect)) //检查板和道具的位置关系
                {
                    ApplyPowerUp(powerUpList[n].Type);
                    remove = true;
                }
                if (remove) powerUpList.RemoveAt(n); //在循环结尾移除道具，不在中间移除
                else powerUpList[n].Move();
            }
        }

        /// <summary>
        /// 当板碰到了道具时候调用，向游戏中添加这个道具效果.
        /// </summary>
        /// <param name="type">道具名称</param>
        private void ApplyPowerUp(string type)
        {
            if (type == "缩短")//把板缩短，但是不至于太短
            {
                if (paddle1.Width > 40) paddle1.Width = paddle1.Width - 20;
            }
            if (type == "伸长") //伸长
            {
                paddle1.Width = paddle1.Width + 20;
            }
            if (type == "多重球") //把球分裂
            {
                Ball ball = ballList[0];
                double angle = Math.Atan2(-ball.YSpeed, ball.XSpeed);
                double speed = Math.Sqrt(ball.XSpeed * ball.XSpeed + ball.YSpeed * ball.YSpeed);
                double angle1 = angle + 2*Math.PI / 3; //初始角度加120度
                double angle2 = angle - 2*Math.PI / 3; //初始角度减120度
                //把速度从极坐标转换到直角坐标系
                float dX1 = (float)(Math.Cos(angle1) * speed);
                float dY1 = (float)(-Math.Sin(angle1) * speed);
                float dX2 = (float)(Math.Cos(angle2) * speed);
                float dY2 = (float)(-Math.Sin(angle2) * speed);
                //在最后一个球的位置添加两个新球，拥有不同的速度
                //在游戏中画出一个球变成三个球（效果会看起来很华丽，哇哈哈）
                ballList.Add(new Ball(ball.Brush, ball.Radius, ball.X, ball.Y, dX1, dY1));
                ballList.Add(new Ball(ball.Brush, ball.Radius, ball.X, ball.Y, dX2, dY2));
            }
        }

        /// <summary>
        /// 屏幕上画出边界
        /// </summary>
        /// <param name="paper">一个Graphics object，需要用其画出背景</param>
        private void DrawBackground(Graphics paper)
        {
            paper.FillRectangle(Brushes.Black,0,0,borderWidth,this.Height); //左边界
            paper.FillRectangle(Brushes.Black,0,0,this.Width, borderWidth); //上边界
            paper.FillRectangle(Brushes.Black,this.Width-borderWidth,0,borderWidth,this.Height); //右边界
            Font statsFont = new Font("微软雅黑", 8);
            paper.DrawString(
                "Level: " + level + 
                " Score: " + score,
                statsFont, Brushes.White, borderWidth, 1);
            //在左上角画出分数和关卡
            paper.DrawString("Lives: " + lives, statsFont, Brushes.White, this.Width - 45 - borderWidth, 1);
            //在右上角画出生命
        }

        /// <summary>
        /// 如果屏幕上一个球都没有的时候调用
        /// </summary>
        private void LostAllBalls()
        {
            lives -= 1;             //生命减一
            timer1.Enabled = false; //暂停游戏
            InitializeBall(level);       //画出一个新球
            powerUpList.Clear();    //清除道具
            if (lives < 0) GameOver();//生命小于0，游戏结束...（这个我解释的太多余了...）
        }

        /// <summary>
        /// 通知玩家分数之后重新开始游戏
        /// </summary>
        private void GameOver()
        {
            MessageBox.Show("唉...不行啊你，这么快就挂了，才 " + score + " 分，太少了啊");
            lives = 3;
            score = 0;
            InitializeGame();
        }

        /// <summary>
        /// 每通过一关，重置游戏并加一条生命
        /// </summary>
        private void NextLevel()
        {
            timer1.Enabled = false;
            lives++; 
            level++; 
            MessageBox.Show("牛B了！下一关"+level +"继续努力吧！奖励你一条命！");
            InitializeGame();
        }
        //键盘左右控制移动，空格是开始游戏
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
            if (e.KeyCode.ToString().Equals("Space"))//按空格开始
            {
                timer1.Enabled = true;
                
            }
            if (e.KeyCode.ToString().Equals("A"))//按A暂停或者恢复
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