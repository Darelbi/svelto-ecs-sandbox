using Svelto.ECS;
using Svelto.ECS.Internal;

namespace GameUnitTest
{
    /// <summary>
    /// Invoke any method of a class passing the given parameters. It works only on classes that implements
    /// any of the Enginse base classes AND it cannot calls methods that are present in those classes
    /// or methods that implements something in those base classes. (basically it can call only methods
    /// of engines.) I tried to make this class as safe as possible.
    /// </summary>
    public static class EngineInvokerExtension
    {
        public static void InvokeMethod< T>( this T obj, string methodName, object[] parameters = null)
        {
            bool type1 = obj is IQueryingEntityViewEngine;
            bool type2 = obj is IHandleEntityViewEngine;

            if (type1 == false && type2 == false)
                return;

            var method = obj.GetType().GetMethod(   methodName, 
                                                System.Reflection.BindingFlags.NonPublic 
                                            |   System.Reflection.BindingFlags.Instance);

            // Check if the method comes from a base class
            bool methodComesFromBaseClass = false;

            if (method != null && !methodComesFromBaseClass )
            {
                method.Invoke( obj, parameters);
            }
        }
    }
}