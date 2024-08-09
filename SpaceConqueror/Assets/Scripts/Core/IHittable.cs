namespace Core
{
    public interface IHittable
    {
        public void GetHit(float damage);
        public float GetHealth();
    }
}