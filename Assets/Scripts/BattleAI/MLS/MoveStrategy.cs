namespace MLS
{
    /// <summary>
    /// 移动策略
    /// 默认移动策略：以得分为目标，优先向星星移动
    /// </summary>

    //移动策略的分类
    public enum MoveStrategyType
    {
        AvoidMoveStrategy,
        StarFirstStrategy
    }

    //移动控制器
    public class MoveController
    {
        private MoveStrategy _currentMoveStrategy;

        public MoveController()
        {
            _currentMoveStrategy = new StarFirstStrategy();
        }

        public void OnUpdate()
        {
            //评估当前条件
            var targetState = _currentMoveStrategy.Evaluate();
            if (targetState != _currentMoveStrategy.type)
            {
                //需要更换移动策略
            }
            _currentMoveStrategy.OnUpdate();
        }
    }
    
    //========================================
    //以下是各类移动的策略
    
    //策略基类
    public class MoveStrategy
    {
        public MoveStrategyType type;

        public MoveStrategy()
        {
            Initial();
        }

        protected virtual void Initial() { }
        
        public MoveStrategyType Evaluate()
        {
            //所有策略的通用评估方法
            return type;
        }
        
        public virtual void OnUpdate()
        {
            
        }
    }
    
    public class StarTarget:MoveStrategy
    {
        public override void OnUpdate()
        {
            base.OnUpdate();
            //将权重最高的星星作为目标
            if (Conditions.Instance.starInfos.Count != 0)
            {
                var target = Conditions.Instance.GetBestStar();
                Conditions.Instance.Self.Move(target.Position);
            }
        }
    }

    public class AvoidMoveStrategy : StarTarget
    {
        protected override void Initial()
        {
            type = MoveStrategyType.AvoidMoveStrategy;
        }


    }

    public class StarFirstStrategy : StarTarget
    {
        protected override void Initial()
        {
            type = MoveStrategyType.StarFirstStrategy;
        }
    }
}