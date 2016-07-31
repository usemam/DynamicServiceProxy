using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using DynamicServiceProxy.Contracts;

using NUnit.Framework;

namespace DynamicServiceProxy.Client.Tests
{
    [TestFixture]
    public class InterfaceConformityTests
    {
        [Test]
        public void Proxy_has_methods_with_correct_names()
        {
            var contractMethodNames =
                GetTypeMethods<IUserService>().Select(x => x.Name).ToArray();
            var proxyMethodNames =
                GetTypeMethods<ClientUserService>().Select(x => x.Name);
            var notContractProxyMethods =
                proxyMethodNames.Where(x => !contractMethodNames.Contains(x)).ToArray();
            Assert.IsTrue(
                notContractProxyMethods.Length == 0,
                $"Following methods do not belong to an interface: {string.Join(",", notContractProxyMethods)}");
        }

        [Test]
        public void Proxy_methods_have_correct_parameters()
        {
            var contractMethods = GetTypeMethods<IUserService>();
            var proxyMethods = GetTypeMethods<ClientUserService>();
            var proxyMethodToContractMethodsMap =
                proxyMethods.ToDictionary(
                    x => x,
                    x => contractMethods.Where(m => x.Name == m.Name).ToList());
            var proxyMethodsWithNotMatchingParams =
                proxyMethodToContractMethodsMap
                    .Where(p => p.Value.All(x => !MethodsMatchByParameters(p.Key, x)))
                    .Select(p => p.Key.Name)
                    .ToArray();
            Assert.IsTrue(
                proxyMethodsWithNotMatchingParams.Length == 0,
                $"Following methods have incorrect list of parameters: {string.Join(",", proxyMethodsWithNotMatchingParams)}");
        }

        [Test]
        public void Proxy_methods_have_correct_return_types()
        {
            var contractMethods = GetTypeMethods<IUserService>();
            var proxyMethods = GetTypeMethods<ClientUserService>();
            var proxyMethodToContractMethodsMap =
                proxyMethods.ToDictionary(
                    x => x,
                    x => contractMethods.Where(m => x.Name == m.Name).ToList());
            var proxyMethodsWithNotMatchingReturnTypes =
                proxyMethodToContractMethodsMap
                    .SelectMany(
                        p =>
                            p.Value.Where(x => MethodsMatchByParameters(p.Key, x))
                                .Select(x => new {First = p.Key, Second = x}))
                    .Where(x => x.First.ReturnType != x.Second.ReturnType)
                    .Select(x => x.First.Name)
                    .ToArray();
            Assert.IsTrue(
                proxyMethodsWithNotMatchingReturnTypes.Length == 0,
                $"Following methods have incorrect return types: {string.Join(",", proxyMethodsWithNotMatchingReturnTypes)}");
        }

        private static IEnumerable<MethodInfo> GetTypeMethods<T>()
        {
            return typeof(T).GetMethods(
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        }

        private static bool MethodsMatchByParameters(MethodInfo first, MethodInfo second)
        {
            var firstParams = first.GetParameters().OrderBy(x => x.Position).ToArray();
            var secondParams = second.GetParameters().OrderBy(x => x.Position).ToArray();
            if (firstParams.Length != secondParams.Length)
            {
                return false;
            }

            var zippedParams =
                firstParams.Zip(secondParams, (f, s) => new {First = f, Second = s});
            return zippedParams.All(x => x.First.ParameterType == x.Second.ParameterType);
        }
    }
}