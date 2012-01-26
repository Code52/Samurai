namespace IdeaStrike.Tests
{
    public abstract class SpecificationFor<T>
    {
        public T Subject;

        public abstract T Given();
        public abstract void When();

        public virtual void SetUp()
        {
            
        }
        protected SpecificationFor()
        {
            SetUp();
            Subject = Given();
            When();
        }
    }
}

