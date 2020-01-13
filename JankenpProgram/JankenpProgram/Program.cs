using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JankenProgram
{
    /// <summary>
    /// ジャンケンプログラムのメインクラス
    /// </summary>
    class Program
    {
        /// <summary>
        /// ジャンケンプログラムのメイン処理
        /// </summary>
        static void Main(string[] args)
        {
            Cpu player1 = new Cpu();
            Man player2 = new Man();

            // 勝利判定処理にプレイヤーの情報を渡す
            Judge judge = new Judge(player1, player2);

            // ジャンケンが5回行われるまで繰り返す
            for(int i = 1; i <= 5; i++)
            {
                judge.Game(i);
            }

            // 最終結果処理を行う
            judge.Winner();

            Console.Read();
        }
    }

    /// <summary>
    /// CPUとプレイヤーの点数カウント処理を行う
    /// </summary>
    abstract class Player
    {
        public int wincount;
        public String name;

        public String Name
        {
            get { return name; }
        }

        public Player()
        {
            this.wincount = 0;
        }

        public abstract int ShowHand();

        public void Count()
        {
            wincount++;
        }

        public int WinCount
        {
            get { return wincount; }
        }
    }

    /// <summary>
    /// CPUのジャンケンの手、CPUの情報を設定する
    /// </summary>
    class Cpu : Player
    {
        /// <summary>
        /// CPUのプレイヤー名を設定する
        /// </summary>
        public Cpu()
        {
            this.name = "CPU";
        }

        /// <summary>
        /// ジャンケンの手をランダムに設定する
        /// </summary>
        /// <returns>0〜2の数字をランダムに返す</returns>
        public override int ShowHand()
        {
            Random rnd = new Random();
            return rnd.Next(0, 3);
        }
    }

    /// <summary>
    /// プレイヤーのジャンケンの手、プレイヤーの情報を設定する
    /// </summary>
    class Man : Player
    {
        /// <summary>
        /// プレイヤー名を設定(取得)する
        /// </summary>
        public Man()
        {
            Console.Write("あなたの名前を入力してください：");
            // 入力を読み取る
            this.name = Console.ReadLine();

            //　入力がない場合（入力せずにエンターキーを押した場合）
            if(name == "")
            {
                name = "名無し";
            }
        }

        /// <summary>
        /// プレイヤーが入力したジャンケンの手を取得する
        /// </summary>
        /// <returns>プレイヤーが入力したジャンケンの手を返す</returns>
        public override int ShowHand()
        {
            int n;

            // 0〜2の数字が入力されるまで以下の処理を繰り返す
            do
            {
                Console.Write(this.name + "の手を入力してください（0：グー, 1：チョキ, 2：パー）：");
                try
                {
                    n = int.Parse(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("「0 〜 2」の数字を入力してください");
                    n = -1;
                }
            }
            while (n != 0 && n != 1 && n != 2);

            return n;
        }
    }

    /// <summary>
    /// ジャンケンの勝利判定を行う
    /// </summary>
    class Judge
    {
        public Player player1;
        public Player player2;

        /// <summary>
        /// ジャンケンの試合開始宣言を行う
        /// </summary>
        /// <param name="p1">CPUの情報</param>
        /// <param name="p2">プレイヤーの情報</param>
        public Judge(Player p1, Player p2)
        {
            this.player1 = p1;
            this.player2 = p2;

            Console.WriteLine(player1.Name + " 対 " + player2.Name + " : ジャンケン開始\n");
        }

        public int hand1, hand2;

        /// <summary>
        /// ジャンケンの試合を行う
        /// </summary>
        /// <param name="n">試合回数</param>
        public void Game(int n)
        {
            Console.WriteLine("*** {0}回戦 ***", n);
            hand1 = player1.ShowHand();
            hand2 = player2.ShowHand();
            Judgement(hand1, hand2);
        }

        /// <summary>
        /// ジャンケンの勝利判定を行う
        /// </summary>
        /// <param name="h1">CPUの手</param>
        /// <param name="h2">プレイヤーの手</param>
        private void Judgement(int h1, int h2)
        {
            Player winner = player1;
            Console.Write(Hand(h1) + " 対 " + Hand(h2) + "で　");

            // CPUと手が同じであった場合
            if(h1 == h2)
            {
                Console.WriteLine("引き分けです");
                return;
            }
            /*
             * 3 = ジャンケンの手の数
             * ("グー":0, "チョキ":1, "パー":2)
             * h1 = CPUの手
             * h2 = プレイヤーの手
             * % = 割った余りの数
             * 
             * (3 + "グー":0 - "グー":0) % 3 = 3 % 3 = 0
             * (3 + "グー":0 - "チョキ":1) % 3 = 2 % 3 = 2
             * (3 + "グー":0 - "パー":2) % 3 = 1 % 3 = 【1】
             * 
             * (3 + "チョキ":1 - "グー":0) % 3 = 4 % 3 =【1】
             * (3 + "チョキ":1 - "チョキ":1) % 3 = 3 % 3 = 0
             * (3 + "チョキ":1 - "パー":2) % 3 = 2 % 3 = 2
             * 
             * (3 + "パー":2 - "グー":0) % 3 = 5 % 3 = 2
             * (3 + "パー":2 - "チョキ":1) % 3 = 4 % 3 =【1】
             * (3 + "パー":2 - "パー":2) % 3 = 3 % 3 = 0
             */
            else if ((3 + h1 - h2) % 3 == 1)
            {
                winner = player2;
            }

            Console.WriteLine(winner.Name + "の勝ちです");
            winner.Count();
        }

        /// <summary>
        /// 0〜2の数字にジャンケンの手を当てはめる
        /// </summary>
        /// <param name="h">ジャンケンの手</param>
        private string Hand(int h)
        {
            string[] hs = { "グー", "チョキ", "パー" };
            return hs[h];
        }

        /// <summary>
        /// 行われた試合の最終判定行う
        /// </summary>
        public void Winner()
        {
            int p1, p2;
            p1 = player1.WinCount;
            p2 = player2.WinCount;

            Player finalwinner = player1;
            Console.Write("\n*** 最終結果 ***\n{0} 対 {1} で", p1, p2);

            // 勝点がCPUと同じの場合
            if(p1 == p2)
            {
                Console.WriteLine("引き分けです");
                return;
            }
            // 勝点がCPUより勝っていた場合
            else if(p1 < p2)
            {
                finalwinner = player2;
            }

            Console.WriteLine(finalwinner.Name + "の勝ちです");
        }
    }
}
