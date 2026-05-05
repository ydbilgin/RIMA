namespace RIMA
{
    public class CastRhythmState : IBasicAttackState
    {
        private readonly int _comboLength;
        private readonly float _comboWindow;
        private int _castStep;
        private float _timer;

        public int CurrentStep => _castStep;
        public float StepWindow => _comboWindow;
        public bool WindowOpen => _timer > 0f || _castStep == 0;
        public bool IsEmpoweredBeat => _castStep == _comboLength - 1;

        public CastRhythmState(int comboLength, float comboWindow)
        {
            _comboLength = comboLength;
            _comboWindow = comboWindow;
            _castStep = 0;
            _timer = 0f;
        }

        public void Advance()
        {
            _castStep = (_castStep + 1) % _comboLength;
            _timer = _comboWindow;
        }

        public void Reset()
        {
            _castStep = 0;
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
