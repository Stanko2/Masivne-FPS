public interface IDamageable
{
    public float Health { get; set; }
    bool ApplyDamage(float amount);
    void Dead();
}