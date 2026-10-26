namespace Utility {
    public enum DamagableTeam {
        Player,
        Opponent,
    }
    
    public interface IDamageable {
        DamagableTeam team { get; }

        float health { get; set; }
        void TakeDamage(float damageAmount);
    }
}