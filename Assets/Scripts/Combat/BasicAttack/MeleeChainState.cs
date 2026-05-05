namespace RIMA
{
    public class MeleeChainState : IBasicAttackState
    {
        private readonly int _comboLength;
        private readonly float _comboWindow;
        private int _step;
        private float _timer;

        public int CurrentStep => _step;
        public float StepWindow => _comboWindow;
        public bool WindowOpen => _timer > 0f || _step == 0;

        public MeleeChainState(int comboLength, float comboWindow)
        {
            _comboLength = comboLength;
            _comboWindow = comboWindow;
            _step = 0;
            _timer = 0f;
        }

        public void Advance()
        {
            _step = (_step + 1) % _comboLength;
            _timer = _comboWindow;
        }

        public void Reset()
        {
            _step = 0;
            _timer = 0f;
        }

        public void Tick(float deltaTime)
        {
            if (_timer > 0f)
            {
                _timer -= deltaTime;
                if (_timer <= 0f)
                    Reset();
            }
        }
    }
}
