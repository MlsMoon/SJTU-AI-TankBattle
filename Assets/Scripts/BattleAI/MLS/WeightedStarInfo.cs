using System.Collections.Generic;
using Main;
using UnityEngine;

namespace MLS
{
    /// <summary>
    /// 会根据星星的各个属性，来判断吃这颗星星的优先度，因为游戏规则下获得分数主要来自吃星
    /// （超级星星的优先级更高）
    /// 0.距自己的距离
    /// 1.离敌人的距离
    /// 2.附近是否有连续星星（如果吃到一颗，其他几颗星星也是可以轻易获得的）
    /// </summary>
    
    public class WeightedStarInfo
    {
        private List<float> _infos;
        public Star star;


        
        public WeightedStarInfo(Star input)
        {
            _infos = new List<float>()
            {
                0,
            };
            star = input;
            UpdateInfo();
        }

        public void UpdateInfo()
        {
            Vector3 tankPos = Conditions.Instance.Self.Position;
            Vector3 starPos = star.Position;
            float val = 1 - Vector3.Distance(tankPos, starPos) / Conditions.MaxDis;
            _infos[0] = val;
        }

        public float GetTotalPriority()
        {
            return _infos[0];
        }
        
        
    }
}