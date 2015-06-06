using System.Diagnostics.Contracts;

namespace EnvCrypt.Core.Verb
{
    public abstract class GenericBuilder
    {
        [Pure]
        public bool IsBuilt { get; protected set; }

        protected void MarkAsNotBuilt()
        {
            IsBuilt = false;
            SetWorkflowToNull();
        }

        protected abstract void SetWorkflowToNull();

        [Pure]
        protected abstract bool IsWorkflowNull();

        protected void ThrowIfNotBuilt()
        {
            if (!IsBuilt)
            {
                throw new EnvCryptException("builder cannot be run because it has not been built");
            }
        }


        [ContractInvariantMethod]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
        private void ObjectInvariant()
        {
            Contract.Invariant(IsBuilt == (!IsWorkflowNull()));
        }
    }
}
