using UnityEngine;

namespace Utility {
    public class Health : MonoBehaviour, IDamagable {
        [SerializeField] private int m_maxHealth = 100;
        [SerializeField] private DoubleIntSignalAsset m_healthChanged;
        [SerializeField] private MonoSignalAsset m_onDeath;
        [field: SerializeField] public DamagableTeam team { get; private set; }
        private int m_health;
        private bool m_isDead;

        public int health {
            get => m_health;
            set {
                m_health = Mathf.Clamp(value, 0, m_maxHealth);
                m_healthChanged?.Invoke(m_health, m_maxHealth);
                if (m_health <= 0 && !m_isDead) {
                    Die();
                }
            }
        }

        private void Awake() {
            m_health = m_maxHealth;
            m_isDead = false;
        }

        private void Start() {
            health = m_maxHealth;
        }

        public virtual void TakeDamage(int _damageAmount) {
            health -= _damageAmount;
        }

        public virtual void Heal(int _amount) {
            if (_amount <= 0) return;
            m_isDead = false;
            health += _amount;
        }

        private void Die() {
            m_isDead = true;
            m_onDeath?.Invoke();
        }
    }
}
