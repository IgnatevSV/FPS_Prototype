namespace FPSProject
{
    public struct DestroyMeta
    {
        public IDestroyableObject DestroyedObject { get; }
        public IBulletObject DestroyBullet { get; }

        public DestroyMeta(IDestroyableObject destroyedObject, IBulletObject destroyBullet)
        {
            DestroyedObject = destroyedObject;
            DestroyBullet = destroyBullet;
        }
    }
}