using System.Collections.Generic;
using Main;
using UnityEngine;

namespace MLS
{
    /// <summary>
    /// 条件信息，只用算一次的信息
    /// </summary>
    class Conditions
    {
        //单例
        #region Singleton
        public static Conditions Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Conditions();
                }
                return _instance;
            }
        }
        private static Conditions _instance;
        #endregion
        
        public const float MaxDis = 70.71f;
        public Tank Self;
        
        public Dictionary<int, WeightedStarInfo> starInfos;
        private List<int> _starDelete;
        public Vector3 currentDest;

        
        
        public Conditions()
        {
            _starDelete = new List<int>();
            starInfos = new Dictionary<int, WeightedStarInfo>();
        }

        /// <summary>
        /// 更新条件信息
        /// </summary>
        public void UpdateConditions()
        {
            //更新场上的星星的信息
            var stars = Match.instance.GetStars();
            var keys = starInfos.Keys;
            //删除
            _starDelete.Clear();
            foreach (var key in keys)
            {
                Star t;
                bool isExist = stars.TryGetValue(key, out t);
                if (!isExist)
                {
                    _starDelete.Add(key);
                }
                else
                {
                    //更新权重
                    starInfos[key].UpdateInfo();
                }
            }
            for (int i = 0; i < _starDelete.Count; i++)
            {
                starInfos.Remove(_starDelete[i]);
            }
            //增加
            var keysInMatch = stars.Keys;
            foreach (var key in keysInMatch)
            {
                WeightedStarInfo t;
                bool isExist = starInfos.TryGetValue(key, out t);
                if (!isExist)
                {
                    Star temp = stars[key];
                    starInfos.Add( key , new WeightedStarInfo(temp) );
                }
            }
            Debug.Log("场上星星数："+ starInfos.Count);
        }

        public Star GetBestStar()
        {
            if (starInfos.Count == 0)
                return null;
            var keys = starInfos.Keys;
            float highest = float.MinValue;
            int target = 0;
            foreach (var intKey in keys)
            {
                var value = starInfos[intKey].GetTotalPriority();
                if (value > highest)
                {
                    target = intKey;
                    highest = value;
                }
            }
            return starInfos[target].star;
        }
    }

    public class MyTank:Tank
    {
        #region  Properties

        private MoveController _moveController;
        private Vector3 _currentTargetPos;

        #endregion
        
        /// <summary>
        /// 初始化
        /// </summary>
        public MyTank()
        {
            _moveController = new MoveController();
            //传入相关参数
            Tank oppTank = Match.instance.GetOppositeTank(Team);
            Conditions.Instance.Self = this;
        }
        
        public override string GetName()
        {
            return "MLS";
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            Conditions.Instance.UpdateConditions();
            _moveController.OnUpdate();
            Fire();
        }
        
        protected override void OnReborn()
        {
            base.OnReborn();
            Time.timeScale = 1.0f;
        }

        protected override void OnOnDrawGizmos()
        {
            base.OnOnDrawGizmos();
            
        }
    }
}