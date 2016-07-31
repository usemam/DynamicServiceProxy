using System;
using System.Reflection;

using Castle.DynamicProxy;

namespace DynamicServiceProxy
{
    public class Proxy<TServiceInterface> : IInterceptor
    {
        #region Constructors

        public Proxy()
        {
            var generator = new ProxyGenerator();
            this.Interface =
                (TServiceInterface) generator.CreateInterfaceProxyWithoutTarget(typeof (TServiceInterface), this);
        }

        #endregion

        #region Properties & Indexers

        #region Public Properties

        public TServiceInterface Interface { get; private set; }

        #endregion

        #endregion

        #region Public methods

        void IInterceptor.Intercept(IInvocation invocation)
        {
            var proxyType = this.GetType();
            var argTypes = this.GetArgumentsTypes(invocation.Arguments);

            var method = proxyType.GetMethod(
                invocation.Method.Name, BindingFlags.Public | BindingFlags.Instance,
                null, argTypes, null);

            if (method != null)
            {
                invocation.ReturnValue = method.Invoke(this, invocation.Arguments);
                return;
            }

            // No match was found for required method
            throw new NotImplementedException(
                $"No match was found for method {invocation.Method.Name}.");
        }

        #endregion

        #region Methods

        private Type[] GetArgumentsTypes(object[] args)
        {
            var argTypes = new Type[args.GetLength(0)];

            var index = 0;
            foreach (var arg in args)
            {
                argTypes[index] = arg.GetType();
                index++;
            }

            return argTypes;
        }

        #endregion
    }
}