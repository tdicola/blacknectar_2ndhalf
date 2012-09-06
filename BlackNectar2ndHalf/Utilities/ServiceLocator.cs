using System;
using System.Collections.Generic;


namespace BlackNectar2ndHalf
{
    public static class ServiceLocator
    {
        static ServiceLocator()
        {
            services = new Dictionary<Type, object>();
        }

        private static Dictionary<Type, object> services;

        public static void Register<T>(object instance)
        {
            services[typeof(T)] = instance;
        }

        public static T Resolve<T>()
        {
            if (services.ContainsKey(typeof(T)))
            {
                return (T)services[typeof(T)];
            }
            else
            {
                return default(T);
            }
        }
    }
}
