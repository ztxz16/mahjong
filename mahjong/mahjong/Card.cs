using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mahjong
{
    /// <summary>
    /// 描述一个花色的牌
    /// </summary>
    class Color
    {
        /// <summary>
        /// 允许的雀头，长沙麻将中为2, 5, 8
        /// </summary>
        public static int[] ChangshaValidPairs = new int[3] { 2, 5, 8 };

        /// <summary>
        /// 允许所有雀头
        /// </summary>
        public static int[] NormalValidPairs = new int[9] { 1, 2, 3, 4, 5, 6, 7, 8, 9};

        /// <summary>
        /// 当前程序中使用的规则
        /// </summary>
        public static int[] ValidPairs = ChangshaValidPairs;

        /// <summary>
        /// 牌的有序字典
        /// </summary>
        public SortedDictionary<int, int> cards = new SortedDictionary<int, int>();

        /// <summary>
        /// 总张数
        /// </summary>
        public int cnt = 0;

        /// <summary>
        /// 插入一张牌
        /// </summary>
        /// <param name="point"> 牌的点数 </param>
        /// <returns> 是否成功删除 </returns>
        public bool Insert(int point)
        {
            if (!cards.ContainsKey(point))
            {
                cards[point] = 0;
            }

            if (cards[point] < 4)
            {
                cards[point] = cards[point] + 1;
                cnt++;
                return true;
            } else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除一张牌
        /// </summary>
        /// <param name="point"> 牌的点数 </param>
        /// <returns> 是否成功删除 </returns>
        public bool Delete(int point)
        {
            if (!cards.ContainsKey(point))
            {
                return false;
            }

            if (cards[point] > 0)
            {
                cards[point]--;
                if (cards[point] == 0)
                {
                    cards.Remove(point);
                }

                cnt--;
                return true;
            } else
            {
                cards.Remove(point);
                return false;
            }
        }

        /// <summary>
        /// 清空所有牌
        /// </summary>
        public void Clear()
        {
            cards.Clear();
            cnt = 0;
        }

        /// <summary>
        /// 判断是否可以拆成1雀头+若干面子
        /// </summary>
        /// <returns></returns>
        public bool HuWithPair()
        {
            if (cnt % 3 != 2)
            {
                return false;
            }

            foreach (int pair in ValidPairs)
            {
                if (cards.ContainsKey(pair) && cards[pair] >= 2)
                {
                    Delete(pair);
                    Delete(pair);

                    bool hu = HuWithoutPair();

                    Insert(pair);
                    Insert(pair);

                    if (hu)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 判断是否可以拆成若干面子
        /// </summary>
        /// <returns></returns>
        public bool HuWithoutPair()
        {
            if (cnt % 3 != 0)
            {
                return false;
            }

            SortedDictionary<int, int> temp = new SortedDictionary<int, int>();
            foreach (var key in cards.Keys)
            {
                temp[key] = cards[key];
            }
            for (int i = 0; i < cnt / 3; i++)
            {
                int point = temp.First().Key;
                int cnt = temp.First().Value;
                if (cnt >= 3)
                {
                    temp[point] -= 3;
                    if (temp[point] == 0)
                    {
                        temp.Remove(point);
                    }
                } else
                {
                    if (point < 8 && temp.ContainsKey(point + 1) && temp.ContainsKey(point + 2) && temp[point + 1] > 0 && temp[point + 2] > 0)
                    {
                        for (int j = point; j <= point + 2; j++) {
                            temp[j]--;
                            if (temp[j] == 0)
                            {
                                temp.Remove(j);
                            }
                        }
                    } else
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }

    /// <summary>
    /// 听了哪些牌
    /// </summary>
    class Ting
    {
        /// <summary>
        /// 条，筒，万分别听了哪些牌
        /// </summary>
        public List<int>[] points = new List<int>[3] { new List<int>(), new List<int>(), new List<int>() };

        /// <summary>
        /// 重置
        /// </summary>
        public void Clear()
        {
            foreach (var point in points)
            {
                point.Clear();
            }
        }

        /// <summary>
        /// 计算听了几张牌
        /// </summary>
        /// <returns> 听了几张牌 </returns>
        public int Size()
        {
            return points[0].Count + points[1].Count + points[2].Count;
        }
    }

    /// <summary>
    /// 描述一个牌区的牌
    /// </summary>
    class CardCollection
    {
        /// <summary>
        /// 条，筒，万分别有哪些牌
        /// </summary>
        public Color[] colors = new Color[3] { new Color(), new Color(), new Color() };

        /// <summary>
        /// 清空所有牌
        /// </summary>
        public void Clear()
        {
            foreach (var color in colors)
            {
                color.Clear();
            }
        }
    }

    /// <summary>
    /// 描述自己的牌
    /// </summary>
    class MyCard
    {
        /// <summary>
        /// 手牌
        /// </summary>
        public CardCollection hand = new CardCollection();

        /// <summary>
        /// 副露，吃、碰、杠的牌
        /// </summary>
        public CardCollection fulu = new CardCollection();

        /// <summary>
        /// 清空所有牌
        /// </summary>
        public void Clear()
        {
            hand.Clear();
            fulu.Clear();
        }

        /// <summary>
        /// 计算是否胡牌
        /// </summary>
        /// <returns> 是否胡牌 </returns>
        public bool CheckHu()
        {
            return
                (hand.colors[0].HuWithPair() && hand.colors[1].HuWithoutPair() && hand.colors[2].HuWithoutPair()) ||
                (hand.colors[0].HuWithoutPair() && hand.colors[1].HuWithPair() && hand.colors[2].HuWithoutPair()) ||
                (hand.colors[0].HuWithoutPair() && hand.colors[1].HuWithoutPair() && hand.colors[2].HuWithPair());
        }

        /// <summary>
        /// 计算听牌情况
        /// </summary>
        /// <param name="ting"> 听了哪些牌 </param>
        public void CalcTing(Ting ting)
        {
            ting.Clear();

            for (int c = 0; c < 3; c++)
            {
                for (int i = 1; i <= 9; i++)
                {
                    if (hand.colors[c].Insert(i))
                    {
                        if (CheckHu())
                        {
                            ting.points[c].Add(i);
                        }

                        hand.colors[c].Delete(i);
                    }
                }
            }
        }

        /// <summary>
        /// 计算是否听牌
        /// </summary>
        /// <returns> 是否听牌 </returns>
        public bool IsTing()
        {
            for (int c = 0; c < 3; c++)
            {
                int[] cnt = new int[3]{ 0, 0, 0 };
                for (int fc = 0; fc < 3; fc++)
                {
                    cnt[(hand.colors[fc].cnt + (fc == c ? 1 : 0)) % 3]++;
                }

                if (cnt[0] != 2 || cnt[2] != 1)
                {
                    continue;
                }

                for (int i = 1; i <= 9; i++)
                {
                    if (hand.colors[c].Insert(i))
                    {
                        if (CheckHu())
                        {
                            hand.colors[c].Delete(i);
                            return true;
                        }

                        hand.colors[c].Delete(i);
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 计算舍牌之后的听牌情况
        /// </summary>
        /// <param name="forgive"> 舍了(花色，数字)之后，听了哪些牌</param>
        public void CalcForgive(Dictionary<KeyValuePair<int, int>, Ting> forgive)
        {
            forgive.Clear();
            for (int c = 0; c < 3; c++)
            {
                for (int i = 1; i <= 9; i++)
                {
                    if (hand.colors[c].Delete(i))
                    {
                        Ting ting = new Ting();
                        CalcTing(ting);
                        if (ting.Size() > 0)
                        {
                            forgive[new KeyValuePair<int, int>(c, i)] = ting;
                        }

                        hand.colors[c].Insert(i);
                    }
                }
            }
        }

        /// <summary>
        /// 计算当前牌型是几向听的
        /// </summary>
        /// <param name="cur"> 当前深度 </param>
        /// <param name="limit"> 递归最大允许深度 </param>
        /// <param name="ans"> 当前最优听数 </param>
        public void GetToTing(int cur, int limit, ref int ans)
        {
            if (cur >= limit || cur >= ans)
            {
                return;
            }

            if (IsTing())
            {
                ans = cur;
                return;
            }

            bool firstJiang = true;
            for (int c = 0; c < 3; c++)
            {
                for (int i = 1; i <= 9; i++)
                {
                    bool dothis = hand.colors[c].cards.ContainsKey(i - 1) || hand.colors[c].cards.ContainsKey(i) || hand.colors[c].cards.ContainsKey(i + 1);
                    if (!dothis)
                    {
                        if (firstJiang && Color.ValidPairs.Contains(i))
                        {
                            dothis = true;
                            firstJiang = false;
                        }
                    }
                    if (dothis)
                    {
                        if (hand.colors[c].Insert(i))
                        {
                            for (int fc = 0; fc < 3; fc++)
                            {
                                var keys = hand.colors[fc].cards.Keys.ToList();
                                foreach (int fi in keys)
                                {
                                    if (fc != c || i != fi)
                                    {
                                        hand.colors[fc].Delete(fi);
                                        GetToTing(cur + 1, limit, ref ans);
                                        hand.colors[fc].Insert(fi);
                                    }
                                }
                            }

                            hand.colors[c].Delete(i);
                        }
                    }
                }
            }
        }
    }
}
